using a2p.WinForm.CustomControls;

namespace a2p.WinForm
{
    partial class MainForm
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
            if (disposing&&(components!=null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            plNordanHeaderLogo = new Panel();
            tplHeader = new TableLayoutPanel();
            lbHeader1 = new Label();
            lbHeader2 = new Label();
            plUniwaveHeaderLogo = new Panel();
            lbHeader3 = new Label();
            lbHeader4 = new Label();
            tlpTitleBar = new TableLayoutPanel();
            btMinimize = new Button();
            btClose = new Button();
            btMaximize = new Button();
            plMiniLogo = new Panel();
            plTitleBar = new Panel();
            plTitleBarAppName = new Label();
            plTBPanel = new Panel();
            slbPath = new ToolStripStatusLabel();
            statusStrip = new StatusStrip();
            tplSideBarHeader = new TableLayoutPanel();
            btnSideBar = new Button();
            btnProperties = new Button();
            plSideBarMain = new Panel();
            btnLoad = new Button();
            plTbSBInfo = new TableLayoutPanel();
            lbInfoErrors = new Label();
            lbInfoFiles = new Label();
            lbInfoErrorCount = new Label();
            lbInfoWarningCount = new Label();
            lbInfoWarnings = new Label();
            lbInfoRowsCount = new Label();
            lbInfoWorksheetsCount = new Label();
            lbInfoOrdersCount = new Label();
            lbInfoFilesCount = new Label();
            lbInfoRows = new Label();
            lbInfoWorksheets = new Label();
            lbInfoOrders = new Label();
            plSBButtons = new Panel();
            plFormContainer = new Panel();
            btnLog = new Button();
            btnImport = new Button();
            btnExit = new Button();
            tplHeader.SuspendLayout();
            tlpTitleBar.SuspendLayout();
            plTitleBar.SuspendLayout();
            plTBPanel.SuspendLayout();
            statusStrip.SuspendLayout();
            tplSideBarHeader.SuspendLayout();
            plSideBarMain.SuspendLayout();
            plTbSBInfo.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(72, 32);
            toolStripStatusLabel1.Text = "Path: ";
            toolStripStatusLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // plNordanHeaderLogo
            // 
            plNordanHeaderLogo.BackgroundImage = Properties.Resources.NordanLogoInversed;
            plNordanHeaderLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plNordanHeaderLogo.Dock = DockStyle.Top;
            plNordanHeaderLogo.Location = new Point(4, 4);
            plNordanHeaderLogo.Margin = new Padding(6);
            plNordanHeaderLogo.Name = "plNordanHeaderLogo";
            tplHeader.SetRowSpan(plNordanHeaderLogo, 2);
            plNordanHeaderLogo.Size = new Size(200, 98);
            plNordanHeaderLogo.TabIndex = 2;
            // 
            // tplHeader
            // 
            tplHeader.AutoSize = true;
            tplHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tplHeader.BackColor = Color.FromArgb(239, 112, 32);
            tplHeader.ColumnCount = 7;
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 204F));
            tplHeader.Controls.Add(lbHeader1, 1, 0);
            tplHeader.Controls.Add(lbHeader2, 2, 1);
            tplHeader.Controls.Add(plUniwaveHeaderLogo, 7, 0);
            tplHeader.Controls.Add(lbHeader3, 3, 0);
            tplHeader.Controls.Add(plNordanHeaderLogo, 0, 0);
            tplHeader.Controls.Add(lbHeader4, 5, 0);
            tplHeader.Dock = DockStyle.Top;
            tplHeader.Location = new Point(0, 50);
            tplHeader.Margin = new Padding(4);
            tplHeader.Name = "tplHeader";
            tplHeader.Padding = new Padding(4);
            tplHeader.RowCount = 3;
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 57F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tplHeader.Size = new Size(2072, 128);
            tplHeader.TabIndex = 1;
            // 
            // lbHeader1
            // 
            lbHeader1.AutoSize = true;
            lbHeader1.BackColor = Color.Transparent;
            lbHeader1.Dock = DockStyle.Fill;
            lbHeader1.Font = new Font("Segoe UI", 20F);
            lbHeader1.ForeColor = Color.FromArgb(248, 248, 249);
            lbHeader1.ImageAlign = ContentAlignment.TopRight;
            lbHeader1.Location = new Point(204, 4);
            lbHeader1.Margin = new Padding(6);
            lbHeader1.Name = "lbHeader1";
            tplHeader.SetRowSpan(lbHeader1, 3);
            lbHeader1.Size = new Size(697, 120);
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
            lbHeader2.Location = new Point(901, 45);
            lbHeader2.Margin = new Padding(6);
            lbHeader2.Name = "lbHeader2";
            tplHeader.SetRowSpan(lbHeader2, 2);
            lbHeader2.Size = new Size(60, 79);
            lbHeader2.TabIndex = 7;
            lbHeader2.Text = "2";
            lbHeader2.TextAlign = ContentAlignment.MiddleCenter;
            lbHeader2.UseCompatibleTextRendering = true;
            // 
            // plUniwaveHeaderLogo
            // 
            plUniwaveHeaderLogo.BackgroundImage = Properties.Resources.UniwaveLogoInversed;
            plUniwaveHeaderLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plUniwaveHeaderLogo.Dock = DockStyle.Top;
            plUniwaveHeaderLogo.Location = new Point(1864, 4);
            plUniwaveHeaderLogo.Margin = new Padding(6);
            plUniwaveHeaderLogo.Name = "plUniwaveHeaderLogo";
            tplHeader.SetRowSpan(plUniwaveHeaderLogo, 2);
            plUniwaveHeaderLogo.Size = new Size(204, 98);
            plUniwaveHeaderLogo.TabIndex = 1;
            // 
            // lbHeader3
            // 
            lbHeader3.AutoSize = true;
            lbHeader3.BackColor = Color.Transparent;
            lbHeader3.Dock = DockStyle.Bottom;
            lbHeader3.Font = new Font("Segoe UI", 16.125F, FontStyle.Bold);
            lbHeader3.ForeColor = Color.FromArgb(248, 248, 249);
            lbHeader3.ImageAlign = ContentAlignment.TopLeft;
            lbHeader3.Location = new Point(961, 36);
            lbHeader3.Margin = new Padding(6);
            lbHeader3.Name = "lbHeader3";
            tplHeader.SetRowSpan(lbHeader3, 2);
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
            lbHeader4.Location = new Point(1167, 4);
            lbHeader4.Margin = new Padding(6);
            lbHeader4.Name = "lbHeader4";
            lbHeader4.Size = new Size(64, 41);
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
            tlpTitleBar.Controls.Add(btMinimize, 2, 0);
            tlpTitleBar.Controls.Add(btClose, 4, 0);
            tlpTitleBar.Controls.Add(btMaximize, 3, 0);
            tlpTitleBar.Controls.Add(plMiniLogo, 0, 0);
            tlpTitleBar.Controls.Add(plTitleBar, 1, 0);
            tlpTitleBar.Dock = DockStyle.Fill;
            tlpTitleBar.Location = new Point(0, 0);
            tlpTitleBar.Margin = new Padding(6);
            tlpTitleBar.MinimumSize = new Size(30, 30);
            tlpTitleBar.Name = "tlpTitleBar";
            tlpTitleBar.RowCount = 1;
            tlpTitleBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpTitleBar.Size = new Size(2072, 50);
            tlpTitleBar.TabIndex = 0;
            // 
            // btMinimize
            // 
            btMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btMinimize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btMinimize.BackColor = Color.Transparent;
            btMinimize.BackgroundImageLayout = ImageLayout.Stretch;
            btMinimize.FlatAppearance.BorderSize = 0;
            btMinimize.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btMinimize.FlatStyle = FlatStyle.Flat;
            btMinimize.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btMinimize.ForeColor = Color.Transparent;
            btMinimize.ImageAlign = ContentAlignment.MiddleLeft;
            btMinimize.Location = new Point(1930, 8);
            btMinimize.Margin = new Padding(6);
            btMinimize.Name = "btMinimize";
            btMinimize.Size = new Size(34, 34);
            btMinimize.TabIndex = 8;
            btMinimize.TextAlign = ContentAlignment.MiddleLeft;
            btMinimize.TextImageRelation = TextImageRelation.ImageBeforeText;
            btMinimize.UseVisualStyleBackColor = false;
            btMinimize.Click += btMinimize_Click;
            btMinimize.MouseHover += btMinimize_MouseHover;
            // 
            // btClose
            // 
            btClose.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btClose.BackColor = Color.Transparent;
            btClose.BackgroundImageLayout = ImageLayout.Stretch;
            btClose.FlatAppearance.BorderSize = 0;
            btClose.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btClose.FlatStyle = FlatStyle.Flat;
            btClose.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btClose.ForeColor = Color.Transparent;
            btClose.ImageAlign = ContentAlignment.MiddleLeft;
            btClose.Location = new Point(2030, 8);
            btClose.Margin = new Padding(6);
            btClose.Name = "btClose";
            btClose.Size = new Size(34, 34);
            btClose.TabIndex = 7;
            btClose.TextAlign = ContentAlignment.MiddleLeft;
            btClose.TextImageRelation = TextImageRelation.ImageBeforeText;
            btClose.UseVisualStyleBackColor = false;
            btClose.Click += btClose_Click;
         // 
            // btMaximize
            // 
            btMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btMaximize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btMaximize.BackColor = Color.Transparent;
            btMaximize.BackgroundImageLayout = ImageLayout.Stretch;
            btMaximize.FlatAppearance.BorderSize = 0;
            btMaximize.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btMaximize.FlatStyle = FlatStyle.Flat;
            btMaximize.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btMaximize.ForeColor = Color.Transparent;
            btMaximize.ImageAlign = ContentAlignment.MiddleLeft;
            btMaximize.Location = new Point(1980, 8);
            btMaximize.Margin = new Padding(6);
            btMaximize.Name = "btMaximize";
            btMaximize.Size = new Size(34, 34);
            btMaximize.TabIndex = 6;
            btMaximize.TextAlign = ContentAlignment.MiddleLeft;
            btMaximize.TextImageRelation = TextImageRelation.ImageBeforeText;
            btMaximize.UseVisualStyleBackColor = false;
            btMaximize.Click += btMaximize_Click;
         
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
            plTitleBar.BackColor = Color.Transparent;
            plTitleBar.Controls.Add(plTitleBarAppName);
            plTitleBar.Dock = DockStyle.Fill;
            plTitleBar.ForeColor = Color.FromArgb(239, 112, 32);
            plTitleBar.Location = new Point(50, 0);
            plTitleBar.Margin = new Padding(6);
            plTitleBar.Name = "plTitleBar";
            plTitleBar.Size = new Size(1872, 50);
            plTitleBar.TabIndex = 4;
            plTitleBar.Text = "Alu 2 PrefSuite v2.0";
            plTitleBar.MouseDown += PlTitleBar_MouseDown;
            // 
            // plTitleBarAppName
            // 
            plTitleBarAppName.Enabled = false;
            plTitleBarAppName.FlatStyle = FlatStyle.Flat;
            plTitleBarAppName.ForeColor = Color.FromArgb(248, 248, 249);
            plTitleBarAppName.Location = new Point(6, 2);
            plTitleBarAppName.Margin = new Padding(6, 0, 6, 0);
            plTitleBarAppName.Name = "plTitleBarAppName";
            plTitleBarAppName.Size = new Size(255, 48);
            plTitleBarAppName.TabIndex = 0;
            plTitleBarAppName.Text = "Aluminum 2 PrefSuite ";
            plTitleBarAppName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // plTBPanel
            // 
            plTBPanel.AutoSize = true;
            plTBPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            plTBPanel.BackColor = Color.FromArgb(239, 112, 32);
            plTBPanel.Controls.Add(tlpTitleBar);
            plTBPanel.Dock = DockStyle.Top;
            plTBPanel.Location = new Point(0, 0);
            plTBPanel.Margin = new Padding(6);
            plTBPanel.Name = "plTBPanel";
            plTBPanel.Size = new Size(2072, 50);
            plTBPanel.TabIndex = 8;
            // 
            // slbPath
            // 
            slbPath.Name = "slbPath";
            slbPath.Size = new Size(0, 32);
            // 
            // statusStrip
            // 
            statusStrip.AccessibleRole = AccessibleRole.Grip;
            statusStrip.BackColor = Color.FromArgb(239, 112, 32);
            statusStrip.GripStyle = ToolStripGripStyle.Visible;
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, slbPath });
            statusStrip.Location = new Point(0, 1274);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(2, 0, 16, 0);
            statusStrip.Size = new Size(2072, 42);
            statusStrip.TabIndex = 0;
            statusStrip.Text = "statusStrip";
            // 
            // tplSideBarHeader
            // 
            tplSideBarHeader.ColumnCount = 3;
            tplSideBarHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tplSideBarHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tplSideBarHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tplSideBarHeader.Controls.Add(btnSideBar, 0, 0);
            tplSideBarHeader.Controls.Add(btnProperties, 2, 0);
            tplSideBarHeader.Dock = DockStyle.Top;
            tplSideBarHeader.Location = new Point(0, 178);
            tplSideBarHeader.Margin = new Padding(6);
            tplSideBarHeader.Name = "tplSideBarHeader";
            tplSideBarHeader.RowCount = 1;
            tplSideBarHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tplSideBarHeader.Size = new Size(2072, 80);
            tplSideBarHeader.TabIndex = 45;
            // 
            // btnSideBar
            // 
            btnSideBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnSideBar.BackColor = Color.Transparent;
            btnSideBar.FlatAppearance.BorderSize = 0;
            btnSideBar.FlatStyle = FlatStyle.Flat;
            btnSideBar.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnSideBar.ForeColor = Color.FromArgb(239, 112, 32);
            btnSideBar.ImageAlign = ContentAlignment.MiddleCenter;
            btnSideBar.Margin = new Padding();
            btnSideBar.Name = "btnSideBar";
            btnSideBar.Size = new Size(80, 80);
            btnSideBar.TabIndex = 49;
            btnSideBar.TextAlign = ContentAlignment.MiddleLeft;
            btnSideBar.UseVisualStyleBackColor = false;
            btnSideBar.Click += BtnSideBarClick;
            // 
            // btnProperties
            // 
            btnProperties.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnProperties.BackColor = Color.Transparent;
            btnProperties.BackgroundImageLayout = ImageLayout.Zoom;
            btnProperties.FlatAppearance.BorderSize = 0;
            btnProperties.FlatStyle = FlatStyle.Flat;
            btnProperties.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnProperties.ForeColor = Color.FromArgb(239, 112, 32);
            btnProperties.ImageAlign = ContentAlignment.MiddleLeft;
            btnProperties.Location = new Point(1992, 0);
            btnProperties.Margin = new Padding(6);
            btnProperties.Name = "btnProperties";
            btnProperties.Size = new Size(80, 80);
            btnProperties.TabIndex = 49;
            btnProperties.TextAlign = ContentAlignment.MiddleLeft;
            btnProperties.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnProperties.UseVisualStyleBackColor = false;
            btnProperties.Click += BtnPropertiesClick;
            // 
            // plSideBarMain
            // 
            plSideBarMain.BackColor = Color.Transparent;
            plSideBarMain.Controls.Add(btnLog);
            plSideBarMain.Controls.Add(btnImport);
            plSideBarMain.Controls.Add(btnLoad);
            plSideBarMain.Controls.Add(plTbSBInfo);
            plSideBarMain.Controls.Add(plSBButtons);
            plSideBarMain.Dock = DockStyle.Left;
            plSideBarMain.ForeColor = Color.Transparent;
            plSideBarMain.Location = new Point(0, 258);
            plSideBarMain.Margin = new Padding(6);
            plSideBarMain.Name = "plSideBarMain";
            plSideBarMain.Size = new Size(400, 1016);
            plSideBarMain.TabIndex = 46;
        
            // 
            // plTbSBInfo
            // 
            plTbSBInfo.BackColor = Color.Transparent;
            plTbSBInfo.ColumnCount = 2;
            plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            plTbSBInfo.Controls.Add(lbInfoErrors, 0, 6);
            plTbSBInfo.Controls.Add(lbInfoFiles, 0, 0);
            plTbSBInfo.Controls.Add(lbInfoErrorCount, 1, 6);
            plTbSBInfo.Controls.Add(lbInfoWarningCount, 1, 5);
            plTbSBInfo.Controls.Add(lbInfoWarnings, 0, 5);
            plTbSBInfo.Controls.Add(lbInfoRowsCount, 1, 3);
            plTbSBInfo.Controls.Add(lbInfoWorksheetsCount, 1, 2);
            plTbSBInfo.Controls.Add(lbInfoOrdersCount, 1, 1);
            plTbSBInfo.Controls.Add(lbInfoFilesCount, 1, 0);
            plTbSBInfo.Controls.Add(lbInfoRows, 0, 3);
            plTbSBInfo.Controls.Add(lbInfoWorksheets, 0, 2);
            plTbSBInfo.Controls.Add(lbInfoOrders, 0, 1);
            plTbSBInfo.Controls.Add(btnExit, 0, 9);
            plTbSBInfo.Dock = DockStyle.Bottom;
            plTbSBInfo.ForeColor = Color.FromArgb(248, 248, 249);
            plTbSBInfo.Location = new Point(0, 511);
            plTbSBInfo.Margin = new Padding(6);
            plTbSBInfo.Name = "plTbSBInfo";
            plTbSBInfo.RowCount = 11;
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            plTbSBInfo.Size = new Size(400, 505);
            plTbSBInfo.TabIndex = 12;
            // 
            // lbInfoErrors
            // 
            lbInfoErrors.AutoSize = true;
            lbInfoErrors.Dock = DockStyle.Fill;
            lbInfoErrors.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoErrors.ForeColor = Color.Crimson;
            lbInfoErrors.Location = new Point(8, 344);
            lbInfoErrors.Margin = new Padding(6);
            lbInfoErrors.Name = "lbInfoErrors";
            lbInfoErrors.Size = new Size(164, 40);
            lbInfoErrors.TabIndex = 9;
            lbInfoErrors.Text = "Errors:";
            lbInfoErrors.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoFiles
            // 
            lbInfoFiles.AutoSize = true;
            lbInfoFiles.Dock = DockStyle.Fill;
            lbInfoFiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoFiles.ForeColor = SystemColors.ScrollBar;
            lbInfoFiles.Location = new Point(8, 8);
            lbInfoFiles.Margin = new Padding(6);
            lbInfoFiles.Name = "lbInfoFiles";
            lbInfoFiles.Size = new Size(164, 40);
            lbInfoFiles.TabIndex = 4;
            lbInfoFiles.Text = "Files:";
            lbInfoFiles.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoErrorCount
            // 
            lbInfoErrorCount.AutoSize = true;
            lbInfoErrorCount.Dock = DockStyle.Fill;
            lbInfoErrorCount.Font = new Font("Segoe UI", 9F);
            lbInfoErrorCount.ForeColor = Color.Red;
            lbInfoErrorCount.Location = new Point(188, 344);
            lbInfoErrorCount.Margin = new Padding(6);
            lbInfoErrorCount.Name = "lbInfoErrorCount";
            lbInfoErrorCount.Size = new Size(204, 40);
            lbInfoErrorCount.TabIndex = 0;
            lbInfoErrorCount.Text = "0";
            lbInfoErrorCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWarningCount
            // 
            lbInfoWarningCount.AutoSize = true;
            lbInfoWarningCount.Dock = DockStyle.Fill;
            lbInfoWarningCount.Font = new Font("Segoe UI", 9F);
            lbInfoWarningCount.ForeColor = Color.Coral;
            lbInfoWarningCount.Location = new Point(188, 288);
            lbInfoWarningCount.Margin = new Padding(6);
            lbInfoWarningCount.Name = "lbInfoWarningCount";
            lbInfoWarningCount.Size = new Size(204, 40);
            lbInfoWarningCount.TabIndex = 0;
            lbInfoWarningCount.Text = "0";
            lbInfoWarningCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWarnings
            // 
            lbInfoWarnings.AutoSize = true;
            lbInfoWarnings.Dock = DockStyle.Fill;
            lbInfoWarnings.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoWarnings.ForeColor = Color.Coral;
            lbInfoWarnings.Location = new Point(8, 288);
            lbInfoWarnings.Margin = new Padding(6);
            lbInfoWarnings.Name = "lbInfoWarnings";
            lbInfoWarnings.Size = new Size(164, 40);
            lbInfoWarnings.TabIndex = 0;
            lbInfoWarnings.Text = "Warnings:";
            lbInfoWarnings.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoRowsCount
            // 
            lbInfoRowsCount.AutoSize = true;
            lbInfoRowsCount.Dock = DockStyle.Fill;
            lbInfoRowsCount.Font = new Font("Segoe UI", 9F);
            lbInfoRowsCount.ForeColor = SystemColors.ScrollBar;
            lbInfoRowsCount.Location = new Point(188, 176);
            lbInfoRowsCount.Margin = new Padding(6);
            lbInfoRowsCount.Name = "lbInfoRowsCount";
            lbInfoRowsCount.Size = new Size(204, 40);
            lbInfoRowsCount.TabIndex = 0;
            lbInfoRowsCount.Text = "10";
            lbInfoRowsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWorksheetsCount
            // 
            lbInfoWorksheetsCount.AutoSize = true;
            lbInfoWorksheetsCount.Dock = DockStyle.Fill;
            lbInfoWorksheetsCount.Font = new Font("Segoe UI", 9F);
            lbInfoWorksheetsCount.ForeColor = SystemColors.ScrollBar;
            lbInfoWorksheetsCount.Location = new Point(188, 120);
            lbInfoWorksheetsCount.Margin = new Padding(6);
            lbInfoWorksheetsCount.Name = "lbInfoWorksheetsCount";
            lbInfoWorksheetsCount.Size = new Size(204, 40);
            lbInfoWorksheetsCount.TabIndex = 0;
            lbInfoWorksheetsCount.Text = "10";
            lbInfoWorksheetsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoOrdersCount
            // 
            lbInfoOrdersCount.AutoSize = true;
            lbInfoOrdersCount.Dock = DockStyle.Fill;
            lbInfoOrdersCount.Font = new Font("Segoe UI", 9F);
            lbInfoOrdersCount.ForeColor = SystemColors.ScrollBar;
            lbInfoOrdersCount.Location = new Point(188, 64);
            lbInfoOrdersCount.Margin = new Padding(6);
            lbInfoOrdersCount.Name = "lbInfoOrdersCount";
            lbInfoOrdersCount.Size = new Size(204, 40);
            lbInfoOrdersCount.TabIndex = 0;
            lbInfoOrdersCount.Text = "10";
            lbInfoOrdersCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoFilesCount
            // 
            lbInfoFilesCount.AutoSize = true;
            lbInfoFilesCount.BackColor = Color.FromArgb(56, 57, 60);
            lbInfoFilesCount.Dock = DockStyle.Fill;
            lbInfoFilesCount.FlatStyle = FlatStyle.System;
            lbInfoFilesCount.Font = new Font("Segoe UI", 9F);
            lbInfoFilesCount.ForeColor = Color.FromArgb(248, 248, 249);
            lbInfoFilesCount.Location = new Point(188, 8);
            lbInfoFilesCount.Margin = new Padding(6);
            lbInfoFilesCount.Name = "lbInfoFilesCount";
            lbInfoFilesCount.Size = new Size(204, 40);
            lbInfoFilesCount.TabIndex = 0;
            lbInfoFilesCount.Text = "10";
            lbInfoFilesCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoRows
            // 
            lbInfoRows.AutoSize = true;
            lbInfoRows.Dock = DockStyle.Fill;
            lbInfoRows.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoRows.ForeColor = SystemColors.ScrollBar;
            lbInfoRows.Location = new Point(8, 176);
            lbInfoRows.Margin = new Padding(6);
            lbInfoRows.Name = "lbInfoRows";
            lbInfoRows.Size = new Size(164, 40);
            lbInfoRows.TabIndex = 0;
            lbInfoRows.Text = "Rows:";
            lbInfoRows.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoWorksheets
            // 
            lbInfoWorksheets.AutoSize = true;
            lbInfoWorksheets.Dock = DockStyle.Fill;
            lbInfoWorksheets.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoWorksheets.ForeColor = SystemColors.ScrollBar;
            lbInfoWorksheets.Location = new Point(8, 120);
            lbInfoWorksheets.Margin = new Padding(6);
            lbInfoWorksheets.Name = "lbInfoWorksheets";
            lbInfoWorksheets.Size = new Size(164, 40);
            lbInfoWorksheets.TabIndex = 0;
            lbInfoWorksheets.Text = "Worksheets:";
            lbInfoWorksheets.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoOrders
            // 
            lbInfoOrders.AutoSize = true;
            lbInfoOrders.Dock = DockStyle.Fill;
            lbInfoOrders.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoOrders.ForeColor = SystemColors.ScrollBar;
            lbInfoOrders.Location = new Point(8, 64);
            lbInfoOrders.Margin = new Padding(6);
            lbInfoOrders.Name = "lbInfoOrders";
            lbInfoOrders.Size = new Size(164, 40);
            lbInfoOrders.TabIndex = 3;
            lbInfoOrders.Text = "Orders:";
            lbInfoOrders.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // plSBButtons
            // 
            plSBButtons.AutoSize = true;
            plSBButtons.BackColor = Color.Transparent;
            plSBButtons.Dock = DockStyle.Top;
            plSBButtons.Location = new Point(0, 0);
            plSBButtons.Margin = new Padding(6);
            plSBButtons.Name = "plSBButtons";
            plSBButtons.Size = new Size(400, 0);
            plSBButtons.TabIndex = 11;
            // 
            // plFormContainer
            // 
            plFormContainer.AutoScroll = true;
            plFormContainer.BackColor = Color.Transparent;
            plFormContainer.Dock = DockStyle.Fill;
            plFormContainer.ForeColor = Color.Transparent;
            plFormContainer.Location = new Point(400, 258);
            plFormContainer.Margin = new Padding(6);
            plFormContainer.MinimumSize = new Size(10, 10);
            plFormContainer.Name = "plFormContainer";
            plFormContainer.Size = new Size(1672, 1016);
            plFormContainer.TabIndex = 47;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.Transparent;
            btnLoad.Dock = DockStyle.Top;
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnLoad.ForeColor = Color.White;
            btnLoad.ImageAlign = ContentAlignment.MiddleLeft;
            btnLoad.Location = new Point(0, 0);
            btnLoad.Margin = new Padding(6);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(400, 80);
            btnLoad.TabIndex = 51;
            btnLoad.Text = "Load Files";
            btnLoad.TextAlign = ContentAlignment.MiddleLeft;
            btnLoad.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += BtnLoadClick;
            // 
            // btnLog
            // 
            btnLog.BackColor = Color.Transparent;
            btnLog.Dock = DockStyle.Top;
            btnLog.FlatAppearance.BorderSize = 0;
            btnLog.FlatStyle = FlatStyle.Flat;
            btnLog.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnLog.ForeColor = Color.White;
            btnLog.ImageAlign = ContentAlignment.MiddleLeft;
            btnLog.Location = new Point(0, 160);
            btnLog.Margin = new Padding(6);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(400, 80);
            btnLog.TabIndex = 53;
            btnLog.Text = "Log Recods";
            btnLog.TextAlign = ContentAlignment.MiddleLeft;
            btnLog.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLog.UseVisualStyleBackColor = false;
            btnLog.Click += BtnLogClick;
            // 
            // btnImport
            // 
            btnImport.BackColor = Color.Transparent;
            btnImport.Dock = DockStyle.Top;
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnImport.ForeColor = Color.White;
            btnImport.ImageAlign = ContentAlignment.MiddleLeft;
            btnImport.Location = new Point(0, 80);
            btnImport.Margin = new Padding(6);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(400, 80);
            btnImport.TabIndex = 52;
            btnImport.Text = "Import Files";
            btnImport.TextAlign = ContentAlignment.MiddleLeft;
            btnImport.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnImport.UseVisualStyleBackColor = false;
            btnImport.Click += BtnImportClick;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.Transparent;
            plTbSBInfo.SetColumnSpan(btnExit, 2);
            btnExit.Dock = DockStyle.Bottom;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnExit.ForeColor = Color.White;
            btnExit.ImageAlign = ContentAlignment.MiddleLeft;
            btnExit.Location = new Point(0, 405);
            btnExit.Margin = new Padding(6);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(400, 80);
            btnExit.TabIndex = 55;
            btnExit.Text = "Exit Application";
            btnExit.TextAlign = ContentAlignment.MiddleLeft;
            btnExit.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += BtnExit_Click;

            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoValidate = AutoValidate.Disable;
            BackColor = Color.FromArgb(56, 57, 60);
            ClientSize = new Size(2072, 1316);
            Controls.Add(plFormContainer);
            Controls.Add(plSideBarMain);
            Controls.Add(tplSideBarHeader);
            Controls.Add(tplHeader);
            Controls.Add(plTBPanel);
            Controls.Add(statusStrip);
            DoubleBuffered = true;
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
            tplHeader.ResumeLayout(false);
            tplHeader.PerformLayout();
            tlpTitleBar.ResumeLayout(false);
            plTitleBar.ResumeLayout(false);
            plTBPanel.ResumeLayout(false);
            plTBPanel.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            tplSideBarHeader.ResumeLayout(false);
            plSideBarMain.ResumeLayout(false);
            plSideBarMain.PerformLayout();
            plTbSBInfo.ResumeLayout(false);
            plTbSBInfo.PerformLayout();
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
        private Panel plNordanHeaderLogo; 
        private Panel plUniwaveHeaderLogo;

    

  
        private Panel plTBPanel;
        private ToolStripStatusLabel slbPath;
        private StatusStrip statusStrip;
        private TableLayoutPanel tplSideBarHeader;

        private Panel plSideBarMain;

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
        private Panel plSBButtons;
        private Panel plFormContainer;
        private Button btnProperties;
        private Button btnLoad;
        private Button btnSideBar;
        private Button btMaximize;
        private Button btClose;
        private Button btMinimize;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button btnLog;
        private Button btnImport;
        private Button btnExit;
    }
}
