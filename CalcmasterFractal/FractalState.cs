using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcmasterFractal
{
    internal struct FractalState
    {
        public double centerX;
        public double centerY;
        public double radius;
        public double limit;
        public double juliaCenterX;
        public double juliaCenterY;
        public double inc;
        public double left;
        public double top;
        public int mode;
    }
}
