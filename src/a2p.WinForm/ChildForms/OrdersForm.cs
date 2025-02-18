using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services;
using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Domain.Entities;
using a2p.Shared.Infrastructure.Interfaces;
using a2p.WinForm.CustomControls;

using Microsoft.Extensions.Configuration;

using System.Data;

using Font = System.Drawing.Font;

namespace a2p.WinForm.ChildForms
{
    public partial class OrdersForm : Form
    {
        private readonly IFileService _fileService;
        private readonly IOrderProcessingService _mappingHandlerService;
        private readonly ILogService _logService;
        private readonly IA2POrderMapper _orderMapper;
        private readonly IConfiguration _configuration;
        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        private DataTable _dataTable;
        private BindingSource _bindingSource;
        private List<A2POrder> _orderList;

        public OrdersForm(IConfiguration configuration, IFileService fileService, IOrderProcessingService mappingService, ILogService logService, IA2POrderMapper orderMapper)
        {
            _fileService = fileService;
            _mappingHandlerService = mappingService;
            _logService = logService;
            _orderMapper = orderMapper;
            _orderList = [];
            _dataTable = new DataTable();
            _bindingSource = [];
            _progressValue = new ProgressValue();
            _configuration = configuration;

            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent();
            InitializeGrid();
            InitializeTable();
        }

        #region -== Initializations ==-
        private void InitializeGrid()
        {
            try
            {
                if (dataGridViewFiles != null)
                {
                    #region -== DataGrid Columns ==-
                    if (!dataGridViewFiles.Columns.Contains("Order"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Order",
                            DataPropertyName = "Order",
                            Name = "Order",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("Currency"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Currency",
                            DataPropertyName = "Currency",
                            Name = "Currency",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("FileCount"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Files",
                            DataPropertyName = "FileCount",
                            Name = "FileCount",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("FileList"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Order List",
                            DataPropertyName = "FileList",
                            Name = "FileList",
                            Visible = false
                        });
                    }
                    //if (!dataGridViewFiles.Columns.Contains("LockedFileCount"))
                    //{
                    //    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    //    {
                    //        HeaderText = "IsLocked",
                    //        DataPropertyName = "LockedFileCount",
                    //        Name = "LockedFileCount",
                    //        ReadOnly = true,
                    //        Visible = false
                    //    });
                    //}
                    //if (!dataGridViewFiles.Columns.Contains("LockedFileList"))
                    //{
                    //    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    //    {
                    //        HeaderText = "LockedList",
                    //        DataPropertyName = "LockedFileList",
                    //        Name = "LockedFileList",
                    //        Visible = false
                    //    });
                    //}
                    if (!dataGridViewFiles.Columns.Contains("WorksheetCount"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Worksheets",
                            DataPropertyName = "WorksheetCount",
                            Name = "WorksheetCount",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("WorksheetList"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Worksheet List",
                            DataPropertyName = "WorksheetList",
                            Name = "WorksheetList",
                            ReadOnly = true,
                            Visible = false
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("Items"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Items",
                            DataPropertyName = "Items",
                            Name = "Items",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("Import"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewCheckBoxColumn
                        {
                            HeaderText = "Import",
                            DataPropertyName = "Import",
                            Name = "Import",
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("ErrorCount"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Errors",
                            DataPropertyName = "ErrorCount",
                            Name = "ErrorCount",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("ErrorList"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "ErrorList",
                            DataPropertyName = "ErrorList",
                            Name = "ErrorList",
                            ReadOnly = true,
                            Visible = false
                        });
                    }
                    #endregion -== DataGrid Columns ==-
                    #region -== DataGrid Columns Style ==-
                    DataGridViewCellStyle ColumnHeadersDefaultCellStyle = new()
                    {
                        BackColor = Color.FromArgb(56, 57, 60),
                        ForeColor = Color.FromArgb(239, 112, 32),
                        SelectionBackColor = Color.FromArgb(239, 112, 32),
                        SelectionForeColor = Color.WhiteSmoke,
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                        Padding = new Padding(5),
                        Alignment = DataGridViewContentAlignment.MiddleCenter,
                    };
                    dataGridViewFiles.ColumnHeadersDefaultCellStyle = ColumnHeadersDefaultCellStyle;

                    DataGridViewCellStyle AlternatingRowsDefaultCellStyle = new()
                    {
                        BackColor = Color.FromArgb(96, 97, 100),
                        ForeColor = Color.WhiteSmoke,
                        SelectionBackColor = Color.FromArgb(239, 112, 32),
                        SelectionForeColor = Color.WhiteSmoke,
                        Font = new Font("Segoe UI", 9F),
                        Padding = new Padding(5),
                        WrapMode = DataGridViewTriState.True,
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    };
                    dataGridViewFiles.AlternatingRowsDefaultCellStyle = AlternatingRowsDefaultCellStyle;

                    DataGridViewCellStyle RowsDefaultCellStyle = new()
                    {
                        BackColor = Color.FromArgb(56, 57, 60),
                        ForeColor = Color.WhiteSmoke,
                        SelectionBackColor = Color.FromArgb(239, 112, 32),
                        SelectionForeColor = Color.WhiteSmoke,
                        Font = new Font("Segoe UI", 9F),
                        Padding = new Padding(5),
                        WrapMode = DataGridViewTriState.True,
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    };
                    dataGridViewFiles.RowsDefaultCellStyle = RowsDefaultCellStyle;


                    DataGridViewCellStyle DefaultCellStyle = new()
                    {
                        BackColor = Color.FromArgb(56, 57, 60),
                        ForeColor = Color.WhiteSmoke,
                        SelectionBackColor = Color.FromArgb(239, 112, 32),
                        SelectionForeColor = Color.WhiteSmoke,
                        Font = new Font("Segoe UI", 9F),
                        Padding = new Padding(5),
                        WrapMode = DataGridViewTriState.True,
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    };
                    dataGridViewFiles.DefaultCellStyle = DefaultCellStyle;
                    #endregion -== DataGrid Columns Style ==-
                }
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "FF: Unhandled Error initializing grid view");
            }

        }

        private void InitializeTable()
        {
            try
            {
                dataGridViewFiles.SuspendLayout();
                _ = _dataTable.Columns.Add("Order", typeof(string));
                _ = _dataTable.Columns.Add("Currency", typeof(string));
                _ = _dataTable.Columns.Add("FileCount", typeof(int));
                _ = _dataTable.Columns.Add("FileList", typeof(string));
                //_ = _dataTable.Columns.Add("LockedFileCount", typeof(int));
                //_ = _dataTable.Columns.Add("LockedFileList", typeof(string));
                _ = _dataTable.Columns.Add("WorksheetCount", typeof(int));
                _ = _dataTable.Columns.Add("WorksheetList", typeof(string));
                _ = _dataTable.Columns.Add("Items", typeof(int));
                _ = _dataTable.Columns.Add("Import", typeof(bool));
                _ = _dataTable.Columns.Add("ErrorCount", typeof(string));
                _ = _dataTable.Columns.Add("ErrorList", typeof(string));

                _bindingSource.DataSource = _dataTable;
                dataGridViewFiles.DataSource = _bindingSource;

            }
            catch (Exception ex)
            {
                _logService.Error(ex, "FF: Unhandled Error initializing data table");

            }
            finally
            {
                dataGridViewFiles.ResumeLayout(false);
            }

        }

        #endregion -== Initializations ==-

        #region -== Form Evenets ==-

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            this.PerformAutoScale();
        }
        private void FileForm_Shown(object sender, EventArgs e)
        {
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private void OrdersForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void FileForm_ResizeBegin(object sender, EventArgs e)
        {
            this.SuspendLayout();
        }
        private void FileForm_ResizeEnd(object sender, EventArgs e)
        {
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion -== Form Evenets ==-

        #region -== Grid Events ==-
        private void dataGridViewFiles_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            _logService.Error("FF:GridViewFiles. Error in column {$Column}, row {$Row}: {$Exception}", e.ColumnIndex, e.RowIndex, e.Exception?.Message ?? "Exception details missing.");
            Console.WriteLine($"GridViewFiles Error in column {e.ColumnIndex}, row {e.RowIndex}: {e.Exception?.Message ?? "Exception details missing."}");
            e.ThrowException = false;
        }
        private void dataGridViewFiles_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                dataGridViewFiles.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dataGridViewFiles.Columns["FileCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["FileCount"].Index && e.Value != null)
                {
                    string? fileList = dataGridViewFiles.Rows[e.RowIndex].Cells["FileList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = fileList;
                }
                else if (dataGridViewFiles.Columns["WorksheetCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["WorksheetCount"].Index && e.Value != null)
                {
                    string? worksheetList = dataGridViewFiles.Rows[e.RowIndex].Cells["WorksheetList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = worksheetList;
                }
                //if (dataGridViewFiles.Columns["ErrorCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["ErrorCount"].Index && e.Value != null)
                //{
                //    if (e.CellStyle != null)
                //    {
                //        e.CellStyle.ForeColor = Color.FromArgb(56, 57, 60);
                //    }
                //    if (int.TryParse(e.Value.ToString(), out int locked))
                //    {
                //        if (e.CellStyle != null)
                //        {
                //            e.CellStyle.ForeColor = locked > 0 ? System.Drawing.Color.LightSalmon : Color.FromArgb(126, 127, 130);
                //        }
                //    }
                //    else
                //    {
                //        if (e.CellStyle != null)
                //        {
                //            e.CellStyle.ForeColor = Color.FromArgb(126, 127, 130);
                //        }
                //    }
                //    string? errorFileList = dataGridViewFiles.Rows[e.RowIndex].Cells["ErrorList"].Value.ToString();
                //    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = errorFileList;
                //}

                if (dataGridViewFiles.Columns["ErrorCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["ErrorCount"].Index && e.Value != null)
                {
                    if (e.CellStyle != null)
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(56, 57, 60);
                    }

                    if (!string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        if (e.CellStyle != null)
                        {
                            e.CellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        if (e.CellStyle != null)
                        {
                            e.CellStyle.ForeColor = Color.FromArgb(126, 127, 130);
                        }
                    }

                    string? errorList = dataGridViewFiles.Rows[e.RowIndex].Cells["ErrorList"].Value.ToString();

                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = errorList;
                }


            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error formatting cell");
            }
        }

        #endregion -== Grid Events ==-

        #region -== Methods ==-

        public async Task OrdersLoad()
        {
            if (_progressValue != null)
            {
                _progressValue.ProgressTitle = "Loading Order Files ...";
                _progressValue.MinValue = 0;
                _progressValue.MaxValue = 100;
                _progressValue.Value = 0;
            }
            else
            {
                _progressValue = new ProgressValue
                {
                    ProgressTitle = "Loading Order Files ...",
                    MinValue = 0,
                    MaxValue = 100,
                    Value = 0
                };

            }


            try
            {


                using ProgressBarForm progressBarForm = new()
                {
                    StartPosition = FormStartPosition.CenterParent // Set to center relative to parent
                };
                progressBarForm.Load += (sender, args) =>
                {
                    progressBarForm.Location = new Point(
                        this.Location.X + ((this.Width - progressBarForm.Width) / 2),
                        this.Location.Y + ((this.Height - progressBarForm.Height) / 2)
                    );
                };

                Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);
                _progress = progress;
                _progress?.Report(_progressValue);


                progressBarForm.Cursor = Cursors.WaitCursor;
                progressBarForm.Show();
                await progressBarForm.FormReadyAsync();
    

                _progress?.Report(_progressValue);


                #region  Start Loading Files



                if (_dataTable.Rows.Count > 0)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            _progressValue.ProgressTitle = "Removing Existing data ...";
                            _progress?.Report(_progressValue);
                            _dataTable.Clear();
                            _orderList.Clear();

                        }));
                    }
                    else
                    {
                        _progressValue.ProgressTitle = "Removing Existing data ...";
                        _progress?.Report(_progressValue);
                        _dataTable.Clear();
                        _orderList.Clear();




                    }

                }


                _progressValue.ProgressTitle = "Refreshing Files ...";
                _progress?.Report(_progressValue);
                _orderList = await _fileService.GetOrdersAsync(_progressValue, _progress);


                _progress?.Report(_progressValue);

                _progress?.Report(_progressValue);
                foreach (A2POrder a2pOrder in _orderList)
                {
                    _progress?.Report(_progressValue);
                    try
                    {
                        OrderDTO order = await _orderMapper.MapToOrderDTOAsync(a2pOrder);
                        _ = _dataTable.Rows.Add
                            (order.Order,
                              order.Currency,
                              order.FileCount,
                              order.FileList,
                              //    order.LockedFileCount,
                              //    order.LockedFileList,
                              order.WorksheetCount,
                              order.WorksheetList,
                              order.ItemCount,
                              order.Import,
                              order.ErrorCount,
                              order.ErrorList
                            );
                        this.PerformLayout();
                    }
                    catch (Exception ex)
                    {
                        _logService.Debug(ex, "Error adding individual order to data table");
                    }
                }
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {

                        plGridPanel.ResumeLayout(false);
                        plGridPanel.PerformLayout();

                    }));
                }
                else
                {
                    plGridPanel.ResumeLayout(false);
                    plGridPanel.PerformLayout();
                }


                progressBarForm.Close();
                #endregion

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($@"An error occurred during the import: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        public async Task ImportFilesAsync()
        {

            if (_progressValue != null)
            {
                _progressValue.ProgressTitle = "Importing Files ...";
                _progressValue.MinValue = 0;
                _progressValue.MaxValue = 100;
                _progressValue.Value = 0;
            }
            else
            {
                _progressValue = new ProgressValue
                {
                    ProgressTitle = "Importing Files....",
                    MinValue = 0,
                    MaxValue = 100,
                    Value = 0
                };

            }
            try
            {

                using ProgressBarForm progressBarForm = new()
                {
                    StartPosition = FormStartPosition.CenterParent // Set to center relative to parent
                };
                progressBarForm.Load += (sender, args) =>
                {
                    progressBarForm.Location = new Point(
                        this.Location.X + ((this.Width - progressBarForm.Width) / 2),
                        this.Location.Y + ((this.Height - progressBarForm.Height) / 2)
                        );
                    progressBarForm.progressBar.Style = ProgressBarStyle.Continuous;
                    progressBarForm.progressBar.ForeColor = Color.FromArgb(239, 112, 32);
                };
                Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);
                _progress = progress;
                _progress?.Report(_progressValue);

                progressBarForm.Cursor = Cursors.WaitCursor;
                progressBarForm.Cursor = Cursors.WaitCursor;
                progressBarForm.Show();
                await progressBarForm.FormReadyAsync();



                List<A2POrder> a2pOrderList = [];


                foreach (DataGridViewRow row in dataGridViewFiles.Rows)
                {
                    if (row.Cells["Import"].Value != null && (bool)row.Cells["Import"].Value)
                    {


                        string? orderNumber = row.Cells["Order"].Value.ToString();
                        if (!string.IsNullOrEmpty(orderNumber))
                        {
                            A2POrder a2pOrder = _orderList.FirstOrDefault(o => o.Order == orderNumber)!;



                            DateTime localDateTime;


                            if (a2pOrder != null && a2pOrder.OrderExists.HasValue)
                            {
                                localDateTime = a2pOrder.OrderExists.Value.ToLocalTime();

                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                                DialogResult result = MessageBox.Show(
                                    $"Order {orderNumber} already exists in the database." +
                                    $"\nLast known import date: {localDateTime}." +
                                    $"\nDo you want to import it again?" +
                                    $"\n" +
                                    $"\nKeep attention! Existing data will be removed!",
                                    "Order Exists",
                                    buttons, MessageBoxIcon.Warning
                                );

                                a2pOrder.OverwriteOrder = result == DialogResult.Yes;

                            }

                            if ((a2pOrder!.OverwriteOrder && a2pOrder.OrderExists.HasValue) || !a2pOrder.OrderExists.HasValue)
                            {
                                a2pOrderList.Add(a2pOrder);
                            }

                        }
                    }
                }



                IEnumerable<A2POrder> s2pOderList = await _mappingHandlerService.MapDataAsync(a2pOrderList, progress);
                string root = _configuration["AppSettings:Folders:WorkFolder"] ?? @"C:\Temp\Import";
                string success = Path.Combine(root, _configuration["AppSettings:Folders:ImportSuccess"] ?? "Import_Success");
                string failed = Path.Combine(root, _configuration["AppSettings:Folders:ImportFailed"] ?? "Import_Failed");

                foreach (A2POrder a2pOrder in s2pOderList)
                {
                    if (a2pOrder.WriteErrors.Count > 0)
                    {

                        foreach (A2PFile a2pFile in a2pOrder.Files)
                        {
                            string destination = Path.Combine(failed, a2pFile.FileName);
                            _fileService.MoveFilesAsync(a2pFile.File, destination);
                        }



                    }
                    else
                    {
                        foreach (A2PFile a2pFile in a2pOrder.Files)
                        {
                            string destination = Path.Combine(success, a2pFile.FileName);
                            _fileService.MoveFilesAsync(a2pFile.File, destination);
                        }
                    }

                }



                progressBarForm.Close();

                await OrdersLoad();
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error loading FileForm");
                _ = MessageBox.Show(@"An error occurred while loading the files.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { dataGridViewFiles.ResumeLayout(false); }
        }

        #endregion -== Methods ==-

        



        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
            {
                dataGridViewFiles.Rows[i].Cells["Import"].Value = true;
            }
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
            {
                dataGridViewFiles.Rows[i].Cells["Import"].Value = false;
            }
        }


    }

}