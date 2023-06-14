using System;
using WK.Libraries.SharpClipboardNS;
using static WK.Libraries.SharpClipboardNS.SharpClipboard;

namespace MORT.ClipboardAssist
{
    public interface IClipboard
    {
        void TextChange(string text);
    }

    public class ClipboardMonitor
    {
        private SharpClipboard _clipboard;
        private IClipboard _callback;

        public ClipboardMonitor(IClipboard callback)
        {
            _callback = callback;
 
        }

        public void SetActive(bool isActive)
        {
            try
            {
                if (isActive)
                {
                    if (_clipboard == null)
                    {
                        _clipboard = new SharpClipboard();
                        _clipboard.StartMonitoring();
                        _clipboard.ClipboardChanged += ClipboardChanged;
                    }
                }
                else
                {
                    if (_clipboard != null)
                    {
                        _clipboard.StopMonitoring();
                        _clipboard.Dispose();
                        _clipboard = null;
                    }
                }
            }
            catch
            {
                _clipboard = null;
            }
        }


        private void ClipboardChanged(Object sender, ClipboardChangedEventArgs e)
        {
            // Is the content copied of text type?
            if (e.ContentType == SharpClipboard.ContentTypes.Text)
            {
                Console.WriteLine("Start clipboard");
                // Get the cut/copied text.
                _callback.TextChange(_clipboard.ClipboardText);
            }
        }
    }
}