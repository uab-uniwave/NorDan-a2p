

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            dataGridViewLog = new DataGridView();
            LogGridMenuStrip = new ContextMenuStrip(components);
            saveLogToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridViewLog).BeginInit();
            LogGridMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewLog
            // 
            dataGridViewLog.AllowUserToAddRows = false;
            dataGridViewLog.AllowUserToDeleteRows = false;
            dataGridViewLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewLog.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewLog.BackgroundColor = Color.DimGray;
            dataGridViewLog.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewLog.ContextMenuStrip = LogGridMenuStrip;
            dataGridViewLog.Dock = DockStyle.Fill;
            dataGridViewLog.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewLog.EnableHeadersVisualStyles = false;
            dataGridViewLog.GridColor = Color.FromArgb(239, 112, 32);
            dataGridViewLog.Location = new Point(0, 0);
            dataGridViewLog.Margin = new Padding(6);
            dataGridViewLog.MultiSelect = false;
            dataGridViewLog.Name = "dataGridViewLog";
            dataGridViewLog.RightToLeft = RightToLeft.No;
            dataGridViewLog.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewLog.RowHeadersVisible = false;
            dataGridViewLog.RowHeadersWidth = 82;
            dataGridViewLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewLog.Size = new Size(2206, 1253);
            dataGridViewLog.TabIndex = 9;
            dataGridViewLog.VirtualMode = true;
            dataGridViewLog.DataError += dataGridViewLog_DataError;
            // 
            // LogGridMenuStrip
            // 
            LogGridMenuStrip.BackColor = Color.FromArgb(56, 57, 60);
            LogGridMenuStrip.ImageScalingSize = new Size(32, 32);
            LogGridMenuStrip.Items.AddRange(new ToolStripItem[] { saveLogToolStripMenuItem });
            LogGridMenuStrip.Name = "contextMenuStrip1";
            LogGridMenuStrip.Size = new Size(122, 26);
            // 
            // saveLogToolStripMenuItem
            // 
            saveLogToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            saveLogToolStripMenuItem.Name = "saveLogToolStripMenuItem";
            saveLogToolStripMenuItem.Size = new Size(121, 22);
            saveLogToolStripMenuItem.Text = "Save Log";
            saveLogToolStripMenuItem.Click += saveLogToolStripMenuItem_Click;
            // 
            // LogForm
            // 
            BackColor = Color.FromArgb(56, 57, 60);
            ClientSize = new Size(2206, 1253);
            Controls.Add(dataGridViewLog);
            ForeColor = Color.FromArgb(248, 248, 249);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6);
            Name = "LogForm";
            Text = "A2P-Log Form ";
            WindowState = FormWindowState.Maximized;
            Load += LogForm_Load;
            Shown += LogForm_Shown;
            DpiChanged += LogForm_DpiChanged;
            ResizeBegin += LogForm_ResizeBegin;
            ResizeEnd += LogForm_ResizeEnd;
            ((System.ComponentModel.ISupportInitialize)dataGridViewLog).EndInit();
            LogGridMenuStrip.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private DataGridView dataGridViewLog;
        private ContextMenuStrip LogGridMenuStrip;
        private ToolStripMenuItem saveLogToolStripMenuItem;
    }
}


