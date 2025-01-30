using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Mappers;
using a2p.Shared.Core.Interfaces.Services.Import;
using a2p.Shared.Core.Interfaces.Services.Other;
using a2p.Shared.Core.Utils;
using a2p.WinForm.ChildForms;
using a2p.WinForm.CustomControls;


using Microsoft.Extensions.Configuration;

using Timer = System.Windows.Forms.Timer;

using a2p.WinForm;


namespace a2p.WinForm
{
    public partial class MainForm : Form
    {



        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly IImportService _importService;
        private readonly IConfiguration _configuration;
        private readonly ILogService _logger;
        private readonly IOrderMapper _orderMapper;
        private IProgress<ProgressValue> _progress;

        private ProgressValue _progressValue;


        private readonly List<Button> _buttonList = [];
        private readonly Dictionary<Button, Panel> _buttonPanelMap = [];
        private readonly Dictionary<Panel, bool> _panelStates = [];
        private ProcessType _processType;
        private ToolTip _toolTip;
        private FileForm _fileForm;
        private LogForm _logForm;
        private SettingForm _settingForm;



        #region -== Custom Form Design Componenets ==-

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        //move form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        //resize form
        private bool isResizing = false; // Track resizing state`
        #endregion -== Custom Form Design Componenets ==-

        public MainForm(IFileService fileService, IExcelService excelService, IImportService importService, IConfiguration configuration, ILogService logger, IOrderMapper orderMapper)
        {
            _fileService=fileService;
            _excelService=excelService;
            _importService=importService;
            _configuration=configuration;
            _logger=logger;
            _orderMapper=orderMapper;
            _progress=new Progress<ProgressValue>();
            _progressValue=new ProgressValue();

            _toolTip=new ToolTip();
            _fileForm=new FileForm(_fileService, _excelService, _importService, _configuration, _logger, _orderMapper);
            _logForm=new LogForm(_fileService, _excelService, _importService, _configuration, _logger);
            _settingForm=new SettingForm(_configuration, _logger);


            this.SuspendLayout();
            InitializeComponent();
            InitializeToolTip();
            InitializeSideBarMappings();
            this.AutoScaleMode=AutoScaleMode.Dpi;
            this.AutoScaleDimensions=new SizeF(96F, 96F);
            this.ResumeLayout(true); // Resume layout

            this.KeyPreview=true; // Allows the form to capture key events before controls do
            this.KeyDown+=MainForm_KeyDown; // Attach key event handler

        }

        #region -== Initialization ==-
        private void InitializeSideBarMappings()
        {

            _buttonList.AddRange(new[] { btLoadFiles, btImport, btLog, btProperties, btExit });
            ;

            // Initialize states for child panels
            foreach (Panel panel in _buttonPanelMap.Values)
            {
                _panelStates[panel]=false;
                panel.Height=0;
            }

            // Add parent panel (plSideBarMain) to the state dictionary
            _panelStates[plSideBarMain]=true; // Assuming the main panel starts in expanded state
        }
        private void InitializeToolTip()
        {
            _toolTip=new ToolTip
            {
                AutoPopDelay=5000, // Tooltip will be visible for 5 seconds
                InitialDelay=500, // Delay before the tooltip appears
                ReshowDelay=100, // Delay before re-showing the tooltip
                ShowAlways=true // Always show tooltips, even when the parent control is inactive
            };

            // Set tooltips for buttons

            _toolTip.SetToolTip(btLoadFiles, "Refresh Files");
            _toolTip.SetToolTip(btImport, "Import Files");
            _toolTip.SetToolTip(btLog, "Refresh Logs");
            _toolTip.SetToolTip(btProperties, "Settings");
            _toolTip.SetToolTip(btExit, "Exit");
        }
        #endregion -== Initialization ==-

        #region --== Main Form Events ==-
        //===============================================================
        // Custom resizing and moving
        //===============================================================
        //Moving form

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if "Ctrl + D" is pressed
            if (e.Control&&e.Alt&&e.KeyCode==Keys.D)
            {
                using Graphics g = this.CreateGraphics();
                float dpiX = g.DpiX; // Get DPI X (horizontal)
                float dpiY = g.DpiY; // Get DPI Y (vertical)

                _=MessageBox.Show($"Current DPI: {dpiX} x {dpiY}", "DPI Debug",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Console.WriteLine($"Current DPI: {dpiX} x {dpiY}");
            }
        }
        private void PlTitleBar_MouseDown(object? sender, MouseEventArgs? e)
        {
            if (e==null)
            {
                return;
            }

            if (e.Button==MouseButtons.Left)
            {
                _=ReleaseCapture();
                _=SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
            if (e.Button==MouseButtons.Left)
            {
                _=ReleaseCapture();
                _=SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        private void MainForm_DpiChanged(object? sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetRoundedCorners(20); // Set the radius for rounded corners
            try
            {

                _panelStates[plSideBarMain]=true;
                slbPath.Text=_configuration.GetValue<string>("AppSettings:RootFolder")??string.Empty;
                statusStrip.SizingGrip=true;
                this.PerformAutoScale(); // Ensure everything is scaled correctly (optional)
                ResumeLayout(false); // Resume layout
            }


            catch (Exception ex)
            {
                _logger.Error(ex, "MF: Unhanded Error while loading main form");

            }
        }
        private async void MainForm_Shown(object? sender, EventArgs? e)
        {
            this.SuspendLayout(); // Suspend layout updates
            try
            {
                await ShowFormAsync(_fileForm, () => new FileForm(_fileService, _excelService, _importService, _configuration, _logger, _orderMapper));
                if (bool.Parse(_configuration["AppSettings:RefreshFilesOnStartup"]??"true"))
                {

                    BtFilesRefresh_Click(btLoadFiles, e);

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "MF: Unhanded Error while showing main form");
            }
            finally
            {
                PerformLayout(); // Resume layout updates
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _logger.DeleteLogFiles();
        }


        private void SetRoundedCorners(int radius)
        {
            GraphicsPath path = new();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(this.Width-radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(this.Width-radius, this.Height-radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height-radius, radius, radius), 90, 90);
            path.CloseFigure();
            this.Region=new Region(path);
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetRoundedCorners(20); // Update the rounded corners when the form is resized
            plFormContainer.Size=new Size(this.ClientSize.Width-plFormContainer.Left, this.ClientSize.Height-plFormContainer.Top);
            plFormContainer.PerformLayout(); // Ensure the layout is updated
        }


        #endregion -== Main Form Events ==-

        #region -== Child Forms Methods ==-
        private async Task ShowFormAsync<T>(T formInstance, Func<T> formCreator) where T : Form
        {
            if (formInstance==null||formInstance.IsDisposed)
            {
                formInstance=formCreator();
            }

            // Load the form into the container
            await LoadChildFormAsync(formInstance);
        }
        private async Task LoadChildFormAsync(Form childForm)
        {
            plFormContainer.Controls.Clear();

            childForm.TopLevel=false;
            childForm.FormBorderStyle=FormBorderStyle.None;
            childForm.Dock=DockStyle.Fill;

            plFormContainer.Controls.Add(childForm);
            plFormContainer.Tag=childForm;

            childForm.BringToFront();
            await Task.Yield(); // Ensures the UI thread is not blocked
            childForm.Show();
        }

        #endregion -== Child Forms Methods ==-

        #region -== Title Bar Buttons Events ==- 
        // Maximize window
        //===========================================================
        private void btMaximize_Click(object sender, EventArgs e)
        {

            this.WindowState=this.WindowState==FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
            PerformLayout();

        }
        private void btMaximize_MouseEnter(object sender, EventArgs e)
        {
            btMaximize.AutoSize=true;

        }
        private void btMaximize_MouseLeave(object sender, EventArgs e)
        {
            btMaximize.AutoSize=false;

        }

        // Minimize window
        //============================================================
        private void btMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState=FormWindowState.Minimized;
        }
        private void btMinimize_MouseEnter(object sender, EventArgs e)
        {
            btMinimize.AutoSize=true;
        }
        private void btMinimize_MouseLeave(object sender, EventArgs e)

        {
            btMinimize.AutoSize=false;
        }

        // Close window
        //===============================================================
        private void BtClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btClose_MouseEnter(object sender, EventArgs e)
        {
            btClose.AutoSize=true;
        }
        private void btClose_MouseLeave(object sender, EventArgs e)
        {
            btClose.AutoSize=false;
        }

        #endregion -== Title Bar buttons Events ==- region

        #region -== Side Bar Buttons Events ==-
        //SideBar buttons
        //================================================================
        private void BtSideBarMain_Click(object sender, EventArgs e)
        {
            if (_panelStates.TryGetValue(plSideBarMain, out bool isExpanded))
            {
                if (isExpanded)
                {

                    CollapseParentPanel(plSideBarMain);

                }
                else
                {
                    ExpandParentPanel(plSideBarMain);
                }
            }
            else
            {
                throw new KeyNotFoundException($"Panel {plSideBarMain.Name} is not initialized in _panelStates.");
            }
        }

        private async void BtFilesRefresh_Click(object sender, EventArgs? e)
        {
            DisableSideBarButtons(); // Disable buttons at the beginning
            plSideBarMain.SuspendLayout(); // Suspend layout before expanding
            _processType=ProcessType.FileImport;
            _progressValue.ProgressTitle="Loading Files ...";
            _progressValue.MinValue=0;
            _progressValue.MaxValue=100;
            _progressValue.Value=0;

            await ShowFormAsync(_fileForm,
            () => new FileForm(_fileService, _excelService, _importService, _configuration, _logger, _orderMapper));

            using ProgressBarForm progressBarForm = new()
            {
                StartPosition=FormStartPosition.CenterParent // Set to center relative to parent
            };
            progressBarForm.Load+=(sender, args) =>
            {
                progressBarForm.Location=new Point(
                    this.Location.X+((this.Width-progressBarForm.Width)/2),
                    this.Location.Y+((this.Height-progressBarForm.Height)/2)
                );
            };

            progressBarForm.Show(this);

            Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);
            _progress=progress;
            _progress?.Report(_progressValue);

            try
            {
                await ShowFormAsync(_fileForm, () => new FileForm(_fileService, _excelService, _importService, _configuration, _logger, _orderMapper));
                await _fileForm.DataTableRefreshAsync(_progress);
            }
            catch (Exception ex)
            {
                _=MessageBox.Show($"An error occurred during the import: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _processType=ProcessType.None;

                progressBarForm.Close();
                EnableSideBarButtons(); // Enable buttons at the end
                plSideBarMain.PerformLayout(); // Resume layout after expanding
            }
        }
        private async void BtFilesImport_Click(object sender, EventArgs e)
        {
            DisableSideBarButtons(); // Disable buttons at the beginning
            plSideBarMain.SuspendLayout(); // Suspend layout before expanding

            _processType=ProcessType.FileImport;

            _progressValue.ProgressTitle="Importing Files ...";
            _progressValue.MinValue=0;
            _progressValue.MaxValue=100;
            _progressValue.Value=0;

            using ProgressBarForm progressBarForm = new()
            {
                StartPosition=FormStartPosition.CenterParent // Set to center relative to parent
            };
            progressBarForm.Load+=(sender, args) =>
            {
                progressBarForm.Location=new Point(
                    this.Location.X+((this.Width-progressBarForm.Width)/2),
                    this.Location.Y+((this.Height-progressBarForm.Height)/2)
                );
            };

            progressBarForm.Show();

            Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);
            _progress=progress;
            _progress?.Report(_progressValue);

            try
            {
                await _fileForm.ImportFilesAsync(_progress);
            }
            catch (Exception ex)
            {
                _=MessageBox.Show($"An error occurred during the import: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _processType=ProcessType.None;

                progressBarForm.Close();
                EnableSideBarButtons(); // Enable buttons at the end
                plSideBarMain.PerformLayout(); // Resume layout after expanding
            }
        }
        // SideBar log buttons
        //========================================================================

        private async void BtLogRefresh_Click(object sender, EventArgs e)
        {
            DisableSideBarButtons(); // Disable buttons at the beginning
            plSideBarMain.SuspendLayout(); // Suspend layout before expanding

            await ShowFormAsync(_logForm,
            () => new LogForm(_fileService, _excelService, _importService, _configuration, _logger));


            _processType=ProcessType.FileImport;
            _progressValue.ProgressTitle="Refreshing Log ...";
            _progressValue.MinValue=0;
            _progressValue.MaxValue=100;
            _progressValue.Value=0;

            using ProgressBarForm progressBarForm = new()
            {
                StartPosition=FormStartPosition.CenterParent // Set to center relative to parent
            };
            progressBarForm.Load+=(sender, args) =>
            {
                progressBarForm.Location=new Point(
                    this.Location.X+((this.Width-progressBarForm.Width)/2),
                    this.Location.Y+((this.Height-progressBarForm.Height)/2)
                );
            };

            progressBarForm.Show();

            Progress<ProgressValue> progress = new(progressBarForm.UpdateProgress);
            _progress=progress;
            _progress?.Report(_progressValue);

            try
            {
                await _logForm.LogClearAsync();
                await _logForm.LogRefreshAsync();
            }
            catch (Exception ex)
            {
                _=MessageBox.Show($"An error occurred during the import: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _processType=ProcessType.None;

                progressBarForm.Close();
                EnableSideBarButtons(); // Enable buttons at the end
                plSideBarMain.PerformLayout(); // Resume layout after expanding
            }
        }


        // SideBar Other Buttons
        //========================================================================
        private async void BtSettings_Click(object sender, EventArgs e)
        {




            await ShowFormAsync(_settingForm,
             () => new SettingForm(_configuration, _logger));
        }
        private void BtExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Buttons style
        //========================================================================




        private void DisableSideBarButtons()
        {
            foreach (Control control in plSideBarMain.Controls)
            {
                if (control is Button button&&button!=btSideBar)
                {
                    button.Enabled=false;
                }
            }
        }

        private void EnableSideBarButtons()
        {
            foreach (Control control in plSideBarMain.Controls)
            {
                if (control is Button button&&button!=btSideBar)
                {
                    button.Enabled=true;
                }
            }
        }
        #endregion -== Side Bar Buttons Events ==-

        #region -== Side Bar Panels Events ==-
        //Side Bars Expand and Collapse
        //========================================================================

        private void UpdateButtonStylesForSidebar(bool isExpanded)
        {
            // Suspend layout for all panels to optimize updates
            plSBButtons.SuspendLayout();
            {
                try
                {
                    plTbSBInfo.Visible=isExpanded;
                    // Iterate over parent and child panels

                    foreach (Control control in plTbSBInfo.Controls)
                    {
                        if (control is Button button)
                        {

                            if (isExpanded)
                            {
                                // Sidebar expanded: Show text and image side by side
                                button.Text=button.Tag as string; // Restore original text from the Tag property
                                button.TextAlign=ContentAlignment.MiddleLeft;
                                button.ImageAlign=ContentAlignment.MiddleLeft;

                                // Disable tooltips
                                _toolTip.SetToolTip(button, null);
                            }
                            else
                            {
                                // Sidebar collapsed: Hide text, show only image
                                //button.TextImageRelation = TextImageRelation.Overlay;
                                button.TextAlign=ContentAlignment.MiddleLeft;
                                button.ImageAlign=ContentAlignment.MiddleLeft;
                                button.Tag=button.Text; // Store the original text in the Tag property
                                button.Text=string.Empty; // Clear the text
                                _toolTip.SetToolTip(button, button.Tag as string);
                            }
                        }
                    }
                }


                catch (Exception ex)
                {
                    _logger.Error(ex, "MF: Unhanded Error while updating button styles for sidebar");
                }
                finally
                {
                    // Resume layout for all panels
                    plSBButtons.PerformLayout();

                }
            }
        }
        private void ExpandParentPanel(Panel panel)
        {
            if (_panelStates[panel])
            {
                return;
            }

            plSideBarMain.SuspendLayout(); // Suspend layout before expanding

            Timer timer = new() { Interval=10 };
            timer.Tick+=(sender, e) =>
            {
                if (panel.Width<GetScaledValue(200))
                {
                    panel.Width+=GetScaledValue(10);
                }
                else
                {
                    panel.Width=GetScaledValue(200);
                    timer.Stop();
                    _panelStates[panel]=true;

                    // Update button styles for expanded state
                    UpdateButtonStylesForSidebar(true);
                    plSideBarMain.ResumeLayout(); // Resume layout after expanding
                }
            };
            timer.Start();
        }

        private void CollapseParentPanel(Panel panel)
        {
            if (!_panelStates[panel])
            {
                return;
            }

            plSideBarMain.SuspendLayout(); // Suspend layout before collapsing

            Timer timer = new() { Interval=5 };
            timer.Tick+=(sender, e) =>
            {
                if (panel.Width>GetScaledValue(40))
                {
                    panel.Width-=GetScaledValue(10);
                }
                else
                {
                    panel.Width=GetScaledValue(40);
                    timer.Stop();
                    _panelStates[panel]=false;

                    // Update button styles for collapsed state
                    UpdateButtonStylesForSidebar(false);
                    plSideBarMain.ResumeLayout(); // Resume layout after collapsing
                }
            };
            timer.Start();
        }
        private int GetScaledValue(int value)
        {
            float scalingFactor = this.CreateGraphics().DpiX/96f;
            return (int)(value*scalingFactor);
        }
        #endregion -== Side Bar Panels Events ==-

        #region -== Overrides Custom Form Methods==-
        protected override void WndProc(ref Message m)
        {
            const int WM_DPICHANGED = 0x02E0;

            if (m.Msg==WM_DPICHANGED)
            {
                this.PerformAutoScale();
            }

            const int WM_NCHITTEST = 0x84; // Windows message for hit-testing
            const int HTBOTTOMRIGHT = 17; // Code for bottom-right corner (resize grip)

            if (m.Msg==WM_NCHITTEST)
            {
                base.WndProc(ref m);

                // Get mouse position
                Point mousePosition = this.PointToClient(new System.Drawing.Point(m.LParam.ToInt32()&0xFFFF, m.LParam.ToInt32()>>16));

                // Check if mouse is over the StatusBar's resize grip
                if (statusStrip.Bounds.Contains(mousePosition))
                {
                    m.Result=HTBOTTOMRIGHT;
                    isResizing=true; // Set resizing flag
                    return;
                }
            }
            else if (m.Msg==0x112) // WM_SYSCOMMAND
            {
                // Detect resize commands
                if ((m.WParam.ToInt32()&0xFFF0)==0xF008) // SC_SIZE
                {
                    isResizing=true; // Set resizing flag
                }
            }
            else if (m.Msg==0x46) // WM_WINDOWPOSCHANGING
            {
                if (isResizing)
                {
                    this.Invalidate(); // Redraw the form during resize
                }
            }
            base.WndProc(ref m);

        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isResizing=false; // Stop resizing
        }

        #endregion -== Overrides Custom Form Methods==-

        private void btSideBar_Click(object sender, EventArgs e)
        {

        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Custom resizing logic
            ResizeControls();
        }

        private void ResizeControls()
        {
            // Example: Resize plFormContainer proportionally
            plFormContainer.Size=new Size(this.ClientSize.Width-plFormContainer.Left, this.ClientSize.Height-plFormContainer.Top);

            // Resize other controls proportionally if needed
            // Example: Resize a button proportionally
            btLoadFiles.Size=new Size(this.ClientSize.Width/4, this.ClientSize.Height/10);
        }
    }
}