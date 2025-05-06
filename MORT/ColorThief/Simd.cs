using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace MORT.ColorThief
{
    static class Simd
    {
        public const int Sigbits = 5;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Rshift(Span<byte> source, int shift)
        {
            if(!Vector.IsHardwareAccelerated || source.Length < Vector<byte>.Count)
            {
                foreach(ref var item in source)
                {
                    item >>= shift;
                }
            }
            else
            {
                ref var current = ref MemoryMarshal.GetReference(source);
                ref var end = ref Unsafe.Add(ref current, source.Length);
                ref var to = ref Unsafe.Add(ref current, source.Length - Vector<byte>.Count);

                // SIMDを使用して処理
                while(Unsafe.IsAddressLessThan(ref current, ref to))
                {
                    var chunk = Vector.LoadUnsafe(ref current);
                    chunk = Vector.ShiftRightLogical(chunk, shift);
                    chunk.StoreUnsafe(ref current);
                    current = ref Unsafe.Add(ref current, Vector<byte>.Count);
                }

                // SIMDで処理できなかった余り部分を処理
                if(Unsafe.IsAddressLessThan(ref current, ref end))
                {
                    var remainingBytes = source.Length % Vector<byte>.Count;
                    ref var last = ref Unsafe.Add(ref current, -remainingBytes);
                    var lastChunk = Vector.LoadUnsafe(ref last, (nuint)remainingBytes);
                    lastChunk = Vector.ShiftRightLogical(lastChunk, shift);
                    lastChunk.StoreUnsafe(ref last, (nuint)remainingBytes);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte min, byte mas) MinMax(ReadOnlySpan<byte> source)
        {
            if(!Vector128.IsHardwareAccelerated || source.Length < Vector128<byte>.Count)
            {
                var min = source[0];
                var max = min;
                for(int i = 1; i < source.Length; i++)
                {
                    min = Math.Min(min, source[i]);
                    max = Math.Max(max, source[i]);
                }
                return (min, max);
            }
            else if(!Vector256.IsHardwareAccelerated || source.Length < Vector256<byte>.Count)
            {
                ref var current = ref MemoryMarshal.GetReference(source);
                ref var to = ref Unsafe.Add(ref current, source.Length - Vector128<byte>.Count);

                var vectorMin = Vector128.LoadUnsafe(ref current);
                var vectorMax = vectorMin;
                current = ref Unsafe.Add(ref current, Vector128<byte>.Count);
                while(Unsafe.IsAddressLessThan(ref current, ref to))
                {
                    vectorMin = Vector128.Min(vectorMin, Vector128.LoadUnsafe(ref current));
                    vectorMax = Vector128.Max(vectorMax, Vector128.LoadUnsafe(ref current));
                    current = ref Unsafe.Add(ref current, Vector128<byte>.Count);
                }
                vectorMin = Vector128.Min(vectorMin, Vector128.LoadUnsafe(ref to));
                vectorMax = Vector128.Max(vectorMax, Vector128.LoadUnsafe(ref to));

                var min = vectorMin[0];
                var max = vectorMax[0];
                for(int i = 1; i < Vector128<byte>.Count; i++)
                {
                    min = Math.Min(min, vectorMin[i]);
                    max = Math.Max(max, vectorMax[i]);
                }
                return (min, max);
            }
            else if(!Vector512.IsHardwareAccelerated || source.Length < Vector512<byte>.Count)
            {
                ref var current = ref MemoryMarshal.GetReference(source);
                ref var to = ref Unsafe.Add(ref current, source.Length - Vector256<byte>.Count);

                var vectorMin = Vector256.LoadUnsafe(ref current);
                var vectorMax = vectorMin;
                current = ref Unsafe.Add(ref current, Vector256<byte>.Count);
                while(Unsafe.IsAddressLessThan(ref current, ref to))
                {
                    vectorMin = Vector256.Min(vectorMin, Vector256.LoadUnsafe(ref current));
                    vectorMax = Vector256.Max(vectorMax, Vector256.LoadUnsafe(ref current));
                    current = ref Unsafe.Add(ref current, Vector256<byte>.Count);
                }
                vectorMin = Vector256.Min(vectorMin, Vector256.LoadUnsafe(ref to));
                vectorMax = Vector256.Max(vectorMax, Vector256.LoadUnsafe(ref to));

                var min = vectorMin[0];
                var max = vectorMax[0];
                for(int i = 1; i < Vector256<byte>.Count; i++)
                {
                    min = Math.Min(min, vectorMin[i]);
                    max = Math.Max(max, vectorMax[i]);
                }
                return (min, max);
            }
            else
            {
                ref var current = ref MemoryMarshal.GetReference(source);
                ref var to = ref Unsafe.Add(ref current, source.Length - Vector512<byte>.Count);

                var vectorMin = Vector512.LoadUnsafe(ref current);
                var vectorMax = vectorMin;
                current = ref Unsafe.Add(ref current, Vector512<byte>.Count);
                while(Unsafe.IsAddressLessThan(ref current, ref to))
                {
                    vectorMin = Vector512.Min(vectorMin, Vector512.LoadUnsafe(ref current));
                    vectorMax = Vector512.Max(vectorMax, Vector512.LoadUnsafe(ref current));
                    current = ref Unsafe.Add(ref current, Vector512<byte>.Count);
                }
                vectorMin = Vector512.Min(vectorMin, Vector512.LoadUnsafe(ref to));
                vectorMax = Vector512.Max(vectorMax, Vector512.LoadUnsafe(ref to));

                var min = vectorMin[0];
                var max = vectorMax[0];
                for(int i = 1; i < Vector512<byte>.Count; i++)
                {
                    min = Math.Min(min, vectorMin[i]);
                    max = Math.Max(max, vectorMax[i]);
                }
                return (min, max);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetColorIndexies(ReadOnlySpan<byte> r, ReadOnlySpan<byte> g, ReadOnlySpan<byte> b, Span<short> indexis)
        {
            if(!Vector.IsHardwareAccelerated || indexis.Length < Vector<byte>.Count)
            {
                for(int i = 0; i < indexis.Length; i++)
                {
                    indexis[i] = (short)((r[i] << (2 * Sigbits)) + (g[i] << Sigbits) + b[i]);
                }
            }
            else
            {
                ref var rCurrent = ref MemoryMarshal.GetReference(r);
                ref var rTo = ref Unsafe.Add(ref rCurrent, r.Length - Vector<byte>.Count);
                ref var gCurrent = ref MemoryMarshal.GetReference(g);
                ref var gTo = ref Unsafe.Add(ref gCurrent, g.Length - Vector<byte>.Count);
                ref var bCurrent = ref MemoryMarshal.GetReference(b);
                ref var bTo = ref Unsafe.Add(ref bCurrent, b.Length - Vector<byte>.Count);
                ref var iCurrent = ref MemoryMarshal.GetReference(indexis);
                ref var iEnd = ref Unsafe.Add(ref iCurrent, indexis.Length);
                ref var iTo = ref Unsafe.Add(ref iCurrent, indexis.Length - Vector<short>.Count);

                while(Unsafe.IsAddressLessThan(ref iCurrent, ref iTo))
                {
                    Vector.Widen(Vector.LoadUnsafe(ref rCurrent), out var rL, out var rU);
                    rL = Vector.ShiftLeft(rL, 2 * Sigbits);
                    Vector.Widen(Vector.LoadUnsafe(ref gCurrent), out var gL, out var gU);
                    gL = Vector.ShiftLeft(gL, Sigbits);
                    Vector.Widen(Vector.LoadUnsafe(ref bCurrent), out var bL, out var bU);
                    var indexL = Vector.Add(Vector.Add(rL, gL), bL);
                    indexL.As<ushort, short>().StoreUnsafe(ref iCurrent);
                    iCurrent = ref Unsafe.Add(ref iCurrent, Vector<short>.Count);

                    // オーバーフローして書き込んでる可能性あるけど、大丈夫そ？
                    rU = Vector.ShiftLeft(rU, 2 * Sigbits);
                    gU = Vector.ShiftLeft(gU, Sigbits);
                    var indexU = Vector.Add(Vector.Add(rU, gU), bU);
                    indexU.As<ushort, short>().StoreUnsafe(ref iCurrent);
                    iCurrent = ref Unsafe.Add(ref iCurrent, Vector<short>.Count);

                    rCurrent = ref Unsafe.Add(ref rCurrent, Vector<byte>.Count);
                    gCurrent = ref Unsafe.Add(ref gCurrent, Vector<byte>.Count);
                    bCurrent = ref Unsafe.Add(ref bCurrent, Vector<byte>.Count);
                }

                // SIMDで処理できなかった余り部分を処理
                if(Unsafe.IsAddressLessThan(ref iCurrent, ref iEnd))
                {
                    var remainingBytes = indexis.Length % Vector<byte>.Count;
                    var remainingShorts = indexis.Length % Vector<short>.Count;
                    ref var rLast = ref Unsafe.Add(ref rCurrent, -remainingBytes);
                    ref var gLast = ref Unsafe.Add(ref gCurrent, -remainingBytes);
                    ref var bLast = ref Unsafe.Add(ref bCurrent, -remainingBytes);
                    ref var iLast = ref Unsafe.Add(ref iCurrent, -remainingShorts);

                    var rL = Vector.WidenLower(Vector.LoadUnsafe(ref rLast, (nuint)remainingBytes));
                    rL = Vector.ShiftLeft(rL, 2 * Sigbits);
                    var gL = Vector.WidenLower(Vector.LoadUnsafe(ref gLast, (nuint)remainingBytes));
                    gL = Vector.ShiftLeft(gL, Sigbits);
                    var bL = Vector.WidenLower(Vector.LoadUnsafe(ref bLast, (nuint)remainingBytes));
                    var indexL = Vector.Add(Vector.Add(rL, gL), bL);
                    indexL.As<ushort, short>().StoreUnsafe(ref iLast, (nuint)remainingShorts);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Init<T>(Span<T> data, T value)
            where T : struct
        {
            if(!Vector.IsHardwareAccelerated || data.Length < Vector<T>.Count)
            {
                for(var l = 0; l < data.Length; l++)
                {
                    data[l] = value;
                }
            }
            else
            {
                ref var current = ref MemoryMarshal.GetReference(data);
                ref var end = ref Unsafe.Add(ref current, data.Length);
                ref var to = ref Unsafe.Add(ref current, data.Length - Vector<T>.Count);
                var initValue = new Vector<T>(value);

                while(Unsafe.IsAddressLessThan(ref current, ref to))
                {
                    initValue.StoreUnsafe(ref current);
                    current = ref Unsafe.Add(ref current, Vector<T>.Count);
                }

                if(Unsafe.IsAddressLessThan(ref current, ref end))
                {
                    var remainingBytes = data.Length % Vector<T>.Count;
                    ref var last = ref Unsafe.Add(ref current, -remainingBytes);
                    initValue.StoreUnsafe(ref last, (nuint)remainingBytes);
                }
            }
        }
    }
}
