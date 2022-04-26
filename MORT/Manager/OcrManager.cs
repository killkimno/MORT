using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MORT.Form1;

namespace MORT.Manager
{
    internal class OcrManager
    {

        public static string GoogleJsonPath
        {
            get
            {
                if (_instance == null)
                {
                    return "";
                }

                return Instace._googleJsonPath;
            }
        }

        private CloudVision.Api _googleOcr = new CloudVision.Api();
        private string _googleJsonPath;


        private static OcrManager _instance;
        public static OcrManager Instace
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OcrManager();
                }
                return _instance;
            }
        }

        public void Init()
        {
            LoadGoogleOcrJson();
        }

        public bool CheckGoogleOcrPriorty
        {
            get
            {
                return AdvencedOptionManager.UseGoogleOCRPriority && _googleOcr.Available;
            }
        }

        public void SetGoogleOcrJsonPath(string path)
        {
            Util.SaveFile(GlobalDefine.GOOGLE_OCR_PATH_FILE, path);
            LoadGoogleOcrJson();
        }

        public void LoadGoogleOcrJson()
        {
            var reader = Util.OpenFile(GlobalDefine.GOOGLE_OCR_PATH_FILE);

            if (reader != null)
            {
                string data = reader.ReadToEnd();
                _googleJsonPath = data;
                reader.Close();
                _googleOcr.InitClient(data);
            }
        }

        public async Task<string> ProcessGoogleAsync(ImgData imgData)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
               var bmp = BitmapFromBytes(imgData.data, imgData.x, imgData.y,imgData.channels,  PixelFormat.Format32bppArgb);
                bmp.Save(stream, ImageFormat.Png);
                bmp.Save(@"F:\Project\visualStudio Projects\MORT\MORT\bin\Release\out.bmp");


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
            string result = "empty;";

            result= _googleOcr.GetText(stream.ToArray());
            return result;
        }


        public static Bitmap BitmapFromBytes(byte[] bytes, int width, int height, int channels, PixelFormat bmpFormat)
        {
            Bitmap bmp = new Bitmap(width, height, bmpFormat);
            unsafe
            {


                var rect = new Rectangle(0, 0, width, height);
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmpFormat);

                uint capacity;
                byte[] data = new byte[width * height * 4];
                int currPixel = 0;

                int count = 0;
                if (channels == 1)
                {  
                    for (uint row = 0; row < height; row++)
                    {
                        for (uint col = 0; col < width; col++)
                        {
                            // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)

                            data[currPixel++] = bytes[count];
                            data[currPixel++] = bytes[count];
                            data[currPixel++] = bytes[count];
                            data[currPixel++] = 255;
                            count++;
                        }
                    }
                }
                else if (channels == 3)
                {
                    //rgb
                    for (uint row = 0; row < height; row++)
                    {
                        for (uint col = 0; col < width; col++)
                        {
                            // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)

                            data[currPixel++] = bytes[count++];
                            data[currPixel++] = bytes[count++];
                            data[currPixel++] = bytes[count++];
                            data[currPixel++] = 255;
                        }
                    }
                }
                else if (channels == 4)
                {  //rgba
                    for (uint row = 0; row < height; row++)
                    {
                        for (uint col = 0; col < width; col++)
                        {
                            // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)

                            data[currPixel++] = bytes[count++];
                            data[currPixel++] = bytes[count++];
                            data[currPixel++] = bytes[count++];
                            data[currPixel++] = 255;
                            count++;
                        }
                    }

                }


                Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
                bmp.UnlockBits(bmpData);
            }

            return bmp;
        }
    }
}
