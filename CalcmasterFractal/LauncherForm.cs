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

    }
}
