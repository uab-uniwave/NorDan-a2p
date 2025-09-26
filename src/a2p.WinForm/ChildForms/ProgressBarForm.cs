// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.DTO;
using a2p.Domain.Entities;
using System.Drawing.Drawing2D;

namespace a2p.WinForm.ChildForms
{
    public partial class ProgressBarForm : Form
    {
        public ProgressBarForm()
        {

            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            PerformAutoScale();
            InitializeComponent();
        }

        private void ProgressBarForm_Load(object? sender, EventArgs e)
        {

            UpdateProgressBar();
            SetRoundedCorners(20);
            PerformAutoScale();

        }

        private void ProgressBarForm_Shown(object? sender, EventArgs e)
        {
            plMainPanel.Visible = true;
            Cursor = Cursors.WaitCursor;
            ResumeLayout(false);
            PerformLayout();
        }
        private void ProgressBarForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            PerformAutoScale();
            ResumeLayout(false);
            PerformLayout();

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
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            if (progressValue.CurrentValue > 0 && progressValue.TotalValue > 0)
            {
                progressValue.Value = (progressValue.CurrentValue) * 100 / progressValue.TotalValue;
            }

            if (progressValue.Value > 100)
            {
                progressBar.Value = 100;
            }
            else { progressBar.Value = (int)progressValue.Value; }

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
            path.AddArc(new Rectangle(Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(Width - radius, Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            Region = new Region(path);
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

        private void ProgressBarForm_FormClosed(object sender, FormClosedEventArgs e) => Cursor = Cursors.Default;
    }
}
