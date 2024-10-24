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
        // Our local copy of the iterations array
        int[] iterations = new int[(1920 * 1080)];

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
            Array.Fill<int>(iterations, 0);
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

        private void btnArrayTest_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < iterations.Length; i++)
            {
                iterations[i] = FractalInterface.GetIterationsAt(fracMap, (UInt64)i);
            }
            tbArrayTest.Text = $"iterations[0]: {iterations[0]}\r\n";
            /*
            iterations = FractalInterface.GetIterations(fracMap);
            // Native.PassOutArrayDouble(out double[] array, out int count);
            //FractalInterface.GetIterations(out int[] iterations, out UInt64 count, fracMap);
            //tbArrayTest.Text = $"count: {count}\n";
            Span<int> span_start = new Span<int>(iterations).Slice(0,20);
            Span<int> span_end = new Span<int>(iterations).Slice(2073579, 20);
            tbArrayTest.Text = string.Join(", ", span_start.ToArray()) + "\n";
            tbArrayTest.AppendText(string.Join(", ", span_end.ToArray()) + "\n");
            */
        }
    }
}
