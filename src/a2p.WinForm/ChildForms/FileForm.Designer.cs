using System.Windows.Forms;

namespace a2p.WinForm.ChildForms
{
 partial class FileForm
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

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            plMainPanel = new Panel();
            plGridPanel = new Panel();
            dataGridViewFiles = new DataGridView();
            plGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).BeginInit();
            SuspendLayout();
            // 
            // plMainPanel
            // 
            plMainPanel.Dock = DockStyle.Top;
            plMainPanel.Location = new Point(0, 0);
            plMainPanel.Margin = new Padding(12, 12, 12, 12);
            plMainPanel.Name = "plMainPanel";
            plMainPanel.Size = new Size(3200, 80);
            plMainPanel.TabIndex = 0;
            // 
            // plGridPanel
            // 
            plGridPanel.AutoScroll = true;
            plGridPanel.BackColor = Color.Transparent;
            plGridPanel.Controls.Add(dataGridViewFiles);
            plGridPanel.Dock = DockStyle.Fill;
            plGridPanel.ForeColor = Color.Transparent;
            plGridPanel.Location = new Point(0, 80);
            plGridPanel.Margin = new Padding(12, 12, 12, 12);
            plGridPanel.Name = "plGridPanel";
            plGridPanel.Size = new Size(3200, 1720);
            plGridPanel.TabIndex = 1;
            // 
            // dataGridViewFiles
            // 
            dataGridViewFiles.AllowUserToAddRows = false;
            dataGridViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewFiles.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewFiles.BackgroundColor =UniwaveColors.a2pGreyDark;
            dataGridViewFiles.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFiles.Dock = DockStyle.Fill;
            dataGridViewFiles.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewFiles.EnableHeadersVisualStyles = false;
            dataGridViewFiles.GridColor = Color.FromArgb(239, 112, 32);
            dataGridViewFiles.Location = new Point(0, 0);
            dataGridViewFiles.Margin = new Padding(12, 12, 12, 12);
            dataGridViewFiles.MultiSelect = false;
            dataGridViewFiles.Name = "dataGridViewFiles";
            dataGridViewFiles.RightToLeft = RightToLeft.No;
            dataGridViewFiles.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewFiles.RowHeadersVisible = false;
            dataGridViewFiles.RowHeadersWidth = 82;
            dataGridViewFiles.Size = new Size(3200, 1720);
            dataGridViewFiles.TabIndex = 2;
            dataGridViewFiles.VirtualMode = true;
            // 
            // FileForm
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor =UniwaveColors.a2pGreyDark;
            ClientSize = new Size(3200, 1800);
            Controls.Add(plGridPanel);
            Controls.Add(plMainPanel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(12, 12, 12, 12);
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

        private Panel plMainPanel;
      private Panel plGridPanel;
      private DataGridView dataGridViewFiles;
     }
}