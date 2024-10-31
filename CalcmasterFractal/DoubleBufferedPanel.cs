using System.Windows.Forms;

namespace CalcmasterFractal
{
    /// <summary>
    /// The DoubleBuffered property of a Panel component is protected
    /// This class allows you to create a Panel that is double buffered
    /// To use this class, create a regular Panel on your form in the designer,
    /// then edit the {FormName}.Designer.cs file and change the data type
    /// from Panel to DoubleBufferedPanel
    /// in the declaration of the panel variable, and also in it's instantiation.
    /// This way if you change the Panel's BackgroundImage at runtime and
    /// force a refresh of the form, there will be no glitch seen when it is redrawn.
    /// </summary>
    internal class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel() : base() { DoubleBuffered = true; }
    }
}
