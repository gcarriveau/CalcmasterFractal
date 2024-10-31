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
    public partial class FractalInterfaceTests : Form
    {
        Fractal gen = new Fractal();

        public FractalInterfaceTests()
        {
            InitializeComponent();
        }

        // *****************************************************************
        // Form Control Event Handlers
        // *****************************************************************
        #region Form Control Event Handlers
        private void FractalInterfaceTests_Load(object sender, EventArgs e)
        {
            ;
        }

        private bool ErrorGenerated()
        {
            if (gen.GetLastErrorCode() == 0) return false;
            MessageBox.Show(gen.GetLastErrorMessage());
            return true;
        }

        // Add Button Clicked
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Make sure the pointer isn't a nullptr
            if (ErrorGenerated()) return;
            int x, y;
            // Make sure we have values
            if (!Int32.TryParse(tbX.Text, out x) || !Int32.TryParse(tbY.Text, out y)) return;
            tbSum.Text = gen.Add(x, y).ToString();
        }

        // Destroy Button Clicked
        private void btnDestroyMap_Click(object sender, EventArgs e)
        {
            gen.Dispose();
            if (gen == null)
                lbPtrStatus.Text = "gen was destroyed.";
            else
                lbPtrStatus.Text = "gen is not null.";
        }
        #endregion Form Control Event Handlers

        private void FractalInterfaceTests_FormClosing(object sender, FormClosingEventArgs e)
        {
            gen.Dispose();
        }

        private void btnArrayTest_Click(object sender, EventArgs e)
        {
            int err = gen.CalculateMap();
            tbArrayTest.AppendText($"Iterations CalculateMap() result code: {err}\r\n");
            // This value should be around 255 if not equal.
            tbArrayTest.AppendText($"iterations[0]: {gen.GetIterationsAt(860, 810)}\r\n");
            tbArrayTest.AppendText($"Number of non-zero iterations: {gen.GetNumNonZeroInts()}\r\n");
        }

        private void btnGetLastError_Click(object sender, EventArgs e)
        {
            OutputLastError();
        }

        private void OutputLastError()
        {
            tbArrayTest.AppendText($"Last error code: {gen.GetLastErrorCode()}\r\n");
            tbArrayTest.AppendText($"Last error message: {gen.GetLastErrorMessage()}\r\n");
        }
    }
}
