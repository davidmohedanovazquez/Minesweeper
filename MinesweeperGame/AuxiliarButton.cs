using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinesweeperGame
{
    public partial class AuxiliarButton : Button
    {
        public AuxiliarButton()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
            SizeF stringSize = e.Graphics.MeasureString(this.Text, this.Font);
            if (this.Text == "🏴" || this.Text == "💣")
                stringSize.Width += 10;
            else if (this.Text == "❌")
                stringSize.Width += 5;
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, new Point((this.ClientRectangle.Width / 2) - (int)(stringSize.Width / 2), (this.ClientRectangle.Height / 2) - (int)(stringSize.Height / 2)), this.ForeColor, this.BackColor);
        }

        Color bg;
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            bg = this.BackColor;
            this.BackColor = Color.FromArgb((int)(BackColor.R / 1.3), (int)(BackColor.G / 1.3), (int)(BackColor.B / 1.3));
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = bg;
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (this.Text == "🏴") return;
            if (mevent.Button == MouseButtons.Left)
                bg = this.BackColor;
        }
    }
}
