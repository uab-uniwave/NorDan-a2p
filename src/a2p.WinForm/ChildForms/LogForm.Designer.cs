
using a2p.WinForm;
namespace a2p.WinForm.ChildForms
{
    partial class LogForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.plLogToolBarPanel = new Panel();
            this.chxVerbose = new CheckBox();
            this.chxDebug = new CheckBox();
            this.chxInformation = new CheckBox();
            this.chxWarning = new CheckBox();
            this.chxError = new CheckBox();
            this.chxFatal = new CheckBox();
            this.plGridPropertiesPanel = new Panel();
            this.dataGridViewProperties = new DataGridView();
            this.dataGridViewLog = new DataGridView();
            this.plLogToolBarPanel.SuspendLayout();
            this.plGridPropertiesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewLog).BeginInit();
            SuspendLayout();
            // 
            // plLogToolBarPanel
            // 
            this.plLogToolBarPanel.BorderStyle = BorderStyle.FixedSingle;
            this.plLogToolBarPanel.Controls.Add(this.chxVerbose);
            this.plLogToolBarPanel.Controls.Add(this.chxDebug);
            this.plLogToolBarPanel.Controls.Add(this.chxInformation);
            this.plLogToolBarPanel.Controls.Add(this.chxWarning);
            this.plLogToolBarPanel.Controls.Add(this.chxError);
            this.plLogToolBarPanel.Controls.Add(this.chxFatal);
            this.plLogToolBarPanel.Dock = DockStyle.Top;
            this.plLogToolBarPanel.Location = new Point(0, 0);
            this.plLogToolBarPanel.Margin = new Padding(6);
            this.plLogToolBarPanel.Name = "plLogToolBarPanel";
            this.plLogToolBarPanel.Size = new Size(2206, 78);
            this.plLogToolBarPanel.TabIndex = 7;
            // 
            // chxVerbose
            // 
            this.chxVerbose.AutoSize = true;
            this.chxVerbose.BackColor = UniwaveColors.a2pGreyDark;
            this.chxVerbose.Checked = true;
            this.chxVerbose.CheckState = CheckState.Checked;
            this.chxVerbose.FlatAppearance.BorderColor = UniwaveColors.uwOrangeDeep;
            this.chxVerbose.FlatAppearance.CheckedBackColor = UniwaveColors.uwOrangeDeep;
            this.chxVerbose.FlatAppearance.MouseDownBackColor = UniwaveColors.uwGreyLight;
            this.chxVerbose.FlatAppearance.MouseOverBackColor = UniwaveColors.uwOrangeDeep;
            this.chxVerbose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.chxVerbose.ForeColor = UniwaveColors.uwOrangeDeep;
            this.chxVerbose.Location = new Point(16, 16);
            this.chxVerbose.Margin = new Padding(6);
            this.chxVerbose.Name = "chxVerbose";
            this.chxVerbose.Size = new Size(71, 19);
            this.chxVerbose.TabIndex = 1;
            this.chxVerbose.Text = "Verbose";
            this.chxVerbose.UseVisualStyleBackColor = false;
            // 
            // chxDebug
            // 
            this.chxDebug.AutoSize = true;
            this.chxDebug.BackColor = UniwaveColors.a2pGreyDark;
            this.chxDebug.Checked = true;
            this.chxDebug.CheckState = CheckState.Checked;
            this.chxDebug.FlatAppearance.BorderColor = UniwaveColors.uwOrangeDeep;
            this.chxDebug.FlatAppearance.CheckedBackColor = UniwaveColors.uwOrangeDeep;
            this.chxDebug.FlatAppearance.MouseDownBackColor = UniwaveColors.uwGreyLight;
            this.chxDebug.FlatAppearance.MouseOverBackColor = UniwaveColors.uwOrangeDeep;
            this.chxDebug.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.chxDebug.ForeColor = UniwaveColors.uwOrangeDeep;
            this.chxDebug.Location = new Point(176, 16);
            this.chxDebug.Margin = new Padding(6);
            this.chxDebug.Name = "chxDebug";
            this.chxDebug.Size = new Size(63, 19);
            this.chxDebug.TabIndex = 2;
            this.chxDebug.Text = "Debug";
            this.chxDebug.UseVisualStyleBackColor = false;
            // 
            // chxInformation
            // 
            this.chxInformation.AutoSize = true;
            this.chxInformation.BackColor = UniwaveColors.a2pGreyDark;
            this.chxInformation.Checked = true;
            this.chxInformation.CheckState = CheckState.Checked;
            this.chxInformation.FlatAppearance.BorderColor = UniwaveColors.uwOrangeDeep;
            this.chxInformation.FlatAppearance.CheckedBackColor = UniwaveColors.uwOrangeDeep;
            this.chxInformation.FlatAppearance.MouseDownBackColor = UniwaveColors.uwGreyLight;
            this.chxInformation.FlatAppearance.MouseOverBackColor = UniwaveColors.uwOrangeDeep;
            this.chxInformation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.chxInformation.ForeColor = UniwaveColors.uwOrangeDeep;
            this.chxInformation.Location = new Point(320, 16);
            this.chxInformation.Margin = new Padding(6);
            this.chxInformation.Name = "chxInformation";
            this.chxInformation.Size = new Size(49, 19);
            this.chxInformation.TabIndex = 3;
            this.chxInformation.Text = "Info";
            this.chxInformation.UseVisualStyleBackColor = false;
            // 
            // chxWarning
            // 
            this.chxWarning.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.chxWarning.AutoSize = true;
            this.chxWarning.BackColor = UniwaveColors.a2pGreyDark;
            this.chxWarning.Checked = true;
            this.chxWarning.CheckState = CheckState.Checked;
            this.chxWarning.FlatAppearance.BorderColor = UniwaveColors.uwOrangeDeep;
            this.chxWarning.FlatAppearance.CheckedBackColor = UniwaveColors.uwOrangeDeep;
            this.chxWarning.FlatAppearance.MouseDownBackColor = UniwaveColors.uwGreyLight;
            this.chxWarning.FlatAppearance.MouseOverBackColor = UniwaveColors.uwOrangeDeep;
            this.chxWarning.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.chxWarning.ForeColor = UniwaveColors.uwOrangeDeep;
            this.chxWarning.Location = new Point(440, 16);
            this.chxWarning.Margin = new Padding(6);
            this.chxWarning.Name = "chxWarning";
            this.chxWarning.Size = new Size(73, 19);
            this.chxWarning.TabIndex = 4;
            this.chxWarning.Text = "Warning";
            this.chxWarning.UseVisualStyleBackColor = false;
            // 
            // chxError
            // 
            this.chxError.AutoSize = true;
            this.chxError.BackColor = UniwaveColors.a2pGreyDark;
            this.chxError.Checked = true;
            this.chxError.CheckState = CheckState.Checked;
            this.chxError.FlatAppearance.BorderColor = UniwaveColors.uwOrangeDeep;
            this.chxError.FlatAppearance.CheckedBackColor = UniwaveColors.uwOrangeDeep;
            this.chxError.FlatAppearance.MouseDownBackColor = UniwaveColors.uwGreyLight;
            this.chxError.FlatAppearance.MouseOverBackColor = UniwaveColors.uwOrangeDeep;
            this.chxError.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.chxError.ForeColor = UniwaveColors.uwOrangeDeep;
            this.chxError.Location = new Point(600, 16);
            this.chxError.Margin = new Padding(6);
            this.chxError.Name = "chxError";
            this.chxError.Size = new Size(54, 19);
            this.chxError.TabIndex = 5;
            this.chxError.Text = "Error";
            this.chxError.UseVisualStyleBackColor = false;
            // 
            // chxFatal
            // 
            this.chxFatal.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.chxFatal.AutoSize = true;
            this.chxFatal.BackColor = UniwaveColors.a2pGreyDark;
            this.chxFatal.Checked = true;
            this.chxFatal.CheckState = CheckState.Checked;
            this.chxFatal.FlatAppearance.BorderColor = UniwaveColors.uwOrangeDeep;
            this.chxFatal.FlatAppearance.CheckedBackColor = UniwaveColors.uwOrangeDeep;
            this.chxFatal.FlatAppearance.MouseDownBackColor = UniwaveColors.uwGreyLight;
            this.chxFatal.FlatAppearance.MouseOverBackColor = UniwaveColors.uwOrangeDeep;
            this.chxFatal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.chxFatal.ForeColor = UniwaveColors.uwOrangeDeep;
            this.chxFatal.Location = new Point(720, 16);
            this.chxFatal.Margin = new Padding(6);
            this.chxFatal.Name = "chxFatal";
            this.chxFatal.Size = new Size(52, 19);
            this.chxFatal.TabIndex = 6;
            this.chxFatal.Text = "Fatal";
            this.chxFatal.UseVisualStyleBackColor = false;
            // 
            // plGridPropertiesPanel
            // 
            this.plGridPropertiesPanel.Controls.Add(this.dataGridViewProperties);
            this.plGridPropertiesPanel.Dock = DockStyle.Right;
            this.plGridPropertiesPanel.Location = new Point(1506, 78);
            this.plGridPropertiesPanel.Margin = new Padding(6);
            this.plGridPropertiesPanel.Name = "plGridPropertiesPanel";
            this.plGridPropertiesPanel.Size = new Size(700, 1175);
            this.plGridPropertiesPanel.TabIndex = 8;
            // 
            // dataGridViewProperties
            // 
            this.dataGridViewProperties.AllowUserToAddRows = false;
            this.dataGridViewProperties.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewProperties.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewProperties.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewProperties.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProperties.Dock = DockStyle.Fill;
            this.dataGridViewProperties.EditMode = DataGridViewEditMode.EditOnEnter;
            this.dataGridViewProperties.EnableHeadersVisualStyles = false;
            this.dataGridViewProperties.GridColor = UniwaveColors.uwOrangeDeep;
            this.dataGridViewProperties.Location = new Point(0, 0);
            this.dataGridViewProperties.Margin = new Padding(6);
            this.dataGridViewProperties.MultiSelect = false;
            this.dataGridViewProperties.Name = "dataGridViewProperties";
            this.dataGridViewProperties.RightToLeft = RightToLeft.No;
            this.dataGridViewProperties.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewProperties.RowHeadersVisible = false;
            this.dataGridViewProperties.RowHeadersWidth = 82;
            this.dataGridViewProperties.Size = new Size(700, 1175);
            this.dataGridViewProperties.TabIndex = 1;
            this.dataGridViewProperties.VirtualMode = true;
            // 
            // dataGridViewLog
            // 
            this.dataGridViewLog.AllowUserToAddRows = false;
            this.dataGridViewLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLog.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewLog.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLog.Dock = DockStyle.Fill;
            this.dataGridViewLog.EditMode = DataGridViewEditMode.EditOnEnter;
            this.dataGridViewLog.EnableHeadersVisualStyles = false;
            this.dataGridViewLog.GridColor = UniwaveColors.uwOrangeDeep;
            this.dataGridViewLog.Location = new Point(0, 78);
            this.dataGridViewLog.Margin = new Padding(6);
            this.dataGridViewLog.MultiSelect = false;
            this.dataGridViewLog.Name = "dataGridViewLog";
            this.dataGridViewLog.RightToLeft = RightToLeft.No;
            this.dataGridViewLog.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewLog.RowHeadersVisible = false;
            this.dataGridViewLog.RowHeadersWidth = 82;
            this.dataGridViewLog.Size = new Size(1506, 1175);
            this.dataGridViewLog.TabIndex = 9;
            this.dataGridViewLog.VirtualMode = true;
            // 
            // LogForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UniwaveColors.a2pGreyDark;
            ClientSize = new Size(2206, 1253);
            Controls.Add(this.dataGridViewLog);
            Controls.Add(this.plGridPropertiesPanel);
            Controls.Add(this.plLogToolBarPanel);
            ForeColor = UniwaveColors.uwGreyLight;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "LogForm";
            Text = "LogForm";
            WindowState = FormWindowState.Maximized;
            ResizeBegin += LogForm_ResizeBegin;
            ResizeEnd += LogForm_ResizeEnd;
            this.plLogToolBarPanel.ResumeLayout(false);
            this.plLogToolBarPanel.PerformLayout();
            this.plGridPropertiesPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewLog).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private Panel plLogToolBarPanel;
        private CheckBox chxVerbose;
        private CheckBox chxDebug;
        private CheckBox chxInformation;
        private CheckBox chxWarning;
        private CheckBox chxError;
        private CheckBox chxFatal;
        private Panel plGridPropertiesPanel;
        private DataGridView dataGridViewProperties;
        private DataGridView dataGridViewLog;
    }
}


