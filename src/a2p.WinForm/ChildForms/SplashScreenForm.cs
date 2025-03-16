using System.Drawing.Drawing2D;

namespace a2p.WinForm
{
    public partial class SplashScreenForm : Form
    {
        public SplashScreenForm()
        {


            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent();
            this.Opacity = 0; // Start fully transparent
        }

        #region -== Form Events ==-
        private void SplashScreenForm_Load(object? sender, EventArgs e)
        {

            SetRoundedCorners(50);
            this.PerformAutoScale();

        }
        private void SplashScreenForm_Shown(object sender, EventArgs e)
        {
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private void SplashScreenForm_DpiChanged(object? sender, DpiChangedEventArgs e)
        {


            this.PerformAutoScale();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion  -== Form Events ==-

        #region -== Methods ==-
        public void FadeIn()
        {
            for (double i = 0; i <= 1; i += 0.1)
            {
                this.Opacity = i;
                Task.Delay(50).Wait();
            }
        }
        public void FadeOut()
        {

            for (double i = 1; i >= 0; i -= 0.1)
            {
                this.Opacity = i;
                Task.Delay(50).Wait();
            }
        }
        private void SetRoundedCorners(int radius)
        {
            GraphicsPath path = new();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
        }
        protected override void WndProc(ref Message m)
        {
            {
                base.WndProc(ref m);

            }

        }
        #endregion -== Methods ==-

    }
}
