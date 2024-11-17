namespace CalcmasterFractal
{
    partial class FractalDisplayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            grayscaleToolStripMenuItem = new ToolStripMenuItem();
            palRandomMono = new ToolStripMenuItem();
            palRandomCompliment = new ToolStripMenuItem();
            palRandomTriad = new ToolStripMenuItem();
            palRandomTetrad = new ToolStripMenuItem();
            rainbowToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            cbStartColor = new ToolStripComboBox();
            halfCycleMenuItem = new ToolStripMenuItem();
            cbHalfCycleValue = new ToolStripComboBox();
            generateNewRandomColorsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            fourCircleSeriesToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            saveAs4CircleSeriesToolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            infoPanel = new DoubleBufferedPanel();
            lbFiGhasItsListCount = new Label();
            label21 = new Label();
            label19 = new Label();
            label18 = new Label();
            lbFiColorPalette = new Label();
            label17 = new Label();
            lbFiJuliaCenter = new Label();
            label16 = new Label();
            lbFiHalfCycleValue = new Label();
            label11 = new Label();
            btnResetFractal = new DoubleBufferedLabel();
            btnResetFilter = new DoubleBufferedLabel();
            pnlMainFractal = new DoubleBufferedPanel();
            lbFiFilterEnd = new Label();
            lbFiFilterStart = new Label();
            label13 = new Label();
            label12 = new Label();
            lbFiIncJulia = new Label();
            label15 = new Label();
            lbFiIncMain = new Label();
            label14 = new Label();
            lbFiZoomRatioJuliaMain = new Label();
            label9 = new Label();
            lbFiSequencePercent = new Label();
            lbFiMainViewCenter = new Label();
            label8 = new Label();
            lbFiJuliaViewCenter = new Label();
            label7 = new Label();
            lbFiCurrentMode = new Label();
            label6 = new Label();
            lbFiFormulaName = new Label();
            label5 = new Label();
            lbFiSequenceImageNo = new Label();
            label4 = new Label();
            label3 = new Label();
            lbFiJuliaZoom = new Label();
            label2 = new Label();
            lbFiMainFractalZoom = new Label();
            label1 = new Label();
            label10 = new Label();
            limitPanel = new DoubleBufferedPanel();
            btnSetMaxIterations = new DoubleBufferedLabel();
            label23 = new Label();
            tbMaxIterations = new TextBox();
            btnSet4CircleRadius = new DoubleBufferedLabel();
            label22 = new Label();
            tb4CircleRadius = new TextBox();
            doubleBufferedPanel1 = new DoubleBufferedPanel();
            btnCloseLimitPanel = new DoubleBufferedLabel();
            label20 = new Label();
            tbLimit = new TextBox();
            btnSetLimit = new DoubleBufferedLabel();
            menuCircleSeriesPreview = new ToolStripMenuItem();
            menuCircleSeriesExport = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            infoPanel.SuspendLayout();
            limitPanel.SuspendLayout();
            doubleBufferedPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem4, halfCycleMenuItem, generateNewRandomColorsToolStripMenuItem, toolStripMenuItem2, toolStripMenuItem3, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(234, 180);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { grayscaleToolStripMenuItem, palRandomMono, palRandomCompliment, palRandomTriad, palRandomTetrad, rainbowToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(233, 22);
            toolStripMenuItem1.Text = "Color Algorithm";
            // 
            // grayscaleToolStripMenuItem
            // 
            grayscaleToolStripMenuItem.Name = "grayscaleToolStripMenuItem";
            grayscaleToolStripMenuItem.Size = new Size(146, 22);
            grayscaleToolStripMenuItem.Text = "Grayscale";
            grayscaleToolStripMenuItem.Click += palGrayscale_Click;
            // 
            // palRandomMono
            // 
            palRandomMono.Name = "palRandomMono";
            palRandomMono.Size = new Size(146, 22);
            palRandomMono.Text = "Monocolor";
            palRandomMono.Click += palRandomMono_Click;
            // 
            // palRandomCompliment
            // 
            palRandomCompliment.Name = "palRandomCompliment";
            palRandomCompliment.Size = new Size(146, 22);
            palRandomCompliment.Text = "Compliments";
            palRandomCompliment.Click += palRandomCompliment_Click;
            // 
            // palRandomTriad
            // 
            palRandomTriad.Name = "palRandomTriad";
            palRandomTriad.Size = new Size(146, 22);
            palRandomTriad.Text = "Triad";
            palRandomTriad.Click += palRandomTriad_Click;
            // 
            // palRandomTetrad
            // 
            palRandomTetrad.Name = "palRandomTetrad";
            palRandomTetrad.Size = new Size(146, 22);
            palRandomTetrad.Text = "Tetrad";
            palRandomTetrad.Click += palRandomTetrad_Click;
            // 
            // rainbowToolStripMenuItem
            // 
            rainbowToolStripMenuItem.Name = "rainbowToolStripMenuItem";
            rainbowToolStripMenuItem.Size = new Size(146, 22);
            rainbowToolStripMenuItem.Text = "Rainbow";
            rainbowToolStripMenuItem.Click += palRainbow_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.DropDownItems.AddRange(new ToolStripItem[] { cbStartColor });
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(233, 22);
            toolStripMenuItem4.Text = "Starting Color";
            // 
            // cbStartColor
            // 
            cbStartColor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStartColor.Name = "cbStartColor";
            cbStartColor.Size = new Size(121, 23);
            cbStartColor.SelectedIndexChanged += cbStartColor_SelectedIndexChanged;
            // 
            // halfCycleMenuItem
            // 
            halfCycleMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cbHalfCycleValue });
            halfCycleMenuItem.Name = "halfCycleMenuItem";
            halfCycleMenuItem.Size = new Size(233, 22);
            halfCycleMenuItem.Text = "Palette Half Cycle Value";
            // 
            // cbHalfCycleValue
            // 
            cbHalfCycleValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cbHalfCycleValue.Items.AddRange(new object[] { "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "125", "150", "175", "200", "250", "300", "350", "400", "450", "500", "550", "600", "650", "700", "750", "800", "850", "900", "950", "1000" });
            cbHalfCycleValue.Name = "cbHalfCycleValue";
            cbHalfCycleValue.Size = new Size(121, 23);
            // 
            // generateNewRandomColorsToolStripMenuItem
            // 
            generateNewRandomColorsToolStripMenuItem.Name = "generateNewRandomColorsToolStripMenuItem";
            generateNewRandomColorsToolStripMenuItem.Size = new Size(233, 22);
            generateNewRandomColorsToolStripMenuItem.Text = "Generate New Random Colors";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { fourCircleSeriesToolStripMenuItem, menuCircleSeriesPreview });
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(233, 22);
            toolStripMenuItem2.Text = "Preview Video Bitmaps";
            // 
            // fourCircleSeriesToolStripMenuItem
            // 
            fourCircleSeriesToolStripMenuItem.Name = "fourCircleSeriesToolStripMenuItem";
            fourCircleSeriesToolStripMenuItem.Size = new Size(180, 22);
            fourCircleSeriesToolStripMenuItem.Text = "Four Circle Series";
            fourCircleSeriesToolStripMenuItem.Click += fourCircleSeriesToolStripMenuItem_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.DropDownItems.AddRange(new ToolStripItem[] { saveAs4CircleSeriesToolStripMenuItem1, menuCircleSeriesExport });
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(233, 22);
            toolStripMenuItem3.Text = "Export Video Bitmaps";
            // 
            // saveAs4CircleSeriesToolStripMenuItem1
            // 
            saveAs4CircleSeriesToolStripMenuItem1.Name = "saveAs4CircleSeriesToolStripMenuItem1";
            saveAs4CircleSeriesToolStripMenuItem1.Size = new Size(201, 22);
            saveAs4CircleSeriesToolStripMenuItem1.Text = "Export Four Circle Series";
            saveAs4CircleSeriesToolStripMenuItem1.Click += saveAs4CircleSeriesToolStripMenuItem1_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(233, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // infoPanel
            // 
            infoPanel.BackColor = SystemColors.ControlDarkDark;
            infoPanel.Controls.Add(lbFiGhasItsListCount);
            infoPanel.Controls.Add(label21);
            infoPanel.Controls.Add(label19);
            infoPanel.Controls.Add(label18);
            infoPanel.Controls.Add(lbFiColorPalette);
            infoPanel.Controls.Add(label17);
            infoPanel.Controls.Add(lbFiJuliaCenter);
            infoPanel.Controls.Add(label16);
            infoPanel.Controls.Add(lbFiHalfCycleValue);
            infoPanel.Controls.Add(label11);
            infoPanel.Controls.Add(btnResetFractal);
            infoPanel.Controls.Add(btnResetFilter);
            infoPanel.Controls.Add(pnlMainFractal);
            infoPanel.Controls.Add(lbFiFilterEnd);
            infoPanel.Controls.Add(lbFiFilterStart);
            infoPanel.Controls.Add(label13);
            infoPanel.Controls.Add(label12);
            infoPanel.Controls.Add(lbFiIncJulia);
            infoPanel.Controls.Add(label15);
            infoPanel.Controls.Add(lbFiIncMain);
            infoPanel.Controls.Add(label14);
            infoPanel.Controls.Add(lbFiZoomRatioJuliaMain);
            infoPanel.Controls.Add(label9);
            infoPanel.Controls.Add(lbFiSequencePercent);
            infoPanel.Controls.Add(lbFiMainViewCenter);
            infoPanel.Controls.Add(label8);
            infoPanel.Controls.Add(lbFiJuliaViewCenter);
            infoPanel.Controls.Add(label7);
            infoPanel.Controls.Add(lbFiCurrentMode);
            infoPanel.Controls.Add(label6);
            infoPanel.Controls.Add(lbFiFormulaName);
            infoPanel.Controls.Add(label5);
            infoPanel.Controls.Add(lbFiSequenceImageNo);
            infoPanel.Controls.Add(label4);
            infoPanel.Controls.Add(label3);
            infoPanel.Controls.Add(lbFiJuliaZoom);
            infoPanel.Controls.Add(label2);
            infoPanel.Controls.Add(lbFiMainFractalZoom);
            infoPanel.Controls.Add(label1);
            infoPanel.Controls.Add(label10);
            infoPanel.ForeColor = SystemColors.HighlightText;
            infoPanel.Location = new Point(12, 12);
            infoPanel.Name = "infoPanel";
            infoPanel.Size = new Size(422, 546);
            infoPanel.TabIndex = 1;
            infoPanel.Visible = false;
            // 
            // lbFiGhasItsListCount
            // 
            lbFiGhasItsListCount.AutoSize = true;
            lbFiGhasItsListCount.Location = new Point(142, 91);
            lbFiGhasItsListCount.Name = "lbFiGhasItsListCount";
            lbFiGhasItsListCount.Size = new Size(115, 15);
            lbFiGhasItsListCount.TabIndex = 75;
            lbFiGhasItsListCount.Text = "lbFiGhasItsListCount";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(24, 91);
            label21.Name = "label21";
            label21.Size = new Size(111, 15);
            label21.TabIndex = 74;
            label21.Text = "Current # of Colors:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(252, 263);
            label19.Name = "label19";
            label19.Size = new Size(111, 15);
            label19.TabIndex = 73;
            label19.Text = "(+/- [alt] ctrl wheel)";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(252, 248);
            label18.Name = "label18";
            label18.Size = new Size(117, 15);
            label18.TabIndex = 72;
            label18.Text = "(+/- [alt] shift wheel)";
            // 
            // lbFiColorPalette
            // 
            lbFiColorPalette.AutoSize = true;
            lbFiColorPalette.Location = new Point(142, 76);
            lbFiColorPalette.Name = "lbFiColorPalette";
            lbFiColorPalette.Size = new Size(91, 15);
            lbFiColorPalette.TabIndex = 71;
            lbFiColorPalette.Text = "lbFiColorPalette";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(0, 76);
            label17.Name = "label17";
            label17.Size = new Size(135, 15);
            label17.TabIndex = 70;
            label17.Text = "Color Palette Algorithm:";
            // 
            // lbFiJuliaCenter
            // 
            lbFiJuliaCenter.AutoSize = true;
            lbFiJuliaCenter.Location = new Point(141, 107);
            lbFiJuliaCenter.Name = "lbFiJuliaCenter";
            lbFiJuliaCenter.Size = new Size(84, 15);
            lbFiJuliaCenter.TabIndex = 39;
            lbFiJuliaCenter.Text = "lbFiJuliaCenter";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(252, 61);
            label16.Name = "label16";
            label16.Size = new Size(67, 15);
            label16.TabIndex = 69;
            label16.Text = "(+/- wheel)";
            // 
            // lbFiHalfCycleValue
            // 
            lbFiHalfCycleValue.AutoSize = true;
            lbFiHalfCycleValue.Location = new Point(142, 61);
            lbFiHalfCycleValue.Name = "lbFiHalfCycleValue";
            lbFiHalfCycleValue.Size = new Size(105, 15);
            lbFiHalfCycleValue.TabIndex = 68;
            lbFiHalfCycleValue.Text = "lbFiHalfCycleValue";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(14, 61);
            label11.Name = "label11";
            label11.Size = new Size(121, 15);
            label11.TabIndex = 67;
            label11.Text = "Palette Half Cycle Val:";
            // 
            // btnResetFractal
            // 
            btnResetFractal.BackColor = SystemColors.ButtonFace;
            btnResetFractal.BorderStyle = BorderStyle.Fixed3D;
            btnResetFractal.ForeColor = SystemColors.ControlText;
            btnResetFractal.Location = new Point(339, 34);
            btnResetFractal.Name = "btnResetFractal";
            btnResetFractal.Size = new Size(79, 23);
            btnResetFractal.TabIndex = 66;
            btnResetFractal.Text = "reset fractal";
            btnResetFractal.TextAlign = ContentAlignment.MiddleCenter;
            btnResetFractal.Click += btnResetFractal_Click;
            // 
            // btnResetFilter
            // 
            btnResetFilter.BackColor = SystemColors.ButtonFace;
            btnResetFilter.BorderStyle = BorderStyle.Fixed3D;
            btnResetFilter.ForeColor = SystemColors.ControlText;
            btnResetFilter.Location = new Point(339, 280);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(79, 23);
            btnResetFilter.TabIndex = 2;
            btnResetFilter.Text = "reset filter";
            btnResetFilter.TextAlign = ContentAlignment.MiddleCenter;
            btnResetFilter.Click += btnResetFilter_Click;
            // 
            // pnlMainFractal
            // 
            pnlMainFractal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pnlMainFractal.BackgroundImageLayout = ImageLayout.Stretch;
            pnlMainFractal.Enabled = false;
            pnlMainFractal.Location = new Point(3, 309);
            pnlMainFractal.Name = "pnlMainFractal";
            pnlMainFractal.Size = new Size(416, 234);
            pnlMainFractal.TabIndex = 65;
            pnlMainFractal.Paint += pnlMainFractal_Paint;
            // 
            // lbFiFilterEnd
            // 
            lbFiFilterEnd.AutoSize = true;
            lbFiFilterEnd.Location = new Point(142, 263);
            lbFiFilterEnd.Name = "lbFiFilterEnd";
            lbFiFilterEnd.Size = new Size(72, 15);
            lbFiFilterEnd.TabIndex = 62;
            lbFiFilterEnd.Text = "lbFiFilterEnd";
            // 
            // lbFiFilterStart
            // 
            lbFiFilterStart.AutoSize = true;
            lbFiFilterStart.Location = new Point(142, 248);
            lbFiFilterStart.Name = "lbFiFilterStart";
            lbFiFilterStart.Size = new Size(76, 15);
            lbFiFilterStart.TabIndex = 61;
            lbFiFilterStart.Text = "lbFiFilterStart";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(29, 263);
            label13.Name = "label13";
            label13.Size = new Size(106, 15);
            label13.TabIndex = 60;
            label13.Text = "Iteration Filter End:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(26, 248);
            label12.Name = "label12";
            label12.Size = new Size(110, 15);
            label12.TabIndex = 59;
            label12.Text = "Iteration Filter Start:";
            // 
            // lbFiIncJulia
            // 
            lbFiIncJulia.AutoSize = true;
            lbFiIncJulia.Location = new Point(141, 182);
            lbFiIncJulia.Name = "lbFiIncJulia";
            lbFiIncJulia.Size = new Size(65, 15);
            lbFiIncJulia.TabIndex = 58;
            lbFiIncJulia.Text = "lbFiIncJulia";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(21, 182);
            label15.Name = "label15";
            label15.Size = new Size(114, 15);
            label15.TabIndex = 57;
            label15.Text = "Between Pixels Julia:";
            // 
            // lbFiIncMain
            // 
            lbFiIncMain.AutoSize = true;
            lbFiIncMain.Location = new Point(141, 167);
            lbFiIncMain.Name = "lbFiIncMain";
            lbFiIncMain.Size = new Size(69, 15);
            lbFiIncMain.TabIndex = 56;
            lbFiIncMain.Text = "lbFiIncMain";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(17, 167);
            label14.Name = "label14";
            label14.Size = new Size(118, 15);
            label14.TabIndex = 55;
            label14.Text = "Between Pixels Main:";
            // 
            // lbFiZoomRatioJuliaMain
            // 
            lbFiZoomRatioJuliaMain.AutoSize = true;
            lbFiZoomRatioJuliaMain.Location = new Point(141, 152);
            lbFiZoomRatioJuliaMain.Name = "lbFiZoomRatioJuliaMain";
            lbFiZoomRatioJuliaMain.Size = new Size(135, 15);
            lbFiZoomRatioJuliaMain.TabIndex = 52;
            lbFiZoomRatioJuliaMain.Text = "lbFiZoomRatioJuliaMain";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(-1, 152);
            label9.Name = "label9";
            label9.Size = new Size(136, 15);
            label9.TabIndex = 51;
            label9.Text = "Zoom Ratio [Julia:Main]:";
            // 
            // lbFiSequencePercent
            // 
            lbFiSequencePercent.AutoSize = true;
            lbFiSequencePercent.Location = new Point(273, 230);
            lbFiSequencePercent.Name = "lbFiSequencePercent";
            lbFiSequencePercent.Size = new Size(117, 15);
            lbFiSequencePercent.TabIndex = 50;
            lbFiSequencePercent.Text = "lbFiSequencePercent";
            // 
            // lbFiMainViewCenter
            // 
            lbFiMainViewCenter.AutoSize = true;
            lbFiMainViewCenter.Location = new Point(141, 197);
            lbFiMainViewCenter.Name = "lbFiMainViewCenter";
            lbFiMainViewCenter.Size = new Size(113, 15);
            lbFiMainViewCenter.TabIndex = 49;
            lbFiMainViewCenter.Text = "lbFiMainViewCenter";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(32, 197);
            label8.Name = "label8";
            label8.Size = new Size(103, 15);
            label8.TabIndex = 48;
            label8.Text = "Main View Center:";
            // 
            // lbFiJuliaViewCenter
            // 
            lbFiJuliaViewCenter.AutoSize = true;
            lbFiJuliaViewCenter.Location = new Point(141, 212);
            lbFiJuliaViewCenter.Name = "lbFiJuliaViewCenter";
            lbFiJuliaViewCenter.Size = new Size(109, 15);
            lbFiJuliaViewCenter.TabIndex = 47;
            lbFiJuliaViewCenter.Text = "lbFiJuliaViewCenter";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(36, 212);
            label7.Name = "label7";
            label7.Size = new Size(99, 15);
            label7.TabIndex = 46;
            label7.Text = "Julia View Center:";
            // 
            // lbFiCurrentMode
            // 
            lbFiCurrentMode.AutoSize = true;
            lbFiCurrentMode.Location = new Point(141, 46);
            lbFiCurrentMode.Name = "lbFiCurrentMode";
            lbFiCurrentMode.Size = new Size(97, 15);
            lbFiCurrentMode.TabIndex = 45;
            lbFiCurrentMode.Text = "lbFiCurrentMode";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(51, 46);
            label6.Name = "label6";
            label6.Size = new Size(84, 15);
            label6.TabIndex = 44;
            label6.Text = "Current Mode:";
            // 
            // lbFiFormulaName
            // 
            lbFiFormulaName.AutoSize = true;
            lbFiFormulaName.Location = new Point(141, 31);
            lbFiFormulaName.Name = "lbFiFormulaName";
            lbFiFormulaName.Size = new Size(102, 15);
            lbFiFormulaName.TabIndex = 43;
            lbFiFormulaName.Text = "lbFiFormulaName";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(43, 31);
            label5.Name = "label5";
            label5.Size = new Size(92, 15);
            label5.TabIndex = 42;
            label5.Text = "Fractal Formula:";
            // 
            // lbFiSequenceImageNo
            // 
            lbFiSequenceImageNo.AutoSize = true;
            lbFiSequenceImageNo.Location = new Point(141, 230);
            lbFiSequenceImageNo.Name = "lbFiSequenceImageNo";
            lbFiSequenceImageNo.Size = new Size(126, 15);
            lbFiSequenceImageNo.TabIndex = 41;
            lbFiSequenceImageNo.Text = "lbFiSequenceImageNo";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 230);
            label4.Name = "label4";
            label4.Size = new Size(119, 15);
            label4.TabIndex = 40;
            label4.Text = "Sequence Image No.:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(51, 107);
            label3.Name = "label3";
            label3.Size = new Size(84, 15);
            label3.TabIndex = 38;
            label3.Text = "Julia Constant:";
            // 
            // lbFiJuliaZoom
            // 
            lbFiJuliaZoom.AutoSize = true;
            lbFiJuliaZoom.Location = new Point(141, 137);
            lbFiJuliaZoom.Name = "lbFiJuliaZoom";
            lbFiJuliaZoom.Size = new Size(81, 15);
            lbFiJuliaZoom.TabIndex = 37;
            lbFiJuliaZoom.Text = "lbFiJuliaZoom";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(67, 137);
            label2.Name = "label2";
            label2.Size = new Size(68, 15);
            label2.TabIndex = 36;
            label2.Text = "Julia Zoom:";
            // 
            // lbFiMainFractalZoom
            // 
            lbFiMainFractalZoom.AutoSize = true;
            lbFiMainFractalZoom.Location = new Point(141, 122);
            lbFiMainFractalZoom.Name = "lbFiMainFractalZoom";
            lbFiMainFractalZoom.Size = new Size(120, 15);
            lbFiMainFractalZoom.TabIndex = 35;
            lbFiMainFractalZoom.Text = "lbFiMainFractalZoom";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 122);
            label1.Name = "label1";
            label1.Size = new Size(110, 15);
            label1.TabIndex = 34;
            label1.Text = "Main Fractal Zoom:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(3, 0);
            label10.Name = "label10";
            label10.Size = new Size(325, 15);
            label10.TabIndex = 20;
            label10.Text = "Press the [i] key to hide/show this information display panel.";
            // 
            // limitPanel
            // 
            limitPanel.BackColor = SystemColors.ControlDarkDark;
            limitPanel.BorderStyle = BorderStyle.FixedSingle;
            limitPanel.Controls.Add(btnSetMaxIterations);
            limitPanel.Controls.Add(label23);
            limitPanel.Controls.Add(tbMaxIterations);
            limitPanel.Controls.Add(btnSet4CircleRadius);
            limitPanel.Controls.Add(label22);
            limitPanel.Controls.Add(tb4CircleRadius);
            limitPanel.Controls.Add(doubleBufferedPanel1);
            limitPanel.Controls.Add(label20);
            limitPanel.Controls.Add(tbLimit);
            limitPanel.Controls.Add(btnSetLimit);
            limitPanel.Location = new Point(440, 12);
            limitPanel.Name = "limitPanel";
            limitPanel.Size = new Size(227, 245);
            limitPanel.TabIndex = 2;
            limitPanel.Visible = false;
            // 
            // btnSetMaxIterations
            // 
            btnSetMaxIterations.BackColor = SystemColors.ButtonFace;
            btnSetMaxIterations.BorderStyle = BorderStyle.Fixed3D;
            btnSetMaxIterations.ForeColor = SystemColors.ControlText;
            btnSetMaxIterations.Location = new Point(14, 188);
            btnSetMaxIterations.Name = "btnSetMaxIterations";
            btnSetMaxIterations.Size = new Size(203, 23);
            btnSetMaxIterations.TabIndex = 9;
            btnSetMaxIterations.Text = "Set Maximum Iterations";
            btnSetMaxIterations.TextAlign = ContentAlignment.MiddleCenter;
            btnSetMaxIterations.Click += btnSetMaxIterations_Click;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.ForeColor = SystemColors.Info;
            label23.Location = new Point(3, 163);
            label23.Name = "label23";
            label23.Size = new Size(104, 15);
            label23.TabIndex = 8;
            label23.Text = "Set Max Iterations:";
            // 
            // tbMaxIterations
            // 
            tbMaxIterations.Enabled = false;
            tbMaxIterations.Location = new Point(117, 160);
            tbMaxIterations.Name = "tbMaxIterations";
            tbMaxIterations.Size = new Size(100, 23);
            tbMaxIterations.TabIndex = 7;
            // 
            // btnSet4CircleRadius
            // 
            btnSet4CircleRadius.BackColor = SystemColors.ButtonFace;
            btnSet4CircleRadius.BorderStyle = BorderStyle.Fixed3D;
            btnSet4CircleRadius.ForeColor = SystemColors.ControlText;
            btnSet4CircleRadius.Location = new Point(14, 124);
            btnSet4CircleRadius.Name = "btnSet4CircleRadius";
            btnSet4CircleRadius.Size = new Size(203, 23);
            btnSet4CircleRadius.TabIndex = 6;
            btnSet4CircleRadius.Text = "Set 4 Circle Radius (for video)";
            btnSet4CircleRadius.TextAlign = ContentAlignment.MiddleCenter;
            btnSet4CircleRadius.Click += btnSet4CircleRadius_Click;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.ForeColor = SystemColors.Info;
            label22.Location = new Point(20, 101);
            label22.Name = "label22";
            label22.Size = new Size(87, 15);
            label22.TabIndex = 5;
            label22.Text = "4 Circle Radius:";
            // 
            // tb4CircleRadius
            // 
            tb4CircleRadius.Enabled = false;
            tb4CircleRadius.Location = new Point(117, 98);
            tb4CircleRadius.Name = "tb4CircleRadius";
            tb4CircleRadius.Size = new Size(100, 23);
            tb4CircleRadius.TabIndex = 4;
            // 
            // doubleBufferedPanel1
            // 
            doubleBufferedPanel1.BackColor = SystemColors.ActiveCaption;
            doubleBufferedPanel1.Controls.Add(btnCloseLimitPanel);
            doubleBufferedPanel1.Location = new Point(-1, -1);
            doubleBufferedPanel1.Name = "doubleBufferedPanel1";
            doubleBufferedPanel1.Size = new Size(227, 23);
            doubleBufferedPanel1.TabIndex = 3;
            // 
            // btnCloseLimitPanel
            // 
            btnCloseLimitPanel.BackColor = SystemColors.ButtonFace;
            btnCloseLimitPanel.BorderStyle = BorderStyle.Fixed3D;
            btnCloseLimitPanel.ForeColor = SystemColors.ControlText;
            btnCloseLimitPanel.Location = new Point(204, 1);
            btnCloseLimitPanel.Name = "btnCloseLimitPanel";
            btnCloseLimitPanel.Size = new Size(22, 20);
            btnCloseLimitPanel.TabIndex = 3;
            btnCloseLimitPanel.Text = "X";
            btnCloseLimitPanel.TextAlign = ContentAlignment.MiddleCenter;
            btnCloseLimitPanel.Click += btnCloseLimitPanel_Click;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.ForeColor = SystemColors.Info;
            label20.Location = new Point(12, 36);
            label20.Name = "label20";
            label20.Size = new Size(95, 15);
            label20.TabIndex = 2;
            label20.Text = "Set Escape Limit:";
            // 
            // tbLimit
            // 
            tbLimit.Enabled = false;
            tbLimit.Location = new Point(117, 33);
            tbLimit.Name = "tbLimit";
            tbLimit.Size = new Size(100, 23);
            tbLimit.TabIndex = 1;
            // 
            // btnSetLimit
            // 
            btnSetLimit.BackColor = SystemColors.ButtonFace;
            btnSetLimit.BorderStyle = BorderStyle.Fixed3D;
            btnSetLimit.ForeColor = SystemColors.ControlText;
            btnSetLimit.Location = new Point(14, 59);
            btnSetLimit.Name = "btnSetLimit";
            btnSetLimit.Size = new Size(203, 23);
            btnSetLimit.TabIndex = 0;
            btnSetLimit.Text = "Set New Limit";
            btnSetLimit.TextAlign = ContentAlignment.MiddleCenter;
            btnSetLimit.Click += btnSetLimit_Click;
            // 
            // menuCircleSeriesPreview
            // 
            menuCircleSeriesPreview.Name = "menuCircleSeriesPreview";
            menuCircleSeriesPreview.Size = new Size(180, 22);
            menuCircleSeriesPreview.Text = "Circle Series";
            menuCircleSeriesPreview.Click += menuCircleSeriesPreview_Click;
            // 
            // menuCircleSeriesExport
            // 
            menuCircleSeriesExport.Name = "menuCircleSeriesExport";
            menuCircleSeriesExport.Size = new Size(201, 22);
            menuCircleSeriesExport.Text = "Export Circle Series";
            menuCircleSeriesExport.Click += menuCircleSeriesExport_Click;
            // 
            // FractalDisplayForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1244, 709);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(limitPanel);
            Controls.Add(infoPanel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "FractalDisplayForm";
            Text = "FractalDisplayForm";
            WindowState = FormWindowState.Maximized;
            FormClosing += FractalDisplayForm_FormClosing;
            Load += FractalDisplayForm_Load;
            Paint += FractalDisplayForm_Paint;
            KeyDown += FractalDisplayForm_KeyDown;
            MouseClick += FractalDisplayForm_MouseClick;
            contextMenuStrip1.ResumeLayout(false);
            infoPanel.ResumeLayout(false);
            infoPanel.PerformLayout();
            limitPanel.ResumeLayout(false);
            limitPanel.PerformLayout();
            doubleBufferedPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem generateNewRandomColorsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem palRandomMono;
        private ToolStripMenuItem palRandomCompliment;
        private ToolStripMenuItem palRandomTriad;
        private ToolStripMenuItem palRandomTetrad;
        private ToolStripMenuItem halfCycleMenuItem;
        private ToolStripComboBox cbHalfCycleValue;
        private DoubleBufferedPanel infoPanel;
        private Label lbFiFilterEnd;
        private Label lbFiFilterStart;
        private Label label13;
        private Label label12;
        private Label lbFiIncJulia;
        private Label label15;
        private Label lbFiIncMain;
        private Label label14;
        private Label lbFiZoomRatioJuliaMain;
        private Label label9;
        private Label lbFiSequencePercent;
        private Label lbFiMainViewCenter;
        private Label label8;
        private Label lbFiJuliaViewCenter;
        private Label label7;
        private Label lbFiCurrentMode;
        private Label label6;
        private Label lbFiFormulaName;
        private Label label5;
        private Label lbFiSequenceImageNo;
        private Label label4;
        private Label lbFiJuliaCenter;
        private Label label3;
        private Label lbFiJuliaZoom;
        private Label label2;
        private Label lbFiMainFractalZoom;
        private Label label1;
        private Label label10;
        private DoubleBufferedPanel pnlMainFractal;
        private DoubleBufferedLabel btnResetFilter;
        private DoubleBufferedLabel btnResetFractal;
        private Label lbFiHalfCycleValue;
        private Label label11;
        private Label label16;
        private Label label17;
        private Label lbFiColorPalette;
        private Label label18;
        private Label label19;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem fourCircleSeriesToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem saveAs4CircleSeriesToolStripMenuItem1;
        private ToolStripMenuItem grayscaleToolStripMenuItem;
        private ToolStripMenuItem rainbowToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripComboBox cbStartColor;
        private DoubleBufferedPanel limitPanel;
        private DoubleBufferedLabel btnCloseLimitPanel;
        private Label label20;
        private TextBox tbLimit;
        private DoubleBufferedLabel btnSetLimit;
        private DoubleBufferedPanel doubleBufferedPanel1;
        private Label label21;
        private Label lbFiGhasItsListCount;
        private DoubleBufferedLabel btnSetMaxIterations;
        private Label label23;
        private TextBox tbMaxIterations;
        private DoubleBufferedLabel btnSet4CircleRadius;
        private Label label22;
        private TextBox tb4CircleRadius;
        private ToolStripMenuItem menuCircleSeriesPreview;
        private ToolStripMenuItem menuCircleSeriesExport;
    }
}