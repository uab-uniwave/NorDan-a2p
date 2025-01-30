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
            this.plMainPanel=new Panel();
            this.plGridPanel=new Panel();
            this.dataGridViewFiles=new DataGridView();
            this.plGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewFiles).BeginInit();
            SuspendLayout();
            // 
            // plMainPanel
            // 
            this.plMainPanel.Dock=DockStyle.Top;
            this.plMainPanel.Location=new Point(0, 0);
            this.plMainPanel.Margin=new Padding(6);
            this.plMainPanel.Name="plMainPanel";
            this.plMainPanel.Size=new Size(1600, 60);
            this.plMainPanel.TabIndex=0;
            // 
            // plGridPanel
            // 
            this.plGridPanel.AutoScroll=true;
            this.plGridPanel.BackColor=Color.Transparent;
            this.plGridPanel.Controls.Add(this.dataGridViewFiles);
            this.plGridPanel.Dock=DockStyle.Fill;
            this.plGridPanel.ForeColor=Color.Transparent;
            this.plGridPanel.Location=new Point(0, 60);
            this.plGridPanel.Margin=new Padding(6);
            this.plGridPanel.Name="plGridPanel";
            this.plGridPanel.Size=new Size(1600, 840);
            this.plGridPanel.TabIndex=1;
            // 
            // dataGridViewFiles
            // 
            this.dataGridViewFiles.AllowUserToAddRows=false;
            this.dataGridViewFiles.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewFiles.AutoSizeRowsMode=DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewFiles.BackgroundColor=Color.FromArgb(88, 89, 81);
            this.dataGridViewFiles.ColumnHeadersBorderStyle=DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewFiles.ColumnHeadersHeightSizeMode=DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFiles.Dock=DockStyle.Fill;
            this.dataGridViewFiles.EditMode=DataGridViewEditMode.EditOnEnter;
            this.dataGridViewFiles.EnableHeadersVisualStyles=false;
            this.dataGridViewFiles.GridColor=UniwaveColors.uwOrangeDeep;
            this.dataGridViewFiles.Location=new Point(0, 0);
            this.dataGridViewFiles.Margin=new Padding(6);
            this.dataGridViewFiles.MultiSelect=false;
            this.dataGridViewFiles.Name="dataGridViewFiles";
            this.dataGridViewFiles.RightToLeft=RightToLeft.No;
            this.dataGridViewFiles.RowHeadersBorderStyle=DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewFiles.RowHeadersVisible=false;
            this.dataGridViewFiles.RowHeadersWidth=82;
            this.dataGridViewFiles.Size=new Size(1600, 840);
            this.dataGridViewFiles.TabIndex=2;
            this.dataGridViewFiles.VirtualMode=true;
            // 
            // FileForm
            // 
            AutoScaleDimensions=new SizeF(96F, 96F);
            AutoScaleMode=AutoScaleMode.Dpi;
            BackColor=Color.FromArgb(88, 89, 81);
            ClientSize=new Size(1600, 900);
            Controls.Add(this.plGridPanel);
            Controls.Add(this.plMainPanel);
            FormBorderStyle=FormBorderStyle.None;
            Margin=new Padding(6);
            Name="FileForm";
            Text="FileForm";
            Shown+=FileForm_Shown;
            ResizeBegin+=FileForm_ResizeBegin;
            ResizeEnd+=FileForm_ResizeEnd;
            this.plGridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewFiles).EndInit();
            ResumeLayout(false);



        }



        #endregion

        private Panel plMainPanel;
      private Panel plGridPanel;
      private DataGridView dataGridViewFiles;
     }
}