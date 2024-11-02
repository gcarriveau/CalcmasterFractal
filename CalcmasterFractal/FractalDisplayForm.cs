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
        private Fractal gen = new Fractal();
        //private Fractal julia = new Fractal();
        //private Fractal carriveau = new Fractal();
        private Bitmap? background = null;
        private Bitmap? backgroundBackup = null;
        private LauncherForm? parent;
        // dirtyIterations set to true if we come back to main
        // fractal mode 0 from a julia variety.
        private bool dirtyIterations = false;
        // mode: 0 = main fractal, 1 = julia set, 2 = TheCalcmasterTwist, 3 = AirOnAJuliaString
        private int mode = 0;
        Rectangle bounds;

        public FractalDisplayForm()
        {
            InitializeComponent();
        }

        public void FractalStart(int fractalFormulaID, LauncherForm f)
        {
            parent = f;
            // Rectangle from which we can get the full screen width and height
            bounds = Screen.FromControl(this).Bounds;
            gen.SelectFractalFormula(fractalFormulaID);
            int err = gen.CalculateMap();
            if (err == 0) background = gen.LastBitmap;
        }

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

            if (e.KeyCode == Keys.Subtract)
            {
                e.Handled = true;
                err = gen.ZoomOut();
                if (err == 0) background = gen.LastBitmap;
                this.Refresh();
            }

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
        }

        private void FractalDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (gen != null) gen.Dispose();
        }

        private void FractalDisplayForm_Paint(object sender, PaintEventArgs e)
        {
            if (background != null)
            {
                e.Graphics.DrawImage(background, 0, 0);
            }
        }

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
    }
}
