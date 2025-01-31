using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Interfaces.Services.Import;
using a2p.Shared.Core.Interfaces.Services.Other;
using a2p.Shared.Core.Utils;

using Microsoft.Extensions.Configuration;

using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace a2p.WinForm.ChildForms
{
    public partial class LogForm : Form
    {
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly IImportService _importService;
        private readonly IConfiguration _configuration;
        private readonly ILogService _logger;

        private DataTable _dataTableLog;
        private DataTable _dataTableProperties;
        private BindingSource _bindingSourceLog;
        private BindingSource _bindingSourceProperties;
        private string _file;


        public LogForm(IFileService fileService, IExcelService excelService, IImportService importService, IConfiguration configuration, ILogService logger)
        {

            _dataTableLog = new DataTable();
            _dataTableProperties = new DataTable();
            _bindingSourceLog = [];
            _bindingSourceProperties = [];
            _fileService = fileService;
            _excelService = excelService;
            _importService = importService;
            _configuration = configuration;
            _logger = logger;
            _file = Path.Combine(_configuration["AppSettings:Folders:RootFolder"] ?? "C://Temp//Import", _configuration["AppSettings:Folders:Log"] ?? "Log", "a2pLog.json");


            //   this.SuspendLayout();
            try
            {
                // Your code that might throw an exception
            }
            catch (Exception ex2)
            {
                string className = nameof(LogForm); // Replace with the actual class name if different
                string methodName = nameof(InitializeDataTable); // Replace with the actual method name if different

                // Log the error
                _logger?.Error(ex2, "Error in {Class}.{Method} {Message}", className, methodName, ex2.Message);

                // Display the error in a MessageBox
                _ = MessageBox.Show($"Error in {className}.{methodName}: {ex2.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SuspendLayout();
            InitializeComponent();
            InitializeDataGridViews();
            InitializeDataTable();


        }
        #region -== Initializations ==-
        private void InitializeDataGridViews()
        {
            try
            {


                dataGridViewLog.CellFormatting += LogGridView_CellFormatting!;
                dataGridViewLog.CellClick += LogGridView_CellClick!;

                // DataGridViewProperties Columns 
                //=====================================================
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Timestamp",
                    DataPropertyName = "Timestamp",
                    Name = "Timestamp",

                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Level",
                    DataPropertyName = "Level",
                    Name = "Level",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Message",
                    DataPropertyName = "Message",
                    Name = "Message",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Order",
                    DataPropertyName = "Order",
                    Name = "Order",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Worksheet",
                    DataPropertyName = "Worksheet",
                    Name = "Worksheet",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Line",
                    DataPropertyName = "Line",
                    Name = "Line",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Exception",
                    DataPropertyName = "Exception",
                    Name = "Exception",
                    ReadOnly = true,
                    Visible = false,

                });
                _ = dataGridViewLog.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Properties",
                    DataPropertyName = "Properties",
                    Name = "Properties",
                    ReadOnly = true,
                    Visible = false,
                });

                // DataGridViewProperties Columns 
                //=====================================================
                _ = dataGridViewProperties.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Property",
                    DataPropertyName = "Properties",
                    Name = "Properties",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                _ = dataGridViewProperties.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Value",
                    DataPropertyName = "Value",
                    Name = "Value",
                    ReadOnly = true,
                    Visible = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill


                });


                //DataGrid Header Style 
                //=======================================================================================
                DataGridViewCellStyle ColumnHeadersDefaultCellStyle = new()
                {
                    BackColor = UniwaveColors.uwGreyDark,
                    ForeColor = UniwaveColors.uwOrangeDeep,
                    SelectionBackColor = UniwaveColors.uwOrangeDeep,
                    SelectionForeColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                    Padding = new Padding(5),
                    Alignment = DataGridViewContentAlignment.MiddleLeft,

                };

                dataGridViewLog.ColumnHeadersDefaultCellStyle = ColumnHeadersDefaultCellStyle;
                dataGridViewProperties.ColumnHeadersDefaultCellStyle = ColumnHeadersDefaultCellStyle;

                //DataGrid Cell and Alternative Rows default Cells Style 
                //=======================================================================================
                DataGridViewCellStyle DefaultCellStyle = new()
                {

                    BackColor = UniwaveColors.uwGreyDark,
                    ForeColor = Color.WhiteSmoke,
                    SelectionBackColor = UniwaveColors.uwOrangeDeep,
                    SelectionForeColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 9F),
                    Padding = new Padding(5),
                    WrapMode = DataGridViewTriState.True
                };
                dataGridViewLog.DefaultCellStyle = DefaultCellStyle;
                dataGridViewProperties.DefaultCellStyle = DefaultCellStyle;

                DataGridViewCellStyle AlternatingRowsDefaultCellStyle = new()
                {
                    BackColor = UniwaveColors.a2pGreyDeep,
                    ForeColor = Color.WhiteSmoke,
                    SelectionBackColor = UniwaveColors.uwOrangeDeep,
                    SelectionForeColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 9F),
                    Padding = new Padding(5),
                    WrapMode = DataGridViewTriState.True,
                    Alignment = DataGridViewContentAlignment.MiddleLeft

                };
                dataGridViewLog.AlternatingRowsDefaultCellStyle = AlternatingRowsDefaultCellStyle;
                dataGridViewProperties.AlternatingRowsDefaultCellStyle = AlternatingRowsDefaultCellStyle;

                //DataGrid Default cells 
                DataGridViewCellStyle RowsDefaultCellStyle = new()
                {
                    BackColor = UniwaveColors.uwGreyDark,
                    ForeColor = Color.WhiteSmoke,
                    SelectionBackColor = UniwaveColors.uwOrangeDeep,
                    SelectionForeColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 9F),
                    Padding = new Padding(5),
                    WrapMode = DataGridViewTriState.True,
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                };
                dataGridViewLog.RowsDefaultCellStyle = ColumnHeadersDefaultCellStyle;
                dataGridViewProperties.RowsDefaultCellStyle = ColumnHeadersDefaultCellStyle;

                dataGridViewLog.Visible = true;
                dataGridViewProperties.Visible = true;
                dataGridViewLog.Enabled = true;
                dataGridViewProperties.Enabled = true;

            }
            catch (Exception ex)
            {
                if (!DesignMode)
                {
                    _logger?.Error("LF: Unhandled Error Log Grid View: {$Exception}", ex.Message);
                }
                else
                {
                    _ = MessageBox.Show($"LF: Design Mode Log Grid View: {ex.Message}");
                }
            }
        }
        private void InitializeDataTable()
        {
            try
            {
                // DataTable for log entries
                _ = _dataTableLog.Columns.Add("Timestamp", typeof(string));
                _ = _dataTableLog.Columns.Add("Level", typeof(string));
                _ = _dataTableLog.Columns.Add("Message", typeof(string));
                _ = _dataTableLog.Columns.Add("Order", typeof(string));
                _ = _dataTableLog.Columns.Add("Worksheet", typeof(string));
                _ = _dataTableLog.Columns.Add("Line", typeof(string));
                _ = _dataTableLog.Columns.Add("Exception", typeof(string));
                _ = _dataTableLog.Columns.Add("Properties", typeof(Dictionary<string, object>));
                _bindingSourceLog.DataSource = _dataTableLog;
                dataGridViewLog.DataSource = _bindingSourceLog;

                // DataTable for log properties 
                _ = _dataTableProperties.Columns.Add("Properties", typeof(string));
                _ = _dataTableProperties.Columns.Add("Value", typeof(string));
                _bindingSourceProperties.DataSource = _dataTableProperties;
                dataGridViewProperties.DataSource = _bindingSourceProperties;
            }
            catch (Exception ex)
            {
                if (!DesignMode)
                {
                    _logger?.Error("LF: Unhandled Error Initializing Log data table LogForm: {$Exception}", ex.Message);
                }
                else { }
                try
                {
                    // Your code that might throw an exception
                }
                catch (Exception ex2)
                {
                    string className = nameof(LogForm); // Replace with the actual class name if different
                    string methodName = nameof(InitializeDataTable); // Replace with the actual method name if different

                    // Log the error
                    this._logger?.Error(ex2, "Error in {Class}.{Method} {Message}", className, methodName, ex2.Message);

                    // Display the error in a MessageBox
                    _ = MessageBox.Show($"Error in {className}.{methodName}: {ex2.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }





        #endregion -== Initializations ==-

        #region -== Form Events ==-

        private void LogForm_Shown(object sender, EventArgs e)
        {
            this.ResumeLayout(true);
        }

        private void LogForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();
        }
        private void LogForm_ResizeBegin(object sender, EventArgs e)
        {
            this.SuspendLayout();
        }

        private void LogForm_ResizeEnd(object sender, EventArgs e)
        {
            this.ResumeLayout(true);
        }

        #endregion -== Form Events ==-

        #region -== Grids Events

        private void dataGridViewLog_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Log any errors that occur during processing
            _logger.Error("LF:GridViewLog. Error in column {$Column}, row {$Row}: {$Exception}", e.ColumnIndex, e.RowIndex, e.Exception?.Message ?? "Exception details missing.");
            Console.WriteLine($"LF:GridViewLog. Error in column {e.ColumnIndex}, row {e.RowIndex}: {e.Exception?.Message ?? "Exception details missing."}");
            e.ThrowException = false;

        }

        private void dataGridViewProperties_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Log any errors that occur during processing
            _logger.Error("LF:GridViewProperties. Error in column {$Column}, row {$Row}: {$Exception}", e.ColumnIndex, e.RowIndex, e.Exception?.Message ?? "Exception details missing.");
            Console.WriteLine($"LF:GridViewProperties. Error in column {e.ColumnIndex}, row {e.RowIndex}: {e.Exception?.Message ?? "Exception details missing."}");
            e.ThrowException = false;

        }





        private void LogGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

                //  Use a dictionary to map log levels to styles for better maintainability
                Dictionary<string, (Color ForeColor, Color BackColor)> logLevelStyles = new()
                   {
                     { "Verbose", (System.Drawing.Color.LightGray,  UniwaveColors.uwGreyDark) },
                     { "Debug", (System.Drawing.Color.LightBlue,   UniwaveColors.uwGreyDark ) },
                     { "Information", (System.Drawing.Color.LightGreen,  UniwaveColors.uwGreyDark) },
                     { "Warning", (System.Drawing.Color.Yellow,  UniwaveColors.uwGreyDark)},
                     { "Error", (System.Drawing.Color.Red,  UniwaveColors.uwGreyDark)},
                     { "Fatal", (System.Drawing.Color.DarkRed,  UniwaveColors.uwGreyDark)}
                   };

                // Apply the style if the log level matches
                if (logLevelStyles.TryGetValue(cellValue, out (Color ForeColor, Color BackColor) style))
                {
                    if (e.CellStyle != null)
                    {
                        e.CellStyle.BackColor = style.BackColor;
                        e.CellStyle.ForeColor = style.ForeColor;
                    }

                }
                else
                {
                    if (e.CellStyle != null)
                    {
                        e.CellStyle.BackColor = UniwaveColors.uwGreyLight;
                        e.CellStyle.ForeColor = UniwaveColors.a2pGreyDark;
                    }
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

            // Format the "Timestamp" column
            if (columnName == "Timestamp" && e.Value != null)
            {
                if (DateTime.TryParse(e.Value.ToString(), out DateTime dateValue))
                {
                    e.Value = dateValue.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                    e.FormattingApplied = true;
                }
            }
        }
        private async void LogGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridViewLog.Rows.Count)
                {
                    await PropertiesAddAsync(e.RowIndex);
                }

            }
            catch (Exception ex)
            {
                // Log any errors that occur during processing
                _logger.Error(ex.Message, "LF: Error Grid ${RowIndex} cell Click. Error getting log properties.", e.RowIndex);
            }
        }
        #endregion -== IGrids events

        #region -== IData Table methods ==-
        public async void LogoMonitorFileAsync()
        {

            using FileStream fileStream = new(_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new(fileStream);

            while (true)
            {
                string line = await streamReader.ReadLineAsync() ?? string.Empty;
                if (!string.IsNullOrEmpty(line))
                {
                    // Parse the JSON line
                    A2PLogGridRecord logEntry = await LogParseLineAsync(line);
                    // Add the parsed data to the DataTable
                    await Task.Run(() => LogAddAsync(logEntry));
                }


            }
        }
        private async Task<A2PLogGridRecord> LogParseLineAsync(string jsonLine)
        {
            try
            {
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip
                };

                using MemoryStream jsonStream = new(System.Text.Encoding.UTF8.GetBytes(jsonLine));
                JsonNode? root = await JsonNode.ParseAsync(jsonStream);

                if (root == null || root["Properties"] is not JsonObject propertiesNode)
                {
                    _logger.Warning("LF: Invalid log entry or missing Properties.");
                    return new A2PLogGridRecord();
                }

                // Convert Properties to a dictionary
                Dictionary<string, object?>? properties = propertiesNode.ToDictionary(
                 kvp => kvp.Key,
                 kvp => kvp.Value?.ToString() as object
                );


                A2PLogGridRecord logRecord = new()
                {
                    Timestamp = root["Timestamp"]?.ToString() ?? string.Empty,
                    Level = root["Level"]?.ToString() ?? string.Empty,
                    Message = propertiesNode["RenderedMessage"]?.ToString() ?? string.Empty,
                    Order = propertiesNode["Order"]?.ToString() ?? string.Empty,
                    Worksheet = propertiesNode["Worksheet"]?.ToString() ?? string.Empty,
                    Line = propertiesNode["Line"]?.ToString() ?? string.Empty,
                    Exception = propertiesNode["Exception"]?.ToString() ?? string.Empty

                };





                if (properties != null && properties.Count > 0)
                {
                    logRecord.Properties = properties ?? [];
                }

                return logRecord;

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "LF: Error parsing log entry: {ex.Message}");
                return new A2PLogGridRecord();
            }
        }
        private void LogAddAsync(A2PLogGridRecord logEntry)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => LogAddAsync(logEntry)));
                    return;
                }

                _ = _dataTableLog.Rows.Add(
                 logEntry.Timestamp,
                 logEntry.Level,
                 logEntry.Message,
                 logEntry.Order,
                 logEntry.Worksheet,
                 logEntry.Line,
                 logEntry.Exception,
                 logEntry.Properties // Directly store the dictionary
                );
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"LF: Error adding log entry to DataTable: {ex.Message}");
            }
        }
        public async Task LogRefreshAsync()
        {
            try
            {


                List<A2PLogGridRecord> logEntries = await _logger.GetRepository(string.Empty);

                if (logEntries != null)
                {
                    // Remove duplicates based on unique properties (e.g., Timestamp, Message, etc.)
                    List<A2PLogGridRecord> distinctLogEntries = logEntries
                     .GroupBy(entry => new
                     {
                         entry.Timestamp,
                         entry.Level,
                         entry.Message,
                         entry.Order,
                         entry.Worksheet,
                         entry.Line,
                         entry.Exception,
                         entry.Properties

                     })
                     .Select(group => group.First())
                     .ToList();

                    // Bind the distinct entries to the grid


                    foreach (A2PLogGridRecord? logEntry in distinctLogEntries)
                    {
                        _ = _dataTableLog.Rows.Add(logEntry.Timestamp, logEntry.Level, logEntry.Message, logEntry.Order, logEntry.Worksheet, logEntry.Line, logEntry.Exception, logEntry.Properties);
                    }

                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during processing
                _logger.Error(ex, $"LF: Error refreshing log entries: {ex.Message}");
            }


        }
        public async Task LogClearAsync()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(async () => await LogClearAsync()));
                }
                else
                {
                    await Task.Run(_dataTableLog.Rows.Clear);
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during processing
                _logger.Error($"LF: Error clearing log file: {ex.Message}");
            }
        }

        //========================================================
        private async Task PropertiesAddAsync(int index)
        {
            try
            {
                // Ensure the index is within bounds
                if (index < 0 || index >= dataGridViewLog.Rows.Count)
                {
                    _logger.Warning("LF: Row index out of range. Can't get log properties.");
                    return;
                }

                // Get the selected row
                DataRow selectedRow = _dataTableLog.Rows[index];

                // Get the dictionary from the "Properties" column
                if (selectedRow["Properties"] is not Dictionary<string, object> properties || properties.Count == 0)
                {
                    _logger.Warning("LF: Selected row has no valid properties.");
                    return;
                }

                // Clear the previous properties
                _dataTableProperties.Rows.Clear();

                // Add each property to the _dataTableProperties
                foreach (KeyValuePair<string, object> property in properties)
                {
                    _ = await Task.Run(() => _dataTableProperties.Rows.Add(property.Key, property.Value?.ToString()));
                }


            }
            catch (Exception ex)
            {
                _logger.Error($"LF: Error processing log entry properties: {ex.Message}");
            }

        }

        #endregion -== Data Table methods ==-

        #region -== Log Level Filters ==-

        private void chxLogLevelWarning_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();

        }

        private void chxLogLevelVerbose_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void chxLogLevelDebug_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void chxLogLevelInfo_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void chxLogLevelError_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void chxLogLevelFatal_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }



        private void ApplyFilter()
        {
            List<string> selectedLevels = [];

            if (chxVerbose.Checked)
            {
                selectedLevels.Add("'Verbose'");
            }

            if (chxDebug.Checked)
            {
                selectedLevels.Add("'Debug'");
            }

            if (chxInformation.Checked)
            {
                selectedLevels.Add("'Information'");
            }

            if (chxWarning.Checked)
            {
                selectedLevels.Add("'Warning'");
            }

            if (chxError.Checked)
            {
                selectedLevels.Add("'Error'");
            }

            if (chxFatal.Checked)
            {
                selectedLevels.Add("'Fatal'");
            }

            if (selectedLevels.Count > 0)
            {
                string filterExpression = $"Level IN ({string.Join(",", selectedLevels)})";
                _bindingSourceLog.Filter = filterExpression;

            }
            else
            {
                _bindingSourceLog.RemoveFilter(); // Show all data if no checkboxes are selected

            }
        }
        #endregion -== Log Level Filters ==-


        private void plLogToolBarPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


