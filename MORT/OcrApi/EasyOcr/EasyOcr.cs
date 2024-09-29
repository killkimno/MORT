using Google.Rpc;
using MORT.Model.OCR;
using MORT.Service.PythonService;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
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
        private bool _moudleInited;

        public List<string> CodeList = new List<string>() { "en", "ja", "ko", "ch_sim", "ch_tra", "id"};
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
            return InintModoule();
        }

        public void UnloadMoudle()
        {
            _inited = false;
            _moudleInited = false;
            _easyocr?.Dispose();
            _builtinsLib?.Dispose();
            _bytes?.Dispose();
            _reader?.Dispose();

            _easyocr = null;
            _builtinsLib = null;
            _bytes = null;
            _reader = null;
        }

        public async Task TryInstallAsync(bool enableGPU, string gpuCommand = "")
        {
            if(_inited)
            {
                return;
            }

            //easy ocr 1.7.1 버전 기준 라이브러리다
            //신규 버전이 나온다면 또 봐야한다
            if(!_moudleInited)
            {
                //1.7.2 되면서 또 문제가 해결되었다 - 난중에 문제 생기면 다시 만든다
                /*
                await _modouleService.InstallModouleAsync("opencv-python-headless==4.9.0.80");
                await _modouleService.InstallModouleAsync("scipy==1.10.1");
                await _modouleService.InstallModouleAsync("numpy==1.24.4");
                await _modouleService.InstallModouleAsync("Pillow==10.2.0");
                await _modouleService.InstallModouleAsync("scikit-image==0.21.0");
                await _modouleService.InstallModouleAsync("python-bidi==0.4.2");
                await _modouleService.InstallModouleAsync("PyYAML==6.0.1");
                await _modouleService.InstallModouleAsync("Shapely==2.0.2");
                await _modouleService.InstallModouleAsync("pyclipper==1.3.0");
                await _modouleService.InstallModouleAsync("ninja==1.11.1.1");
                */
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

            InintModoule();

            lock (_lockObject)
            {
                using (Py.GIL())
                {                 
                    if(_code != code)
                    {
                        _reader?.Dispose();
                        _reader = _easyocr.Reader(new[] { code }, gpu: true);
                    }                 
                }
            }

            _inited = true;
        }

        private bool InintModoule()
        {
            try
            {
                if (!_moudleInited)
                {
                    lock (_lockObject)
                    {
                        using (Py.GIL())
                        {
                            _easyocr = Py.Import("easyocr");
                            _builtinsLib = Py.Import("builtins");
                            _bytes = _builtinsLib.GetAttr("bytes");
                            _moudleInited = true;

                            Util.ShowLog("SDDFDFDFDSF");
                        }
                    }
                }
            }
            catch
            {
                UnloadMoudle();
                return false;
            }

            return true;            
        }

        public EasyOcrResultModel ProcessOcr(byte[] byteData, int channel, int width, int height)
        {
            EasyOcrResultModel easyOcrResultModel = EasyOcrResultModel.Empty;

            int targetChannels = 3;

            lock (_lockObject)
            {
                using (Py.GIL())
                {
                    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    BitmapData bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    IntPtr pNative = bmData.Scan0;
                    byte[] newArr = new byte[width * height * targetChannels];

                    //TODO : stride를 이용해야 함
                    var currPixel = 0;
                    if(channel == 1)
                    {
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
                    }
                    else if (channel == 3)
                    {
                        //rgb
                        int count = 0;
                        for (uint row = 0; row < height; row++)
                        {
                            for (uint col = 0; col < width; col++)
                            {
                                // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)

                                newArr[currPixel + 0] = byteData[count++];
                                newArr[currPixel + 1] = byteData[count++];
                                newArr[currPixel + 2] = byteData[count++];

                                currPixel += targetChannels;
                            }
                        }
                    }
                    else if (channel == 4)
                    {  //rgba
                        int count = 0;
                        for (uint row = 0; row < height; row++)
                        {
                            for (uint col = 0; col < width; col++)
                            {
                                // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)

                                newArr[currPixel + 0] = byteData[count++];
                                newArr[currPixel + 1] = byteData[count++];
                                newArr[currPixel + 2] = byteData[count++];
                                count++;
                                //복사본 배열은 3채널로 처리한다
                                currPixel += targetChannels;
                            }
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

                    //bitmap.Save(Path.GetFullPath(".") + "\\test.bmp", ImageFormat.Bmp);

                    try
                    {
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
                            for (int i = 0; i < length; i++)
                            {
                                var pos = item1[i];
                                int x = (int)Convert.ToDouble(pos.GetItem(0).ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                int y = (int)Convert.ToDouble(pos.GetItem(1).ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                list.Add(new(x, y));
                            }

                            points.Add(list);

                            words.Add(pyObj.GetItem(1).ToString());
                        }

                        easyOcrResultModel = new EasyOcrResultModel(points, words);
                    }
                    catch (Exception ex)
                    {
                        Util.ShowLog(ex.Message);
                    }
                 

                    Array.Clear(newArr, 0, newArr.Length);

                    int identificador = GC.GetGeneration(newArr);
                    GC.Collect(identificador, GCCollectionMode.Forced);

                  
                }
            }


            return easyOcrResultModel;
        }
    }
}
