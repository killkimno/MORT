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
using System;
using System.Numerics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.UI.Composition;

namespace CaptureSampleCore
{
    public class BasicSampleApplication : IDisposable
    {

        private IDirect3DDevice device;
        private BasicCapture capture;

        public BasicSampleApplication()
        {
            device = Direct3D11Helper.CreateDevice();
        }


        public void Dispose()
        {
            StopCapture();
            device.Dispose();
        }

        public void StartCaptureFromItem(GraphicsCaptureItem item, IntPtr hWnd)
        {
            StopCapture();
         
            capture = new BasicCapture(device, item, hWnd);

            capture.StartCapture();
        }

        public void PrepareStart()
        {
            if (capture != null)
            {
                capture.isDataSuccess = false;
                capture.isWait = false;
                capture.isStartCapture = false;
            }
        }

        public void StartDataCapture()
        {
            capture.isStartCapture = true;
        }

        public bool GetData(ref byte[] array, ref int x, ref int y, ref int positionX, ref int positionY)
        {
            bool isSuccess = false;

            if(capture != null && capture.isDataSuccess)
            {
                isSuccess = true;
                capture.isDataSuccess = false;
                array = capture.array;
                x = capture.lastX;
                y = capture.lastY;

                positionX = capture.lastPositionX;
                positionY = capture.lastPositionY;

            }

            return isSuccess;
        }

        public void StopCapture()
        {
            capture?.Dispose();
        }
    }
}
