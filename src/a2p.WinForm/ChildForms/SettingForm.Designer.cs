namespace a2p.WinForm.ChildForms
{
 partial class SettingForm
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
   if (disposing && (components != null))
   {
    components.Dispose();
   }
   base.Dispose(disposing);
  }

        #region Component Designer generated code


        private void InitializeComponent()
        {
            this.chbLoadOnStart=new CheckBox();
            this.btSave=new Button();
            this.button3=new Button();
            this.button2=new Button();
            this.button1=new Button();
            this.tbSuccess=new TextBox();
            this.tbWorkingFolder=new TextBox();
            this.chbStaging=new CheckBox();
            this.lbFailFolder=new Label();
            this.lbSuccessFolder=new Label();
            this.tbFailed=new TextBox();
            this.lbWorkingFolder=new Label();
            this.lsStaging=new Label();
            this.lbLoadOnStart=new Label();
            this.tableLayoutPanel1=new TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // chbLoadOnStart
            // 
            this.chbLoadOnStart.BackColor=Color.Transparent;
            this.chbLoadOnStart.Dock=DockStyle.Fill;
            this.chbLoadOnStart.FlatAppearance.BorderColor=UniwaveColors.uwOrangeDeep;
            this.chbLoadOnStart.FlatAppearance.BorderSize=2;
            this.chbLoadOnStart.FlatAppearance.CheckedBackColor=Color.DimGray;
            this.chbLoadOnStart.FlatAppearance.MouseDownBackColor=Color.Silver;
            this.chbLoadOnStart.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.chbLoadOnStart.ForeColor=UniwaveColors.uwOrangeDeep;
            this.chbLoadOnStart.Location=new Point(392, 326);
            this.chbLoadOnStart.Margin=new Padding(16, 6, 6, 6);
            this.chbLoadOnStart.Name="chbLoadOnStart";
            this.chbLoadOnStart.Size=new Size(482, 52);
            this.chbLoadOnStart.TabIndex=31;
            this.chbLoadOnStart.Text="Load";
            this.chbLoadOnStart.UseVisualStyleBackColor=false;
            // 
            // btSave
            // 
            this.btSave.Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Right;
            this.btSave.BackColor=UniwaveColors.a2pGreyDark;
            this.btSave.BackgroundImageLayout=ImageLayout.Center;
            this.tableLayoutPanel1.SetColumnSpan(this.btSave, 2);
            this.btSave.FlatAppearance.BorderColor=UniwaveColors.uwOrangeDeep;
            this.btSave.FlatAppearance.CheckedBackColor=UniwaveColors.uwOrangeDeep;
            this.btSave.FlatAppearance.MouseDownBackColor=UniwaveColors.uwGreyLight;
            this.btSave.FlatAppearance.MouseOverBackColor=UniwaveColors.uwOrangeDeep;
            this.btSave.FlatStyle=FlatStyle.Flat;
            this.btSave.ForeColor=UniwaveColors.uwGreyLight;
            this.btSave.Location=new Point(786, 390);
            this.btSave.Margin=new Padding(6, 6, 6, 6);
            this.btSave.Name="btSave";
            this.btSave.Size=new Size(148, 52);
            this.btSave.TabIndex=33;
            this.btSave.Text="Save";
            this.btSave.UseVisualStyleBackColor=false;
            this.btSave.Click+=btSave_Click;
            // 
            // button3
            // 
            this.button3.Anchor=AnchorStyles.Top|AnchorStyles.Right;
            this.button3.BackColor=UniwaveColors.a2pGreyDark;
            this.button3.BackgroundImageLayout=ImageLayout.Center;
            this.button3.FlatAppearance.BorderColor=UniwaveColors.uwOrangeDeep;
            this.button3.FlatAppearance.CheckedBackColor=UniwaveColors.uwOrangeDeep;
            this.button3.FlatAppearance.MouseDownBackColor=UniwaveColors.uwGreyLight;
            this.button3.FlatAppearance.MouseOverBackColor=UniwaveColors.uwOrangeDeep;
            this.button3.FlatStyle=FlatStyle.Flat;
            this.button3.ForeColor=UniwaveColors.uwGreyLight;
            this.button3.Location=new Point(886, 198);
            this.button3.Margin=new Padding(6, 6, 6, 6);
            this.button3.Name="button3";
            this.button3.Size=new Size(48, 50);
            this.button3.TabIndex=36;
            this.button3.Text="...";
            this.button3.UseVisualStyleBackColor=false;
            // 
            // button2
            // 
            this.button2.Anchor=AnchorStyles.Top|AnchorStyles.Right;
            this.button2.BackColor=UniwaveColors.a2pGreyDark;
            this.button2.BackgroundImageLayout=ImageLayout.Center;
            this.button2.FlatAppearance.BorderColor=UniwaveColors.uwOrangeDeep;
            this.button2.FlatAppearance.CheckedBackColor=UniwaveColors.uwOrangeDeep;
            this.button2.FlatAppearance.MouseDownBackColor=UniwaveColors.uwGreyLight;
            this.button2.FlatAppearance.MouseOverBackColor=UniwaveColors.uwOrangeDeep;
            this.button2.FlatStyle=FlatStyle.Flat;
            this.button2.ForeColor=UniwaveColors.uwGreyLight;
            this.button2.Location=new Point(886, 134);
            this.button2.Margin=new Padding(6, 6, 6, 6);
            this.button2.Name="button2";
            this.button2.Size=new Size(48, 50);
            this.button2.TabIndex=35;
            this.button2.Text="...";
            this.button2.UseVisualStyleBackColor=false;
            // 
            // button1
            // 
            this.button1.Anchor=AnchorStyles.Top|AnchorStyles.Right;
            this.button1.BackColor=UniwaveColors.a2pGreyDark;
            this.button1.BackgroundImageLayout=ImageLayout.Center;
            this.button1.FlatAppearance.BorderColor=UniwaveColors.uwOrangeDeep;
            this.button1.FlatAppearance.CheckedBackColor=UniwaveColors.uwOrangeDeep;
            this.button1.FlatAppearance.MouseDownBackColor=UniwaveColors.uwGreyLight;
            this.button1.FlatAppearance.MouseOverBackColor=UniwaveColors.uwOrangeDeep;
            this.button1.FlatStyle=FlatStyle.Flat;
            this.button1.ForeColor=UniwaveColors.uwGreyLight;
            this.button1.Location=new Point(886, 70);
            this.button1.Margin=new Padding(6, 6, 6, 6);
            this.button1.Name="button1";
            this.button1.Size=new Size(48, 50);
            this.button1.TabIndex=34;
            this.button1.Text="...";
            this.button1.UseVisualStyleBackColor=false;
            // 
            // tbSuccess
            // 
            this.tbSuccess.AccessibleRole=AccessibleRole.Text;
            this.tbSuccess.Anchor=AnchorStyles.Left|AnchorStyles.Right;
            this.tbSuccess.BackColor=UniwaveColors.a2pGreyDark;
            this.tbSuccess.BorderStyle=BorderStyle.None;
            this.tbSuccess.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.tbSuccess.ForeColor=UniwaveColors.uwOrangeDeep;
            this.tbSuccess.Location=new Point(380, 144);
            this.tbSuccess.Margin=new Padding(4, 4, 4, 4);
            this.tbSuccess.MaxLength=800;
            this.tbSuccess.MinimumSize=new Size(320, 48);
            this.tbSuccess.Name="tbSuccess";
            this.tbSuccess.PlaceholderText="Browse Folder";
            this.tbSuccess.ReadOnly=true;
            this.tbSuccess.ShortcutsEnabled=false;
            this.tbSuccess.Size=new Size(496, 48);
            this.tbSuccess.TabIndex=20;
            // 
            // tbWorkingFolder
            // 
            this.tbWorkingFolder.AccessibleRole=AccessibleRole.Text;
            this.tbWorkingFolder.Anchor=AnchorStyles.Left|AnchorStyles.Right;
            this.tbWorkingFolder.BackColor=UniwaveColors.a2pGreyDark;
            this.tbWorkingFolder.BorderStyle=BorderStyle.None;
            this.tbWorkingFolder.Font=new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.tbWorkingFolder.ForeColor=UniwaveColors.uwOrangeDeep;
            this.tbWorkingFolder.Location=new Point(380, 80);
            this.tbWorkingFolder.Margin=new Padding(4, 4, 4, 4);
            this.tbWorkingFolder.MaxLength=800;
            this.tbWorkingFolder.MinimumSize=new Size(320, 48);
            this.tbWorkingFolder.Name="tbWorkingFolder";
            this.tbWorkingFolder.PlaceholderText="Browse Folder";
            this.tbWorkingFolder.ReadOnly=true;
            this.tbWorkingFolder.ShortcutsEnabled=false;
            this.tbWorkingFolder.Size=new Size(496, 48);
            this.tbWorkingFolder.TabIndex=27;
            // 
            // chbStaging
            // 
            this.chbStaging.BackColor=Color.Transparent;
            this.chbStaging.Dock=DockStyle.Fill;
            this.chbStaging.FlatAppearance.BorderColor=UniwaveColors.uwOrangeDeep;
            this.chbStaging.FlatAppearance.BorderSize=2;
            this.chbStaging.FlatAppearance.CheckedBackColor=Color.DimGray;
            this.chbStaging.FlatAppearance.MouseDownBackColor=Color.Silver;
            this.chbStaging.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.chbStaging.ForeColor=UniwaveColors.uwOrangeDeep;
            this.chbStaging.Location=new Point(392, 262);
            this.chbStaging.Margin=new Padding(16, 6, 6, 6);
            this.chbStaging.MinimumSize=new Size(48, 48);
            this.chbStaging.Name="chbStaging";
            this.chbStaging.Size=new Size(482, 52);
            this.chbStaging.TabIndex=31;
            this.chbStaging.Text="Staging Mode";
            this.chbStaging.UseVisualStyleBackColor=false;
            // 
            // lbFailFolder
            // 
            this.lbFailFolder.AutoSize=true;
            this.lbFailFolder.BackColor=Color.Transparent;
            this.lbFailFolder.Dock=DockStyle.Fill;
            this.lbFailFolder.FlatStyle=FlatStyle.Flat;
            this.lbFailFolder.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbFailFolder.ForeColor=UniwaveColors.uwOrangeDeep;
            this.lbFailFolder.Location=new Point(46, 134);
            this.lbFailFolder.Margin=new Padding(6, 6, 6, 6);
            this.lbFailFolder.Name="lbFailFolder";
            this.lbFailFolder.Size=new Size(324, 52);
            this.lbFailFolder.TabIndex=26;
            this.lbFailFolder.Text="Failed Process Files Folder: ";
            this.lbFailFolder.TextAlign=ContentAlignment.MiddleRight;
            // 
            // lbSuccessFolder
            // 
            this.lbSuccessFolder.AutoSize=true;
            this.lbSuccessFolder.BackColor=Color.Transparent;
            this.lbSuccessFolder.Dock=DockStyle.Fill;
            this.lbSuccessFolder.FlatStyle=FlatStyle.Flat;
            this.lbSuccessFolder.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbSuccessFolder.ForeColor=UniwaveColors.uwOrangeDeep;
            this.lbSuccessFolder.Location=new Point(46, 198);
            this.lbSuccessFolder.Margin=new Padding(6, 6, 6, 6);
            this.lbSuccessFolder.Name="lbSuccessFolder";
            this.lbSuccessFolder.Size=new Size(324, 52);
            this.lbSuccessFolder.TabIndex=27;
            this.lbSuccessFolder.Text="Success Processed Files Folder:";
            this.lbSuccessFolder.TextAlign=ContentAlignment.MiddleRight;
            // 
            // tbFailed
            // 
            this.tbFailed.AccessibleRole=AccessibleRole.Text;
            this.tbFailed.Anchor=AnchorStyles.Left|AnchorStyles.Right;
            this.tbFailed.BackColor=UniwaveColors.a2pGreyDark;
            this.tbFailed.BorderStyle=BorderStyle.None;
            this.tbFailed.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.tbFailed.ForeColor=UniwaveColors.uwOrangeDeep;
            this.tbFailed.Location=new Point(380, 208);
            this.tbFailed.Margin=new Padding(4, 4, 4, 4);
            this.tbFailed.MaxLength=800;
            this.tbFailed.MinimumSize=new Size(320, 48);
            this.tbFailed.Name="tbFailed";
            this.tbFailed.PlaceholderText="Browse Folder";
            this.tbFailed.ReadOnly=true;
            this.tbFailed.ShortcutsEnabled=false;
            this.tbFailed.Size=new Size(496, 48);
            this.tbFailed.TabIndex=28;
            this.tbFailed.WordWrap=false;
            // 
            // lbWorkingFolder
            // 
            this.lbWorkingFolder.AutoSize=true;
            this.lbWorkingFolder.BackColor=Color.Transparent;
            this.lbWorkingFolder.Dock=DockStyle.Fill;
            this.lbWorkingFolder.FlatStyle=FlatStyle.Flat;
            this.lbWorkingFolder.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbWorkingFolder.ForeColor=UniwaveColors.uwOrangeDeep;
            this.lbWorkingFolder.Location=new Point(46, 70);
            this.lbWorkingFolder.Margin=new Padding(6, 6, 6, 6);
            this.lbWorkingFolder.Name="lbWorkingFolder";
            this.lbWorkingFolder.Size=new Size(324, 52);
            this.lbWorkingFolder.TabIndex=25;
            this.lbWorkingFolder.Text="Working Folder: ";
            this.lbWorkingFolder.TextAlign=ContentAlignment.MiddleRight;
            // 
            // lsStaging
            // 
            this.lsStaging.AutoSize=true;
            this.lsStaging.BackColor=Color.Transparent;
            this.lsStaging.Dock=DockStyle.Fill;
            this.lsStaging.FlatStyle=FlatStyle.Flat;
            this.lsStaging.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lsStaging.ForeColor=UniwaveColors.uwOrangeDeep;
            this.lsStaging.Location=new Point(46, 262);
            this.lsStaging.Margin=new Padding(6, 6, 6, 6);
            this.lsStaging.Name="lsStaging";
            this.lsStaging.Size=new Size(324, 52);
            this.lsStaging.TabIndex=30;
            this.lsStaging.Text="On Staging DB not Populated:";
            this.lsStaging.TextAlign=ContentAlignment.MiddleRight;
            // 
            // lbLoadOnStart
            // 
            this.lbLoadOnStart.AutoSize=true;
            this.lbLoadOnStart.BackColor=Color.Transparent;
            this.lbLoadOnStart.Dock=DockStyle.Fill;
            this.lbLoadOnStart.FlatStyle=FlatStyle.Flat;
            this.lbLoadOnStart.Font=new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lbLoadOnStart.ForeColor=UniwaveColors.uwOrangeDeep;
            this.lbLoadOnStart.Location=new Point(46, 326);
            this.lbLoadOnStart.Margin=new Padding(6, 6, 6, 6);
            this.lbLoadOnStart.Name="lbLoadOnStart";
            this.lbLoadOnStart.Size=new Size(324, 52);
            this.lbLoadOnStart.TabIndex=32;
            this.lbLoadOnStart.Text="Load Files on Start:";
            this.lbLoadOnStart.TextAlign=ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize=true;
            this.tableLayoutPanel1.AutoSizeMode=AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor=UniwaveColors.a2pGreyDark;
            this.tableLayoutPanel1.ColumnCount=5;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this.lbLoadOnStart, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lsStaging, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbWorkingFolder, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbFailed, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbSuccessFolder, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbFailFolder, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.chbStaging, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbWorkingFolder, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbSuccess, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.button1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.button2, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.button3, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.chbLoadOnStart, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btSave, 2, 6);
            this.tableLayoutPanel1.Dock=DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle=TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location=new Point(0, 0);
            this.tableLayoutPanel1.Margin=new Padding(0);
            this.tableLayoutPanel1.MinimumSize=new Size(640, 640);
            this.tableLayoutPanel1.Name="tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount=8;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size=new Size(980, 640);
            this.tableLayoutPanel1.TabIndex=29;
            this.tableLayoutPanel1.Paint+=tableLayoutPanel1_Paint;
            // 
            // SettingForm
            // 

            AutoScaleMode=AutoScaleMode.Dpi;
            AutoSize=true;
            AutoSizeMode=AutoSizeMode.GrowAndShrink;
            BackColor  = UniwaveColors.uwGreyLight;
            ClientSize =new Size(980, 640);
            Controls.Add(this.tableLayoutPanel1);
            ForeColor=UniwaveColors.uwGreyLight;
            FormBorderStyle=FormBorderStyle.None;
            Margin=new Padding(20, 108, 20, 20);
            MinimumSize=new Size(640, 640);
            Name="SettingForm";
            Load+=SettingForm_Load;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chbLoadOnStart;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lbLoadOnStart;
        private Label lsStaging;
        private Label lbWorkingFolder;
        private TextBox tbFailed;
        private Label lbSuccessFolder;
        private Label lbFailFolder;
        private CheckBox chbStaging;
        private TextBox tbWorkingFolder;
        private TextBox tbSuccess;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button btSave;
    }
}
