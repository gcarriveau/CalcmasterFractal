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
    public partial class LauncherForm : Form
    {
        /// <summary>
        /// List of FractalFormula structures bound to combobox
        /// </summary>
        List<FractalFormula>? formulaList;

        public LauncherForm()
        {
            InitializeComponent();
        }

        // *****************************************************************
        // Form Control Event Handlers
        // *****************************************************************
        #region Form Control Event Handlers

        /// <summary>
        /// Prepare the form controls
        /// </summary>
        /// <param name="sender">This form</param>
        /// <param name="e">Event arguments</param>
        private void LauncherForm_Load(object sender, EventArgs e)
        {
            // Load the fractal metadata from the fractals.json file
            formulaList = FractalInterface.GetFractalFormulas();

            // Handle any errors loading or converting the fractals.json file to a List<FractalFormula>
            if (formulaList == null || formulaList.Count == 0 || formulaList[index: 0].id == -1)
            {
                lblError.Text = "Failed to load fractal formulas from the fractals.json file.\nSee error.log for details.";
                cbFormulas.Enabled = false;
                btnGo.Enabled = false;
                return;
            }

            // Bind the formulas list to the combobox
            cbFormulas.DataSource = formulaList;
            cbFormulas.DisplayMember = "name";
            lblError.Text = "";
        }

        /// <summary>
        /// Free the memory that was allocated for the FractalGenerator test instance
        /// when the form is closed.
        /// </summary>
        /// <param name="sender">This form</param>
        /// <param name="e">CloseReason property: The reason why the method was called</param>
        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            return;
        }
        #endregion Form Control Event Handlers

        // *****************************************************************
        // FractalDisplayForm open/close
        // *****************************************************************
        private FractalDisplayForm? m_FractalDisplayForm = null;
        private void btnGo_Click(object sender, EventArgs e)
        {
            FractalFormula? ff = (FractalFormula?)cbFormulas.SelectedValue;
            if (ff == null) return;

            if (m_FractalDisplayForm != null)
            {
                m_FractalDisplayForm.Focus();
                return;
            }
            m_FractalDisplayForm = new();
            m_FractalDisplayForm.Show();
            // Send the selected fractal formula to the FractalDisplayForm
            m_FractalDisplayForm.FractalStart(ff, this);
            m_FractalDisplayForm.FormClosed += m_FractalDisplayForm_FormClosed;
            // To do: Create FractalStart function in FractalDisplayForm
        }
        private void m_FractalDisplayForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            // Get rid of the reference so we can open a new FractalDisplayForm
            m_FractalDisplayForm = null;
        }

        // *****************************************************************
        // FractalInterfaceTests open/close
        // *****************************************************************
        private FractalInterfaceTests? m_FractalInterfaceTests = null;
        private void btnFractalInterfaceTests_Click(object sender, EventArgs e)
        {
            if (m_FractalInterfaceTests != null)
            {
                m_FractalInterfaceTests.Focus();
                return;
            }
            m_FractalInterfaceTests = new();
            m_FractalInterfaceTests.Show();
            m_FractalInterfaceTests.FormClosed += m_FractalInterfaeTests_FormClosed;
        }
        private void m_FractalInterfaeTests_FormClosed(object? sender, FormClosedEventArgs e)
        {
            m_FractalInterfaceTests = null;
        }
    }
}
