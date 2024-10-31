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
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { generateNewRandomColorsToolStripMenuItem, saveAsSpiralSeriesToolStripMenuItem, saveAsCIrcleSeriesToolStripMenuItem, saveAs4CircleSeriesToolStripMenuItem, saveAsGoldenRatioToolStripMenuItem, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(234, 136);
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
    }
}