using System.Runtime.InteropServices;

namespace MORT.Service.Overlay
{
    internal static class OverlayRenderSynchronizationService
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmFlush();

        public static void Flush()
        {
            DwmFlush();
        }
    }
}
