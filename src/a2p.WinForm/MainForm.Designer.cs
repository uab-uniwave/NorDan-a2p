using a2p.WinForm;
using a2p.WinForm.Properties;

using System.Windows.Forms;
using System.Windows;
using System.Drawing;


namespace a2p.WinForm
    {
    partial class MainForm
    {
        /// <summary>
        /// RequiredQuantity designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing&&(components!=null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// RequiredQuantity method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tplHeader = new TableLayoutPanel();
            plNordanHeaderLogo = new Panel();
            lbHeader1 = new Label();
            lbHeader2 = new Label();
            lbHeader3 = new Label();
            lbHeader4 = new Label();
            plTBPanel = new Panel();
            miniToolStrip = new StatusStrip();
            statusStrip = new StatusStrip();
            slbPathTitle = new ToolStripStatusLabel();
            slbPathValue = new ToolStripStatusLabel();
            slbDataSourceTitle = new ToolStripStatusLabel();
            slbDataSourceValue = new ToolStripStatusLabel();
            plSideBarMain = new Panel();
            btnProperties = new Button();
            btnExit = new Button();
            btnLog = new Button();
            btnImport = new Button();
            btnLoad = new Button();
            plFormContainer = new Panel();
            tplHeader.SuspendLayout();
            statusStrip.SuspendLayout();
            plSideBarMain.SuspendLayout();
            SuspendLayout();
            // 
            // tplHeader
            // 
            tplHeader.AutoSize = true;
            tplHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tplHeader.BackColor = Color.FromArgb(239, 112, 32);
            tplHeader.ColumnCount = 6;
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tplHeader.Controls.Add(plNordanHeaderLogo, 5, 0);
            tplHeader.Controls.Add(lbHeader1, 0, 0);
            tplHeader.Controls.Add(lbHeader2, 1, 0);
            tplHeader.Controls.Add(lbHeader3, 2, 0);
            tplHeader.Controls.Add(lbHeader4, 4, 0);
            tplHeader.Dock = DockStyle.Top;
            tplHeader.Location = new Point(0, 0);
            tplHeader.Margin = new Padding(2);
            tplHeader.Name = "tplHeader";
            tplHeader.Padding = new Padding(2);
            tplHeader.RowCount = 1;
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 11F));
            tplHeader.Size = new Size(970, 64);
            tplHeader.TabIndex = 1;
            // 
            // plNordanHeaderLogo
            // 
            plNordanHeaderLogo.BackgroundImage = Resources.NordanLogoInversed;
            plNordanHeaderLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plNordanHeaderLogo.Dock = DockStyle.Fill;
            plNordanHeaderLogo.Location = new Point(718, 2);
            plNordanHeaderLogo.Margin = new Padding(0);
            plNordanHeaderLogo.Name = "plNordanHeaderLogo";
            tplHeader.SetRowSpan(plNordanHeaderLogo, 3);
            plNordanHeaderLogo.Size = new Size(250, 60);
            plNordanHeaderLogo.TabIndex = 11;
            // 
            // lbHeader1
            // 
            lbHeader1.AutoSize = true;
            lbHeader1.BackColor = Color.Transparent;
            lbHeader1.Dock = DockStyle.Fill;
            lbHeader1.Font = new Font("Segoe UI", 20F);
            lbHeader1.ForeColor = Color.FromArgb(248, 248, 249);
            lbHeader1.ImageAlign = ContentAlignment.TopRight;
            lbHeader1.Location = new Point(5, 5);
            lbHeader1.Margin = new Padding(3);
            lbHeader1.Name = "lbHeader1";
            tplHeader.SetRowSpan(lbHeader1, 2);
            lbHeader1.Size = new Size(134, 43);
            lbHeader1.TabIndex = 6;
            lbHeader1.Text = "Aluminum";
            lbHeader1.TextAlign = ContentAlignment.MiddleRight;
            lbHeader1.UseCompatibleTextRendering = true;
            // 
            // lbHeader2
            // 
            lbHeader2.BackColor = Color.Transparent;
            lbHeader2.Dock = DockStyle.Fill;
            lbHeader2.FlatStyle = FlatStyle.Flat;
            lbHeader2.Font = new Font("Segoe UI Black", 30F, FontStyle.Bold);
            lbHeader2.ForeColor = Color.FromArgb(248, 248, 249);
            lbHeader2.Location = new Point(145, 5);
            lbHeader2.Margin = new Padding(3);
            lbHeader2.Name = "lbHeader2";
            tplHeader.SetRowSpan(lbHeader2, 2);
            lbHeader2.Size = new Size(24, 43);
            lbHeader2.TabIndex = 7;
            lbHeader2.Text = "2";
            lbHeader2.TextAlign = ContentAlignment.MiddleCenter;
            lbHeader2.UseCompatibleTextRendering = true;
            // 
            // lbHeader3
            // 
            lbHeader3.AutoSize = true;
            lbHeader3.BackColor = Color.Transparent;
            lbHeader3.Dock = DockStyle.Bottom;
            lbHeader3.Font = new Font("Segoe UI", 16.125F, FontStyle.Bold);
            lbHeader3.ForeColor = Color.FromArgb(248, 248, 249);
            lbHeader3.ImageAlign = ContentAlignment.TopLeft;
            lbHeader3.Location = new Point(175, 24);
            lbHeader3.Margin = new Padding(3);
            lbHeader3.Name = "lbHeader3";
            tplHeader.SetRowSpan(lbHeader3, 3);
            lbHeader3.Size = new Size(103, 35);
            lbHeader3.TabIndex = 4;
            lbHeader3.Text = "PrefSuite";
            lbHeader3.UseCompatibleTextRendering = true;
            // 
            // lbHeader4
            // 
            lbHeader4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lbHeader4.AutoSize = true;
            lbHeader4.BackColor = Color.Transparent;
            lbHeader4.Font = new Font("Segoe UI", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 10, true);
            lbHeader4.ForeColor = Color.Transparent;
            lbHeader4.ImageAlign = ContentAlignment.TopLeft;
            lbHeader4.Location = new Point(284, 5);
            lbHeader4.Margin = new Padding(3);
            lbHeader4.Name = "lbHeader4";
            lbHeader4.Size = new Size(32, 22);
            lbHeader4.TabIndex = 5;
            lbHeader4.Text = "v2.0";
            lbHeader4.TextAlign = ContentAlignment.BottomLeft;
            lbHeader4.UseCompatibleTextRendering = true;
            // 
            // plTBPanel
            // 
            plTBPanel.AutoSize = true;
            plTBPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            plTBPanel.BackColor = Color.FromArgb(56, 57, 60);
            plTBPanel.Dock = DockStyle.Top;
            plTBPanel.ForeColor = Color.FromArgb(56, 57, 60);
            plTBPanel.Location = new Point(0, 0);
            plTBPanel.Name = "plTBPanel";
            plTBPanel.Size = new Size(970, 0);
            plTBPanel.TabIndex = 8;
            // 
            // miniToolStrip
            // 
            miniToolStrip.AccessibleName = "New item selection";
            miniToolStrip.AccessibleRole = AccessibleRole.ButtonDropDown;
            miniToolStrip.AutoSize = false;
            miniToolStrip.BackColor = Color.FromArgb(239, 112, 32);
            miniToolStrip.Dock = DockStyle.None;
            miniToolStrip.ImageScalingSize = new Size(32, 32);
            miniToolStrip.Location = new Point(254, 4);
            miniToolStrip.Name = "miniToolStrip";
            miniToolStrip.Padding = new Padding(1, 0, 8, 0);
            miniToolStrip.Size = new Size(970, 27);
            miniToolStrip.TabIndex = 61;
            // 
            // statusStrip
            // 
            statusStrip.AutoSize = false;
            statusStrip.BackColor = Color.FromArgb(239, 112, 32);
            statusStrip.GripStyle = ToolStripGripStyle.Visible;
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Items.AddRange(new ToolStripItem[] { slbPathTitle, slbPathValue, slbDataSourceTitle, slbDataSourceValue });
            statusStrip.Location = new Point(0, 523);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 8, 0);
            statusStrip.Size = new Size(970, 27);
            statusStrip.TabIndex = 62;
            statusStrip.Text = "statusStrip";
            // 
            // slbPathTitle
            // 
            slbPathTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            slbPathTitle.ForeColor = Color.White;
            slbPathTitle.Margin = new Padding(6, 6, 0, 6);
            slbPathTitle.MergeIndex = 1;
            slbPathTitle.Name = "slbPathTitle";
            slbPathTitle.Size = new Size(57, 15);
            slbPathTitle.Text = "FilePath: ";
            slbPathTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // slbPathValue
            // 
            slbPathValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            slbPathValue.ForeColor = Color.White;
            slbPathValue.Margin = new Padding(0, 6, 6, 6);
            slbPathValue.MergeIndex = 1;
            slbPathValue.Name = "slbPathValue";
            slbPathValue.Size = new Size(52, 15);
            slbPathValue.Text = "c:\\Temp";
            slbPathValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // slbDataSourceTitle
            // 
            slbDataSourceTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            slbDataSourceTitle.ForeColor = Color.White;
            slbDataSourceTitle.Margin = new Padding(6, 6, 0, 6);
            slbDataSourceTitle.MergeIndex = 2;
            slbDataSourceTitle.Name = "slbDataSourceTitle";
            slbDataSourceTitle.Size = new Size(76, 15);
            slbDataSourceTitle.Text = "Data source:";
            slbDataSourceTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // slbDataSourceValue
            // 
            slbDataSourceValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            slbDataSourceValue.ForeColor = Color.White;
            slbDataSourceValue.Margin = new Padding(0, 6, 6, 6);
            slbDataSourceValue.MergeIndex = 2;
            slbDataSourceValue.Name = "slbDataSourceValue";
            slbDataSourceValue.Size = new Size(44, 15);
            slbDataSourceValue.Text = "source";
            slbDataSourceValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // plSideBarMain
            // 
            plSideBarMain.BackColor = Color.Transparent;
            plSideBarMain.Controls.Add(btnProperties);
            plSideBarMain.Controls.Add(btnExit);
            plSideBarMain.Controls.Add(btnLog);
            plSideBarMain.Controls.Add(btnImport);
            plSideBarMain.Controls.Add(btnLoad);
            plSideBarMain.Dock = DockStyle.Left;
            plSideBarMain.ForeColor = Color.Transparent;
            plSideBarMain.Location = new Point(0, 64);
            plSideBarMain.Name = "plSideBarMain";
            plSideBarMain.Size = new Size(127, 459);
            plSideBarMain.TabIndex = 63;
            // 
            // btnProperties
            // 
            btnProperties.BackColor = Color.Transparent;
            btnProperties.BackgroundImageLayout = ImageLayout.Zoom;
            btnProperties.Dock = DockStyle.Top;
            btnProperties.FlatAppearance.BorderSize = 0;
            btnProperties.FlatStyle = FlatStyle.Flat;
            btnProperties.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnProperties.ForeColor = Color.LightGray;
            btnProperties.ImageAlign = ContentAlignment.MiddleLeft;
            btnProperties.Location = new Point(0, 120);
            btnProperties.Name = "btnProperties";
            btnProperties.Size = new Size(127, 34);
            btnProperties.TabIndex = 59;
            btnProperties.Text = "Properties";
            btnProperties.TextAlign = ContentAlignment.MiddleLeft;
            btnProperties.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnProperties.UseVisualStyleBackColor = false;
            btnProperties.Click += BtnProperties_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.Transparent;
            btnExit.Dock = DockStyle.Bottom;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnExit.ForeColor = Color.LightGray;
            btnExit.ImageAlign = ContentAlignment.MiddleLeft;
            btnExit.Location = new Point(0, 419);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(127, 40);
            btnExit.TabIndex = 57;
            btnExit.Text = "Exit Application";
            btnExit.TextAlign = ContentAlignment.MiddleLeft;
            btnExit.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += BtnExit_Click;
            // 
            // btnLog
            // 
            btnLog.BackColor = Color.Transparent;
            btnLog.Dock = DockStyle.Top;
            btnLog.Enabled = false;
            btnLog.FlatAppearance.BorderSize = 0;
            btnLog.FlatStyle = FlatStyle.Flat;
            btnLog.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnLog.ForeColor = Color.LightGray;
            btnLog.ImageAlign = ContentAlignment.MiddleLeft;
            btnLog.Location = new Point(0, 80);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(127, 40);
            btnLog.TabIndex = 56;
            btnLog.Text = "Log Recods";
            btnLog.TextAlign = ContentAlignment.MiddleLeft;
            btnLog.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLog.UseVisualStyleBackColor = false;
            btnLog.Click += BtnLog_Click;
            // 
            // btnImport
            // 
            btnImport.BackColor = Color.Transparent;
            btnImport.Dock = DockStyle.Top;
            btnImport.Enabled = false;
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnImport.ForeColor = Color.LightGray;
            btnImport.ImageAlign = ContentAlignment.MiddleLeft;
            btnImport.Location = new Point(0, 40);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(127, 40);
            btnImport.TabIndex = 55;
            btnImport.Text = "Import Files";
            btnImport.TextAlign = ContentAlignment.MiddleLeft;
            btnImport.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnImport.UseVisualStyleBackColor = false;
            btnImport.Click += BtnImport_Click;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.Transparent;
            btnLoad.Dock = DockStyle.Top;
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnLoad.ForeColor = Color.LightGray;
            btnLoad.ImageAlign = ContentAlignment.MiddleLeft;
            btnLoad.Location = new Point(0, 0);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(127, 40);
            btnLoad.TabIndex = 54;
            btnLoad.Text = "Load Files";
            btnLoad.TextAlign = ContentAlignment.MiddleLeft;
            btnLoad.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += BtnLoad_Click;
            // 
            // plFormContainer
            // 
            plFormContainer.AutoScroll = true;
            plFormContainer.BackColor = Color.Transparent;
            plFormContainer.Dock = DockStyle.Fill;
            plFormContainer.ForeColor = Color.Transparent;
            plFormContainer.Location = new Point(127, 64);
            plFormContainer.MinimumSize = new Size(5, 5);
            plFormContainer.Name = "plFormContainer";
            plFormContainer.Size = new Size(843, 459);
            plFormContainer.TabIndex = 64;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoValidate = AutoValidate.Disable;
            BackColor = Color.FromArgb(56, 57, 60);
            ClientSize = new Size(970, 550);
            Controls.Add(plFormContainer);
            Controls.Add(plSideBarMain);
            Controls.Add(statusStrip);
            Controls.Add(tplHeader);
            Controls.Add(plTBPanel);
            DoubleBuffered = true;
            ForeColor = Color.FromArgb(56, 57, 60);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 1, 2, 1);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimumSize = new Size(320, 400);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "A2P v1.0.0.16";
            WindowState = FormWindowState.Maximized;
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            DpiChanged += MainForm_DpiChanged;
            ResizeBegin += MainForm_ResizeBegin;
            ResizeEnd += MainForm_ResizeEnd;
            tplHeader.ResumeLayout(false);
            tplHeader.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            plSideBarMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private TableLayoutPanel tplHeader;
        private Label lbHeader1;
        private Label lbHeader2;
        private Label lbHeader3;
        private Label lbHeader4;

    

  
        private Panel plTBPanel;
        private TableLayoutPanel plTbSBInfo;
        private Label lbInfoErrors;
        private Label lbInfoFiles;
        private Label lbInfoErrorCount;
        private Label lbInfoWarningCount;
        private Label lbInfoWarnings;
        private Label lbInfoRowsCount;
        private Label lbInfoWorksheetsCount;
        private Label lbInfoOrdersCount;
        private Label lbInfoFilesCount;
        private Label lbInfoRows;
        private Label lbInfoWorksheets;
        private Label lbInfoOrders;
        private Panel plNordanHeaderLogo;
        public StatusStrip miniToolStrip;
        public StatusStrip statusStrip;
        private ToolStripStatusLabel slbPathTitle;
        public ToolStripStatusLabel slbPathValue;
        private ToolStripStatusLabel slbDataSourceTitle;
        public ToolStripStatusLabel slbDataSourceValue;
        private Panel plSideBarMain;
        private Button btnProperties;
        private Button btnExit;
        private Button btnLog;
        private Button btnImport;
        private Button btnLoad;
        private Panel plFormContainer;
    }
}
