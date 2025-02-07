using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Interfaces.Services;

using System.Data;

using Font = System.Drawing.Font;

namespace a2p.WinForm.ChildForms
{
    public partial class OrdersForm : Form
    {
        private readonly IFileService _fileService;
        private readonly IMappingService _mappingService;
        private readonly ILogService _logService;
        private readonly IA2POrderMapper _orderMapper;
        private ProgressValue _progressValue;
        private DataTable _dataTable;
        private BindingSource _bindingSource;
        private List<A2POrder> _orderList;

        public OrdersForm(IFileService fileService, IMappingService mappingService, ILogService logService, IA2POrderMapper orderMapper)
        {
            _fileService = fileService;
            _mappingService = mappingService;
            _logService = logService;
            _orderMapper = orderMapper;
            _orderList = [];
            _dataTable = new DataTable();
            _bindingSource = [];
            _progressValue = new ProgressValue();

            this.SuspendLayout();

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent();
            InitializeGrid();
            InitializeTable();
        }

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
                            HeaderText = "OrderFiles",
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
                    if (!dataGridViewFiles.Columns.Contains("LockedFileCount"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "IsLocked",
                            DataPropertyName = "LockedFileCount",
                            Name = "LockedFileCount",
                            ReadOnly = true,
                            Visible = false
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("LockedFileList"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "LockedList",
                            DataPropertyName = "LockedFileList",
                            Name = "LockedFileList",
                            Visible = false
                        });
                    }
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
                _ = _dataTable.Columns.Add("LockedFileCount", typeof(int));
                _ = _dataTable.Columns.Add("LockedFileList", typeof(string));
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



        #region -== File Form Evenets ==-
        private void FileForm_Shown(object sender, EventArgs e)
        {
            ResumeLayout(true);
        }

        private void FileForm_ResizeEnd(object sender, EventArgs e)
        {
            ResumeLayout(true);
        }

        private void FileForm_ResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }
        #endregion -== File Form Evenets ==-

        #region -== Data Grid Events ==-
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
                else if (dataGridViewFiles.Columns["LockedFileCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["LockedFileCount"].Index && e.Value != null)
                {
                    string? lockedFileList = dataGridViewFiles.Rows[e.RowIndex].Cells["LockedFileList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = lockedFileList;
                }
                else if (dataGridViewFiles.Columns["WorksheetCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["WorksheetCount"].Index && e.Value != null)
                {
                    string? worksheetList = dataGridViewFiles.Rows[e.RowIndex].Cells["WorksheetList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = worksheetList;
                }

                if (dataGridViewFiles.Columns["LockedFileCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["LockedFileCount"].Index && e.Value != null)
                {
                    if (e.CellStyle != null)
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(56, 57, 60);
                    }

                    if (int.TryParse(e.Value.ToString(), out int locked))
                    {
                        if (e.CellStyle != null)
                        {
                            e.CellStyle.ForeColor = locked > 0 ? System.Drawing.Color.LightSalmon : Color.FromArgb(126, 127, 130);
                        }
                    }
                    else
                    {
                        if (e.CellStyle != null)
                        {
                            e.CellStyle.ForeColor = Color.FromArgb(126, 127, 130);
                        }
                    }

                    string? lockedFileList = dataGridViewFiles.Rows[e.RowIndex].Cells["LockedFileList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = lockedFileList;
                }

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

        #endregion -== Data Grid Events ==-

        #region -== File Form Methods ==-
        public async Task OrdersLoad(IProgress<ProgressValue>? progress = null)
        {
            _progressValue.ProgressTitle = "Refreshing OrderFiles ...";

            if (_dataTable.Rows.Count > 0)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        _progressValue.ProgressTitle = "Removing Existing data ...";
                        progress?.Report(_progressValue);

                        _dataTable.Clear();
                    }));
                }
                else
                {
                    _progressValue.ProgressTitle = "Removing Existing data ...";
                    progress?.Report(_progressValue);
                    _dataTable.Clear();


                }

            }

            progress?.Report(_progressValue);
            _orderList = await _fileService.GetOrdersAsync(_orderList, _progressValue, progress);
            try
            {
                foreach (A2POrder a2pOrder in _orderList)
                {
                    try
                    {
                        OrderDTO order = await _orderMapper.MapToOrderDTOAsync(a2pOrder);

                        _ = _dataTable.Rows.Add(
                         order.Order,
                         order.Currency,
                         order.FileCount,
                         order.FileList,
                         order.LockedFileCount,
                         order.LockedFileList,
                         order.WorksheetCount,
                         order.WorksheetList,
                         order.ItemCount,
                         order.Import,
                         order.ErrorCount,
                         order.ErrorList
                        );
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
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error loading FileForm");
                _ = MessageBox.Show(@"FF: Unhandled error occurred while loading the files.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public async Task ImportFilesAsync(IProgress<ProgressValue>? progress = null)
        {
            dataGridViewFiles.SuspendLayout();
            try
            {
                List<A2POrder> a2pOrderList = [];
                foreach (DataGridViewRow row in dataGridViewFiles.Rows)
                {
                    if (row.Cells["Import"].Value != null && (bool)row.Cells["Import"].Value)
                    {
                        string? orderNumber = row.Cells["Order"].Value.ToString();
                        if (!string.IsNullOrEmpty(orderNumber))
                        {
                            A2POrder? readOrder = _orderList.FirstOrDefault(o => o.Order == orderNumber);

                            if (readOrder != null)
                            {
                                a2pOrderList.Add(readOrder);


                            }
                        }
                    }
                }
                await _mappingService.MapDataAsync(a2pOrderList, progress);
                dataGridViewFiles.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error loading FileForm");
                _ = MessageBox.Show(@"An error occurred while loading the files.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { dataGridViewFiles.ResumeLayout(false); }
        }

        #endregion -== File Form Methods ==-


    }
}