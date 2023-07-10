using System;
using System.Drawing;


namespace MORT.SettingData
{
    [Serializable]
    public class SerializableFont
    {
        public string FontFamily { get; set; }
        public GraphicsUnit GraphicsUnit { get; set; }
        public float Size { get; set; }
        public FontStyle Style { get; set; }

        /// <summary>
        /// Intended for xml serialization purposes only
        /// </summary>
        private SerializableFont() { }

        public SerializableFont(Font f)
        {
            FontFamily = f.FontFamily.Name;
            GraphicsUnit = f.Unit;
            Size = f.Size;
            Style = f.Style;
        }

        public static SerializableFont FromFont(Font f)
        {
            return new SerializableFont(f);
        }

        public Font ToFont()
        {
            return new Font(FontFamily, Size, Style,
                GraphicsUnit);
        }
    }
}
