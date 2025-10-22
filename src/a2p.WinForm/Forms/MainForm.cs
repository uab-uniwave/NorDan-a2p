using Application.Interfaces.Excel;
using Application.Interfaces.Files;
using Application.Interfaces.Orchestrators;
using Application.Interfaces.Services;
using Application.Models;

using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace WinFormApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly Color selectedBackgroundColor = Color.FromArgb(239, 112, 32);
        private readonly Color borderColor = Color.White;
        private const int borderWidth = 1;
        private Button? selectedButton;

        private readonly IReadService _readService;
        private readonly IWriteService _writeService;
        private readonly IExcelService _excelService;
        private readonly ISettingsService _settingsService;
        private readonly ILogger<MainForm> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IFileService _fileService;

        private readonly ProgressValue _progressValue = new();
        private readonly IProgress<ProgressValue> _progress;
        private readonly ToolTip _toolTip = new()
        {
            AutoPopDelay = 5000,
            InitialDelay = 500,
            ReshowDelay = 100,
            ShowAlways = true
        };

        private readonly OrdersForm _orderForm;
        private readonly LogsForm _logForm;
        private readonly SettingsForm _settingForm;
        private readonly SettingsContainer _settingsContainer;
        private readonly AppSettings _appSettings;

        #region -== P/Invoke Declarations ==-

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        #endregion

        private bool _isResizing;

        public MainForm(IReadService readService,
                       IExcelService excelService,
                       ILoggerFactory loggerFactory, // changed: accept ILoggerFactory
                       IFileService fileService,
                       ISettingsService settingsService,
                       IWriteService writeService)
        {
            _readService = readService ?? throw new ArgumentNullException(nameof(readService));
            _writeService = writeService ?? throw new ArgumentNullException(nameof(writeService));
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = _loggerFactory.CreateLogger<MainForm>(); // create MainForm logger
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            _appSettings = _settingsService.GetAppSettings();
            _settingsContainer = _settingsService.GetSettings();
            _progress = new Progress<ProgressValue>();

            // Initialize child forms with correct logger types
            _orderForm = new OrdersForm(
                _settingsService,
                _loggerFactory.CreateLogger<OrdersForm>(),
                _fileService,
                _excelService,
                _readService,
                _writeService);

            _logForm = new LogsForm(
                _settingsService,
                _loggerFactory.CreateLogger<LogsForm>());

            _settingForm = new SettingsForm(
                _loggerFactory.CreateLogger<SettingsForm>(),
                _settingsService);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.SuspendLayout();
            InitializeComponent();
            SetupButtons();
            InitializeToolTips();
            this.ResumeLayout(true);

            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;

            slbDataSourceValue.Text = $"{_settingForm.ExtractValueFromConnectionString("Data Source")}\\{_settingForm.ExtractValueFromConnectionString("Initial Catalog")}";
            slbPathValue.Text = _appSettings.Folders.RootFolder.Replace("/", "\\");
        }

        private void InitializeToolTips()
        {
            _toolTip.SetToolTip(btnLoad, "Refresh FilesDto");
            _toolTip.SetToolTip(btnImport, "Import FilesDto");
            _toolTip.SetToolTip(btnLog, "Refresh Logs");
            _toolTip.SetToolTip(btnProperties, "Settings");
            _toolTip.SetToolTip(btnExit, "Exit");
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            string settingsPath = _settingsService.GetSettingsFilePath();
            if (!System.IO.File.Exists(settingsPath))
            {
                _appSettings.Folders.RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _appSettings.Folders.ImportFailed = "Import_Failed";
                _appSettings.Folders.ImportSuccess = "Import_Success";
                _appSettings.Folders.Log = "Log";
                await Task.Run(() => _settingsService.SetAppSettings(_appSettings));
            }
            else
            {
                _logger.LogInformation("User settings file loaded successfully.");
            }

            try
            {
                slbPathValue.Text = _appSettings.Folders.RootFolder;
                statusStrip.SizingGrip = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MF: Unhandled error while loading main form: {ex.Message}");
            }
        }

        private async void MainForm_Shown(object? sender, EventArgs? e)
        {
            this.SuspendLayout();
            try
            {
                await ShowFormAsync(_orderForm,
                    () => new OrdersForm(_settingsService, _loggerFactory.CreateLogger<OrdersForm>(), _fileService, _excelService, _readService, _writeService));

                if (_appSettings.RefreshFilesOnStartup)
                {
                    await BtnLoad_ClickAsync(btnLoad, e);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MF: Unhandled error while showing main form: {ex.Message}");
            }
            finally
            {
                this.PerformAutoScale();
                this.PerformLayout();
            }
        }

        private async Task ShowFormAsync<T>(T formInstance, Func<T> formCreator) where T : Form
        {
            if (formInstance.IsDisposed)
            {
                formInstance = formCreator();
            }

            await LoadChildFormAsync(formInstance);
        }

        private async Task LoadChildFormAsync(Form childForm)
        {
            plFormContainer.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            plFormContainer.Controls.Add(childForm);
            plFormContainer.Tag = childForm;

            childForm.BringToFront();
            await Task.Yield();
            childForm.Show();
        }

        private void SetupButtons()
        {
            void ConfigureButton(Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.Transparent;

                btn.Image = LoadImage(btn.Name, btn.Height / 2, btn.Height / 2);
                btn.ForeColor = Color.LightGray;

                btn.MouseEnter += (s, e) =>
                {
                    btn.BackColor = Color.FromArgb(80, selectedBackgroundColor);
                    btn.Image = LoadImage(btn.Name, btn.Height / 5 * 3, btn.Height / 5 * 3);
                    btn.ForeColor = Color.LightGray;
                    btn.FlatAppearance.BorderSize = 1;
                };

                btn.MouseLeave += (s, e) =>
                {
                    if (selectedButton != btn)
                    {
                        btn.BackColor = Color.Transparent;
                    }

                    btn.Image = LoadImage(btn.Name, btn.Height / 2, btn.Height / 2);
                    btn.ForeColor = Color.LightGray;
                    btn.FlatAppearance.BorderSize = 0;
                };

                btn.Click += (s, e) => SelectButton(btn);
                btn.Paint += DrawSelectionBorder;
            }

            ConfigureButton(btnLoad);
            ConfigureButton(btnProperties);
            ConfigureButton(btnImport);
            ConfigureButton(btnLog);
            ConfigureButton(btnExit);
        }

        private void SelectButton(Button btn)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.Transparent;
                selectedButton.Invalidate();
            }

            selectedButton = btn;
            selectedButton.BackColor = selectedBackgroundColor;
            selectedButton.Invalidate();
        }

        private void DrawSelectionBorder(object? sender, PaintEventArgs e)
        {
            if (sender is Button btn && btn == selectedButton)
            {
                using Pen pen = new(borderColor, borderWidth);
                e.Graphics.DrawRectangle(pen, 1, 1, btn.Width - 2, btn.Height - 2);
            }
        }

        private Image? LoadImage(string imageName, int width, int height)
        {
            Image? originalImage = GetImageByName(imageName);
            if (originalImage == null)
            {
                _logger.LogWarning($"Image resource '{imageName}' not found or invalid.");
                return null;
            }
            try
            {
                Bitmap resizedBitmap = new(width, height);
                using (Graphics g = Graphics.FromImage(resizedBitmap))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(originalImage, new Rectangle(0, 0, width, height));
                }
                return resizedBitmap;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Failed to load or resize image '{imageName}': {ex.Message}");
                return null;
            }
        }

        private Image? GetImageByName(string imageName)
        {
            try
            {
                object? obj = Properties.Resources.ResourceManager.GetObject(imageName);
                if (obj is Image img)
                {
                    return img;
                }

                _logger.LogWarning($"Resource '{imageName}' is not a valid image.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorDto loading image resource '{imageName}': {ex.Message}");
                return null;
            }
        }

        private void btMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
            base.OnResize(e);
        }

        private void btMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private async Task BtnLoad_ClickAsync(object sender, EventArgs? e)
        {
            await Task.Run(DisableButtons);
            plSideBarMain.SuspendLayout();

            try
            {
                await ShowFormAsync(_orderForm,
                    () => new OrdersForm(_settingsService, _loggerFactory.CreateLogger<OrdersForm>(), _fileService, _excelService, _readService, _writeService));

                await _orderForm.OrdersLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during the import: {ex.Message}", "ErrorDto",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await Task.Run(EnableButtons);
                if (_orderForm != null && !_orderForm.IsDisposed)
                {
                    _orderForm.lbTitle.Text = "LOADED";
                }
                plSideBarMain.PerformLayout();
            }
        }

        private async void BtnImport_Click(object sender, EventArgs e)
        {
            if (_orderForm != null && !_orderForm.IsDisposed)
            {
                _orderForm.lbTitle.Text = string.Empty;
            }

            try
            {
                await Task.Run(DisableButtons);

                await ShowFormAsync(_orderForm,
                    () => new OrdersForm(_settingsService, _loggerFactory.CreateLogger<OrdersForm>(), _fileService, _excelService, _readService, _writeService));
                await _orderForm.ImportAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during the import: {ex.Message}", "ErrorDto",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_orderForm != null && !_orderForm.IsDisposed)
                {
                    _orderForm.lbTitle.Text = "IMPORTED";
                }
                await Task.Run(EnableButtons);
            }
        }

        private async void BtnLog_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(DisableButtons);

                await ShowFormAsync(_logForm,
                    () => new LogsForm(_settingsService, _loggerFactory.CreateLogger<LogsForm>()));

                //     await _log_form.LogRefreshAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred refreshing logs: {ex.Message}", "ErrorDto",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await Task.Run(EnableButtons);
            }
        }

        private async void BtnProperties_Click(object sender, EventArgs e)
        {
            await ShowFormAsync(_settingForm,
                () => new SettingsForm(_loggerFactory.CreateLogger<SettingsForm>(), _settingsService));
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void DisableButtons()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    foreach (Control control in plSideBarMain.Controls)
                    {
                        if (control is Button button)
                        {
                            button.Enabled = false;
                        }
                    }
                }));
                return;
            }

            foreach (Control control in plSideBarMain.Controls)
            {
                if (control is Button button)
                {
                    button.Enabled = false;
                }
            }
        }

        private void EnableButtons()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(EnableButtons));
                return;
            }

            btnLoad.Enabled = true;
            btnImport.Enabled = _orderForm?.dataGridViewFiles.Rows.Count > 0 &&
                              _orderForm?.lbTitle.Text != "IMPORTED";
            btnLog.Enabled = true;
            btnProperties.Enabled = true;
            btnExit.Enabled = true;

            if (!btnImport.Enabled)
            {
                btnLoad.Focus();
                SelectButton(btnLoad);
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DPICHANGED = 0x02E0;
            const int WM_NCHITTEST = 0x84;
            const int HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_DPICHANGED)
            {
                this.PerformAutoScale();
            }
            else if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                Point mousePosition = this.PointToClient(new Point(m.LParam.ToInt32() & 0xFFFF, m.LParam.ToInt32() >> 16));

                if (statusStrip.Bounds.Contains(mousePosition))
                {
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                    _isResizing = true;
                    return;
                }
            }
            else if (m.Msg == 0x112 && (m.WParam.ToInt32() & 0xFFF0) == 0xF008)
            {
                _isResizing = true;
            }
            else if (m.Msg == 0x46 && _isResizing)
            {
                this.Invalidate();
            }

            base.WndProc(ref m);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isResizing = false;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ResizeControls();

            Control[] controls = new Control[]
            {
                tplHeader, plTBPanel, statusStrip, plFormContainer,
                plNordanHeaderLogo, plSideBarMain
            };

            foreach (Control control in controls)
            {
                control.ResumeLayout(false);
                control.PerformLayout();
            }

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ResizeControls()
        {
            if (plFormContainer != null)
            {
                plFormContainer.Size = new Size(
                    this.ClientSize.Width - plFormContainer.Left,
                    this.ClientSize.Height - plFormContainer.Top);
            }
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.D)
            {
                using Graphics g = CreateGraphics();
                MessageBox.Show(
                    $"Current DPI: {g.DpiX} x {g.DpiY}",
                    "DPI Debug",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void PlTitleBar_MouseDown(object? sender, MouseEventArgs? e)
        {
            if (e?.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private async void BtnLoad_Click(object sender, EventArgs e)
        {
            await BtnLoad_ClickAsync(sender, e);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // _logger.DeleteLogFiles();
        }

        private void MainForm_DpiChanged(object? sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();
            ResizeControls();

            Control[] controls = new Control[]
            {
                tplHeader, plTBPanel, statusStrip, plFormContainer,
                plNordanHeaderLogo, plSideBarMain
            };

            foreach (Control control in controls)
            {
                control.ResumeLayout(false);
                control.PerformLayout();
            }

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            this.SuspendLayout();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            this.PerformLayout();
        }

        private void plNordanHeaderLogo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
