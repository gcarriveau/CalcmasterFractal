using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcmasterFractal
{
    /// <summary>
    /// Used to store information about the state of the fractal calculator engine.
    /// Future: This data could be saved in json format and later used to restore the engine's state.
    /// The information is obtained from the struct pointer.
    /// that is returned by the Dll's GetState() function.
    /// </summary>
    internal struct FractalState
    {
        public double centerX { get; set; }
        public double centerY { get; set; }
        public double radius { get; set; }
        public double limit { get; set; }
        public double juliaCenterX { get; set; }
        public double juliaCenterY { get; set; }
        public double inc { get; set; }
        public double left { get; set; }
        public double top { get; set; }
        public int mode { get; set; }
    }
}
