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
            plTitleBarPanel = new Panel();
            lbProgressBarTitle = new Label();
            progressBar = new ProgressBar();
            plProgressBarPanel = new Panel();
            lbProgressBarTask3 = new Label();
            lbProgressBarTask2 = new Label();
            lbProgressBarTask1 = new Label();
            plTaskLabelsPanel = new Panel();
            plTitleBarPanel.SuspendLayout();
            plProgressBarPanel.SuspendLayout();
            plTaskLabelsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // plTitleBarPanel
            // 
            plTitleBarPanel.Controls.Add(lbProgressBarTitle);
            plTitleBarPanel.Dock = DockStyle.Top;
            plTitleBarPanel.Location = new Point(0, 0);
            plTitleBarPanel.Name = "plTitleBarPanel";
            plTitleBarPanel.Size = new Size(800, 50);
            plTitleBarPanel.TabIndex = 30;
            // 
            // lbProgressBarTitle
            // 
            lbProgressBarTitle.BackColor = Color.FromArgb(122, 123, 124);
            lbProgressBarTitle.Dock = DockStyle.Bottom;
            lbProgressBarTitle.FlatStyle = FlatStyle.Flat;
            lbProgressBarTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTitle.ForeColor = Color.White;
            lbProgressBarTitle.Location = new Point(0, 0);
            lbProgressBarTitle.Margin = new Padding(3);
            lbProgressBarTitle.Name = "lbProgressBarTitle";
            lbProgressBarTitle.Size = new Size(800, 50);
            lbProgressBarTitle.TabIndex = 29;
            lbProgressBarTitle.Text = "Progress Title";
            lbProgressBarTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.ForeColor = Color.FromArgb(239, 112, 32);
            progressBar.Location = new Point(25, 25);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(652, 50);
            progressBar.Step = 1;
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 30;
            // 
            // plProgressBarPanel
            // 
            plProgressBarPanel.BackColor = Color.FromArgb(239, 112, 32);
            plProgressBarPanel.BackgroundImageLayout = ImageLayout.None;
            plProgressBarPanel.Controls.Add(progressBar);
            plProgressBarPanel.ForeColor = Color.White;
            plProgressBarPanel.Location = new Point(50, 50);
            plProgressBarPanel.Margin = new Padding(20, 3, 3, 3);
            plProgressBarPanel.Name = "plProgressBarPanel";
            plProgressBarPanel.Size = new Size(700, 100);
            plProgressBarPanel.TabIndex = 31;
            // 
            // lbProgressBarTask3
            // 
            lbProgressBarTask3.BackColor = Color.Transparent;
            lbProgressBarTask3.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask3.ForeColor = Color.White;
            lbProgressBarTask3.Location = new Point(50, 100);
            lbProgressBarTask3.Margin = new Padding(3);
            lbProgressBarTask3.Name = "lbProgressBarTask3";
            lbProgressBarTask3.Size = new Size(700, 50);
            lbProgressBarTask3.TabIndex = 26;
            lbProgressBarTask3.Text = "Progress Task 3";
            lbProgressBarTask3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbProgressBarTask2
            // 
            lbProgressBarTask2.BackColor = Color.Transparent;
            lbProgressBarTask2.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask2.ForeColor = Color.White;
            lbProgressBarTask2.Location = new Point(50, 50);
            lbProgressBarTask2.Margin = new Padding(20, 3, 3, 3);
            lbProgressBarTask2.Name = "lbProgressBarTask2";
            lbProgressBarTask2.Size = new Size(700, 50);
            lbProgressBarTask2.TabIndex = 27;
            lbProgressBarTask2.Text = "Progress Task 2";
            lbProgressBarTask2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbProgressBarTask1
            // 
            lbProgressBarTask1.BackColor = Color.Transparent;
            lbProgressBarTask1.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask1.ForeColor = Color.White;
            lbProgressBarTask1.Location = new Point(50, 0);
            lbProgressBarTask1.Margin = new Padding(3);
            lbProgressBarTask1.Name = "lbProgressBarTask1";
            lbProgressBarTask1.Size = new Size(700, 50);
            lbProgressBarTask1.TabIndex = 28;
            lbProgressBarTask1.Text = "Progress Task 1";
            lbProgressBarTask1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // plTaskLabelsPanel
            // 
            plTaskLabelsPanel.BackColor = Color.Transparent;
            plTaskLabelsPanel.Controls.Add(lbProgressBarTask1);
            plTaskLabelsPanel.Controls.Add(lbProgressBarTask2);
            plTaskLabelsPanel.Controls.Add(lbProgressBarTask3);
            plTaskLabelsPanel.Cursor = Cursors.UpArrow;
            plTaskLabelsPanel.Dock = DockStyle.Bottom;
            plTaskLabelsPanel.Font = new Font("Segoe UI", 9F);
            plTaskLabelsPanel.Location = new Point(0, 150);
            plTaskLabelsPanel.Margin = new Padding(20, 3, 3, 3);
            plTaskLabelsPanel.Name = "plTaskLabelsPanel";
            plTaskLabelsPanel.Size = new Size(800, 150);
            plTaskLabelsPanel.TabIndex = 29;
            // 
            // ProgressBarForm
            // 
            BackColor = Color.FromArgb(239, 112, 32);
            ClientSize = new Size(800, 300);
            Controls.Add(plProgressBarPanel);
            Controls.Add(plTitleBarPanel);
            Controls.Add(plTaskLabelsPanel);
            DoubleBuffered = true;
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "ProgressBarForm";
            Opacity = 0.8D;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ProgressBarForm";
            Load += ProgressBarForm_Load;
            Shown += ProgressBarForm_Shown;
            DpiChanged += ProgressBarForm_DpiChanged;
            plTitleBarPanel.ResumeLayout(false);
            plProgressBarPanel.ResumeLayout(false);
            plTaskLabelsPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel plTitleBarPanel;
        private Label lbProgressBarTitle;
        private ProgressBar progressBar;
        private Panel plProgressBarPanel;
        private Label lbProgressBarTask3;
        private Label lbProgressBarTask2;
        private Label lbProgressBarTask1;
        private Panel plTaskLabelsPanel;
    }
}