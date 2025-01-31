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
            this.lbHeader4=new Label();
            this.plNordanHeaderLogo=new Panel();
            this.lbHeader3=new Label();
            this.plUniwaveHeaderLogo=new Panel();
            this.lbHeader2=new Label();
            this.lbHeader1=new Label();
            this.tplHeader=new TableLayoutPanel();
            this.plSBButtons=new Panel();
            this.plTbSBInfo=new TableLayoutPanel();
            this.lbInfoErrors=new Label();
            this.lbInfoFiles=new Label();
            this.lbErrorCount=new Label();
            this.lbWarningCount=new Label();
            this.lbInfoWarnings=new Label();
            this.rowsCount=new Label();
            this.lbWorksheetsCount=new Label();
            this.lbOrdersCount=new Label();
            this.lbFilesCount=new Label();
            this.lbInfoRows=new Label();
            this.lbInfoWorksheets=new Label();
            this.lbInfoOrders=new Label();
            this.tableLayoutPanel1=new TableLayoutPanel();
            this.btSideBar=new SideBarButton();
            this.btLoadFiles=new SideBarButton();
            this.btImport=new SideBarButton();
            this.btLog=new SideBarButton();
            this.btProperties=new SideBarButton();
            this.btExit=new SideBarButton();
            this.plSideBarMain=new Panel();
            this.tlpTitleBar=new TableLayoutPanel();
            this.btMinimize=new Button();
            this.btMaximize=new Button();
            this.btClose=new SideBarButton();
            this.plMiniLogo=new Panel();
            this.plTitleBar=new Panel();
            this.plTitleBarAppName=new Label();
            this.plTBPanel=new Panel();
            this.lbErrors=new Label();
            this.slbPath=new ToolStripStatusLabel();
            this.statusStrip=new StatusStrip();
            this.plFormContainer=new Panel();
            toolStripStatusLabel1=new ToolStripStatusLabel();
            this.tplHeader.SuspendLayout();
            this.plTbSBInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.plSideBarMain.SuspendLayout();
            this.tlpTitleBar.SuspendLayout();
            this.plTitleBar.SuspendLayout();
            this.plTBPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name="toolStripStatusLabel1";
            toolStripStatusLabel1.Size=new Size(37, 17);
            toolStripStatusLabel1.Text="Path: ";
            toolStripStatusLabel1.TextAlign=ContentAlignment.MiddleLeft;
            // 
            // lbHeader4
            // 
            this.lbHeader4.Anchor=AnchorStyles.Bottom|AnchorStyles.Left;
            this.lbHeader4.AutoSize=true;
            this.lbHeader4.BackColor=Color.Transparent;
            this.lbHeader4.Font=new Font("Segoe UI", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 10, true);
            this.lbHeader4.ForeColor=Color.Transparent;
            this.lbHeader4.ImageAlign=ContentAlignment.TopLeft;
            this.lbHeader4.Location=new Point(583, 2);
            this.lbHeader4.Margin=new Padding(0);
            this.lbHeader4.Name="lbHeader4";
            this.lbHeader4.Size=new Size(32, 20);
            this.lbHeader4.TabIndex=5;
            this.lbHeader4.Text="v2.0";
            this.lbHeader4.TextAlign=ContentAlignment.BottomLeft;
            this.lbHeader4.UseCompatibleTextRendering=true;
            // 
            // plNordanHeaderLogo
            // 
            this.plNordanHeaderLogo.BackgroundImage=(Image)resources.GetObject("plNordanHeaderLogo.BackgroundImage");
            this.plNordanHeaderLogo.BackgroundImageLayout=ImageLayout.Zoom;
            this.plNordanHeaderLogo.Dock=DockStyle.Top;
            this.plNordanHeaderLogo.Location=new Point(2, 2);
            this.plNordanHeaderLogo.Margin=new Padding(0);
            this.plNordanHeaderLogo.Name="plNordanHeaderLogo";
            this.tplHeader.SetRowSpan(this.plNordanHeaderLogo, 2);
            this.plNordanHeaderLogo.Size=new Size(100, 49);
            this.plNordanHeaderLogo.TabIndex=2;
            // 
            // lbHeader3
            // 
            this.lbHeader3.AutoSize=true;
            this.lbHeader3.BackColor=Color.Transparent;
            this.lbHeader3.Dock=DockStyle.Bottom;
            this.lbHeader3.Font=new Font("Segoe UI", 16.125F, FontStyle.Bold);
            this.lbHeader3.ForeColor=UniwaveColors.uwGreyLight;
            this.lbHeader3.ImageAlign=ContentAlignment.TopLeft;
            this.lbHeader3.Location=new Point(480, 16);
            this.lbHeader3.Margin=new Padding(0);
            this.lbHeader3.Name="lbHeader3";
            this.tplHeader.SetRowSpan(this.lbHeader3, 2);
            this.lbHeader3.Size=new Size(103, 35);
            this.lbHeader3.TabIndex=4;
            this.lbHeader3.Text="PrefSuite";
            this.lbHeader3.UseCompatibleTextRendering=true;
            // 
            // plUniwaveHeaderLogo
            // 
            this.plUniwaveHeaderLogo.BackgroundImage=(Image)resources.GetObject("plUniwaveHeaderLogo.BackgroundImage");
            this.plUniwaveHeaderLogo.BackgroundImageLayout=ImageLayout.Zoom;
            this.plUniwaveHeaderLogo.Dock=DockStyle.Top;
            this.plUniwaveHeaderLogo.Location=new Point(931, 2);
            this.plUniwaveHeaderLogo.Margin=new Padding(0);
            this.plUniwaveHeaderLogo.Name="plUniwaveHeaderLogo";
            this.tplHeader.SetRowSpan(this.plUniwaveHeaderLogo, 2);
            this.plUniwaveHeaderLogo.Size=new Size(103, 49);
            this.plUniwaveHeaderLogo.TabIndex=1;
            // 
            // lbHeader2
            // 
            this.lbHeader2.BackColor=Color.Transparent;
            this.lbHeader2.Dock=DockStyle.Fill;
            this.lbHeader2.FlatStyle=FlatStyle.Flat;
            this.lbHeader2.Font=new Font("Segoe UI Black", 30F, FontStyle.Bold);
            this.lbHeader2.ForeColor=UniwaveColors.uwGreyLight;
            this.lbHeader2.Location=new Point(450, 22);
            this.lbHeader2.Margin=new Padding(0);
            this.lbHeader2.Name="lbHeader2";
            this.tplHeader.SetRowSpan(this.lbHeader2, 2);
            this.lbHeader2.Size=new Size(30, 40);
            this.lbHeader2.TabIndex=7;
            this.lbHeader2.Text="2";
            this.lbHeader2.TextAlign=ContentAlignment.MiddleCenter;
            this.lbHeader2.UseCompatibleTextRendering=true;
            // 
            // lbHeader1
            // 
            this.lbHeader1.AutoSize=true;
            this.lbHeader1.BackColor=Color.Transparent;
            this.lbHeader1.Dock=DockStyle.Fill;
            this.lbHeader1.Font=new Font("Segoe UI", 20F);
            this.lbHeader1.ForeColor=UniwaveColors.uwGreyLight;
            this.lbHeader1.ImageAlign=ContentAlignment.TopRight;
            this.lbHeader1.Location=new Point(102, 2);
            this.lbHeader1.Margin=new Padding(0);
            this.lbHeader1.Name="lbHeader1";
            this.tplHeader.SetRowSpan(this.lbHeader1, 3);
            this.lbHeader1.Size=new Size(348, 60);
            this.lbHeader1.TabIndex=6;
            this.lbHeader1.Text="Aluminum";
            this.lbHeader1.TextAlign=ContentAlignment.MiddleRight;
            this.lbHeader1.UseCompatibleTextRendering=true;
            // 
            // tplHeader
            // 
            this.tplHeader.AutoSize=true;
            this.tplHeader.AutoSizeMode=AutoSizeMode.GrowAndShrink;
            this.tplHeader.BackColor=UniwaveColors.uwOrangeDeep;
            this.tplHeader.ColumnCount=7;
            this.tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            this.tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            this.tplHeader.ColumnStyles.Add(new ColumnStyle());
            this.tplHeader.ColumnStyles.Add(new ColumnStyle());
            this.tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 102F));
            this.tplHeader.Controls.Add(this.lbHeader1, 1, 0);
            this.tplHeader.Controls.Add(this.lbHeader2, 2, 1);
            this.tplHeader.Controls.Add(this.plUniwaveHeaderLogo, 7, 0);
            this.tplHeader.Controls.Add(this.lbHeader3, 3, 0);
            this.tplHeader.Controls.Add(this.plNordanHeaderLogo, 0, 0);
            this.tplHeader.Controls.Add(this.lbHeader4, 5, 0);
            this.tplHeader.Dock=DockStyle.Top;
            this.tplHeader.Location=new Point(0, 29);
            this.tplHeader.Margin=new Padding(2);
            this.tplHeader.Name="tplHeader";
            this.tplHeader.Padding=new Padding(2);
            this.tplHeader.RowCount=3;
            this.tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            this.tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            this.tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 11F));
            this.tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            this.tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            this.tplHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
          
            this.tplHeader.Size=new Size(1036, 64);
            this.tplHeader.TabIndex=1;
            // 
            // plSBButtons
            // 
            this.plSBButtons.AutoSize=true;
            this.plSBButtons.BackColor=Color.Transparent;
            this.plSBButtons.Dock=DockStyle.Top;
            this.plSBButtons.Location=new Point(0, 0);
            this.plSBButtons.Name="plSBButtons";
            this.plSBButtons.Size=new Size(200, 0);
            this.plSBButtons.TabIndex=11;
            // 
            // plTbSBInfo
            // 
            this.plTbSBInfo.BackColor=Color.Transparent;
            this.plTbSBInfo.ColumnCount=2;
            this.plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88F));
            this.plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.plTbSBInfo.Controls.Add(this.lbInfoErrors, 0, 6);
            this.plTbSBInfo.Controls.Add(this.lbInfoFiles, 0, 0);
            this.plTbSBInfo.Controls.Add(this.lbErrorCount, 1, 6);
            this.plTbSBInfo.Controls.Add(this.lbWarningCount, 1, 5);
            this.plTbSBInfo.Controls.Add(this.lbInfoWarnings, 0, 5);
            this.plTbSBInfo.Controls.Add(this.rowsCount, 1, 3);
            this.plTbSBInfo.Controls.Add(this.lbWorksheetsCount, 1, 2);
            this.plTbSBInfo.Controls.Add(this.lbOrdersCount, 1, 1);
            this.plTbSBInfo.Controls.Add(this.lbFilesCount, 1, 0);
            this.plTbSBInfo.Controls.Add(this.lbInfoRows, 0, 3);
            this.plTbSBInfo.Controls.Add(this.lbInfoWorksheets, 0, 2);
            this.plTbSBInfo.Controls.Add(this.lbInfoOrders, 0, 1);
            this.plTbSBInfo.Dock=DockStyle.Bottom;
            this.plTbSBInfo.ForeColor=UniwaveColors.uwGreyLight;
            this.plTbSBInfo.Location=new Point(0, 317);
            this.plTbSBInfo.Margin=new Padding(2, 1, 2, 1);
            this.plTbSBInfo.Name="plTbSBInfo";
            this.plTbSBInfo.RowCount=8;
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            this.plTbSBInfo.Size=new Size(200, 226);
            this.plTbSBInfo.TabIndex=12;
            // 
            // lbInfoErrors
            // 
            this.lbInfoErrors.AutoSize=true;
            this.lbInfoErrors.Dock=DockStyle.Fill;
            this.lbInfoErrors.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbInfoErrors.ForeColor=Color.Crimson;
            this.lbInfoErrors.Location=new Point(4, 172);
            this.lbInfoErrors.Margin=new Padding(4);
            this.lbInfoErrors.Name="lbInfoErrors";
            this.lbInfoErrors.Size=new Size(80, 20);
            this.lbInfoErrors.TabIndex=9;
            this.lbInfoErrors.Text="Errors:";
            this.lbInfoErrors.TextAlign=ContentAlignment.MiddleLeft;
            // 
            // lbInfoFiles
            // 
            this.lbInfoFiles.AutoSize=true;
            this.lbInfoFiles.Dock=DockStyle.Fill;
            this.lbInfoFiles.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbInfoFiles.ForeColor=SystemColors.ScrollBar;
            this.lbInfoFiles.Location=new Point(4, 4);
            this.lbInfoFiles.Margin=new Padding(4);
            this.lbInfoFiles.Name="lbInfoFiles";
            this.lbInfoFiles.Size=new Size(80, 20);
            this.lbInfoFiles.TabIndex=4;
            this.lbInfoFiles.Text="Files:";
            this.lbInfoFiles.TextAlign=ContentAlignment.MiddleLeft;
            // 
            // lbErrorCount
            // 
            this.lbErrorCount.AutoSize=true;
            this.lbErrorCount.Dock=DockStyle.Fill;
            this.lbErrorCount.Font=new Font("Segoe UI", 9F);
            this.lbErrorCount.ForeColor=Color.Red;
            this.lbErrorCount.Location=new Point(92, 172);
            this.lbErrorCount.Margin=new Padding(4);
            this.lbErrorCount.Name="lbErrorCount";
            this.lbErrorCount.Size=new Size(104, 20);
            this.lbErrorCount.TabIndex=0;
            this.lbErrorCount.Text="0";
            this.lbErrorCount.TextAlign=ContentAlignment.MiddleCenter;
            // 
            // lbWarningCount
            // 
            this.lbWarningCount.AutoSize=true;
            this.lbWarningCount.Dock=DockStyle.Fill;
            this.lbWarningCount.Font=new Font("Segoe UI", 9F);
            this.lbWarningCount.ForeColor=Color.Coral;
            this.lbWarningCount.Location=new Point(92, 144);
            this.lbWarningCount.Margin=new Padding(4);
            this.lbWarningCount.Name="lbWarningCount";
            this.lbWarningCount.Size=new Size(104, 20);
            this.lbWarningCount.TabIndex=0;
            this.lbWarningCount.Text="0";
            this.lbWarningCount.TextAlign=ContentAlignment.MiddleCenter;
            // 
            // lbInfoWarnings
            // 
            this.lbInfoWarnings.AutoSize=true;
            this.lbInfoWarnings.Dock=DockStyle.Fill;
            this.lbInfoWarnings.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbInfoWarnings.ForeColor=Color.Coral;
            this.lbInfoWarnings.Location=new Point(4, 144);
            this.lbInfoWarnings.Margin=new Padding(4);
            this.lbInfoWarnings.Name="lbInfoWarnings";
            this.lbInfoWarnings.Size=new Size(80, 20);
            this.lbInfoWarnings.TabIndex=0;
            this.lbInfoWarnings.Text="Warnings:";
            this.lbInfoWarnings.TextAlign=ContentAlignment.MiddleLeft;
            // 
            // rowsCount
            // 
            this.rowsCount.AutoSize=true;
            this.rowsCount.Dock=DockStyle.Fill;
            this.rowsCount.Font=new Font("Segoe UI", 9F);
            this.rowsCount.ForeColor=SystemColors.ScrollBar;
            this.rowsCount.Location=new Point(92, 88);
            this.rowsCount.Margin=new Padding(4);
            this.rowsCount.Name="rowsCount";
            this.rowsCount.Size=new Size(104, 20);
            this.rowsCount.TabIndex=0;
            this.rowsCount.Text="10";
            this.rowsCount.TextAlign=ContentAlignment.MiddleCenter;
            // 
            // lbWorksheetsCount
            // 
            this.lbWorksheetsCount.AutoSize=true;
            this.lbWorksheetsCount.Dock=DockStyle.Fill;
            this.lbWorksheetsCount.Font=new Font("Segoe UI", 9F);
            this.lbWorksheetsCount.ForeColor=SystemColors.ScrollBar;
            this.lbWorksheetsCount.Location=new Point(92, 60);
            this.lbWorksheetsCount.Margin=new Padding(4);
            this.lbWorksheetsCount.Name="lbWorksheetsCount";
            this.lbWorksheetsCount.Size=new Size(104, 20);
            this.lbWorksheetsCount.TabIndex=0;
            this.lbWorksheetsCount.Text="10";
            this.lbWorksheetsCount.TextAlign=ContentAlignment.MiddleCenter;
            // 
            // lbOrdersCount
            // 
            this.lbOrdersCount.AutoSize=true;
            this.lbOrdersCount.Dock=DockStyle.Fill;
            this.lbOrdersCount.Font=new Font("Segoe UI", 9F);
            this.lbOrdersCount.ForeColor=SystemColors.ScrollBar;
            this.lbOrdersCount.Location=new Point(92, 32);
            this.lbOrdersCount.Margin=new Padding(4);
            this.lbOrdersCount.Name="lbOrdersCount";
            this.lbOrdersCount.Size=new Size(104, 20);
            this.lbOrdersCount.TabIndex=0;
            this.lbOrdersCount.Text="10";
            this.lbOrdersCount.TextAlign=ContentAlignment.MiddleCenter;
            // 
            // lbFilesCount
            // 
            this.lbFilesCount.AutoSize=true;
            this.lbFilesCount.BackColor=Color.FromArgb(88, 89, 81);
            this.lbFilesCount.Dock=DockStyle.Fill;
            this.lbFilesCount.FlatStyle=FlatStyle.System;
            this.lbFilesCount.Font=new Font("Segoe UI", 9F);
            this.lbFilesCount.ForeColor=UniwaveColors.uwGreyLight;
            this.lbFilesCount.Location=new Point(92, 4);
            this.lbFilesCount.Margin=new Padding(4);
            this.lbFilesCount.Name="lbFilesCount";
            this.lbFilesCount.Size=new Size(104, 20);
            this.lbFilesCount.TabIndex=0;
            this.lbFilesCount.Text="10";
            this.lbFilesCount.TextAlign=ContentAlignment.MiddleCenter;
            // 
            // lbInfoRows
            // 
            this.lbInfoRows.AutoSize=true;
            this.lbInfoRows.Dock=DockStyle.Fill;
            this.lbInfoRows.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbInfoRows.ForeColor=SystemColors.ScrollBar;
            this.lbInfoRows.Location=new Point(4, 88);
            this.lbInfoRows.Margin=new Padding(4);
            this.lbInfoRows.Name="lbInfoRows";
            this.lbInfoRows.Size=new Size(80, 20);
            this.lbInfoRows.TabIndex=0;
            this.lbInfoRows.Text="Rows:";
            this.lbInfoRows.TextAlign=ContentAlignment.MiddleLeft;
            // 
            // lbInfoWorksheets
            // 
            this.lbInfoWorksheets.AutoSize=true;
            this.lbInfoWorksheets.Dock=DockStyle.Fill;
            this.lbInfoWorksheets.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbInfoWorksheets.ForeColor=SystemColors.ScrollBar;
            this.lbInfoWorksheets.Location=new Point(4, 60);
            this.lbInfoWorksheets.Margin=new Padding(4);
            this.lbInfoWorksheets.Name="lbInfoWorksheets";
            this.lbInfoWorksheets.Size=new Size(80, 20);
            this.lbInfoWorksheets.TabIndex=0;
            this.lbInfoWorksheets.Text="Worksheets:";
            this.lbInfoWorksheets.TextAlign=ContentAlignment.MiddleLeft;
            // 
            // lbInfoOrders
            // 
            this.lbInfoOrders.AutoSize=true;
            this.lbInfoOrders.Dock=DockStyle.Fill;
            this.lbInfoOrders.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbInfoOrders.ForeColor=SystemColors.ScrollBar;
            this.lbInfoOrders.Location=new Point(4, 32);
            this.lbInfoOrders.Margin=new Padding(4);
            this.lbInfoOrders.Name="lbInfoOrders";
            this.lbInfoOrders.Size=new Size(80, 20);
            this.lbInfoOrders.TabIndex=3;
            this.lbInfoOrders.Text="Orders:";
            this.lbInfoOrders.TextAlign=ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount=3;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btSideBar, 0, 0);
            this.tableLayoutPanel1.Dock=DockStyle.Top;
            this.tableLayoutPanel1.Location=new Point(0, 0);
            this.tableLayoutPanel1.Name="tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount=1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size=new Size(200, 40);
            this.tableLayoutPanel1.TabIndex=43;
            // 
            // btSideBar
            // 
            this.btSideBar.BackColor=Color.Transparent;
            this.btSideBar.BackgroundImage=(Image)resources.GetObject("btSideBar.BackgroundImage");
            this.btSideBar.BackgroundImageLayout=ImageLayout.Center;
            this.btSideBar.Dock=DockStyle.Fill;
            this.btSideBar.FlatAppearance.BorderColor=Color.FromArgb(88, 89, 81);
            this.btSideBar.FlatAppearance.BorderSize=0;
            this.btSideBar.FlatStyle=FlatStyle.Flat;
            this.btSideBar.Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.btSideBar.ForeColor=UniwaveColors.uwOrangeDeep;
            this.btSideBar.ImageAlign=ContentAlignment.MiddleLeft;
            this.btSideBar.Location=new Point(0, 0);
            this.btSideBar.Margin=new Padding(0);
            this.btSideBar.Name="btSideBar";
            this.btSideBar.SelectedOne=false;
            this.btSideBar.Size=new Size(40, 40);
            this.btSideBar.TabIndex=22;
            this.btSideBar.TextAlign=ContentAlignment.MiddleLeft;
            this.btSideBar.TextImageRelation=TextImageRelation.ImageBeforeText;
            this.btSideBar.UseVisualStyleBackColor=false;
            this.btSideBar.Click+=btSideBar_Click;
            // 
            // btLoadFiles
            // 
            this.btLoadFiles.BackColor=Color.Transparent;
            this.btLoadFiles.Dock=DockStyle.Top;
            this.btLoadFiles.FlatAppearance.BorderColor=Color.FromArgb(88, 89, 81);
            this.btLoadFiles.FlatAppearance.BorderSize=0;
            this.btLoadFiles.FlatStyle=FlatStyle.Flat;
            this.btLoadFiles.Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.btLoadFiles.ForeColor=UniwaveColors.uwOrangeDeep;
            this.btLoadFiles.Image=(Image)resources.GetObject("btLoadFiles.Image");
            this.btLoadFiles.ImageAlign=ContentAlignment.MiddleLeft;
            this.btLoadFiles.Location=new Point(0, 40);
            this.btLoadFiles.Margin=new Padding(0);
            this.btLoadFiles.Name="btLoadFiles";
            this.btLoadFiles.Padding=new Padding(12, 0, 0, 0);
            this.btLoadFiles.SelectedOne=false;
            this.btLoadFiles.Size=new Size(200, 40);
            this.btLoadFiles.TabIndex=44;
            this.btLoadFiles.Text="  Load &Files";
            this.btLoadFiles.TextAlign=ContentAlignment.MiddleLeft;
            this.btLoadFiles.TextImageRelation=TextImageRelation.ImageBeforeText;
            this.btLoadFiles.UseVisualStyleBackColor=false;
            this.btLoadFiles.Click+=BtFilesRefresh_Click;
            // 
            // btImport
            // 
            this.btImport.BackColor=Color.Transparent;
            this.btImport.Dock=DockStyle.Top;
            this.btImport.FlatAppearance.BorderColor=Color.FromArgb(88, 89, 81);
            this.btImport.FlatAppearance.BorderSize=0;
            this.btImport.FlatStyle=FlatStyle.Flat;
            this.btImport.Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.btImport.ForeColor=UniwaveColors.uwOrangeDeep;
            this.btImport.Image=(Image)resources.GetObject("btImport.Image");
            this.btImport.ImageAlign=ContentAlignment.MiddleLeft;
            this.btImport.Location=new Point(0, 80);
            this.btImport.Margin=new Padding(0);
            this.btImport.Name="btImport";
            this.btImport.Padding=new Padding(12, 0, 0, 0);
            this.btImport.SelectedOne=false;
            this.btImport.Size=new Size(200, 40);
            this.btImport.TabIndex=45;
            this.btImport.Text="  &Import";
            this.btImport.TextAlign=ContentAlignment.MiddleLeft;
            this.btImport.TextImageRelation=TextImageRelation.ImageBeforeText;
            this.btImport.UseVisualStyleBackColor=false;
            this.btImport.Click+=BtFilesImport_Click;
            // 
            // btLog
            // 
            this.btLog.BackColor=Color.Transparent;
            this.btLog.Dock=DockStyle.Top;
            this.btLog.FlatAppearance.BorderColor=Color.FromArgb(88, 89, 81);
            this.btLog.FlatAppearance.BorderSize=0;
            this.btLog.FlatStyle=FlatStyle.Flat;
            this.btLog.Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.btLog.ForeColor=UniwaveColors.uwOrangeDeep;
            this.btLog.Image=(Image)resources.GetObject("btLog.Image");
            this.btLog.ImageAlign=ContentAlignment.MiddleLeft;
            this.btLog.Location=new Point(0, 120);
            this.btLog.Margin=new Padding(0);
            this.btLog.Name="btLog";
            this.btLog.Padding=new Padding(12, 0, 0, 0);
            this.btLog.SelectedOne=false;
            this.btLog.Size=new Size(200, 40);
            this.btLog.TabIndex=46;
            this.btLog.Text="  &Log";
            this.btLog.TextAlign=ContentAlignment.MiddleLeft;
            this.btLog.TextImageRelation=TextImageRelation.ImageBeforeText;
            this.btLog.UseVisualStyleBackColor=true;
            this.btLog.Click+=BtLogRefresh_Click;
            // 
            // btProperties
            // 
            this.btProperties.BackColor=Color.Transparent;
            this.btProperties.Dock=DockStyle.Top;
            this.btProperties.FlatAppearance.BorderColor=Color.FromArgb(88, 89, 81);
            this.btProperties.FlatAppearance.BorderSize=0;
            this.btProperties.FlatStyle=FlatStyle.Flat;
            this.btProperties.Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.btProperties.ForeColor=UniwaveColors.uwOrangeDeep;
            this.btProperties.Image=(Image)resources.GetObject("btProperties.Image");
            this.btProperties.ImageAlign=ContentAlignment.MiddleLeft;
            this.btProperties.Location=new Point(0, 160);
            this.btProperties.Margin=new Padding(0);
            this.btProperties.Name="btProperties";
            this.btProperties.Padding=new Padding(12, 0, 0, 0);
            this.btProperties.SelectedOne=false;
            this.btProperties.Size=new Size(200, 40);
            this.btProperties.TabIndex=47;
            this.btProperties.Text="  &Properties";
            this.btProperties.TextAlign=ContentAlignment.MiddleLeft;
            this.btProperties.TextImageRelation=TextImageRelation.ImageBeforeText;
            this.btProperties.UseVisualStyleBackColor=false;
            this.btProperties.Click+=BtSettings_Click;
            // 
            // btExit
            // 
            this.btExit.BackColor=Color.Transparent;
            this.btExit.Dock=DockStyle.Top;
            this.btExit.FlatAppearance.BorderColor=Color.FromArgb(88, 89, 81);
            this.btExit.FlatAppearance.BorderSize=0;
            this.btExit.FlatStyle=FlatStyle.Flat;
            this.btExit.Font=new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.btExit.ForeColor=UniwaveColors.uwOrangeDeep;
            this.btExit.Image=(Image)resources.GetObject("btExit.Image");
            this.btExit.ImageAlign=ContentAlignment.MiddleLeft;
            this.btExit.Location=new Point(0, 200);
            this.btExit.Margin=new Padding(0);
            this.btExit.Name="btExit";
            this.btExit.Padding=new Padding(12, 0, 0, 0);
            this.btExit.SelectedOne=false;
            this.btExit.Size=new Size(200, 40);
            this.btExit.TabIndex=48;
            this.btExit.Text="  E&xit";
            this.btExit.TextAlign=ContentAlignment.MiddleLeft;
            this.btExit.TextImageRelation=TextImageRelation.ImageBeforeText;
            this.btExit.UseVisualStyleBackColor=false;
            this.btExit.Click+=BtClose_Click;
            // 
            // plSideBarMain
            // 
            this.plSideBarMain.BackColor=Color.Transparent;
            this.plSideBarMain.Controls.Add(this.btExit);
            this.plSideBarMain.Controls.Add(this.btProperties);
            this.plSideBarMain.Controls.Add(this.btLog);
            this.plSideBarMain.Controls.Add(this.btImport);
            this.plSideBarMain.Controls.Add(this.btLoadFiles);
            this.plSideBarMain.Controls.Add(this.tableLayoutPanel1);
            this.plSideBarMain.Controls.Add(this.plTbSBInfo);
            this.plSideBarMain.Controls.Add(this.plSBButtons);
            this.plSideBarMain.Dock=DockStyle.Left;
            this.plSideBarMain.ForeColor=Color.Transparent;
            this.plSideBarMain.Location=new Point(0, 93);
            this.plSideBarMain.Margin=new Padding(0);
            this.plSideBarMain.Name="plSideBarMain";
            this.plSideBarMain.Size=new Size(200, 543);
            this.plSideBarMain.TabIndex=14;
            // 
            // tlpTitleBar
            // 
            this.tlpTitleBar.AutoSize=true;
            this.tlpTitleBar.BackColor=Color.FromArgb(88, 89, 81);
            this.tlpTitleBar.ColumnCount=5;
            this.tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25F));
            this.tlpTitleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpTitleBar.ColumnStyles.Add(new ColumnStyle());
            this.tlpTitleBar.ColumnStyles.Add(new ColumnStyle());
            this.tlpTitleBar.ColumnStyles.Add(new ColumnStyle());
            this.tlpTitleBar.Controls.Add(this.btMinimize, 2, 0);
            this.tlpTitleBar.Controls.Add(this.btMaximize, 3, 0);
            this.tlpTitleBar.Controls.Add(this.btClose, 4, 0);
            this.tlpTitleBar.Controls.Add(this.plMiniLogo, 0, 0);
            this.tlpTitleBar.Controls.Add(this.plTitleBar, 1, 0);
            this.tlpTitleBar.Dock=DockStyle.Top;
            this.tlpTitleBar.Location=new Point(0, 0);
            this.tlpTitleBar.Margin=new Padding(6);
            this.tlpTitleBar.MinimumSize=new Size(15, 15);
            this.tlpTitleBar.Name="tlpTitleBar";
            this.tlpTitleBar.RowCount=1;
            this.tlpTitleBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tlpTitleBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            this.tlpTitleBar.Size=new Size(1036, 29);
            this.tlpTitleBar.TabIndex=0;
            // 
            // btMinimize
            // 
            this.btMinimize.Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right;
            this.btMinimize.AutoSize=true;
            this.btMinimize.AutoSizeMode=AutoSizeMode.GrowAndShrink;
            this.btMinimize.BackColor=Color.Transparent;
            this.btMinimize.BackgroundImage=(Image)resources.GetObject("btMinimize.BackgroundImage");
            this.btMinimize.BackgroundImageLayout=ImageLayout.Zoom;
            this.btMinimize.FlatAppearance.BorderSize=0;
            this.btMinimize.FlatAppearance.MouseDownBackColor=Color.Transparent;
            this.btMinimize.FlatAppearance.MouseOverBackColor=Color.Transparent;
            this.btMinimize.FlatStyle=FlatStyle.Flat;
            this.btMinimize.ForeColor=Color.Transparent;
            this.btMinimize.Location=new Point(971, 4);
            this.btMinimize.Margin=new Padding(4);
            this.btMinimize.MaximumSize=new Size(25, 25);
            this.btMinimize.MinimumSize=new Size(15, 15);
            this.btMinimize.Name="btMinimize";
            this.btMinimize.Size=new Size(15, 21);
            this.btMinimize.TabIndex=1;
            this.btMinimize.UseVisualStyleBackColor=false;
            this.btMinimize.Click+=btMinimize_Click;
            this.btMinimize.MouseEnter+=btMinimize_MouseEnter;
            this.btMinimize.MouseLeave+=btMinimize_MouseLeave;
            // 
            // btMaximize
            // 
            this.btMaximize.Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right;
            this.btMaximize.AutoSize=true;
            this.btMaximize.AutoSizeMode=AutoSizeMode.GrowAndShrink;
            this.btMaximize.BackColor=Color.Transparent;
            this.btMaximize.BackgroundImage=(Image)resources.GetObject("btMaximize.BackgroundImage");
            this.btMaximize.BackgroundImageLayout=ImageLayout.Zoom;
            this.btMaximize.FlatAppearance.BorderSize=0;
            this.btMaximize.FlatAppearance.MouseDownBackColor=Color.Transparent;
            this.btMaximize.FlatAppearance.MouseOverBackColor=Color.Transparent;
            this.btMaximize.FlatStyle=FlatStyle.Flat;
            this.btMaximize.ForeColor=Color.Transparent;
            this.btMaximize.Location=new Point(994, 4);
            this.btMaximize.Margin=new Padding(4);
            this.btMaximize.MaximumSize=new Size(25, 25);
            this.btMaximize.MinimumSize=new Size(15, 15);
            this.btMaximize.Name="btMaximize";
            this.btMaximize.Size=new Size(15, 21);
            this.btMaximize.TabIndex=1;
            this.btMaximize.UseVisualStyleBackColor=false;
            this.btMaximize.Click+=btMaximize_Click;
            this.btMaximize.MouseEnter+=btMaximize_MouseEnter;
            this.btMaximize.MouseLeave+=btMaximize_MouseLeave;
            // 
            // btClose
            // 
            this.btClose.Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right;
            this.btClose.AutoSize=true;
            this.btClose.AutoSizeMode=AutoSizeMode.GrowAndShrink;
            this.btClose.BackColor=Color.Transparent;
            this.btClose.BackgroundImage=(Image)resources.GetObject("btClose.BackgroundImage");
            this.btClose.BackgroundImageLayout=ImageLayout.Zoom;
            this.btClose.FlatAppearance.BorderSize=0;
            this.btClose.FlatAppearance.MouseDownBackColor=Color.Transparent;
            this.btClose.FlatStyle=FlatStyle.Flat;
            this.btClose.Font=new Font("Segoe UI", 8.75F, FontStyle.Bold);
            this.btClose.ForeColor=Color.Transparent;
            this.btClose.ImageAlign=ContentAlignment.MiddleLeft;
            this.btClose.Location=new Point(1017, 4);
            this.btClose.Margin=new Padding(4);
            this.btClose.MaximumSize=new Size(25, 25);
            this.btClose.MinimumSize=new Size(15, 15);
            this.btClose.Name="btClose";
            this.btClose.SelectedOne=true;
            this.btClose.Size=new Size(15, 21);
            this.btClose.TabIndex=2;
            this.btClose.TextAlign=ContentAlignment.MiddleLeft;
            this.btClose.TextImageRelation=TextImageRelation.ImageBeforeText;
            this.btClose.UseVisualStyleBackColor=false;
            this.btClose.Click+=BtClose_Click;
            this.btClose.MouseEnter+=btClose_MouseEnter;
            this.btClose.MouseLeave+=btClose_MouseLeave;
            // 
            // plMiniLogo
            // 
            this.plMiniLogo.BackgroundImage=(Image)resources.GetObject("plMiniLogo.BackgroundImage");
            this.plMiniLogo.BackgroundImageLayout=ImageLayout.Zoom;
            this.plMiniLogo.Dock=DockStyle.Fill;
            this.plMiniLogo.Location=new Point(3, 3);
            this.plMiniLogo.Name="plMiniLogo";
            this.plMiniLogo.Size=new Size(19, 23);
            this.plMiniLogo.TabIndex=3;
            // 
            // plTitleBar
            // 
            this.plTitleBar.BackColor=Color.Transparent;
            this.plTitleBar.Controls.Add(this.plTitleBarAppName);
            this.plTitleBar.Dock=DockStyle.Fill;
            this.plTitleBar.ForeColor=UniwaveColors.uwOrangeDeep;
            this.plTitleBar.Location=new Point(28, 3);
            this.plTitleBar.Name="plTitleBar";
            this.plTitleBar.Size=new Size(936, 23);
            this.plTitleBar.TabIndex=4;
            this.plTitleBar.Text="Alu 2 PrefSuite v2.0";
            this.plTitleBar.MouseDown+=PlTitleBar_MouseDown;
            // 
            // plTitleBarAppName
            // 
            this.plTitleBarAppName.AutoSize=true;
            this.plTitleBarAppName.Enabled=false;
            this.plTitleBarAppName.FlatStyle=FlatStyle.Flat;
            this.plTitleBarAppName.ForeColor=UniwaveColors.uwGreyLight;
            this.plTitleBarAppName.Location=new Point(3, 1);
            this.plTitleBarAppName.Name="plTitleBarAppName";
            this.plTitleBarAppName.Size=new Size(126, 15);
            this.plTitleBarAppName.TabIndex=0;
            this.plTitleBarAppName.Text="Aluminum 2 PrefSuite ";
            // 
            // plTBPanel
            // 
            this.plTBPanel.AutoSize=true;
            this.plTBPanel.AutoSizeMode=AutoSizeMode.GrowAndShrink;
            this.plTBPanel.BackColor=UniwaveColors.uwOrangeDeep;
            this.plTBPanel.Controls.Add(this.tlpTitleBar);
            this.plTBPanel.Dock=DockStyle.Top;
            this.plTBPanel.Location=new Point(0, 0);
            this.plTBPanel.Margin=new Padding(0);
            this.plTBPanel.Name="plTBPanel";
            this.plTBPanel.Size=new Size(1036, 29);
            this.plTBPanel.TabIndex=8;
            // 
            // lbErrors
            // 
            this.lbErrors.AutoSize=true;
            this.lbErrors.Dock=DockStyle.Fill;
            this.lbErrors.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbErrors.ForeColor=Color.Red;
            this.lbErrors.Location=new Point(4, 200);
            this.lbErrors.Margin=new Padding(4);
            this.lbErrors.Name="lbErrors";
            this.lbErrors.Size=new Size(80, 20);
            this.lbErrors.TabIndex=0;
            this.lbErrors.Text="Errors:";
            this.lbErrors.TextAlign=ContentAlignment.MiddleLeft;
            // 
            // slbPath
            // 
            this.slbPath.Name="slbPath";
            this.slbPath.Size=new Size(0, 17);
            // 
            // statusStrip
            // 
            this.statusStrip.AccessibleRole=AccessibleRole.Grip;
            this.statusStrip.BackColor=UniwaveColors.uwOrangeDeep;
            this.statusStrip.GripStyle=ToolStripGripStyle.Visible;
            this.statusStrip.ImageScalingSize=new Size(32, 32);
            this.statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, this.slbPath });
            this.statusStrip.Location=new Point(0, 636);
            this.statusStrip.Name="statusStrip";
            this.statusStrip.Padding=new Padding(1, 0, 8, 0);
            this.statusStrip.Size=new Size(1036, 22);
            this.statusStrip.TabIndex=0;
            this.statusStrip.Text="statusStrip";
            // 
            // plFormContainer
            // 
            this.plFormContainer.AutoScroll=true;
            this.plFormContainer.BackColor=Color.Transparent;
            this.plFormContainer.Dock=DockStyle.Fill;
            this.plFormContainer.ForeColor=Color.Transparent;
            this.plFormContainer.Location=new Point(200, 93);
            this.plFormContainer.Margin=new Padding(0);
            this.plFormContainer.Name="plFormContainer";
            this.plFormContainer.Size=new Size(836, 543);
            this.plFormContainer.TabIndex=15;
            // 
            // MainForm
            // 
            AcceptButton=this.btProperties;
            AutoScaleDimensions=new SizeF(96F, 96F);
            AutoScaleMode=AutoScaleMode.Dpi;
            AutoValidate=AutoValidate.Disable;
            BackColor=Color.FromArgb(88, 89, 91);
            ClientSize=new Size(1036, 658);
            Controls.Add(this.plFormContainer);
            Controls.Add(this.plSideBarMain);
            Controls.Add(this.tplHeader);
            Controls.Add(this.plTBPanel);
            Controls.Add(this.statusStrip);
            DoubleBuffered=true;
            FormBorderStyle=FormBorderStyle.None;
            Margin=new Padding(2, 1, 2, 1);
            MaximizeBox=false;
            MdiChildrenMinimizedAnchorBottom=false;
            MinimizeBox=false;
            MinimumSize=new Size(320, 400);
            Name="MainForm";
            StartPosition=FormStartPosition.CenterScreen;
            Text="Alu 2 PrefSuite v2.0";
            WindowState=FormWindowState.Maximized;
            FormClosed+=MainForm_FormClosed;
            Load+=MainForm_Load;
            Shown+=MainForm_Shown;
            this.tplHeader.ResumeLayout(false);
            this.tplHeader.PerformLayout();
            this.plTbSBInfo.ResumeLayout(false);
            this.plTbSBInfo.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.plSideBarMain.ResumeLayout(false);
            this.plSideBarMain.PerformLayout();
            this.tlpTitleBar.ResumeLayout(false);
            this.tlpTitleBar.PerformLayout();
            this.plTitleBar.ResumeLayout(false);
            this.plTitleBar.PerformLayout();
            this.plTBPanel.ResumeLayout(false);
            this.plTBPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
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
