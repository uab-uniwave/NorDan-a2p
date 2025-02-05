using System.Drawing.Drawing2D;

namespace a2p.WinForm
{
    public partial class SplashScreenForm : Form
    {
        public SplashScreenForm()
        {

            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            //    this.AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent(); // Initialize components first

            // Attach events before setting DPI to catch any initial changes
            this.DpiChanged += SplashScreenForm_DpiChanged;
            this.Load += SplashScreenForm_Load;

            // Set DPI mode and dimensions after attaching events


            this.ResumeLayout(true);
            this.Opacity = 0; // Start fully transparent
        }


        private void SplashScreenForm_DpiChanged(object? sender, DpiChangedEventArgs e)
        {



            this.PerformAutoScale();

        }

        private void SplashScreenForm_Load(object? sender, EventArgs e)
        {

            SetRoundedCorners(10);

        }
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
                const int WM_DPICHANGED = 0x02E0;

                if (m.Msg == WM_DPICHANGED)
                {
                    int newDpi = m.WParam.ToInt32() & 0xFFFF; // Extract DPI value
                    float scaleFactor = newDpi / 192f;

                    // Resize form properly based on new DPI
                    this.Scale(new SizeF(scaleFactor, scaleFactor));
                }
                base.WndProc(ref m);

            }

        }
    }
}