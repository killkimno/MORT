using MORT.Model.OCR;
using MORT.Service.PythonService;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MORT.OcrApi.EasyOcr
{
    public class EasyOcr
    {
        private readonly PythonModouleService _modouleService;
        private dynamic _easyocr;
        private dynamic _builtinsLib;
        private dynamic _bytes;
        private dynamic _reader;

        private object _lockObject = new object();
        private bool _inited;

        public List<string> CodeList = new List<string>() { "en", "ja" };
        private string _code;

        public EasyOcr(PythonModouleService modouleService)
        {
            _modouleService = modouleService;
        }

        // 모듈 체크 방법
        // 1. 설치가 되어 있나 -> IsModuleInstalled("easyocr") / IsPipInstalled
        // -> 없으면 무조건 설치해야 한다
        // 1.2 TryInstallAsync(bool enableGPU, bool force, string gpuCommand)
        // 2. 초기화가 되어 있나?
        // -> 안 되어 있으면 py.import를 한다
        // 3. 적용으로 온 건가? 1,2 모두 통과했으면 _reader만 다시 한다

        public bool IsInstalled()
        {
            return _modouleService.IsInstalled("easyocr");
        }

        public void UnloadMoudle()
        {
            if (_inited)
            {
                _inited = false;

                _easyocr?.Dispose();
                _builtinsLib?.Dispose();
                _bytes?.Dispose();
                _reader?.Dispose();

                _easyocr = null;
                _builtinsLib = null;
                _bytes = null;
                _reader = null;
            }
        }

        public async Task TryInstallAsync(bool enableGPU, string gpuCommand = "")
        {
            if(_inited)
            {
                return;
            }

            if(enableGPU)
            {
                //예문
                //await _modouleService.InstallModouleAsync("torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu121");
                await _modouleService.InstallModouleAsync(gpuCommand);
            }
           
            await _modouleService.InstallModouleAsync("easyocr");
        }

        public async Task TryInitAsync(string code)
        {
            if(_inited && _code == code)
            {
                return;
            }

            lock (_lockObject)
            {
                using (Py.GIL())
                {
                    if(!_inited)
                    {
                        _easyocr = Py.Import("easyocr");
                        _builtinsLib = Py.Import("builtins");
                        _bytes = _builtinsLib.GetAttr("bytes");
                    }
                 
                    if(_code != code)
                    {
                        _reader?.Dispose();
                        _reader = _easyocr.Reader(new[] { code }, gpu: true);
                    }                 
                }
            }

            _inited = true;
        }

        public EasyOcrResultModel ProcessOcr(byte[] byteData, int width, int height)
        {
            EasyOcrResultModel easyOcrResultModel = EasyOcrResultModel.Empty;

            int targetChannels = 3;
            string result = "";

            lock (_lockObject)
            {
                using (Py.GIL())
                {
                    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    BitmapData bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    IntPtr pNative = bmData.Scan0;
                    byte[] newArr = new byte[width * height * targetChannels];

                    //TODO : WIN OCR걸 이용하자
                    var currPixel = 0;
                    for (var y = 0; y < height; y++)
                    {
                        for (var x = 0; x < width; x++)
                        {
                            newArr[currPixel] = byteData[y * width + x];
                            newArr[currPixel + 1] = byteData[y * width + x];
                            newArr[currPixel + 2] = byteData[y * width + x];
                            currPixel += targetChannels;
                        }
                    }



                    Marshal.Copy(newArr, 0, pNative, width * height * targetChannels);
                    bitmap.UnlockBits(bmData);

                    byte[] resultBytes;
                    using (var ms = new MemoryStream())
                    {

                        bitmap.Save(ms, ImageFormat.Tiff);
                        resultBytes = ms.ToArray();
                    }

                    bitmap.Save(Path.GetFullPath(".") + "\\test.bmp", ImageFormat.Bmp);
                    Console.WriteLine("Ready");

                    dynamic ocrResult = _reader.readtext(_bytes.Invoke(resultBytes.ToPython()), detail: 1, paragraph: false);
                    //var pyList = reader.readtext("https://scalar.usc.edu/works/chronicles/media/frog-j.png");

                    List<List<(int x, int y)>> points = new List<List<(int x, int y)>>();
                    List<string> words = new List<string>();
                    foreach (var py in ocrResult)
                    {
                        PyObject pyObj = py;
                        var item1 = pyObj.GetItem(0);

                        long length = item1.Length();

                        List<(int x, int y)> list = new List<(int x, int y)>();
                        for(int i = 0; i < length; i++)
                        {
                            var pos = item1[i];
                            int x = (int)Convert.ToDouble(pos.GetItem(0).ToString());
                            int y = (int)Convert.ToDouble(pos.GetItem(1).ToString());
                            list.Add(new(x, y));
                        }

                        points.Add(list);

                        words.Add(pyObj.GetItem(1).ToString());

                        result += py;
                        Console.WriteLine("뭔가 " + py);
                    }

                    Console.WriteLine("된듯? " + result);

                    Array.Clear(newArr, 0, newArr.Length);

                    int identificador = GC.GetGeneration(newArr);
                    GC.Collect(identificador, GCCollectionMode.Forced);

                    easyOcrResultModel = new EasyOcrResultModel(points, words);
                }
            }


            return easyOcrResultModel;
        }
    }
}
