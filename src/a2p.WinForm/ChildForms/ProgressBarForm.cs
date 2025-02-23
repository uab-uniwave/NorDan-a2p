using System.Drawing.Drawing2D;

using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.WinForm.CustomControls
{
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {

            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.PerformAutoScale();
            InitializeComponent();
        }

        private void ProgressBarForm_Load(object? sender, EventArgs e)
        {

            UpdateProgressBar();
            SetRoundedCorners(20);
            this.PerformAutoScale();

        }

        private void ProgressBarForm_Shown(object? sender, EventArgs e)
        {
            plMainPanel.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private void ProgressBarForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #region -== Form Evenets ==-

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
            plMainPanel.ResumeLayout(false);
            plMainPanel.PerformLayout();
        }
        #endregion -== Form Evenets ==-

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

        public Task FormReadyAsync()
        {
            TaskCompletionSource tcs = new();
            this.Shown += (s, e) => tcs.SetResult();
            return tcs.Task;
        }

        private void UpdateProgressBar()
        {

            // Example update logic
            lbProgressBarTitle.Text = "Initializing ...";
            lbProgressBarTask1.Text = "";
            lbProgressBarTask2.Text = "";
            lbProgressBarTask3.Text = "";
            progressBar.Value = 0; // Example progress value
            progressBar.Maximum = 100; // Example max value
            progressBar.Minimum = 0;

        }

        private void ProgressBarForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
    }
}