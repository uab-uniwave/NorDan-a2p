
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
            plMainPanel = new Panel();
            lbProgressBarTitle = new Label();
            progressBar = new ProgressBar();
            lbProgressBarTask3 = new Label();
            lbProgressBarTask1 = new Label();
            lbProgressBarTask2 = new Label();
            plMainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Interval = 50;
            timer1.Tick += Timer1_Tick;
            // 
            // plMainPanel
            // 
            plMainPanel.BackColor = Color.Transparent;
            plMainPanel.Controls.Add(lbProgressBarTitle);
            plMainPanel.Controls.Add(progressBar);
            plMainPanel.Controls.Add(lbProgressBarTask3);
            plMainPanel.Controls.Add(lbProgressBarTask1);
            plMainPanel.Controls.Add(lbProgressBarTask2);
            plMainPanel.Cursor = Cursors.UpArrow;
            plMainPanel.Dock = DockStyle.Fill;
            plMainPanel.Font = new Font("Segoe UI", 9F);
            plMainPanel.Location = new Point(0, 0);
            plMainPanel.Margin = new Padding(20, 3, 3, 3);
            plMainPanel.Name = "plMainPanel";
            plMainPanel.Size = new Size(800, 300);
            plMainPanel.TabIndex = 29;
            plMainPanel.UseWaitCursor = true;
            plMainPanel.Visible = false;
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
            lbProgressBarTitle.TabIndex = 46;
            lbProgressBarTitle.Text = "Progress Title";
            lbProgressBarTitle.TextAlign = ContentAlignment.MiddleCenter;
            lbProgressBarTitle.UseWaitCursor = true;
            // 
            // progressBar
            // 
            progressBar.AccessibleRole = AccessibleRole.ProgressBar;
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.BackColor = Color.FromArgb(96, 97, 100);
            progressBar.Cursor = Cursors.WaitCursor;
            progressBar.ForeColor = Color.FromArgb(239, 112, 32);
            progressBar.Location = new Point(32, 78);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(735, 47);
            progressBar.Step = 1;
            progressBar.TabIndex = 45;
            progressBar.UseWaitCursor = true;
            // 
            // lbProgressBarTask3
            // 
            lbProgressBarTask3.BackColor = Color.Transparent;
            lbProgressBarTask3.Dock = DockStyle.Bottom;
            lbProgressBarTask3.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask3.ForeColor = Color.White;
            lbProgressBarTask3.Location = new Point(0, 150);
            lbProgressBarTask3.Margin = new Padding(3);
            lbProgressBarTask3.Name = "lbProgressBarTask3";
            lbProgressBarTask3.Size = new Size(800, 50);
            lbProgressBarTask3.TabIndex = 44;
            lbProgressBarTask3.Text = "Progress Task 3";
            lbProgressBarTask3.TextAlign = ContentAlignment.TopCenter;
            lbProgressBarTask3.UseWaitCursor = true;
            // 
            // lbProgressBarTask1
            // 
            lbProgressBarTask1.BackColor = Color.Transparent;
            lbProgressBarTask1.Dock = DockStyle.Bottom;
            lbProgressBarTask1.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask1.ForeColor = Color.White;
            lbProgressBarTask1.Location = new Point(0, 200);
            lbProgressBarTask1.Margin = new Padding(3);
            lbProgressBarTask1.Name = "lbProgressBarTask1";
            lbProgressBarTask1.Size = new Size(800, 50);
            lbProgressBarTask1.TabIndex = 43;
            lbProgressBarTask1.Text = "Progress Task 1";
            lbProgressBarTask1.TextAlign = ContentAlignment.TopCenter;
            lbProgressBarTask1.UseWaitCursor = true;
            // 
            // lbProgressBarTask2
            // 
            lbProgressBarTask2.BackColor = Color.Transparent;
            lbProgressBarTask2.Dock = DockStyle.Bottom;
            lbProgressBarTask2.FlatStyle = FlatStyle.Flat;
            lbProgressBarTask2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbProgressBarTask2.ForeColor = Color.White;
            lbProgressBarTask2.Location = new Point(0, 250);
            lbProgressBarTask2.Margin = new Padding(20, 3, 3, 3);
            lbProgressBarTask2.Name = "lbProgressBarTask2";
            lbProgressBarTask2.Size = new Size(800, 50);
            lbProgressBarTask2.TabIndex = 40;
            lbProgressBarTask2.Text = "Progress Task 2";
            lbProgressBarTask2.TextAlign = ContentAlignment.TopCenter;
            lbProgressBarTask2.UseWaitCursor = true;
            // 
            // ProgressBarForm
            // 
            AutoSize = true;
            BackColor = Color.FromArgb(239, 112, 32);
            ClientSize = new Size(800, 300);
            ControlBox = false;
            Controls.Add(plMainPanel);
            DoubleBuffered = true;
            ForeColor = Color.Transparent;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "ProgressBarForm";
            Opacity = 0.8D;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ProgressBarForm";
            UseWaitCursor = true;
            FormClosed += ProgressBarForm_FormClosed;
            Load += ProgressBarForm_Load;
            Shown += ProgressBarForm_Shown;
            DpiChanged += ProgressBarForm_DpiChanged;
            plMainPanel.ResumeLayout(false);
            ResumeLayout(false);
        }


        #endregion
        private Panel plTitleBarPanel;
        public System.Windows.Forms.Timer timer1;
        public Label lbProgressBarTask3;
        public Label lbProgressBarTask1;
        public Label lbProgressBarTask2;
        private Panel plProgressBarPanel;
        public ProgressBar progressBar;
        public Panel plMainPanel;
        public Label lbProgressBarTitle;
    }
}