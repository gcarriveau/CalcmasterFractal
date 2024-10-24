using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalcmasterFractal
{
    /// <summary>
    /// Wrapper class for accessing the C++ functions that are exported by CalcmasterFractalDll.dll.
    /// </summary>
    internal class FractalInterface
    {

        // *****************************************************************
        // Utility Functions
        // *****************************************************************

        /// <summary>
        /// Loads and converts the fractals.json file to a List&lt;FractalFormula&gt;
        /// </summary>
        /// <returns>List of fractal formula metadata objects</returns>
        public static List<FractalFormula>? GetFractalFormulas()
        {
            string json = "[]";

            try
            {
                using (TextReader r = File.OpenText(path: "fractals.json"))
                {
                    json = r.ReadToEnd();
                }
                return System.Text.Json.JsonSerializer.Deserialize<List<FractalFormula>>(json: json);
            }
            catch (Exception ex)
            {
                using (TextWriter w = File.CreateText(path: "error.log"))
                {
                    w.WriteLine(value: "**********************************************************");
                    w.WriteLine(value: DateTime.Now.ToString(format: "yyyy-MM-dd HH:mm:ss"));
                    w.WriteLine(value: $"Error: {ex.Message}");
                    w.WriteLine(value: $"StackTrace:");
                    w.WriteLine(value: ex.StackTrace);
                }
            }
            return [new() { id = -1, name = "Error" }];
        }

        private const string fracDll = @"lib\CalcmasterFractalDll.dll";

        // *****************************************************************
        // FractalGenerator Constructor/Destructor
        // *****************************************************************

        /// <summary>
        /// Calls the constructor of the C++ FractalGenerator class
        /// </summary>
        /// <returns>Pointer to a new instance of FractalGenerator</returns>
        [DllImport(dllName: fracDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern IntPtr InstantiateFractalGenerator();

        /// <summary>
        /// Destroys the C++ FractalGenerator class instantiation
        /// </summary>
        /// <param name="generator">Pointer to an instance of FractalGenerator</param>
        /// <returns>true if successful, false otherwise</returns>
        [DllImport(dllName: fracDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern bool DestroyFractalGenerator(IntPtr generator);

        // *****************************************************************
        // FractalGenerator Instance API
        // *****************************************************************

        /// <summary>
        /// Test function that adds two integers and returns an int result
        /// </summary>
        /// <param name="t">Pointer to an instance of FractalGenerator</param>
        /// <param name="x">first integer value</param>
        /// <param name="y">second integer value</param>
        /// <returns></returns>
        [DllImport(dllName: fracDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern int Add(IntPtr t, int x, int y);

        [DllImport(dllName: fracDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetIterationsAt(IntPtr t, UInt64 index);
    }
}
