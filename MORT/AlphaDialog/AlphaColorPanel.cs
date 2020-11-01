using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Opulos.Core.UI {

public class AlphaColorPanel : Panel {

	public event EventHandler AlphaChanged;

	NumericUpDown nudAlpha = new NumericUpDown { AutoSize = true, Minimum = 0, Maximum = 255, DecimalPlaces = 0, Increment = 1, Value = 255, Anchor = AnchorStyles.Top };
	TrackBar trackBar = new TrackBar2 { Minimum = 0, Maximum = 255, TickFrequency = 5, TickStyle = TickStyle.None, Orientation = Orientation.Horizontal, Value = 255, Anchor = AnchorStyles.Left | AnchorStyles.Right };
	Color[] colors = new Color[] { Color.White, Color.Black, Color.Green, Color.Blue, Color.Red, Color.Yellow };
	public int Cols { get; set; }
	public int SwatchSize { get; set; }

	private Color color = Color.Empty;

	public AlphaColorPanel() : base() {
		Dock = DockStyle.Fill;
		AutoSize = true;
		AutoSizeMode = AutoSizeMode.GrowAndShrink;
		DoubleBuffered = true;
		//TabStop = true;
		//SetStyle(ControlStyles.Selectable, true);
		ResizeRedraw = true;

		Cols = 3;
		SwatchSize = 100;
		trackBar.ValueChanged += trackBar_ValueChanged;
		nudAlpha.ValueChanged += nudAlpha_ValueChanged;
		Alpha = 255;

		TableLayoutPanel p = new TableLayoutPanel { Dock = DockStyle.Bottom };
		p.AutoSize = true;
		p.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		p.Controls.Add(nudAlpha, 0, 0);
		p.Controls.Add(trackBar, 1, 0);
		p.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
		p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
		Controls.Add(p);

		nudAlpha.KeyDown += nudAlpha_KeyDown;
		trackBar.KeyDown += trackBar_KeyDown;
	}

		public void SetAlpha(int alpha)
        {
			this.Alpha = alpha;
        }

	void trackBar_KeyDown(object sender, KeyEventArgs e) {
		HandleKeyEvent((Control) sender, e);
	}

	void nudAlpha_KeyDown(object sender, KeyEventArgs e) {
		HandleKeyEvent((Control) sender, e);
	}

	private void HandleKeyEvent(Control sender, KeyEventArgs e) {
		if (e.KeyCode == Keys.Enter) {
			e.SuppressKeyPress = true; // stop beep
			e.Handled = true;
		}
		else if (e.KeyCode == Keys.Escape) {
			e.SuppressKeyPress = true;
			e.Handled = true;
			Form f = FindForm();
			if (f != null)
				f.Close();
		}
		else if (e.KeyCode == Keys.Tab) {
			// seems like because the Form is displays with Show(Window) but
			// it is not modal, that stops tabs from working correctly, so
			// it is handled manually:
			sender.Parent.SelectNextControl(sender, true, true, true, true);
			e.SuppressKeyPress = true;
			e.Handled = true;
		}
	}

	void nudAlpha_ValueChanged(object sender, EventArgs e) {
		trackBar.Value = Convert.ToInt32(nudAlpha.Value);
	}

	public override Size GetPreferredSize(Size proposedSize) {
		int w = SwatchSize * Cols;
		int h = SwatchSize * (((Colors.Length - 1) / Cols) + 1);
		h += Math.Max(trackBar.Height, nudAlpha.Height);
		return new Size(w, h);
	}

	public Color Color {
		get {
			return color;
		}
		set {
			var c = value;
			color = Color.FromArgb(Alpha, c.R, c.G, c.B);
			Invalidate(); //Refresh();
		}
	}

	public int Alpha {
		get {
			return trackBar.Value;
		}
		set {
			trackBar.Value = value;
		}
	}

	public Color[] Colors {
		get {
			return colors;
		}
		set {
			colors = value;
		}
	}

	void trackBar_ValueChanged(object sender, EventArgs e) {
		nudAlpha.Value = trackBar.Value;
		Color c = Color;
		Color = Color.FromArgb(trackBar.Value, c.R, c.G, c.B);
		Refresh();
		if (AlphaChanged != null)
			AlphaChanged(this, EventArgs.Empty);
	}

	protected override void OnPaint(PaintEventArgs e) {
		base.OnPaint(e);

		int rows = ((Colors.Length - 1) / Cols) + 1;
		int r1 = Width / Cols;
		int r2 = Height / Cols;
		int r = Math.Min(r1, r2);
		if (r < SwatchSize)
			r = SwatchSize;

		int offsetX = (Width - r * Cols) / 2;
		int offsetY = ((Height - Math.Max(nudAlpha.Height, trackBar.Height)) - r * rows) / 2;

		var g = e.Graphics;
		int x = 0;
		int y = 0;
		for (int i = 0, j = 1; i < colors.Length; i++, j++) {
			Color c = colors[i];

			using (var b = new SolidBrush(c))
				g.FillRectangle(b, x + offsetX, y + offsetY, r, r);

			if (j == Cols) {
				j = 0;
				x = 0;
				y += r;
			}
			else {
				x += r;
			}
		}

		using (var b = new SolidBrush(Color))
			g.FillRectangle(b, r / 2 + offsetX, r / 2 + offsetY, 2 * r, r);
	}
}


class TrackBar2 : TrackBar {

	public TrackBar2() : base() {
		AutoSize = false;
		RECT r = GetThumbRect(this);
		Height = r.Bottom - r.Top;
	}

	public override Size GetPreferredSize(Size proposedSize) {
		Size sz = base.GetPreferredSize(proposedSize);
		RECT r = GetThumbRect(this);
		sz.Height = r.Bottom - r.Top;
		return sz;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct RECT {
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}

	[DllImport("user32.dll")]
	private static extern void SendMessage(IntPtr hwnd, uint msg, IntPtr wp, ref RECT lp);

	private const uint TBM_GETTHUMBRECT = 0x419;
	private static RECT GetThumbRect(TrackBar trackBar) {
		RECT rc = new RECT();
		SendMessage(trackBar.Handle, TBM_GETTHUMBRECT, IntPtr.Zero, ref rc);
		return rc;
	}
}


}