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
            cbxLoadOnStart = new CheckBox();
            btnSave = new Button();
            btnFailed = new Button();
            btnSucess = new Button();
            btnWorkingFolder = new Button();
            tbxSuccess = new TextBox();
            tbxWorkingFolder = new TextBox();
            cbxStaging = new CheckBox();
            lbFailFolder = new Label();
            lbSuccessFolder = new Label();
            tbxFailed = new TextBox();
            lbWorkingFolder = new Label();
            lblStaging = new Label();
            lbLoadOnStart = new Label();
            tbplSettings = new TableLayoutPanel();
            tbplSettings.SuspendLayout();
            SuspendLayout();
            // 
            // cbxLoadOnStart
            // 
            cbxLoadOnStart.BackColor = Color.Transparent;
            cbxLoadOnStart.Dock = DockStyle.Fill;
            cbxLoadOnStart.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            cbxLoadOnStart.FlatAppearance.BorderSize = 2;
            cbxLoadOnStart.FlatAppearance.CheckedBackColor = Color.DimGray;
            cbxLoadOnStart.FlatAppearance.MouseDownBackColor = Color.Silver;
            cbxLoadOnStart.Font = new Font("Segoe UI", 9F);
            cbxLoadOnStart.ForeColor = Color.FromArgb(239, 112, 32);
            cbxLoadOnStart.Location = new Point(396, 286);
            cbxLoadOnStart.Margin = new Padding(16, 6, 6, 6);
            cbxLoadOnStart.Name = "cbxLoadOnStart";
            cbxLoadOnStart.Size = new Size(465, 44);
            cbxLoadOnStart.TabIndex = 31;
            cbxLoadOnStart.Text = "Load";
            cbxLoadOnStart.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(224, 224, 224);
            btnSave.BackgroundImageLayout = ImageLayout.Center;
            tbplSettings.SetColumnSpan(btnSave, 2);
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            btnSave.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(797, 342);
            btnSave.Margin = new Padding(6);
            btnSave.MaximumSize = new Size(120, 48);
            btnSave.MinimumSize = new Size(120, 48);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 48);
            btnSave.TabIndex = 33;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btSave_Click;
            // 
            // btnFailed
            // 
            btnFailed.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnFailed.BackColor = Color.FromArgb(224, 224, 224);
            btnFailed.BackgroundImageLayout = ImageLayout.Center;
            btnFailed.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            btnFailed.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            btnFailed.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            btnFailed.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            btnFailed.FlatStyle = FlatStyle.Flat;
            btnFailed.ForeColor = Color.Black;
            btnFailed.Location = new Point(873, 174);
            btnFailed.Margin = new Padding(6);
            btnFailed.MaximumSize = new Size(48, 48);
            btnFailed.MinimumSize = new Size(48, 48);
            btnFailed.Name = "btnFailed";
            btnFailed.Size = new Size(48, 48);
            btnFailed.TabIndex = 36;
            btnFailed.Text = "...";
            btnFailed.UseVisualStyleBackColor = false;
            // 
            // btnSucess
            // 
            btnSucess.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnSucess.BackColor = Color.FromArgb(224, 224, 224);
            btnSucess.BackgroundImageLayout = ImageLayout.Center;
            btnSucess.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            btnSucess.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            btnSucess.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            btnSucess.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            btnSucess.FlatStyle = FlatStyle.Flat;
            btnSucess.ForeColor = Color.Black;
            btnSucess.Location = new Point(873, 118);
            btnSucess.Margin = new Padding(6);
            btnSucess.MaximumSize = new Size(48, 48);
            btnSucess.MinimumSize = new Size(48, 48);
            btnSucess.Name = "btnSucess";
            btnSucess.Size = new Size(48, 48);
            btnSucess.TabIndex = 35;
            btnSucess.Text = "...";
            btnSucess.UseVisualStyleBackColor = false;
            // 
            // btnWorkingFolder
            // 
            btnWorkingFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnWorkingFolder.BackColor = Color.FromArgb(224, 224, 224);
            btnWorkingFolder.BackgroundImageLayout = ImageLayout.Center;
            btnWorkingFolder.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            btnWorkingFolder.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            btnWorkingFolder.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            btnWorkingFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            btnWorkingFolder.FlatStyle = FlatStyle.Flat;
            btnWorkingFolder.ForeColor = Color.Black;
            btnWorkingFolder.Location = new Point(873, 62);
            btnWorkingFolder.Margin = new Padding(6);
            btnWorkingFolder.MaximumSize = new Size(48, 48);
            btnWorkingFolder.MinimumSize = new Size(48, 48);
            btnWorkingFolder.Name = "btnWorkingFolder";
            btnWorkingFolder.Size = new Size(48, 48);
            btnWorkingFolder.TabIndex = 34;
            btnWorkingFolder.Text = "...";
            btnWorkingFolder.UseVisualStyleBackColor = false;
            btnWorkingFolder.Click += btnWorkingFolder_Click;
            // 
            // tbxSuccess
            // 
            tbxSuccess.AccessibleRole = AccessibleRole.Text;
            tbxSuccess.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbxSuccess.BackColor = Color.FromArgb(224, 224, 224);
            tbxSuccess.BorderStyle = BorderStyle.None;
            tbxSuccess.Font = new Font("Segoe UI", 9F);
            tbxSuccess.ForeColor = Color.FromArgb(239, 112, 32);
            tbxSuccess.Location = new Point(386, 118);
            tbxSuccess.Margin = new Padding(6);
            tbxSuccess.MaximumSize = new Size(0, 48);
            tbxSuccess.MaxLength = 800;
            tbxSuccess.MinimumSize = new Size(320, 48);
            tbxSuccess.Name = "tbxSuccess";
            tbxSuccess.ReadOnly = true;
            tbxSuccess.ShortcutsEnabled = false;
            tbxSuccess.Size = new Size(475, 48);
            tbxSuccess.TabIndex = 20;
            // 
            // tbxWorkingFolder
            // 
            tbxWorkingFolder.AccessibleRole = AccessibleRole.Text;
            tbxWorkingFolder.Anchor = AnchorStyles.None;
            tbxWorkingFolder.BackColor = Color.FromArgb(224, 224, 224);
            tbxWorkingFolder.BorderStyle = BorderStyle.None;
            tbxWorkingFolder.Font = new Font("Segoe UI", 9F);
            tbxWorkingFolder.ForeColor = Color.FromArgb(239, 112, 32);
            tbxWorkingFolder.Location = new Point(386, 62);
            tbxWorkingFolder.Margin = new Padding(6);
            tbxWorkingFolder.MaximumSize = new Size(0, 48);
            tbxWorkingFolder.MaxLength = 800;
            tbxWorkingFolder.MinimumSize = new Size(320, 48);
            tbxWorkingFolder.Name = "tbxWorkingFolder";
            tbxWorkingFolder.ReadOnly = true;
            tbxWorkingFolder.ShortcutsEnabled = false;
            tbxWorkingFolder.Size = new Size(475, 48);
            tbxWorkingFolder.TabIndex = 27;
            // 
            // cbxStaging
            // 
            cbxStaging.BackColor = Color.Transparent;
            cbxStaging.Dock = DockStyle.Fill;
            cbxStaging.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            cbxStaging.FlatAppearance.BorderSize = 2;
            cbxStaging.FlatAppearance.CheckedBackColor = Color.DimGray;
            cbxStaging.FlatAppearance.MouseDownBackColor = Color.Silver;
            cbxStaging.Font = new Font("Segoe UI", 9F);
            cbxStaging.ForeColor = Color.FromArgb(239, 112, 32);
            cbxStaging.Location = new Point(396, 230);
            cbxStaging.Margin = new Padding(16, 6, 6, 6);
            cbxStaging.MinimumSize = new Size(48, 48);
            cbxStaging.Name = "cbxStaging";
            cbxStaging.Size = new Size(465, 48);
            cbxStaging.TabIndex = 31;
            cbxStaging.Text = "Staging Mode";
            cbxStaging.UseVisualStyleBackColor = false;
            // 
            // lbFailFolder
            // 
            lbFailFolder.AutoSize = true;
            lbFailFolder.BackColor = Color.Transparent;
            lbFailFolder.Dock = DockStyle.Fill;
            lbFailFolder.FlatStyle = FlatStyle.Flat;
            lbFailFolder.Font = new Font("Segoe UI", 9F);
            lbFailFolder.ForeColor = Color.FromArgb(239, 112, 32);
            lbFailFolder.Location = new Point(62, 118);
            lbFailFolder.Margin = new Padding(6);
            lbFailFolder.Name = "lbFailFolder";
            lbFailFolder.Size = new Size(312, 44);
            lbFailFolder.TabIndex = 26;
            lbFailFolder.Text = "Failed Process Files Folder: ";
            lbFailFolder.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbSuccessFolder
            // 
            lbSuccessFolder.AutoSize = true;
            lbSuccessFolder.BackColor = Color.Transparent;
            lbSuccessFolder.Dock = DockStyle.Fill;
            lbSuccessFolder.FlatStyle = FlatStyle.Flat;
            lbSuccessFolder.Font = new Font("Segoe UI", 9F);
            lbSuccessFolder.ForeColor = Color.FromArgb(239, 112, 32);
            lbSuccessFolder.Location = new Point(62, 174);
            lbSuccessFolder.Margin = new Padding(6);
            lbSuccessFolder.Name = "lbSuccessFolder";
            lbSuccessFolder.Size = new Size(312, 44);
            lbSuccessFolder.TabIndex = 27;
            lbSuccessFolder.Text = "Success Processed Files Folder:";
            lbSuccessFolder.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbxFailed
            // 
            tbxFailed.AccessibleRole = AccessibleRole.Text;
            tbxFailed.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbxFailed.BackColor = Color.FromArgb(224, 224, 224);
            tbxFailed.BorderStyle = BorderStyle.None;
            tbxFailed.Font = new Font("Segoe UI", 9F);
            tbxFailed.ForeColor = Color.FromArgb(239, 112, 32);
            tbxFailed.Location = new Point(386, 174);
            tbxFailed.Margin = new Padding(6);
            tbxFailed.MaximumSize = new Size(0, 48);
            tbxFailed.MaxLength = 800;
            tbxFailed.MinimumSize = new Size(320, 48);
            tbxFailed.Name = "tbxFailed";
            tbxFailed.ReadOnly = true;
            tbxFailed.ShortcutsEnabled = false;
            tbxFailed.Size = new Size(475, 48);
            tbxFailed.TabIndex = 28;
            tbxFailed.WordWrap = false;
            // 
            // lbWorkingFolder
            // 
            lbWorkingFolder.AutoSize = true;
            lbWorkingFolder.BackColor = Color.Transparent;
            lbWorkingFolder.Dock = DockStyle.Fill;
            lbWorkingFolder.FlatStyle = FlatStyle.Flat;
            lbWorkingFolder.Font = new Font("Segoe UI", 9F);
            lbWorkingFolder.ForeColor = Color.FromArgb(239, 112, 32);
            lbWorkingFolder.Location = new Point(62, 62);
            lbWorkingFolder.Margin = new Padding(6);
            lbWorkingFolder.Name = "lbWorkingFolder";
            lbWorkingFolder.Size = new Size(312, 44);
            lbWorkingFolder.TabIndex = 25;
            lbWorkingFolder.Text = "Working Folder: ";
            lbWorkingFolder.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblStaging
            // 
            lblStaging.AutoSize = true;
            lblStaging.BackColor = Color.Transparent;
            lblStaging.Dock = DockStyle.Fill;
            lblStaging.FlatStyle = FlatStyle.Flat;
            lblStaging.Font = new Font("Segoe UI", 9F);
            lblStaging.ForeColor = Color.FromArgb(239, 112, 32);
            lblStaging.Location = new Point(62, 230);
            lblStaging.Margin = new Padding(6);
            lblStaging.Name = "lblStaging";
            lblStaging.Size = new Size(312, 44);
            lblStaging.TabIndex = 30;
            lblStaging.Text = "On Staging DB not Populated:";
            lblStaging.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbLoadOnStart
            // 
            lbLoadOnStart.AutoSize = true;
            lbLoadOnStart.BackColor = Color.Transparent;
            lbLoadOnStart.Dock = DockStyle.Fill;
            lbLoadOnStart.FlatStyle = FlatStyle.Flat;
            lbLoadOnStart.Font = new Font("Segoe UI", 9F);
            lbLoadOnStart.ForeColor = Color.FromArgb(239, 112, 32);
            lbLoadOnStart.Location = new Point(62, 286);
            lbLoadOnStart.Margin = new Padding(6);
            lbLoadOnStart.Name = "lbLoadOnStart";
            lbLoadOnStart.Size = new Size(312, 44);
            lbLoadOnStart.TabIndex = 32;
            lbLoadOnStart.Text = "Load Files on Start:";
            lbLoadOnStart.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbplSettings
            // 
            tbplSettings.AutoSize = true;
            tbplSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tbplSettings.BackColor = Color.DimGray;
            tbplSettings.ColumnCount = 5;
            tbplSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 56F));
            tbplSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tbplSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tbplSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 56F));
            tbplSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 56F));
            tbplSettings.Controls.Add(lbLoadOnStart, 1, 5);
            tbplSettings.Controls.Add(lblStaging, 1, 4);
            tbplSettings.Controls.Add(lbWorkingFolder, 1, 1);
            tbplSettings.Controls.Add(tbxFailed, 2, 3);
            tbplSettings.Controls.Add(lbSuccessFolder, 1, 3);
            tbplSettings.Controls.Add(lbFailFolder, 1, 2);
            tbplSettings.Controls.Add(cbxStaging, 2, 4);
            tbplSettings.Controls.Add(tbxSuccess, 2, 2);
            tbplSettings.Controls.Add(btnWorkingFolder, 3, 1);
            tbplSettings.Controls.Add(btnSucess, 3, 2);
            tbplSettings.Controls.Add(btnFailed, 3, 3);
            tbplSettings.Controls.Add(cbxLoadOnStart, 2, 5);
            tbplSettings.Controls.Add(btnSave, 2, 6);
            tbplSettings.Controls.Add(tbxWorkingFolder, 2, 1);
            tbplSettings.Dock = DockStyle.Fill;
            tbplSettings.Location = new Point(0, 0);
            tbplSettings.Margin = new Padding(6);
            tbplSettings.MinimumSize = new Size(640, 640);
            tbplSettings.Name = "tbplSettings";
            tbplSettings.RowCount = 8;
            tbplSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tbplSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tbplSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tbplSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tbplSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tbplSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tbplSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tbplSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tbplSettings.Size = new Size(980, 640);
            tbplSettings.TabIndex = 29;
            // 
            // SettingForm
            // 
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.DimGray;
            ClientSize = new Size(980, 640);
            Controls.Add(tbplSettings);
            ForeColor = Color.FromArgb(248, 248, 249);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(20, 108, 20, 20);
            MinimumSize = new Size(640, 640);
            Name = "SettingForm";
            tbplSettings.ResumeLayout(false);
            tbplSettings.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox cbxLoadOnStart;
		private TableLayoutPanel tbplSettings;
		private Label lbLoadOnStart;
		private Label lblStaging;
		private Label lbWorkingFolder;
		private TextBox tbxFailed;
		private Label lbSuccessFolder;
		private Label lbFailFolder;
		private CheckBox cbxStaging;
		private TextBox tbxWorkingFolder;
		private TextBox tbxSuccess;
		private Button btnWorkingFolder;
		private Button btnSucess;
		private Button btnFailed;
		private Button btnSave;
	}
}
