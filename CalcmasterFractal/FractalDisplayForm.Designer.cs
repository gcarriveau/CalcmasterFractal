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
            palRandomMono = new ToolStripMenuItem();
            palRandomCompliment = new ToolStripMenuItem();
            palRandomTriad = new ToolStripMenuItem();
            palRandomTetrad = new ToolStripMenuItem();
            halfCycleMenuItem = new ToolStripMenuItem();
            cbHalfCycleValue = new ToolStripComboBox();
            generateNewRandomColorsToolStripMenuItem = new ToolStripMenuItem();
            saveAsSpiralSeriesToolStripMenuItem = new ToolStripMenuItem();
            saveAsCIrcleSeriesToolStripMenuItem = new ToolStripMenuItem();
            saveAs4CircleSeriesToolStripMenuItem = new ToolStripMenuItem();
            saveAsGoldenRatioToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            infoPanel = new DoubleBufferedPanel();
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
            label18 = new Label();
            label19 = new Label();
            contextMenuStrip1.SuspendLayout();
            infoPanel.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, halfCycleMenuItem, generateNewRandomColorsToolStripMenuItem, saveAsSpiralSeriesToolStripMenuItem, saveAsCIrcleSeriesToolStripMenuItem, saveAs4CircleSeriesToolStripMenuItem, saveAsGoldenRatioToolStripMenuItem, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(234, 180);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { palRandomMono, palRandomCompliment, palRandomTriad, palRandomTetrad });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(233, 22);
            toolStripMenuItem1.Text = "Color Algorithm";
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
            // saveAsSpiralSeriesToolStripMenuItem
            // 
            saveAsSpiralSeriesToolStripMenuItem.Name = "saveAsSpiralSeriesToolStripMenuItem";
            saveAsSpiralSeriesToolStripMenuItem.Size = new Size(233, 22);
            saveAsSpiralSeriesToolStripMenuItem.Text = "Save as Spiral Series";
            // 
            // saveAsCIrcleSeriesToolStripMenuItem
            // 
            saveAsCIrcleSeriesToolStripMenuItem.Name = "saveAsCIrcleSeriesToolStripMenuItem";
            saveAsCIrcleSeriesToolStripMenuItem.Size = new Size(233, 22);
            saveAsCIrcleSeriesToolStripMenuItem.Text = "Save as CIrcle Series";
            // 
            // saveAs4CircleSeriesToolStripMenuItem
            // 
            saveAs4CircleSeriesToolStripMenuItem.Name = "saveAs4CircleSeriesToolStripMenuItem";
            saveAs4CircleSeriesToolStripMenuItem.Size = new Size(233, 22);
            saveAs4CircleSeriesToolStripMenuItem.Text = "Save as 4 Circle Series";
            // 
            // saveAsGoldenRatioToolStripMenuItem
            // 
            saveAsGoldenRatioToolStripMenuItem.Name = "saveAsGoldenRatioToolStripMenuItem";
            saveAsGoldenRatioToolStripMenuItem.Size = new Size(233, 22);
            saveAsGoldenRatioToolStripMenuItem.Text = "Save as Golden Ratio";
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
            lbFiJuliaCenter.Location = new Point(141, 93);
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
            btnResetFilter.Location = new Point(339, 266);
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
            lbFiFilterEnd.Location = new Point(142, 249);
            lbFiFilterEnd.Name = "lbFiFilterEnd";
            lbFiFilterEnd.Size = new Size(72, 15);
            lbFiFilterEnd.TabIndex = 62;
            lbFiFilterEnd.Text = "lbFiFilterEnd";
            // 
            // lbFiFilterStart
            // 
            lbFiFilterStart.AutoSize = true;
            lbFiFilterStart.Location = new Point(142, 234);
            lbFiFilterStart.Name = "lbFiFilterStart";
            lbFiFilterStart.Size = new Size(76, 15);
            lbFiFilterStart.TabIndex = 61;
            lbFiFilterStart.Text = "lbFiFilterStart";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(29, 249);
            label13.Name = "label13";
            label13.Size = new Size(106, 15);
            label13.TabIndex = 60;
            label13.Text = "Iteration Filter End:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(26, 234);
            label12.Name = "label12";
            label12.Size = new Size(110, 15);
            label12.TabIndex = 59;
            label12.Text = "Iteration Filter Start:";
            // 
            // lbFiIncJulia
            // 
            lbFiIncJulia.AutoSize = true;
            lbFiIncJulia.Location = new Point(141, 168);
            lbFiIncJulia.Name = "lbFiIncJulia";
            lbFiIncJulia.Size = new Size(65, 15);
            lbFiIncJulia.TabIndex = 58;
            lbFiIncJulia.Text = "lbFiIncJulia";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(21, 168);
            label15.Name = "label15";
            label15.Size = new Size(114, 15);
            label15.TabIndex = 57;
            label15.Text = "Between Pixels Julia:";
            // 
            // lbFiIncMain
            // 
            lbFiIncMain.AutoSize = true;
            lbFiIncMain.Location = new Point(141, 153);
            lbFiIncMain.Name = "lbFiIncMain";
            lbFiIncMain.Size = new Size(69, 15);
            lbFiIncMain.TabIndex = 56;
            lbFiIncMain.Text = "lbFiIncMain";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(17, 153);
            label14.Name = "label14";
            label14.Size = new Size(118, 15);
            label14.TabIndex = 55;
            label14.Text = "Between Pixels Main:";
            // 
            // lbFiZoomRatioJuliaMain
            // 
            lbFiZoomRatioJuliaMain.AutoSize = true;
            lbFiZoomRatioJuliaMain.Location = new Point(141, 138);
            lbFiZoomRatioJuliaMain.Name = "lbFiZoomRatioJuliaMain";
            lbFiZoomRatioJuliaMain.Size = new Size(135, 15);
            lbFiZoomRatioJuliaMain.TabIndex = 52;
            lbFiZoomRatioJuliaMain.Text = "lbFiZoomRatioJuliaMain";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(-1, 138);
            label9.Name = "label9";
            label9.Size = new Size(136, 15);
            label9.TabIndex = 51;
            label9.Text = "Zoom Ratio [Julia:Main]:";
            // 
            // lbFiSequencePercent
            // 
            lbFiSequencePercent.AutoSize = true;
            lbFiSequencePercent.Location = new Point(273, 216);
            lbFiSequencePercent.Name = "lbFiSequencePercent";
            lbFiSequencePercent.Size = new Size(117, 15);
            lbFiSequencePercent.TabIndex = 50;
            lbFiSequencePercent.Text = "lbFiSequencePercent";
            // 
            // lbFiMainViewCenter
            // 
            lbFiMainViewCenter.AutoSize = true;
            lbFiMainViewCenter.Location = new Point(141, 183);
            lbFiMainViewCenter.Name = "lbFiMainViewCenter";
            lbFiMainViewCenter.Size = new Size(113, 15);
            lbFiMainViewCenter.TabIndex = 49;
            lbFiMainViewCenter.Text = "lbFiMainViewCenter";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(32, 183);
            label8.Name = "label8";
            label8.Size = new Size(103, 15);
            label8.TabIndex = 48;
            label8.Text = "Main View Center:";
            // 
            // lbFiJuliaViewCenter
            // 
            lbFiJuliaViewCenter.AutoSize = true;
            lbFiJuliaViewCenter.Location = new Point(141, 198);
            lbFiJuliaViewCenter.Name = "lbFiJuliaViewCenter";
            lbFiJuliaViewCenter.Size = new Size(109, 15);
            lbFiJuliaViewCenter.TabIndex = 47;
            lbFiJuliaViewCenter.Text = "lbFiJuliaViewCenter";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(36, 198);
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
            lbFiSequenceImageNo.Location = new Point(141, 216);
            lbFiSequenceImageNo.Name = "lbFiSequenceImageNo";
            lbFiSequenceImageNo.Size = new Size(126, 15);
            lbFiSequenceImageNo.TabIndex = 41;
            lbFiSequenceImageNo.Text = "lbFiSequenceImageNo";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 216);
            label4.Name = "label4";
            label4.Size = new Size(119, 15);
            label4.TabIndex = 40;
            label4.Text = "Sequence Image No.:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(51, 93);
            label3.Name = "label3";
            label3.Size = new Size(84, 15);
            label3.TabIndex = 38;
            label3.Text = "Julia Constant:";
            // 
            // lbFiJuliaZoom
            // 
            lbFiJuliaZoom.AutoSize = true;
            lbFiJuliaZoom.Location = new Point(141, 123);
            lbFiJuliaZoom.Name = "lbFiJuliaZoom";
            lbFiJuliaZoom.Size = new Size(81, 15);
            lbFiJuliaZoom.TabIndex = 37;
            lbFiJuliaZoom.Text = "lbFiJuliaZoom";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(67, 123);
            label2.Name = "label2";
            label2.Size = new Size(68, 15);
            label2.TabIndex = 36;
            label2.Text = "Julia Zoom:";
            // 
            // lbFiMainFractalZoom
            // 
            lbFiMainFractalZoom.AutoSize = true;
            lbFiMainFractalZoom.Location = new Point(141, 108);
            lbFiMainFractalZoom.Name = "lbFiMainFractalZoom";
            lbFiMainFractalZoom.Size = new Size(120, 15);
            lbFiMainFractalZoom.TabIndex = 35;
            lbFiMainFractalZoom.Text = "lbFiMainFractalZoom";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 108);
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
            label10.Size = new Size(390, 15);
            label10.TabIndex = 20;
            label10.Text = "Press the [i] key to hide/show this information display panel, [h] for help.";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(252, 234);
            label18.Name = "label18";
            label18.Size = new Size(117, 15);
            label18.TabIndex = 72;
            label18.Text = "(+/- [alt] shift wheel)";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(252, 249);
            label19.Name = "label19";
            label19.Size = new Size(111, 15);
            label19.TabIndex = 73;
            label19.Text = "(+/- [alt] ctrl wheel)";
            // 
            // FractalDisplayForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(631, 681);
            ContextMenuStrip = contextMenuStrip1;
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
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem generateNewRandomColorsToolStripMenuItem;
        private ToolStripMenuItem saveAsSpiralSeriesToolStripMenuItem;
        private ToolStripMenuItem saveAsCIrcleSeriesToolStripMenuItem;
        private ToolStripMenuItem saveAs4CircleSeriesToolStripMenuItem;
        private ToolStripMenuItem saveAsGoldenRatioToolStripMenuItem;
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
    }
}