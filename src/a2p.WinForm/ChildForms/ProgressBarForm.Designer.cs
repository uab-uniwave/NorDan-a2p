namespace a2p.WinForm.CustomControls
{
    partial class ProgressBarForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.plProgressBarPanel = new Panel();
            this.lbProgressBarTask3 = new Label();
            this.lbProgressBarTask2 = new Label();
            this.lbProgressBarTask1 = new Label();
            this.lbProgressBarTitle = new Label();
            this.progressBar = new ProgressBar();

            this.plProgressBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // plProgressBarPanel
            // 
            this.plProgressBarPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.plProgressBarPanel.Controls.Add(this.progressBar);
            this.plProgressBarPanel.Location = new Point(0, 65);
            this.plProgressBarPanel.Name = "plProgressBarPanel";
            this.plProgressBarPanel.Size = new Size(400, 60);
            this.plProgressBarPanel.TabIndex = 27;
            // 
            // lbProgressBarTask3
            // 
            this.lbProgressBarTask3.BackColor = UniwaveColors.a2pGreyDeep;
            this.lbProgressBarTask3.Dock = DockStyle.Bottom;
            this.lbProgressBarTask3.FlatStyle = FlatStyle.Flat;
            this.lbProgressBarTask3.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            this.lbProgressBarTask3.ForeColor = UniwaveColors.uwOrangeDeep;
            this.lbProgressBarTask3.Location = new Point(0, 175);
            this.lbProgressBarTask3.Margin = new Padding(6, 0, 6, 0);
            this.lbProgressBarTask3.Name = "lbProgressBarTask3";
            this.lbProgressBarTask3.Size = new Size(400, 25);
            this.lbProgressBarTask3.TabIndex = 18;
            this.lbProgressBarTask3.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbProgressBarTask2
            // 
            this.lbProgressBarTask2.BackColor = UniwaveColors.a2pGreyDeep;
            this.lbProgressBarTask2.Dock = DockStyle.Bottom;
            this.lbProgressBarTask2.FlatStyle = FlatStyle.Flat;
            this.lbProgressBarTask2.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            this.lbProgressBarTask2.ForeColor = UniwaveColors.uwOrangeDeep;
            this.lbProgressBarTask2.Location = new Point(0, 150);
            this.lbProgressBarTask2.Margin = new Padding(6, 0, 6, 0);
            this.lbProgressBarTask2.Name = "lbProgressBarTask2";
            this.lbProgressBarTask2.Size = new Size(400, 25);
            this.lbProgressBarTask2.TabIndex = 19;
            this.lbProgressBarTask2.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbProgressBarTask1
            // 
            this.lbProgressBarTask1.BackColor = UniwaveColors.a2pGreyDeep;
            this.lbProgressBarTask1.Dock = DockStyle.Bottom;
            this.lbProgressBarTask1.FlatStyle = FlatStyle.Flat;
            this.lbProgressBarTask1.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            this.lbProgressBarTask1.ForeColor = UniwaveColors.uwOrangeDeep;
            this.lbProgressBarTask1.Location = new Point(0, 125);
            this.lbProgressBarTask1.Margin = new Padding(6, 0, 6, 0);
            this.lbProgressBarTask1.Name = "lbProgressBarTask1";
            this.lbProgressBarTask1.Size = new Size(400, 25);
            this.lbProgressBarTask1.TabIndex = 25;
            this.lbProgressBarTask1.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbProgressBarTitle
            // 
            this.lbProgressBarTitle.AccessibleName = "";
            this.lbProgressBarTitle.BackColor = Color.Transparent;
            this.lbProgressBarTitle.Cursor = Cursors.WaitCursor;
            this.lbProgressBarTitle.Dock = DockStyle.Top;
            this.lbProgressBarTitle.FlatStyle = FlatStyle.Flat;
            this.lbProgressBarTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbProgressBarTitle.ForeColor = UniwaveColors.uwOrangeDeep;
            this.lbProgressBarTitle.Location = new Point(0, 0);
            this.lbProgressBarTitle.Margin = new Padding(6, 0, 6, 0);
            this.lbProgressBarTitle.Name = "lbProgressBarTitle";
            this.lbProgressBarTitle.Size = new Size(400, 65);
            this.lbProgressBarTitle.TabIndex = 28;
            this.lbProgressBarTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.progressBar.ForeColor = UniwaveColors.uwOrangeDeep;
            this.progressBar.Location = new Point(44, 18);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(300, 23);
            this.progressBar.TabIndex = 30;
            // 
            // ProgressBarForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UniwaveColors.a2pGreyDeep;
            ClientSize = new Size(400, 200);
            Controls.Add(this.lbProgressBarTitle);
            Controls.Add(this.plProgressBarPanel);
            Controls.Add(this.lbProgressBarTask1);
            Controls.Add(this.lbProgressBarTask2);
            Controls.Add(this.lbProgressBarTask3);
            DoubleBuffered = true;
            ForeColor = UniwaveColors.uwOrangeDeep;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "ProgressBarForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ProgressBarForm";
            Load += ProgressBarForm_Load;
            Shown += ProgressBarForm_Shown;
            this.plProgressBarPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Panel plProgressBarPanel;
        private Label lbProgressBarTask3;
        private Label lbProgressBarTask2;
        private Label lbProgressBarTask1;
        private Label lbProgressBarTitle;
        private ProgressBar progressBar;
    }
}