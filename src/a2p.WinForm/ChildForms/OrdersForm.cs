// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;
using a2p.WinForm.CustomControls;

using Microsoft.Extensions.Configuration;

using Font = System.Drawing.Font;

namespace a2p.WinForm.ChildForms
{
    public partial class OrdersForm : Form
    {
        private readonly IFileService _fileService;
        private readonly IOrderWriteProcessor _orderWriteProcessor;
        private readonly ILogService _logService;
        private readonly DataCache _dataCache;
        private readonly IOrderReadProcessor _readServices;
        private readonly IExcelReadService _excelReadServices;

        private readonly IConfiguration _configuration;
        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        private System.Data.DataTable _dataTable;
        private BindingSource _bindingSource;
        //        private List<A2POrder> _a2pOrders;
        //        private List<A2POrder> _a2pOrdersPendingImport;

        public OrdersForm(IConfiguration configuration,
                          IOrderWriteProcessor orderWriteProcessor,
                          ILogService logService,
                          IOrderReadProcessor orderReadProcessor,
                          IExcelReadService excelReadServices,
                          IFileService fileService, DataCache dataCache)

        {

            _orderWriteProcessor = orderWriteProcessor;
            _logService = logService;
            _dataTable = new System.Data.DataTable();
            _bindingSource = [];
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _configuration = configuration;
            _readServices = orderReadProcessor;
            _excelReadServices = excelReadServices;
            //_a2pOrders = [];
            //_a2pOrdersPendingImport = [];
            _dataTable = new System.Data.DataTable();
            _bindingSource = [];
            _dataCache = dataCache;
            _fileService = fileService;

            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent();
            InitializeGrid();
            InitializeTable();
            _readServices = orderReadProcessor;
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
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
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
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
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
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
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

                    if (!dataGridViewFiles.Columns.Contains("WorksheetCount"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Worksheets",
                            DataPropertyName = "WorksheetCount",
                            Name = "WorksheetCount",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
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
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        });
                    }

                    if (!dataGridViewFiles.Columns.Contains("ItemList"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Item List",
                            DataPropertyName = "ItemList",
                            Name = "ItemList",
                            ReadOnly = true,
                            Visible = false
                        });
                    }

                    if (!dataGridViewFiles.Columns.Contains("Materials"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Materials",
                            DataPropertyName = "Materials",
                            Name = "Materials",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("Import"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewCheckBoxColumn
                        {
                            HeaderText = "Import",
                            DataPropertyName = "Import",
                            Name = "Import",
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("WarningCount"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Warnings",
                            DataPropertyName = "WarningCount",
                            Name = "WarningCount",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        });
                    }
                    if (!dataGridViewFiles.Columns.Contains("WarningList"))
                    {
                        _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "WarningList",
                            DataPropertyName = "WarningList",
                            Name = "WarningList",
                            ReadOnly = true,
                            Visible = false
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
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
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
                _ = _dataTable.Columns.Add("WorksheetCount", typeof(int));
                _ = _dataTable.Columns.Add("WorksheetList", typeof(string));
                _ = _dataTable.Columns.Add("Items", typeof(int));
                _ = _dataTable.Columns.Add("ItemList", typeof(string));
                _ = _dataTable.Columns.Add("Materials", typeof(int));
                _ = _dataTable.Columns.Add("Import", typeof(bool));
                _ = _dataTable.Columns.Add("WarningCount", typeof(string));
                _ = _dataTable.Columns.Add("WarningList", typeof(string));
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

                if (dataGridViewFiles.Columns["Items"] != null && e.ColumnIndex == dataGridViewFiles.Columns["Items"].Index && e.Value != null)
                {
                    string? fileList = dataGridViewFiles.Rows[e.RowIndex].Cells["ItemList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = fileList;
                }
                if (dataGridViewFiles.Columns["WorksheetCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["WorksheetCount"].Index && e.Value != null)
                {
                    string? worksheetList = dataGridViewFiles.Rows[e.RowIndex].Cells["WorksheetList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = worksheetList;
                }
                if (dataGridViewFiles.Columns["ErrorCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["ErrorCount"].Index && e.Value != null)
                {
                    if (e.CellStyle != null)
                    {
                        e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(56, 57, 60);
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

                if (dataGridViewFiles.Columns["WarningCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["WarningCount"].Index && e.Value != null)
                {
                    if (e.CellStyle != null)
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(56, 57, 60);
                    }
                    if (!string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        if (e.CellStyle != null)
                        {
                            e.CellStyle.ForeColor = System.Drawing.Color.Yellow;
                        }
                    }
                    else
                    {
                        if (e.CellStyle != null)
                        {
                            e.CellStyle.ForeColor = Color.FromArgb(239, 112, 32);
                        }
                    }
                    string? warningList = dataGridViewFiles.Rows[e.RowIndex].Cells["WarningList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = warningList;
                }
                if (dataGridViewFiles.Columns["ErrorCount"] != null && Convert.ToInt32(dataGridViewFiles.Rows[e.RowIndex].Cells["ErrorCount"].Value) > 0)
                {
                    if (e.CellStyle != null)
                    {
                        e.CellStyle.ForeColor = Color.Red;

                        string? errorFileList = dataGridViewFiles.Rows[e.RowIndex].Cells["ErrorList"].Value.ToString();
                        dataGridViewFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = errorFileList;
                    }
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

            _dataCache.ClearCache();
            int orderCount = 0;
            int fileCount = 0;
            int worksheetCount = 0;
            int ItemCount = 0;
            int materialCount = 0;
            int warningCount = 0;
            int errorCount = 0;

            lbInfoOrdersCount.Text = orderCount.ToString();
            lbInfoFilesCount.Text = fileCount.ToString();
            lbInfoWorksheetsCount.Text = worksheetCount.ToString();
            lbInfoItemsCount.Text = ItemCount.ToString();
            lbInfoMaterialCount.Text = materialCount.ToString();
            lbInfoWarningCount.Text = warningCount.ToString();
            lbInfoErrorCount.Text = errorCount.ToString();

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

                _progressValue.MinValue = 0;
                _progressValue.MaxValue = 100;
                _progressValue.Value = 0;
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;

                if (_dataTable.Rows.Count > 0)
                {

                    _progressValue.ProgressTitle = "Removing Existing data ...";
                    _progress?.Report(_progressValue);

                    if (_dataTable.Rows.Count > 0)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {

                                _dataTable.Clear();

                            }));
                        }
                        else
                        {

                            _dataTable.Clear();

                        }

                    }
                }

                progressBarForm.Cursor = Cursors.WaitCursor;
                progressBarForm.Show();
                await progressBarForm.FormReadyAsync();

                #region  Start Loading Files

                _progressValue.ProgressTitle = "Refreshing Files ...";
                _progress?.Report(_progressValue);

                await _readServices.ReadAsync(_progressValue, _progress);
                _progress?.Report(_progressValue);

                List<A2POrder> a2pOrders = _dataCache.GetAllOrders();
                if (a2pOrders.Count == 0)
                {
                    progressBarForm.Close();
                    _ = MessageBox.Show("No Orders found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                foreach (A2POrder a2pOrder in a2pOrders)
                {

                    _progress?.Report(_progressValue);
                    try
                    {

                        OrderDTO orderDTO = await MapToOrderDTOAsync(a2pOrder);

                        int fatal = CountReadFatal(a2pOrder);
                        int error = CountReadError(a2pOrder);
                        int alreadyImportedErrors = AlreadyImportedErrors(a2pOrder);

                        if (error > 0 || fatal > 0)
                        {
                            orderDTO.Import = false;
                        }

                        _ = _dataTable.Rows.Add
                            (orderDTO.Order,
                              orderDTO.Currency,
                              orderDTO.FileCount,
                              orderDTO.FileList,
                              orderDTO.WorksheetCount,
                              orderDTO.WorksheetList,
                              orderDTO.Items,
                              orderDTO.ItemList,
                              orderDTO.Materials,
                              orderDTO.Import,
                              orderDTO.WarningCount,
                              orderDTO.WarningList,
                              orderDTO.ErrorCount,
                              orderDTO.ErrorList
                            );
                        this.PerformLayout();

                        orderCount++;
                        fileCount += orderDTO.FileCount;
                        worksheetCount += orderDTO.WorksheetCount;
                        ItemCount += orderDTO.Items;
                        materialCount += orderDTO.Materials;
                        warningCount += orderDTO.WarningCount;
                        errorCount += orderDTO.ErrorCount;

                        lbInfoOrdersCount.Text = orderCount.ToString();
                        lbInfoFilesCount.Text = fileCount.ToString();
                        lbInfoWorksheetsCount.Text = worksheetCount.ToString();
                        lbInfoItemsCount.Text = ItemCount.ToString();
                        lbInfoMaterialCount.Text = materialCount.ToString();
                        lbInfoWarningCount.Text = warningCount.ToString();
                        lbInfoErrorCount.Text = errorCount.ToString();

                    }
                    catch (Exception ex)
                    {
                        _logService.Debug(ex, "Error adding individual a2pOrder to data table");
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
                DataGridViewLogReadOnlyRows();
                progressBarForm.Close();
                #endregion

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($@"An a2pOrderError occurred during the import: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void DataGridViewLogReadOnlyRows()
        {

            foreach (DataGridViewRow row in dataGridViewFiles.Rows)
            {
                int rowIndex = row.Index;
                string? orderNumber = dataGridViewFiles.Rows[rowIndex].Cells["Order"].Value.ToString();
                if (orderNumber == null)
                {
                    continue;
                }

                A2POrder? a2pOrder = _dataCache.GetOrder(orderNumber);

                if (a2pOrder == null)
                {
                    return;
                }
                int fatal = CountReadFatal(a2pOrder);

                _ = CountReadError(a2pOrder);

                _ = AlreadyImportedErrors(a2pOrder);
                if (fatal > 0)
                {
                    dataGridViewFiles.Rows[rowIndex].Cells["Import"].Value = false;
                    dataGridViewFiles.Rows[rowIndex].Cells["Import"].ReadOnly = true;
                }

            }

        }

        public async Task ImportFilesAsync()
        {
            Dictionary<string, int> keyValuePairs = [];
            for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
            {

                if ((bool)dataGridViewFiles.Rows[i].Cells["Import"].Value)
                {

                    A2POrder? a2pOrder = _dataCache.GetOrder(dataGridViewFiles.Rows[i].Cells["Order"].Value.ToString()!);
                    if (a2pOrder != null)
                    {
                        if (AlreadyImportedErrors(a2pOrder) > 0)
                        {
                            keyValuePairs.Add(a2pOrder.Order, i);
                        }
                    }

                }

            }

            bool cancel = OrdersToOverwrite(keyValuePairs);
            if (cancel)
            {
                return;
            }

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
                //ProgressBar. Create a new instance of the ProgressBarForm   
                //=======================================================================================================
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
                    progressBarForm.progressBar.ForeColor = System.Drawing.Color.FromArgb(239, 112, 32);
                };
                Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);
                _progress = progress;
                _progress?.Report(_progressValue);
                progressBarForm.plMainPanel.Cursor = Cursors.WaitCursor;
                progressBarForm.Show();
                await progressBarForm.FormReadyAsync();
                await _orderWriteProcessor.WriteAsync(_progressValue, _progress);

                string root = _configuration["AppSettings:Folders:WorkFolder"] ?? @"C:\Temp\Import";
                string success = Path.Combine(root, _configuration["AppSettings:Folders:ImportSuccess"] ?? "Import_Success");
                string failed = Path.Combine(root, _configuration["AppSettings:Folders:ImportFailed"] ?? "Import_Failed");

                for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridViewFiles.Rows[i];
                    if (row.Cells["Import"].Value != null && (bool)row.Cells["Import"].Value)
                    {
                        A2POrder? a2pOrder = _dataCache.GetOrder(row.Cells["Order"].Value.ToString()!);

                        if (a2pOrder == null)
                        {
                            return;
                        }

                        int fatal = CountReadFatal(a2pOrder);
                        int error = CountReadError(a2pOrder);
                        int alreadyImportedErrors = AlreadyImportedErrors(a2pOrder);

                        object orderNumber = row.Cells["Import"].Value;

                        _dataTable.Rows[i].Delete();

                        if (orderNumber != null)
                        {
                            _ = _dataCache.RemoveOrder(orderNumber.ToString()!);
                        }

                    }

                }
                progressBarForm.Close();
                _fileService.MoveFilesAsync();
                await OrdersLoad();
            }

            catch (Exception ex)
            {
                _logService.Error(ex, "Error loading FileForm");
                _ = MessageBox.Show(@"An a2pOrderError occurred while loading the files.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { dataGridViewFiles.ResumeLayout(false); }
        }

        public async Task<OrderDTO> MapToOrderDTOAsync(A2POrder a2pOrder) => await Task.Run(() =>
        {
            OrderDTO orderDTO = new()
            {
                Order = a2pOrder.Order,
                Currency = a2pOrder.Files
                    .SelectMany(file => file.Worksheets)
                    .FirstOrDefault(worksheet => !string.IsNullOrEmpty(worksheet.Currency))?.Currency ?? string.Empty,
                FileCount = a2pOrder.Files.Count,
                FileList = string.Join("\n", a2pOrder.Files.Select(file => file.FileName)),

                WorksheetCount = a2pOrder.Files.Sum(file => file.Worksheets?.Count ?? 0),
                WorksheetList = string.Join("\n", a2pOrder.Files.SelectMany(file => file.Worksheets).Select(ws => ws.Name)),

                Items = a2pOrder.Items.Count, // Added line to count Items
                ItemList = string.Join("\n", a2pOrder.Items.Select(item => item.Item)),
                Materials = a2pOrder.Materials.Count, // Added line to count Materials
                WarningCount = a2pOrder.ReadErrors
                    .Where(error => error.Level is ErrorLevel.Warning)
                    .Select(error => new { error.Level, error.Code, error.Message })
                    .Distinct()
                    .Count(),
                WarningList = string.Join("\n", a2pOrder.ReadErrors
                    .Where(error => error.Level is ErrorLevel.Warning)
                    .Select(error => $"ErrorLevel: {error.Level}, ErrorCode: {error.Code}, Message: {error.Message}")
                    .Distinct()),

                ErrorCount = a2pOrder.ReadErrors
                    .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                    .Select(error => new { error.Level, error.Code, error.Message })
                    .Distinct()
                    .Count(),
                ErrorList = string.Join("\n", a2pOrder.ReadErrors
                    .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                    .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                    .Distinct()),
                Import = CountReadFatal(a2pOrder) + CountReadError(a2pOrder) + AlreadyImportedErrors(a2pOrder) > 0,

            };
            return orderDTO;
        });

        #endregion -== Methods ==-
        #region -== Form Evenets ==-

        private void OrdersForm_Load(object sender, EventArgs e) => this.PerformAutoScale();
        private void FileForm_Shown(object sender, EventArgs e)
        {
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private void OrdersForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();
            plGridPanel.ResumeLayout(true);
            plTbSBInfo.ResumeLayout(true);
            dataGridViewFiles.ResumeLayout(true);
            this.ResumeLayout(true);

        }
        private void FileForm_ResizeBegin(object sender, EventArgs e) => this.SuspendLayout();
        private void FileForm_ResizeEnd(object sender, EventArgs e)
        {
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion -== Form Evenets ==-

        #region -== Contextual Menu Evenets ==-
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
        private void dataGridViewFiles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        private int CountReadFatal(A2POrder a2pOrder) => a2pOrder.ReadErrors.Count(error => error.Level == ErrorLevel.Fatal);

        private int CountReadError(A2POrder a2pOrder) => a2pOrder.ReadErrors.Count(error => error.Level == ErrorLevel.Error);

        private int CountWriteFatal(A2POrder a2pOrder) => a2pOrder.WriteErrors.Count(error => error.Level == ErrorLevel.Fatal);

        private int CountWriteError(A2POrder a2pOrder) => a2pOrder.WriteErrors.Count(error => error.Level == ErrorLevel.Error);
        private int AlreadyImportedErrors(A2POrder a2pOrder) => a2pOrder.ReadErrors.Count(error => error.Code == ErrorCode.DatabaseRead_OrderAlreadyImported);

        private bool OrdersToOverwrite(Dictionary<string, int> keyValuePairs)
        {

            if (keyValuePairs.Count == 0)
            {
                return false;
            }

            string orderlist = string.Join("\n", keyValuePairs.Keys);

            string WarningMessage = $"The following Orders have already been imported and will be overwritten:\n\n{orderlist}\n\nDo you want to continue?";

            DialogResult result = MessageBox.Show(WarningMessage, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {

                return false;
            }
            else
            {

                for (int i = 0; i < keyValuePairs.Count; i++)
                {
                    dataGridViewFiles.Rows[keyValuePairs.ElementAt(i).Value].Cells["Import"].Value = false;
                }
                return true;
            }
        }
        #endregion 
    }
}
