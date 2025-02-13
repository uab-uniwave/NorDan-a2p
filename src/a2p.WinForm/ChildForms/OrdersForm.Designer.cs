using System.Windows.Forms;

namespace a2p.WinForm.ChildForms
{
 partial class OrdersForm
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

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            plGridPanel = new Panel();
            dataGridViewFiles = new DataGridView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            selectAllToolStripMenuItem = new ToolStripMenuItem();
            deselectAllToolStripMenuItem = new ToolStripMenuItem();
            plTbSBInfo = new TableLayoutPanel();
            lbInfoErrors = new Label();
            lbInfoFiles = new Label();
            lbInfoErrorCount = new Label();
            lbInfoWarningCount = new Label();
            lbInfoWarnings = new Label();
            lbInfoRowsCount = new Label();
            lbInfoWorksheetsCount = new Label();
            lbInfoOrdersCount = new Label();
            lbInfoFilesCount = new Label();
            lbInfoRows = new Label();
            lbInfoWorksheets = new Label();
            lbInfoOrders = new Label();
            plGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).BeginInit();
            contextMenuStrip1.SuspendLayout();
            plTbSBInfo.SuspendLayout();
            SuspendLayout();
            // 
            // plGridPanel
            // 
            plGridPanel.AutoScroll = true;
            plGridPanel.BackColor = Color.Transparent;
            plGridPanel.Controls.Add(dataGridViewFiles);
            plGridPanel.Controls.Add(plTbSBInfo);
            plGridPanel.Dock = DockStyle.Fill;
            plGridPanel.ForeColor = Color.Transparent;
            plGridPanel.Location = new Point(0, 0);
            plGridPanel.Margin = new Padding(12);
            plGridPanel.Name = "plGridPanel";
            plGridPanel.Size = new Size(1600, 800);
            plGridPanel.TabIndex = 1;
            // 
            // dataGridViewFiles
            // 
            dataGridViewFiles.AllowUserToAddRows = false;
            dataGridViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewFiles.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewFiles.BackgroundColor = Color.DimGray;
            dataGridViewFiles.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFiles.ContextMenuStrip = contextMenuStrip1;
            dataGridViewFiles.Dock = DockStyle.Fill;
            dataGridViewFiles.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewFiles.EnableHeadersVisualStyles = false;
            dataGridViewFiles.GridColor = Color.FromArgb(239, 112, 32);
            dataGridViewFiles.Location = new Point(0, 0);
            dataGridViewFiles.Margin = new Padding(12);
            dataGridViewFiles.MultiSelect = false;
            dataGridViewFiles.Name = "dataGridViewFiles";
            dataGridViewFiles.RightToLeft = RightToLeft.No;
            dataGridViewFiles.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewFiles.RowHeadersVisible = false;
            dataGridViewFiles.RowHeadersWidth = 82;
            dataGridViewFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFiles.Size = new Size(1308, 800);
            dataGridViewFiles.TabIndex = 61;
            dataGridViewFiles.VirtualMode = true;
            dataGridViewFiles.CellFormatting += dataGridViewFiles_CellFormatting;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.BackColor = Color.DimGray;
            contextMenuStrip1.BackgroundImageLayout = ImageLayout.None;
            contextMenuStrip1.ImageScalingSize = new Size(32, 32);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { selectAllToolStripMenuItem, deselectAllToolStripMenuItem });
            contextMenuStrip1.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.RenderMode = ToolStripRenderMode.Professional;
            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.Size = new Size(189, 80);
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.Size = new Size(188, 38);
            selectAllToolStripMenuItem.Text = "Select All";
            selectAllToolStripMenuItem.Click += selectAllToolStripMenuItem_Click;
            // 
            // deselectAllToolStripMenuItem
            // 
            deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
            deselectAllToolStripMenuItem.Size = new Size(188, 38);
            deselectAllToolStripMenuItem.Text = "Deselect All";
            deselectAllToolStripMenuItem.Click += deselectAllToolStripMenuItem_Click;
            // 
            // plTbSBInfo
            // 
            plTbSBInfo.BackColor = Color.Transparent;
            plTbSBInfo.ColumnCount = 2;
            plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
            plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            plTbSBInfo.Controls.Add(lbInfoErrors, 0, 6);
            plTbSBInfo.Controls.Add(lbInfoFiles, 0, 0);
            plTbSBInfo.Controls.Add(lbInfoErrorCount, 1, 6);
            plTbSBInfo.Controls.Add(lbInfoWarningCount, 1, 5);
            plTbSBInfo.Controls.Add(lbInfoWarnings, 0, 5);
            plTbSBInfo.Controls.Add(lbInfoRowsCount, 1, 3);
            plTbSBInfo.Controls.Add(lbInfoWorksheetsCount, 1, 2);
            plTbSBInfo.Controls.Add(lbInfoOrdersCount, 1, 1);
            plTbSBInfo.Controls.Add(lbInfoFilesCount, 1, 0);
            plTbSBInfo.Controls.Add(lbInfoRows, 0, 3);
            plTbSBInfo.Controls.Add(lbInfoWorksheets, 0, 2);
            plTbSBInfo.Controls.Add(lbInfoOrders, 0, 1);
            plTbSBInfo.Dock = DockStyle.Right;
            plTbSBInfo.ForeColor = Color.FromArgb(248, 248, 249);
            plTbSBInfo.Location = new Point(1308, 0);
            plTbSBInfo.Margin = new Padding(6);
            plTbSBInfo.Name = "plTbSBInfo";
            plTbSBInfo.RowCount = 11;
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            plTbSBInfo.Size = new Size(292, 800);
            plTbSBInfo.TabIndex = 60;
            // 
            // lbInfoErrors
            // 
            lbInfoErrors.AutoSize = true;
            lbInfoErrors.Dock = DockStyle.Fill;
            lbInfoErrors.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoErrors.ForeColor = Color.Crimson;
            lbInfoErrors.Location = new Point(6, 342);
            lbInfoErrors.Margin = new Padding(6);
            lbInfoErrors.Name = "lbInfoErrors";
            lbInfoErrors.Size = new Size(153, 44);
            lbInfoErrors.TabIndex = 9;
            lbInfoErrors.Text = "Errors:";
            lbInfoErrors.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoFiles
            // 
            lbInfoFiles.AutoSize = true;
            lbInfoFiles.Dock = DockStyle.Fill;
            lbInfoFiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoFiles.ForeColor = Color.DarkGray;
            lbInfoFiles.Location = new Point(6, 6);
            lbInfoFiles.Margin = new Padding(6);
            lbInfoFiles.Name = "lbInfoFiles";
            lbInfoFiles.Size = new Size(153, 44);
            lbInfoFiles.TabIndex = 4;
            lbInfoFiles.Text = "Files:";
            lbInfoFiles.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoErrorCount
            // 
            lbInfoErrorCount.AutoSize = true;
            lbInfoErrorCount.Dock = DockStyle.Fill;
            lbInfoErrorCount.Font = new Font("Segoe UI", 9F);
            lbInfoErrorCount.ForeColor = Color.Red;
            lbInfoErrorCount.Location = new Point(171, 342);
            lbInfoErrorCount.Margin = new Padding(6);
            lbInfoErrorCount.Name = "lbInfoErrorCount";
            lbInfoErrorCount.Size = new Size(115, 44);
            lbInfoErrorCount.TabIndex = 0;
            lbInfoErrorCount.Text = "0";
            lbInfoErrorCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWarningCount
            // 
            lbInfoWarningCount.AutoSize = true;
            lbInfoWarningCount.Dock = DockStyle.Fill;
            lbInfoWarningCount.Font = new Font("Segoe UI", 9F);
            lbInfoWarningCount.ForeColor = Color.Coral;
            lbInfoWarningCount.Location = new Point(171, 286);
            lbInfoWarningCount.Margin = new Padding(6);
            lbInfoWarningCount.Name = "lbInfoWarningCount";
            lbInfoWarningCount.Size = new Size(115, 44);
            lbInfoWarningCount.TabIndex = 0;
            lbInfoWarningCount.Text = "0";
            lbInfoWarningCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWarnings
            // 
            lbInfoWarnings.AutoSize = true;
            lbInfoWarnings.Dock = DockStyle.Fill;
            lbInfoWarnings.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoWarnings.ForeColor = Color.Coral;
            lbInfoWarnings.Location = new Point(6, 286);
            lbInfoWarnings.Margin = new Padding(6);
            lbInfoWarnings.Name = "lbInfoWarnings";
            lbInfoWarnings.Size = new Size(153, 44);
            lbInfoWarnings.TabIndex = 0;
            lbInfoWarnings.Text = "Warnings:";
            lbInfoWarnings.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoRowsCount
            // 
            lbInfoRowsCount.AutoSize = true;
            lbInfoRowsCount.Dock = DockStyle.Fill;
            lbInfoRowsCount.FlatStyle = FlatStyle.Flat;
            lbInfoRowsCount.Font = new Font("Segoe UI", 9F);
            lbInfoRowsCount.ForeColor = Color.DarkGray;
            lbInfoRowsCount.Location = new Point(171, 174);
            lbInfoRowsCount.Margin = new Padding(6);
            lbInfoRowsCount.Name = "lbInfoRowsCount";
            lbInfoRowsCount.Size = new Size(115, 44);
            lbInfoRowsCount.TabIndex = 0;
            lbInfoRowsCount.Text = "10";
            lbInfoRowsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWorksheetsCount
            // 
            lbInfoWorksheetsCount.AutoSize = true;
            lbInfoWorksheetsCount.Dock = DockStyle.Fill;
            lbInfoWorksheetsCount.FlatStyle = FlatStyle.Flat;
            lbInfoWorksheetsCount.Font = new Font("Segoe UI", 9F);
            lbInfoWorksheetsCount.ForeColor = Color.DarkGray;
            lbInfoWorksheetsCount.Location = new Point(171, 118);
            lbInfoWorksheetsCount.Margin = new Padding(6);
            lbInfoWorksheetsCount.Name = "lbInfoWorksheetsCount";
            lbInfoWorksheetsCount.Size = new Size(115, 44);
            lbInfoWorksheetsCount.TabIndex = 0;
            lbInfoWorksheetsCount.Text = "10";
            lbInfoWorksheetsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoOrdersCount
            // 
            lbInfoOrdersCount.AutoSize = true;
            lbInfoOrdersCount.Dock = DockStyle.Fill;
            lbInfoOrdersCount.FlatStyle = FlatStyle.Flat;
            lbInfoOrdersCount.Font = new Font("Segoe UI", 9F);
            lbInfoOrdersCount.ForeColor = Color.DarkGray;
            lbInfoOrdersCount.Location = new Point(171, 62);
            lbInfoOrdersCount.Margin = new Padding(6);
            lbInfoOrdersCount.Name = "lbInfoOrdersCount";
            lbInfoOrdersCount.Size = new Size(115, 44);
            lbInfoOrdersCount.TabIndex = 0;
            lbInfoOrdersCount.Text = "10";
            lbInfoOrdersCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoFilesCount
            // 
            lbInfoFilesCount.AutoSize = true;
            lbInfoFilesCount.BackColor = Color.FromArgb(56, 57, 60);
            lbInfoFilesCount.Dock = DockStyle.Fill;
            lbInfoFilesCount.FlatStyle = FlatStyle.Flat;
            lbInfoFilesCount.Font = new Font("Segoe UI", 9F);
            lbInfoFilesCount.ForeColor = Color.DarkGray;
            lbInfoFilesCount.Location = new Point(171, 6);
            lbInfoFilesCount.Margin = new Padding(6);
            lbInfoFilesCount.Name = "lbInfoFilesCount";
            lbInfoFilesCount.Size = new Size(115, 44);
            lbInfoFilesCount.TabIndex = 0;
            lbInfoFilesCount.Text = "10";
            lbInfoFilesCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoRows
            // 
            lbInfoRows.AutoSize = true;
            lbInfoRows.Dock = DockStyle.Fill;
            lbInfoRows.FlatStyle = FlatStyle.Flat;
            lbInfoRows.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoRows.ForeColor = Color.DarkGray;
            lbInfoRows.Location = new Point(6, 174);
            lbInfoRows.Margin = new Padding(6);
            lbInfoRows.Name = "lbInfoRows";
            lbInfoRows.Size = new Size(153, 44);
            lbInfoRows.TabIndex = 0;
            lbInfoRows.Text = "Rows:";
            lbInfoRows.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoWorksheets
            // 
            lbInfoWorksheets.AutoSize = true;
            lbInfoWorksheets.Dock = DockStyle.Fill;
            lbInfoWorksheets.FlatStyle = FlatStyle.Flat;
            lbInfoWorksheets.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoWorksheets.ForeColor = Color.DarkGray;
            lbInfoWorksheets.Location = new Point(6, 118);
            lbInfoWorksheets.Margin = new Padding(6);
            lbInfoWorksheets.Name = "lbInfoWorksheets";
            lbInfoWorksheets.Size = new Size(153, 44);
            lbInfoWorksheets.TabIndex = 0;
            lbInfoWorksheets.Text = "Worksheets:";
            lbInfoWorksheets.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoOrders
            // 
            lbInfoOrders.AutoSize = true;
            lbInfoOrders.Dock = DockStyle.Fill;
            lbInfoOrders.FlatStyle = FlatStyle.Flat;
            lbInfoOrders.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoOrders.ForeColor = Color.DarkGray;
            lbInfoOrders.Location = new Point(6, 62);
            lbInfoOrders.Margin = new Padding(6);
            lbInfoOrders.Name = "lbInfoOrders";
            lbInfoOrders.Size = new Size(153, 44);
            lbInfoOrders.TabIndex = 3;
            lbInfoOrders.Text = "Orders:";
            lbInfoOrders.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // OrdersForm
            // 
            BackColor = Color.FromArgb(56, 57, 60);
            ClientSize = new Size(1600, 800);
            Controls.Add(plGridPanel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(12);
            Name = "OrdersForm";
            Text = "FileForm";
            Load += OrdersForm_Load;
            Shown += FileForm_Shown;
            DpiChanged += OrdersForm_DpiChanged;
            ResizeBegin += FileForm_ResizeBegin;
            ResizeEnd += FileForm_ResizeEnd;
            plGridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            plTbSBInfo.ResumeLayout(false);
            plTbSBInfo.PerformLayout();
            ResumeLayout(false);



        }

        #endregion
        private Panel plGridPanel;
        private TableLayoutPanel plTbSBInfo;
        private Label lbInfoErrors;
        private Label lbInfoFiles;
        private Label lbInfoErrorCount;
        private Label lbInfoWarningCount;
        private Label lbInfoWarnings;
        private Label lbInfoRowsCount;
        private Label lbInfoWorksheetsCount;
        private Label lbInfoOrdersCount;
        private Label lbInfoFilesCount;
        private Label lbInfoRows;
        private Label lbInfoWorksheets;
        private Label lbInfoOrders;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem deselectAllToolStripMenuItem;
        public DataGridView dataGridViewFiles;
    }
}