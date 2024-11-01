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
        private Fractal currentFractal;
        private Bitmap? background = null;
        private LauncherForm? parent;
        //
        private int mode;
        Rectangle bounds;

        public FractalDisplayForm()
        {
            InitializeComponent();
        }

        public void FractalStart(int fractalFormulaID, LauncherForm f)
        {
            parent = f;
            currentFractal = gen;
            // Rectangle from which we can get the full screen width and height
            bounds = Screen.FromControl(this).Bounds;
            gen.SelectFractalFormula(fractalFormulaID);
            int err = gen.CalculateMap();
            if (err == 0) background = gen.LastBitmap;
        }

        private void FractalDisplayForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                if (currentFractal.Equals(gen))
                {
                    this.Close();
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
            int err = currentFractal.ZoomInAtPoint(e.X, e.Y);
            if (err == 0) background = currentFractal.LastBitmap;
            this.Refresh();
        }
    }
}
