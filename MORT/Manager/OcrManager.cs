using CloudVision;
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
using System.Xml.Serialization;
using static MORT.Form1;

namespace MORT.Manager
{
    public class OcrManager
    {
        public enum OcrMethodType
        {
            Normal,
            Once,
            Snap
        }

        public const string GoogleJson = "@JsonPath ";
        public const string GoogleCount = "@{0}_Count ";
        public const string GoogleCheckDay = "@{0}_CheckDay ";
        public static string GoogleJsonPath
        {
            get
            {
                if (_instance == null)
                {
                    return "";
                }

                return Instace._googleOcrSetting.Path;
            }
        }

        public static int GoogleCurrentCount
        {
            get
            {
                if (_instance == null)
                {
                    return 0;
                }

                return Instace._googleOcrSetting.Count;
            }
        }



        [Serializable]
        public class GoogleOcrInfo
        {
            public string Key;
            public DateTime RefreshTime;
            public int Count;
        }
        [Serializable]

        public class GoogleOcrSetting
        {
            public string Path;
            public List<GoogleOcrInfo> InfoList = new List<GoogleOcrInfo>();
            

            private GoogleOcrInfo _current = null;

            public void SetPath(string path)
            {   
                Path = path;
                Init();
            }

            public void Init()
            {
                if (!File.Exists(Path))
                {
                    Path = "";
                }

                if (!string.IsNullOrEmpty(Path))
                {
                    _current = InfoList.FirstOrDefault(r => r.Key == Path);
                    if (_current == null)
                    {
                        _current = new GoogleOcrInfo { Key = Path, RefreshTime = DateTime.Now, Count = 0 };
                        InfoList.Add(_current);
                    }
                }

                DateTime dtNow = DateTime.Now;
                foreach (var info in InfoList)
                {
                    if (info.RefreshTime.Year != dtNow.Year || info.RefreshTime.Month != dtNow.Month)
                    {
                        info.Count = 0;
                        info.RefreshTime = dtNow;
                    }
                }
            }

            public int Count => _current != null ? _current.Count : 0;

            public bool Limited
            {
                get
                {
                    var info = _current;

                    if(info == null)
                    {
                        return false;
                    }

                    DateTime dtNow = DateTime.Now;

                    if (info.RefreshTime.Year != dtNow.Year || info.RefreshTime.Month != dtNow.Month)
                    {
                        info.Count = 0;
                        info.RefreshTime = dtNow;
                    }

                    if (info.Count >= AdvencedOptionManager.GoogleOcrLimit)
                    {
                        return true;
                    }

                    return false;
                }
            }

            public void AddCount()
            {
                var info = _current;

                if (info == null)
                {
                    info = new GoogleOcrInfo();
                    info.Key = Path;
                    info.Count = 0;
                    info.RefreshTime = DateTime.Now;
                }
                DateTime dtNow = DateTime.Now;

                if(info.RefreshTime.Year != dtNow.Year || info.RefreshTime.Month != dtNow.Month)
                {
                    info.Count = 0;
                    info.RefreshTime = dtNow;
                }

                info.Count++;
            }

        }
        private CloudVision.Api _googleOcr = new CloudVision.Api();
        private GoogleOcrSetting _googleOcrSetting = new GoogleOcrSetting();


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
            try
            {            
                _googleOcrSetting.SetPath(path);
                SaveGoogleSetting();
                LoadGoogleOcrJson();
            }
            catch
            {

            }     
        }

        private void SaveGoogleSetting()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(_googleOcrSetting.GetType());
                using (StringWriter sw = new StringWriter())
                {
                    serializer.Serialize(sw, _googleOcrSetting);
                    Util.SaveFile(GlobalDefine.GOOGLE_OCR_PATH_FILE, sw.ToString());
                }
            }
            catch
            {

            }
            
        }


        public void LoadGoogleOcrJson()
        {
            var reader = Util.OpenFile(GlobalDefine.GOOGLE_OCR_PATH_FILE);

            if (reader != null)
            {
                try
                {
                    string data = reader.ReadToEnd();

                    XmlSerializer deserializer = new XmlSerializer(typeof(GoogleOcrSetting));
                    using (TextReader tr = new StringReader(data))
                    {
                        GoogleOcrSetting googleModel = (GoogleOcrSetting)deserializer.Deserialize(tr);
                        _googleOcrSetting = googleModel;
                        _googleOcrSetting.Init();
                    }
                }
                catch
                {

                }
                
                reader.Close();
                _googleOcr.InitClient(_googleOcrSetting.Path);
            }
        }

        public async Task<List<GoogleOcrResult>> ProcessGoogleAsync(ImgData imgData)
        {
            if(string.IsNullOrEmpty(_googleOcrSetting.Path))
            {
                GoogleOcrResult error = new GoogleOcrResult();
                error.Text = "Json 파일이 없습니다! API 설정에서 Json 파일을 선택하시기 바랍니다" ;
                error.Main = true;
                return new List<GoogleOcrResult>() { error };
            }
            
            else if (_googleOcrSetting.Limited)
            {
                GoogleOcrResult error = new GoogleOcrResult();
                error.Text = "이번달에 사용할 수 있는 횟수를 초과했습니다.\n다른 Json 파일 또는 다음달까지 기다려 주시기 바랍니다";
                error.Main = true;
                return new List<GoogleOcrResult>() { error };
            }

            MemoryStream stream = new MemoryStream();
            try
            {
               var bmp = BitmapFromBytes(imgData.data, imgData.x, imgData.y,imgData.channels,  PixelFormat.Format32bppArgb);
                bmp.Save(stream, ImageFormat.Png);
                //bmp.Save(@"F:\Project\visualStudio Projects\MORT\MORT\bin\Release\out.bmp");


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                GoogleOcrResult error = new GoogleOcrResult();
                error.Text = e.Message;
                error.Main = true;
                return new List<GoogleOcrResult>() { error } ;
            }

            string result = "empty;";
            List<GoogleOcrResult> resultList = _googleOcr.GetText2(stream.ToArray());
            result= _googleOcr.GetText(stream.ToArray());

            _googleOcrSetting.AddCount();
            SaveGoogleSetting();
            return resultList;
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
