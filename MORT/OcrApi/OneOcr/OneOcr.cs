using Google.Api;
using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static MORT.OcrApi.OneOcr.NativeMethods;

namespace MORT.OcrApi.OneOcr
{
    public class OneOcr
    {
        private const string OneOcrDll = "oneocr.dll";
        public const string OneOcrModel = "oneocr.onemodel";
        private const string OnnxRuntimeDll = "onnxruntime.dll";
        private string OneOcrPath { get; } = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".MORT_TEST"), "OneOcr");


        public bool IsAvailable { get; private set; } = false;

        private string InstallPath { get; set; }
        private long Context { get; set; }

        private bool _initalized;


        public void CopyDll(string path)
        {
            Directory.CreateDirectory(OneOcrPath);
            //File.Delete(Path.Combine(OneOcrPath, ErrorPath));
            try
            {
                File.Copy(Path.Combine(path, OneOcrDll), Path.Combine(OneOcrPath, OneOcrDll), true);
                File.Copy(Path.Combine(path, OneOcrModel), Path.Combine(OneOcrPath, OneOcrModel), true);
                File.Copy(Path.Combine(path, OnnxRuntimeDll), Path.Combine(OneOcrPath, OnnxRuntimeDll), true);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public async ValueTask InitalizeAsync()
        {
            if(_initalized)
            {
                return;
            }

            var scketch = await GetInstallLocation("Microsoft.ScreenSketch").ConfigureAwait(false);
            if(!string.IsNullOrEmpty(scketch))
            {
                var path = Path.Combine(scketch, "SnippingTool");
                if(File.Exists(Path.Combine(path, "oneocr.dll")))
                {
                    CopyDll(path);
                }
            }

            _initalized = true;
        }

        private static async ValueTask<string?> GetInstallLocation(string appName)
        {
            var info = new ProcessStartInfo("powershell.exe", $"-Command \"(Get-AppxPackage -Name {appName}).InstallLocation\"")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
            };
            using var p = Process.Start(info);
            await p!.WaitForExitAsync().ConfigureAwait(false);
            if(p.ExitCode != 0)
            {
                return null;
            }
            var path = await p.StandardOutput.ReadToEndAsync();
            path = path.Trim();
            if(string.IsNullOrEmpty(path))
            {
                return null;
            }


            return path;
        }

        private Line[] RunOcr(Img img)
        {
            // Model key and path
            string key = "kj)TGtrK>f]b[Piow.gU+nC@s\"\"\"\"\"\"4";
            string modelPath = "oneocr.onemodel";

            var ctx = Context;

            // Create OCR pipeline
            long res = NativeMethods.CreateOcrPipeline(modelPath, key, ctx, out long pipeline);
            if(res != 0)
            {
                Console.Error.WriteLine("Failed to create OCR pipeline. Error code: " + res);
                return null;
            }

            // Set process options
            res = NativeMethods.CreateOcrProcessOptions(out long opt);
            if(res != 0)
            {
                Console.Error.WriteLine("Failed to create OCR process options.");
                return null;
            }

            res = NativeMethods.OcrProcessOptionsSetMaxRecognitionLineCount(opt, 1000);
            if(res != 0)
            {
                Console.Error.WriteLine("Failed to set max recognition line count.");
                return null;
            }
            // Run OCR pipeline
            res = NativeMethods.RunOcrPipeline(pipeline, ref img, opt, out long instance);
            if(res != 0)
            {
                Console.Error.WriteLine("Failed to run OCR pipeline. Error code: " + res);
                return null;
            }

            // Get the number of recognized lines
            res = NativeMethods.GetOcrLineCount(instance, out long lineCount);
            if(res != 0)
            {
                Console.Error.WriteLine("Failed to get OCR line count.");
                return null;
            }

            List<Line> lines = new List<Line>();

            // Get the content of each line
            for(long i = 0; i < lineCount; i++)
            {
                res = NativeMethods.GetOcrLine(instance, i, out long line);
                if(res != 0 || line == 0)
                {
                    continue;
                }

                res = NativeMethods.GetOcrLineContent(line, out IntPtr lineContentPtr);
                if(res != 0)
                {
                    continue;
                }

                string lineContent = PtrToStringUTF8(lineContentPtr);

                // Get the pointer to the bounding box
                res = NativeMethods.GetOcrLineBoundingBox(line, out IntPtr boundingBoxPtr);
                if(res == 0)
                {
                    // Map the pointer to the structure
                    BoundingBox boundingBox = Marshal.PtrToStructure<BoundingBox>(boundingBoxPtr);

                    Line data = new Line
                    {
                        Text = lineContent,
                        X1 = boundingBox.x1,
                        Y1 = boundingBox.y1,
                        X2 = boundingBox.x2,
                        Y2 = boundingBox.y2,
                        X3 = boundingBox.x3,
                        Y3 = boundingBox.y3,
                        X4 = boundingBox.x4,
                        Y4 = boundingBox.y4
                    };

                    res = NativeMethods.GetOcrLineWordCount(line, out long wordCount);
                    if(res != 0)
                    {
                        Console.Error.WriteLine("Failed to get OCR word count.");
                        return null;
                    }
                    List<Word> words = new List<Word>();
                    for(long j = 0; j < wordCount; j++)
                    {
                        res = NativeMethods.GetOcrWord(line, j, out long word);
                        if(res != 0 || word == 0)
                        {
                            continue;
                        }

                        res = NativeMethods.GetOcrWordContent(word, out IntPtr wordContentPtr);
                        if(res != 0)
                        {
                            continue;
                        }

                        string wordContent = PtrToStringUTF8(wordContentPtr);

                        // Get the pointer to the bounding box
                        res = NativeMethods.GetOcrWordBoundingBox(word, out IntPtr wordBoundingBoxPtr);
                        if(res == 0)
                        {
                            // Map the pointer to the structure
                            BoundingBox wordBoundingBox = Marshal.PtrToStructure<BoundingBox>(wordBoundingBoxPtr);
                            Word w = new Word
                            {
                                Text = wordContent,
                                X1 = wordBoundingBox.x1,
                                Y1 = wordBoundingBox.y1,
                                X2 = wordBoundingBox.x2,
                                Y2 = wordBoundingBox.y2,
                                X3 = wordBoundingBox.x3,
                                Y3 = wordBoundingBox.y3,
                                X4 = wordBoundingBox.x4,
                                Y4 = wordBoundingBox.y4
                            };
                            words.Add(w);
                        }
                        else
                        {
                            Console.Error.WriteLine("Failed to get bounding box.");

                        }
                    }
                    data.Words = words.ToArray();
                    lines.Add(data);

                }
                else
                {
                    Console.Error.WriteLine("Failed to get bounding box.");

                }
            }
            return lines.ToArray();
        }
        private string PtrToStringUTF8(IntPtr ptr)
        {
            if(ptr == IntPtr.Zero)
                return null;

            // Get the length of the string (read until null terminator)
            int length = 0;
            while(Marshal.ReadByte(ptr, length) != 0)
            {
                length++;
            }

            // Create a byte array and copy data from the pointer
            byte[] buffer = new byte[length];
            Marshal.Copy(ptr, buffer, 0, length);

            // Decode with UTF-8 encoding
            return Encoding.UTF8.GetString(buffer);
        }
        public async ValueTask<Line[]> ConvertToTextAsync(byte[] byteData, int channel, int width, int height)
        {
            await InitalizeAsync().ConfigureAwait(false);
            string imageFileName = @"e:/test.png";
            if(!IsAvailable)
            {
                //return null;
            }

            // Load the image
            Bitmap img;
            try
            {
                img = new Bitmap(imageFileName);
            }
            catch(Exception)
            {
                Console.Error.WriteLine("Can't read image!");
                return null;
            }

            // Convert the image format to BGRA
            try
            {
                using(Bitmap imgRgba = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb))
                {
                    using(Graphics g = Graphics.FromImage(imgRgba))
                    {
                        g.DrawImage(img, 0, 0);
                    }

                    int rows = imgRgba.Height;
                    int cols = imgRgba.Width;
                    int step = System.Drawing.Image.GetPixelFormatSize(imgRgba.PixelFormat) / 8 * cols;

                    // Get pixel data
                    BitmapData bitmapData = imgRgba.LockBits(new Rectangle(0, 0, imgRgba.Width, imgRgba.Height), ImageLockMode.ReadOnly, imgRgba.PixelFormat);
                    IntPtr dataPtr = bitmapData.Scan0;

                    // Create an instance of the Img structure
                    Img formattedImage = new Img
                    {
                        t = 3,
                        col = cols,
                        row = rows,
                        _unk = 0,
                        step = step,
                        data_ptr = dataPtr
                    };

                    // Execute OCR processing
                    Line[] result = RunOcr(formattedImage);

                    imgRgba.UnlockBits(bitmapData);
                    return result;
                }
            }
            finally
            {
                img.Dispose();
            }
        }


        public class Word
        {
            public string Text { get; set; }
            public float X1 { get; set; }
            public float Y1 { get; set; }
            public float X2 { get; set; }
            public float Y2 { get; set; }
            public float X3 { get; set; }
            public float Y3 { get; set; }
            public float X4 { get; set; }
            public float Y4 { get; set; }
        }

        public class Line
        {
            public string Text { get; set; }
            public float X1 { get; set; }
            public float Y1 { get; set; }
            public float X2 { get; set; }
            public float Y2 { get; set; }
            public float X3 { get; set; }
            public float Y3 { get; set; }
            public float X4 { get; set; }
            public float Y4 { get; set; }

            public Word[] Words { get; set; }

            public override string ToString()
            {
                return $"{Text}: ({X1},{Y1}),({X2},{Y2}),({X3},{Y3}),({X4},{Y4})";
            }
        }
    }
}
