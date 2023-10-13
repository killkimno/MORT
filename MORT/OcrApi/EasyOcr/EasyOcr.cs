using MORT.OcrApi.WindowOcr;
using MORT.Service.PythonService;
using Python.Runtime;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using WinRT;

namespace MORT.OcrApi.EasyOcr
{
    public class EasyOcr
    {
        private readonly PythonModouleService _modouleService;
        private dynamic _easyocr;
        private dynamic _builtinsLib;
        private dynamic _bytes;
        private dynamic _reader;

        private bool _inited;
        public EasyOcr(PythonModouleService modouleService)
        {
            _modouleService = modouleService;
        }

        public async Task TryInitAsync()
        {
            if(_inited)
            {
                return;
            }
            //TODO : 세분화 해야한다
            await _modouleService.InstallModouleAsync("easyocr");

            using(Py.GIL())
            {
                _easyocr = Py.Import("easyocr");
                _builtinsLib = Py.Import("builtins");
                _bytes = _builtinsLib.GetAttr("bytes");
                _reader = _easyocr.Reader(new[] { "en" }, gpu: true);
            }


            _inited = true;
        }

        public string ProcessOcr(byte[] byteData, int width, int height)
        {
            int targetChannels = 3;
            string result = "";

            System.Object lockThis = new System.Object();

            lock (lockThis)
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

                    dynamic ocrResult = _reader.readtext(_bytes.Invoke(resultBytes.ToPython()), detail: 0, paragraph: true);
                    //var pyList = reader.readtext("https://scalar.usc.edu/works/chronicles/media/frog-j.png");

                    foreach (var py in ocrResult)
                    {
                        result += py;
                        Console.WriteLine("뭔가 " + py);
                    }

                    Console.WriteLine("된듯? " + result);

                    Array.Clear(newArr, 0, newArr.Length);

                    int identificador = GC.GetGeneration(newArr);
                    GC.Collect(identificador, GCCollectionMode.Forced);
                }
            }
            return result;
        }
    }
}
