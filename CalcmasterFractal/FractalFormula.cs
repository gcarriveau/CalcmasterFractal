using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcmasterFractal
{
    public class FractalFormula
    {
        public int id { get; set; } = -1;
        public string name { get; set; } = "none";
        public double centerX { get; set; } = 0.0;
        public double centerY { get; set; } = 0.0;
        public double radius { get; set; } = 2.0;
        public double limit { get; set; } = 2.0;
        public string kernel { get; set; } = "none.cu";
    }
}
