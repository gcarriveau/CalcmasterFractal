using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcmasterFractal
{
    public partial class FractalDisplayForm : Form
    {

        // ******************************************************************
        // Private properties
        // ******************************************************************
        #region Private properties

        // Instance of the Fractal class
        private Fractal gen = new Fractal();
        // Fractal formula parameters
        private FractalFormula fractalFormula = new FractalFormula();

        // Fractal bitmaps
        private Bitmap? background = null;
        private Bitmap? backgroundBackup = null;
        // Storage for main fractal state when switching to Julia
        private FractalState fractalStateBackup = new FractalState();

        // LauncherForm handle
        private LauncherForm? parent;

        // dirtyIterations set to true if we come back to main
        // fractal mode 0 from a julia variety.
        private bool dirtyIterations = false;

        // Calculation mode:
        // 0 = main fractal, 1 = julia set, 2 = TheCalcmasterTwist, 3 = AirOnAJuliaString
        private int mode = 0;

        // FractalDisplayForm client area height and width
        private Rectangle bounds;

        #endregion Private properties

        // ******************************************************************
        // Initialization - runs once
        // ******************************************************************
        #region Initialization - runs once

        /// <summary>
        /// Constructor
        /// </summary>
        public FractalDisplayForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// UI setup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FractalDisplayForm_Load(object sender, EventArgs e)
        {
            // Palette half cycle value - initial value
            cbHalfCycleValue.SelectedIndex = 1; // 20
            cbHalfCycleValue.SelectedIndexChanged += cbHalfCycleValue_SelectedIndexChanged;
        }

        /// <summary>
        /// Called by the LauncherForm
        /// </summary>
        /// <param name="fractalFormulaID">fractals.json algorithm id</param>
        /// <param name="f">pointer to the LauncherForm instance</param>
        public void FractalStart(FractalFormula ff, LauncherForm f)
        {
            parent = f;
            fractalFormula = ff;
            // Rectangle from which we can get the full screen width and height
            bounds = Screen.FromControl(this).Bounds;
            gen.SelectFractalFormula(fractalFormula.id);
            int err = gen.CalculateMap();
            if (err != 0)
            {
                MessageBox.Show(gen.GetLastErrorMessage());
                return;
            }
            background = gen.LastBitmap;
            // Mouse wheel increments or decrements the halfCycle value
            // that is used in the UpdateRandomColors() function.
            // It's somewhat similar to changing the "contrast" of the image.
            this.MouseWheel += MouseWheelHandler;
            UpdateInfoPanel();
        }

        #endregion Initialization - runs once

        // ******************************************************************
        // WinForm event handling
        // ******************************************************************
        #region WinForm event handling

        // ***************************************
        // ********** KEYBOARD *******************
        // ***************************************

        /// <summary>
        /// Handles key-down events (not to be confused with KeyPress).<br />
        /// ESC:    Exits julia mode, otherwise closes the form.<br />
        /// R:      Recalculate random color array.<br />
        /// -:      Zoom out from the center of the fractal.<br />
        /// Arrows: Used to move viewport UP, DOWN, LEFT, or RIGHT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">e.KeyCode holds the key that went down</param>
        private void FractalDisplayForm_KeyDown(object sender, KeyEventArgs e)
        {
            int err = 0;

            // ESC
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                if (mode == 0)
                {
                    this.Close();
                    return;
                }
                else
                {
                    mode = 0;
                    dirtyIterations = true;
                    gen.SetMode(0, 0, 0);
                    //err = gen.CalculateMap();
                    //if (err == 0) background = gen.LastBitmap;
                    if (backgroundBackup != null)
                    {
                        background = backgroundBackup;
                    }
                    else
                    {
                        err = gen.CalculateMap();
                        if (err == 0) background = gen.LastBitmap;
                    }
                    UpdateInfoPanel();
                    this.Refresh();
                    return;
                }
            }

            // R    (Picks a new random starting color and updates image using a new palette)
            if (e.KeyCode == Keys.R)
            {
                e.Handled = true;
                gen.ResetStartEndColors();
                gen.UpdateRandomColors();
                if (dirtyIterations)
                {
                    err = gen.CalculateMap();
                    dirtyIterations = false;
                }
                if (err == 0) background = gen.BitmapFromIterations();
                UpdateInfoPanel();
                this.Refresh();
            }

            // -    Zoom Out
            if (e.KeyCode == Keys.Subtract)
            {
                e.Handled = true;
                err = gen.ZoomOut();
                if (err == 0) background = gen.LastBitmap;
                UpdateInfoPanel();
                this.Refresh();
            }

            // Arw  Move
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        err = gen.Move(FractalInterface.Direction.UP);
                        break;
                    case Keys.Down:
                        err = gen.Move(FractalInterface.Direction.DOWN);
                        break;
                    case Keys.Left:
                        err = gen.Move(FractalInterface.Direction.LEFT);
                        break;
                    case Keys.Right:
                        err = gen.Move(FractalInterface.Direction.RIGHT);
                        break;
                }
                if (err == 0)
                {
                    background = gen.LastBitmap;
                    UpdateInfoPanel();
                    this.Refresh();
                }
                else
                {
                    MessageBox.Show($"err = {err}");
                }
            }

            // V    Toggles bitmap color inVersion
            if (e.KeyCode == Keys.V)
            {
                gen.InverseToggle = !gen.InverseToggle;
                background = gen.BitmapFromIterations();
                UpdateInfoPanel();
                this.Refresh();
            }

            // I    Toggles the information panel
            if (e.KeyCode == Keys.I)
            {
                infoPanel.Visible = !infoPanel.Visible;
            }
        }

        private void FractalDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (gen != null) gen.Dispose();
        }

        // ***************************************
        // ********** PAINT **********************
        // ***************************************

        /// <summary>
        /// Fills the form's client area surface with the fractal bitmap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FractalDisplayForm_Paint(object sender, PaintEventArgs e)
        {
            if (background != null)
            {
                //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                /*
                Rectangle srcRect = new Rectangle(0, 0, bounds.Width, bounds.Height);
                Rectangle destRect = new Rectangle(0, 0, bounds.Width * 4, bounds.Height * 4);
                Bitmap tempB = new Bitmap(background.Width * 4, background.Height * 4);
                Graphics tempG = Graphics.FromImage(tempB);
                tempG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                tempG.DrawImage(background, destRect);
                e.Graphics.DrawImage(tempB, srcRect);
                */
                e.Graphics.DrawImageUnscaled(background, 0, 0);
            }
        }

        // ***************************************
        // ********** MOUSE **********************
        // ***************************************

        /// <summary>
        /// Handles mouse-click events in combination with ModifierKeys.<br />
        /// Click:          Zoom in, centering on the pixel that was clicked.<br />
        /// Shift-Click:    Julia Set mode -> 1<br />
        /// Ctrl-Click:     TheCalcmasterTwist -> 2<br />
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">e.X and e.Y hold the coordinates of the pixel that was clicked upon</param>
        private void FractalDisplayForm_MouseClick(object sender, MouseEventArgs e)
        {
            int err = 0;
            if (mode == 0 && Control.ModifierKeys == Keys.Shift)
            {
                // make a backup of the main fractal data
                fractalStateBackup = gen.GetFractalState();
                if (background != null)
                {
                    backgroundBackup = background;
                }

                // generate the julia set
                mode = 1;
                gen.SetMode(1, e.X, e.Y);
                err = gen.CalculateMap();
                if (err == 0)
                {
                    dirtyIterations = false;
                    background = gen.LastBitmap;
                }
                UpdateInfoPanel();
                this.Refresh();
                return;
            }
            if (mode == 0 && Control.ModifierKeys == Keys.Control)
            {
                // make a backup of the main fractal data
                fractalStateBackup = gen.GetFractalState();
                if (background != null)
                {
                    backgroundBackup = background;
                }

                // generate TheCalcmasterTwist modified julia set
                mode = 2;
                gen.SetMode(2, e.X, e.Y);
                err = gen.CalculateMap();
                if (err == 0)
                {
                    dirtyIterations = false;
                    background = gen.LastBitmap;
                }
                UpdateInfoPanel();
                this.Refresh();
                return;
            }
            err = gen.ZoomInAtPoint(e.X, e.Y);
            if (err == 0) background = gen.LastBitmap;
            UpdateInfoPanel();
            this.Refresh();
        }

        /// <summary>
        /// The mouse wheel increases or decreases the Palette Half Cycle Value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseWheelHandler(object? sender, MouseEventArgs e)
        {
            // 10 20 30 40 50 60 70 80 90 100 125 150 175 200 250 300 350 400 450 500 550 600 650 700 750 800 850 900 950 1000
            if (e.Delta > 0)
            {
                if (cbHalfCycleValue.SelectedIndex < cbHalfCycleValue.Items.Count - 1)
                {
                    cbHalfCycleValue.SelectedIndex++;
                }
            }
            else
            {
                if (cbHalfCycleValue.SelectedIndex > 0)
                {
                    cbHalfCycleValue.SelectedIndex--;
                }
            }
            /*
            Array.IndexOf()
            int d = e.Delta < 0 ? -1 : 1;
            switch ()
            gen.SetHalfCycleValue();
            */
        }

        #endregion WinForm event handling

        // ***************************************
        // ********** Other Functions ************
        // ***************************************

        private void UpdateInfoPanel()
        {
            lbFiFormulaName.Text = fractalFormula.name;
            FractalState fs = gen.GetFractalState();
            switch (mode)
            {
                case 0:
                    lbFiCurrentMode.Text = "Map";
                    break;
                case 1:
                    lbFiCurrentMode.Text = "Julia Set";
                    break;
                case 2:
                    lbFiCurrentMode.Text = "The Calcmaster Twist";
                    break;
                case 3:
                    lbFiCurrentMode.Text = "Air On A Julia String";
                    break;
            }

            // viewing main fractal
            if (mode == 0)
            {
                // MaIn FrAcTaL ZoOm
                Int64 mainZoom = Convert.ToInt64(fractalFormula.radius / fs.radius);
                lbFiMainFractalZoom.Text = $"{mainZoom}X";
                // JuLiA ZoOm
                lbFiJuliaZoom.Text = "n/a";
                // Julia Set : Main Fractal zoom ratio
                lbFiZoomRatioJuliaMain.Text = "n/a";
                // Distance Between Pixels Main
                lbFiIncMain.Text = fs.inc.ToString();
                // Distance Between Pixels Julia
                lbFiIncJulia.Text = "n/a";
                // Main Viewport Center Coordinates
                lbFiMainViewCenter.Text = $"{fs.centerX},i{fs.centerY}";
                // Julia Set Viewport Center Coordinates
                lbFiJuliaViewCenter.Text = "n/a";
                // Julia Calculation P (the point that was shift-clicked)
                lbFiJuliaCenter.Text = "n/a";
            }

            // viewing a julia set type
            else
            {
                // MaIn FrAcTaL ZoOm (a constant in this state)
                Int64 mainZoom = Convert.ToInt64(fractalFormula.radius / fractalStateBackup.radius);
                lbFiMainFractalZoom.Text = $"{mainZoom}X";
                // JuLiA ZoOm
                Int64 juliaZoom = Convert.ToInt64(fractalFormula.radius / fs.radius);
                lbFiJuliaZoom.Text = $"{juliaZoom}";
                // Julia Set : Main Fractal zoom ratio
                Int64 julia2MainZoomRatio = Convert.ToInt64(fs.radius / fractalStateBackup.radius);
                lbFiZoomRatioJuliaMain.Text = julia2MainZoomRatio.ToString();
                // Distance Between Pixels Main
                lbFiIncMain.Text = fractalStateBackup.inc.ToString();
                // Distance Between Pixels Julia
                lbFiIncJulia.Text = fs.inc.ToString();
                // Main Viewport Center Coordinates
                lbFiMainViewCenter.Text = $"{fractalStateBackup.centerX},i{fractalStateBackup.centerY}";
                // Julia Set Viewport Center Coordinates
                lbFiJuliaViewCenter.Text = $"({fs.centerX},i{fs.centerY})";
                // Julia Calculation P (the point that was shift-clicked)
                lbFiJuliaCenter.Text = $"({fs.juliaCenterX},i{fs.juliaCenterY})";
            }
        }

        private void UpdateBitmap()
        {
            int err = 0;
            if (dirtyIterations)
            {
                err = gen.CalculateMap();
                if (err == 0)
                {
                    dirtyIterations = false;
                    background = gen.LastBitmap;
                    UpdateInfoPanel();
                    this.Refresh();
                }
                return;
            }
            background = gen.BitmapFromIterations();
            UpdateInfoPanel();
            this.Refresh();

        }
        private void UpdateRandomPalette(Fractal.ColorPalette pal)
        {
            if (pal.Equals(gen.GetPalette())) return;

            gen.SetPalette(pal);
            UpdateBitmap();
        }
        private void palRandomMono_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.RandomMono);
        }

        private void palRandomCompliment_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.RandomCompliment);
        }

        private void palRandomTriad_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.RandomTriad);
        }

        private void palRandomTetrad_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.RandomTetrad);
        }

        private void cbHalfCycleValue_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int halfCycleValue = Int32.Parse(cbHalfCycleValue.SelectedItem == null ? "20" : cbHalfCycleValue.SelectedItem.ToString() ?? "20");
            // Update the pallete
            gen.SetHalfCycleValue(halfCycleValue);
            // Show the updated bitmap
            UpdateBitmap();
        }
    }
}
