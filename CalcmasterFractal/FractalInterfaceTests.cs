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
        /// <summary>
        /// Stores a pointer to the FractalGenerator class defined in libs\CalcmasterFractalDll.dll
        /// </summary>
        IntPtr fracMap = IntPtr.Zero;

        public FractalInterfaceTests()
        {
            InitializeComponent();
        }

        // *****************************************************************
        // Memory Allocation Methods and Form Load/Close Event Handlers
        // *****************************************************************
        #region Memory Allocation Methods and Event Handlers

        bool isNullptr(IntPtr ptr)
        {
            if (ptr.Equals(IntPtr.Zero)) return true;
            return false;
        }
        #endregion Memory Allocation Methods and Event Handlers

        // *****************************************************************
        // Form Control Event Handlers
        // *****************************************************************
        #region Form Control Event Handlers
        private void FractalInterfaceTests_Load(object sender, EventArgs e)
        {
            // DEBUG: Generate an instance of the FractalGenerator class from the Dll for test purposes
            fracMap = FractalInterface.InstantiateFractalGenerator();
            lbPtrStatus.Text = "fracMap pointer initialized successfully.";
        }


        // Add Button Clicked
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Make sure the pointer isn't a nullptr
            if (isNullptr(fracMap)) return;
            int x, y;
            // Make sure we have values
            if (!Int32.TryParse(tbX.Text, out x) || !Int32.TryParse(tbY.Text, out y)) return;
            tbSum.Text = FractalInterface.Add(fracMap, x, y).ToString();
        }

        // Destroy Button Clicked
        private void btnDestroyMap_Click(object sender, EventArgs e)
        {
            if (isNullptr(fracMap)) return;
            bool res = FractalInterface.DestroyFractalGenerator(fracMap);
            fracMap = IntPtr.Zero;
            if (res)
                lbPtrStatus.Text = "fracMap object was destroyed.";
            else
                lbPtrStatus.Text = "no action, pointer was already null.";
        }
        #endregion Form Control Event Handlers

        private void FractalInterfaceTests_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isNullptr(fracMap)) return;
            bool res = FractalInterface.DestroyFractalGenerator(fracMap);
        }
    }
}
