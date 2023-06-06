//  ---------------------------------------------------------------------------------
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

using Composition.WindowsRuntimeHelpers;
using SharpDX.Direct3D11;
using System;

using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.UI.Composition;

using System.Drawing;
using System.Drawing.Imaging;
using SharpDX;
using SharpDX.DXGI;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CaptureSampleCore
{
    public enum BorderStateType
    {
        None,
        Enable,
        Disable,
        ForceEnable
    }
    public class BasicCapture : IDisposable
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

        private IDirect3DDevice device;
        private SharpDX.Direct3D11.Device d3dDevice;
        private SharpDX.DXGI.SwapChain1 swapChain;

        public byte[] array = default(byte[]);
        public int lastX;
        public int lastY;

        public int lastPositionX;
        public int lastPositionY;


        IntPtr hWnd = IntPtr.Zero;




        public BasicCapture(IDirect3DDevice d, GraphicsCaptureItem i, IntPtr hWnd)
        {
            this.hWnd = hWnd;
            item = i;

            device = d;
            d3dDevice = Direct3D11Helper.CreateSharpDXDevice(device);

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
            swapChain = new SharpDX.DXGI.SwapChain1(dxgiFactory, d3dDevice, ref description);

            framePool = Direct3D11CaptureFramePool.Create(
                device,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                2,
                i.Size);
            session = framePool.CreateCaptureSession(i);
            session.IsCursorCaptureEnabled = false;

            lastSize = i.Size;

            framePool.FrameArrived += OnFrameArrived;
        }

        public void SetRequireBoard(bool enableBoard)
        {
            BorderStateType = enableBoard ? BorderStateType.Enable : BorderStateType.Disable;
            try
            {
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

            }
            catch
            {

            }
        }

        public void Dispose()
        {
            session?.Dispose();
            framePool?.Dispose();
            swapChain?.Dispose();
            d3dDevice?.Dispose();
        }

        public void StartCapture()
        {
            session.StartCapture();
        }

        public bool isWait = false;
        public bool isStartCapture = false;
        public bool isDataSuccess = false;

        private void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            var newSize = false;

            using (var frame = sender.TryGetNextFrame())
            {

                if (!isWait)
                {
                    if (isStartCapture)
                    {
                        if (frame.ContentSize.Width != lastSize.Width || frame.ContentSize.Height != lastSize.Height)
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

                        using (var backBuffer = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0))
                        using (var bitmap = Direct3D11Helper.CreateSharpDXTexture2D(frame.Surface))
                        {
                            //d3dDevice.ImmediateContext.CopyResource(bitmap, backBuffer);
                            CopyBitmap(frame.Surface, bitmap, frame.ContentSize.Width, frame.ContentSize.Height);

                            Console.WriteLine("Capture " + DateTime.Now.ToString());

                            bitmap.Dispose();
                        };

                        isStartCapture = false;

                        swapChain.Present(0, SharpDX.DXGI.PresentFlags.None);

                        if (newSize)
                        {
                            framePool.Recreate(
                                device,
                                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                                2,
                                lastSize);
                        }
                    }
                }

                //using (var backBuffer = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0))           

            } // Retire the frame.


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


                using (var screenTexture = new Texture2D(d3dDevice, textureDesc))
                {
                    d3dDevice.ImmediateContext.CopyResource(screenTexture2D, screenTexture);

                    var mapSource = d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);
                    //var bitmap = new System.Drawing.Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                    var boundsRect = new System.Drawing.Rectangle(0, 0, Width, Height);
                    ;

                    //var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    var sourcePtr = mapSource.DataPointer;
                    //var destPtr = mapDest.Scan0;


                    byte[] managedArray = new byte[Width * Height * 4];


                    for (int y = 0; y < Height; y++)
                    {
                        System.Runtime.InteropServices.Marshal.Copy(sourcePtr, managedArray, y * Width * 4, Width * 4);
                        sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                    }

                    //bitmap.UnlockBits(mapDest);
                    d3dDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);

                    //array = ConvertBitmapToByteArray(bitmap);
                    array = managedArray;
                    lastX = Width;
                    lastY = Height;


                    if (hWnd != IntPtr.Zero)
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
            catch (Exception e)
            {
                Console.WriteLine("Error - " + e.ToString());
            }
            isWait = false;

        }

    }
}
