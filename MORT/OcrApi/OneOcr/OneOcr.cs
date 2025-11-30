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

namespace MORT.OcrApi.OneOcr
{
    public class OneOcr
    {
        private const string OneOcrDll = "oneocr.dll";
        public const string OneOcrModel = "oneocr.onemodel";
        private const string OnnxRuntimeDll = "onnxruntime.dll";
        private string OneOcrPath { get; } = Path.Combine(AppContext.BaseDirectory, "DLL");

        public bool VerticalMode { get; set; } = true;
        public bool IsAvailable { get; private set; } = false;

        private string InstallPath { get; set; }

        private long Context { get; set; }

        private bool _initalized;

        // 안전한 DLL 검색 경로 등록 관련 필드
        private IntPtr _dllDirectoryCookie = IntPtr.Zero;
        private bool _setDllDirectoryUsed = false;
        private bool _dllPathRegistered = false;
        private const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;

        // 안전한 방법: SetDefaultDllDirectories + AddDllDirectory (Windows 8+) -> fallback SetDllDirectory
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetDefaultDllDirectories(uint directoryFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr AddDllDirectory([MarshalAs(UnmanagedType.LPWStr)] string newDirectory);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool RemoveDllDirectory(IntPtr cookie);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName);


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
                Console.WriteLine(ex.Message);
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
        private bool _test;
        private long _pipeline;
        private long _opt;
        private Line[] RunOcr(Img img)
        {
            try
            {
                // Model key and path
                string key = "kj)TGtrK>f]b[Piow.gU+nC@s\"\"\"\"\"\"4";
                string modelPath = Path.Combine(OneOcrPath, OneOcrModel);
                modelPath = Path.GetFullPath(modelPath);

                var ctx = Context;
                Console.WriteLine($"OneOcr DLL Path: {modelPath}");
                // Create OCR pipeline
                long res = NativeMethods.CreateOcrPipeline(modelPath, key, ctx, out long pipeline);
                if(res != 0)
                {
                    var msg = $"CreateOcrPipeline failed. res={res}, modelPath={modelPath}, dllPath={OneOcrPath}";
                    Console.Error.WriteLine(msg);
                    Debug.WriteLine(msg);
                    return null;
                }

                // Set process options
                res = NativeMethods.CreateOcrProcessOptions(out long opt);
                if(res != 0)
                {
                    var msg = $"CreateOcrProcessOptions failed. res={res}";
                    Console.Error.WriteLine(msg);
                    Debug.WriteLine(msg);
                    // try to cleanup pipeline if needed (if native provides)
                    return null;
                }

                res = NativeMethods.OcrProcessOptionsSetMaxRecognitionLineCount(opt, 1000);
                if(res != 0)
                {
                    var msg = $"OcrProcessOptionsSetMaxRecognitionLineCount failed. res={res}";
                    Console.Error.WriteLine(msg);
                    Debug.WriteLine(msg);
                    return null;
                }

                // Run OCR pipeline
                res = NativeMethods.RunOcrPipeline(pipeline, ref img, opt, out long instance);
                if(res != 0)
                {
                    var msg = $"RunOcrPipeline failed. res={res}";
                    Console.Error.WriteLine(msg);
                    Debug.WriteLine(msg);
                    return null;
                }

                // Get the number of recognized lines
                res = NativeMethods.GetOcrLineCount(instance, out long lineCount);
                if(res != 0)
                {
                    var msg = $"GetOcrLineCount failed. res={res}";
                    Console.Error.WriteLine(msg);
                    Debug.WriteLine(msg);
                    return null;
                }

                List<Line> lines = new List<Line>();

                // Get the content of each line
                for(long i = 0; i < lineCount; i++)
                {
                    res = NativeMethods.GetOcrLine(instance, i, out long line);
                    if(res != 0 || line == 0)
                    {
                        Console.WriteLine($"GetOcrLine failed for index {i}. res={res}, line={line}");
                        continue;
                    }

                    res = NativeMethods.GetOcrLineContent(line, out IntPtr lineContentPtr);
                    if(res != 0 || lineContentPtr == IntPtr.Zero)
                    {
                        Console.WriteLine($"GetOcrLineContent failed for line {line}. res={res}");
                        continue;
                    }

                    string lineContent = PtrToStringUTF8(lineContentPtr);

                    res = NativeMethods.GetOcrLineBoundingBox(line, out IntPtr boundingBoxPtr);
                    if(res != 0 || boundingBoxPtr == IntPtr.Zero)
                    {
                        Console.WriteLine($"GetOcrLineBoundingBox failed for line {line}. res={res}");
                        continue;
                    }

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
                        Console.WriteLine($"GetOcrLineWordCount failed for line {line}. res={res}");
                        return null;
                    }

                    List<Word> words = new List<Word>();
                    for(long j = 0; j < wordCount; j++)
                    {
                        res = NativeMethods.GetOcrWord(line, j, out long word);
                        if(res != 0 || word == 0)
                        {
                            Console.WriteLine($"GetOcrWord failed for line {line} idx {j}. res={res}, word={word}");
                            continue;
                        }

                        res = NativeMethods.GetOcrWordContent(word, out IntPtr wordContentPtr);
                        if(res != 0 || wordContentPtr == IntPtr.Zero)
                        {
                            Console.WriteLine($"GetOcrWordContent failed for word {word}. res={res}");
                            continue;
                        }

                        string wordContent = PtrToStringUTF8(wordContentPtr);

                        res = NativeMethods.GetOcrWordBoundingBox(word, out IntPtr wordBoundingBoxPtr);
                        if(res != 0 || wordBoundingBoxPtr == IntPtr.Zero)
                        {
                            Console.WriteLine($"GetOcrWordBoundingBox failed for word {word}. res={res}");
                            continue;
                        }

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
                    data.Words = words.ToArray();
                    lines.Add(data);
                }

                return lines.ToArray();
            }
            catch(DllNotFoundException ex)
            {
                var msg = $"DLL not found: {ex.Message}";
                Console.Error.WriteLine(msg);
                Debug.WriteLine(msg);
                throw;
            }
            catch(BadImageFormatException ex)
            {
                var msg = $"BadImageFormat (bitness mismatch?): {ex.Message}";
                Console.Error.WriteLine(msg);
                Debug.WriteLine(msg);
                throw;
            }
            catch(Exception ex)
            {
                var msg = $"RunOcr unexpected error: {ex.Message}\n{ex.StackTrace}";
                Console.Error.WriteLine(msg);
                Debug.WriteLine(msg);
                throw;
            }
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

        private (Bitmap bitmap, BitmapData bitmapData) CreateBitmapDataFromBytes(in byte[] byteData, int channel, int width, int height)
        {
            // 생성된 Bitmap과 LockBits 결과(BitmapData)를 반환합니다.
            // 호출자는 사용 후 반드시 bitmap.UnlockBits(bitmapData) 및 bitmap.Dispose()를 호출해야 합니다.
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            try
            {
                int stride = Math.Abs(bmData.Stride);
                int targetChannels = 3; // Format24bppRgb
                byte[] buffer = new byte[stride * height];

                // 입력 바이트 배열은 다음 중 하나라고 가정:
                // channel == 1 : 그레이스케일 (width*height)
                // channel == 3 : 3채널(RGB) (width*height*3)
                // channel == 4 : 4채널(RGBA) (width*height*4)
                for(int y = 0; y < height; y++)
                {
                    int srcRowOffset = y * width * channel;
                    int dstRowOffset = y * stride;
                    int src = srcRowOffset;
                    int dst = dstRowOffset;

                    for(int x = 0; x < width; x++)
                    {
                        if(channel == 1)
                        {
                            byte v = byteData[src++];
                            buffer[dst++] = v;
                            buffer[dst++] = v;
                            buffer[dst++] = v;
                        }
                        else if(channel == 3)
                        {
                            // 입력이 RGB 순서라 가정. (프로젝트 기존 코드와 동일하게 처리)
                            buffer[dst++] = byteData[src++]; // R or B depending on input format
                            buffer[dst++] = byteData[src++];
                            buffer[dst++] = byteData[src++];
                        }
                        else if(channel == 4)
                        {
                            // RGBA -> RGB (알파 무시)
                            buffer[dst++] = byteData[src++]; // R or B depending on input format
                            buffer[dst++] = byteData[src++];
                            buffer[dst++] = byteData[src++];
                            src++; // skip alpha
                        }
                        else
                        {
                            // 알 수 없는 채널 수는 그레이스케일로 처리
                            byte v = byteData[src++];
                            buffer[dst++] = v;
                            buffer[dst++] = v;
                            buffer[dst++] = v;
                        }
                    }
                    // stride의 패딩 바이트는 기본값(0)으로 남겨둠
                }

                Marshal.Copy(buffer, 0, bmData.Scan0, buffer.Length);

                // bitmap과 bmData를 반환 (bmData는 아직 lock 상태)
                return (bitmap, bmData);
            }
            catch
            {
                // 실패 시 Unlock 및 Dispose 처리
                try { bitmap.UnlockBits(bmData); } catch { }
                bitmap.Dispose();
                throw;
            }
        }

        public async ValueTask<Line[]> ConvertToTextAsync(byte[] byteData, int channel, int width, int height, Action clearBitmapCallback)
        {
            await InitalizeAsync().ConfigureAwait(false);

            if(!IsAvailable)
            {
                // 필요시 null 반환 (기존 동작과 동일하게 유지)
            }

            Bitmap bitmap = null;
            BitmapData bitmapData = null;

            try
            {
                // byte 배열로부터 Bitmap과 LockBits된 BitmapData 생성
                (bitmap, bitmapData) = CreateBitmapDataFromBytes(byteData, channel, width, height);

                int rows = height;
                int cols = width;
                long step = Math.Abs(bitmapData.Stride);
                IntPtr dataPtr = bitmapData.Scan0;

                // 네이티브 라이브러리에 맞춘 Img 구조체 생성
                Img formattedImage = new Img
                {
                    t = channel,
                    col = cols,
                    row = rows,
                    _unk = 0,
                    step = step,
                    data_ptr = dataPtr
                };

                clearBitmapCallback?.Invoke();
                // OCR 실행 (bitmap은 lock 상태여야 함)
                Line[] result = RunOcr(formattedImage);
                if(result == null)
                {
                    return null;
                }

                return AdjustLines(result);
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine("ConvertToTextAsync 오류: " + ex.Message);
                return null;
            }
            finally
            {
                // UnlockBits 및 리소스 해제
                if(bitmap != null && bitmapData != null)
                {
                    try { bitmap.UnlockBits(bitmapData); } catch { }
                    bitmap.Dispose();
                }

            }
        }

        private Line[] AdjustLines(Line[] lines)
        {
            if(lines == null || lines.Length == 0)
                return lines;

            // VerticalMode가 아닐 때는 그대로 반환
            if(!this.VerticalMode)
                return lines;

            const float verticalRatioThreshold = 1.5f; // 높이/너비 비율 임계값(필요 시 조정)

            var verticalIndices = new List<int>();
            var verticalLines = new List<Line>();

            for(int i = 0; i < lines.Length; i++)
            {
                var l = lines[i];
                if(l == null) continue;

                if(IsVerticalLine(l, verticalRatioThreshold))
                {
                    verticalIndices.Add(i);
                    verticalLines.Add(l);
                }
            }

            if(verticalLines.Count == 0)
                return lines;

            // X1 내림차순, Y1 오름차순으로 정렬
            verticalLines = verticalLines
                .OrderByDescending(l => l.X1)
                .ThenBy(l => l.Y1)
                .ToList();

            // 원본 위치 유지: 수직 라인 자리만 정렬된 라인으로 교체
            var result = (Line[])lines.Clone();
            for(int k = 0; k < verticalIndices.Count; k++)
            {
                result[verticalIndices[k]] = verticalLines[k];
            }

            return result;
        }

        private static bool IsVerticalLine(Line l, float ratioThreshold)
        {
            // 네 점에서 bounding box 계산
            float minX = Math.Min(Math.Min(l.X1, l.X2), Math.Min(l.X3, l.X4));
            float maxX = Math.Max(Math.Max(l.X1, l.X2), Math.Max(l.X3, l.X4));
            float minY = Math.Min(Math.Min(l.Y1, l.Y2), Math.Min(l.Y3, l.Y4));
            float maxY = Math.Max(Math.Max(l.Y1, l.Y2), Math.Max(l.Y3, l.Y4));

            float width = Math.Max(0.0f, maxX - minX);
            float height = Math.Max(0.0f, maxY - minY);

            if(width == 0f)
            {
                return height > 0f;
            }

            return height > width * ratioThreshold;
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