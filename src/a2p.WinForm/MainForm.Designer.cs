using a2p.WinForm.CustomControls;
using System.Net;
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
            ToolStripStatusLabel toolStripStatusLabel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            lbHeader4 = new Label();
            plNordanHeaderLogo = new Panel();
            lbHeader3 = new Label();
            plUniwaveHeaderLogo = new Panel();
            lbHeader2 = new Label();
            lbHeader1 = new Label();
            tplHeader = new TableLayoutPanel();
            plSBButtons = new Panel();
            plTbSBInfo = new TableLayoutPanel();
            lbInfoErrors = new Label();
            lbInfoFiles = new Label();
            lbErrorCount = new Label();
            lbWarningCount = new Label();
            lbInfoWarnings = new Label();
            rowsCount = new Label();
            lbWorksheetsCount = new Label();
            lbOrdersCount = new Label();
            lbFilesCount = new Label();
            lbInfoRows = new Label();
            lbInfoWorksheets = new Label();
            lbInfoOrders = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            btSideBar = new SideBarButton();
            btLoadFiles = new SideBarButton();
            btImport = new SideBarButton();
            btLog = new SideBarButton();
            btProperties = new SideBarButton();
            btExit = new SideBarButton();
            plSideBarMain = new Panel();
            tlpTitleBar = new TableLayoutPanel();
            btMinimize = new Button();
            btMaximize = new Button();
            btClose = new SideBarButton();
            plMiniLogo = new Panel();
            plTitleBar = new Panel();
            plTitleBarAppName = new Label();
            plTBPanel = new Panel();
            lbErrors = new Label();
            slbPath = new ToolStripStatusLabel();
            statusStrip = new StatusStrip();
            plFormContainer = new Panel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tplHeader.SuspendLayout();
            plTbSBInfo.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            plSideBarMain.SuspendLayout();
            tlpTitleBar.SuspendLayout();
            plTitleBar.SuspendLayout();
            plTBPanel.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(72, 32);
            toolStripStatusLabel1.Text = "Path: ";
            toolStripStatusLabel1.TextAlign = ContentAlignment.MiddleLeft;
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
            lbHeader4.Margin = new Padding(0);
            lbHeader4.Name = "lbHeader4";
            lbHeader4.Size = new Size(64, 40);
            lbHeader4.TabIndex = 5;
            lbHeader4.Text = "v2.0";
            lbHeader4.TextAlign = ContentAlignment.BottomLeft;
            lbHeader4.UseCompatibleTextRendering = true;
            // 
            // plNordanHeaderLogo
            // 
            plNordanHeaderLogo.BackgroundImage = (Image)resources.GetObject("plNordanHeaderLogo.BackgroundImage");
            plNordanHeaderLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plNordanHeaderLogo.Dock = DockStyle.Top;
            plNordanHeaderLogo.Location = new Point(4, 4);
            plNordanHeaderLogo.Margin = new Padding(0);
            plNordanHeaderLogo.Name = "plNordanHeaderLogo";
            tplHeader.SetRowSpan(plNordanHeaderLogo, 2);
            plNordanHeaderLogo.Size = new Size(200, 98);
            plNordanHeaderLogo.TabIndex = 2;
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
            lbHeader3.Margin = new Padding(0);
            lbHeader3.Name = "lbHeader3";
            tplHeader.SetRowSpan(lbHeader3, 2);
            lbHeader3.Size = new Size(206, 66);
            lbHeader3.TabIndex = 4;
            lbHeader3.Text = "PrefSuite";
            lbHeader3.UseCompatibleTextRendering = true;
            // 
            // plUniwaveHeaderLogo
            // 
            plUniwaveHeaderLogo.BackgroundImage = (Image)resources.GetObject("plUniwaveHeaderLogo.BackgroundImage");
            plUniwaveHeaderLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plUniwaveHeaderLogo.Dock = DockStyle.Top;
            plUniwaveHeaderLogo.Location = new Point(1864, 4);
            plUniwaveHeaderLogo.Margin = new Padding(0);
            plUniwaveHeaderLogo.Name = "plUniwaveHeaderLogo";
            tplHeader.SetRowSpan(plUniwaveHeaderLogo, 2);
            plUniwaveHeaderLogo.Size = new Size(204, 98);
            plUniwaveHeaderLogo.TabIndex = 1;
            // 
            // lbHeader2
            // 
            lbHeader2.BackColor = Color.Transparent;
            lbHeader2.Dock = DockStyle.Fill;
            lbHeader2.FlatStyle = FlatStyle.Flat;
            lbHeader2.Font = new Font("Segoe UI Black", 30F, FontStyle.Bold);
            lbHeader2.ForeColor = Color.FromArgb(248, 248, 249);
            lbHeader2.Location = new Point(901, 44);
            lbHeader2.Margin = new Padding(0);
            lbHeader2.Name = "lbHeader2";
            tplHeader.SetRowSpan(lbHeader2, 2);
            lbHeader2.Size = new Size(60, 80);
            lbHeader2.TabIndex = 7;
            lbHeader2.Text = "2";
            lbHeader2.TextAlign = ContentAlignment.MiddleCenter;
            lbHeader2.UseCompatibleTextRendering = true;
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
            lbHeader1.Margin = new Padding(0);
            lbHeader1.Name = "lbHeader1";
            tplHeader.SetRowSpan(lbHeader1, 3);
            lbHeader1.Size = new Size(697, 120);
            lbHeader1.TabIndex = 6;
            lbHeader1.Text = "Aluminum";
            lbHeader1.TextAlign = ContentAlignment.MiddleRight;
            lbHeader1.UseCompatibleTextRendering = true;
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
            tplHeader.Location = new Point(0, 58);
            tplHeader.Margin = new Padding(4, 4, 4, 4);
            tplHeader.Name = "tplHeader";
            tplHeader.Padding = new Padding(4, 4, 4, 4);
            tplHeader.RowCount = 3;
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tplHeader.Size = new Size(2072, 128);
            tplHeader.TabIndex = 1;
            // 
            // plSBButtons
            // 
            plSBButtons.AutoSize = true;
            plSBButtons.BackColor = Color.Transparent;
            plSBButtons.Dock = DockStyle.Top;
            plSBButtons.Location = new Point(0, 0);
            plSBButtons.Margin = new Padding(6, 6, 6, 6);
            plSBButtons.Name = "plSBButtons";
            plSBButtons.Size = new Size(400, 0);
            plSBButtons.TabIndex = 11;
            // 
            // plTbSBInfo
            // 
            plTbSBInfo.BackColor = Color.Transparent;
            plTbSBInfo.ColumnCount = 2;
            plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
            plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            plTbSBInfo.Controls.Add(lbInfoErrors, 0, 6);
            plTbSBInfo.Controls.Add(lbInfoFiles, 0, 0);
            plTbSBInfo.Controls.Add(lbErrorCount, 1, 6);
            plTbSBInfo.Controls.Add(lbWarningCount, 1, 5);
            plTbSBInfo.Controls.Add(lbInfoWarnings, 0, 5);
            plTbSBInfo.Controls.Add(rowsCount, 1, 3);
            plTbSBInfo.Controls.Add(lbWorksheetsCount, 1, 2);
            plTbSBInfo.Controls.Add(lbOrdersCount, 1, 1);
            plTbSBInfo.Controls.Add(lbFilesCount, 1, 0);
            plTbSBInfo.Controls.Add(lbInfoRows, 0, 3);
            plTbSBInfo.Controls.Add(lbInfoWorksheets, 0, 2);
            plTbSBInfo.Controls.Add(lbInfoOrders, 0, 1);
            plTbSBInfo.Dock = DockStyle.Bottom;
            plTbSBInfo.ForeColor = Color.FromArgb(248, 248, 249);
            plTbSBInfo.Location = new Point(0, 636);
            plTbSBInfo.Margin = new Padding(4, 2, 4, 2);
            plTbSBInfo.Name = "plTbSBInfo";
            plTbSBInfo.RowCount = 8;
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            plTbSBInfo.Size = new Size(400, 452);
            plTbSBInfo.TabIndex = 12;
            // 
            // lbInfoErrors
            // 
            lbInfoErrors.AutoSize = true;
            lbInfoErrors.Dock = DockStyle.Fill;
            lbInfoErrors.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoErrors.ForeColor = Color.Crimson;
            lbInfoErrors.Location = new Point(8, 344);
            lbInfoErrors.Margin = new Padding(8, 8, 8, 8);
            lbInfoErrors.Name = "lbInfoErrors";
            lbInfoErrors.Size = new Size(160, 40);
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
            lbInfoFiles.Margin = new Padding(8, 8, 8, 8);
            lbInfoFiles.Name = "lbInfoFiles";
            lbInfoFiles.Size = new Size(160, 40);
            lbInfoFiles.TabIndex = 4;
            lbInfoFiles.Text = "Files:";
            lbInfoFiles.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbErrorCount
            // 
            lbErrorCount.AutoSize = true;
            lbErrorCount.Dock = DockStyle.Fill;
            lbErrorCount.Font = new Font("Segoe UI", 9F);
            lbErrorCount.ForeColor = Color.Red;
            lbErrorCount.Location = new Point(184, 344);
            lbErrorCount.Margin = new Padding(8, 8, 8, 8);
            lbErrorCount.Name = "lbErrorCount";
            lbErrorCount.Size = new Size(208, 40);
            lbErrorCount.TabIndex = 0;
            lbErrorCount.Text = "0";
            lbErrorCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbWarningCount
            // 
            lbWarningCount.AutoSize = true;
            lbWarningCount.Dock = DockStyle.Fill;
            lbWarningCount.Font = new Font("Segoe UI", 9F);
            lbWarningCount.ForeColor = Color.Coral;
            lbWarningCount.Location = new Point(184, 288);
            lbWarningCount.Margin = new Padding(8, 8, 8, 8);
            lbWarningCount.Name = "lbWarningCount";
            lbWarningCount.Size = new Size(208, 40);
            lbWarningCount.TabIndex = 0;
            lbWarningCount.Text = "0";
            lbWarningCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWarnings
            // 
            lbInfoWarnings.AutoSize = true;
            lbInfoWarnings.Dock = DockStyle.Fill;
            lbInfoWarnings.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoWarnings.ForeColor = Color.Coral;
            lbInfoWarnings.Location = new Point(8, 288);
            lbInfoWarnings.Margin = new Padding(8, 8, 8, 8);
            lbInfoWarnings.Name = "lbInfoWarnings";
            lbInfoWarnings.Size = new Size(160, 40);
            lbInfoWarnings.TabIndex = 0;
            lbInfoWarnings.Text = "Warnings:";
            lbInfoWarnings.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // rowsCount
            // 
            rowsCount.AutoSize = true;
            rowsCount.Dock = DockStyle.Fill;
            rowsCount.Font = new Font("Segoe UI", 9F);
            rowsCount.ForeColor = SystemColors.ScrollBar;
            rowsCount.Location = new Point(184, 176);
            rowsCount.Margin = new Padding(8, 8, 8, 8);
            rowsCount.Name = "rowsCount";
            rowsCount.Size = new Size(208, 40);
            rowsCount.TabIndex = 0;
            rowsCount.Text = "10";
            rowsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbWorksheetsCount
            // 
            lbWorksheetsCount.AutoSize = true;
            lbWorksheetsCount.Dock = DockStyle.Fill;
            lbWorksheetsCount.Font = new Font("Segoe UI", 9F);
            lbWorksheetsCount.ForeColor = SystemColors.ScrollBar;
            lbWorksheetsCount.Location = new Point(184, 120);
            lbWorksheetsCount.Margin = new Padding(8, 8, 8, 8);
            lbWorksheetsCount.Name = "lbWorksheetsCount";
            lbWorksheetsCount.Size = new Size(208, 40);
            lbWorksheetsCount.TabIndex = 0;
            lbWorksheetsCount.Text = "10";
            lbWorksheetsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbOrdersCount
            // 
            lbOrdersCount.AutoSize = true;
            lbOrdersCount.Dock = DockStyle.Fill;
            lbOrdersCount.Font = new Font("Segoe UI", 9F);
            lbOrdersCount.ForeColor = SystemColors.ScrollBar;
            lbOrdersCount.Location = new Point(184, 64);
            lbOrdersCount.Margin = new Padding(8, 8, 8, 8);
            lbOrdersCount.Name = "lbOrdersCount";
            lbOrdersCount.Size = new Size(208, 40);
            lbOrdersCount.TabIndex = 0;
            lbOrdersCount.Text = "10";
            lbOrdersCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbFilesCount
            // 
            lbFilesCount.AutoSize = true;
            lbFilesCount.BackColor = UniwaveColors.a2pGreyDark;
            lbFilesCount.Dock = DockStyle.Fill;
            lbFilesCount.FlatStyle = FlatStyle.System;
            lbFilesCount.Font = new Font("Segoe UI", 9F);
            lbFilesCount.ForeColor = Color.FromArgb(248, 248, 249);
            lbFilesCount.Location = new Point(184, 8);
            lbFilesCount.Margin = new Padding(8, 8, 8, 8);
            lbFilesCount.Name = "lbFilesCount";
            lbFilesCount.Size = new Size(208, 40);
            lbFilesCount.TabIndex = 0;
            lbFilesCount.Text = "10";
            lbFilesCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoRows
            // 
            lbInfoRows.AutoSize = true;
            lbInfoRows.Dock = DockStyle.Fill;
            lbInfoRows.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoRows.ForeColor = SystemColors.ScrollBar;
            lbInfoRows.Location = new Point(8, 176);
            lbInfoRows.Margin = new Padding(8, 8, 8, 8);
            lbInfoRows.Name = "lbInfoRows";
            lbInfoRows.Size = new Size(160, 40);
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
            lbInfoWorksheets.Margin = new Padding(8, 8, 8, 8);
            lbInfoWorksheets.Name = "lbInfoWorksheets";
            lbInfoWorksheets.Size = new Size(160, 40);
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
            lbInfoOrders.Margin = new Padding(8, 8, 8, 8);
            lbInfoOrders.Name = "lbInfoOrders";
            lbInfoOrders.Size = new Size(160, 40);
            lbInfoOrders.TabIndex = 3;
            lbInfoOrders.Text = "Orders:";
            lbInfoOrders.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(btSideBar, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(6, 6, 6, 6);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(400, 80);
            tableLayoutPanel1.TabIndex = 43;
            // 
            // btSideBar
            // 
            btSideBar.BackColor = Color.Transparent;
            btSideBar.BackgroundImage = (Image)resources.GetObject("btSideBar.BackgroundImage");
            btSideBar.BackgroundImageLayout = ImageLayout.Center;
            btSideBar.Dock = DockStyle.Fill;
            btSideBar.FlatAppearance.BorderColor =UniwaveColors.a2pGreyDark;
            btSideBar.FlatAppearance.BorderSize = 0;
            btSideBar.FlatStyle = FlatStyle.Flat;
            btSideBar.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btSideBar.ForeColor = Color.FromArgb(239, 112, 32);
            btSideBar.ImageAlign = ContentAlignment.MiddleLeft;
            btSideBar.Location = new Point(0, 0);
            btSideBar.Margin = new Padding(0);
            btSideBar.Name = "btSideBar";
            btSideBar.SelectedOne = false;
            btSideBar.Size = new Size(80, 80);
            btSideBar.TabIndex = 22;
            btSideBar.TextAlign = ContentAlignment.MiddleLeft;
            btSideBar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btSideBar.UseVisualStyleBackColor = false;
            btSideBar.Click += btSideBar_Click;
            // 
            // btLoadFiles
            // 
            btLoadFiles.BackColor = Color.Transparent;
            btLoadFiles.Dock = DockStyle.Top;
            btLoadFiles.FlatAppearance.BorderColor =UniwaveColors.a2pGreyDark;
            btLoadFiles.FlatAppearance.BorderSize = 0;
            btLoadFiles.FlatStyle = FlatStyle.Flat;
            btLoadFiles.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btLoadFiles.ForeColor = Color.FromArgb(239, 112, 32);
            btLoadFiles.Image = (Image)resources.GetObject("btLoadFiles.Image");
            btLoadFiles.ImageAlign = ContentAlignment.MiddleLeft;
            btLoadFiles.Location = new Point(0, 80);
            btLoadFiles.Margin = new Padding(0);
            btLoadFiles.Name = "btLoadFiles";
            btLoadFiles.Padding = new Padding(24, 0, 0, 0);
            btLoadFiles.SelectedOne = false;
            btLoadFiles.Size = new Size(400, 80);
            btLoadFiles.TabIndex = 44;
            btLoadFiles.Text = "  Load &Files";
            btLoadFiles.TextAlign = ContentAlignment.MiddleLeft;
            btLoadFiles.TextImageRelation = TextImageRelation.ImageBeforeText;
            btLoadFiles.UseVisualStyleBackColor = false;
            btLoadFiles.Click += BtFilesRefresh_Click;
            // 
            // btImport
            // 
            btImport.BackColor = Color.Transparent;
            btImport.Dock = DockStyle.Top;
            btImport.FlatAppearance.BorderColor =UniwaveColors.a2pGreyDark;
            btImport.FlatAppearance.BorderSize = 0;
            btImport.FlatStyle = FlatStyle.Flat;
            btImport.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btImport.ForeColor = Color.FromArgb(239, 112, 32);
            btImport.Image = (Image)resources.GetObject("btImport.Image");
            btImport.ImageAlign = ContentAlignment.MiddleLeft;
            btImport.Location = new Point(0, 160);
            btImport.Margin = new Padding(0);
            btImport.Name = "btImport";
            btImport.Padding = new Padding(24, 0, 0, 0);
            btImport.SelectedOne = false;
            btImport.Size = new Size(400, 80);
            btImport.TabIndex = 45;
            btImport.Text = "  &Import";
            btImport.TextAlign = ContentAlignment.MiddleLeft;
            btImport.TextImageRelation = TextImageRelation.ImageBeforeText;
            btImport.UseVisualStyleBackColor = false;
            btImport.Click += BtFilesImport_Click;
            // 
            // btLog
            // 
            btLog.BackColor = Color.Transparent;
            btLog.Dock = DockStyle.Top;
            btLog.FlatAppearance.BorderColor =UniwaveColors.a2pGreyDark;
            btLog.FlatAppearance.BorderSize = 0;
            btLog.FlatStyle = FlatStyle.Flat;
            btLog.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btLog.ForeColor = Color.FromArgb(239, 112, 32);
            btLog.Image = (Image)resources.GetObject("btLog.Image");
            btLog.ImageAlign = ContentAlignment.MiddleLeft;
            btLog.Location = new Point(0, 240);
            btLog.Margin = new Padding(0);
            btLog.Name = "btLog";
            btLog.Padding = new Padding(24, 0, 0, 0);
            btLog.SelectedOne = false;
            btLog.Size = new Size(400, 80);
            btLog.TabIndex = 46;
            btLog.Text = "  &Log";
            btLog.TextAlign = ContentAlignment.MiddleLeft;
            btLog.TextImageRelation = TextImageRelation.ImageBeforeText;
            btLog.UseVisualStyleBackColor = true;
            btLog.Click += BtLogRefresh_Click;
            // 
            // btProperties
            // 
            btProperties.BackColor = Color.Transparent;
            btProperties.Dock = DockStyle.Top;
            btProperties.FlatAppearance.BorderColor =UniwaveColors.a2pGreyDark;
            btProperties.FlatAppearance.BorderSize = 0;
            btProperties.FlatStyle = FlatStyle.Flat;
            btProperties.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btProperties.ForeColor = Color.FromArgb(239, 112, 32);
            btProperties.Image = (Image)resources.GetObject("btProperties.Image");
            btProperties.ImageAlign = ContentAlignment.MiddleLeft;
            btProperties.Location = new Point(0, 320);
            btProperties.Margin = new Padding(0);
            btProperties.Name = "btProperties";
            btProperties.Padding = new Padding(24, 0, 0, 0);
            btProperties.SelectedOne = false;
            btProperties.Size = new Size(400, 80);
            btProperties.TabIndex = 47;
            btProperties.Text = "  &Properties";
            btProperties.TextAlign = ContentAlignment.MiddleLeft;
            btProperties.TextImageRelation = TextImageRelation.ImageBeforeText;
            btProperties.UseVisualStyleBackColor = false;
            btProperties.Click += BtSettings_Click;
            // 
            // btExit
            // 
            btExit.BackColor = Color.Transparent;
            btExit.Dock = DockStyle.Top;
            btExit.FlatAppearance.BorderColor =UniwaveColors.a2pGreyDark;
            btExit.FlatAppearance.BorderSize = 0;
            btExit.FlatStyle = FlatStyle.Flat;
            btExit.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btExit.ForeColor = Color.FromArgb(239, 112, 32);
            btExit.Image = (Image)resources.GetObject("btExit.Image");
            btExit.ImageAlign = ContentAlignment.MiddleLeft;
            btExit.Location = new Point(0, 400);
            btExit.Margin = new Padding(0);
            btExit.Name = "btExit";
            btExit.Padding = new Padding(24, 0, 0, 0);
            btExit.SelectedOne = false;
            btExit.Size = new Size(400, 80);
            btExit.TabIndex = 48;
            btExit.Text = "  E&xit";
            btExit.TextAlign = ContentAlignment.MiddleLeft;
            btExit.TextImageRelation = TextImageRelation.ImageBeforeText;
            btExit.UseVisualStyleBackColor = false;
            btExit.Click += BtClose_Click;
            // 
            // plSideBarMain
            // 
            plSideBarMain.BackColor = Color.Transparent;
            plSideBarMain.Controls.Add(btExit);
            plSideBarMain.Controls.Add(btProperties);
            plSideBarMain.Controls.Add(btLog);
            plSideBarMain.Controls.Add(btImport);
            plSideBarMain.Controls.Add(btLoadFiles);
            plSideBarMain.Controls.Add(tableLayoutPanel1);
            plSideBarMain.Controls.Add(plTbSBInfo);
            plSideBarMain.Controls.Add(plSBButtons);
            plSideBarMain.Dock = DockStyle.Left;
            plSideBarMain.ForeColor = Color.Transparent;
            plSideBarMain.Location = new Point(0, 186);
            plSideBarMain.Margin = new Padding(0);
            plSideBarMain.Name = "plSideBarMain";
            plSideBarMain.Size = new Size(400, 1088);
            plSideBarMain.TabIndex = 14;
            // 
            // tlpTitleBar
            // 
            tlpTitleBar.AutoSize = true;
            tlpTitleBar.BackColor = UniwaveColors.a2pGreyDark;
            tlpTitleBar.ColumnCount = 5;
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle());
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle());
            tlpTitleBar.ColumnStyles.Add(new ColumnStyle());
            tlpTitleBar.Controls.Add(btMinimize, 2, 0);
            tlpTitleBar.Controls.Add(btMaximize, 3, 0);
            tlpTitleBar.Controls.Add(btClose, 4, 0);
            tlpTitleBar.Controls.Add(plMiniLogo, 0, 0);
            tlpTitleBar.Controls.Add(plTitleBar, 1, 0);
            tlpTitleBar.Dock = DockStyle.Top;
            tlpTitleBar.Location = new Point(0, 0);
            tlpTitleBar.Margin = new Padding(12, 12, 12, 12);
            tlpTitleBar.MinimumSize = new Size(30, 30);
            tlpTitleBar.Name = "tlpTitleBar";
            tlpTitleBar.RowCount = 1;
            tlpTitleBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTitleBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            tlpTitleBar.Size = new Size(2072, 58);
            tlpTitleBar.TabIndex = 0;
            // 
            // btMinimize
            // 
            btMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btMinimize.AutoSize = true;
            btMinimize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btMinimize.BackColor = Color.Transparent;
            btMinimize.BackgroundImage = (Image)resources.GetObject("btMinimize.BackgroundImage");
            btMinimize.BackgroundImageLayout = ImageLayout.Zoom;
            btMinimize.FlatAppearance.BorderSize = 0;
            btMinimize.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btMinimize.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btMinimize.FlatStyle = FlatStyle.Flat;
            btMinimize.ForeColor = Color.Transparent;
            btMinimize.Location = new Point(1942, 8);
            btMinimize.Margin = new Padding(8, 8, 8, 8);
            btMinimize.MaximumSize = new Size(50, 50);
            btMinimize.MinimumSize = new Size(30, 30);
            btMinimize.Name = "btMinimize";
            btMinimize.Size = new Size(30, 42);
            btMinimize.TabIndex = 1;
            btMinimize.UseVisualStyleBackColor = false;
            btMinimize.Click += btMinimize_Click;
            btMinimize.MouseEnter += btMinimize_MouseEnter;
            btMinimize.MouseLeave += btMinimize_MouseLeave;
            // 
            // btMaximize
            // 
            btMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btMaximize.AutoSize = true;
            btMaximize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btMaximize.BackColor = Color.Transparent;
            btMaximize.BackgroundImage = (Image)resources.GetObject("btMaximize.BackgroundImage");
            btMaximize.BackgroundImageLayout = ImageLayout.Zoom;
            btMaximize.FlatAppearance.BorderSize = 0;
            btMaximize.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btMaximize.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btMaximize.FlatStyle = FlatStyle.Flat;
            btMaximize.ForeColor = Color.Transparent;
            btMaximize.Location = new Point(1988, 8);
            btMaximize.Margin = new Padding(8, 8, 8, 8);
            btMaximize.MaximumSize = new Size(50, 50);
            btMaximize.MinimumSize = new Size(30, 30);
            btMaximize.Name = "btMaximize";
            btMaximize.Size = new Size(30, 42);
            btMaximize.TabIndex = 1;
            btMaximize.UseVisualStyleBackColor = false;
            btMaximize.Click += btMaximize_Click;
            btMaximize.MouseEnter += btMaximize_MouseEnter;
            btMaximize.MouseLeave += btMaximize_MouseLeave;
            // 
            // btClose
            // 
            btClose.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btClose.AutoSize = true;
            btClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btClose.BackColor = Color.Transparent;
            btClose.BackgroundImage = (Image)resources.GetObject("btClose.BackgroundImage");
            btClose.BackgroundImageLayout = ImageLayout.Zoom;
            btClose.FlatAppearance.BorderSize = 0;
            btClose.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btClose.FlatStyle = FlatStyle.Flat;
            btClose.Font = new Font("Segoe UI", 8.75F, FontStyle.Bold);
            btClose.ForeColor = Color.Transparent;
            btClose.ImageAlign = ContentAlignment.MiddleLeft;
            btClose.Location = new Point(2034, 8);
            btClose.Margin = new Padding(8, 8, 8, 8);
            btClose.MaximumSize = new Size(50, 50);
            btClose.MinimumSize = new Size(30, 30);
            btClose.Name = "btClose";
            btClose.SelectedOne = true;
            btClose.Size = new Size(30, 42);
            btClose.TabIndex = 2;
            btClose.TextAlign = ContentAlignment.MiddleLeft;
            btClose.TextImageRelation = TextImageRelation.ImageBeforeText;
            btClose.UseVisualStyleBackColor = false;
            btClose.Click += BtClose_Click;
            btClose.MouseEnter += btClose_MouseEnter;
            btClose.MouseLeave += btClose_MouseLeave;
            // 
            // plMiniLogo
            // 
            plMiniLogo.BackgroundImage = (Image)resources.GetObject("plMiniLogo.BackgroundImage");
            plMiniLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plMiniLogo.Dock = DockStyle.Fill;
            plMiniLogo.Location = new Point(6, 6);
            plMiniLogo.Margin = new Padding(6, 6, 6, 6);
            plMiniLogo.Name = "plMiniLogo";
            plMiniLogo.Size = new Size(38, 46);
            plMiniLogo.TabIndex = 3;
            // 
            // plTitleBar
            // 
            plTitleBar.BackColor = Color.Transparent;
            plTitleBar.Controls.Add(plTitleBarAppName);
            plTitleBar.Dock = DockStyle.Fill;
            plTitleBar.ForeColor = Color.FromArgb(239, 112, 32);
            plTitleBar.Location = new Point(56, 6);
            plTitleBar.Margin = new Padding(6, 6, 6, 6);
            plTitleBar.Name = "plTitleBar";
            plTitleBar.Size = new Size(1872, 46);
            plTitleBar.TabIndex = 4;
            plTitleBar.Text = "Alu 2 PrefSuite v2.0";
            plTitleBar.MouseDown += PlTitleBar_MouseDown;
            // 
            // plTitleBarAppName
            // 
            plTitleBarAppName.AutoSize = true;
            plTitleBarAppName.Enabled = false;
            plTitleBarAppName.FlatStyle = FlatStyle.Flat;
            plTitleBarAppName.ForeColor = Color.FromArgb(248, 248, 249);
            plTitleBarAppName.Location = new Point(6, 2);
            plTitleBarAppName.Margin = new Padding(6, 0, 6, 0);
            plTitleBarAppName.Name = "plTitleBarAppName";
            plTitleBarAppName.Size = new Size(255, 32);
            plTitleBarAppName.TabIndex = 0;
            plTitleBarAppName.Text = "Aluminum 2 PrefSuite ";
            // 
            // plTBPanel
            // 
            plTBPanel.AutoSize = true;
            plTBPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            plTBPanel.BackColor = Color.FromArgb(239, 112, 32);
            plTBPanel.Controls.Add(tlpTitleBar);
            plTBPanel.Dock = DockStyle.Top;
            plTBPanel.Location = new Point(0, 0);
            plTBPanel.Margin = new Padding(0);
            plTBPanel.Name = "plTBPanel";
            plTBPanel.Size = new Size(2072, 58);
            plTBPanel.TabIndex = 8;
            // 
            // lbErrors
            // 
            lbErrors.AutoSize = true;
            lbErrors.Dock = DockStyle.Fill;
            lbErrors.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbErrors.ForeColor = Color.Red;
            lbErrors.Location = new Point(4, 200);
            lbErrors.Margin = new Padding(4);
            lbErrors.Name = "lbErrors";
            lbErrors.Size = new Size(80, 20);
            lbErrors.TabIndex = 0;
            lbErrors.Text = "Errors:";
            lbErrors.TextAlign = ContentAlignment.MiddleLeft;
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
            // plFormContainer
            // 
            plFormContainer.AutoScroll = true;
            plFormContainer.BackColor = Color.Transparent;
            plFormContainer.Dock = DockStyle.Fill;
            plFormContainer.ForeColor = Color.Transparent;
            plFormContainer.Location = new Point(400, 186);
            plFormContainer.Margin = new Padding(0);
            plFormContainer.Name = "plFormContainer";
            plFormContainer.Size = new Size(1672, 1088);
            plFormContainer.TabIndex = 15;
            // 
            // MainForm
            // 
            AcceptButton = btProperties;
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoValidate = AutoValidate.Disable;
            BackColor = UniwaveColors.a2pGreyDark;
            ClientSize = new Size(2072, 1316);
            Controls.Add(plFormContainer);
            Controls.Add(plSideBarMain);
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
            plTbSBInfo.ResumeLayout(false);
            plTbSBInfo.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            plSideBarMain.ResumeLayout(false);
            plSideBarMain.PerformLayout();
            tlpTitleBar.ResumeLayout(false);
            tlpTitleBar.PerformLayout();
            plTitleBar.ResumeLayout(false);
            plTitleBar.PerformLayout();
            plTBPanel.ResumeLayout(false);
            plTBPanel.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbHeader4;
        private Panel plNordanHeaderLogo;
        private TableLayoutPanel tplHeader;
        private Label lbHeader1;
        private Label lbHeader2;
        private Panel plUniwaveHeaderLogo;
        private Label lbHeader3;
        private Panel plSBButtons;
        private TableLayoutPanel plTbSBInfo;
        private Label lbInfoErrors;
        private Label lbInfoFiles;
        private Label lbErrorCount;
        private Label lbWarningCount;
        private Label lbInfoWarnings;
        private Label rowsCount;
        private Label lbWorksheetsCount;
        private Label lbOrdersCount;
        private Label lbFilesCount;
        private Label lbInfoRows;
        private Label lbInfoWorksheets;
        private Label lbInfoOrders;
        private TableLayoutPanel tableLayoutPanel1;
        private SideBarButton btSideBar;
        private SideBarButton btLoadFiles;
        private SideBarButton btImport;
        private SideBarButton btLog;
        private SideBarButton btProperties;
        private SideBarButton btExit;
        private Panel plSideBarMain;
        private TableLayoutPanel tlpTitleBar;
        private Button btMinimize;
        private Button btMaximize;
        private SideBarButton btClose;
        private Panel plMiniLogo;
        private Panel plTitleBar;
        private Label plTitleBarAppName;
        private Panel plTBPanel;
        private Label lbErrors;
        private ToolStripStatusLabel slbPath;
        private StatusStrip statusStrip;
        private Panel plFormContainer;
    }
}
