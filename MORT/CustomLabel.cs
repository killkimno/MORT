using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MORT
{
    [ToolboxItem(true)]
    public partial class CustomLabel : System.Windows.Forms.Label
    {

        public static bool isActiveGDI = true;
        public Font TextFont { get; set; }
        public Color TextColor { get; set; }
        public Color OutlineForeColor { get; set; }
        public Color OutlineForecolor2 { get; set; }
        public Color BackColor { get; set; }
        public bool IsFillBackColor { get; set; }
        public bool IsAlignmentCenter { get; set; }
        public bool AlignmentRight { get; set; }

        public CustomLabel()
        {
            InitializeComponent();

            OutlineForeColor = Color.Green;
            TextFont = new Font("맑은 고딕", 15);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsFillBackColor)
                e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

            using (GraphicsPath gp = new GraphicsPath())
            using (Pen outline = new Pen(OutlineForeColor, 2) { LineJoin = LineJoin.Round })
            using (StringFormat sf = new StringFormat())
            using (Brush foreBrush = new SolidBrush(TextColor))
            {

                if (IsAlignmentCenter)
                    sf.Alignment = StringAlignment.Center;
                else if(AlignmentRight)
                {
                    sf.Alignment = StringAlignment.Far;
                }
                else
                {
                    sf.Alignment = StringAlignment.Near;
                }


                if (!isActiveGDI)
                {
                    e.Graphics.DrawString(Text, TextFont, foreBrush, 0, 0);
                }
                else
                {
                    try
                    {
                        gp.AddString(Text, TextFont.FontFamily, (int)TextFont.Style, e.Graphics.DpiY * TextFont.Size / 72, ClientRectangle, sf);
                        //e.Graphics.ScaleTransform(0.8f, 0.8f);
                        //e.Graphics.ScaleTransform(1.3f, 1.35f);

                        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                        using (Pen outline2 = new Pen(OutlineForecolor2, 5) { LineJoin = LineJoin.Round })
                            e.Graphics.DrawPath(outline2, gp);
                        e.Graphics.DrawPath(outline, gp);
                        e.Graphics.FillPath(foreBrush, gp);
                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show(ex.ToString());
                        TransFormLayer.isActiveGDI = false;
                        CustomLabel.isActiveGDI = false;
                        if (DialogResult.OK == MessageBox.Show("GDI+ 가 작동하지 않습니다. \n레이어 번역창의 일부 기능을 사용할 수 없습니다.\n해결법을 확인해 보겠습니까? ", "GDI+ 에서 일반 오류가 발생했습니다.", MessageBoxButtons.OKCancel))
                        {
                            Util.OpenURL("https://blog.naver.com/killkimno/70185869419");
                        }
                        e.Graphics.DrawString(Text, TextFont, foreBrush, 0, 0);
                    }


                }
            }
        }
    }
}
