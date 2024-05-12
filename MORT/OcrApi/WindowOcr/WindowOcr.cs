using System;
using System.Collections.Generic;
using WinRT;
using System.Threading.Tasks;
using Windows.Media.Ocr;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Globalization;
using System.Runtime.InteropServices;
using Windows.Media.SpeechSynthesis;
using Windows.Media.Playback;
using Windows.Media.Core;

namespace MORT.OcrApi.WindowOcr
{
    [ComImport]
    [Guid("5b0d3235-4dba-4d44-865e-8f1d0e4fd04d")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    unsafe interface IMemoryBufferByteAccess
    {
        void GetBuffer(out byte* buffer, out uint capacity);
    }

    //ready = 이미지 받을 준비.
    //process = ocr 처리중
    //
    public enum ProcessType
    {
        Ready = 0, Process = 1, ImgLoading = 2,
    }

    public class WindowOcr
    {
        private MediaPlayer mediaElement = new MediaPlayer();
        //MediaElement mediaElement = new MediaElement();
        private string resultString = "";
        private int result = 0;
        private SoftwareBitmap bitmap = null;
        private SoftwareBitmap bitmap2 = null;
        private ProcessType processType = ProcessType.Ready;
        public OcrEngine ocrEngine = null;
        public Windows.Media.Ocr.OcrResult ocrResult = null;

        public void TextToSpeach(string text, int type)
        {
            AsyncTextToSpeach(text, type);

        }

        private async Task AsyncTextToSpeach(string text, int type)
        {

            if(type == 1)
            {
                if(mediaElement.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
                {
                    return;
                }
            }

            //     MediaElement mediaElement = new MediaElement();
            // The object for controlling the speech synthesis engine (voice).
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);

            // Send the stream to the media object.
            //mediaElement.SetSource(stream, stream.ContentType);

            mediaElement.Source = MediaSource.CreateFromStream(stream, stream.ContentType);

            mediaElement.Volume = 1;
            mediaElement.Play();


        }

        //OCR 에 사용할 이미지 저장.
        public void StartMakeBitmap()
        {
            if(processType != ProcessType.Process)
            {
                DoMakeBitmap();
            }
        }

        public void SetBitMap(byte[] dataList, int channels, int x, int y)
        {
            SetBitMapData(dataList, channels, x, y);
        }


        public void LoadImgFromByte(byte[] data, int x, int y)
        {
            if(processType != ProcessType.Process)
            {
                LoadBitMapFromData(data, x, y);
            }

        }

        private byte[] dataList = null;

        private List<byte> r = null;
        private List<byte> g = null;
        private List<byte> b = null;

        private int channels;
        private int dataX;
        private int dataY;
        private void SetBitMapData_Old(List<byte> r, List<byte> g, List<byte> b, int x, int y)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            dataX = x;
            dataY = y;
        }

        private void SetBitMapData(byte[] data, int channels, int x, int y)
        {
            this.dataList = data;
            this.channels = channels;
            dataX = x;
            dataY = y;
        }

        public void ClearList(List<byte> lista)
        {
            if(lista == null)
                return;

            lista.Clear();
            int identificador = GC.GetGeneration(lista);
            GC.Collect(identificador, GCCollectionMode.Forced);

            lista.TrimExcess();
            lista = null;
        }

        public void DoMakeBitmap()
        {
            processType = ProcessType.ImgLoading;

            int BYTES_PER_PIXEL = 4;

            bitmap2 = new SoftwareBitmap(BitmapPixelFormat.Bgra8, dataX, dataY);

            unsafe
            {
                using(BitmapBuffer buffer = bitmap2.LockBuffer(BitmapBufferAccessMode.Write))
                {

                    using(var referenceDest = buffer.CreateReference())
                    {
                        byte* data;
                        uint capacity;
                        var desc = buffer.GetPlaneDescription(0);
                        var memoryBuffer = referenceDest.As<IMemoryBufferByteAccess>();

                        memoryBuffer.GetBuffer(out data, out capacity);
                        //referenceDest.As<IMemoryBufferByteAccess>().GetBuffer(out  data, out  capacity);
                        //((IMemoryBufferByteAccess)referenceDest).GetBuffer(out data, out capacity);


                        //Marshal.Copy((IntPtr)data, dataList, 0, dataList.Length);

                        int count = 0;
                        if(channels == 1)
                        {
                            for(uint row = 0; row < dataY; row++)
                            {
                                for(uint col = 0; col < dataX; col++)
                                {
                                    var currPixel = desc.StartIndex + desc.Stride * row + BYTES_PER_PIXEL * col;

                                    // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)

                                    data[currPixel + 0] = dataList[count];
                                    data[currPixel + 1] = dataList[count];
                                    data[currPixel + 2] = dataList[count];
                                    count++;
                                }
                            }
                        }
                        else if(channels == 3)
                        {
                            //rgb
                            for(uint row = 0; row < dataY; row++)
                            {
                                for(uint col = 0; col < dataX; col++)
                                {
                                    var currPixel = desc.StartIndex + desc.Stride * row + BYTES_PER_PIXEL * col;

                                    // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)

                                    data[currPixel + 0] = dataList[count++];
                                    data[currPixel + 1] = dataList[count++];
                                    data[currPixel + 2] = dataList[count++];
                                }
                            }
                        }
                        else if(channels == 4)
                        {  //rgba
                            for(uint row = 0; row < dataY; row++)
                            {
                                for(uint col = 0; col < dataX; col++)
                                {
                                    var currPixel = desc.StartIndex + desc.Stride * row + BYTES_PER_PIXEL * col;

                                    // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)

                                    data[currPixel + 0] = dataList[count++];
                                    data[currPixel + 1] = dataList[count++];
                                    data[currPixel + 2] = dataList[count++];
                                    data[currPixel + 3] = 255;
                                    count++;
                                }
                            }

                        }


                    }
                }
            }
            Array.Clear(dataList, 0, dataList.Length);

            int identificador = GC.GetGeneration(dataList);
            GC.Collect(identificador, GCCollectionMode.Forced);

            dataList = null;

            processType = ProcessType.Ready;
            //SaveImageAsync("mort_resut.png");
        }


        public void LoadBitMapFromData(byte[] byteData, int x, int y)
        {
            processType = ProcessType.ImgLoading;
            int BYTES_PER_PIXEL = 4;
            bitmap2 = new SoftwareBitmap(BitmapPixelFormat.Bgra8, x, y);
            unsafe
            {
                using(BitmapBuffer buffer = bitmap2.LockBuffer(BitmapBufferAccessMode.Write))
                {
                    using(var referenceDest = buffer.CreateReference())
                    {
                        byte* data;
                        uint capacity;
                        var desc = buffer.GetPlaneDescription(0);
                        ((IMemoryBufferByteAccess)referenceDest).GetBuffer(out data, out capacity);
                        int count = 0;
                        for(uint row = 0; row < y; row++)
                        {
                            for(uint col = 0; col < x; col++)
                            {
                                var currPixel = desc.StartIndex + desc.Stride * row + BYTES_PER_PIXEL * col;

                                data[currPixel + 0] = byteData[count + 0]; // Blue
                                data[currPixel + 1] = byteData[count + 1];  // Green
                                data[currPixel + 2] = byteData[count + 2]; // Red
                                data[currPixel + 3] = byteData[count + 3]; ; // alpha

                                count = count + 4;

                            }
                        }
                    }
                }
            }
            //SaveImageAsync("mort_resut.png");
            processType = ProcessType.Ready;
        }

        //ocr 처리.
        public string ProcessOCR()
        {
            OCR();

            return resultString;
        }

        //OCR 사용 가능한지 확인.
        public bool GetIsAvailable()
        {
            bool isAvailable = false;

            if(processType == ProcessType.Ready)
            {
                isAvailable = true;
            }

            return isAvailable;
        }

        public List<(string Code, string DisplayName)> GetAvailableLanguageList()
        {
            List<(string, string)> codeList = new();
            IReadOnlyList<Language> list = OcrEngine.AvailableRecognizerLanguages;

            for(int i = 0; i < list.Count; i++)
            {
                codeList.Add((list[i].LanguageTag, list[i].DisplayName));
            }

            return codeList;
        }

        //OCR 처리
        public void InitOcr(string code)
        {
            ocrEngine = null;
            if(code == "")
            {
                ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            }
            else
            {
                Language ocrLanguage = new Language(code);
                if(OcrEngine.IsLanguageSupported(ocrLanguage))
                {
                    ocrEngine = OcrEngine.TryCreateFromLanguage(ocrLanguage);
                }
                else
                {
                    ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
                }

            }

            processType = ProcessType.Ready;

        }

        //이 dll 자체를 사용 가능한지 확인.
        public bool GetIsAvailableDLL()
        {
            bool isAvailable = false;

            return isAvailable;
        }

        private async void OCR()
        {
            //resultString = "sdfsdf@!ER";
            if(processType != ProcessType.Process)
            {

                if(bitmap2 != null && ocrEngine != null)
                {
                    processType = ProcessType.Process;
                    //resultString = "1";
                    ocrResult = null;
                    ocrResult = await ocrEngine.RecognizeAsync(bitmap2);

                    //---------
                    //라인으로 하기.
                    string result = "";
                    for(int i = 0; i < ocrResult.Lines.Count; i++)
                    {
                        result += ocrResult.Lines[i].Text + System.Environment.NewLine;
                    }
                    resultString = result;
                    //------------

                    //원문으로 하기.
                    //resultString = ocrResult.Text;
                    processType = ProcessType.Ready;

                }
                else
                {
                    resultString = "bitmap null";
                    processType = ProcessType.Ready;
                }
            }
            else
            {
                resultString = " not ready";
            }
        }

        public WinOCRResultData MakeResultData()
        {
            WinOCRResultData data = new WinOCRResultData();
            if(ocrResult != null)
            {
                data.lineCount = ocrResult.Lines.Count;
                if(data.lineCount == 0)
                {
                    data.isEmpty = true;

                    data.wordCounts = new int[1];
                    data.words = new string[1];
                    data.x = new double[1];
                    data.y = new double[1];
                    data.sizeY = new double[1];
                    data.sizeX = new double[1];
                }
                else
                {
                    data.isEmpty = false;
                    int totalWordCount = 0;
                    data.wordCounts = new int[data.lineCount];
                    for(int i = 0; i < ocrResult.Lines.Count; i++)
                    {
                        int wordCount = 0;
                        for(int j = 0; j < ocrResult.Lines[i].Words.Count; j++)
                        {
                            wordCount++;
                            totalWordCount++;
                        }
                        data.wordCounts[i] = wordCount;
                    }

                    data.wordsIndex = totalWordCount;

                    if(ocrResult.TextAngle == null)
                    {
                        data.angle = 180;
                    }
                    else
                    {
                        data.angle = (double)ocrResult.TextAngle;
                    }

                    data.words = new string[totalWordCount];
                    data.x = new double[totalWordCount];
                    data.y = new double[totalWordCount];
                    data.sizeY = new double[totalWordCount];
                    data.sizeX = new double[totalWordCount];

                    int index = 0;
                    for(int i = 0; i < ocrResult.Lines.Count; i++)
                    {
                        for(int j = 0; j < ocrResult.Lines[i].Words.Count; j++)
                        {
                            data.words[index] = ocrResult.Lines[i].Words[j].Text;
                            data.x[index] = ocrResult.Lines[i].Words[j].BoundingRect.X;
                            data.y[index] = ocrResult.Lines[i].Words[j].BoundingRect.Y;
                            data.sizeY[index] = ocrResult.Lines[i].Words[j].BoundingRect.Height;
                            data.sizeX[index] = ocrResult.Lines[i].Words[j].BoundingRect.Width;
                            index++;
                        }

                    }
                }
            }

            return data;
        }

        public string GetText()
        {
            return resultString;
        }

        public string Test(byte[] test)
        {
            // OCR();

            return resultString;
        }

        public string TestMat(byte[] data)
        {
            return "";
        }



        private async Task LoadImage(StorageFile file)
        {
            using(var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                bitmap2 = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                Windows.Graphics.Imaging.PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
            }

        }

        private async Task SaveImageAsync(string filename)
        {
            StorageFolder pictureFolder = KnownFolders.DocumentsLibrary;
            StorageFile file = await pictureFolder.CreateFileAsync(
                  filename,
                  CreationCollisionOption.ReplaceExisting);

            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId, await file.OpenAsync(FileAccessMode.ReadWrite));

            encoder.SetSoftwareBitmap(bitmap2);

            await encoder.FlushAsync();

        }
        private async Task LoadSampleImage()
        {
            var file = await KnownFolders.PicturesLibrary.GetFileAsync("mortresult.png");
            await LoadImage(file);
        }
    }
}
