
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
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            plTitleBarPanel = new Panel();
            lbProgressBarTitle = new Label();
            plTaskLabelsPanel = new Panel();
            lbProgressBarTask3 = new Label();
            lbProgressBarTask2 = new Label();
            lbProgressBarTask1 = new Label();
            plProgressBarPanel = new Panel();
            progressBar = new ProgressBar();
            plTitleBarPanel.SuspendLayout();
            plTaskLabelsPanel.SuspendLayout();
            plProgressBarPanel.SuspendLayout();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Interval = 50;
            timer1.Tick += Timer1_Tick;
            // 
            // plTitleBarPanel
            // 
            plTitleBarPanel.BackColor = Color.Transparent;
            plTitleBarPanel.Controls.Add(lbProgressBarTitle);
            plTitleBarPanel.Dock = DockStyle.Top;
            plTitleBarPanel.ForeColor = Color.Transparent;
            plTitleBarPanel.Location = new Point(0, 0);
            plTitleBarPanel.Name = "plTitleBarPanel";
            plTitleBarPanel.Size = new Size(800, 50);
            plTitleBarPanel.TabIndex = 30;
            // 
            // lbProgressBarTitle
            // 
            lbProgressBarTitle.BackColor = Color.FromArgb(96, 97, 100);
            lbProgressBarTitle.Dock = DockStyle.Top;
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
            // plTaskLabelsPanel
            // 
            plTaskLabelsPanel.BackColor = Color.Transparent;
            plTaskLabelsPanel.Controls.Add(lbProgressBarTask3);
            plTaskLabelsPanel.Controls.Add(lbProgressBarTask2);
            plTaskLabelsPanel.Controls.Add(lbProgressBarTask1);
            plTaskLabelsPanel.Cursor = Cursors.UpArrow;
            plTaskLabelsPanel.Dock = DockStyle.Bottom;
            plTaskLabelsPanel.Font = new Font("Segoe UI", 9F);
            plTaskLabelsPanel.Location = new Point(0, 150);
            plTaskLabelsPanel.Margin = new Padding(20, 3, 3, 3);
            plTaskLabelsPanel.Name = "plTaskLabelsPanel";
            plTaskLabelsPanel.Size = new Size(800, 150);
            plTaskLabelsPanel.TabIndex = 29;
            // 
            // lbProgressBarTask3
            // 
            lbProgressBarTask3.BackColor = Color.Transparent;
            lbProgressBarTask3.Dock = DockStyle.Top;
            lbProgressBarTask3.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask3.ForeColor = Color.White;
            lbProgressBarTask3.Location = new Point(0, 100);
            lbProgressBarTask3.Margin = new Padding(3);
            lbProgressBarTask3.Name = "lbProgressBarTask3";
            lbProgressBarTask3.Size = new Size(800, 50);
            lbProgressBarTask3.TabIndex = 37;
            lbProgressBarTask3.Text = "Progress Task 3";
            lbProgressBarTask3.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbProgressBarTask2
            // 
            lbProgressBarTask2.BackColor = Color.Transparent;
            lbProgressBarTask2.Dock = DockStyle.Top;
            lbProgressBarTask2.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask2.ForeColor = Color.White;
            lbProgressBarTask2.Location = new Point(0, 50);
            lbProgressBarTask2.Margin = new Padding(20, 3, 3, 3);
            lbProgressBarTask2.Name = "lbProgressBarTask2";
            lbProgressBarTask2.Size = new Size(800, 50);
            lbProgressBarTask2.TabIndex = 36;
            lbProgressBarTask2.Text = "Progress Task 2";
            lbProgressBarTask2.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbProgressBarTask1
            // 
            lbProgressBarTask1.BackColor = Color.Transparent;
            lbProgressBarTask1.Dock = DockStyle.Top;
            lbProgressBarTask1.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask1.ForeColor = Color.White;
            lbProgressBarTask1.Location = new Point(0, 0);
            lbProgressBarTask1.Margin = new Padding(3);
            lbProgressBarTask1.Name = "lbProgressBarTask1";
            lbProgressBarTask1.Size = new Size(800, 50);
            lbProgressBarTask1.TabIndex = 35;
            lbProgressBarTask1.Text = "Progress Task 1";
            lbProgressBarTask1.TextAlign = ContentAlignment.TopCenter;
            // 
            // plProgressBarPanel
            // 
            plProgressBarPanel.BackColor = Color.Transparent;
            plProgressBarPanel.BackgroundImageLayout = ImageLayout.None;
            plProgressBarPanel.Controls.Add(progressBar);
            plProgressBarPanel.Dock = DockStyle.Top;
            plProgressBarPanel.ForeColor = Color.White;
            plProgressBarPanel.Location = new Point(0, 50);
            plProgressBarPanel.Margin = new Padding(20, 3, 3, 3);
            plProgressBarPanel.Name = "plProgressBarPanel";
            plProgressBarPanel.Size = new Size(800, 100);
            plProgressBarPanel.TabIndex = 33;
            // 
            // progressBar
            // 
            progressBar.AccessibleRole = AccessibleRole.ProgressBar;
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.BackColor = Color.FromArgb(96, 97, 100);
            progressBar.Cursor = Cursors.WaitCursor;
            progressBar.ForeColor = Color.FromArgb(239, 112, 32);
            progressBar.Location = new Point(25, 25);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(750, 50);
            progressBar.Step = 1;
            progressBar.TabIndex = 30;
            // 
            // ProgressBarForm
            // 
            AutoSize = true;
            BackColor = Color.FromArgb(239, 112, 32);
            ClientSize = new Size(800, 300);
            ControlBox = false;
            Controls.Add(plProgressBarPanel);
            Controls.Add(plTitleBarPanel);
            Controls.Add(plTaskLabelsPanel);
            DoubleBuffered = true;
            ForeColor = Color.Transparent;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "ProgressBarForm";
            Opacity = 0.8D;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ProgressBarForm";
            TopMost = true;
            Load += ProgressBarForm_Load;
            Shown += ProgressBarForm_Shown;
            DpiChanged += ProgressBarForm_DpiChanged;
            plTitleBarPanel.ResumeLayout(false);
            plTaskLabelsPanel.ResumeLayout(false);
            plProgressBarPanel.ResumeLayout(false);
            ResumeLayout(false);
        }


        #endregion
        private Panel plTitleBarPanel;
        private Panel plTaskLabelsPanel;
        public Label lbProgressBarTitle;
        public Label lbProgressBarTask3;
        public Label lbProgressBarTask2;
        public Label lbProgressBarTask1;
        private Panel plProgressBarPanel;
        public ProgressBar progressBar;
        public System.Windows.Forms.Timer timer1;

    }
}