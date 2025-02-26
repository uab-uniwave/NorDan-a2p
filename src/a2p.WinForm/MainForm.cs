// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;
using a2p.WinForm.ChildForms;

using Microsoft.Extensions.Configuration;

namespace a2p.WinForm
{
    public partial class MainForm : Form
    {

        private System.Drawing.Color selectedBackgroundColor = System.Drawing.Color.FromArgb(239, 112, 32);
        private System.Drawing.Color borderColor = System.Drawing.Color.White;
        private int borderWidth = 1;
        private Button? selectedButton = null;

        private readonly IOrderReadProcessor _orderReadProcessor;
        private readonly IExcelReadService _excelReadService;
        private readonly IOrderWriteProcessor _orderProcessingService;
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly DataCache _dataCache;

        private static ProgressValue? _progressValue;
        private IProgress<ProgressValue> _progress;
        private static ProcessType _processType = ProcessType.None;
        private ToolTip _toolTip;
        private readonly OrdersForm _orderForm;
        private readonly LogForm _logForm;
        private readonly SettingForm _settingForm;

        #region -== Custom Form Design Componenets ==-

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        //move form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        //resize form
        private bool _isResizing = false; // Track resizing state`

        #endregion -== Custom Form Design Componenets ==-

        public MainForm(IOrderReadProcessor orderReadProcessor, IExcelReadService excelReadService, IOrderWriteProcessor orderProcessingService,
            IConfiguration configuration, ILogService logService, IFileService fileService, DataCache dataCache)
        {
            _orderReadProcessor = orderReadProcessor;
            _excelReadService = excelReadService;
            _orderProcessingService = orderProcessingService;
            _configuration = configuration;
            _logService = logService;
            _fileService = fileService;
            _dataCache = dataCache;

            _progressValue = new ProgressValue();

            _progress = new Progress<ProgressValue>();

            _toolTip = new ToolTip();
            _orderForm = new OrdersForm(_configuration, _orderProcessingService, _logService, _orderReadProcessor, _excelReadService, _fileService, _dataCache);
            _logForm = new LogForm(_configuration, _logService);
            _settingForm = new SettingForm(_configuration, _logService);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.SuspendLayout();
            InitializeComponent();

            SetupButtons();

            InitializeToolTip();

            this.ResumeLayout(true); // Resume layout

            this.KeyPreview = true; // Allows the form to capture key events before controls do
            this.KeyDown += MainForm_KeyDown; // Attach key event handler
            _dataCache = dataCache;
        }

        #region -== Initialization ==-

        private void InitializeToolTip()
        {
            _toolTip = new ToolTip
            {
                AutoPopDelay = 5000, // Tooltip will be visible for 5 seconds
                InitialDelay = 500, // Delay before the tooltip appears
                ReshowDelay = 100, // Delay before re-showing the tooltip
                ShowAlways = true // Always show tooltips, even when the parent control is inactive
            };

            // Set tooltips for buttons

            _toolTip.SetToolTip(btnLoad, "Refresh Files");
            _toolTip.SetToolTip(btnImport, "Import Files");
            _toolTip.SetToolTip(btnLog, "Refresh Logs");
            _toolTip.SetToolTip(btnProperties, "Settings");
            _toolTip.SetToolTip(btnExit, "Exit");
        }

        #endregion -== Initialization ==-

        #region --== Form Events ==-
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetRoundedCorners(20); // Set the radius for rounded corners
            try
            {

                slbPath.Text = _configuration.GetValue<string>("AppSettings:RootFolder") ?? string.Empty;
                statusStrip.SizingGrip = true;
                this.PerformAutoScale(); // Ensure everything is scaled correctly (optional)
            }

            catch (Exception ex)
            {
                _logService.Error(ex, "MF: Unhanded Error while loading main form");

            }
        }

        private async void MainForm_Shown(object? sender, EventArgs? e)
        {
            this.SuspendLayout(); // Suspend layout updates
            try
            {
                await ShowFormAsync(_orderForm,
                    () => new OrdersForm(_configuration, _orderProcessingService, _logService, _orderReadProcessor, _excelReadService, _fileService, _dataCache));
                if (bool.Parse(_configuration["AppSettings:RefreshFilesOnStartup"] ?? "true"))
                {

                    BtnLoad_Click(btnLoad, e);

                }

            }
            catch (Exception ex)
            {
                _logService.Error(ex, "MF: Unhanded Error while showing main form");
            }
            finally
            {

                PerformLayout(); // Resume layout updates
            }
        }
        private void MainForm_DpiChanged(object? sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();
            ResizeControls();
            tplHeader.ResumeLayout(false);
            tplHeader.PerformLayout();
            tlpTitleBar.ResumeLayout(false);
            tlpTitleBar.PerformLayout();
            plTitleBar.ResumeLayout(false);
            plTBPanel.ResumeLayout(false);
            plTBPanel.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            plFormContainer.ResumeLayout(false);
            plFormContainer.PerformLayout();
            plNordanHeaderLogo.ResumeLayout(false);
            plNordanHeaderLogo.PerformLayout();
            plTbSBInfo.ResumeLayout(false);
            plTbSBInfo.PerformLayout();
            plMiniLogo.ResumeLayout(false);
            plMiniLogo.PerformLayout();
            plTitleBarAppName.ResumeLayout(false);
            plTitleBarAppName.PerformLayout();
            plSideBarMain.ResumeLayout(false);
            plSideBarMain.PerformLayout();
            this.ResumeLayout(false);

            this.PerformLayout();
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // Check if "Ctrl + D" is pressed
            if (e.Control && e.Alt && e.KeyCode == Keys.D)
            {
                using Graphics g = this.CreateGraphics();
                float dpiX = g.DpiX; // Get DPI X (horizontal)
                float dpiY = g.DpiY; // Get DPI Y (vertical)

                _ = MessageBox.Show($@"Current DPI: {dpiX} x {dpiY}", "DPI Debug",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Console.WriteLine($"Current DPI: {dpiX} x {dpiY}");
            }
        }

        private void PlTitleBar_MouseDown(object? sender, MouseEventArgs? e)
        {
            if (e == null)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                _ = ReleaseCapture();
                _ = SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }

            if (e.Button == MouseButtons.Left)
            {
                _ = ReleaseCapture();
                _ = SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void SetRoundedCorners(int radius)
        {
            GraphicsPath path = new();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetRoundedCorners(20); // Update the rounded corners when the form is resized
            if (plFormContainer != null)
            {
                plFormContainer.Size = new Size(this.ClientSize.Width - plFormContainer.Left,
                    this.ClientSize.Height - plFormContainer.Top);
                plFormContainer.PerformLayout(); // Ensure the layout is updated
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) => _logService.DeleteLogFiles();

        private void MainForm_ResizeBegin(object sender, EventArgs e) => this.SuspendLayout();
        private void MainForm_ResizeEnd(object sender, EventArgs e) => this.PerformLayout();

        #endregion -== Main Form Events ==-

        #region -== Child Forms Methods ==-

        private async Task ShowFormAsync<T>(T formInstance, Func<T> formCreator) where T : Form
        {
            if (formInstance.IsDisposed)
            {
                formInstance = formCreator();
            }

            // Load the form into the container
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
            await Task.Yield(); // Ensures the UI thread is not blocked
            childForm.Show();
        }

        #endregion -== Child Forms Methods ==-

        #region -== Setup Buttons ==-

        private void SetupButtons()
        {
            // Circular buttons

            SetupCircularButton(btnMaximize);
            SetupCircularButton(btnMinimize);
            SetupCircularButton(btnClose);

            // Group buttons with selection
            SetupGroupButton(btnLoad);
            SetupGroupButton(btnProperties);
            SetupGroupButton(btnImport);
            SetupGroupButton(btnLog);
            SetupGroupButton(btnExit);

        }

        /// <summary>
        /// Sets up circular buttons with hover effect.
        /// </summary>
        private void SetupCircularButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = System.Drawing.Color.FromArgb(56, 57, 60);

            // Set circular shape
            GraphicsPath path = new();
            path.AddEllipse(0, 0, btn.Width, btn.Height);
            btn.Region = new Region(path);

            // Load images
            btn.Image = LoadImage(btn.Name, btn.Width - 4, btn.Height - 4);

            // Hover effect
            btn.MouseEnter += (s, e) => btn.Image = LoadImage(btn.Name + "Hover", btn.Width, btn.Height);
            btn.MouseLeave += (s, e) => btn.Image = LoadImage(btn.Name, btn.Width - 4, btn.Height - 4);
        }

        /// <summary>
        /// Sets up group buttons with selection & hover effect.
        /// </summary>
        private void SetupGroupButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = System.Drawing.Color.Transparent;

            // Load normal image
            btn.Image = LoadImage(btn.Name, btn.Height / 2, btn.Height / 2);
            btn.ForeColor = System.Drawing.Color.LightGray;

            // Hover effect
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = System.Drawing.Color.FromArgb(80, selectedBackgroundColor);
                btn.Image = LoadImage(btn.Name, btn.Height / 5 * 3, btn.Height / 5 * 3);
                btn.ForeColor = System.Drawing.Color.LightGray;
                btn.FlatAppearance.BorderSize = 1;
            };
            btn.MouseLeave += (s, e) =>
            {
                if (selectedButton != btn)
                {
                    btn.BackColor = System.Drawing.Color.Transparent;
                }

                btn.Image = LoadImage(btn.Name, btn.Height / 2, btn.Height / 2);
                btn.ForeColor = System.Drawing.Color.LightGray;
                btn.FlatAppearance.BorderSize = 0;

            };

            // Click event for selection
            btn.Click += (s, e) => SelectButton(btn);

            // Attach Paint event for border drawing
            btn.Paint += DrawSelectionBorder;
        }

        private void SelectButton(Button btn)
        {
            // Reset previous selection
            if (selectedButton != null)
            {
                selectedButton.BackColor = System.Drawing.Color.Transparent;
                selectedButton.Invalidate(); // Redraw previous button
            }

            // Apply new selection
            selectedButton = btn;
            selectedButton.BackColor = selectedBackgroundColor;
            selectedButton.Invalidate(); // Redraw new selection
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
            return originalImage == null ? null : ResizeImage(originalImage, new Size(width, height));
        }

        private Image? GetImageByName(string imageName)
        {
            try
            {

                Image? image = (Image?)Properties.Resources.ResourceManager.GetObject(imageName);
                return image ?? null;
            }
            catch
            {
                _logService.Debug($"Error loading image: {imageName}");
                return null;

            }
        }

        private Image ResizeImage(Image imgToResize, Size size)
        {
            Bitmap resizedBitmap = new(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, new Rectangle(0, 0, size.Width, size.Height));
            }

            return resizedBitmap;
        }

        #endregion -== Setup Buttons ==-

        #region -== Title Bar buttons Events ==-

        // Maximize window
        //===========================================================
        private void btMaximize_Click(object sender, EventArgs e)
        {

            this.WindowState = this.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;

            base.OnResize(e);

        }

        // Minimize window
        //============================================================
        private void btMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        // Close window
        //===============================================================
        private void btClose_Click(object sender, EventArgs e) => Application.Exit();

        #endregion -== Title Bar buttons Events ==-

        #region -== Side Bar Buttons Events ==-

        //SideBar buttons
        //================================================================

        private async void BtnLoad_Click(object sender, EventArgs? e)
        {
            await Task.Run(DisableButtons); // Disable buttons at the beginning
            plSideBarMain.SuspendLayout(); // Suspend layout before expanding
            _processType = ProcessType.FileImport;

            try
            {
                await ShowFormAsync(_orderForm,
                    () => new OrdersForm(_configuration, _orderProcessingService, _logService, _orderReadProcessor, _excelReadService, _fileService, _dataCache));

                await _orderForm.OrdersLoad();

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($@"An error occurred during the import: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await Task.Run(EnableButtons);  // Enable buttons at the end
                plSideBarMain.PerformLayout(); // Resume layout after expanding
            }
        }
        private async void BtnImport_Click(object sender, EventArgs e)
        {

            try
            {
                await Task.Run(DisableButtons); // Disable buttons at the beginning
                _processType = ProcessType.LogRefresh;

                await ShowFormAsync(_orderForm,
                     () => new OrdersForm(_configuration, _orderProcessingService, _logService, _orderReadProcessor, _excelReadService, _fileService, _dataCache));
                await _orderForm.ImportFilesAsync();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($@"An error occurred during the import: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _processType = ProcessType.None;
                await Task.Run(EnableButtons); // Enable buttons at the end 

            }
        }

        private async void BtnLog_Click(object sender, EventArgs e)
        {

            try
            {

                await Task.Run(DisableButtons); // Disable buttons at the beginning

                await ShowFormAsync(_logForm,
                  () => new LogForm(_configuration, _logService));
                _processType = ProcessType.LogRefresh;

                await _logForm.LogRefreshAsync();

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($@"An error occurred during the import: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _processType = ProcessType.None;

                await Task.Run(EnableButtons);

            }

        }

        // SideBar Other Buttons
        //========================================================================
        private async void BtnProperties_Click(object sender, EventArgs e) => await ShowFormAsync(_settingForm,
                () => new SettingForm(_configuration, _logService));

        private void BtnExit_Click(object sender, EventArgs e) => Application.Exit();

        //Buttons style
        //========================================================================

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
            }
            else
            {
                foreach (Control control in plSideBarMain.Controls)
                {
                    if (control is Button button)
                    {
                        button.Enabled = false;
                    }
                }
            }
        }

        private void EnableButtons() //TODO: Enable buttons
        {

            if (InvokeRequired)
            {
                Invoke(new Action(() => EnableButtons()));

            }
            else
            {

                btnLoad.Enabled = true;
                if (_orderForm.dataGridViewFiles.Rows.Count > 0)
                {
                    btnImport.Enabled = true;
                }
                else
                {
                    btnImport.Enabled = false;
                    _ = btnLoad.Focus();
                    SelectButton(btnLoad);
                }
                btnLog.Enabled = true;
                btnProperties.Enabled = true;
                btnExit.Enabled = true;

            }
        }

        #endregion -== Side Bar Buttons Events ==-

        #region -== Overrides Custom Form Methods==-
        protected override void WndProc(ref Message m)
        {
            const int WM_DPICHANGED = 0x02E0;

            if (m.Msg == WM_DPICHANGED)
            {
                this.PerformAutoScale();
            }

            const int WM_NCHITTEST = 0x84; // Windows message for hit-testing
            const int HTBOTTOMRIGHT = 17; // Code for bottom-right corner (resize grip)

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                // Get mouse position
                Point mousePosition = this.PointToClient(new System.Drawing.Point(m.LParam.ToInt32() & 0xFFFF, m.LParam.ToInt32() >> 16));

                // Check if mouse is over the StatusBar's resize grip
                if (statusStrip.Bounds.Contains(mousePosition))
                {
                    m.Result = HTBOTTOMRIGHT;
                    _isResizing = true; // Set resizing flag
                    return;
                }
            }
            else if (m.Msg == 0x112) // WM_SYSCOMMAND
            {
                // Detect resize commands
                if ((m.WParam.ToInt32() & 0xFFF0) == 0xF008) // SC_SIZE
                {
                    _isResizing = true; // Set resizing flag
                }
            }
            else if (m.Msg == 0x46) // WM_WINDOWPOSCHANGING
            {
                if (_isResizing)
                {
                    this.Invalidate(); // Redraw the form during resize
                }
            }
            base.WndProc(ref m);

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isResizing = false; // Stop resizing
        }

        #endregion -== Overrides Custom Form Methods==-

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Custom resizing logic
            ResizeControls();
            tplHeader.ResumeLayout(false);
            tplHeader.PerformLayout();
            tlpTitleBar.ResumeLayout(false);
            tlpTitleBar.PerformLayout();
            plTitleBar.ResumeLayout(false);
            plTBPanel.ResumeLayout(false);
            plTBPanel.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            plFormContainer.ResumeLayout(false);
            plFormContainer.PerformLayout();
            plNordanHeaderLogo.ResumeLayout(false);
            plMiniLogo.ResumeLayout(false);
            plMiniLogo.PerformLayout();
            plTitleBarAppName.ResumeLayout(false);
            plTitleBarAppName.PerformLayout();
            plSideBarMain.ResumeLayout(false);
            plSideBarMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void ResizeControls()
        {
            if (plFormContainer != null)
            {
                plFormContainer.Size = new Size(this.ClientSize.Width - plFormContainer.Left, this.ClientSize.Height - plFormContainer.Top);
            }

        }

    }
}
