using System;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;

namespace MORT.ScreenCapture
{
    public class ScreenCapture : IDisposable
    {
        private IDirect3DDevice device;
        private SharpDX.Direct3D11.Device d3dDevice;

        private ScreenCaptureProcesser capture;

        public ScreenCapture()
        {
            d3dDevice = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware, SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);

            device = Direct3D11Helper.CreateDirect3DDeviceFromSharpDXDevice(d3dDevice);
        }


        public void Dispose()
        {
            StopCapture();
            device.Dispose();
        }

        public void StartCaptureFromItem(GraphicsCaptureItem item, IntPtr hWnd, bool enableYellowBoard, out BorderStateType borderStateType)
        {
            StopCapture();

            capture = new ScreenCaptureProcesser(d3dDevice, device, item, hWnd);
            capture.SetRequireBoard(enableYellowBoard);
            capture.StartCapture();

            borderStateType = capture.BorderStateType;
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

            if (capture != null && capture.isDataSuccess)
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
