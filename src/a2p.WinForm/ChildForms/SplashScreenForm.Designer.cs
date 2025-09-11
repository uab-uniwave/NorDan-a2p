using a2p.WinForm.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace a2p.WinForm
{
    partial class SplashScreenForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreenForm));
            panel1 = new Panel();
            panel2 = new Panel();
            panel4 = new Panel();
            tplHeader = new TableLayoutPanel();
            lbHeader4 = new Label();
            lbHeader1 = new Label();
            lbHeader3 = new Label();
            lbHeader2 = new Label();
            plUniwaveHeaderLogo = new Panel();
            panel3 = new Panel();
            label1 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            tplHeader.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(tplHeader);
            panel1.Controls.Add(panel3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 800);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.BackgroundImage = (Image)resources.GetObject("panel2.BackgroundImage");
            panel2.BackgroundImageLayout = ImageLayout.Zoom;
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 90);
            panel2.Margin = new Padding(6);
            panel2.Name = "panel2";
            panel2.Size = new Size(1200, 646);
            panel2.TabIndex = 17;
            panel2.UseWaitCursor = true;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(56, 57, 60);
            panel4.BorderStyle = BorderStyle.Fixed3D;
            panel4.Dock = DockStyle.Top;
            panel4.ForeColor = Color.WhiteSmoke;
            panel4.Location = new Point(0, 70);
            panel4.Margin = new Padding(6);
            panel4.Name = "panel4";
            panel4.Size = new Size(1200, 20);
            panel4.TabIndex = 16;
            panel4.UseWaitCursor = true;
            // 
            // tplHeader
            // 
            tplHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tplHeader.BackColor = Color.White;
            tplHeader.ColumnCount = 7;
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle());
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tplHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tplHeader.Controls.Add(lbHeader4, 3, 0);
            tplHeader.Controls.Add(lbHeader1, 0, 0);
            tplHeader.Controls.Add(lbHeader3, 2, 0);
            tplHeader.Controls.Add(lbHeader2, 1, 0);
            tplHeader.Controls.Add(plUniwaveHeaderLogo, 7, 0);
            tplHeader.Dock = DockStyle.Top;
            tplHeader.Location = new Point(0, 0);
            tplHeader.Margin = new Padding(0);
            tplHeader.Name = "tplHeader";
            tplHeader.Padding = new Padding(6);
            tplHeader.RowCount = 1;
            tplHeader.RowStyles.Add(new RowStyle());
            tplHeader.Size = new Size(1200, 70);
            tplHeader.TabIndex = 11;
            tplHeader.UseWaitCursor = true;
            // 
            // lbHeader4
            // 
            lbHeader4.AutoSize = true;
            lbHeader4.BackColor = Color.White;
            lbHeader4.Dock = DockStyle.Top;
            lbHeader4.Font = new Font("Segoe UI", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 10, true);
            lbHeader4.ForeColor = Color.FromArgb(239, 112, 32);
            lbHeader4.ImageAlign = ContentAlignment.TopLeft;
            lbHeader4.Location = new Point(472, 12);
            lbHeader4.Margin = new Padding(0, 6, 0, 6);
            lbHeader4.Name = "lbHeader4";
            lbHeader4.Size = new Size(64, 43);
            lbHeader4.TabIndex = 0;
            lbHeader4.Text = "v2.0";
            lbHeader4.UseCompatibleTextRendering = true;
            lbHeader4.UseWaitCursor = true;
            // 
            // lbHeader1
            // 
            lbHeader1.AutoSize = true;
            lbHeader1.BackColor = Color.White;
            lbHeader1.Dock = DockStyle.Top;
            lbHeader1.Font = new Font("Segoe UI", 16.125F);
            lbHeader1.ForeColor = Color.FromArgb(239, 112, 32);
            lbHeader1.ImageAlign = ContentAlignment.TopRight;
            lbHeader1.Location = new Point(6, 12);
            lbHeader1.Margin = new Padding(0, 6, 6, 0);
            lbHeader1.Name = "lbHeader1";
            lbHeader1.Size = new Size(216, 66);
            lbHeader1.TabIndex = 0;
            lbHeader1.Text = "Aluminum";
            lbHeader1.TextAlign = ContentAlignment.TopRight;
            lbHeader1.UseCompatibleTextRendering = true;
            lbHeader1.UseWaitCursor = true;
            // 
            // lbHeader3
            // 
            lbHeader3.BackColor = Color.White;
            lbHeader3.Dock = DockStyle.Top;
            lbHeader3.Font = new Font("Segoe UI", 16.125F, FontStyle.Bold);
            lbHeader3.ForeColor = Color.FromArgb(239, 112, 32);
            lbHeader3.ImageAlign = ContentAlignment.TopLeft;
            lbHeader3.Location = new Point(266, 12);
            lbHeader3.Margin = new Padding(0, 6, 0, 6);
            lbHeader3.Name = "lbHeader3";
            lbHeader3.Size = new Size(206, 60);
            lbHeader3.TabIndex = 0;
            lbHeader3.Text = "PrefSuite";
            lbHeader3.UseCompatibleTextRendering = true;
            lbHeader3.UseWaitCursor = true;
            // 
            // lbHeader2
            // 
            lbHeader2.BackColor = Color.Transparent;
            lbHeader2.Dock = DockStyle.Fill;
            lbHeader2.FlatStyle = FlatStyle.Flat;
            lbHeader2.Font = new Font("Segoe UI Black", 18.5F, FontStyle.Bold);
            lbHeader2.ForeColor = Color.FromArgb(239, 112, 32);
            lbHeader2.Location = new Point(228, 12);
            lbHeader2.Margin = new Padding(0, 6, 0, 0);
            lbHeader2.Name = "lbHeader2";
            lbHeader2.Size = new Size(38, 70);
            lbHeader2.TabIndex = 0;
            lbHeader2.Text = "2";
            lbHeader2.TextAlign = ContentAlignment.TopCenter;
            lbHeader2.UseCompatibleTextRendering = true;
            lbHeader2.UseWaitCursor = true;
            // 
            // plUniwaveHeaderLogo
            // 
            plUniwaveHeaderLogo.BackgroundImage = Resources.UniwaveLogo;
            plUniwaveHeaderLogo.BackgroundImageLayout = ImageLayout.Zoom;
            plUniwaveHeaderLogo.Dock = DockStyle.Top;
            plUniwaveHeaderLogo.Location = new Point(994, 6);
            plUniwaveHeaderLogo.Margin = new Padding(0);
            plUniwaveHeaderLogo.Name = "plUniwaveHeaderLogo";
            plUniwaveHeaderLogo.Size = new Size(200, 68);
            plUniwaveHeaderLogo.TabIndex = 1;
            plUniwaveHeaderLogo.UseWaitCursor = true;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(56, 57, 60);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 736);
            panel3.Margin = new Padding(6);
            panel3.Name = "panel3";
            panel3.Size = new Size(1200, 64);
            panel3.TabIndex = 12;
            panel3.UseWaitCursor = true;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(1000, 12);
            label1.Margin = new Padding(10);
            label1.Name = "label1";
            label1.Size = new Size(160, 40);
            label1.TabIndex = 10;
            label1.Text = "v.1.0.0.17";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.UseWaitCursor = true;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(0, 0);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new Size(1200, 64);
            label2.TabIndex = 9;
            label2.Text = "All Rights Reserved ⓒ UAB Uniwave 2004 - 2025";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.UseWaitCursor = true;
            // 
            // SplashScreenForm
            // 
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1200, 800);
            Controls.Add(panel1);
            DoubleBuffered = true;
            ForeColor = Color.Transparent;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "SplashScreenForm";
            StartPosition = FormStartPosition.CenterScreen;
            Load += SplashScreenForm_Load;
            Shown += SplashScreenForm_Shown;
            DpiChanged += SplashScreenForm_DpiChanged;
            panel1.ResumeLayout(false);
            tplHeader.ResumeLayout(false);
            tplHeader.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);


        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel tplHeader;
        private Label lbHeader4;
        private Label lbHeader1;
        private Label lbHeader3;
        private Label lbHeader2;
        private Panel plUniwaveHeaderLogo;
        private Panel panel3;
        private Label label1;
        private Label label2;
        private Panel panel2;
        private Panel panel4;
    }
}
