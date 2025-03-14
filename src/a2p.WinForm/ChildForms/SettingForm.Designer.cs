namespace a2p.WinForm.ChildForms
{
 partial class SettingForm
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
   if (disposing && (components != null))
   {
    components.Dispose();
   }
   base.Dispose(disposing);
  }

        #region Component Designer generated code


        private void InitializeComponent()
        {
            chxLoadOnStart = new CheckBox();
            btnSave = new Button();
            btnWorkingFolder = new Button();
            chxStaging = new CheckBox();
            lbSuccessFolder = new Label();
            tlpSettings = new TableLayoutPanel();
            lbUser = new Label();
            lbPassword = new Label();
            lbWorkingFolderLabel = new Label();
            lbLogFolder = new Label();
            lbOtherSettingsTitle = new Label();
            chxTrusted = new CheckBox();
            lbDatabaseLabel = new Label();
            lbServerLabel = new Label();
            lbSQLTitle = new Label();
            lbFoldersTitle = new Label();
            txbSuccessFolder = new TextBox();
            txbLogFolder = new TextBox();
            txbServerName = new TextBox();
            txbDatabaseName = new TextBox();
            txbPassword = new TextBox();
            txbUserName = new TextBox();
            label2 = new Label();
            txbWorkingFolder = new TextBox();
            txbFailedFolder = new TextBox();
            btnTest = new Button();
            cbxLogLevel = new ComboBox();
            sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            tlpSettings.SuspendLayout();
            SuspendLayout();
            // 
            // chxLoadOnStart
            // 
            chxLoadOnStart.BackColor = Color.Transparent;
            chxLoadOnStart.Dock = DockStyle.Fill;
            chxLoadOnStart.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            chxLoadOnStart.FlatAppearance.BorderSize = 2;
            chxLoadOnStart.FlatAppearance.CheckedBackColor = Color.DimGray;
            chxLoadOnStart.FlatAppearance.MouseDownBackColor = Color.Silver;
            chxLoadOnStart.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxLoadOnStart.ForeColor = Color.FromArgb(239, 112, 32);
            chxLoadOnStart.Location = new Point(396, 790);
            chxLoadOnStart.Margin = new Padding(16, 6, 6, 6);
            chxLoadOnStart.Name = "chxLoadOnStart";
            chxLoadOnStart.Size = new Size(465, 44);
            chxLoadOnStart.TabIndex = 31;
            chxLoadOnStart.Text = "Load orders on start";
            chxLoadOnStart.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(56, 57, 60);
            btnSave.BackgroundImageLayout = ImageLayout.Center;
            tlpSettings.SetColumnSpan(btnSave, 2);
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            btnSave.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.FromArgb(239, 112, 32);
            btnSave.Location = new Point(796, 902);
            btnSave.Margin = new Padding(6);
            btnSave.MaximumSize = new Size(120, 48);
            btnSave.MinimumSize = new Size(120, 48);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 48);
            btnSave.TabIndex = 33;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnWorkingFolder
            // 
            btnWorkingFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnWorkingFolder.BackColor = Color.FromArgb(56, 57, 60);
            btnWorkingFolder.BackgroundImageLayout = ImageLayout.Center;
            btnWorkingFolder.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            btnWorkingFolder.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            btnWorkingFolder.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            btnWorkingFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            btnWorkingFolder.FlatStyle = FlatStyle.Flat;
            btnWorkingFolder.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnWorkingFolder.ForeColor = Color.FromArgb(239, 112, 32);
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
            // chxStaging
            // 
            chxStaging.BackColor = Color.Transparent;
            chxStaging.Dock = DockStyle.Fill;
            chxStaging.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            chxStaging.FlatAppearance.BorderSize = 2;
            chxStaging.FlatAppearance.CheckedBackColor = Color.DimGray;
            chxStaging.FlatAppearance.MouseDownBackColor = Color.Silver;
            chxStaging.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxStaging.ForeColor = Color.FromArgb(239, 112, 32);
            chxStaging.Location = new Point(396, 734);
            chxStaging.Margin = new Padding(16, 6, 6, 6);
            chxStaging.MinimumSize = new Size(48, 48);
            chxStaging.Name = "chxStaging";
            chxStaging.Size = new Size(465, 48);
            chxStaging.TabIndex = 31;
            chxStaging.Text = "Staging Mode (no DB changes)";
            chxStaging.UseVisualStyleBackColor = false;
            // 
            // lbSuccessFolder
            // 
            lbSuccessFolder.AutoSize = true;
            lbSuccessFolder.BackColor = Color.Transparent;
            tlpSettings.SetColumnSpan(lbSuccessFolder, 2);
            lbSuccessFolder.Dock = DockStyle.Fill;
            lbSuccessFolder.FlatStyle = FlatStyle.Flat;
            lbSuccessFolder.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbSuccessFolder.ForeColor = Color.FromArgb(239, 112, 32);
            lbSuccessFolder.Location = new Point(6, 174);
            lbSuccessFolder.Margin = new Padding(6);
            lbSuccessFolder.Name = "lbSuccessFolder";
            lbSuccessFolder.Size = new Size(368, 44);
            lbSuccessFolder.TabIndex = 27;
            lbSuccessFolder.Text = "Folder for success file:";
            lbSuccessFolder.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tlpSettings
            // 
            tlpSettings.AutoSize = true;
            tlpSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpSettings.BackColor = Color.Transparent;
            tlpSettings.ColumnCount = 5;
            tlpSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 56F));
            tlpSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));
            tlpSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 57F));
            tlpSettings.Controls.Add(lbUser, 0, 9);
            tlpSettings.Controls.Add(lbPassword, 0, 10);
            tlpSettings.Controls.Add(lbWorkingFolderLabel, 0, 1);
            tlpSettings.Controls.Add(lbLogFolder, 0, 4);
            tlpSettings.Controls.Add(lbOtherSettingsTitle, 1, 12);
            tlpSettings.Controls.Add(chxTrusted, 2, 8);
            tlpSettings.Controls.Add(lbDatabaseLabel, 0, 7);
            tlpSettings.Controls.Add(lbServerLabel, 0, 6);
            tlpSettings.Controls.Add(lbSuccessFolder, 0, 3);
            tlpSettings.Controls.Add(chxStaging, 2, 13);
            tlpSettings.Controls.Add(btnWorkingFolder, 3, 1);
            tlpSettings.Controls.Add(chxLoadOnStart, 2, 14);
            tlpSettings.Controls.Add(lbSQLTitle, 1, 5);
            tlpSettings.Controls.Add(lbFoldersTitle, 1, 0);
            tlpSettings.Controls.Add(txbSuccessFolder, 2, 3);
            tlpSettings.Controls.Add(txbLogFolder, 2, 4);
            tlpSettings.Controls.Add(txbServerName, 2, 6);
            tlpSettings.Controls.Add(txbDatabaseName, 2, 7);
            tlpSettings.Controls.Add(txbPassword, 2, 10);
            tlpSettings.Controls.Add(txbUserName, 2, 9);
            tlpSettings.Controls.Add(label2, 1, 2);
            tlpSettings.Controls.Add(txbWorkingFolder, 2, 1);
            tlpSettings.Controls.Add(txbFailedFolder, 2, 2);
            tlpSettings.Controls.Add(btnTest, 2, 11);
            tlpSettings.Controls.Add(btnSave, 2, 16);
            tlpSettings.Controls.Add(cbxLogLevel, 2, 15);
            tlpSettings.Dock = DockStyle.Fill;
            tlpSettings.Location = new Point(0, 0);
            tlpSettings.Margin = new Padding(6);
            tlpSettings.MinimumSize = new Size(640, 640);
            tlpSettings.Name = "tlpSettings";
            tlpSettings.RowCount = 17;
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpSettings.Size = new Size(980, 980);
            tlpSettings.TabIndex = 29;
            // 
            // lbUser
            // 
            lbUser.AutoSize = true;
            lbUser.BackColor = Color.Transparent;
            tlpSettings.SetColumnSpan(lbUser, 2);
            lbUser.Dock = DockStyle.Fill;
            lbUser.FlatStyle = FlatStyle.Flat;
            lbUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbUser.ForeColor = Color.FromArgb(239, 112, 32);
            lbUser.Location = new Point(6, 510);
            lbUser.Margin = new Padding(6);
            lbUser.Name = "lbUser";
            lbUser.Size = new Size(368, 44);
            lbUser.TabIndex = 77;
            lbUser.Text = "User: ";
            lbUser.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbPassword
            // 
            lbPassword.AutoSize = true;
            lbPassword.BackColor = Color.Transparent;
            tlpSettings.SetColumnSpan(lbPassword, 2);
            lbPassword.Dock = DockStyle.Fill;
            lbPassword.FlatStyle = FlatStyle.Flat;
            lbPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbPassword.ForeColor = Color.FromArgb(239, 112, 32);
            lbPassword.Location = new Point(6, 566);
            lbPassword.Margin = new Padding(6);
            lbPassword.Name = "lbPassword";
            lbPassword.Size = new Size(368, 44);
            lbPassword.TabIndex = 76;
            lbPassword.Text = "Password: ";
            lbPassword.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbWorkingFolderLabel
            // 
            lbWorkingFolderLabel.AutoSize = true;
            lbWorkingFolderLabel.BackColor = Color.Transparent;
            tlpSettings.SetColumnSpan(lbWorkingFolderLabel, 2);
            lbWorkingFolderLabel.Dock = DockStyle.Fill;
            lbWorkingFolderLabel.FlatStyle = FlatStyle.Flat;
            lbWorkingFolderLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbWorkingFolderLabel.ForeColor = Color.FromArgb(239, 112, 32);
            lbWorkingFolderLabel.Location = new Point(6, 62);
            lbWorkingFolderLabel.Margin = new Padding(6);
            lbWorkingFolderLabel.Name = "lbWorkingFolderLabel";
            lbWorkingFolderLabel.Size = new Size(368, 44);
            lbWorkingFolderLabel.TabIndex = 74;
            lbWorkingFolderLabel.Text = "Successfully processed files:";
            lbWorkingFolderLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbLogFolder
            // 
            lbLogFolder.AutoSize = true;
            lbLogFolder.BackColor = Color.Transparent;
            tlpSettings.SetColumnSpan(lbLogFolder, 2);
            lbLogFolder.Dock = DockStyle.Fill;
            lbLogFolder.FlatStyle = FlatStyle.Flat;
            lbLogFolder.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbLogFolder.ForeColor = Color.FromArgb(239, 112, 32);
            lbLogFolder.Location = new Point(6, 230);
            lbLogFolder.Margin = new Padding(6);
            lbLogFolder.Name = "lbLogFolder";
            lbLogFolder.Size = new Size(368, 44);
            lbLogFolder.TabIndex = 72;
            lbLogFolder.Text = "Log Folder:";
            lbLogFolder.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbOtherSettingsTitle
            // 
            lbOtherSettingsTitle.AutoSize = true;
            tlpSettings.SetColumnSpan(lbOtherSettingsTitle, 3);
            lbOtherSettingsTitle.Dock = DockStyle.Fill;
            lbOtherSettingsTitle.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbOtherSettingsTitle.ForeColor = Color.FromArgb(239, 112, 32);
            lbOtherSettingsTitle.ImageAlign = ContentAlignment.TopCenter;
            lbOtherSettingsTitle.Location = new Point(62, 678);
            lbOtherSettingsTitle.Margin = new Padding(6);
            lbOtherSettingsTitle.Name = "lbOtherSettingsTitle";
            lbOtherSettingsTitle.Size = new Size(854, 44);
            lbOtherSettingsTitle.TabIndex = 71;
            lbOtherSettingsTitle.Text = "OTHER SETTINGS";
            lbOtherSettingsTitle.TextAlign = ContentAlignment.BottomCenter;
            // 
            // chxTrusted
            // 
            chxTrusted.AutoSize = true;
            chxTrusted.Checked = true;
            chxTrusted.CheckState = CheckState.Checked;
            chxTrusted.Dock = DockStyle.Fill;
            chxTrusted.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxTrusted.ForeColor = Color.FromArgb(239, 112, 32);
            chxTrusted.Location = new Point(396, 454);
            chxTrusted.Margin = new Padding(16, 6, 6, 6);
            chxTrusted.Name = "chxTrusted";
            chxTrusted.Size = new Size(465, 44);
            chxTrusted.TabIndex = 67;
            chxTrusted.Text = "Trusted Connection";
            chxTrusted.UseVisualStyleBackColor = false;
            chxTrusted.CheckedChanged += chxTrusted_CheckedChanged;
            // 
            // lbDatabaseLabel
            // 
            lbDatabaseLabel.AutoSize = true;
            lbDatabaseLabel.BackColor = Color.Transparent;
            tlpSettings.SetColumnSpan(lbDatabaseLabel, 2);
            lbDatabaseLabel.Dock = DockStyle.Fill;
            lbDatabaseLabel.FlatStyle = FlatStyle.Flat;
            lbDatabaseLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbDatabaseLabel.ForeColor = Color.FromArgb(239, 112, 32);
            lbDatabaseLabel.Location = new Point(6, 398);
            lbDatabaseLabel.Margin = new Padding(6);
            lbDatabaseLabel.Name = "lbDatabaseLabel";
            lbDatabaseLabel.Size = new Size(368, 44);
            lbDatabaseLabel.TabIndex = 57;
            lbDatabaseLabel.Text = "Database: ";
            lbDatabaseLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbServerLabel
            // 
            lbServerLabel.AutoSize = true;
            lbServerLabel.BackColor = Color.Transparent;
            tlpSettings.SetColumnSpan(lbServerLabel, 2);
            lbServerLabel.Dock = DockStyle.Fill;
            lbServerLabel.FlatStyle = FlatStyle.Flat;
            lbServerLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbServerLabel.ForeColor = Color.FromArgb(239, 112, 32);
            lbServerLabel.Location = new Point(6, 342);
            lbServerLabel.Margin = new Padding(6);
            lbServerLabel.Name = "lbServerLabel";
            lbServerLabel.Size = new Size(368, 44);
            lbServerLabel.TabIndex = 55;
            lbServerLabel.Text = "Server: ";
            lbServerLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbSQLTitle
            // 
            lbSQLTitle.AutoSize = true;
            tlpSettings.SetColumnSpan(lbSQLTitle, 3);
            lbSQLTitle.Dock = DockStyle.Fill;
            lbSQLTitle.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbSQLTitle.ForeColor = Color.FromArgb(239, 112, 32);
            lbSQLTitle.ImageAlign = ContentAlignment.TopCenter;
            lbSQLTitle.Location = new Point(62, 286);
            lbSQLTitle.Margin = new Padding(6);
            lbSQLTitle.Name = "lbSQLTitle";
            lbSQLTitle.Size = new Size(854, 44);
            lbSQLTitle.TabIndex = 47;
            lbSQLTitle.Text = "SQL CONNECTION";
            lbSQLTitle.TextAlign = ContentAlignment.BottomCenter;
            // 
            // lbFoldersTitle
            // 
            lbFoldersTitle.AutoSize = true;
            tlpSettings.SetColumnSpan(lbFoldersTitle, 3);
            lbFoldersTitle.Dock = DockStyle.Fill;
            lbFoldersTitle.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold);
            lbFoldersTitle.ForeColor = Color.FromArgb(239, 112, 32);
            lbFoldersTitle.Location = new Point(62, 6);
            lbFoldersTitle.Margin = new Padding(6);
            lbFoldersTitle.Name = "lbFoldersTitle";
            lbFoldersTitle.Size = new Size(854, 44);
            lbFoldersTitle.TabIndex = 46;
            lbFoldersTitle.Text = "FOLDERS";
            lbFoldersTitle.TextAlign = ContentAlignment.BottomCenter;
            // 
            // txbSuccessFolder
            // 
            txbSuccessFolder.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpSettings.SetColumnSpan(txbSuccessFolder, 2);
            txbSuccessFolder.Location = new Point(386, 176);
            txbSuccessFolder.Margin = new Padding(6);
            txbSuccessFolder.Name = "txbSuccessFolder";
            txbSuccessFolder.Size = new Size(530, 39);
            txbSuccessFolder.TabIndex = 51;
            // 
            // txbLogFolder
            // 
            txbLogFolder.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpSettings.SetColumnSpan(txbLogFolder, 2);
            txbLogFolder.Location = new Point(386, 232);
            txbLogFolder.Margin = new Padding(6);
            txbLogFolder.Name = "txbLogFolder";
            txbLogFolder.Size = new Size(530, 39);
            txbLogFolder.TabIndex = 52;
            // 
            // txbServerName
            // 
            txbServerName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpSettings.SetColumnSpan(txbServerName, 2);
            txbServerName.Location = new Point(386, 344);
            txbServerName.Margin = new Padding(6);
            txbServerName.Name = "txbServerName";
            txbServerName.Size = new Size(530, 39);
            txbServerName.TabIndex = 53;
            // 
            // txbDatabaseName
            // 
            txbDatabaseName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpSettings.SetColumnSpan(txbDatabaseName, 2);
            txbDatabaseName.Location = new Point(386, 400);
            txbDatabaseName.Margin = new Padding(6);
            txbDatabaseName.Name = "txbDatabaseName";
            txbDatabaseName.Size = new Size(530, 39);
            txbDatabaseName.TabIndex = 65;
            // 
            // txbPassword
            // 
            txbPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpSettings.SetColumnSpan(txbPassword, 2);
            txbPassword.Location = new Point(386, 568);
            txbPassword.Margin = new Padding(6);
            txbPassword.Name = "txbPassword";
            txbPassword.PasswordChar = '*';
            txbPassword.Size = new Size(530, 39);
            txbPassword.TabIndex = 68;
            // 
            // txbUserName
            // 
            txbUserName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpSettings.SetColumnSpan(txbUserName, 2);
            txbUserName.Location = new Point(386, 512);
            txbUserName.Margin = new Padding(6);
            txbUserName.Name = "txbUserName";
            txbUserName.Size = new Size(530, 39);
            txbUserName.TabIndex = 70;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Dock = DockStyle.Fill;
            label2.FlatStyle = FlatStyle.Flat;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(239, 112, 32);
            label2.Location = new Point(62, 118);
            label2.Margin = new Padding(6);
            label2.Name = "label2";
            label2.Size = new Size(312, 44);
            label2.TabIndex = 73;
            label2.Text = "Folder for failed files:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txbWorkingFolder
            // 
            txbWorkingFolder.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txbWorkingFolder.Location = new Point(383, 64);
            txbWorkingFolder.Name = "txbWorkingFolder";
            txbWorkingFolder.Size = new Size(481, 39);
            txbWorkingFolder.TabIndex = 42;
            // 
            // txbFailedFolder
            // 
            txbFailedFolder.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpSettings.SetColumnSpan(txbFailedFolder, 2);
            txbFailedFolder.Location = new Point(386, 120);
            txbFailedFolder.Margin = new Padding(6);
            txbFailedFolder.Name = "txbFailedFolder";
            txbFailedFolder.Size = new Size(530, 39);
            txbFailedFolder.TabIndex = 50;
            // 
            // btnTest
            // 
            btnTest.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnTest.BackColor = Color.FromArgb(56, 57, 60);
            btnTest.BackgroundImageLayout = ImageLayout.Center;
            tlpSettings.SetColumnSpan(btnTest, 2);
            btnTest.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            btnTest.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            btnTest.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            btnTest.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            btnTest.FlatStyle = FlatStyle.Flat;
            btnTest.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTest.ForeColor = Color.FromArgb(239, 112, 32);
            btnTest.Location = new Point(796, 622);
            btnTest.Margin = new Padding(6);
            btnTest.MaximumSize = new Size(120, 48);
            btnTest.MinimumSize = new Size(120, 48);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(120, 48);
            btnTest.TabIndex = 75;
            btnTest.Text = "Test";
            btnTest.UseVisualStyleBackColor = false;
            btnTest.Click += btnTest_Click;
            // 
            // cbxLogLevel
            // 
            cbxLogLevel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpSettings.SetColumnSpan(cbxLogLevel, 2);
            cbxLogLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxLogLevel.FlatStyle = FlatStyle.Flat;
            cbxLogLevel.FormattingEnabled = true;
            cbxLogLevel.Items.AddRange(new object[] { "Verbose", "Debug", "Information", "Warning", "Error", "Critical", "Fatal" });
            cbxLogLevel.Location = new Point(386, 848);
            cbxLogLevel.Margin = new Padding(6);
            cbxLogLevel.Name = "cbxLogLevel";
            cbxLogLevel.Size = new Size(530, 40);
            cbxLogLevel.TabIndex = 78;
            // 
            // sqlCommand1
            // 
            sqlCommand1.CommandTimeout = 30;
            sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // SettingForm
            // 
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.DimGray;
            ClientSize = new Size(980, 980);
            Controls.Add(tlpSettings);
            ForeColor = Color.FromArgb(248, 248, 249);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(20, 108, 20, 20);
            MinimumSize = new Size(640, 640);
            Name = "SettingForm";
            Load += SettingForm_Load;
            Shown += SettingForm_Shown;
            tlpSettings.ResumeLayout(false);
            tlpSettings.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chxLoadOnStart;
        private TableLayoutPanel tlpSettings;
        private Label lbSuccessFolder;
        private CheckBox chxStaging;
        private Button btnWorkingFolder;
        private Button btnSuccess;
        private Button btnFailed;
        private Button btnSave;
        private TextBox txbWorkingFolder;
        private Label lbFoldersTitle;
        private Label lbSQLTitle;
        private TextBox txbFailedFolder;
        private TextBox txbSuccessFolder;
        private TextBox txbLogFolder;
        private TextBox txbServerName;
        private Label lbDatabaseLabel;
        private Label lbServerLabel;
        private TextBox txbDatabaseName;
        private CheckBox chxTrusted;
        private Label lbOtherSettingsTitle;
        private TextBox txbPassword;
        private TextBox txbUserName;
        private Label label2;
        private Label lbLogFolder;
        private Label lbWorkingFolderLabel;
        private Button btnTest;
        private Label lbUser;
        private Label lbPassword;
        private ComboBox cbxLogLevel;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
    }
}
