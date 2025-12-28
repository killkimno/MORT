using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MORT.OcrApi.OneOcr
{
    [StructLayout(LayoutKind.Sequential)]
    struct Img
    {
        public int t;
        public int col;
        public int row;
        public int _unk;
        public long step;
        public IntPtr data_ptr;
    }
    [StructLayout(LayoutKind.Sequential)]
    struct BoundingBox
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public float x3;
        public float y3;
        public float x4;
        public float y4;
    }
    internal class NativeMethods
    {
        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long CreateOcrInitOptions(out long ctx);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long GetOcrLineCount(long instance, out long count);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long GetOcrLine(long instance, long index, out long line);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long GetOcrLineContent(long line, out IntPtr content);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long GetOcrLineBoundingBox(long line, out IntPtr boundingBoxPtr);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long GetOcrLineWordCount(long instance, out long count);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long GetOcrWord(long instance, long index, out long line);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long GetOcrWordContent(long line, out IntPtr content);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long GetOcrWordBoundingBox(long line, out IntPtr boundingBoxPtr);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long OcrProcessOptionsSetMaxRecognitionLineCount(long opt, long count);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long RunOcrPipeline(long pipeline, ref Img img, long opt, out long instance);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long CreateOcrProcessOptions(out long opt);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long OcrInitOptionsSetUseModelDelayLoad(long ctx, byte flag);

        [DllImport(@"DLL\\oneocr.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long CreateOcrPipeline(string modelPath,string key,long ctx,out long pipeline);

        [DllImport(@"DLL\\oneocr.dll", EntryPoint = "CreateOcrPipeline", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern long CreateOcrPipeline_Utf16(string modelPath, string key, long ctx, out long pipeline);


    }
}
