using a2p.WinForm.CustomControls;

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
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tplHeader = new TableLayoutPanel();
            plNordanHeaderLogo = new Panel();
            lbHeader1 = new Label();
            lbHeader2 = new Label();
            plUniwaveHeaderLogo = new Panel();
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
            slbPath = new ToolStripStatusLabel();
            statusStrip = new StatusStrip();
            plSideBarMain = new Panel();
            btnProperties = new Button();
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
            plTbSBInfo.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(72, 32);
            toolStripStatusLabel1.Text = "FilePath: ";
            toolStripStatusLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tplHeader
            // 
            tplHeader.AutoSize = true;
            tplHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tplHeader.BackColor = Color.FromArgb(239, 112, 32);
            tplHeader.ColumnCount = 7;
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tplHeader.Controls.Add(plNordanHeaderLogo, 6, 2);
            tplHeader.Controls.Add(lbHeader1, 0, 0);
            tplHeader.Controls.Add(lbHeader2, 1, 1);
            tplHeader.Controls.Add(plUniwaveHeaderLogo, 6, 0);
            tplHeader.Controls.Add(lbHeader3, 2, 0);
            tplHeader.Controls.Add(lbHeader4, 4, 0);
            tplHeader.Dock = DockStyle.Top;
            tplHeader.Location = new Point(0, 50);
            tplHeader.Margin = new Padding(4);
            tplHeader.Name = "tplHeader";
            tplHeader.Padding = new Padding(4);
            tplHeader.RowCount = 3;
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 57F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tplHeader.Size = new Size(2072, 148);
            tplHeader.TabIndex = 1;
            // 
            // plNordanHeaderLogo
            // 
            plNordanHeaderLogo.BackgroundImage = Properties.Resources.NordanLogoInversed;
            plNordanHeaderLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plNordanHeaderLogo.Dock = DockStyle.Top;
            plNordanHeaderLogo.Location = new Point(1868, 102);
            plNordanHeaderLogo.Margin = new Padding(0);
            plNordanHeaderLogo.Name = "plNordanHeaderLogo";
            tplHeader.SetRowSpan(plNordanHeaderLogo, 2);
            plNordanHeaderLogo.Size = new Size(200, 41);
            plNordanHeaderLogo.TabIndex = 9;
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
            tplHeader.SetRowSpan(lbHeader1, 3);
            lbHeader1.Size = new Size(268, 108);
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
            lbHeader2.Location = new Point(290, 51);
            lbHeader2.Margin = new Padding(6);
            lbHeader2.Name = "lbHeader2";
            tplHeader.SetRowSpan(lbHeader2, 2);
            lbHeader2.Size = new Size(48, 67);
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
            plUniwaveHeaderLogo.Location = new Point(1868, 4);
            plUniwaveHeaderLogo.Margin = new Padding(0);
            plUniwaveHeaderLogo.Name = "plUniwaveHeaderLogo";
            plUniwaveHeaderLogo.Size = new Size(200, 41);
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
            lbHeader3.Location = new Point(350, 30);
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
            lbHeader4.Location = new Point(568, 10);
            lbHeader4.Margin = new Padding(6);
            lbHeader4.Name = "lbHeader4";
            lbHeader4.Size = new Size(64, 29);
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
            tlpTitleBar.Dock = DockStyle.Fill;
            tlpTitleBar.ForeColor = Color.FromArgb(56, 57, 60);
            tlpTitleBar.Location = new Point(0, 0);
            tlpTitleBar.Margin = new Padding(6);
            tlpTitleBar.MinimumSize = new Size(30, 30);
            tlpTitleBar.Name = "tlpTitleBar";
            tlpTitleBar.RowCount = 1;
            tlpTitleBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpTitleBar.Size = new Size(2072, 50);
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
            btnMinimize.Location = new Point(1930, 8);
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
            btnClose.Location = new Point(2030, 8);
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
            btnMaximize.Location = new Point(1980, 8);
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
            plTitleBar.Size = new Size(1860, 38);
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
            plTitleBarAppName.Size = new Size(255, 38);
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
            // plSideBarMain
            // 
            plSideBarMain.BackColor = Color.Transparent;
            plSideBarMain.Controls.Add(btnProperties);
            plSideBarMain.Controls.Add(plTbSBInfo);
            plSideBarMain.Controls.Add(btnExit);
            plSideBarMain.Controls.Add(btnLog);
            plSideBarMain.Controls.Add(btnImport);
            plSideBarMain.Controls.Add(btnLoad);
            plSideBarMain.Dock = DockStyle.Left;
            plSideBarMain.ForeColor = Color.Transparent;
            plSideBarMain.Location = new Point(0, 198);
            plSideBarMain.Margin = new Padding(6);
            plSideBarMain.Name = "plSideBarMain";
            plSideBarMain.Size = new Size(400, 1076);
            plSideBarMain.TabIndex = 46;
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
            btnProperties.Size = new Size(400, 68);
            btnProperties.TabIndex = 59;
            btnProperties.Text = "Properties";
            btnProperties.TextAlign = ContentAlignment.MiddleLeft;
            btnProperties.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnProperties.UseVisualStyleBackColor = false;
            btnProperties.Click += BtnProperties_Click;
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
            plTbSBInfo.Dock = DockStyle.Bottom;
            plTbSBInfo.ForeColor = Color.FromArgb(248, 248, 249);
            plTbSBInfo.Location = new Point(0, 491);
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
            plTbSBInfo.TabIndex = 58;
            // 
            // lbInfoErrors
            // 
            lbInfoErrors.AutoSize = true;
            lbInfoErrors.Dock = DockStyle.Fill;
            lbInfoErrors.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoErrors.ForeColor = Color.Crimson;
            lbInfoErrors.Location = new Point(6, 342);
            lbInfoErrors.Margin = new Padding(6);
            lbInfoErrors.Name = "lbInfoErrors";
            lbInfoErrors.Size = new Size(168, 44);
            lbInfoErrors.TabIndex = 9;
            lbInfoErrors.Text = "Errors:";
            lbInfoErrors.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoFiles
            // 
            lbInfoFiles.AutoSize = true;
            lbInfoFiles.Dock = DockStyle.Fill;
            lbInfoFiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoFiles.ForeColor = Color.DarkGray;
            lbInfoFiles.Location = new Point(6, 6);
            lbInfoFiles.Margin = new Padding(6);
            lbInfoFiles.Name = "lbInfoFiles";
            lbInfoFiles.Size = new Size(168, 44);
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
            lbInfoErrorCount.Location = new Point(186, 342);
            lbInfoErrorCount.Margin = new Padding(6);
            lbInfoErrorCount.Name = "lbInfoErrorCount";
            lbInfoErrorCount.Size = new Size(208, 44);
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
            lbInfoWarningCount.Location = new Point(186, 286);
            lbInfoWarningCount.Margin = new Padding(6);
            lbInfoWarningCount.Name = "lbInfoWarningCount";
            lbInfoWarningCount.Size = new Size(208, 44);
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
            lbInfoWarnings.Location = new Point(6, 286);
            lbInfoWarnings.Margin = new Padding(6);
            lbInfoWarnings.Name = "lbInfoWarnings";
            lbInfoWarnings.Size = new Size(168, 44);
            lbInfoWarnings.TabIndex = 0;
            lbInfoWarnings.Text = "Warnings:";
            lbInfoWarnings.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoRowsCount
            // 
            lbInfoRowsCount.AutoSize = true;
            lbInfoRowsCount.Dock = DockStyle.Fill;
            lbInfoRowsCount.FlatStyle = FlatStyle.Flat;
            lbInfoRowsCount.Font = new Font("Segoe UI", 9F);
            lbInfoRowsCount.ForeColor = Color.DarkGray;
            lbInfoRowsCount.Location = new Point(186, 174);
            lbInfoRowsCount.Margin = new Padding(6);
            lbInfoRowsCount.Name = "lbInfoRowsCount";
            lbInfoRowsCount.Size = new Size(208, 44);
            lbInfoRowsCount.TabIndex = 0;
            lbInfoRowsCount.Text = "10";
            lbInfoRowsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWorksheetsCount
            // 
            lbInfoWorksheetsCount.AutoSize = true;
            lbInfoWorksheetsCount.Dock = DockStyle.Fill;
            lbInfoWorksheetsCount.FlatStyle = FlatStyle.Flat;
            lbInfoWorksheetsCount.Font = new Font("Segoe UI", 9F);
            lbInfoWorksheetsCount.ForeColor = Color.DarkGray;
            lbInfoWorksheetsCount.Location = new Point(186, 118);
            lbInfoWorksheetsCount.Margin = new Padding(6);
            lbInfoWorksheetsCount.Name = "lbInfoWorksheetsCount";
            lbInfoWorksheetsCount.Size = new Size(208, 44);
            lbInfoWorksheetsCount.TabIndex = 0;
            lbInfoWorksheetsCount.Text = "10";
            lbInfoWorksheetsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoOrdersCount
            // 
            lbInfoOrdersCount.AutoSize = true;
            lbInfoOrdersCount.Dock = DockStyle.Fill;
            lbInfoOrdersCount.FlatStyle = FlatStyle.Flat;
            lbInfoOrdersCount.Font = new Font("Segoe UI", 9F);
            lbInfoOrdersCount.ForeColor = Color.DarkGray;
            lbInfoOrdersCount.Location = new Point(186, 62);
            lbInfoOrdersCount.Margin = new Padding(6);
            lbInfoOrdersCount.Name = "lbInfoOrdersCount";
            lbInfoOrdersCount.Size = new Size(208, 44);
            lbInfoOrdersCount.TabIndex = 0;
            lbInfoOrdersCount.Text = "10";
            lbInfoOrdersCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoFilesCount
            // 
            lbInfoFilesCount.AutoSize = true;
            lbInfoFilesCount.BackColor = Color.FromArgb(56, 57, 60);
            lbInfoFilesCount.Dock = DockStyle.Fill;
            lbInfoFilesCount.FlatStyle = FlatStyle.Flat;
            lbInfoFilesCount.Font = new Font("Segoe UI", 9F);
            lbInfoFilesCount.ForeColor = Color.DarkGray;
            lbInfoFilesCount.Location = new Point(186, 6);
            lbInfoFilesCount.Margin = new Padding(6);
            lbInfoFilesCount.Name = "lbInfoFilesCount";
            lbInfoFilesCount.Size = new Size(208, 44);
            lbInfoFilesCount.TabIndex = 0;
            lbInfoFilesCount.Text = "10";
            lbInfoFilesCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoRows
            // 
            lbInfoRows.AutoSize = true;
            lbInfoRows.Dock = DockStyle.Fill;
            lbInfoRows.FlatStyle = FlatStyle.Flat;
            lbInfoRows.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoRows.ForeColor = Color.DarkGray;
            lbInfoRows.Location = new Point(6, 174);
            lbInfoRows.Margin = new Padding(6);
            lbInfoRows.Name = "lbInfoRows";
            lbInfoRows.Size = new Size(168, 44);
            lbInfoRows.TabIndex = 0;
            lbInfoRows.Text = "Rows:";
            lbInfoRows.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoWorksheets
            // 
            lbInfoWorksheets.AutoSize = true;
            lbInfoWorksheets.Dock = DockStyle.Fill;
            lbInfoWorksheets.FlatStyle = FlatStyle.Flat;
            lbInfoWorksheets.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoWorksheets.ForeColor = Color.DarkGray;
            lbInfoWorksheets.Location = new Point(6, 118);
            lbInfoWorksheets.Margin = new Padding(6);
            lbInfoWorksheets.Name = "lbInfoWorksheets";
            lbInfoWorksheets.Size = new Size(168, 44);
            lbInfoWorksheets.TabIndex = 0;
            lbInfoWorksheets.Text = "Worksheets:";
            lbInfoWorksheets.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoOrders
            // 
            lbInfoOrders.AutoSize = true;
            lbInfoOrders.Dock = DockStyle.Fill;
            lbInfoOrders.FlatStyle = FlatStyle.Flat;
            lbInfoOrders.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoOrders.ForeColor = Color.DarkGray;
            lbInfoOrders.Location = new Point(6, 62);
            lbInfoOrders.Margin = new Padding(6);
            lbInfoOrders.Name = "lbInfoOrders";
            lbInfoOrders.Size = new Size(168, 44);
            lbInfoOrders.TabIndex = 3;
            lbInfoOrders.Text = "Orders:";
            lbInfoOrders.TextAlign = ContentAlignment.MiddleLeft;
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
            btnExit.Location = new Point(0, 996);
            btnExit.Margin = new Padding(6);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(400, 80);
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
            btnLog.FlatAppearance.BorderSize = 0;
            btnLog.FlatStyle = FlatStyle.Flat;
            btnLog.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnLog.ForeColor = Color.LightGray;
            btnLog.ImageAlign = ContentAlignment.MiddleLeft;
            btnLog.Location = new Point(0, 160);
            btnLog.Margin = new Padding(6);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(400, 80);
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
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnImport.ForeColor = Color.LightGray;
            btnImport.ImageAlign = ContentAlignment.MiddleLeft;
            btnImport.Location = new Point(0, 80);
            btnImport.Margin = new Padding(6);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(400, 80);
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
            btnLoad.Size = new Size(400, 80);
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
            plFormContainer.Location = new Point(400, 198);
            plFormContainer.Margin = new Padding(6);
            plFormContainer.MinimumSize = new Size(10, 10);
            plFormContainer.Name = "plFormContainer";
            plFormContainer.Size = new Size(1672, 1076);
            plFormContainer.TabIndex = 47;
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
            Controls.Add(tplHeader);
            Controls.Add(plTBPanel);
            Controls.Add(statusStrip);
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
            tplHeader.ResumeLayout(false);
            tplHeader.PerformLayout();
            tlpTitleBar.ResumeLayout(false);
            plTitleBar.ResumeLayout(false);
            plTBPanel.ResumeLayout(false);
            plTBPanel.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            plSideBarMain.ResumeLayout(false);
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
        private Panel plUniwaveHeaderLogo;

    

  
        private Panel plTBPanel;
        private ToolStripStatusLabel slbPath;
        private StatusStrip statusStrip;

        private Panel plSideBarMain;
        private Panel plFormContainer;
        private Button btnMaximize;
        private Button btnClose;
        private Button btnMinimize;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button btnLoad;
        private Button btnLog;
        private Button btnImport;
        private Button btnExit;
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
        private Button btnProperties;
        private Panel plNordanHeaderLogo;
    }
}
