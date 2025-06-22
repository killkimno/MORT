﻿//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using SharpDX.Direct3D11;
using System;

using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;
using SharpDX.DXGI;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using MORT.OcrApi.WindowOcr;
using System.Security.Cryptography.Xml;
using WinRT;
using System.Windows.Media.Media3D;
using System.Linq;

namespace MORT.ScreenCapture
{
    public enum BorderStateType
    {
        None,
        Enable,
        Disable,
        ForceEnable
    }
    public class ScreenCaptureProcesser : IDisposable
    {
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
        [Guid("f2cdd966-22ae-5ea1-9596-3a289344c3be")]
        public interface IBorderRequired
        {
            bool IsBorderRequired { get; set; }
        }
        public BorderStateType BorderStateType { get; private set; }

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);


        private GraphicsCaptureItem item;
        private Direct3D11CaptureFramePool framePool;
        private GraphicsCaptureSession session;
        private SizeInt32 lastSize;

        private IDirect3DDevice _device;
        private SharpDX.Direct3D11.Device _d3dDevice;
        private SharpDX.DXGI.SwapChain1 swapChain;

        public byte[] array = default(byte[]);
        public int lastX;
        public int lastY;

        public int lastPositionX;
        public int lastPositionY;


        IntPtr hWnd = IntPtr.Zero;

        public ScreenCaptureProcesser(SharpDX.Direct3D11.Device d3dDevice, IDirect3DDevice device, GraphicsCaptureItem i, IntPtr hWnd)
        {
            this.hWnd = hWnd;
            item = i;

            this._device = device;
            this._d3dDevice = d3dDevice;

            //this.d3dDevice = Direct3D11Helper.CreateSharpDXDevice(device);


            var dxgiFactory = new SharpDX.DXGI.Factory2();
            var description = new SharpDX.DXGI.SwapChainDescription1()
            {

                Width = item.Size.Width,
                Height = item.Size.Height,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Stereo = false,
                SampleDescription = new SharpDX.DXGI.SampleDescription()
                {
                    Count = 1,
                    Quality = 0
                },
                Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                BufferCount = 2,
                Scaling = SharpDX.DXGI.Scaling.Stretch,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
                AlphaMode = SharpDX.DXGI.AlphaMode.Premultiplied,
                Flags = SharpDX.DXGI.SwapChainFlags.None
            };
            swapChain = new SharpDX.DXGI.SwapChain1(dxgiFactory, _d3dDevice, ref description);

            try
            {
                framePool = Direct3D11CaptureFramePool.Create(_device, DirectXPixelFormat.B8G8R8A8UIntNormalized, 2, i.Size);
                session = framePool.CreateCaptureSession(i);
                session.IsCursorCaptureEnabled = false;

                lastSize = i.Size;

                framePool.FrameArrived += OnFrameArrived;
            }
            catch(Exception ex) { }

        }

        public void SetRequireBoard(bool enableBoard)
        {
            BorderStateType = enableBoard ? BorderStateType.Enable : BorderStateType.Disable;
            try
            {


                session.IsBorderRequired = enableBoard;
                /*
                var pUnk = Marshal.GetIUnknownForObject(session);
                if (pUnk != IntPtr.Zero)
                {
                    var borderRequired = Marshal.GetObjectForIUnknown(pUnk) as IBorderRequired;

                    if (borderRequired != null)
                    {
                        borderRequired.IsBorderRequired = enableBoard;
                    }
                    else
                    {
                        BorderStateType = BorderStateType.ForceEnable;
                    }
                }
                */

            }
            catch
            {
                BorderStateType = BorderStateType.ForceEnable;
            }
        }

        public void Dispose()
        {
            isStartCapture = false;
            session?.Dispose();
            framePool?.Dispose();
            swapChain?.Dispose();
        }

        public void StartCapture()
        {
            session.StartCapture();
        }

        public bool isWait = false;
        public bool isStartCapture = false;
        public bool isDataSuccess = false;

        private DateTime _dtLastUpdate = DateTime.MinValue;
        public bool AvailableGetData
        {
            get
            {
                if(isDataSuccess)
                {
                    return true;
                }

                if(array != null && array.Length > 1 && !isWait && _dtLastUpdate.AddSeconds(0.1f) <= DateTime.Now)
                {
                    return true;
                }


                return false;
            }
        }

        public void GetLastData(ref int x, ref int y, ref int positionX, ref int positionY, ref byte[] dataArray)
        {
            isDataSuccess = false;
            x = lastX;
            y = lastY;
            positionX = lastPositionX;
            positionY = lastPositionY;
            dataArray = array;
            _remainbackupFrameCount = 5;
        }


        public int TestIndex = 0;
        private int _remainbackupFrameCount = 5;
        private int _autoCaptureFrameCounter = 0;

        private void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            var newSize = false;

            using(var frame = sender.TryGetNextFrame())
            {
                _dtLastUpdate = DateTime.Now;
                //Console.WriteLine($"arrive : {TestIndex} isStartCapture {isStartCapture} isDataSuccess {isDataSuccess}");

                bool autoCapture = false;
                if(!isStartCapture)
                {
                    _autoCaptureFrameCounter++;
                    if(_autoCaptureFrameCounter >= 10)
                    {
                        autoCapture = true;
                        _autoCaptureFrameCounter = 0;
                    }
                }
                else
                {
                    _autoCaptureFrameCounter = 0;
                }

                if(isStartCapture || _remainbackupFrameCount >= 0 || autoCapture)
                {
                    if(isWait)
                    {
                        return;
                    }
                    _remainbackupFrameCount--;

                    if(_remainbackupFrameCount < -1)
                    {
                        _remainbackupFrameCount = -1;
                    }

                    isWait = true;
                    if(frame.ContentSize.Width != lastSize.Width || frame.ContentSize.Height != lastSize.Height)
                    {
                        // The thing we have been capturing has changed size.
                        // We need to resize the swap chain first, then blit the pixels.
                        // After we do that, retire the frame and then recreate the frame pool.
                        newSize = true;
                        lastSize = frame.ContentSize;

                        swapChain.ResizeBuffers(
                            2,
                            lastSize.Width,
                            lastSize.Height,
                            SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                            SharpDX.DXGI.SwapChainFlags.None);

                    }
                    using(var backBuffer = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0))
                    {
                        using(var softwareBitmap = Task.Run(async () => await SoftwareBitmap.CreateCopyFromSurfaceAsync(frame.Surface)).Result)
                        using(var buffer = softwareBitmap.LockBuffer(BitmapBufferAccessMode.ReadWrite))
                        {
                            using(var reference = buffer.CreateReference())
                            {
                                unsafe
                                {
                                    byte* data;
                                    uint capacity;
                                    var desc = buffer.GetPlaneDescription(0);
                                    var memoryBuffer = reference.As<IMemoryBufferByteAccess>();

                                    memoryBuffer.GetBuffer(out data, out capacity);

                                    byte[] arr = new byte[softwareBitmap.PixelWidth * softwareBitmap.PixelHeight * 4];
                                    Marshal.Copy((IntPtr)data, arr, 0, arr.Length);

                                    array = arr;
                                    lastX = softwareBitmap.PixelWidth;
                                    lastY = softwareBitmap.PixelHeight;

                                    try
                                    {
                                        Rect rect = new Rect();
                                        GetWindowRect(hWnd, ref rect);

                                        lastPositionX = rect.Left;
                                        lastPositionY = rect.Top;
                                        TestIndex++;
                                        //Console.WriteLine("Capture : " + _remainbackupFrameCount + "/" + DateTime.Now.ToString());
                                    }
                                    catch
                                    {

                                    }


                                    isDataSuccess = true;

                                }
                            }
                        }

                    }

                    isStartCapture = false;

                    swapChain.Present(0, SharpDX.DXGI.PresentFlags.None);

                    if(newSize)
                    {
                        framePool.Recreate(
                            _device,
                            DirectXPixelFormat.B8G8R8A8UIntNormalized,
                            2,
                            lastSize);
                    }

                    isWait = false;
                }
            }
        }


        private void CopyBitmap(IDirect3DSurface surface, SharpDX.Direct3D11.Texture2D screenTexture2D, int Width, int Height)
        {
            //isWait = true;
            try
            {                //screenTexture 카피대상
                //screenTexture2D 화면

                var textureDesc = new Texture2DDescription
                {
                    CpuAccessFlags = CpuAccessFlags.Read,
                    BindFlags = BindFlags.None,
                    Format = Format.B8G8R8A8_UNorm,
                    Width = Width,
                    Height = Height,
                    OptionFlags = ResourceOptionFlags.None,
                    MipLevels = 1,
                    ArraySize = 1,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Staging
                };


                using(var screenTexture = new Texture2D(_d3dDevice, textureDesc))
                {
                    _d3dDevice.ImmediateContext.CopyResource(screenTexture2D, screenTexture);

                    var mapSource = _d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);
                    //var bitmap = new System.Drawing.Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                    var boundsRect = new System.Drawing.Rectangle(0, 0, Width, Height);
                    ;

                    //var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    var sourcePtr = mapSource.DataPointer;
                    //var destPtr = mapDest.Scan0;


                    byte[] managedArray = new byte[Width * Height * 4];


                    for(int y = 0; y < Height; y++)
                    {
                        System.Runtime.InteropServices.Marshal.Copy(sourcePtr, managedArray, y * Width * 4, Width * 4);
                        sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                    }

                    //bitmap.UnlockBits(mapDest);
                    _d3dDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);

                    //array = ConvertBitmapToByteArray(bitmap);
                    array = managedArray;
                    lastX = Width;
                    lastY = Height;


                    if(hWnd != IntPtr.Zero)
                    {
                        Rect rect = new Rect();
                        GetWindowRect(hWnd, ref rect);

                        lastPositionX = rect.Left;
                        lastPositionY = rect.Top;

                        Console.WriteLine(rect.Left + " / " + rect.Right + " / " + rect.Top + " / " + rect.Bottom);
                    }

                    screenTexture.Dispose();
                    //bitmap.Save("./result.png");
                    //bitmap.Dispose();

                    isDataSuccess = true;
                }

            }
            catch(Exception e)
            {
                Console.WriteLine("Error - " + e.ToString());
            }
            isWait = false;

        }
    }
}
