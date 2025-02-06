using a2p.Shared.Core.Entities.Models;

namespace a2p.WinForm.CustomControls
{
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {




            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent();
            this.PerformAutoScale(); // Ensure everything is scaled correctly (optional)
            this.ResumeLayout(true); //                this.DpiChanged+=SplashScreenForm_DpiChanged;
                                     // Attach events before setting DPI to catch any initial changes



            // Set DPI mode and dimensions after attaching events


            this.ResumeLayout(true);
        }

        public void UpdateProgress(ProgressValue progressValue)
        {
            if (IsHandleCreated && !IsDisposed)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => UpdateProgressInternal(progressValue)));
                }
                else
                {
                    UpdateProgressInternal(progressValue);
                }
            }
        }

        private void UpdateProgressInternal(ProgressValue progressValue)
        {
            progressBar.Minimum = progressValue.MinValue;
            progressBar.Maximum = progressValue.MaxValue;
            progressBar.Value = progressValue.Value;

            lbProgressBarTitle.Text = progressValue.ProgressTitle ?? string.Empty;
            lbProgressBarTask1.Text = progressValue.ProgressTask1 ?? string.Empty;
            lbProgressBarTask2.Text = progressValue.ProgressTask2 ?? string.Empty;
            lbProgressBarTask3.Text = progressValue.ProgressTask3 ?? string.Empty;
        }


        private void ProgressBarForm_Load(object? sender, EventArgs e)
        {

            UpdateProgressBar();
            SuspendLayout();

        }


        private void UpdateProgressBar()
        {
            // Example update logic
            lbProgressBarTitle.Text = "Loading OrderFiles ...";
            lbProgressBarTask1.Text = "";
            lbProgressBarTask2.Text = "";
            lbProgressBarTask3.Text = "";
            progressBar.Value = 50; // Example progress value
        }

        private void ProgressBarForm_Shown(object? sender, EventArgs e)
        {
            ResumeLayout(false);

        }
        protected override void WndProc(ref Message m)
        {

            const int WM_DPICHANGED = 0x02E0;

            if (m.Msg == WM_DPICHANGED)
            {
                int newDpi = m.WParam.ToInt32() & 0xFFFF; // Extract DPI value
                float scaleFactor = newDpi / 96f;

                // Resize form properly based on new DPI
                this.Scale(new SizeF(scaleFactor, scaleFactor));
            }
            base.WndProc(ref m);

        }




        private void ProgressBarForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();

        }
    }
}