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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrdersForm));
            plGridPanel = new Panel();
            dataGridViewFiles = new DataGridView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            selectAllToolStripMenuItem = new ToolStripMenuItem();
            deselectAllToolStripMenuItem = new ToolStripMenuItem();
            plTbSBInfo = new TableLayoutPanel();
            lbTitle = new Label();
            lbInfoOrders = new Label();
            lbInfoOrdersCount = new Label();
            lbInfoFiles = new Label();
            lbInfoFilesCount = new Label();
            lbInfoWorksheets = new Label();
            lbInfoWorksheetsCount = new Label();
            lbInfoItems = new Label();
            lbInfoItemsCount = new Label();
            lbInfoMaterialCount = new Label();
            lbInfoMaterial = new Label();
            lbInfoWarnings = new Label();
            lbInfoWarningCount = new Label();
            lbInfoErrors = new Label();
            lbInfoErrorCount = new Label();
            lbInfoRows = new Label();
            imageList1 = new ImageList(components);
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
            dataGridViewFiles.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewFiles.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFiles.ContextMenuStrip = contextMenuStrip1;
            dataGridViewFiles.Dock = DockStyle.Fill;
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
            dataGridViewFiles.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewFiles.Size = new Size(1308, 800);
            dataGridViewFiles.TabIndex = 61;
            dataGridViewFiles.VirtualMode = true;
            dataGridViewFiles.CellFormatting += DataGridViewFiles_CellFormatting;
            dataGridViewFiles.CellValueChanged += DataGridViewFiles_CellValueChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.BackColor = Color.FromArgb(56, 57, 60);
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
            selectAllToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.Size = new Size(188, 38);
            selectAllToolStripMenuItem.Text = "Select All";
            selectAllToolStripMenuItem.Click += SelectAllToolStripMenuItem_Click;
            // 
            // deselectAllToolStripMenuItem
            // 
            deselectAllToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
            deselectAllToolStripMenuItem.Size = new Size(188, 38);
            deselectAllToolStripMenuItem.Text = "Deselect All";
            deselectAllToolStripMenuItem.Click += DeselectAllToolStripMenuItem_Click;
            // 
            // plTbSBInfo
            // 
            plTbSBInfo.BackColor = Color.Transparent;
            plTbSBInfo.ColumnCount = 2;
            plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
            plTbSBInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            plTbSBInfo.Controls.Add(lbTitle, 0, 0);
            plTbSBInfo.Controls.Add(lbInfoOrders, 0, 1);
            plTbSBInfo.Controls.Add(lbInfoOrdersCount, 1, 1);
            plTbSBInfo.Controls.Add(lbInfoFiles, 0, 2);
            plTbSBInfo.Controls.Add(lbInfoFilesCount, 1, 2);
            plTbSBInfo.Controls.Add(lbInfoWorksheets, 0, 3);
            plTbSBInfo.Controls.Add(lbInfoWorksheetsCount, 1, 3);
            plTbSBInfo.Controls.Add(lbInfoItems, 0, 4);
            plTbSBInfo.Controls.Add(lbInfoItemsCount, 1, 4);
            plTbSBInfo.Controls.Add(lbInfoMaterialCount, 1, 5);
            plTbSBInfo.Controls.Add(lbInfoMaterial, 0, 5);
            plTbSBInfo.Controls.Add(lbInfoWarnings, 0, 6);
            plTbSBInfo.Controls.Add(lbInfoWarningCount, 1, 6);
            plTbSBInfo.Controls.Add(lbInfoErrors, 0, 7);
            plTbSBInfo.Controls.Add(lbInfoErrorCount, 1, 7);
            plTbSBInfo.Dock = DockStyle.Right;
            plTbSBInfo.ForeColor = Color.FromArgb(248, 248, 249);
            plTbSBInfo.Location = new Point(1308, 0);
            plTbSBInfo.Margin = new Padding(6);
            plTbSBInfo.Name = "plTbSBInfo";
            plTbSBInfo.RowCount = 12;
            plTbSBInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
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
            // lbTitle
            // 
            lbTitle.AutoSize = true;
            plTbSBInfo.SetColumnSpan(lbTitle, 2);
            lbTitle.Dock = DockStyle.Fill;
            lbTitle.FlatStyle = FlatStyle.Flat;
            lbTitle.Font = new Font("Segoe UI Black", 12.875F, FontStyle.Bold);
            lbTitle.ForeColor = Color.DarkGray;
            lbTitle.Location = new Point(6, 6);
            lbTitle.Margin = new Padding(6);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new Size(280, 44);
            lbTitle.TabIndex = 13;
            lbTitle.Text = "LOADED";
            lbTitle.TextAlign = ContentAlignment.MiddleCenter;
            lbTitle.Click += lbTitle_Click;
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
            lbInfoOrdersCount.Text = "0";
            lbInfoOrdersCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoFiles
            // 
            lbInfoFiles.AutoSize = true;
            lbInfoFiles.Dock = DockStyle.Fill;
            lbInfoFiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoFiles.ForeColor = Color.DarkGray;
            lbInfoFiles.Location = new Point(6, 118);
            lbInfoFiles.Margin = new Padding(6);
            lbInfoFiles.Name = "lbInfoFiles";
            lbInfoFiles.Size = new Size(153, 44);
            lbInfoFiles.TabIndex = 4;
            lbInfoFiles.Text = "Files:";
            lbInfoFiles.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoFilesCount
            // 
            lbInfoFilesCount.AutoSize = true;
            lbInfoFilesCount.BackColor = Color.FromArgb(56, 57, 60);
            lbInfoFilesCount.Dock = DockStyle.Fill;
            lbInfoFilesCount.FlatStyle = FlatStyle.Flat;
            lbInfoFilesCount.Font = new Font("Segoe UI", 9F);
            lbInfoFilesCount.ForeColor = Color.DarkGray;
            lbInfoFilesCount.Location = new Point(171, 118);
            lbInfoFilesCount.Margin = new Padding(6);
            lbInfoFilesCount.Name = "lbInfoFilesCount";
            lbInfoFilesCount.Size = new Size(115, 44);
            lbInfoFilesCount.TabIndex = 0;
            lbInfoFilesCount.Text = "0";
            lbInfoFilesCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoWorksheets
            // 
            lbInfoWorksheets.AutoSize = true;
            lbInfoWorksheets.Dock = DockStyle.Fill;
            lbInfoWorksheets.FlatStyle = FlatStyle.Flat;
            lbInfoWorksheets.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoWorksheets.ForeColor = Color.DarkGray;
            lbInfoWorksheets.Location = new Point(6, 174);
            lbInfoWorksheets.Margin = new Padding(6);
            lbInfoWorksheets.Name = "lbInfoWorksheets";
            lbInfoWorksheets.Size = new Size(153, 44);
            lbInfoWorksheets.TabIndex = 0;
            lbInfoWorksheets.Text = "Worksheets:";
            lbInfoWorksheets.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoWorksheetsCount
            // 
            lbInfoWorksheetsCount.AutoSize = true;
            lbInfoWorksheetsCount.Dock = DockStyle.Fill;
            lbInfoWorksheetsCount.FlatStyle = FlatStyle.Flat;
            lbInfoWorksheetsCount.Font = new Font("Segoe UI", 9F);
            lbInfoWorksheetsCount.ForeColor = Color.DarkGray;
            lbInfoWorksheetsCount.Location = new Point(171, 174);
            lbInfoWorksheetsCount.Margin = new Padding(6);
            lbInfoWorksheetsCount.Name = "lbInfoWorksheetsCount";
            lbInfoWorksheetsCount.Size = new Size(115, 44);
            lbInfoWorksheetsCount.TabIndex = 0;
            lbInfoWorksheetsCount.Text = "0";
            lbInfoWorksheetsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoItems
            // 
            lbInfoItems.AutoSize = true;
            lbInfoItems.Dock = DockStyle.Fill;
            lbInfoItems.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoItems.ForeColor = Color.DarkGray;
            lbInfoItems.Location = new Point(6, 230);
            lbInfoItems.Margin = new Padding(6);
            lbInfoItems.Name = "lbInfoItems";
            lbInfoItems.Size = new Size(153, 44);
            lbInfoItems.TabIndex = 12;
            lbInfoItems.Text = "Items:";
            lbInfoItems.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoItemsCount
            // 
            lbInfoItemsCount.AutoSize = true;
            lbInfoItemsCount.Dock = DockStyle.Fill;
            lbInfoItemsCount.FlatStyle = FlatStyle.Flat;
            lbInfoItemsCount.Font = new Font("Segoe UI", 9F);
            lbInfoItemsCount.ForeColor = Color.DarkGray;
            lbInfoItemsCount.Location = new Point(171, 230);
            lbInfoItemsCount.Margin = new Padding(6);
            lbInfoItemsCount.Name = "lbInfoItemsCount";
            lbInfoItemsCount.Size = new Size(115, 44);
            lbInfoItemsCount.TabIndex = 0;
            lbInfoItemsCount.Text = "0";
            lbInfoItemsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoMaterialCount
            // 
            lbInfoMaterialCount.AutoSize = true;
            lbInfoMaterialCount.Dock = DockStyle.Fill;
            lbInfoMaterialCount.FlatStyle = FlatStyle.Flat;
            lbInfoMaterialCount.Font = new Font("Segoe UI", 9F);
            lbInfoMaterialCount.ForeColor = Color.DarkGray;
            lbInfoMaterialCount.Location = new Point(171, 286);
            lbInfoMaterialCount.Margin = new Padding(6);
            lbInfoMaterialCount.Name = "lbInfoMaterialCount";
            lbInfoMaterialCount.Size = new Size(115, 44);
            lbInfoMaterialCount.TabIndex = 11;
            lbInfoMaterialCount.Text = "0";
            lbInfoMaterialCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoMaterial
            // 
            lbInfoMaterial.AutoSize = true;
            lbInfoMaterial.Dock = DockStyle.Fill;
            lbInfoMaterial.FlatStyle = FlatStyle.Flat;
            lbInfoMaterial.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoMaterial.ForeColor = Color.DarkGray;
            lbInfoMaterial.Location = new Point(6, 286);
            lbInfoMaterial.Margin = new Padding(6);
            lbInfoMaterial.Name = "lbInfoMaterial";
            lbInfoMaterial.Size = new Size(153, 44);
            lbInfoMaterial.TabIndex = 10;
            lbInfoMaterial.Text = "Materials:";
            lbInfoMaterial.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoWarnings
            // 
            lbInfoWarnings.AutoSize = true;
            lbInfoWarnings.Dock = DockStyle.Fill;
            lbInfoWarnings.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoWarnings.ForeColor = Color.Yellow;
            lbInfoWarnings.Location = new Point(6, 342);
            lbInfoWarnings.Margin = new Padding(6);
            lbInfoWarnings.Name = "lbInfoWarnings";
            lbInfoWarnings.Size = new Size(153, 44);
            lbInfoWarnings.TabIndex = 0;
            lbInfoWarnings.Text = "Warnings:";
            lbInfoWarnings.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoWarningCount
            // 
            lbInfoWarningCount.AutoSize = true;
            lbInfoWarningCount.Dock = DockStyle.Fill;
            lbInfoWarningCount.Font = new Font("Segoe UI", 9F);
            lbInfoWarningCount.ForeColor = Color.Yellow;
            lbInfoWarningCount.Location = new Point(171, 342);
            lbInfoWarningCount.Margin = new Padding(6);
            lbInfoWarningCount.Name = "lbInfoWarningCount";
            lbInfoWarningCount.Size = new Size(115, 44);
            lbInfoWarningCount.TabIndex = 0;
            lbInfoWarningCount.Text = "0";
            lbInfoWarningCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbInfoErrors
            // 
            lbInfoErrors.AutoSize = true;
            lbInfoErrors.Dock = DockStyle.Fill;
            lbInfoErrors.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbInfoErrors.ForeColor = Color.Crimson;
            lbInfoErrors.Location = new Point(6, 398);
            lbInfoErrors.Margin = new Padding(6);
            lbInfoErrors.Name = "lbInfoErrors";
            lbInfoErrors.Size = new Size(153, 44);
            lbInfoErrors.TabIndex = 9;
            lbInfoErrors.Text = "Errors:";
            lbInfoErrors.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbInfoErrorCount
            // 
            lbInfoErrorCount.AutoSize = true;
            lbInfoErrorCount.Dock = DockStyle.Fill;
            lbInfoErrorCount.Font = new Font("Segoe UI", 9F);
            lbInfoErrorCount.ForeColor = Color.Red;
            lbInfoErrorCount.Location = new Point(171, 398);
            lbInfoErrorCount.Margin = new Padding(6);
            lbInfoErrorCount.Name = "lbInfoErrorCount";
            lbInfoErrorCount.Size = new Size(115, 44);
            lbInfoErrorCount.TabIndex = 0;
            lbInfoErrorCount.Text = "0";
            lbInfoErrorCount.TextAlign = ContentAlignment.MiddleCenter;
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
            lbInfoRows.Text = "Items:";
            lbInfoRows.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "st_grean.png");
            imageList1.Images.SetKeyName(1, "st_yellow.png");
            imageList1.Images.SetKeyName(2, "st_orange.png");
            imageList1.Images.SetKeyName(3, "st_red.png");
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
        private TableLayoutPanel plTbSBInfo;
        private Label lbInfoErrors;
        private Label lbInfoFiles;
        private Label lbInfoErrorCount;
        private Label lbInfoWarningCount;
        private Label lbInfoWarnings;
        private Label lbInfoItemsCount;
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
        private Label lbInfoMaterial;
        private Label lbInfoMaterialCount;
        public Panel plGridPanel;
        private Label lbInfoItems;
        private ImageList imageList1;
        public Label lbTitle;
    }
}
