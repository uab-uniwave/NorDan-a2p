using System.Data;
using System.Text.Json.Nodes;

using a2p.Application.Interfaces.Services;
using a2p.Application.Models;
using a2p.Domain.Entities;

using ClosedXML.Excel;

namespace a2p.WinForm.Forms
{
    public partial class ChildFormLog : Form
    {

        private readonly ILogger _logger;
        private readonly Color _backColor = Color.FromArgb(56, 57, 60);
        private ISettingsService _settingsService;
        private ProgressValue _progressValue;
        private AppSettings _appSettings;
        private DataTable _dataTableLog;
        private DataTable _dataTableProperties;
        private BindingSource _bindingSourceLog;
        private BindingSource _bindingSourceProperties;
        private SettingsContainer _settingsContainer;

        private string _file;

        public ChildFormLog(ISettingsService settingsService, ILogger logger)
        {
            _logger = logger;
            _dataTableLog = new DataTable();
            _bindingSourceLog = new BindingSource();
            _dataTableProperties = new DataTable();
            _bindingSourceProperties = new BindingSource();
            _progressValue = new ProgressValue();
            _settingsService = settingsService;
            _appSettings = _settingsService.GetAppSettings();
            _settingsContainer = _settingsService.GetSettings();

            _file = Path.Combine(_appSettings.Folders.RootFolder, _appSettings.Folders.Log, "a2pLog.json");

            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent();
            InitializeGrid();
            InitializeTable();
        }
        #region -== Initializations ==-
        private void InitializeGrid()
        {
            try
            {

                dataGridViewLog.CellFormatting += LogGridView_CellFormatting;
                dataGridViewLog.CellClick += LogGridView_CellClick;

                // DataGridViewProperties Columns 
                //=====================================================
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "OrderNumber",
                    DataPropertyName = "OrderNumber",
                    Name = "OrderNumber",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "WorksheetDto",
                    DataPropertyName = "WorksheetDto",
                    Name = "WorksheetDto",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,

                });

                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Reference",
                    DataPropertyName = "Reference",
                    Name = "Reference",
                    ReadOnly = true,
                    Visible = true,

                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Color / Description",
                    DataPropertyName = "Color",
                    Name = "Color",
                    ReadOnly = true,
                    Visible = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable

                });

                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Level",
                    DataPropertyName = "Level",
                    Name = "Level",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    SortMode = DataGridViewColumnSortMode.NotSortable

                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Message",
                    DataPropertyName = "Message",
                    Name = "Message",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    SortMode = DataGridViewColumnSortMode.NotSortable

                });

                //DataGrid Header Style 
                //===================================================================================================================
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

                dataGridViewLog.ColumnHeadersDefaultCellStyle = ColumnHeadersDefaultCellStyle;

                //DataGrid Cell and Alternative Rows default Cells Style 
                //===================================================================================================================
                DataGridViewCellStyle DefaultCellStyle = new()
                {

                    BackColor = Color.FromArgb(56, 57, 60),
                    ForeColor = Color.WhiteSmoke,
                    SelectionBackColor = Color.FromArgb(239, 112, 32),
                    SelectionForeColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 9F),
                    Padding = new Padding(5),
                    WrapMode = DataGridViewTriState.True
                };
                dataGridViewLog.DefaultCellStyle = DefaultCellStyle;

                DataGridViewCellStyle AlternatingRowsDefaultCellStyle = new()
                {
                    BackColor = Color.FromArgb(96, 97, 100),
                    ForeColor = Color.WhiteSmoke,
                    SelectionBackColor = Color.FromArgb(239, 112, 32),
                    SelectionForeColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 9F),
                    Padding = new Padding(5),
                    WrapMode = DataGridViewTriState.True,
                    Alignment = DataGridViewContentAlignment.MiddleLeft

                };
                dataGridViewLog.AlternatingRowsDefaultCellStyle = AlternatingRowsDefaultCellStyle;

                //DataGrid Default cells 
                DataGridViewCellStyle RowsDefaultCellStyle = new()
                {
                    BackColor = Color.FromArgb(56, 57, 60),
                    ForeColor = Color.WhiteSmoke,
                    SelectionBackColor = Color.FromArgb(239, 112, 32),
                    SelectionForeColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 9F),
                    Padding = new Padding(5),
                    WrapMode = DataGridViewTriState.True,
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                };
                dataGridViewLog.RowsDefaultCellStyle = RowsDefaultCellStyle;

                dataGridViewLog.Visible = true;
                dataGridViewLog.Enabled = true;

            }
            catch (Exception ex)
            {
                if (!DesignMode)
                {
                    _logger.LogError("Log form: Unhandled ErrorDto Log Grid View: {$Exception}", ex.Message);
                }
                else
                {
                    _ = MessageBox.Show($"Log form: Design Mode Log Grid View: {ex.Message}");
                }
            }
        }
        private void InitializeTable()
        {
            try
            {
                // DataTable for log entries
                _ = _dataTableLog.Columns.Add("OrderNumber", typeof(string));
                _ = _dataTableLog.Columns.Add("WorksheetDto", typeof(string));
                _ = _dataTableLog.Columns.Add("Reference", typeof(string));
                _ = _dataTableLog.Columns.Add("Color", typeof(string));
                _ = _dataTableLog.Columns.Add("Level", typeof(string));
                _ = _dataTableLog.Columns.Add("Message", typeof(string));
                _bindingSourceLog.DataSource = _dataTableLog;
                dataGridViewLog.DataSource = _bindingSourceLog;

            }

            catch (Exception ex2)
            {
                string className = nameof(ChildFormLog); // Replace with the actual class name if different
                string methodName = nameof(InitializeTable); // Replace with the actual method name if different

                // Log the error
                _logger.LogError("ErrorDto in {Class}.{Method}. Exception {Message}", className, methodName, ex2.Message);

                // Display the error in a MessageBox
                _ = MessageBox.Show($@"ErrorDto in {className}.{methodName}: {ex2.Message}", "ErrorDto", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        #endregion -== Initializations ==-

        #region -== Form Events ==-

        private void LogForm_Load(object sender, EventArgs e) => PerformAutoScale();

        private void LogForm_Shown(object sender, EventArgs e)
        {
            ResumeLayout(false);
            PerformLayout();
        }

        private void LogForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            PerformAutoScale();
            ResumeLayout(false);
            PerformLayout();

        }
        private void LogForm_ResizeBegin(object sender, EventArgs e) => SuspendLayout();

        private void LogForm_ResizeEnd(object sender, EventArgs e)
        {
            PerformAutoScale();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion -== Form Events ==-

        #region -== Grids Events ==- 

        private void dataGridViewLog_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Log any errors that occur during processing
            //_logger.LogError("Log Form: GridViewLog. ErrorDto in column {$Column}, row {$Row}: {$Exception}", e.ColumnIndex, e.RowIndex, e.Exception?.Message ?? "Exception details missing.");
            //e.ThrowException = false;

        }

        private void LogGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Ensure the event is triggered for the correct column
                string columnName = dataGridViewLog.Columns[e.ColumnIndex].Name;

                // Apply styles based on the "Level" column
                if (columnName == "Level" && e.Value != null)
                {
                    if (string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        return;
                    }

                    string cellValue = e.Value.ToString() ?? string.Empty;

                    if (e.CellStyle != null)
                    {

                        // Apply the style if the log level matches
                        if (e.Value.ToString() == "Fatal")
                        {
                            e.CellStyle.ForeColor = Color.DarkRed;
                        }
                        else if (e.Value.ToString() == "ErrorDto")
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else if (e.Value.ToString() == "Warning")
                        {
                            e.CellStyle.ForeColor = Color.Yellow;
                        }

                        else if (e.Value.ToString() == "Warning")
                        {
                            e.CellStyle.ForeColor = Color.LightGreen;

                        }

                        else if (e.Value.ToString() == "Debug(")
                        {
                            e.CellStyle.ForeColor = Color.LightBlue;
                        }
                        else if (e.Value.ToString() == "Verbose")
                        {
                            e.CellStyle.ForeColor = Color.LightGray;
                        }

                    }

                    // Apply wrapping for the "Message" column
                    if (columnName == "Message")
                    {
                        if (e.CellStyle != null)
                        {
                            e.CellStyle.WrapMode = DataGridViewTriState.True;
                        }
                    }
                    else
                    {
                        if (e.CellStyle != null)
                        {
                            e.CellStyle.WrapMode = DataGridViewTriState.False;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during processing
                _logger.LogError(ex.Message, "Log Form: ErrorDto Grid ${RowIndex} cell Formatting. ErrorDto formatting log cell.", e.RowIndex);

            }

        }
        private void LogGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                // Log any errors that occur during processing
                _logger.LogError(ex.Message, "LF: ErrorDto Grid ${RowIndex} cell Click. ErrorDto getting log properties.", e.RowIndex);
            }
        }

        #endregion -== Grids events == -

        #region -== Data Table methods ==-
        public async Task LogoMonitorFileAsync()
        {

            using FileStream fileStream = new(_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new(fileStream);

            while (true)
            {
                string line = await streamReader.ReadLineAsync() ?? string.Empty;
                if (!string.IsNullOrEmpty(line))
                {
                    // Parse the JSON line
                    LogEntity logEntry = await LogParseLineAsync(line);
                    // Add the parsed data to the DataTable
                    await Task.Run(() => LogAddAsync(logEntry));
                }

            }
        }
        private async Task<LogEntity> LogParseLineAsync(string jsonLine)
        {
            try
            {

                using MemoryStream jsonStream = new(System.Text.Encoding.UTF8.GetBytes(jsonLine));
                JsonNode? root = await JsonNode.ParseAsync(jsonStream);

                if (root == null || root["Properties"] is not JsonObject propertiesNode)
                {
                    _logger.LogWarning("LF: Invalid log entry or missing Properties.");
                    return new LogEntity();
                }

                // Convert Properties to a dictionary
                Dictionary<string, object?>? properties = propertiesNode.ToDictionary(
                 kvp => kvp.Key,
                 kvp => kvp.Value?.ToString() as object
                );

                LogEntity logRecord = new()
                {
                    Order = propertiesNode["OrderNumber"]?.ToString() ?? string.Empty,
                    Worksheet = propertiesNode["WorksheetDto"]?.ToString() ?? string.Empty,
                    Reference = propertiesNode["Reference"]?.ToString() ?? string.Empty,
                    Color = propertiesNode["Color"]?.ToString() ?? string.Empty,
                    Level = root["Level"]?.ToString() ?? string.Empty,
                    Message = propertiesNode["RenderedMessage"]?.ToString() ?? string.Empty,

                };

                return properties == null ? logRecord : properties.Count == 0 ? logRecord : logRecord;
            }
            catch (Exception ex)
            {
                _logger.LogError("LF: ErrorDto parsing log entry: {Exception}", ex.Message);
                return new LogEntity();
            }
        }
        private void LogAddAsync(LogEntity logEntry)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => LogAddAsync(logEntry)));
                    return;
                }

                _ = _dataTableLog.Rows.Add(

                 logEntry.Order,
                 logEntry.Worksheet,
                 logEntry.Reference,
                 logEntry.Color,
                 logEntry.Level,
                 logEntry.Message
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("LF: ErrorDto adding log entry to DataTable: {Exception}", ex.Message);
            }
        }
        //public async Task LogRefreshAsync()
        //{
        //    LogClear();

        //    try
        //    {

        //        List<LogEntity> logEntries = await _logger.GetRepository(string.Empty);

        //        if (logEntries != null)
        //        {

        //            // Remove duplicates based on unique properties (e.g., Timestamp, Message, etc.)
        //            List<LogEntity> distinctLogEntries = logEntries
        //             .GroupBy(entry => new
        //             {
        //                 entry.Order,
        //                 entry.WorksheetDto,
        //                 entry.Reference,
        //                 entry.Color,
        //                 entry.Level,
        //                 entry.Message,
        //             })
        //             .Select(group => group.First())
        //             .ToList();

        //            foreach (LogEntity? logEntry in distinctLogEntries)
        //            {

        //                _ = _dataTableLog.Rows.Add(logEntry.Order, logEntry.WorksheetDto, logEntry.Reference, logEntry.Color, logEntry.Level, logEntry.Message);
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Log any errors that occur during processing
        //        _logger.LogError($"LF: ErrorDto refreshing log entries: {ex.Message}");
        //    }

        //}

        //public void LogClear()
        //{
        //    try
        //    {
        //        if (InvokeRequired)
        //        {
        //            LogClear();
        //        }
        //        else
        //        {

        //            if (_dataTableLog.Rows.Count > 0)
        //            {
        //                _dataTableLog.Rows.Clear();
        //                _dataTableProperties.Rows.Clear();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log any errors that occur during processing
        //        _logger.LogError($"LF: ErrorDto clearing log file: {ex.Message}");
        //    }
        //}

        //========================================================
        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string defaultFileName = $"Log_{DateTime.Now:yyyy-MM-dd_HH-mm}.xlsx";
            SaveFileDialog saveLog = new()
            {
                Filter = "Excel file (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                FileName = defaultFileName
            };

            if (saveLog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveLog.FileName;
                using (XLWorkbook workbook = new())
                {
                    DataTable dataTable = ((DataTable)_bindingSourceLog.DataSource).Copy();
                    // After creating the worksheet, set column formatting as requested
                    IXLWorksheet worksheet = workbook.Worksheets.Add(dataTable, "Log Records");

                    // Set word wrap for column 5 (indexing is 1-based in ClosedXML)

                    // Only apply header style to used columns in Row(1)
                    IXLRow HeaderRow = worksheet.Row(1);
                    int usedColumnCount = worksheet.ColumnsUsed().Count();
                    for (int i = 1; i <= usedColumnCount; i++)
                    {
                        IXLCell cell = HeaderRow.Cell(i);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 12;
                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(56, 57, 60);
                        cell.Style.Font.FontColor = XLColor.FromArgb(239, 112, 32);
                    }
                    // Set borders for all used cells to color (239, 112, 32)
                    XLColor borderColor = XLColor.FromArgb(239, 112, 32);
                    foreach (IXLCell? cell in worksheet.CellsUsed())
                    {
                        cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.TopBorderColor = borderColor;
                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.BottomBorderColor = borderColor;
                        cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.LeftBorderColor = borderColor;
                        cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.RightBorderColor = borderColor;
                    }

                    IXLColumn colMessage = worksheet.Column(6);
                    colMessage.Style.Alignment.WrapText = true;

                    // Auto-size all columns
                    for (int i = 1; i <= worksheet.ColumnsUsed().Count(); i++)
                    {
                        worksheet.Column(i).AdjustToContents();
                    }

                    workbook.SaveAs(fileName);
                }
                _logger.LogInformation("Log saved successfully to {FileName}", fileName);
            }
        }

        #endregion -== Contextual Menu ==-
    }
}

