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
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, halfCycleMenuItem, generateNewRandomColorsToolStripMenuItem, saveAsSpiralSeriesToolStripMenuItem, saveAsCIrcleSeriesToolStripMenuItem, saveAs4CircleSeriesToolStripMenuItem, saveAsGoldenRatioToolStripMenuItem, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(234, 202);
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
            cbHalfCycleValue.Items.AddRange(new object[] { "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "125", "150", "175", "200", "300", "400", "500", "600", "700", "800", "900", "1000" });
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
            // 
            // FractalDisplayForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ContextMenuStrip = contextMenuStrip1;
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
    }
}