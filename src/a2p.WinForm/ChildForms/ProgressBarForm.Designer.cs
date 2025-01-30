namespace a2p.WinForm.CustomControls
{
 partial class ProgressBarForm
 {
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
   if (disposing && (components != null))
   {
    components.Dispose();
   }
   base.Dispose(disposing);
  }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            lbProgressBarTask3 = new Label();
            lbProgressBarTask2 = new Label();
            lbProgressBarTask1 = new Label();
            lbProgressBarTitle = new Label();
            progressBar = new ProgressBar();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(progressBar);
            panel1.Location = new Point(0, 65);
            panel1.Name = "panel1";
            panel1.Size = new Size(400, 60);
            panel1.TabIndex = 27;
            // 
            // lbProgressBarTask3
            // 
            lbProgressBarTask3.BackColor = UniwaveColors.uwGreyDark;
            lbProgressBarTask3.Dock = DockStyle.Bottom;
            lbProgressBarTask3.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask3.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lbProgressBarTask3.ForeColor = Color.FromArgb(239, 112, 32);
            lbProgressBarTask3.Location = new Point(0, 175);
            lbProgressBarTask3.Margin = new Padding(6, 0, 6, 0);
            lbProgressBarTask3.Name = "lbProgressBarTask3";
            lbProgressBarTask3.Size = new Size(400, 25);
            lbProgressBarTask3.TabIndex = 18;
            lbProgressBarTask3.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbProgressBarTask2
            // 
            lbProgressBarTask2.BackColor = Color.FromArgb(88, 89, 81);
            lbProgressBarTask2.Dock = DockStyle.Bottom;
            lbProgressBarTask2.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask2.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lbProgressBarTask2.ForeColor = Color.FromArgb(239, 112, 32);
            lbProgressBarTask2.Location = new Point(0, 150);
            lbProgressBarTask2.Margin = new Padding(6, 0, 6, 0);
            lbProgressBarTask2.Name = "lbProgressBarTask2";
            lbProgressBarTask2.Size = new Size(400, 25);
            lbProgressBarTask2.TabIndex = 19;
            lbProgressBarTask2.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbProgressBarTask1
            // 
            lbProgressBarTask1.BackColor = Color.FromArgb(88, 89, 81);
            lbProgressBarTask1.Dock = DockStyle.Bottom;
            lbProgressBarTask1.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask1.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lbProgressBarTask1.ForeColor = Color.FromArgb(239, 112, 32);
            lbProgressBarTask1.Location = new Point(0, 125);
            lbProgressBarTask1.Margin = new Padding(6, 0, 6, 0);
            lbProgressBarTask1.Name = "lbProgressBarTask1";
            lbProgressBarTask1.Size = new Size(400, 25);
            lbProgressBarTask1.TabIndex = 25;
            lbProgressBarTask1.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbProgressBarTitle
            // 
            lbProgressBarTitle.AccessibleName = "";
            lbProgressBarTitle.BackColor = Color.Transparent;
            lbProgressBarTitle.Cursor = Cursors.WaitCursor;
            lbProgressBarTitle.Dock = DockStyle.Top;
            lbProgressBarTitle.FlatStyle = FlatStyle.Flat;
            lbProgressBarTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTitle.ForeColor = Color.FromArgb(239, 112, 32);
            lbProgressBarTitle.Location = new Point(0, 0);
            lbProgressBarTitle.Margin = new Padding(6, 0, 6, 0);
            lbProgressBarTitle.Name = "lbProgressBarTitle";
            lbProgressBarTitle.Size = new Size(400, 65);
            lbProgressBarTitle.TabIndex = 28;
            lbProgressBarTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.ForeColor = Color.FromArgb(239, 112, 32);
            progressBar.Location = new Point(44, 18);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(300, 23);
            progressBar.TabIndex = 30;
            // 
            // ProgressBarForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(88, 89, 81);
            ClientSize = new Size(400, 200);
            Controls.Add(lbProgressBarTitle);
            Controls.Add(panel1);
            Controls.Add(lbProgressBarTask1);
            Controls.Add(lbProgressBarTask2);
            Controls.Add(lbProgressBarTask3);
            DoubleBuffered = true;
            ForeColor = Color.FromArgb(239, 112, 32);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "ProgressBarForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ProgressBarForm";
            Load += ProgressBarForm_Load;
            DpiChanged += ProgressBarForm_DpiChanged;
            panel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private Panel panel1;
        private Label lbProgressBarTask3;
        private Label lbProgressBarTask2;
        private Label lbProgressBarTask1;
        private Label lbProgressBarTitle;
        private ProgressBar progressBar;
    }
}