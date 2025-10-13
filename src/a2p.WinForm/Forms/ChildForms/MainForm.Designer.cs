using Alu2PrefSuite.ChildForms;

namespace Alu2PrefSuite

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
            tlpTitleBar = new TableLayoutPanel();
            btnMinimize = new Button();
            btnClose = new Button();
            btnMaximize = new Button();
            plMiniLogo = new Panel();
            plTitleBar = new Panel();
            plTitleBarAppName = new Label();
            plTBPanel = new Panel();
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
            tlpTitleBar.SuspendLayout();
            plTitleBar.SuspendLayout();
            plTBPanel.SuspendLayout();
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
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500F));
            tplHeader.Controls.Add(plNordanHeaderLogo, 5, 0);
            tplHeader.Controls.Add(lbHeader1, 0, 0);
            tplHeader.Controls.Add(lbHeader2, 1, 0);
            tplHeader.Controls.Add(lbHeader3, 2, 0);
            tplHeader.Controls.Add(lbHeader4, 4, 0);
            tplHeader.Dock = DockStyle.Top;
            tplHeader.Location = new Point(0, 50);
            tplHeader.Margin = new Padding(4);
            tplHeader.Name = "tplHeader";
            tplHeader.Padding = new Padding(4);
            tplHeader.RowCount = 1;
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tplHeader.Size = new Size(1940, 128);
            tplHeader.TabIndex = 1;
            // 
            // plNordanHeaderLogo
            // 
            plNordanHeaderLogo.BackgroundImage = Alu2PrefSuite.Properties.Resources.ResourceManager.GetObject(name: "NordanLogoInversed");       
            plNordanHeaderLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plNordanHeaderLogo.Dock = DockStyle.Fill;
            plNordanHeaderLogo.Location = new Point(1436, 4);
            plNordanHeaderLogo.Margin = new Padding(0);
            plNordanHeaderLogo.Name = "plNordanHeaderLogo";
            tplHeader.SetRowSpan(plNordanHeaderLogo, 3);
            plNordanHeaderLogo.Size = new Size(500, 120);
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
            lbHeader1.Location = new Point(10, 10);
            lbHeader1.Margin = new Padding(6);
            lbHeader1.Name = "lbHeader1";
            tplHeader.SetRowSpan(lbHeader1, 2);
            lbHeader1.Size = new Size(268, 86);
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
            lbHeader2.Location = new Point(290, 10);
            lbHeader2.Margin = new Padding(6);
            lbHeader2.Name = "lbHeader2";
            tplHeader.SetRowSpan(lbHeader2, 2);
            lbHeader2.Size = new Size(48, 86);
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
            lbHeader3.Location = new Point(350, 52);
            lbHeader3.Margin = new Padding(6);
            lbHeader3.Name = "lbHeader3";
            tplHeader.SetRowSpan(lbHeader3, 3);
            lbHeader3.Size = new Size(206, 66);
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
            lbHeader4.Location = new Point(568, 11);
            lbHeader4.Margin = new Padding(6);
            lbHeader4.Name = "lbHeader4";
            lbHeader4.Size = new Size(64, 43);
            lbHeader4.TabIndex = 5;
            lbHeader4.Text = "v2.0";
            lbHeader4.TextAlign = ContentAlignment.BottomLeft;
            lbHeader4.UseCompatibleTextRendering = true;
            // 
            // tlpTitleBar
            // 
            tlpTitleBar.AutoSize = true;
            tlpTitleBar.BackColor = Color.FromArgb(56, 57, 60);
            tlpTitleBar.ColumnCount = 5;
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tlpTitleBar.Controls.Add(btnMinimize, 2, 0);
            tlpTitleBar.Controls.Add(btnClose, 4, 0);
            tlpTitleBar.Controls.Add(btnMaximize, 3, 0);
            tlpTitleBar.Controls.Add(plMiniLogo, 0, 0);
            tlpTitleBar.Controls.Add(plTitleBar, 1, 0);
            tlpTitleBar.Dock = DockStyle.Top;
            tlpTitleBar.ForeColor = Color.FromArgb(56, 57, 60);
            tlpTitleBar.Location = new Point(0, 0);
            tlpTitleBar.Margin = new Padding(6);
            tlpTitleBar.MinimumSize = new Size(30, 30);
            tlpTitleBar.Name = "tlpTitleBar";
            tlpTitleBar.RowCount = 1;
            tlpTitleBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpTitleBar.Size = new Size(1940, 50);
            tlpTitleBar.TabIndex = 0;
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnMinimize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnMinimize.BackColor = Color.FromArgb(56, 57, 60);
            btnMinimize.BackgroundImageLayout = ImageLayout.Stretch;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnMinimize.ForeColor = Color.FromArgb(56, 57, 60);
            btnMinimize.Location = new Point(1798, 8);
            btnMinimize.Margin = new Padding(8);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(34, 34);
            btnMinimize.TabIndex = 8;
            btnMinimize.TextAlign = ContentAlignment.MiddleLeft;
            btnMinimize.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnMinimize.UseVisualStyleBackColor = false;
            btnMinimize.Click += btMinimize_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnClose.BackColor = Color.FromArgb(56, 57, 60);
            btnClose.BackgroundImageLayout = ImageLayout.Stretch;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnClose.ForeColor = Color.FromArgb(56, 57, 60);
            btnClose.Location = new Point(1898, 8);
            btnClose.Margin = new Padding(8);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(34, 34);
            btnClose.TabIndex = 7;
            btnClose.TextAlign = ContentAlignment.MiddleLeft;
            btnClose.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btClose_Click;
            // 
            // btnMaximize
            // 
            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnMaximize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnMaximize.BackColor = Color.FromArgb(56, 57, 60);
            btnMaximize.BackgroundImageLayout = ImageLayout.Stretch;
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnMaximize.FlatStyle = FlatStyle.Flat;
            btnMaximize.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnMaximize.ForeColor = Color.FromArgb(56, 57, 60);
            btnMaximize.Location = new Point(1848, 8);
            btnMaximize.Margin = new Padding(8);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(34, 34);
            btnMaximize.TabIndex = 6;
            btnMaximize.TextAlign = ContentAlignment.MiddleLeft;
            btnMaximize.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnMaximize.UseVisualStyleBackColor = false;
            btnMaximize.Click += btMaximize_Click;
            // 
            // plMiniLogo
            // 
            plMiniLogo.BackColor = Color.FromArgb(56, 57, 60);
            plMiniLogo.BackgroundImage = (Image)resources.GetObject("plMiniLogo.BackgroundImage");
            plMiniLogo.BackgroundImageLayout = ImageLayout.Stretch;
            plMiniLogo.Dock = DockStyle.Fill;
            plMiniLogo.Location = new Point(6, 6);
            plMiniLogo.Margin = new Padding(6);
            plMiniLogo.Name = "plMiniLogo";
            plMiniLogo.Size = new Size(38, 38);
            plMiniLogo.TabIndex = 3;
            // 
            // plTitleBar
            // 
            plTitleBar.BackColor = Color.FromArgb(56, 57, 60);
            plTitleBar.Controls.Add(plTitleBarAppName);
            plTitleBar.Dock = DockStyle.Fill;
            plTitleBar.ForeColor = Color.FromArgb(239, 112, 32);
            plTitleBar.Location = new Point(56, 6);
            plTitleBar.Margin = new Padding(6);
            plTitleBar.Name = "plTitleBar";
            plTitleBar.Size = new Size(1728, 38);
            plTitleBar.TabIndex = 4;
            plTitleBar.Text = "Alu 2 PrefSuite v2.0";
            plTitleBar.MouseDown += PlTitleBar_MouseDown;
            // 
            // plTitleBarAppName
            // 
            plTitleBarAppName.Dock = DockStyle.Left;
            plTitleBarAppName.Enabled = false;
            plTitleBarAppName.FlatStyle = FlatStyle.Flat;
            plTitleBarAppName.ForeColor = Color.FromArgb(248, 248, 249);
            plTitleBarAppName.Location = new Point(0, 0);
            plTitleBarAppName.Margin = new Padding(6, 0, 6, 0);
            plTitleBarAppName.Name = "plTitleBarAppName";
            plTitleBarAppName.Size = new Size(256, 38);
            plTitleBarAppName.TabIndex = 0;
            plTitleBarAppName.Text = "Aluminum 2 PrefSuite ";
            plTitleBarAppName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // plTBPanel
            // 
            plTBPanel.AutoSize = true;
            plTBPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            plTBPanel.BackColor = Color.FromArgb(56, 57, 60);
            plTBPanel.Controls.Add(tlpTitleBar);
            plTBPanel.Dock = DockStyle.Top;
            plTBPanel.ForeColor = Color.FromArgb(56, 57, 60);
            plTBPanel.Location = new Point(0, 0);
            plTBPanel.Margin = new Padding(6);
            plTBPanel.Name = "plTBPanel";
            plTBPanel.Size = new Size(1940, 50);
            plTBPanel.TabIndex = 8;
            // 
            // statusStrip
            // 
            statusStrip.AutoSize = false;
            statusStrip.BackColor = Color.FromArgb(239, 112, 32);
            statusStrip.GripStyle = ToolStripGripStyle.Visible;
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Items.AddRange(new ToolStripItem[] { slbPathTitle, slbPathValue, slbDataSourceTitle, slbDataSourceValue });
            statusStrip.Location = new Point(0, 1046);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(2, 0, 16, 0);
            statusStrip.Size = new Size(1940, 54);
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
            slbPathTitle.Size = new Size(117, 42);
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
            slbPathValue.Size = new Size(105, 42);
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
            slbDataSourceTitle.Size = new Size(157, 42);
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
            slbDataSourceValue.Size = new Size(90, 42);
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
            plSideBarMain.Location = new Point(0, 178);
            plSideBarMain.Margin = new Padding(6);
            plSideBarMain.Name = "plSideBarMain";
            plSideBarMain.Size = new Size(338, 868);
            plSideBarMain.TabIndex = 63;
            plSideBarMain.Click += BtnLoad_Click;
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
            btnProperties.Location = new Point(0, 240);
            btnProperties.Margin = new Padding(6);
            btnProperties.Name = "btnProperties";
            btnProperties.Size = new Size(338, 68);
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
            btnExit.Location = new Point(0, 788);
            btnExit.Margin = new Padding(6);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(338, 80);
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
            btnLog.Location = new Point(0, 160);
            btnLog.Margin = new Padding(6);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(338, 80);
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
            btnImport.Location = new Point(0, 80);
            btnImport.Margin = new Padding(6);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(338, 80);
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
            btnLoad.Margin = new Padding(6);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(338, 80);
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
            plFormContainer.Location = new Point(338, 178);
            plFormContainer.Margin = new Padding(6);
            plFormContainer.MinimumSize = new Size(10, 10);
            plFormContainer.Name = "plFormContainer";
            plFormContainer.Size = new Size(1602, 868);
            plFormContainer.TabIndex = 64;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoValidate = AutoValidate.Disable;
            BackColor = Color.FromArgb(56, 57, 60);
            ClientSize = new Size(1940, 1100);
            Controls.Add(plFormContainer);
            Controls.Add(plSideBarMain);
            Controls.Add(statusStrip);
            Controls.Add(tplHeader);
            Controls.Add(plTBPanel);
            DoubleBuffered = true;
            ForeColor = Color.FromArgb(56, 57, 60);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 2, 4, 2);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 800);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Alu 2 PrefSuite v2.0";
            WindowState = FormWindowState.Maximized;
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            DpiChanged += MainForm_DpiChanged;
            ResizeBegin += MainForm_ResizeBegin;
            ResizeEnd += MainForm_ResizeEnd;
            tplHeader.ResumeLayout(false);
            tplHeader.PerformLayout();
            tlpTitleBar.ResumeLayout(false);
            plTitleBar.ResumeLayout(false);
            plTBPanel.ResumeLayout(false);
            plTBPanel.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            plSideBarMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }



        #endregion

        private TableLayoutPanel tlpTitleBar;
        private Panel plMiniLogo;
        private Panel plTitleBar;
        private Label plTitleBarAppName;
        private TableLayoutPanel tplHeader;
        private Label lbHeader1;
        private Label lbHeader2;
        private Label lbHeader3;
        private Label lbHeader4;

    

  
        private Panel plTBPanel;
        private Button btnMaximize;
        private Button btnClose;
        private Button btnMinimize;
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
