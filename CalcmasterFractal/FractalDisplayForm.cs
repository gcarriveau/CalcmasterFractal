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

        // Fractal bitmaps
        private Bitmap? background = null;
        private Bitmap? backgroundBackup = null;

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
        public void FractalStart(int fractalFormulaID, LauncherForm f)
        {
            parent = f;
            // Rectangle from which we can get the full screen width and height
            bounds = Screen.FromControl(this).Bounds;
            gen.SelectFractalFormula(fractalFormulaID);
            int err = gen.CalculateMap();
            if (err == 0) background = gen.LastBitmap;
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
                this.Refresh();
            }

            // -    Zoom Out
            if (e.KeyCode == Keys.Subtract)
            {
                e.Handled = true;
                err = gen.ZoomOut();
                if (err == 0) background = gen.LastBitmap;
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
                    this.Refresh();
                }
                else
                {
                    MessageBox.Show($"err = {err}");
                }
            }

            // I    Toggles bitmap color inversion
            if (e.KeyCode == Keys.I)
            {
                gen.InverseToggle = !gen.InverseToggle;
                background = gen.BitmapFromIterations();
                this.Refresh();
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
                e.Graphics.DrawImage(background, 0, 0);
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
                mode = 1;
                gen.SetMode(1, e.X, e.Y);
                err = gen.CalculateMap();
                if (err == 0)
                {
                    dirtyIterations = false;
                    background = gen.LastBitmap;
                }
                this.Refresh();
                return;
            }
            if (mode == 0 && Control.ModifierKeys == Keys.Control)
            {
                if (background != null)
                {
                    backgroundBackup = background;
                }
                mode = 2;
                gen.SetMode(2, e.X, e.Y);
                err = gen.CalculateMap();
                //MessageBox.Show($"err = {err}; num non-zero elements in iterations: {gen.GetNumNonZeroInts()}");
                if (err == 0)
                {
                    dirtyIterations = false;
                    background = gen.LastBitmap;
                }
                this.Refresh();
                return;
            }
            err = gen.ZoomInAtPoint(e.X, e.Y);
            if (err == 0) background = gen.LastBitmap;
            this.Refresh();
        }
        #endregion WinForm event handling

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
                    this.Refresh();
                }
                return;
            }
            background = gen.BitmapFromIterations();
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
