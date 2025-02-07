using System.Windows.Forms;

namespace a2p.WinForm.ChildForms
{
 partial class OrdersForm
 {
  /// <summary>
  /// QuantityRequired designer variable.
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
            plGridPanel = new Panel();
            dataGridViewFiles = new DataGridView();
            plGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).BeginInit();
            SuspendLayout();
            // 
            // plGridPanel
            // 
            plGridPanel.AutoScroll = true;
            plGridPanel.BackColor = Color.Transparent;
            plGridPanel.Controls.Add(dataGridViewFiles);
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
            dataGridViewFiles.Size = new Size(1600, 800);
            dataGridViewFiles.TabIndex = 2;
            dataGridViewFiles.VirtualMode = true;
            dataGridViewFiles.CellFormatting += dataGridViewFiles_CellFormatting;
            dataGridViewFiles.DataError += dataGridViewFiles_DataError;
            // 
            // FileForm
            // 
            BackColor = Color.FromArgb(56, 57, 60);
            ClientSize = new Size(1600, 800);
            Controls.Add(plGridPanel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(12);
            Name = "FileForm";
            Text = "FileForm";
            Shown += FileForm_Shown;
            ResizeBegin += FileForm_ResizeBegin;
            ResizeEnd += FileForm_ResizeEnd;
            plGridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).EndInit();
            ResumeLayout(false);



        }

        #endregion
      private Panel plGridPanel;
      private DataGridView dataGridViewFiles;
     }
}