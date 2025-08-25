using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Models;
using a2p.Shared.Core.DTO.a2p.Shared.Core.DTO;
using a2p.Shared.Infrastructure.Interfaces;

using System.Data;

namespace a2p.WinForm.ChildForms
{
    public partial class OrdersForm : Form
    {
        private readonly IUserSettingsService _userSettingsService;
        private SettingsContainer _settingsContainer;
        private AppSettings _appSettings;
        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly ISQLRepository _sqlRepository;
        private readonly IReadService _readService;
        private readonly IWriteService _writeService;
        private List<A2POrder> _a2pOrders;

        private DataTable _dataTable;
        private BindingSource _bindingSource;
        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        public OrdersForm(IUserSettingsService userSettingsService,
                          ILogService logService,
                          IFileService fileService,
                          IExcelService excelService,
                          ISQLRepository sqlRepository,
                          IReadService readService,
                          IWriteService writeService,
                          DataCache dataCache)
        {

            _userSettingsService = userSettingsService;
            _appSettings = _userSettingsService.LoadSettings();
            _settingsContainer = _userSettingsService.LoadAllSettings();
            _fileService = fileService;
            _logService = logService;
            _excelService = excelService;
            _writeService = writeService;
            _readService = readService;
            _sqlRepository = sqlRepository;

            _a2pOrders = [];
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
                _logService.Error("Order Form: Unhandled Error initializing grid view. Exception: {$Exception}.", ex.Message);
            }

        }
        private void InitializeTable()
        {

            try
            {
                dataGridViewFiles.SuspendLayout();
                _ = _dataTable.Columns.Add("Image", typeof(Image));
                _ = _dataTable.Columns.Add("Order", typeof(string));
                _ = _dataTable.Columns.Add("SalesDocument", typeof(string));
                _ = _dataTable.Columns.Add("Items", typeof(int));
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
                _logService.Error("Order Form: Unhandled Error initializing data table. Exception {$Exception}.", ex.Message);

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
            _logService.Error("FF:GridViewFiles. Error in column {$Column}, row {$Row}: {$Exception}", e.ColumnIndex, e.RowIndex, e.Exception?.Message ?? "Exception details missing.");
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

                if (dataGridViewFiles.Columns["Items"] != null && e.ColumnIndex == dataGridViewFiles.Columns["Items"].Index && e.Value != null)
                {
                    string? itemList = dataGridViewFiles.Rows[e.RowIndex].Cells["ItemList"].Value.ToString();
                    dataGridViewFiles.Rows[e.RowIndex].Cells["Items"].ToolTipText = itemList;
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
                _logService.Error("Order Form: Unhandled Error formatting cells!");
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
                    string? orderNumber = dataGridViewFiles.Rows[rowIndex].Cells["Order"].Value.ToString();
                    if (orderNumber == null)
                    {
                        continue;
                    }

                    for (int j = 0; j < _a2pOrders.Count; j++)
                    {
                        if (_a2pOrders[j].Order == orderNumber)
                        {
                            rowIndex = j;

                            int readFatal = CountReadFatal(_a2pOrders[j]);
                            int readError = CountReadError(_a2pOrders[j]);

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

                _logService.Error("Order Form: Unhandled error setting grid row readonly. Exception: {$Exception}.", ex.Message);
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
                using ProgressBarForm progressBarForm = new()
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
                List<A2POrder> a2pOrders = await _readService.ReadAsync(_progressValue, _progress);
                _progress?.Report(_progressValue);

                if (a2pOrders == null || a2pOrders.Count == 0)
                {
                    return;

                }
                _a2pOrders = a2pOrders;

                //Populate DataTable
                //=====================================================================================================
                await UpdateDatable(a2pOrders, 1);
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
                _logService.Error("Order Form: Unhandled Error loading Orders. Exception: {$Exception}.", ex.Message);
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
            List<A2POrder> importOrders = [];

            try
            {

                Dictionary<string, int> keyValuePairs = [];
                for (int i = 0; i < dataGridViewFiles.Rows.Count; i++)
                {
                    if ((bool)dataGridViewFiles.Rows[i].Cells["Import"].Value)
                    {

                        for (int j = 0; j < _a2pOrders.Count; j++)
                        {

                            if (_a2pOrders[j].Order == dataGridViewFiles.Rows[i].Cells["Order"].Value.ToString())
                            {
                                _a2pOrders[j].Import = true;

                                importOrders.Add(_a2pOrders[j]);

                                if (CountReadExistsError(_a2pOrders[j]) > 0)
                                {
                                    keyValuePairs.Add(_a2pOrders[j].Order, i);
                                }

                                break;

                            }

                        }

                    }

                }

                bool cancel = OrdersToOverwrite(keyValuePairs);
                if (cancel)
                {

                    foreach (KeyValuePair<string, int> keyValuePair in keyValuePairs)
                    {
                        dataGridViewFiles.Rows[keyValuePair.Value].Cells["Import"].Value = false;
                    }

                }
                else
                {
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
                                Location.X + ((Width - progressBarForm.Width) / 2),
                                Location.Y + ((Height - progressBarForm.Height) / 2)
                                );
                            progressBarForm.progressBar.Style = ProgressBarStyle.Continuous;
                            progressBarForm.progressBar.ForeColor = Color.FromArgb(239, 112, 32);
                        };

                        ProgressValue progressvalue = new ProgressValue();
                        Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);

                        int totalItems = importOrders.Sum(order => order.Items.Count);
                        int totalMaterials = importOrders.Sum(order => order.Materials.Count);
                        int totalOrders = importOrders.Count;

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




                        //20pts x1   single per import- order Form Preparing Import 


                        //30 pts x 1  per Order - Write Service  Deletig existinfg data

                        //100  x1 per Order - PrefsuiteService  Save Doc
                        //30 pts x 1  per Order - Write Service  inserting material needa
                        //100  x1 per Order - PrefsuiteService  Load Doc
                        //100  x1 per Item  - PrefsuiteService  Insert Item

                        //10pts. x 1 per  Item -  Write service -   Inserting data into DB
                        //1 pts x per material   Write service 







                        //20pts x1   single per import- order Form finishing

                        _progressValue.CurrentValue = _progressValue.CurrentValue + 30; //x20 / x1  
                        _progressValue.ProgressTitle = "Importing orders ... ";
                        _progressValue.ProgressTask1 = $"Orders Count {totalOrders} pending import";
                        _progressValue.ProgressTask2 = $"Items Count {totalItems} pending import";
                        _progressValue.ProgressTask3 = $"Orders Count {totalMaterials} pending import";
                        _progress?.Report(_progressValue);
                        progressBarForm.Show();


                        _progress?.Report(_progressValue);
                        _progressValue.ProgressTask1 = string.Empty;
                        _progressValue.ProgressTask2 = string.Empty;
                        _progressValue.ProgressTask3 = string.Empty;

                        for (int i = 0; i < importOrders.Count; i++)
                        {

                            _progressValue.ProgressTask1 = $"Inserting Order {i + 1} of {importOrders.Count} - Order # {importOrders[i].Order} ({importOrders[i].SalesDocumentNumber}/{importOrders[i].SalesDocumentVersion}...";
                            _progressValue.ProgressTask3 = string.Empty;
                            progressBarForm.Show();
                            var Result = await _writeService.WriteAsync(importOrders[i], _progressValue, _progress);


                            var a2POrder = Result.Item1;
                            _progressValue = Result.Item2;


                            importOrders[i] = a2POrder;

                        }

                        await UpdateDatable(importOrders, 2);





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
                        _logService.Error("Order Form: Unhandled error loading importing orders. Exception: {$Exception}.", ex.Message);
                        _ = MessageBox.Show($"An Error occurred while loading the files." +
                            $"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        dataGridViewFiles.ResumeLayout(false);
                    }
                }
            }
            catch (Exception ex)
            {

                _logService.Error("Order Form: Unhandled error setting Importing data. Exception: {$Exception}.", ex.Message);

            }
        }
        public async Task<OrderDTO> MapToReadOrderDTOAsync(A2POrder a2pOrder, int type)   // type 1 - read; 2 - write 
        {
            try
            {

                Image a = imageList1.Images[0];
                OrderDTO orderDTO = new();
                int warningCount = 0;
                int errorCount = 0;
                int fatalCount = 0;

                await Task.Run(() =>
                {
                    orderDTO.Order = a2pOrder.Order;
                    orderDTO.SalesDocument = $"{a2pOrder.SalesDocumentNumber}/{a2pOrder.SalesDocumentVersion}";
                    orderDTO.Items = a2pOrder.Items.Count; // Added line to count Items
                    orderDTO.ItemList = string.Join("\n", a2pOrder.Items.Select(item => item.Item));
                    orderDTO.Quantity = a2pOrder.Items.ToArray().Sum(item => item.Quantity);
                    orderDTO.Area = a2pOrder.Items.ToArray().Sum(item => item.TotalArea);
                    orderDTO.Weight = a2pOrder.Items.ToArray().Sum(item => item.TotalWeight);
                    orderDTO.Hours = a2pOrder.Items.ToArray().Sum(item => item.TotalHours);
                    orderDTO.Cost = a2pOrder.Items.ToArray().Sum(item => item.TotalCost);
                    orderDTO.Amount = a2pOrder.Items.ToArray().Sum(item => item.TotalPrice);
                    orderDTO.Currency = a2pOrder.Files
                        .SelectMany(file => file.Worksheets)
                        .FirstOrDefault(worksheet => !string.IsNullOrEmpty(worksheet.Currency))?.Currency ?? string.Empty;
                    orderDTO.FileCount = a2pOrder.Files.Count;
                    orderDTO.FileList = string.Join("\n", a2pOrder.Files.Select(file => file.FileName));
                    orderDTO.WorksheetCount = a2pOrder.Files.Sum(file => file.Worksheets?.Count ?? 0);
                    orderDTO.WorksheetList = string.Join("\n", a2pOrder.Files.SelectMany(file => file.Worksheets).Select(ws => ws.Name));
                    orderDTO.Materials = a2pOrder.Materials.Count; // Added line to count Materials

                    if (type == 1)
                    {
                        warningCount = a2pOrder.ErrorsRead
                            .Where(error => error.Level is ErrorLevel.Warning)
                            .Where(error => (int)error.Code < 3000)
                            .Select(error => new { error.Level, error.Code, error.Message })
                            .Distinct()
                            .Count();

                        orderDTO.WarningCount = warningCount;
                        orderDTO.WarningList = string.Join("\n", a2pOrder.ErrorsRead
                                    .Where(error => error.Level is ErrorLevel.Warning)
                                    .Select(error => $"ErrorLevel: {error.Level}, ErrorCode: {error.Code}, Message: {error.Message}")
                                    .Distinct());

                        errorCount = a2pOrder.ErrorsRead
                            .Where(error => error.Level is ErrorLevel.Error)
                            .Select(error => new { error.Level, error.Code, error.Message })
                            .Distinct()
                            .Count();


                        orderDTO.ErrorCount = errorCount;

                        orderDTO.ErrorList = string.Join("\n", a2pOrder.ErrorsRead
                            .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                            .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                            .Distinct());

                        fatalCount = a2pOrder.ErrorsRead
                        .Where(error => error.Level is ErrorLevel.Fatal)
                        .Select(error => new { error.Level, error.Code, error.Message })
                        .Distinct()
                        .Count();

                        orderDTO.FatalCount = fatalCount;
                        orderDTO.FatalList = string.Join("\n", a2pOrder.ErrorsRead
                            .Where(error => error.Level is ErrorLevel.Fatal)
                            .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                            .Distinct());

                    }


                    if (type == 2)
                    {


                        warningCount = a2pOrder.ErrorsRead
                             .Where(error => error.Level is ErrorLevel.Warning)
                             .Where(error => (int)error.Code > 3000)
                             .Select(error => new { error.Level, error.Code, error.Message })

                             .Distinct()
                             .Count();
                        orderDTO.WarningCount = warningCount;
                        orderDTO.WarningList = string.Join("\n", a2pOrder.ErrorsRead
                                    .Where(error => error.Level is ErrorLevel.Warning)
                                    .Select(error => $"ErrorLevel: {error.Level}, ErrorCode: {error.Code}, Message: {error.Message}")
                                    .Distinct());

                        errorCount = a2pOrder.ErrorsRead
                            .Where(error => error.Level is ErrorLevel.Error)
                            .Select(error => new { error.Level, error.Code, error.Message })
                            .Distinct()
                            .Count()
                        + a2pOrder.ErrorsWrite
                            .Where(error => error.Level is ErrorLevel.Error)
                            .Select(error => new { error.Level, error.Code, error.Message })
                            .Distinct()
                            .Count();

                        orderDTO.ErrorCount = errorCount;

                        orderDTO.ErrorList = string.Join("\n", a2pOrder.ErrorsRead
                            .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                            .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                            .Distinct()) +

                        string.Join("\n", a2pOrder.ErrorsWrite
                            .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                            .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                            .Distinct());

                        fatalCount = a2pOrder.ErrorsRead
                        .Where(error => error.Level is ErrorLevel.Fatal)
                        .Select(error => new { error.Level, error.Code, error.Message })
                        .Distinct()
                        .Count() + a2pOrder.ErrorsWrite
                        .Where(error => error.Level is ErrorLevel.Fatal)
                        .Select(error => new { error.Level, error.Code, error.Message })
                        .Distinct()
                        .Count();

                        orderDTO.FatalCount = fatalCount;
                        orderDTO.FatalList = string.Join("\n", a2pOrder.ErrorsRead
                            .Where(error => error.Level is ErrorLevel.Fatal)
                            .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")
                            .Distinct()) +
                                            string.Join("\n", a2pOrder.ErrorsWrite
                                                   .Where(error => error.Level is ErrorLevel.Fatal)
                                                   .Select(error => $"Level: {error.Level}, Code: {(int)error.Code}, Message: {error.Message}")

                                                   .Distinct());



                    }
                    orderDTO.Import = CountReadTotalError(a2pOrder) <= 0;

                });





                return orderDTO;

            }
            catch (Exception ex)
            {
                _logService.Error("Order Form: Unhandled error mapping OrderDTO for grid : {$Exception}.", ex.Message);
                return new OrderDTO();
            }
        }

        //===============================================================
        // -= Read Errors =-
        //===============================================================
        private int CountReadWarning(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsRead.Count(error => error.Level == ErrorLevel.Warning);
        }

        private int CountReadError(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsRead.Count(error => error.Level == ErrorLevel.Error);
        }

        private int CountReadFatal(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsRead.Count(error => error.Level == ErrorLevel.Fatal);
        }

        private int CountReadExistsError(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsRead.Count(error => error.Code == ErrorCode.DatabaseRead_OrderAlreadyImported);
        }

        private int CountReadTotalError(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsRead.Count(error => error.Level is ErrorLevel.Warning or ErrorLevel.Error or ErrorLevel.Fatal);
        }

        private int CountWriteFatal(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsWrite.Count(error => error.Level == ErrorLevel.Fatal);
        }

        private int CountWriteError(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsWrite.Count(error => error.Level == ErrorLevel.Error);
        }

        private int CountWriteWarning(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsWrite.Count(error => error.Level == ErrorLevel.Warning);
        }

        private int CountWriteTotalError(A2POrder a2pOrder)
        {
            return a2pOrder.ErrorsWrite.Count(error => error.Level is ErrorLevel.Warning or ErrorLevel.Error or ErrorLevel.Fatal);
        }

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


        private async Task UpdateDatable(List<A2POrder> a2pOrders, int type)
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
            int fatalCount = 0;
            foreach (A2POrder a2pOrder in a2pOrders)
            {

                if (a2pOrder.Files.SelectMany(f => f.Worksheets).Count(w => w.WorksheetType == WorksheetType.Items) == 0)
                {

                    _logService.Warning("Found order {$Order}, files, but items worksheet  is missing", a2pOrder.Order);
                    continue;

                }

                if (type == 1)
                {
                    dataGridViewFiles.Columns["Import"].Visible = true;
                }
                if (type == 2)
                {
                    if (a2pOrder.Import == false)
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

                    OrderDTO orderDTO = await MapToReadOrderDTOAsync(a2pOrder, type);   // 2 means import and should be used write errors

                    Image image;

                    if (orderDTO.FatalCount > 0)
                    {
                        orderDTO.Import = false;
                        image = imageList1.Images[3];

                    }

                    else if (orderDTO.ErrorCount > 0)
                    {
                        orderDTO.Import = false;
                        image = imageList1.Images[2];

                    }

                    else if (orderDTO.WarningCount > 0)
                    {
                        orderDTO.Import = false;
                        image = imageList1.Images[1];

                    }
                    else
                    {
                        orderDTO.Import = true;
                        image = imageList1.Images[0];
                    }

                    _ = _dataTable.Rows.Add
                    (

                        image,                         //"Image", typeof(Image));
                        orderDTO.Order,                       //"Order", typeof(string));
                        orderDTO.SalesDocument,                      //"SalesDocument", typeof(string));
                        orderDTO.Items,                              //"Items", typeof(int));
                        orderDTO.ItemList,                           //"ItemList", typeof(string));
                        orderDTO.Quantity,                           //"Quantity", typeof(int));
                        Math.Round(orderDTO.Area, 2),    //"Area", typeof(string));
                        Math.Round(orderDTO.Weight, 2),  //"Weight", typeof(string));
                        Math.Round(orderDTO.Hours, 2),   //"Hours", typeof(string));
                        Math.Round(orderDTO.Cost, 2),     //"Cost", typeof(string));
                        Math.Round(orderDTO.Amount, 2),   //"Amount", typeof(string));
                        orderDTO.Currency,                           //"Currency", typeof(string));
                        orderDTO.FileCount,                          //"FileList", typeof(string));
                        orderDTO.FileList,                           //"WorksheetCount", typeof(int));
                        orderDTO.WorksheetCount,                     //"WorksheetList", typeof(string));
                        orderDTO.WorksheetList,                      //"Materials", typeof(int));
                        orderDTO.Materials,                          //"Import", typeof(bool));
                        orderDTO.Import,                             //"WarningCount", typeof(string));
                        orderDTO.WarningCount,                       //"WarningList", typeof(string));
                        orderDTO.WarningList,                        //"ErrorCount", typeof(string));
                        orderDTO.ErrorCount,                         //"ErrorList", typeof(string));
                        orderDTO.ErrorList,                          //"FatalCount", typeof(string));
                        orderDTO.FatalCount,                         //"FatalList", typeof(string));
                    orderDTO.FatalList            //

                    );

                    orderCount++;
                    fileCount += orderDTO.FileCount;
                    worksheetCount += orderDTO.WorksheetCount;
                    itemCount += orderDTO.Items;
                    quantity += orderDTO.Items;
                    area += orderDTO.Area;
                    weight += orderDTO.Weight;
                    hours += orderDTO.Hours;
                    cost += orderDTO.Cost;
                    amount += orderDTO.Amount;
                    materialCount += orderDTO.Materials;
                    warningCount += orderDTO.WarningCount;
                    errorCount += orderDTO.ErrorCount;
                    fatalCount += orderDTO.FatalCount;
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
                        var fileNames = a2pOrder.Files.Select(f => f.File).ToList();
                        if (orderDTO.ErrorCount + orderDTO.FatalCount > 0)
                        {
                            _fileService.MoveOrderFiles(fileNames, false);
                        }
                        else
                        {
                            _fileService.MoveOrderFiles(fileNames, true);
                        }
                    }



                }
                catch (Exception ex)
                {
                    _logService.Debug("Error adding individual a2pOrder to data table. Excepton: {$Exceptiom}", ex.Message);
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
