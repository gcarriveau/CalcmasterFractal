using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcmasterFractal
{
    // Used as a button on the information panel.
    // Reason: It doesn't interfere with the form's key press events.
    internal class DoubleBufferedLabel : Label
    {
        public DoubleBufferedLabel() : base() {
            AutoSize = false;
            DoubleBuffered = true;
            BackColor = SystemColors.ButtonFace;
            BorderStyle = BorderStyle.Fixed3D;
            ForeColor = SystemColors.ControlText;
            TextAlign = ContentAlignment.MiddleCenter;
        }
    }
}
