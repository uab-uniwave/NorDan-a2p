using DocumentFormat.OpenXml.Spreadsheet;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;

namespace a2p.WinForm
{
    partial class SplashScreenForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreenForm));
            this.tplHeader=new TableLayoutPanel();
            this.lbHeader4=new Label();
            this.lbHeader1=new Label();
            this.lbHeader3=new Label();
            this.lbHeader2=new Label();
            this.plUniwaveHeaderLogo=new Panel();
            this.label2=new Label();
            this.label1=new Label();
            this.panel2=new Panel();
            this.panel3=new Panel();
            this.panel1=new Panel();
            this.tplHeader.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tplHeader
            // 
            this.tplHeader.AutoSizeMode=AutoSizeMode.GrowAndShrink;
            this.tplHeader.BackColor=Color.Transparent;
            this.tplHeader.ColumnCount=7;
            this.tplHeader.ColumnStyles.Add(new ColumnStyle());
            this.tplHeader.ColumnStyles.Add(new ColumnStyle());
            this.tplHeader.ColumnStyles.Add(new ColumnStyle());
            this.tplHeader.ColumnStyles.Add(new ColumnStyle());
            this.tplHeader.ColumnStyles.Add(new ColumnStyle());
            this.tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            this.tplHeader.Controls.Add(this.lbHeader4, 3, 0);
            this.tplHeader.Controls.Add(this.lbHeader1, 0, 0);
            this.tplHeader.Controls.Add(this.lbHeader3, 2, 0);
            this.tplHeader.Controls.Add(this.lbHeader2, 1, 0);
            this.tplHeader.Controls.Add(this.plUniwaveHeaderLogo, 7, 0);
            this.tplHeader.Dock=DockStyle.Top;
            this.tplHeader.Location=new Point(0, 0);
            this.tplHeader.Name="tplHeader";
            this.tplHeader.Padding=new Padding(3);
            this.tplHeader.RowCount=1;
            this.tplHeader.RowStyles.Add(new RowStyle());
            this.tplHeader.Size=new Size(600, 35);
            this.tplHeader.TabIndex=3;
            this.tplHeader.UseWaitCursor=true;
            // 
            // lbHeader4
            // 
            this.lbHeader4.AutoSize=true;
            this.lbHeader4.BackColor=Color.Transparent;
            this.lbHeader4.Dock=DockStyle.Top;
            this.lbHeader4.Font=new Font("Segoe UI", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 10, true);
            this.lbHeader4.ImageAlign=ContentAlignment.TopLeft;
            this.lbHeader4.Location=new Point(240, 3);
            this.lbHeader4.Margin=new Padding(0);
            this.lbHeader4.Name="lbHeader4";
            this.lbHeader4.Size=new Size(32, 23);
            this.lbHeader4.TabIndex=0;
            this.lbHeader4.Text="v2.0";
            this.lbHeader4.UseCompatibleTextRendering=true;
            this.lbHeader4.UseWaitCursor=true;
            // 
            // lbHeader1
            // 
            this.lbHeader1.AutoSize=true;
            this.lbHeader1.BackColor=Color.Transparent;
            this.lbHeader1.Dock=DockStyle.Bottom;
            this.lbHeader1.Font=new Font("Segoe UI", 16.125F);
            this.lbHeader1.ImageAlign=ContentAlignment.TopRight;
            this.lbHeader1.Location=new Point(3, 3);
            this.lbHeader1.Margin=new Padding(0);
            this.lbHeader1.Name="lbHeader1";
            this.lbHeader1.Size=new Size(108, 35);
            this.lbHeader1.TabIndex=0;
            this.lbHeader1.Text="Aluminum";
            this.lbHeader1.UseCompatibleTextRendering=true;
            this.lbHeader1.UseWaitCursor=true;
            // 
            // lbHeader3
            // 
            this.lbHeader3.BackColor=Color.Transparent;
            this.lbHeader3.Dock=DockStyle.Top;
            this.lbHeader3.Font=new Font("Segoe UI", 16.125F, FontStyle.Bold);
            this.lbHeader3.ImageAlign=ContentAlignment.TopLeft;
            this.lbHeader3.Location=new Point(137, 3);
            this.lbHeader3.Margin=new Padding(0);
            this.lbHeader3.Name="lbHeader3";
            this.lbHeader3.Size=new Size(103, 30);
            this.lbHeader3.TabIndex=0;
            this.lbHeader3.Text="PrefSuite";
            this.lbHeader3.UseCompatibleTextRendering=true;
            this.lbHeader3.UseWaitCursor=true;
            // 
            // lbHeader2
            // 
            this.lbHeader2.BackColor=Color.Transparent;
            this.lbHeader2.Dock=DockStyle.Fill;
            this.lbHeader2.FlatStyle=FlatStyle.Flat;
            this.lbHeader2.Font=new Font("Segoe UI Black", 20.5F, FontStyle.Bold);
            this.lbHeader2.Location=new Point(111, 3);
            this.lbHeader2.Margin=new Padding(0);
            this.lbHeader2.Name="lbHeader2";
            this.lbHeader2.Size=new Size(26, 35);
            this.lbHeader2.TabIndex=0;
            this.lbHeader2.Text="2";
            this.lbHeader2.TextAlign=ContentAlignment.TopCenter;
            this.lbHeader2.UseCompatibleTextRendering=true;
            this.lbHeader2.UseWaitCursor=true;
            // 
            // plUniwaveHeaderLogo
            // 
            this.plUniwaveHeaderLogo.BackgroundImage=(Image)resources.GetObject("plUniwaveHeaderLogo.BackgroundImage");
            this.plUniwaveHeaderLogo.BackgroundImageLayout=ImageLayout.Zoom;
            this.plUniwaveHeaderLogo.Dock=DockStyle.Top;
            this.plUniwaveHeaderLogo.Location=new Point(497, 3);
            this.plUniwaveHeaderLogo.Margin=new Padding(0);
            this.plUniwaveHeaderLogo.Name="plUniwaveHeaderLogo";
            this.plUniwaveHeaderLogo.Size=new Size(100, 34);
            this.plUniwaveHeaderLogo.TabIndex=1;
            this.plUniwaveHeaderLogo.UseWaitCursor=true;
            // 
            // label2
            // 
            this.label2.Anchor=AnchorStyles.Top|AnchorStyles.Right;
            this.label2.BackColor=Color.Transparent;
            this.label2.Font=new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.ForeColor=Color.White;
            this.label2.Location=new Point(796, 0);
            this.label2.Name="label2";
            this.label2.Size=new Size(596, 21);
            this.label2.TabIndex=7;
            this.label2.Text="ⓒ UAB Uniwave 2004 - 2025";
            this.label2.TextAlign=ContentAlignment.MiddleCenter;
            this.label2.UseWaitCursor=true;
            // 
            // label1
            // 
            this.label1.Anchor=AnchorStyles.Top|AnchorStyles.Right;
            this.label1.BackColor=Color.Transparent;
            this.label1.Font=new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.ForeColor=Color.White;
            this.label1.Location=new Point(2120, 335);
            this.label1.Margin=new Padding(5);
            this.label1.Name="label1";
            this.label1.Size=new Size(80, 20);
            this.label1.TabIndex=8;
            this.label1.Text="v.1.0.0.1";
            this.label1.TextAlign=ContentAlignment.MiddleCenter;
            this.label1.UseWaitCursor=true;
            // 
            // panel2
            // 
            this.panel2.BackColor = UniwaveColors.a2pGreyDark;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock=DockStyle.Bottom;
            this.panel2.Location=new Point(0, 368);
            this.panel2.Margin=new Padding(0);
            this.panel2.Name="panel2";
            this.panel2.Size=new Size(600, 32);
            this.panel2.TabIndex=5;
            this.panel2.UseWaitCursor=true;
            // 
            // panel3
            // 
            this.panel3.BackColor= UniwaveColors.a2pGreyDark;
            this.panel2.Controls.Add(this.label2);
            this.panel3.BorderStyle=BorderStyle.Fixed3D;
            this.panel3.Dock=DockStyle.Top;
            this.panel3.Location=new Point(0, 35);
            this.panel3.Margin=new Padding(0);
            this.panel3.Name="panel3";
            this.panel3.Size=new Size(600, 8);
            this.panel3.TabIndex=9;
            this.panel3.UseWaitCursor=true;
            // 
            // plProgressBarPanel
            // 
            this.panel1.BackColor=Color.Transparent;
            this.panel1.BackgroundImage=(Image)resources.GetObject("plProgressBarPanel.BackgroundImage");
            this.panel1.BackgroundImageLayout=ImageLayout.Stretch;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock=DockStyle.Fill;
            this.panel1.Location=new Point(0, 43);
            this.panel1.Margin=new Padding(0);
            this.panel1.Name="plProgressBarPanel";
            this.panel1.Size=new Size(600, 325);
            this.panel1.TabIndex=10;
            this.panel1.UseWaitCursor=true;
            // 
            // SplashScreenForm
            // 
            AutoScaleDimensions=new SizeF(96F, 96F);
            AutoScaleMode=AutoScaleMode.Dpi;
            BackColor=Color.White;
            BackgroundImageLayout=ImageLayout.Stretch;
            ClientSize=new Size(600, 400);
            Controls.Add(this.panel1);
            Controls.Add(this.panel3);
            Controls.Add(this.panel2);
            Controls.Add(this.tplHeader);
            DoubleBuffered=true;
            ForeColor=Color.Transparent;
            FormBorderStyle=FormBorderStyle.None;
            Name="SplashScreenForm";
            StartPosition=FormStartPosition.CenterScreen;
            this.tplHeader.ResumeLayout(false);
            this.tplHeader.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ResumeLayout(false);


        }


        #endregion
        private TableLayoutPanel tplHeader;
        private Label lbHeader4;
        private Label lbHeader1;
        private Label lbHeader3;
        private Panel plUniwaveHeaderLogo;
        private Label lbHeader2;
        private Label label2;
        private Panel panel2;
        private Panel panel3;
        private Panel panel1;
        private Label label1;
    }
}