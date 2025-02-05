
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
            plLogToolBarPanel = new Panel();
            chxVerbose = new CheckBox();
            chxDebug = new CheckBox();
            chxInformation = new CheckBox();
            chxWarning = new CheckBox();
            chxError = new CheckBox();
            chxFatal = new CheckBox();
            plGridPropertiesPanel = new Panel();
            dataGridViewProperties = new DataGridView();
            dataGridViewLog = new DataGridView();
            
            ((System.ComponentModel.ISupportInitialize)dataGridViewProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewLog).BeginInit();
            plLogToolBarPanel.SuspendLayout();
            plGridPropertiesPanel.SuspendLayout();
            SuspendLayout();
            // 
            // plLogToolBarPanel
            // 
            plLogToolBarPanel.BorderStyle = BorderStyle.FixedSingle;
            plLogToolBarPanel.Controls.Add(chxVerbose);
            plLogToolBarPanel.Controls.Add(chxDebug);
            plLogToolBarPanel.Controls.Add(chxInformation);
            plLogToolBarPanel.Controls.Add(chxWarning);
            plLogToolBarPanel.Controls.Add(chxError);
            plLogToolBarPanel.Controls.Add(chxFatal);
            plLogToolBarPanel.Dock = DockStyle.Top;
            plLogToolBarPanel.Location = new Point(0, 0);
            plLogToolBarPanel.Margin = new Padding(6);
            plLogToolBarPanel.Name = "plLogToolBarPanel";
            plLogToolBarPanel.Size = new Size(2206, 78);
            plLogToolBarPanel.TabIndex = 7;
            // 
            // chxVerbose
            // 
            chxVerbose.AutoSize = true;
            chxVerbose.BackColor = Color.FromArgb(56, 57, 60);
            chxVerbose.Checked = true;
            chxVerbose.CheckState = CheckState.Checked;
            chxVerbose.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            chxVerbose.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            chxVerbose.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            chxVerbose.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            chxVerbose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxVerbose.ForeColor = Color.FromArgb(239, 112, 32);
            chxVerbose.Location = new Point(16, 16);
            chxVerbose.Margin = new Padding(6);
            chxVerbose.Name = "chxVerbose";
            chxVerbose.Size = new Size(137, 36);
            chxVerbose.TabIndex = 1;
            chxVerbose.Text = "Verbose";
            chxVerbose.UseVisualStyleBackColor = false;
            // 
            // chxDebug
            // 
            chxDebug.AutoSize = true;
            chxDebug.BackColor = Color.FromArgb(56, 57, 60);
            chxDebug.Checked = true;
            chxDebug.CheckState = CheckState.Checked;
            chxDebug.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            chxDebug.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            chxDebug.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            chxDebug.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            chxDebug.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxDebug.ForeColor = Color.FromArgb(239, 112, 32);
            chxDebug.Location = new Point(176, 16);
            chxDebug.Margin = new Padding(6);
            chxDebug.Name = "chxDebug";
            chxDebug.Size = new Size(122, 36);
            chxDebug.TabIndex = 2;
            chxDebug.Text = "Debug";
            chxDebug.UseVisualStyleBackColor = false;
            // 
            // chxInformation
            // 
            chxInformation.AutoSize = true;
            chxInformation.BackColor = Color.FromArgb(56, 57, 60);
            chxInformation.Checked = true;
            chxInformation.CheckState = CheckState.Checked;
            chxInformation.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            chxInformation.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            chxInformation.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            chxInformation.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            chxInformation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxInformation.ForeColor = Color.FromArgb(239, 112, 32);
            chxInformation.Location = new Point(320, 16);
            chxInformation.Margin = new Padding(6);
            chxInformation.Name = "chxInformation";
            chxInformation.Size = new Size(93, 36);
            chxInformation.TabIndex = 3;
            chxInformation.Text = "Info";
            chxInformation.UseVisualStyleBackColor = false;
            // 
            // chxWarning
            // 
            chxWarning.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            chxWarning.AutoSize = true;
            chxWarning.BackColor = Color.FromArgb(56, 57, 60);
            chxWarning.Checked = true;
            chxWarning.CheckState = CheckState.Checked;
            chxWarning.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            chxWarning.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            chxWarning.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            chxWarning.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            chxWarning.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxWarning.ForeColor = Color.FromArgb(239, 112, 32);
            chxWarning.Location = new Point(440, 16);
            chxWarning.Margin = new Padding(6);
            chxWarning.Name = "chxWarning";
            chxWarning.Size = new Size(144, 36);
            chxWarning.TabIndex = 4;
            chxWarning.Text = "Warning";
            chxWarning.UseVisualStyleBackColor = false;
            // 
            // chxError
            // 
            chxError.AutoSize = true;
            chxError.BackColor = Color.FromArgb(56, 57, 60);
            chxError.Checked = true;
            chxError.CheckState = CheckState.Checked;
            chxError.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            chxError.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            chxError.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            chxError.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            chxError.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxError.ForeColor = Color.FromArgb(239, 112, 32);
            chxError.Location = new Point(600, 16);
            chxError.Margin = new Padding(6);
            chxError.Name = "chxError";
            chxError.Size = new Size(104, 36);
            chxError.TabIndex = 5;
            chxError.Text = "Error";
            chxError.UseVisualStyleBackColor = false;
            // 
            // chxFatal
            // 
            chxFatal.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            chxFatal.AutoSize = true;
            chxFatal.BackColor = Color.FromArgb(56, 57, 60);
            chxFatal.Checked = true;
            chxFatal.CheckState = CheckState.Checked;
            chxFatal.FlatAppearance.BorderColor = Color.FromArgb(239, 112, 32);
            chxFatal.FlatAppearance.CheckedBackColor = Color.FromArgb(239, 112, 32);
            chxFatal.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 249);
            chxFatal.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 112, 32);
            chxFatal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chxFatal.ForeColor = Color.FromArgb(239, 112, 32);
            chxFatal.Location = new Point(720, 16);
            chxFatal.Margin = new Padding(6);
            chxFatal.Name = "chxFatal";
            chxFatal.Size = new Size(99, 36);
            chxFatal.TabIndex = 6;
            chxFatal.Text = "Fatal";
            chxFatal.UseVisualStyleBackColor = false;
            // 
            // plGridPropertiesPanel
            // 
            plGridPropertiesPanel.Controls.Add(dataGridViewProperties);
            plGridPropertiesPanel.Dock = DockStyle.Right;
            plGridPropertiesPanel.Location = new Point(1506, 78);
            plGridPropertiesPanel.Margin = new Padding(6);
            plGridPropertiesPanel.Name = "plGridPropertiesPanel";
            plGridPropertiesPanel.Size = new Size(700, 1175);
            plGridPropertiesPanel.TabIndex = 8;
            // 
            // dataGridViewProperties
            // 
            dataGridViewProperties.AllowUserToAddRows = false;
            dataGridViewProperties.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProperties.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewProperties.BackgroundColor = Color.DimGray;
            dataGridViewProperties.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewProperties.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewProperties.Dock = DockStyle.Fill;
            dataGridViewProperties.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewProperties.EnableHeadersVisualStyles = false;
            dataGridViewProperties.GridColor = Color.FromArgb(239, 112, 32);
            dataGridViewProperties.Location = new Point(0, 0);
            dataGridViewProperties.Margin = new Padding(6);
            dataGridViewProperties.MultiSelect = false;
            dataGridViewProperties.Name = "dataGridViewProperties";
            dataGridViewProperties.RightToLeft = RightToLeft.No;
            dataGridViewProperties.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewProperties.RowHeadersVisible = false;
            dataGridViewProperties.RowHeadersWidth = 82;
            dataGridViewProperties.Size = new Size(700, 1175);
            dataGridViewProperties.TabIndex = 1;
            dataGridViewProperties.VirtualMode = true;
            // 
            // dataGridViewLog
            // 
            dataGridViewLog.AllowUserToAddRows = false;
            dataGridViewLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewLog.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewLog.BackgroundColor = Color.DimGray;
            dataGridViewLog.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewLog.Dock = DockStyle.Fill;
            dataGridViewLog.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewLog.EnableHeadersVisualStyles = false;
            dataGridViewLog.GridColor = Color.FromArgb(239, 112, 32);
            dataGridViewLog.Location = new Point(0, 78);
            dataGridViewLog.Margin = new Padding(6);
            dataGridViewLog.MultiSelect = false;
            dataGridViewLog.Name = "dataGridViewLog";
            dataGridViewLog.RightToLeft = RightToLeft.No;
            dataGridViewLog.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewLog.RowHeadersVisible = false;
            dataGridViewLog.RowHeadersWidth = 82;
            dataGridViewLog.Size = new Size(1506, 1175);
            dataGridViewLog.TabIndex = 9;
            dataGridViewLog.VirtualMode = true;
            // 
            // LogForm
            // 
            BackColor = Color.FromArgb(56, 57, 60);
            ClientSize = new Size(2206, 1253);
            Controls.Add(dataGridViewLog);
            Controls.Add(plGridPropertiesPanel);
            Controls.Add(plLogToolBarPanel);
            ForeColor = Color.FromArgb(248, 248, 249);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "LogForm";
            Text = "LogForm";
            WindowState = FormWindowState.Maximized;
            ResizeBegin += LogForm_ResizeBegin;
            ResizeEnd += LogForm_ResizeEnd;
            plLogToolBarPanel.ResumeLayout(false);
            plLogToolBarPanel.PerformLayout();
            plGridPropertiesPanel.ResumeLayout(false);
            plGridPropertiesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewLog).EndInit();
            ResumeLayout(false);
            PerformLayout();

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


