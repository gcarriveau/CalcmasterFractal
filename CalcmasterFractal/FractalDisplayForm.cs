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
    public partial class FractalDisplayForm : Form
    {

        // ***************************************
        // ****** PRIVATE PROPERTIES *************
        // ***************************************

        #region Private properties

        // Instance of the Fractal class
        private Fractal gen = new Fractal();

        // Fractal formula parameters sent to the Dll
        private FractalFormula fractalFormula = new FractalFormula();

        // Fractal bitmaps
        // background is the bitmap that is painted onto the surface of the
        // FractalDisplayForm after calculating iterations for pixels or
        // updating the color palette in some way.
        private Bitmap? background = null;
        // Saves the main fractal image while we are in a julia mode
        // so that coming back to the main fractal does not require recalculation.
        private Bitmap? backgroundBackup = null;
        // backgroundMini is used to draw a small version of the main fractal onto
        // pnlMainFractal inside the paint algorithm.
        private Bitmap? backgroundMini;

        // Stores all of the state data of the Dll for the main fractal
        // while in a julia mode so that the state of the Dll can be reset when
        // coming back to the main fractal.
        private FractalState fractalStateBackup = new FractalState();

        // LauncherForm handle
        private LauncherForm? parent;
        private FractalDisplayForm? myself;

        // dirtyIterations set to true when we come back to main
        // fractal mode 0 from a julia variety !0.
        private bool dirtyIterations = false;

        // Calculation mode:
        // 0 = main fractal, 1 = julia set, 2 = TheCalcmasterTwist, 3 = AirOnAJuliaString
        private int mode = 0;

        // FractalDisplayForm client area height and width
        private Rectangle bounds;
        // Last mouse click x,y
        int lastClickX = 0;
        int lastClickY = 0;

        // Video Bitmap Series Export
        private double fourCircleRadius = 5.0;
        private bool cancelVideo = false;

        #endregion Private properties

        // ***************************************
        // ***** Initialization - runs once ******
        // ***************************************

        #region Initialization - runs once

        /// <summary>
        /// Constructor
        /// </summary>
        public FractalDisplayForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// UI setup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FractalDisplayForm_Load(object sender, EventArgs e)
        {
            // Palette half cycle value - initial value
            cbHalfCycleValue.SelectedIndex = 1; // 20
            cbHalfCycleValue.SelectedIndexChanged += cbHalfCycleValue_SelectedIndexChanged;
            myself = this;
            for (int i = 28; i <= 167; i++)
                cbStartColor.Items.Add(((KnownColor)i).ToString());
        }

        /// <summary>
        /// Called by the LauncherForm
        /// </summary>
        /// <param name="fractalFormulaID">fractals.json algorithm id</param>
        /// <param name="f">pointer to the LauncherForm instance</param>
        public void FractalStart(FractalFormula ff, LauncherForm f)
        {
            parent = f;
            fractalFormula = ff;
            // Rectangle from which we can get the full screen width and height
            bounds = Screen.FromControl(this).Bounds;
            gen.SelectFractalFormula(fractalFormula.id);
            UpdateBitmap(forceCalc: true);
            if (background != null)
            {
                // Copy the main fractal to a smaller sized bitmap that fits just right on the pnlMainFractal panel.
                backgroundMini = new Bitmap(416, 234);
                Graphics.FromImage(backgroundMini).DrawImage(background, 0, 0, 416, 234);

            }
            // Mouse wheel increments or decrements the halfCycle value
            // that is used in the UpdateRandomColors() function.
            // It's somewhat similar to changing the "contrast" of the image.
            this.MouseWheel += MouseWheelHandler;
            infoPanel.Visible = true;
        }

        #endregion Initialization - runs once


        // ***************************************
        // ******** PRIVATE FUNCTIONS ************
        // ***************************************

        #region Private Functions

        // update the values shown on the infoForm pannel
        private void UpdateInfoPanel()
        {
            lbFiFormulaName.Text = fractalFormula.name;
            lbFiHalfCycleValue.Text = (string)(cbHalfCycleValue.SelectedItem ?? "");
            lbFiColorPalette.Text = gen.GetPalette().ToString();
            lbFiFilterStart.Text = gen.GetFilterStart().ToString();
            lbFiFilterEnd.Text = gen.GetFilterEnd().ToString();
            lbFiGhasItsListCount.Text = gen.G_hasItsList.Count.ToString();
            FractalState fs = gen.GetFractalState();
            switch (mode)
            {
                case 0:
                    lbFiCurrentMode.Text = "Map";
                    break;
                case 1:
                    lbFiCurrentMode.Text = "Julia Set";
                    break;
                case 2:
                    lbFiCurrentMode.Text = "The Calcmaster Twist";
                    break;
                case 3:
                    lbFiCurrentMode.Text = "Air On A Julia String";
                    break;
            }

            // viewing main fractal
            if (mode == 0)
            {
                // MaIn FrAcTaL ZoOm
                double mainZoom = fractalFormula.radius / fs.radius;
                lbFiMainFractalZoom.Text = $"{mainZoom}X";
                // JuLiA ZoOm
                lbFiJuliaZoom.Text = "n/a";
                // Julia Set : Main Fractal zoom ratio
                lbFiZoomRatioJuliaMain.Text = "n/a";
                // Distance Between Pixels Main
                lbFiIncMain.Text = fs.inc.ToString();
                // Distance Between Pixels Julia
                lbFiIncJulia.Text = "n/a";
                // Main Viewport Center Coordinates
                lbFiMainViewCenter.Text = $"{fs.centerX},i{fs.centerY}";
                // Julia Set Viewport Center Coordinates
                lbFiJuliaViewCenter.Text = "n/a";
                // Julia Calculation P (the point that was shift-clicked)
                lbFiJuliaCenter.Text = "n/a";
            }

            // viewing a julia set type
            else
            {
                // MaIn FrAcTaL ZoOm (a constant in this state)
                double mainZoom = fractalFormula.radius / fractalStateBackup.radius;
                lbFiMainFractalZoom.Text = $"{mainZoom}X";
                // JuLiA ZoOm
                double juliaZoom = fractalFormula.radius / fs.radius;
                lbFiJuliaZoom.Text = $"{juliaZoom}X";
                // Julia Set : Main Fractal zoom ratio
                double julia2MainZoomRatio = fs.radius / fractalStateBackup.radius;
                lbFiZoomRatioJuliaMain.Text = julia2MainZoomRatio.ToString();
                // Distance Between Pixels Main
                lbFiIncMain.Text = fractalStateBackup.inc.ToString();
                // Distance Between Pixels Julia
                lbFiIncJulia.Text = fs.inc.ToString();
                // Main Viewport Center Coordinates
                lbFiMainViewCenter.Text = $"{fractalStateBackup.centerX},i{fractalStateBackup.centerY}";
                // Julia Set Viewport Center Coordinates
                lbFiJuliaViewCenter.Text = $"({fs.centerX},i{fs.centerY})";
                // Julia Calculation P (the point that was shift-clicked)
                lbFiJuliaCenter.Text = $"({fs.juliaCenterX},i{fs.juliaCenterY})";
            }
        }

        // Update and refresh the image that is painted on the form's surface.
        private void UpdateBitmap(bool forceCalc = false)
        {
            int err = 0;
            if (dirtyIterations || forceCalc)
            {
                err = gen.CalculateMap();
                if (err == 0)
                {
                    dirtyIterations = false;
                    background = gen.LastBitmap;
                    UpdateInfoPanel();
                    this.Refresh();
                }
                return;
            }
            background = gen.BitmapFromIterations();
            UpdateInfoPanel();
            this.Refresh();
        }

        // send the selected color palette algorithm to Fractal class
        // and reclacluate the palette.  Then apply the new colors to the image.
        private void UpdateRandomPalette(Fractal.ColorPalette pal)
        {
            if (pal.Equals(gen.GetPalette())) return;

            gen.SetPalette(pal);
            UpdateBitmap();
        }

        #endregion Private Functions

        // ***************************************
        // ********** MAKE VIDEOS ****************
        // ***************************************

        #region Make Videos

        // Make a Circle video
        // Capture bitmaps of julia sets with centers located on
        // the edge of a circle with radius "fourCircleRadius".
        private void MakeVideoCircle(bool preview)
        {
            // Not applicable to main fractal viewing mode
            if (mode == 0) return;
            // Create a folder for the series
            string foldername = $"{Environment.CurrentDirectory}\\{fractalFormula.name}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
            if (!preview) Directory.CreateDirectory(foldername);

            // Back up the current state of the julia set so we can return to it when done.
            FractalState juliaBackup = gen.GetFractalState();
            double radius = fourCircleRadius * fractalStateBackup.inc;
            Int32 totalFrames = 360 * 4;
            double angleInc = 2 * Math.PI / totalFrames;
            // Loop de loop
            for (int i = 0; i < totalFrames; i++)
            {
                if (cancelVideo)
                {
                    // Video Cancelled, so reset the julia set back to where it was.
                    gen.SetJuliaCenter(juliaBackup.juliaCenterX, juliaBackup.juliaCenterY);
                    gen.CalculateMap();
                    if (myself != null)
                        myself.Invoke((Action)(() =>
                        {
                            UpdateBitmap();
                        }));
                    break;
                }
                double curJuliaCenterX = Math.Cos(i * angleInc) * radius + juliaBackup.juliaCenterX;
                double curJuliaCenterY = Math.Sin(i * angleInc) * radius + juliaBackup.juliaCenterY;
                // Calculate the iterations and refresh the image on the main thread:
                gen.SetJuliaCenter(curJuliaCenterX, curJuliaCenterY);
                int err = gen.CalculateMap();
                if (myself != null && err == 0)
                    myself.Invoke((Action)(() =>
                    {
                        lbFiSequenceImageNo.Text = i.ToString();
                        double percentComplete = 100 * i / totalFrames;
                        lbFiSequencePercent.Text = $"{percentComplete}%";
                        UpdateBitmap();
                    }));

                // save the bitmap to disk
                string filename = $"image_{i.ToString().PadLeft(totalWidth: 5, paddingChar: '0')}.png";
                if (!preview && err == 0 && gen.LastBitmap != null) gen.LastBitmap.Save(filename: $"{foldername}\\{filename}");
            }
            if (cancelVideo)
            {
                MessageBox.Show("Video Sequence Aborted.");
                cancelVideo = false;
            }
            else
                MessageBox.Show("Video Sequence Finished.");
        }


        // Make a 4 circle video
        // Capture bitmaps of 4 circles, with centers at radius distance from
        // current julia set constant P.
        // (right up left down) from the julia center coords.
        // A bitmap is calculated for every 1 degree around each circle.
        // Radius should be 20 * inc at the current zoom
        private void MakeVideo4Circles(bool preview)
        {
            // Not applicable to main fractal viewing mode
            if (mode == 0) return;
            //            await Task.Factory.StartNew(() =>
            //            {

            // Create a folder for the series
            string foldername = $"{Environment.CurrentDirectory}\\{fractalFormula.name}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
            if (!preview) Directory.CreateDirectory(foldername);

            // Back up the current state of the julia set so we can return to it when done.
            FractalState juliaBackup = gen.GetFractalState();
            double radius = fourCircleRadius * fractalStateBackup.inc;
            Int32 fileNumber = 1;
            Int32 totalFrames = 360 * 4;

            // 4 Circles - centers at right, up, left, and down
            for (int circleNumber = 0; circleNumber < 4; circleNumber++)
            {
                double circleCenterAngle = Math.PI * (double)circleNumber / 2.0;
                double curAngle = circleCenterAngle + Math.PI;
                double angleInc = 2 * Math.PI / 360.0;
                double circleCenterX = Math.Cos(circleCenterAngle) * radius + juliaBackup.juliaCenterX;
                double circleCenterY = Math.Sin(circleCenterAngle) * radius + juliaBackup.juliaCenterY;

                // Generate bitmaps for the current circle
                for (int angle = 0; angle < 360; angle++)
                {
                    if (cancelVideo)
                    {
                        // Video Cancelled, so reset the julia set back to where it was.
                        gen.SetJuliaCenter(juliaBackup.juliaCenterX, juliaBackup.juliaCenterY);
                        gen.CalculateMap();
                        if (myself != null)
                            myself.Invoke((Action)(() =>
                            {
                                UpdateBitmap();
                            }));
                        break;
                    }
                    double curJuliaCenterX = Math.Cos(curAngle) * radius + circleCenterX;
                    double curJuliaCenterY = Math.Sin(curAngle) * radius + circleCenterY;

                    // Calculate the iterations and refresh the image on the main thread:
                    gen.SetJuliaCenter(curJuliaCenterX, curJuliaCenterY);
                    int err = gen.CalculateMap();
                    if (myself != null && err == 0)
                        myself.Invoke((Action)(() =>
                        {
                            lbFiSequenceImageNo.Text = fileNumber.ToString();
                            double percentComplete = 100 * fileNumber / totalFrames;
                            lbFiSequencePercent.Text = $"{percentComplete}%";
                            UpdateBitmap();
                        }));

                    // save the bitmap to disk
                    string filename = $"image_{fileNumber.ToString().PadLeft(totalWidth: 5, paddingChar: '0')}.png";
                    if (!preview && err == 0 && gen.LastBitmap != null) gen.LastBitmap.Save(filename: $"{foldername}\\{filename}");

                    // get ready for another cycle
                    fileNumber++;
                    curAngle += angleInc;
                }
            }
            //});
            if (cancelVideo)
            {
                MessageBox.Show("Video Sequence Aborted.");
                cancelVideo = false;
            }
            else
                MessageBox.Show("Video Sequence Finished.");
        }

        #endregion Make Videos

        // ***************************************
        // ********** KEYBOARD EVENTS ************
        // ***************************************

        #region Keyboard events

        /// <summary>
        /// Handles key-down events (not to be confused with KeyPress).<br />
        /// ESC:    Exits julia mode, otherwise closes the form.<br />
        /// R:      Recalculate random color array.<br />
        /// -:      Zoom out from the center of the fractal.<br />
        /// Arrows: Used to move viewport UP, DOWN, LEFT, or RIGHT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">e.KeyCode holds the key that went down</param>
        private void FractalDisplayForm_KeyDown(object sender, KeyEventArgs e)
        {
            int err = 0;

            // ESC
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                if (mode == 0)
                {
                    this.Close();
                    return;
                }
                else
                {
                    mode = 0;
                    // set dirtyIterations to true so that any further operations on
                    // the main fractal such as color palette modifications
                    // will recalculate the iterations.
                    dirtyIterations = true;
                    gen.SetMode(0, 0, 0);
                    if (backgroundBackup != null)
                    {
                        // Don't recalculate main if we're coming back from a julia mode
                        background = backgroundBackup;
                        UpdateInfoPanel();
                        this.Refresh();
                    }
                    else
                    {
                        UpdateBitmap(forceCalc: true);
                    }
                    return;
                }
            }

            // R    (Picks a new random starting color and updates image using a new palette)
            if (e.KeyCode == Keys.R)
            {
                e.Handled = true;
                gen.ResetStartEndColors();
                gen.UpdateRandomColors();
                UpdateBitmap();
            }

            // -    Zoom Out
            if (e.KeyCode == Keys.Subtract)
            {
                e.Handled = true;
                err = gen.ZoomOut();
                UpdateBitmap();
            }

            // Arw  Move
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        err = gen.Move(FractalInterface.Direction.UP);
                        break;
                    case Keys.Down:
                        err = gen.Move(FractalInterface.Direction.DOWN);
                        break;
                    case Keys.Left:
                        err = gen.Move(FractalInterface.Direction.LEFT);
                        break;
                    case Keys.Right:
                        err = gen.Move(FractalInterface.Direction.RIGHT);
                        break;
                }
                UpdateBitmap();
            }

            // V    Toggles bitmap color inVersion
            if (e.KeyCode == Keys.V)
            {
                gen.InverseToggle = !gen.InverseToggle;
                UpdateBitmap();
            }

            // I    Toggles the information panel
            if (e.KeyCode == Keys.I)
            {
                infoPanel.Visible = !infoPanel.Visible;
            }

            // A    Antialiasing Algorithm
            if (e.KeyCode == Keys.A)
            {
                switch (gen.CurAntiAliasAlg)
                {
                    case Fractal.AntiAliasAlg.NoModification:
                        gen.CurAntiAliasAlg = Fractal.AntiAliasAlg.OneAway;
                        break;
                    case Fractal.AntiAliasAlg.OneAway:
                        gen.CurAntiAliasAlg = Fractal.AntiAliasAlg.TwoAway;
                        break;
                    case Fractal.AntiAliasAlg.TwoAway:
                        gen.CurAntiAliasAlg = Fractal.AntiAliasAlg.NoModification;
                        break;
                }
            }

            // F    Sets filterEnd to whatever the current maximum number of iterations is for a pixel
            if (e.KeyCode == Keys.F)
            {
                gen.SetFilterEndToCurrentMaxIts();
                UpdateBitmap();
            }

            // C    Aborts video creation loop
            if (e.KeyCode == Keys.C)
            {
                cancelVideo = true;
            }

            //L     Opens the Limit adjustment panel
            if (e.KeyCode == Keys.L)
            {
                // Manual adjustment of Escape Limit
                tbLimit.Text = gen.GetLimit().ToString();
                tbLimit.Enabled = true;
                // 4 Circles (radius of each) for bitmap series export
                tb4CircleRadius.Text = fourCircleRadius.ToString();
                tb4CircleRadius.Enabled = true;
                // Manual adjustment of MaxIterations
                tbMaxIterations.Text = gen.MaxIterations.ToString();
                tbMaxIterations.Enabled = true;
                // now show the panel
                limitPanel.Visible = true;
            }
        }

        private void FractalDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (gen != null) gen.Dispose();
        }

        #endregion Keyboard events

        // ***************************************
        // ********** PAINT EVENTS ***************
        // ***************************************

        #region Paint events

        /// <summary>
        /// Fills the form's client area surface with the fractal bitmap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FractalDisplayForm_Paint(object sender, PaintEventArgs e)
        {
            if (background != null)
            {
                /* Anti-alias the bitmap... (this is slow and blurs the image)
                //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                Rectangle srcRect = new Rectangle(0, 0, bounds.Width, bounds.Height);
                Rectangle destRect = new Rectangle(0, 0, bounds.Width * 4, bounds.Height * 4);
                Bitmap tempB = new Bitmap(background.Width * 4, background.Height * 4);
                Graphics tempG = Graphics.FromImage(tempB);
                tempG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                tempG.DrawImage(background, destRect);
                e.Graphics.DrawImage(tempB, srcRect);
                */

                // FractalDisplayForm draw background image:
                // No anti-alias .. fast, but curves can be grainy at certain angles
                e.Graphics.DrawImageUnscaled(background, 0, 0);

            }
        }

        // Draws the Mini-Map current location with a yellow circle
        private void pnlMainFractal_Paint(object sender, PaintEventArgs e)
        {
            if (backgroundMini != null)
            {
                FractalState fstemp = gen.GetFractalState();
                double inc = fractalFormula.radius * 2 / backgroundMini.Height;
                int pixelX = Convert.ToInt32(backgroundMini.Width / 2 + ((mode == 0 ? fstemp.centerX : fstemp.juliaCenterX) - fractalFormula.centerX) / inc);
                int pixelY = Convert.ToInt32(backgroundMini.Height / 2 + (fractalFormula.centerY - (mode == 0 ? fstemp.centerY : fstemp.juliaCenterY)) / inc);
                e.Graphics.DrawImage(backgroundMini, 0, 0);
                e.Graphics.DrawArc(new Pen(Color.Yellow, 1), new Rectangle(pixelX - 5, pixelY - 5, 10, 10), 0, 360);
            }
        }

        #endregion Paint events

        // ***************************************
        // ********** MOUSE EVENTS ***************
        // ***************************************

        #region Mouse Events

        /// <summary>
        /// Handles mouse-click events in combination with ModifierKeys.<br />
        /// Click:          Zoom in, centering on the pixel that was clicked.<br />
        /// Shift-Click:    Julia Set mode -> 1<br />
        /// Ctrl-Click:     TheCalcmasterTwist -> 2<br />
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">e.X and e.Y hold the coordinates of the pixel that was clicked upon</param>
        private void FractalDisplayForm_MouseClick(object sender, MouseEventArgs e)
        {
            lastClickX = e.X;
            lastClickY = e.Y;

            int err = 0;
            if (mode == 0 && Control.ModifierKeys == Keys.Shift)
            {
                // make a backup of the main fractal data
                fractalStateBackup = gen.GetFractalState();
                if (background != null)
                {
                    backgroundBackup = background;
                }

                // generate the julia set
                mode = 1;
                gen.SetMode(1, e.X, e.Y);
                UpdateBitmap(forceCalc: true);
                return;
            }
            if (mode == 0 && Control.ModifierKeys == Keys.Control)
            {
                // make a backup of the main fractal data
                fractalStateBackup = gen.GetFractalState();
                if (background != null)
                {
                    backgroundBackup = background;
                }

                // generate TheCalcmasterTwist modified julia set
                mode = 2;
                gen.SetMode(2, e.X, e.Y);
                UpdateBitmap(forceCalc: true);
                return;
            }
            err = gen.ZoomInAtPoint(e.X, e.Y);
            UpdateBitmap();
        }

        /// <summary>
        /// The mouse wheel increases or decreases the Palette Half Cycle Value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseWheelHandler(object? sender, MouseEventArgs e)
        {
            // Palette Half Cycle Value Series
            // (number of colors between darkest and lightest versions of a single color in the palette.)
            // 10 20 30 40 50 60 70 80 90 100 125 150 175 200 250 300 350 400 450 500 550 600 650 700 750 800 850 900 950 1000

            if (e.Delta > 0)
            {
                // Alt+Shift+Wheel raise the start filter value by 10
                if (Control.ModifierKeys == (Keys.Alt | Keys.Shift))
                {
                    gen.IncFilterStart(10);
                    UpdateBitmap();
                    return;
                }
                // Shift+Wheel rase the start filter value by 1
                if (Control.ModifierKeys == Keys.Shift)
                {
                    gen.IncFilterStart(1);
                    UpdateBitmap();
                    return;
                }
                // Alt+Ctrl+Wheel raise the end filter value by 25
                if (Control.ModifierKeys == (Keys.Alt | Keys.Control))
                {
                    gen.IncFilterEnd(25);
                    UpdateBitmap();
                    return;
                }
                // Ctrl+Wheel raise the end filter value by 1
                if (Control.ModifierKeys == Keys.Control)
                {
                    gen.IncFilterEnd(1);
                    UpdateBitmap();
                    return;
                }
                // otherwise Raise the Palette Half Cycle Value
                if (cbHalfCycleValue.SelectedIndex < cbHalfCycleValue.Items.Count - 1)
                {
                    cbHalfCycleValue.SelectedIndex++;
                }
            }
            else
            {
                // Alt+Shift+Wheel = lower the start filter value by 10
                if (Control.ModifierKeys == (Keys.Alt | Keys.Shift))
                {
                    gen.DecFilterStart(10);
                    UpdateBitmap();
                    return;
                }
                // Shift+Wheel = lower the start filter value by 1
                if (Control.ModifierKeys == Keys.Shift)
                {
                    gen.DecFilterStart(1);
                    UpdateBitmap();
                    return;
                }
                // Alt+Ctrl+Wheel lower the end filter value by 25
                if (Control.ModifierKeys == (Keys.Alt | Keys.Control))
                {
                    gen.DecFilterEnd(25);
                    UpdateBitmap();
                    return;
                }
                // Ctrl+Wheel lower the end filter value by 1
                if (Control.ModifierKeys == Keys.Control)
                {
                    gen.DecFilterEnd(1);
                    UpdateBitmap();
                    return;
                }
                // otherwise Lower the Palette Half Cycle Value
                if (cbHalfCycleValue.SelectedIndex > 0)
                {
                    cbHalfCycleValue.SelectedIndex--;
                }
            }
        }

        #endregion Mouse Events

        // ***************************************
        // ******* CONTEXT MENU EVENTS ***********
        // ***************************************

        #region Context menu events

        // recalculate palette using sinusoidal grayscale
        private void palGrayscale_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.Grayscale);
        }

        // recalculate palette using a single color
        private void palRandomMono_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.RandomMono);
        }

        // recalculate palette using 2 complimentary colors
        private void palRandomCompliment_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.RandomCompliment);
        }

        // recalculate palette using 3 triad colors
        private void palRandomTriad_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.RandomTriad);
        }

        // recalculate palette using 4 tetrad colors
        private void palRandomTetrad_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.RandomTetrad);
        }

        // recalculate palette using Hue angle starting at m_startColor
        private void palRainbow_Click(object sender, EventArgs e)
        {
            UpdateRandomPalette(Fractal.ColorPalette.Rainbow);
        }


        // Context menu selection of palette half cycle value
        private void cbHalfCycleValue_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int halfCycleValue = Int32.Parse(cbHalfCycleValue.SelectedItem == null ? "20" : cbHalfCycleValue.SelectedItem.ToString() ?? "20");
            // Update the pallete
            gen.SetHalfCycleValue(halfCycleValue);
            // Show the updated bitmap
            UpdateBitmap();
        }

        // Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void fourCircleSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mode == 0) return;
            await Task.Factory.StartNew(() =>
            {
                MakeVideo4Circles(preview: true);
            });
        }

        private async void saveAs4CircleSeriesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (mode == 0) return;
            await Task.Factory.StartNew(() =>
            {
                MakeVideo4Circles(preview: false);
            });
        }

        private async void menuCircleSeriesPreview_Click(object sender, EventArgs e)
        {
            if (mode == 0) return;
            await Task.Factory.StartNew(() =>
            {
                MakeVideoCircle(preview: true);
            });
        }

        private async void menuCircleSeriesExport_Click(object sender, EventArgs e)
        {
            if (mode == 0) return;
            await Task.Factory.StartNew(() =>
            {
                MakeVideoCircle(preview: false);
            });
        }



        #endregion Context menu events

        // ***************************************
        // ********** INFOPANEL EVENTS ***********
        // ***************************************

        #region InfoPanel Events

        // Reset the fractal to the original size and position as described in fractals.json parameters
        private void btnResetFractal_Click(object sender, EventArgs e)
        {
            gen.SelectFractalFormula(fractalFormula.id);
            UpdateBitmap(forceCalc: true);
        }

        // Resets iteration filter boundaries back to no-filter
        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            gen.ResetFilterRange();
            UpdateBitmap();
        }

        #endregion  InfoPanel Events

        // Set the start color to a named color from the Starting Color pick list
        private void cbStartColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStartColor.SelectedIndex == -1) return;
            gen.SetStartColor(cbStartColor.SelectedItem.ToString());
            UpdateBitmap();
        }

        private void btnCloseLimitPanel_Click(object sender, EventArgs e)
        {
            tbLimit.Enabled = false;
            tb4CircleRadius.Enabled = false;
            tbMaxIterations.Enabled = false;
            limitPanel.Visible = false;
            this.Refresh();
        }

        private void btnSetLimit_Click(object sender, EventArgs e)
        {
            double limit = fractalFormula.limit;
            bool ok = double.TryParse(tbLimit.Text, out limit);
            if (ok) gen.SetLimit(limit);
            UpdateBitmap(forceCalc: true);
        }

        private void btnSet4CircleRadius_Click(object sender, EventArgs e)
        {
            double radius = 5.0;
            bool ok = double.TryParse(tb4CircleRadius.Text, out radius);
            if (ok) fourCircleRadius = radius;
        }

        private void btnSetMaxIterations_Click(object sender, EventArgs e)
        {
            int its = gen.MaxIterations;
            bool ok = int.TryParse(tbMaxIterations.Text, out its);
            if (ok && its > 0 && its < 5000) gen.MaxIterations = its;
            UpdateBitmap();
        }

    }
}
