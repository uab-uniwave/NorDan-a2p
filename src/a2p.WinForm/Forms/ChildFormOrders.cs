using System.Data;

using a2p.Application.DTOs;
using a2p.Application.Interfaces.Excel;
using a2p.Application.Interfaces.Excel.Files;
using a2p.Application.Interfaces.Orchestrators;
using a2p.Application.Interfaces.Services;
using a2p.Application.Models;
using a2p.Domain.Enums;
using a2p.Infrastructure.Models;


namespace a2p.WinForm.Forms
{
    public partial class ChildFormOrders : Form
    {
        private readonly ISettingsService _settingsService;
        private SettingsContainer _settingsContainer;
        private AppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly IReadService _readService;
        private readonly IWriteService _writeService;
        private List<OrderDto> _orders;

        private DataTable _dataTable;
        private BindingSource _bindingSource;
        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        public ChildFormOrders(ISettingsService userSettingsService,
                          ILogger logger,
                          IFileService fileService,
                          IExcelService excelService,
                          IReadService readService,
                          IWriteService writeService)

        {

            _settingsService = userSettingsService;
            _appSettings = _settingsService.LoadSettings();
            _settingsContainer = _settingsService.LoadAllSettings();
            _fileService = fileService;
            _logger = logger;
            _excelService = excelService;
            _writeService = writeService;
            _readService = readService;

            _orders = [];
            //==================================
            _dataTable = new DataTable();
            _bindingSource = [];
            //==================================
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent();
            InitializeGrid();
            InitializeTable();

        }
        //===============================================================
        // -= Initialization =-
        //===============================================================
        private void InitializeGrid()
        {
            try
            {
                if (!dataGridViewFiles.Columns.Contains("Image"))

                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewImageColumn
                    {
                        HeaderText = string.Empty,
                        MinimumWidth = 40,
                        Width = 40,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                        Resizable = DataGridViewTriState.False,
                        DataPropertyName = "Image",
                        Name = "Image",
                        ReadOnly = true,

                    });

                }
                if (!dataGridViewFiles.Columns.Contains("OrderNumber"))
                {

                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {

                        HeaderText = "OrderNumber",
                        DataPropertyName = "OrderNumber",
                        Name = "OrderNumber",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                if (!dataGridViewFiles.Columns.Contains("SalesDocument"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Sales Document",
                        DataPropertyName = "SalesDocument",
                        Name = "Document",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                if (!dataGridViewFiles.Columns.Contains("ItemsDto"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "ItemsDto",
                        DataPropertyName = "ItemsDto",
                        Name = "ItemsDto",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }
                if (!dataGridViewFiles.Columns.Contains("ItemList"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "ItemName List",
                        DataPropertyName = "ItemList",
                        Name = "ItemList",
                        ReadOnly = true,
                        Visible = false
                    });
                }
                if (!dataGridViewFiles.Columns.Contains("Quantity"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Quantity",
                        DataPropertyName = "Quantity",
                        Name = "Quantity",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                if (!dataGridViewFiles.Columns.Contains("Area"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Area",
                        DataPropertyName = "Area",
                        Name = "Area",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                if (!dataGridViewFiles.Columns.Contains("Weight"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Weight",
                        DataPropertyName = "Weight",
                        Name = "Weight",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                if (!dataGridViewFiles.Columns.Contains("Hours"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Hours",
                        DataPropertyName = "Hours",
                        Name = "Hours",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                if (!dataGridViewFiles.Columns.Contains("Cost"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Cost",
                        DataPropertyName = "Cost",
                        Name = "Cost",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }

                if (!dataGridViewFiles.Columns.Contains("Amount"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Amount",
                        DataPropertyName = "Amount",
                        Name = "Amount",
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
                        HeaderText = "OrderNumber List",
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

                if (!dataGridViewFiles.Columns.Contains("Materials"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Materials",
                        DataPropertyName = "Materials",
                        Name = "Materials",
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
                if (!dataGridViewFiles.Columns.Contains("WarningCount"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Warnings",
                        DataPropertyName = "WarningCount",
                        Name = "WarningCount",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
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

                if (!dataGridViewFiles.Columns.Contains("FatalCount"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Fatal",
                        DataPropertyName = "FatalCount",
                        Name = "FatalCount",
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }
                if (!dataGridViewFiles.Columns.Contains("FatalList"))
                {
                    _ = dataGridViewFiles.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "FatalList",
                        DataPropertyName = "FatalList",
                        Name = "FatalList",
                        ReadOnly = true,
                        Visible = false
                    });
                }

                //=========================================================================================
                DataGridViewCellStyle ColumnHeadersDefaultCellStyle = new()
                {
                    BackColor = Color.FromArgb(56, 57, 60),
                    ForeColor = Color.FromArgb(239, 112, 32),
                    //    SelectionBackColor = Color.FromArgb(239, 112, 32),
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

            }
            catch (Exception ex)
            {
                _logger.LogError("OrderNumber Form: Unhandled Error initializing grid view. Exception: {$Exception}.", ex.Message);
            }

        }
        private void InitializeTable()
        {

            try
            {
                dataGridViewFiles.SuspendLayout();
                _ = _dataTable.Columns.Add("Image", typeof(Image));
                _ = _dataTable.Columns.Add("OrderNumber", typeof(string));
                _ = _dataTable.Columns.Add("SalesDocument", typeof(string));
                _ = _dataTable.Columns.Add("ItemsDto", typeof(int));
                _ = _dataTable.Columns.Add("ItemList", typeof(string));
                _ = _dataTable.Columns.Add("Quantity", typeof(int));
                _ = _dataTable.Columns.Add("Area", typeof(string));
                _ = _dataTable.Columns.Add("Weight", typeof(string));
                _ = _dataTable.Columns.Add("Hours", typeof(string));
                _ = _dataTable.Columns.Add("Cost", typeof(string));
                _ = _dataTable.Columns.Add("Amount", typeof(string));
                _ = _dataTable.Columns.Add("Currency", typeof(string));
                _ = _dataTable.Columns.Add("FileCount", typeof(string));
                _ = _dataTable.Columns.Add("FileList", typeof(string));
                _ = _dataTable.Columns.Add("WorksheetCount", typeof(int));
                _ = _dataTable.Columns.Add("WorksheetList", typeof(string));
                _ = _dataTable.Columns.Add("Materials", typeof(int));
                _ = _dataTable.Columns.Add("Import", typeof(bool));
                _ = _dataTable.Columns.Add("WarningCount", typeof(string));
                _ = _dataTable.Columns.Add("WarningList", typeof(string));
                _ = _dataTable.Columns.Add("ErrorCount", typeof(string));
                _ = _dataTable.Columns.Add("ErrorList", typeof(string));
                _ = _dataTable.Columns.Add("FatalCount", typeof(string));
                _ = _dataTable.Columns.Add("FatalList", typeof(string));

                _bindingSource.DataSource = _dataTable;
                dataGridViewFiles.DataSource = _bindingSource;

            }
            catch (Exception ex)
            {
                _logger.LogError("OrderNumber Form: Unhandled Error initializing data table. Exception {$Exception}.", ex.Message);

            }
            finally
            {
                dataGridViewFiles.ResumeLayout(false);
            }

        }

        private void Initialize()
        {
            _progressValue = new();
            _progress = new Progress<ProgressValue>();

        }
        //===============================================================
        // -= Form Events =-
        //===============================================================
        private void OrdersForm_Load(object sender, EventArgs e)
        {
            PerformAutoScale();
        }
        private void FileForm_Shown(object sender, EventArgs e)
        {
            this.lbTitle.Text = "";
            ResumeLayout(false);
            PerformLayout();
        }
        private void OrdersForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            PerformAutoScale();
            plGridPanel.ResumeLayout(true);
            plTbSBInfo.ResumeLayout(true);
            dataGridViewFiles.ResumeLayout(true);
            ResumeLayout(true);

        }
        private void FileForm_ResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }
        private void FileForm_ResizeEnd(object sender, EventArgs e)
        {
            ResumeLayout(false);
            PerformLayout();
        }
        //===============================================================
        // -= Data Grids =-
        //===============================================================
        private void DataGridViewFiles_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            _logger.LogError("FF:GridViewFiles. Error in column {$Column}, row {$Row}: {$Exception}", e.ColumnIndex, e.RowIndex, e.Exception?.Message ?? "Exception details missing.");
            Console.WriteLine($"GridViewFiles Error in column {e.ColumnIndex}, row {e.RowIndex}: {e.Exception?.Message ?? "Exception details missing."}");
            e.ThrowException = false;

        }
        private bool isFormatting = false;
        private void DataGridViewFiles_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (isFormatting)
            {
                return;
            }

            try
            {
                isFormatting = true;

                if (e.CellStyle == null)
                {
                    return;
                }

                dataGridViewFiles.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (dataGridViewFiles.Columns["ItemsDto"] != null && e.ColumnIndex == dataGridViewFiles.Columns["ItemsDto"].Index && e.Value != null)
                {
                    string? itemList = dataGridViewFiles.Rows[e.RowIndex].Cells["ItemList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells["ItemsDto"].ToolTipText = itemList;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }

                if (dataGridViewFiles.Columns["FileCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["FileCount"].Index && e.Value != null)
                {
                    string? fileList = dataGridViewFiles.Rows[e.RowIndex].Cells["FileList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells["FileCount"].ToolTipText = fileList;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }

                if (dataGridViewFiles.Columns["WorksheetCount"] != null && e.ColumnIndex == dataGridViewFiles.Columns["WorksheetCount"].Index && e.Value != null)
                {
                    string? worksheetList = dataGridViewFiles.Rows[e.RowIndex].Cells["WorksheetList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells["WorksheetCount"].ToolTipText = worksheetList;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }

                if (Convert.ToInt32(dataGridViewFiles.Rows[e.RowIndex].Cells["WarningCount"].Value) > 0)
                {
                    e.CellStyle.ForeColor = Color.Yellow;
                    string? warningList = dataGridViewFiles.Rows[e.RowIndex].Cells["WarningList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells["WarningCount"].ToolTipText = warningList;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }

                if (Convert.ToInt32(dataGridViewFiles.Rows[e.RowIndex].Cells["ErrorCount"].Value) > 0)
                {
                    e.CellStyle.ForeColor = Color.Orange;
                    string? errorList = dataGridViewFiles.Rows[e.RowIndex].Cells["ErrorList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells["ErrorCount"].ToolTipText = errorList;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }

                if (Convert.ToInt32(dataGridViewFiles.Rows[e.RowIndex].Cells["FatalCount"].Value) > 0)
                {
                    e.CellStyle.ForeColor = Color.WhiteSmoke;
                    e.CellStyle.BackColor = Color.Red;
                    // Fix: Font.Bold is read-only, so create a new Font with Bold style
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);

                    string? fatalList = dataGridViewFiles.Rows[e.RowIndex].Cells["FatalList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells["FatalCount"].ToolTipText = fatalList;

                }
                if (Convert.ToInt32(dataGridViewFiles.Rows[e.RowIndex].Cells["FatalCount"].Value) +
                    Convert.ToInt32(dataGridViewFiles.Rows[e.RowIndex].Cells["ErrorCount"].Value) +

                    Convert.ToInt32(dataGridViewFiles.Rows[e.RowIndex].Cells["WarningCount"].Value) +
                    Convert.ToInt32(dataGridViewFiles.Rows[e.RowIndex].Cells["WarningCount"].Value) == 0)
                {
                    e.CellStyle.ForeColor = Color.YellowGreen;
                }

            }
            catch (Exception)
            {
                _logger.LogError("OrderNumber Form: Unhandled Error formatting cells!");
            }
            finally
            {
                isFormatting = false;
            }
        }
        private void DataGridViewLogReadOnlyRows()
        {
            try
            {

                for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
                {
                    int rowIndex = i;
                    string? orderNumber = dataGridViewFiles.Rows[rowIndex].Cells["OrderNumber"].Value.ToString();
                    if (orderNumber == null)
                    {
                        continue;
                    }

                    for (int j = 0; j < _orders.Count; j++)
                    {
                        if (_orders[j].OrderNumber == orderNumber)
                        {
                            rowIndex = j;

                            int readFatal = CountReadFatal(_orders[j]);
                            int readError = CountReadError(_orders[j]);

                            if (readFatal > 0)
                            {
                                dataGridViewFiles.Rows[rowIndex].Cells["Import"].Value = false;
                                dataGridViewFiles.Rows[rowIndex].Cells["Import"].ReadOnly = true;
                            }
                            else if (readError > 0)
                            {
                                dataGridViewFiles.Rows[rowIndex].Cells["Import"].Value = false;
                                dataGridViewFiles.Rows[rowIndex].Cells["Import"].ReadOnly = false;
                            }
                            else
                            {
                                dataGridViewFiles.Rows[rowIndex].Cells["Import"].Value = true;
                                dataGridViewFiles.Rows[rowIndex].Cells["Import"].ReadOnly = false;
                            }

                            if ((bool)dataGridViewFiles.Rows[rowIndex].Cells["Import"].Value == false)
                            {
                                dataGridViewFiles.Rows[rowIndex].Cells["Import"].Style.ForeColor = Color.LightGray;
                            }

                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                _logger.LogError("OrderNumber Form: Unhandled error setting grid row readonly. Exception: {$Exception}.", ex.Message);
            }

        }



        private void DataGridViewFiles_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dataGridViewFiles.IsCurrentCellDirty)
            {
                dataGridViewFiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        //===============================================================
        // -= Context menu =-
        //===============================================================
        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = _dataTable.Rows.Count;

            for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
            {
                dataGridViewFiles.Rows[i].Cells["Import"].Value = true;
            }
        }
        private void DeselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
            {
                dataGridViewFiles.Rows[i].Cells["Import"].Value = false;
            }
        }
        //===============================================================
        // -= Main Events =-
        //===============================================================
        public async Task OrdersLoad()
        {

            try
            {

                await ClearData();

                //ProgressBar. Create a new instance of the ProgressBarForm
                //=====================================================================================================
                using FormProgressBar progressBarForm = new()
                {
                    StartPosition = FormStartPosition.CenterParent // Set to center relative to parent
                };
                progressBarForm.Load += (sender, args) =>
                {
                    progressBarForm.Location = new Point(
                        Location.X + ((Width - progressBarForm.Width) / 2),
                        Location.Y + ((Height - progressBarForm.Height) / 2)
                    );
                };
                Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);
                _progress = progress;

                _progressValue.ProgressTitle = "Loading Orders...";
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = "Start Searching ...";
                _progressValue.ProgressTask3 = string.Empty;

                _progress?.Report(_progressValue);
                progressBarForm.Show();


                //Read Orders Data
                //=====================================================================================================
                List<OrderDto> orders = await _readService.ReadAsync(_progressValue, _progress);
                _progress?.Report(_progressValue);

                if (orders == null || orders.Count == 0)
                {
                    return;

                }
                _orders = orders;

                //Populate DataTable
                //=====================================================================================================
                await UpdateDatable(orders, 1);
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = "Loading Finished.";
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue); progressBarForm.Show();

                progressBarForm.Close();


                DataGridViewLogReadOnlyRows();
                _progressValue.ProgressTitle = string.Empty;
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

            }
            catch (Exception ex)
            {
                _logger.LogError("OrderNumber Form: Unhandled Error loading Orders. Exception: {$Exception}.", ex.Message);
            }

        }
        private async Task ClearData()
        {

            _progressValue.ProgressTitle = string.Empty;
            _progressValue.ProgressTask1 = string.Empty;
            _progressValue.ProgressTask2 = string.Empty;
            _progressValue.ProgressTask3 = string.Empty;
            _progressValue.Value = 0;
            _progressValue.MinValue = 0;
            _progressValue.MaxValue = 100;
            _progress?.Report(_progressValue);

            await Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(_dataTable.Clear));
                }

            });

        }
        public async Task ImportAsync()
        {
            List<OrderDto> importOrdersDto
                = [];

            try
            {


                for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
                {
                    if ((bool)dataGridViewFiles.Rows[i].Cells["Import"].Value)
                    {

                        for (int j = 0; j < _orders.Count; j++)
                        {

                            if (_orders[j].OrderNumber == dataGridViewFiles.Rows[i].Cells["OrderNumber"].Value.ToString())
                            {
                                _orders[j].Import = true;

                                importOrdersDto.Add(_orders[j]);

                                if (CountReadExistsError(_orders[j]) > 0)
                                {

                                    DialogResult result
                                        = MessageBox.Show($"OrderNumber {_orders[j].OrderNumber} contains data!\n" +
                               "\nYes - Positions and Material Needs will be deleted." +
                               "\nNo - Positions and Material Needs will kept." +
                               "\n         Attention!!! Positions and Material Needs could be duplicated!" +
                               "\nCancel - OrderNumber will not be imported.", "Warning",
                                                                 MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                                    if (result == DialogResult.Yes)
                                    {
                                        _orders[j].DeleteExistsing = true;



                                    }
                                    if (result == DialogResult.No)
                                    {

                                        _orders[j].DeleteExistsing = false;


                                    }

                                    if (result == DialogResult.Cancel)
                                    {

                                        dataGridViewFiles.Rows[i].Cells["Import"].Value = false;
                                        importOrdersDto.Remove(_orders[j]);

                                    }



                                }

                                break;

                            }

                        }

                    }

                }



                //ProgressBar. Create a new instance of the ProgressBarForm   
                //=======================================================================================================
                using FormProgressBar progressBarForm = new()
                {
                    StartPosition = FormStartPosition.CenterParent // Set to center relative to parent
                };
                progressBarForm.Load += (sender, args) =>
                {
                    progressBarForm.Location = new Point(
                        Location.X + ((Width - progressBarForm.Width) / 2),
                        Location.Y + ((Height - progressBarForm.Height) / 2)
                        );
                    progressBarForm.progressBar.Style = ProgressBarStyle.Continuous;
                    progressBarForm.progressBar.ForeColor = Color.FromArgb(239, 112, 32);
                };

                ProgressValue progressvalue = new ProgressValue();
                Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);

                int totalItems = importOrdersDto.Sum(order => order.ItemsDto.Count);
                int totalMaterials = importOrdersDto.Sum(order => order.MaterialsDto.Count);
                int totalOrders = importOrdersDto.Count;

                _progress = progress;
                _progressValue = progressvalue;
                _progressValue.ProgressTitle = "Importing Orders...";
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;

                _progressValue.TotalValue = totalItems * 20 + totalMaterials * 1 + totalOrders * 230 + 40;
                _progressValue.CurrentValue = 0;
                _progressValue.Value = 0;
                _progressValue.MaxValue = 100;
                _progressValue.MinValue = 0;



                //20pts x1   single per import- exceExcelOrderDto Form Preparing Import 


                //30 pts x 1  per OrderNumber - Write Service  Deletig existinfg data

                //100  x1 per OrderNumber - PrefsuiteService  Save Doc
                //30 pts x 1  per OrderNumber - Write Service  inserting material needa
                //100  x1 per OrderNumber - PrefsuiteService  Load Doc
                //100  x1 per ItemName  - PrefsuiteService  Insert ItemName

                //10pts. x 1 per  ItemName -  Write service -   Inserting data into DB
                //1 pts x per material   Write service 







                //20pts x1   single per import- exceExcelOrderDto Form finishing

                _progressValue.CurrentValue = _progressValue.CurrentValue + 30; //x20 / x1  
                _progressValue.ProgressTitle = "Importing orders ... ";
                _progressValue.ProgressTask1 = $"Orders Count {totalOrders} pending import";
                _progressValue.ProgressTask2 = $"ItemsDto Count {totalItems} pending import";
                _progressValue.ProgressTask3 = $"Orders Count {totalMaterials} pending import";
                _progress?.Report(_progressValue);
                progressBarForm.Show();


                _progress?.Report(_progressValue);
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;

                for (int i = 0; i < importOrdersDto.Count; i++)
                {

                    _progressValue.ProgressTask1 = $"Inserting OrderNumber {i + 1} of {importOrdersDto.Count} - OrderNumber # {importOrdersDto[i].OrderNumber} ({importOrdersDto[i].SalesDocument.Number}/{importOrdersDto[i].SalesDocument.Number})...";
                    _progressValue.ProgressTask3 = string.Empty;
                    progressBarForm.Show();
                    await _writeService.WriteAsync(importOrdersDto[i], _progressValue, _progress);


                    //var a2POrder = Result.Item1;
                    //_progressValue = Result.Item2;


                    //importOrdersDto[i] = a2POrder;

                }

                await UpdateDatable(importOrdersDto, 2);



                _progressValue.CurrentValue = _progressValue.CurrentValue + 20; //x20 /x2  
                _progressValue.ProgressTitle = "Importing orders ... ";
                _progressValue.ProgressTask1 = "Import Finished";
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);



                progressBarForm.Close(); // TODO: on cancel overwrite should stay grid data
            }

            catch (Exception ex)
            {
                _logger.LogError("OrderNumber Form: Unhandled error loading importing orders. Exception: {$Exception}.", ex.Message);
                _ = MessageBox.Show($"An Error occurred while loading the files." +
                    $"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dataGridViewFiles.ResumeLayout(false);
            }
        }

        public async Task<OrderRecord> MapToReadOrderDTOAsync(OrderDto exceExcelOrderDto
            , int type)   // type 1 - read; 2 - write 
        {
            try
            {

                Image a = imageList1.Images[0];
                OrderRecord orderRecord = new();
                int warningCount = 0;
                int errorCount = 0;
                int fatalCount = 0;

                await Task.Run(() =>
                {
                    orderRecord.OrderNumber = exceExcelOrderDto.OrderNumber;
                    orderRecord.SalesDocument = $"{exceExcelOrderDto.SalesDocument.Number}/{exceExcelOrderDto.SalesDocument.Version}";
                    orderRecord.Items = exceExcelOrderDto.ItemsDto.Count(); // Added line to count ItemsDto
                    orderRecord.ItemList = string.Join("\n", exceExcelOrderDto.ItemsDto.Select(item => item.ItemName));
                    orderRecord.Quantity = exceExcelOrderDto.ItemsDto.ToArray().Sum(item => item.Quantity);
                    orderRecord.Area = exceExcelOrderDto.ItemsDto.ToArray().Sum(item => item.TotalArea);
                    orderRecord.Weight = exceExcelOrderDto.ItemsDto.ToArray().Sum(item => item.TotalWeight);
                    orderRecord.Hours = exceExcelOrderDto.ItemsDto.ToArray().Sum(item => item.TotalHours);
                    orderRecord.Cost = exceExcelOrderDto.ItemsDto.ToArray().Sum(item => item.TotalCost);
                    orderRecord.Amount = exceExcelOrderDto.ItemsDto.ToArray().Sum(item => item.TotalPrice);
                    orderRecord.Currency = exceExcelOrderDto.Currency ?? string.Empty;
                    //  orderRecord.FileCount = exceExcelOrderDto.Files.Count;
                    //orderRecord.FileList = string.Join("\n", exceExcelOrderDto.Files.Select(file => file.FileName));
                    // orderRecord.WorksheetCount = exceExcelOrderDto.Files.Sum(file => file.Worksheets?.Count ?? 0);
                    //  orderRecord.WorksheetList = string.Join("\n", exceExcelOrderDto.Files.SelectMany(file => file.Worksheets).Select(ws => ws.Name));
                    orderRecord.Materials = exceExcelOrderDto.MaterialsDto.Count(); // Added line to count Materials

                    if (type == 1)
                    {
                        warningCount = 0;

                        //       exceExcelOrderDto.Errors
                        //.Where(error => error.Level is ErrorLevel.Warning)
                        //.Where(error => (int)error.Code < 3000)
                        //.Select(error => new { error.Level, error.Code, error.Message })
                        //.Distinct()
                        //.Count();

                        orderRecord.WarningCount = 0;
                        //      warningCount;
                        //orderRecord.WarningList = string.Join("\n", exceExcelOrderDto.Errors
                        //            .Where(error => error.Level is ErrorLevel.Warning)
                        //            .Select(error => $"ErrorLevel: {error.Level}, ErrorCode: {error.Code}, Message: {error.Message}")
                        //            .Distinct());

                        errorCount = 0;
                        //exceExcelOrderDto.Errors
                        //    .Where(error => error.Level is ErrorLevel.Error)
                        //    .Select(error => new { error.Level, error.Code, error.Message })
                        //    .Distinct()
                        //    .Count();


                        orderRecord.ErrorCount = errorCount;

                        //orderRecord.ErrorList = string.Join("\n", exceExcelOrderDto.Errors
                        //    .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                        //    .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                        //    .Distinct());

                        //fatalCount = exceExcelOrderDto.Errors
                        //.Where(error => error.Level is ErrorLevel.Fatal)
                        //.Select(error => new { error.Level, error.Code, error.Message })
                        //.Distinct()
                        //.Count();

                        //orderRecord.FatalCount = fatalCount;
                        //orderRecord.FatalList = string.Join("\n", exceExcelOrderDto.Errors
                        //    .Where(error => error.Level is ErrorLevel.Fatal)
                        //    .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                        //    .Distinct());

                    }


                    if (type == 2)
                    {


                        //warningCount = exceExcelOrderDto.Errors
                        //     .Where(error => error.Level is ErrorLevel.Warning)
                        //     .Where(error => (int)error.Code > 3000)
                        //     .Select(error => new { error.Level, error.Code, error.Message })

                        //     .Distinct()
                        //     .Count();
                        //orderRecord.WarningCount = warningCount;
                        //orderRecord.WarningList = string.Join("\n", exceExcelOrderDto.Errors
                        //            .Where(error => error.Level is ErrorLevel.Warning)
                        //            .Select(error => $"ErrorLevel: {error.Level}, ErrorCode: {error.Code}, Message: {error.Message}")
                        //            .Distinct());

                        //errorCount = exceExcelOrderDto.Errors
                        //    .Where(error => error.Level is ErrorLevel.Error)
                        //    .Select(error => new { error.Level, error.Code, error.Message })
                        //    .Distinct()
                        //    .Count()
                        //+ exceExcelOrderDto.Errors
                        //    .Where(error => error.Level is ErrorLevel.Error)
                        //    .Select(error => new { error.Level, error.Code, error.Message })
                        //    .Distinct()
                        //    .Count();

                        //orderRecord.ErrorCount = errorCount;

                        //orderRecord.ErrorList = string.Join("\n", exceExcelOrderDto.Errors
                        //    .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                        //    .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                        //    .Distinct()) +

                        //string.Join("\n", exceExcelOrderDto.Errors
                        //    .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                        //    .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                        //    .Distinct());

                        //fatalCount = exceExcelOrderDto.Errors
                        //.Where(error => error.Level is ErrorLevel.Fatal)
                        //.Select(error => new { error.Level, error.Code, error.Message })
                        //.Distinct()
                        //.Count() + exceExcelOrderDto.Errors
                        //.Where(error => error.Level is ErrorLevel.Fatal)
                        //.Select(error => new { error.Level, error.Code, error.Message })
                        //.Distinct()
                        //.Count();

                        //orderRecord.FatalCount = fatalCount;
                        //orderRecord.FatalList = string.Join("\n", exceExcelOrderDto.Errors
                        //    .Where(error => error.Level is ErrorLevel.Fatal)
                        //    .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                        //    .Distinct()) +
                        //                    string.Join("\n", exceExcelOrderDto.Errors
                        //                           .Where(error => error.Level is ErrorLevel.Fatal)
                        //                           .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")

                        //                           .Distinct());



                    }
                    orderRecord.Import = CountReadTotalError(exceExcelOrderDto) <= 0;

                });





                return orderRecord;

            }
            catch (Exception ex)
            {
                _logger.LogError("OrderNumber Form: Unhandled error mapping ExcelOrderDto for grid : {$Exception}.", ex.Message);
                return new OrderRecord();
            }
        }

        //===============================================================
        // -= Read Errors =-
        //===============================================================
        private int CountReadWarning(Application.DTOs.OrderDto order)
        {
            //        return exceExcelOrderDto.Errors.Count(error => error.Level == ErrorLevel.Warning);
            return 0;
        }

        private int CountReadError(Application.DTOs.OrderDto order)

        {
            //      return exceExcelOrderDto.Errors.Count(error => error.Level == ErrorLevel.Error);
            return 0;
        }

        private int CountReadFatal(Application.DTOs.OrderDto order)
        {
            //     return exceExcelOrderDto.Errors.Count(error => error.Level == ErrorLevel.Fatal);
            return 0;
        }

        private int CountReadExistsError(Application.DTOs.OrderDto order)
        {
            //  return exceExcelOrderDto.Errors.Count(error => error.Code == ErrorCode.DatabaseRead_OrderAlreadyImported);
            return 0;
        }

        private int CountReadTotalError(Application.DTOs.OrderDto order)
        {
            //      return exceExcelOrderDto.Errors.Count(error => error.Level is ErrorLevel.Warning or ErrorLevel.Error or ErrorLevel.Fatal);
            return 0;
        }

        private int CountWriteFatal(Application.DTOs.OrderDto order)
        {
            //      return exceExcelOrderDto.Errors.Count(error => error.Level == ErrorLevel.Fatal);
            return 0;
        }

        private int CountWriteError(Application.DTOs.OrderDto order)
        {
            //        return exceExcelOrderDto.Errors.Count(error => error.Level == ErrorLevel.Error);
            return 0;
        }

        private int CountWriteWarning(Application.DTOs.OrderDto order)
        {
            //return exceExcelOrderDto.Errors.Count(error => error.Level == ErrorLevel.Warning);
            return 0;
        }

        private int CountWriteTotalError(Application.DTOs.OrderDto order)
        {
            //return exceExcelOrderDto.Errors.Count(error => error.Level is ErrorLevel.Warning or ErrorLevel.Error or ErrorLevel.Fatal);
            return 0;
        }



        private async Task UpdateDatable(List<Application.DTOs.OrderDto> ordersDto, int type)
        {

            _dataTable.Rows.Clear();

            int orderCount = 0;
            int fileCount = 0;
            int worksheetCount = 0;
            int itemCount = 0;
            int quantity = 0;
            decimal area = 0m;
            decimal weight = 0m;
            decimal hours = 0m;
            decimal cost = 0m;
            decimal amount = 0m;
            int materialCount = 0;
            int warningCount = 0;
            int errorCount = 0;
            foreach (OrderDto order in ordersDto)
            {

                if (order.Files.SelectMany(f => f.Worksheets).Count(w => w.WorksheetType == WorksheetType.Items) == 0)
                {

                    _logger.LogWarning("Found exceExcelOrderDto {$OrderNumber}, files, but items worksheet  is missing", order.OrderNumber);
                    continue;

                    //}

                    if (type == 1)
                    {
                        dataGridViewFiles.Columns["Import"].Visible = true;
                    }
                    if (type == 2)
                    {
                        if (order.Import == false)
                        {
                            continue;
                        }

                        dataGridViewFiles.Columns["Import"].Visible = false;

                    }

                    try
                    {

                        lbInfoOrdersCount.Text = orderCount.ToString();
                        lbInfoFilesCount.Text = fileCount.ToString();
                        lbInfoWorksheetsCount.Text = worksheetCount.ToString();
                        lbInfoItemsCount.Text = itemCount.ToString();
                        lbInfoMaterialCount.Text = materialCount.ToString();
                        lbInfoWarningCount.Text = warningCount.ToString();
                        lbInfoErrorCount.Text = errorCount.ToString();

                        OrderRecord orderRecord = await MapToReadOrderDTOAsync(order, type);   // 2 means import and should be used write errors

                        Image image;

                        if (orderRecord.FatalCount > 0)
                        {
                            orderRecord.Import = false;
                            image = imageList1.Images[3];

                        }

                        else if (orderRecord.ErrorCount > 0)
                        {
                            orderRecord.Import = false;
                            image = imageList1.Images[2];

                        }

                        else if (orderRecord.WarningCount > 0)
                        {
                            orderRecord.Import = false;
                            image = imageList1.Images[1];

                        }
                        else
                        {
                            orderRecord.Import = true;
                            image = imageList1.Images[0];
                        }

                        _ = _dataTable.Rows.Add
                        (

                            image,                         //"Image", typeof(Image));
                            orderRecord.OrderNumber,                       //"OrderNumber", typeof(string));
                            orderRecord.SalesDocument,                      //"SalesDocument", typeof(string));
                            orderRecord.Items,                              //"ItemsDto", typeof(int));
                            orderRecord.ItemList,                           //"ItemList", typeof(string));
                            orderRecord.Quantity,                           //"Quantity", typeof(int));
                            Math.Round(orderRecord.Area, 2),    //"Area", typeof(string));
                            Math.Round(orderRecord.Weight, 2),  //"Weight", typeof(string));
                            Math.Round(orderRecord.Hours, 2),   //"Hours", typeof(string));
                            Math.Round(orderRecord.Cost, 2),     //"Cost", typeof(string));
                            Math.Round(orderRecord.Amount, 2),   //"Amount", typeof(string));
                            orderRecord.Currency,                           //"Currency", typeof(string));
                            orderRecord.FileCount,                          //"FileList", typeof(string));
                            orderRecord.FileList,                           //"WorksheetCount", typeof(int));
                            orderRecord.WorksheetCount,                     //"WorksheetList", typeof(string));
                            orderRecord.WorksheetList,                      //"Materials", typeof(int));
                            orderRecord.Materials,                          //"Import", typeof(bool));
                            orderRecord.Import,                             //"WarningCount", typeof(string));
                            orderRecord.WarningCount,                       //"WarningList", typeof(string));
                            orderRecord.WarningList,                        //"ErrorCount", typeof(string));
                            orderRecord.ErrorCount,                         //"ErrorList", typeof(string));
                            orderRecord.ErrorList,                          //"FatalCount", typeof(string));
                            orderRecord.FatalCount,                         //"FatalList", typeof(string));
                        orderRecord.FatalList            //

                        );

                        orderCount++;
                        fileCount += orderRecord.FileCount;
                        worksheetCount += orderRecord.WorksheetCount;
                        itemCount += orderRecord.Items;
                        quantity += orderRecord.Items;
                        area += orderRecord.Area;
                        weight += orderRecord.Weight;
                        hours += orderRecord.Hours;
                        cost += orderRecord.Cost;
                        amount += orderRecord.Amount;
                        materialCount += orderRecord.Materials;
                        warningCount += orderRecord.WarningCount;
                        errorCount += orderRecord.ErrorCount;
                        //fatalCount += orderRecord.FatalCount;
                        lbInfoOrdersCount.Text = orderCount.ToString();
                        lbInfoFilesCount.Text = fileCount.ToString();
                        lbInfoWorksheetsCount.Text = worksheetCount.ToString();
                        lbInfoItemsCount.Text = itemCount.ToString();
                        lbInfoMaterialCount.Text = materialCount.ToString();
                        lbInfoWarningCount.Text = warningCount.ToString();
                        lbInfoErrorCount.Text = errorCount.ToString();
                        plGridPanel.ResumeLayout(false);
                        plGridPanel.PerformLayout();



                        if (type == 2)
                        {
                            // Fix: Select file names as strings, not as chars
                            var fileNames = order.Files.Select(f => f.FileName).ToList();
                            if (orderRecord.ErrorCount + orderRecord.FatalCount > 0)
                            {
                                _fileService.MoveOrderFiles(fileNames, false);
                            }
                            else
                            {
                                //    _fileService.MoveOrderFiles(fileNames, true);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug("Error adding individual exceExcelOrderDto to data table. Excepton: {$Exceptiom}", ex.Message);
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


        }
    }
}
