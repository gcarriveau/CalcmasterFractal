﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CalcmasterFractal
{
    /// <summary>
    /// This class creates an instantiation of the CalcmasterFractalDLL's FractalGenerator class
    /// so that the Windows Form UI doesn't need to keep track of state nor use a pointer to call
    /// the FractalInterface static functions.
    /// 
    /// It also cleans up the unmanaged instance of the FractalGenerator by inheriting from IDisposable and calls
    /// FractalInterface.DestroyFractalGenerator when it is garbage collected so that you don't have
    /// to do that in your C# Form code.
    /// </summary>
    internal class Fractal : IDisposable
    {

        // ******************************************************************
        // Constructor/Destructor
        // ******************************************************************
        #region Constructor/Destructor

        /// <summary>
        /// Constructor which gets a pointer to a new instance of the FractalGenerator class.
        /// The default fractal type exposed by the FractalGenerator class is the Mandelbrot set
        /// </summary>
        public Fractal()
        {
            // dll stuff
            m_ptrFractalGenerator = FractalInterface.InstantiateFractalGenerator();
            m_iterations = new int[(m_height * m_width)];
            m_re = new double[(m_height * m_width)];
            m_im = new double[(m_height * m_width)];
            // Warm up the random number generator
            int randomWarmupCycles = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            for (int n = 0; n < randomWarmupCycles; n++) r.NextDouble();
            //UpdateRandomColorList();
            ResetStartEndColors();
            UpdateRandomColors();
        }

        // Implement IDisposable.
        public void Dispose()
        {
            Dispose(disposing: true);
            // This object will be cleaned up by the Dispose method,
            // therefore, you should call GC.SuppressFinalize to take this object off the finalization
            // queue and prevent finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose of any other managed C# IDisposable objects that this class uses (none)
                    ;
                }
                disposed = FractalInterface.DestroyFractalGenerator(m_ptrFractalGenerator);
                m_ptrFractalGenerator = IntPtr.Zero;
            }
        }

        // Use C# finalizer syntax for finalization code.
        // This finalizer will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide finalizer in types derived from this class.
        ~Fractal()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(disposing: false) is optimal in terms of
            // readability and maintainability.
            Dispose(disposing: false);
        }

        #endregion Constructor/Destructor

        // ******************************************************************
        // Public properties
        // ******************************************************************
        #region Public properties

        // m_arrColor length
        public const int NumberOfColors = 5000;
        // Antialiasing algorithm
        public AntiAliasAlg CurAntiAliasAlg = AntiAliasAlg.NoModification;

        // ITERATIONS LIMIT ACCORDING TO CURRENT ZOOM
        // For every 1000x zoom, add 50 to MaxIterations.. zoom inc is 1.5x
        public int Mode { get; private set; } = 0;
        public const double ZoomFactor = 1.5;
        public double CurZoom { get; set; } = 1.0;
        public double CurZoomBackup { get; set; } = 1.0;
        public const int MinIterationsBoundary = 250;
        public const int MaxIterationsBoundary = 5000;
        public int MaxIterations { get; set; } = MinIterationsBoundary;
        public bool MaxIterationsLocked { get; set; } = false;
        public int MaxIterationsBackup { get; set; } = MaxIterationsBoundary;
        public List<int> G_hasItsList { get; private set; } = new List<int>();

        #endregion Public properties

        // ******************************************************************
        // Public methods
        // ******************************************************************
        #region Public methods and Enums

        public enum ColorPalette
        {
            RandomMono, RandomCompliment, RandomTriad, RandomTetrad, Rainbow, MonoDistributed, Grayscale
            //, Random2, Random3, Monochrome, Yellows, RedGreen
        }

        public enum AntiAliasAlg { NoModification, OneAway, TwoAway }

        /// <summary>
        /// Gets or updates the color palette algorithm.
        /// Setting the algorithm updates the internal color array, but does not update the bitmap.
        /// </summary>
        public void SetPalette(ColorPalette palette)
        {
            m_palette = palette;
            UpdateRandomColors();
        }
        public ColorPalette GetPalette()
        {
            return m_palette;
        }
        public void SetStartColor(string colorName)
        {
            m_startColor = Color.FromName(colorName);
            UpdateRandomColors();
        }

        /// <summary>
        /// If true, it change the behavior of the GetColor(int numIts) function so that the color returned
        /// is based on maxIts - numIts instead of numIts.
        /// </summary>
        public bool InverseToggle { get; set; } = false;

        /// <summary>
        /// Stores the last bitmap generated by BitmapFromIterations()
        /// </summary>
        public Bitmap? LastBitmap { get; internal set; }

        /// <summary>
        /// Returns the last error code generated by the Dll's FractalGenerator class, 0 if no error has occurred.
        /// </summary>
        /// <returns>Error code</returns>
        public int GetLastErrorCode()
        {
            return m_ptrFractalGenerator == IntPtr.Zero ? -1 : FractalInterface.GetLastErrorCode(m_ptrFractalGenerator);
        }

        /// <summary>
        /// Returns an error text message corresponding to the value returned by GetLastErrorCode()
        /// </summary>
        /// <returns>A short error description.</returns>
        public string GetLastErrorMessage()
        {
            if (m_ptrFractalGenerator == IntPtr.Zero) return "The underlying class has been destroyed.";
            int code = FractalInterface.GetLastErrorCode(m_ptrFractalGenerator);
            switch (code)
            {
                case 0:
                    return "OK";
                case 1:
                    return "Error in FractalGenerator() constructor locating/loading/opening the fractals.json file.";
                case 2:
                    return "Error in FractalGenerator() constructor parsing the fractals.json file into a valid JSON Document.";
                case 3:
                    return "Error in setDimensions(height, width). Either height or width is a value <= 0.";
                case 4:
                    return "Error in calculateMap() allocating memory to the CUDA compute device.";
                case 5:
                    return "Error in calculateMap() copying host data to device memory.";
                case 6:
                    return "Error in calculateMap() launching the kernel.";
                case 7:
                    return "Error in calculateMap() copying iterations back to m_iterations.";
                case 8:
                    return "Error in calculateMap() freeing device global memory.";
                case 9:
                    return "Error in move(). Invalid direction parameter value.";
                default:
                    return $"Uknown error with code [{code}] occurred.";
            }
        }

        /// <summary>
        /// Sets the fractal image size height and width in pixels.
        /// If the FractalDisplayForm is maximized and has no border, you can get the
        /// full size of the screen with Screen.FromControl(this).Bounds
        /// </summary>
        /// <param name="bounds">fractal viewing area rectangle</param>
        public void SetDimensions(Rectangle bounds)
        {
            // Exit if the default dimensions are the same.
            if (m_height == bounds.Height && m_width == bounds.Width) return;

            // Otherwise, resize the arrays.
            m_height = bounds.Height;
            m_width = bounds.Width;
            FractalInterface.SetDimensions(m_ptrFractalGenerator, m_height, m_width);
            m_iterations = new int[(m_height * m_width)];
            m_re = new double[(m_height * m_width)];
            m_im = new double[(m_height * m_width)];
        }

        /// <summary>
        /// Gets iterations for the pixel corresponding to image pixel coordinates (col,row).
        /// Useful for the user interface to get information about a pixel.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetIterationsAt(int col, int row)
        {
            return m_iterations[row * m_width + col];
        }

        // Histogram weighted value from 0.0 to 1.0
        public float GetIterationsWeight(int numIts)
        {
            if (m_iterations_weights == null) return 0;
            return m_iterations_weights[numIts];
        }

        /// <summary>
        /// A simple test function for making the DLL add two numbers and return a result
        /// </summary>
        /// <param name="x">first addend</param>
        /// <param name="y">second addend</param>
        /// <returns>the sum of x + y</returns>
        public int Add(int x, int y)
        {
            return FractalInterface.Add(m_ptrFractalGenerator, x, y);
        }

        /// <summary>
        /// Resets the FractalGenerator class for use with the parameter collection from
        /// the fractals.json file with the selected ID.  If a corresponding kernel does not exist
        /// MapCalc.cu, it will execute the algMandelbrot kernel.
        /// </summary>
        /// <param name="fractalFormulaID">The corresponding "id" value from the fractals.json file.</param>
        public void SelectFractalFormula(int fractalFormulaID)
        {
            FractalInterface.SelectFractalFormula(m_ptrFractalGenerator, fractalFormulaID);
        }

        public void SetMode(int mode, int mouseX, int mouseY)
        {
            if (Mode == 0 && mode != 0)
            {
                Mode = mode;
                CurZoomBackup = CurZoom;
                MaxIterationsBackup = MaxIterations;
                CurZoom = 1.0;
                MaxIterations = MinIterationsBoundary;
            }
            else if (Mode != 0 && mode == 0)
            {
                Mode = mode;
                CurZoom = CurZoomBackup;
                MaxIterations = MaxIterationsBackup;
            }
            FractalInterface.SetMode(m_ptrFractalGenerator, mode, mouseX, mouseY);
        }

        public void SetJuliaCenter(double juliaCenterX, double juliaCenterY)
        {
            FractalInterface.SetJuliaCenter(m_ptrFractalGenerator, juliaCenterX, juliaCenterY);
        }

        public FractalState GetFractalState()
        {
            FractalState fs = Marshal.PtrToStructure<FractalState>(FractalInterface.GetState(m_ptrFractalGenerator));
            return fs;
        }

        /// <summary>
        /// Calculates the iterations for the main fractal and updates LastBitmap
        /// </summary>
        /// <returns></returns>
        public int CalculateMap(int ismove = 0)
        {
            int err = FractalInterface.CalculateMap(m_ptrFractalGenerator, Math.Min(MaxIterations,m_filterEnd), ismove);
            if (err == 0)
            {
                UpdateIterations();
                LastBitmap = BitmapFromIterations();
            }
            return err;
        }

        /// <summary>
        /// Creates a new bitmap calculating pixel colors based on current palette in m_arrColors[] and contents of m_iterations[].
        /// When finished, updates the LastBitmap property with the returned result.
        /// </summary>
        /// <returns>The generated bitmap</returns>
        public Bitmap BitmapFromIterations()
        {
            //if (_filterWidth > 0) return FilteredBitmapFromIterations(_filterStart, _filterWidth);

            // FAST! Generate the bitmap using the new color set based on our stored set of iterations
            Bitmap bmp = new Bitmap(m_width, m_height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, m_width, m_height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            // Set pixel colors based on iteration counts
            byte[] pixelArray = new byte[4 * m_iterations.Length];
            Int32 index = 0;
            for (int i = 0; i < m_iterations.Length; i++)
            {
                Color c = GetColor(m_iterations[i]);
                pixelArray[index] = c.B;
                pixelArray[index + 1] = c.G;
                pixelArray[index + 2] = c.R;
                pixelArray[index + 3] = 255;
                index += 4;
            }
            // Copy the bytes to the bitmap memory location
            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(pixelArray, 0, ptr, pixelArray.Length);
            bmp.UnlockBits(bmpData);
            LastBitmap = bmp;
            return LastBitmap;
        }

        /// <summary>
        /// Zoom in 1.5x with new center where the mouse was clicked.
        /// </summary>
        /// <param name="x">column</param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int ZoomInAtPoint(int col, int row)
        {
            CurZoom *= ZoomFactor;
            // Test of user set MaxIterations manually so something higher, and keep it
            MaxIterations = MaxIterationsLocked ? MaxIterations : Convert.ToInt32(CurZoom / 6.0 + MinIterationsBoundary);
            if (MaxIterations < MinIterationsBoundary) MaxIterations = MinIterationsBoundary;
            if (MaxIterations > MaxIterationsBoundary) MaxIterations = MaxIterationsBoundary-1;
            int err = FractalInterface.ZoomInAtPoint(m_ptrFractalGenerator, col, row);
            if (err == 0) return CalculateMap();
            return err;
        }

        /// <summary>
        /// Zoom out 1.5x keeping same center
        /// </summary>
        /// <returns></returns>
        public int ZoomOut()
        {
            CurZoom /= ZoomFactor;
            MaxIterations = MaxIterationsLocked ? MaxIterations: Convert.ToInt32(CurZoom / 6.0 + MinIterationsBoundary);
            if (MaxIterations < MinIterationsBoundary) MaxIterations = MinIterationsBoundary;
            if (MaxIterations > MaxIterationsBoundary) MaxIterations = MaxIterationsBoundary-1;
            int err = FractalInterface.ZoomOut(m_ptrFractalGenerator);
            if (err == 0) return CalculateMap();
            return err;
        }

        public int ResetToAutoMaxIterations()
        {
            MaxIterations = Convert.ToInt32(CurZoom / 6.0 + MinIterationsBoundary);
            if (MaxIterations < MinIterationsBoundary) MaxIterations = MinIterationsBoundary;
            if (MaxIterations > MaxIterationsBoundary) MaxIterations = MaxIterationsBoundary - 1;
            int err = FractalInterface.ZoomOut(m_ptrFractalGenerator);
            if (err == 0) return CalculateMap();
            return err;
        }

        public int Move(FractalInterface.Direction d)
        {
            int err = FractalInterface.Move(m_ptrFractalGenerator, ((int)d));
            if (err == 0) return CalculateMap(ismove: 1);
            return err;
        }

        /// <summary>
        /// Counts how many non-zero integers are found in the iterations vector
        /// </summary>
        /// <returns></returns>
        public int GetNumNonZeroInts()
        {
            int count = 0;
            for(int i = 0; i < m_iterations.Length; i++)
            {
                if (m_iterations[i] != 0) count++;
            }
            return count;
        }


        /// <summary>
        /// Gets the current escape limit for the fractal
        /// </summary>
        public void SetLimit(double limit)
        {
            FractalInterface.SetLimit(m_ptrFractalGenerator, limit);
        }
        /// <summary>
        /// Gets the current escape limit for the fractal
        /// </summary>
        /// <returns></returns>
        public double GetLimit()
        {
            return FractalInterface.GetLimit(m_ptrFractalGenerator);
        }

        #endregion Public methods

        // ******************************************************************
        // Private properties
        // ******************************************************************
        #region Private properties

        /// <summary>
        /// Pointer to an instance of FractalGenerator
        /// </summary>
        private IntPtr m_ptrFractalGenerator = IntPtr.Zero;

        /// <summary>
        /// Track whether the Dispose() has been called.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Width of the fractal image in pixels
        /// </summary>
        private int m_width = 1920;

        /// <summary>
        /// Height of the fractal image in pixels
        /// </summary>
        private int m_height = 1080;

        /// <summary>
        /// To do:
        /// m_maxIts is essential for selecting BLACK as the pixel color when maxIts is reached.
        /// it needs to correspond with the DLL's m_maxIts
        /// </summary>
        private int m_maxIts = 1000;

        // Our local copy of the iterations array
        private int[] m_iterations;
        // Number of pixels having x number of iterations
        private int[] m_iterations_counts;
        private int m_iterations_counts_max = 1;
        // Color weight given to the number of iterations
        private float[] m_iterations_weights;
        
        // Iterations filter upper and lower boundaries
        // If m_iterations[x] < m_filterStart, GetColor(int its) returns Color.Black
        // If m_iterations[x] > m_filterEnd, GetColor(int its) returns Color.Black
        private int m_filterStart = 0;
        private int m_filterEnd = MaxIterationsBoundary;


        // holds the real fractal plane coordinates (unused, but can be updated from the DLL using UpdatePoints())
        private double[] m_re;
        // holds the imaginary fractal plane coordinates (unused, but can be updated from the DLL using UpdatePoints())
        private double[] m_im;

        #endregion Private properties

        // ******************************************************************
        // Private methods
        // ******************************************************************
        #region Private methods

        /// <summary>
        /// Updates the local m_iterations[] array with the values of the FractalGenerator's m_iterations vector.
        /// </summary>
        private void UpdateIterations()
        {
            IntPtr ptr = FractalInterface.GetIterations(m_ptrFractalGenerator);
            Marshal.Copy(ptr, m_iterations, 0, m_height * m_width);
            
            // Analysis of the result:
            // Reset m_maxIts to the max number of iterations found in m_iterations
            // This makes sure that pixels with the highest number of iterations are black
            // and gives us pretty fine lines and borders at those points.
            m_maxIts = 0;  // maximum number of iterations reached in the run.
            G_hasItsList = new List<int>();
            int thirdrow = m_width * 2;
            int thirdtolast = m_iterations.Length - thirdrow;
            int pos = 0;
            int avg = 0;
            int halfWay = m_iterations.Length / 2;
            // counts tags non-zero numbers of iterations
            m_iterations_counts = new int[10000];
            switch (CurAntiAliasAlg)
            {
                case AntiAliasAlg.OneAway:
                    for (int i = 0; i < halfWay; i++)
                    {
                        // antialias technique
                        if (i > thirdrow && i < thirdtolast)
                        {
                            avg = m_iterations[i - 1] + m_iterations[i] + m_iterations[i + 1];
                            pos = i - m_width;
                            avg += m_iterations[pos];
                            pos = i + m_width;
                            avg += m_iterations[pos];
                            m_iterations[i] = avg / 5;
                        }
                        m_maxIts = Math.Max(m_maxIts, m_iterations[i]);
                        m_iterations_counts[m_iterations[i]] = 1;
                    }
                    for (int i = m_iterations.Length - 1; i > halfWay; i--)
                    {
                        // antialias technique
                        if (i > thirdrow && i < thirdtolast)
                        {
                            avg = m_iterations[i - 1] + m_iterations[i] + m_iterations[i + 1];
                            pos = i - m_width;
                            avg += m_iterations[pos];
                            pos = i + m_width;
                            avg += m_iterations[pos];
                            m_iterations[i] = avg / 5;
                        }
                        m_maxIts = Math.Max(m_maxIts, m_iterations[i]);
                        m_iterations_counts[m_iterations[i]] = 1;
                    }
                    break;
                case AntiAliasAlg.TwoAway:
                    for (int i = 0; i < halfWay; i++)
                    {
                        // antialias technique
                        if (i > thirdrow && i < thirdtolast)
                        {
                            avg = m_iterations[i - 2] + m_iterations[i - 1] + m_iterations[i] + m_iterations[i + 1] + m_iterations[i + 2];
                            pos = i - m_width;
                            avg += m_iterations[pos - 1] + m_iterations[pos] + m_iterations[pos + 1];
                            pos -= - m_width;
                            avg += m_iterations[pos];
                            pos = i + m_width;
                            avg += m_iterations[pos - 1] + m_iterations[pos] + m_iterations[pos + 1];
                            pos += m_width;
                            avg += m_iterations[pos];
                            m_iterations[i] = avg / 13;
                        }
                        m_maxIts = Math.Max(m_maxIts, m_iterations[i]);
                        m_iterations_counts[m_iterations[i]] = 1;
                    }
                    for (int i = m_iterations.Length - 1; i > halfWay; i--)
                    {
                        // antialias technique
                        if (i > thirdrow && i < thirdtolast)
                        {
                            avg = m_iterations[i - 2] + m_iterations[i - 1] + m_iterations[i] + m_iterations[i + 1] + m_iterations[i + 2];
                            pos = i - m_width;
                            avg += m_iterations[pos - 1] + m_iterations[pos] + m_iterations[pos + 1];
                            pos -= -m_width;
                            avg += m_iterations[pos];
                            pos = i + m_width;
                            avg += m_iterations[pos - 1] + m_iterations[pos] + m_iterations[pos + 1];
                            pos += m_width;
                            avg += m_iterations[pos];
                            m_iterations[i] = avg / 13;
                        }
                        m_maxIts = Math.Max(m_maxIts, m_iterations[i]);
                        m_iterations_counts[m_iterations[i]] = 1;
                    }
                    break;
                default:
                    for (int i = 0; i < m_iterations.Length; i++)
                    {
                        m_maxIts = Math.Max(m_maxIts, m_iterations[i]);
                        m_iterations_counts[m_iterations[i]] = 1;
                    }
                    break;
            }
            /*
            foreach (int numIts in m_iterations)
            {
                m_maxIts = Math.Max(m_maxIts, numIts);
                m_iterations_counts[numIts] = 1;
            }
            */
            for (int i = 0; i < m_iterations_counts.Length; i++)
            {
                if (m_iterations_counts[i] == 1) G_hasItsList.Add(i);
            }
            G_hasItsList.Sort();
            if (m_palette == ColorPalette.Rainbow || m_palette == ColorPalette.Grayscale || m_palette == ColorPalette.MonoDistributed)
                UpdateRandomColors();

            /*
            // slow
            hasItsList.Sort();
            int numElms = hasItsList.Count;
            int w = 0;
            double piOver2 = Math.PI / 2.0;
            for (int i = 0; i < numElms; i++)
            {

                w = Convert.ToInt32(255.0 * Math.Sin(piOver2 * (double)i / (double)numElms));
                m_arrColors[hasItsList[i]] = Color.FromArgb(w,w,w);
            }
            */

                /*
                // Histogram
                // number of pixels having x number of iterations.. 1920 x 1080 = 2,073,600 pixels
                m_iterations_counts = new int[m_maxIts + 1];
                Array.Fill<int>(m_iterations_counts, 0); // <-- do we need to do this in C#?
                m_iterations_weights = new float[m_maxIts + 1];
                m_iterations_counts_max = 1;
                foreach (int numIts in m_iterations)
                    m_iterations_counts[numIts]++;
                foreach (int numPix in m_iterations_counts)
                    m_iterations_counts_max = Math.Max(m_iterations_counts_max, numPix);
                for (int i = 0; i < MaxIterations; i++)
                {
                    m_iterations_weights[i] = (float)m_iterations_counts[i] / (float)m_iterations_counts_max;
                    int c = 255 - Convert.ToInt32(255 * m_iterations_weights[i]);
                    m_arrColors[i] = Color.FromArgb(c, c, c);
                }
                */

                // Create an iteration statistics dictionary

                //UpdateRandomColors();
                //if (m_maxIts > m_maxRandomColors)
                //{
                //    m_maxRandomColors = m_maxIts;
                //    UpdateRandomColorList();
                //}
        }

        /// <summary>
        /// Updates the local m_re and m_im real and imaginary fractal plane coordinate
        /// arrays with the values of the FractalGenerator's m_re and m_im vectors.
        /// </summary>
        private void UpdatePoints()
        {
            IntPtr ptr = FractalInterface.GetReals(m_ptrFractalGenerator);
            Marshal.Copy(ptr, m_re, 0, m_height * m_width);
            ptr = FractalInterface.GetImaginaries(m_ptrFractalGenerator);
            Marshal.Copy(ptr, m_im, 0, m_height * m_width);
        }

        #endregion Private methods

        // ***********************************************************************************
        // Color related
        // ***********************************************************************************
        #region Coloring variables and functions

        // The current color chooser algorithm
        private ColorPalette m_palette = ColorPalette.RandomCompliment;

        // palette generationi variables
        private int halfCycle = 20;
        private Color m_startColor;
        private Color m_endColor;
        private Color[] m_arrColors = new Color[1];
        // A random number generator should be instantiated once.  It is primed in the constructor.
        private static Random r = new Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour);

        public void ResetStartEndColors()
        {
            // let's pick one from the list  28 to 167 converted to KnownColor
            m_startColor = Color.FromKnownColor((KnownColor)r.Next(28, 167));
            return;
            SuperColor sc = new SuperColor();
            sc.V = 0.9d;
            sc.S = 1.0d;
            sc.H = (double)r.Next(0, 359);
            m_startColor = sc.Color;
            sc.H = sc.H + 180;
            m_endColor = sc.Color;
            /*
            double radius = 80.0;
            // R G B are at 120 degree angles from one another.
            // R at 0 = 127 + radius; at 180 = 127 - radius.. R = 127 + sin(angle) * radius
            // G at angle + 2PI/3
            // B at angle - 2PI/3
            // Pick a color that is k distance away from the center of the color wheel
            double rAngle = r.NextDouble() * 2.0 * Math.PI;
            double gAngle = rAngle + 2.0 * Math.PI / 3.0;
            double bAngle = rAngle - 2.0 * Math.PI / 3.0;
            int nextR = 127 + (Int32)(Math.Sin(rAngle) * radius);
            int nextG = 127 + (Int32)(Math.Sin(gAngle) * radius);
            int nextB = 127 + (Int32)(Math.Sin(bAngle) * radius);
            m_startColor = Color.FromArgb(nextR, nextG, nextB);
            m_endColor = Color.FromArgb(255 - nextR, 255 - nextG, 255 - nextB);
            */
        }

        /// <summary>
        /// The half cycle value controls the number of colors between the lightest and darkest shade of a color in the palette.
        /// Higher values sharpen small areas of pixels that have a high numbers of iterations of which vary considerably from pixel to pixel.
        /// Lower values add constrast to large areas of pixels that have similar numbers of iterations.
        /// </summary>
        /// <param name="value"></param>
        public void SetHalfCycleValue(int value)
        {
            halfCycle = value;
            UpdateRandomColors();
        }
        public int GetHalfCycleValue()
        {
            return halfCycle;
        }

        #region *************** FILTER FUNCTIONS *****************
        public void ResetFilterRange()
        {
            m_filterStart = 0;
            m_filterEnd = MaxIterationsBoundary;
        }
        public int SetFilterEndToCurrentMaxIts()
        {
            m_filterEnd = m_maxIts;
            return m_maxIts;
        }
        public int GetFilterStart()
        {
            return m_filterStart;
        }
        public int IncFilterStart(int increment = 1)
        {
            m_filterStart += increment;
            if (m_filterStart > m_filterEnd) m_filterStart = m_filterEnd - 1;
            return m_filterStart;
        }
        public int DecFilterStart(int decrement = 1)
        {
            m_filterStart -= decrement;
            if (m_filterStart < 0) m_filterStart = 0;
            return m_filterStart;
        }
        public int GetFilterEnd()
        {
            return m_filterEnd;
        }
        public int IncFilterEnd(int increment = 1)
        {
            m_filterEnd += increment;
            if (m_filterEnd > MaxIterations) m_filterEnd = MaxIterations;
            return m_filterEnd;
        }
        public int DecFilterEnd(int decrement = 1)
        {
            m_filterEnd -= decrement;
            if (m_filterEnd <= m_filterStart) m_filterEnd = m_filterStart + 1;
            return m_filterEnd;
        }
        #endregion *************** FILTER FUNCTIONS *****************

        /// <summary>
        /// Recalculates 5000 colors in a cycle starting from m_startColor;
        /// </summary>
        public void UpdateRandomColors()
        {
            SuperColor sc;
            // Rainbow, Grayscale
            if (m_palette == ColorPalette.Rainbow || m_palette == ColorPalette.MonoDistributed || m_palette == ColorPalette.Grayscale)
            {
                int numElms = G_hasItsList.Count;
                int w = 0;
                double piOver2 = Math.PI / 2.0;
                sc = new SuperColor(m_startColor);
                double startH = sc.H;
                double startV = sc.V;
                switch (m_palette)
                {
                    case ColorPalette.Rainbow:
                        // Color based on Hue angle
                        for (int i = 0; i < numElms; i++)
                        {
                            sc.H = startH + 360.0 * Math.Sin(piOver2 * (double)i / (double)numElms);
                            m_arrColors[G_hasItsList[i]] = sc.Color;
                        }
                        break;
                    case ColorPalette.MonoDistributed:
                        // Value down to up
                        for (int i = 0; i < numElms; i++)
                        {
                            //sc.V = 1 - Math.Sin(Math.PI * (double)i / (double)numElms);
                            sc.V = Math.Sin(Math.PI * (double)i / (double)numElms);
                            m_arrColors[G_hasItsList[i]] = sc.Color;
                        }
                        break;
                    case ColorPalette.Grayscale:
                        // Grayscale
                        for (int i = 0; i < numElms; i++)
                        {
                            w = Convert.ToInt32(255.0 * Math.Sin(piOver2 * (double)i / (double)numElms));
                            m_arrColors[G_hasItsList[i]] = Color.FromArgb(w, w, w);
                        }
                        break;
                }
                return;
            }

            // RandomMono, RandomCompliment, RandomTriad, RandomTetrad
            sc = new SuperColor(m_startColor);
            m_arrColors = new Color[NumberOfColors];
            double incSat = sc.S / (double)halfCycle;
            double incVal = sc.V / (double)halfCycle;
            double incHue = 180;
            switch(m_palette)
            {
                case ColorPalette.RandomCompliment:
                    incHue = 180;
                    break;
                case ColorPalette.RandomTriad:
                    incHue = 120;
                    break;
                case ColorPalette.RandomTetrad:
                    incHue = 90;
                    break;
                case ColorPalette.RandomMono:
                    incHue = 0;
                    break;
            }
            int it = 0;
            while (it < m_arrColors.Length)
            {
                int cycle = 0;
                while (cycle < halfCycle && it < m_arrColors.Length)
                {
                    sc.S -= incSat;
                    sc.V -= incVal;
                    m_arrColors[it] = sc.Color;
                    cycle++;
                    it++;
                }
                cycle = 0;
                sc.H += incHue;
                while (cycle < halfCycle && it < m_arrColors.Length)
                {
                    sc.S += incSat;
                    sc.V += incVal;
                    m_arrColors[it] = sc.Color;
                    cycle++;
                    it++;
                }
                if (m_palette == ColorPalette.RandomMono) continue;
                cycle = 0;
                while (cycle < halfCycle && it < m_arrColors.Length)
                {
                    sc.S -= incSat;
                    sc.V -= incVal;
                    m_arrColors[it] = sc.Color;
                    cycle++;
                    it++;
                }
                cycle = 0;
                sc.H += incHue;
                while (cycle < halfCycle && it < m_arrColors.Length)
                {
                    sc.S += incSat;
                    sc.V += incVal;
                    m_arrColors[it] = sc.Color;
                    cycle++;
                    it++;
                }
                if (m_palette == ColorPalette.RandomCompliment) continue;
                cycle = 0;
                while (cycle < halfCycle && it < m_arrColors.Length)
                {
                    sc.S -= incSat;
                    sc.V -= incVal;
                    m_arrColors[it] = sc.Color;
                    cycle++;
                    it++;
                }
                cycle = 0;
                sc.H += incHue;
                while (cycle < halfCycle && it < m_arrColors.Length)
                {
                    sc.S += incSat;
                    sc.V += incVal;
                    m_arrColors[it] = sc.Color;
                    cycle++;
                    it++;
                }
                if (m_palette == ColorPalette.RandomTriad) continue;
                cycle = 0;
                while (cycle < halfCycle && it < m_arrColors.Length)
                {
                    sc.S -= incSat;
                    sc.V -= incVal;
                    m_arrColors[it] = sc.Color;
                    cycle++;
                    it++;
                }
                cycle = 0;
                sc.H += incHue;
                while (cycle < halfCycle && it < m_arrColors.Length)
                {
                    sc.S += incSat;
                    sc.V += incVal;
                    m_arrColors[it] = sc.Color;
                    cycle++;
                    it++;
                }
            }

            /*
            for (int numIts = 0; numIts < m_maxIts; numIts++)
            {
                sc.S -= incSat;
                m_arrColors[numIts] = sc.Color;
                //sc.H = sc.H + numIts;
                //double percent = (double)numIts / (double)m_maxIts;
                //int r = (int)((double)m_startColor.R + ((double)(m_endColor.R - m_startColor.R) * percent));
                //int g = (int)((double)m_startColor.G + ((double)(m_endColor.G - m_startColor.G) * percent));
                //int b = (int)((double)m_startColor.B + ((double)(m_endColor.B - m_startColor.B) * percent));
                //m_arrColors[numIts] = Color.FromArgb(r, g, b);
                ////int level = 255 - (int)(255.0 * Math.Sin(Math.PI * percent));
                //int levelR = (int)(r * (1 + Math.Cos(2 * Math.PI * percent)) / 2);
                //int levelG = (int)(g * (1 + Math.Cos(2 * Math.PI * percent)) / 2);
                //int levelB = (int)(g * (1 + Math.Cos(2 * Math.PI * percent)) / 2);
                //m_arrColors[numIts] = Color.FromArgb(levelR, levelG, levelB);
            }
            */

        }

        /// <summary>
        /// Fetches a Color from m_arrColors at the numIts position
        /// </summary>
        /// <param name="numIts"></param>
        /// <returns></returns>
        public Color GetColor(Int32 numIts)
        {
            if (numIts == 0 || numIts == m_maxIts) return Color.Black;
            if (numIts < m_filterStart || numIts > m_filterEnd) return Color.Black;
            // Grayscale histogram..
            //int weight = 255 - Convert.ToInt32(GetIterationsWeight(numIts) * 255);
            //return Color.FromArgb(255, weight, weight, weight);
            return InverseToggle ? m_arrColors[m_maxIts - numIts] : m_arrColors[numIts];
        }

        #endregion Coloring variables and functions

        // ***********************************************************************************
        // Old, unused
        // ***********************************************************************************
        #region Unused

        // holds a smoothly distributed list of colors for the bitmap image generator
        // number of random colors to generate in m_randomColors list
        private Int32 m_maxRandomColors = 1024;
        private List<Color> m_randomColors { get; set; } = new List<Color>(5000); // Holds a randomly generated list of colors which grows according to increases in _maxIts (See UpdateRandomColorList())

        // (old, unused)
        public Color GetColor_backup(Int32 numIts)
        {
            if (numIts >= m_maxIts) return Color.Black;
            int colorIndex = numIts * m_maxRandomColors / m_maxIts;
            //if (numIts < 0) numIts *= -1;
            /*
            // Set first few its to black
            if (!julia && numIts < 8) return Color.Black;
            if (julia && numIts < 4) return Color.Black;
            */
            // Random
            return m_randomColors[numIts];
            /*
            //if (m_palette == ColorPalette.Random) return m_randomColors[numIts % m_maxRandomColors];
            //if (m_palette == ColorPalette.Random) return m_randomColors[colorIndex];
            // Monochrome
            if (m_palette == ColorPalette.Monochrome) return numIts % 2 == 0 ? Color.Black : Color.White;

            //Int32 brilliance = (Int32)double.Floor(255.0 * numIts / (double)_maxIts);
            Int32 brilliance = numIts > 255 ? numIts % 255 : numIts;
            if (brilliance % 2 == 1)
            {
                return Color.FromArgb(255, 0, 0, 0); // black
                                                     //return Color.FromArgb(255, brilliance, brilliance, 0); // yellows
                                                     //return Color.FromArgb(255, 255 - brilliance, 255 - brilliance, 255 - brilliance); // inverse gray-scale
            }
            else
            {
                var r = new Random(brilliance);
                //return Color.FromArgb(255, r.Next(256), r.Next(256), r.Next(256)); // random colors
                //return Color.FromArgb(255, r.Next(256), 127, r.Next(256));

                //return Color.FromArgb(255, 255 - brilliance, 127, brilliance);
                //return Color.FromArgb(255, 255-brilliance, brilliance, brilliance); // cyan
                //return Color.FromArgb(255, 0, brilliance, brilliance); // cyan
                //return Color.FromArgb(255, brilliance, brilliance, 0); // yellow
                //return Color.FromArgb(255, brilliance, 255 - brilliance, 0); // red/green transitions
                //return Color.FromArgb(255, 255-brilliance, 255 - brilliance, 255-brilliance); // inverse gray-scale

                // random gray scale
                int b = r.Next(256);
                switch (m_palette)
                {
                    case ColorPalette.GrayScale:
                        return Color.FromArgb(255, 255 - b, 255 - b, 255 - b); // gray scale
                        break;
                    case ColorPalette.Random2:
                        return Color.FromArgb(255, r.Next(256), r.Next(256), r.Next(256)); // random
                        break;
                    case ColorPalette.Random3:
                        return Color.FromArgb(255, r.Next(256), 127, r.Next(256)); // random 2
                        break;
                    case ColorPalette.Yellows:
                        return Color.FromArgb(255, 255 - b, 255 - b, 0); // yellows
                        break;
                    case ColorPalette.RedGreen:
                        return Color.FromArgb(255, brilliance, 255 - b, 0); // red/green transitions
                        break;
                    default: // Grayscale
                        b = r.Next(256);
                        return Color.FromArgb(255, 255 - b, 255 - b, 255 - b); // gray scale
                        break;
                }
                //return Color.FromArgb(255, brilliance, 255 - b, 0); // red/green transitions
                //return Color.FromArgb(255, 255 - b, 255 - b, 0); // yellows
                //return Color.FromArgb(255, 255-b, b, b);
            }
            */
        }
        
        // Tinted Complimentary Colors (old, unused)
        private void UpdateRandomColorList(bool clear = false)
        {
            List<Color> newColors;
            if (clear)
                newColors = new List<Color>(m_maxRandomColors);
            else
                newColors = m_randomColors;

            //Int32 numColors = Math.Max(_maxRandomColors, m_maxIts);
            Int32 numColors = m_maxRandomColors;

            Color targetColor, targetComplement, tetradColor, tetradComplement;
            if (newColors.Count < numColors)
            {
                DateTime now = DateTime.Now;
                Random r = new Random(now.Second + now.Minute + now.Hour);
                Int32 nextR, nextG, nextB;

                if (newColors.Count == 0)
                {
                    //_randomColors.Add(GetRandomColor());
                    newColors.Add(Color.Black);
                }
                Color lastColor = newColors[newColors.Count - 1];
                double radius = 80.0;
                // R G B are at 120 degree angles from one another.
                // R at 0 = 127 + radius; at 180 = 127 - radius.. R = 127 + sin(angle) * radius
                // G at angle + 2PI/3
                // B at angle - 2PI/3
                // Pick a color that is k distance away from the center of the color wheel
                double rAngle = r.NextDouble() * 2.0 * Math.PI;
                double gAngle = rAngle + 2.0 * Math.PI / 3.0;
                double bAngle = rAngle - 2.0 * Math.PI / 3.0;
                nextR = 127 + (Int32)(Math.Sin(rAngle) * radius);
                nextG = 127 + (Int32)(Math.Sin(gAngle) * radius);
                nextB = 127 + (Int32)(Math.Sin(bAngle) * radius);
                targetColor = Color.FromArgb(nextR, nextG, nextB);
                targetComplement = Color.FromArgb(255 - nextR, 255 - nextG, 255 - nextB);
                // calculate tetrad color and complement
                rAngle = rAngle + Math.PI / 2.0;
                gAngle = rAngle + 2.0 * Math.PI / 3.0;
                bAngle = rAngle - 2.0 * Math.PI / 3.0;
                nextR = 127 + (Int32)(Math.Sin(rAngle) * radius);
                nextG = 127 + (Int32)(Math.Sin(gAngle) * radius);
                nextB = 127 + (Int32)(Math.Sin(bAngle) * radius);
                tetradColor = Color.FromArgb(nextR, nextG, nextB);
                tetradComplement = Color.FromArgb(255 - nextR, 255 - nextG, 255 - nextB);
                // calculate the colors
                while (newColors.Count < numColors)
                {
                    // brighten to targetColor
                    for (int i = 10; i < 100; i += 5)
                    {
                        Int32 redVal = (255 - targetColor.R) * i / 100;
                        Int32 blueVal = (255 - targetColor.B) * i / 100;
                        Int32 greenVal = (255 - targetColor.G) * i / 100;
                        newColors.Add(Color.FromArgb(255, redVal, blueVal, greenVal));
                    }
                    // darken targetColor
                    for (int i = 90; i > 0; i -= 5)
                    {
                        Int32 redVal = (255 - targetColor.R) * i / 100;
                        Int32 blueVal = (255 - targetColor.B) * i / 100;
                        Int32 greenVal = (255 - targetColor.G) * i / 100;
                        newColors.Add(Color.FromArgb(255, redVal, blueVal, greenVal));
                    }
                    // brighten toward complement
                    for (int i = 10; i < 100; i += 5)
                    {
                        Int32 redVal = (255 - targetComplement.R) * i / 100;
                        Int32 blueVal = (255 - targetComplement.B) * i / 100;
                        Int32 greenVal = (255 - targetComplement.G) * i / 100;
                        newColors.Add(Color.FromArgb(255, redVal, blueVal, greenVal));
                    }
                    // darken complement
                    for (int i = 90; i > 0; i -= 5)
                    {
                        Int32 redVal = (255 - targetComplement.R) * i / 100;
                        Int32 blueVal = (255 - targetComplement.B) * i / 100;
                        Int32 greenVal = (255 - targetComplement.G) * i / 100;
                        newColors.Add(Color.FromArgb(255, redVal, blueVal, greenVal));
                    }
                    /*
                    // brighten toward tetrad
                    for (int i = 10; i < 100; i += 5)
                    {
                        Int32 redVal = (255 - tetradColor.R) * i / 100;
                        Int32 blueVal = (255 - tetradColor.B) * i / 100;
                        Int32 greenVal = (255 - tetradColor.G) * i / 100;
                        newColors.Add(Color.FromArgb(255, redVal, blueVal, greenVal));
                    }
                    // darken tetrad
                    for (int i = 90; i > 0; i -= 5)
                    {
                        Int32 redVal = (255 - tetradColor.R) * i / 100;
                        Int32 blueVal = (255 - tetradColor.B) * i / 100;
                        Int32 greenVal = (255 - tetradColor.G) * i / 100;
                        newColors.Add(Color.FromArgb(255, redVal, blueVal, greenVal));
                    }
                    // brighten toward tetrad
                    for (int i = 10; i < 100; i += 5)
                    {
                        Int32 redVal = (255 - tetradComplement.R) * i / 100;
                        Int32 blueVal = (255 - tetradComplement.B) * i / 100;
                        Int32 greenVal = (255 - tetradComplement.G) * i / 100;
                        newColors.Add(Color.FromArgb(255, redVal, blueVal, greenVal));
                    }
                    // darken toward tetradComplement
                    for (int i = 90; i > 0; i -= 5)
                    {
                        Int32 redVal = (255 - tetradComplement.R) * i / 100;
                        Int32 blueVal = (255 - tetradComplement.B) * i / 100;
                        Int32 greenVal = (255 - tetradComplement.G) * i / 100;
                        newColors.Add(Color.FromArgb(255, redVal, blueVal, greenVal));
                    }
                    */
                }
            }
            m_randomColors = newColors;
        }

        #endregion Unused
    }
}
