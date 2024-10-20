namespace CalcmasterFractal
{
    partial class LauncherForm
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
            lblError = new Label();
            cbFormulas = new ComboBox();
            label5 = new Label();
            btnGo = new Button();
            SuspendLayout();
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Location = new Point(12, 32);
            lblError.Name = "lblError";
            lblError.Size = new Size(111, 15);
            lblError.TabIndex = 9;
            lblError.Text = "Loading Formulas...";
            // 
            // cbFormulas
            // 
            cbFormulas.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFormulas.FormattingEnabled = true;
            cbFormulas.Location = new Point(110, 6);
            cbFormulas.Name = "cbFormulas";
            cbFormulas.Size = new Size(239, 23);
            cbFormulas.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 9);
            label5.Name = "label5";
            label5.Size = new Size(92, 15);
            label5.TabIndex = 11;
            label5.Text = "Fractal Formula:";
            // 
            // btnGo
            // 
            btnGo.Location = new Point(355, 5);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(75, 23);
            btnGo.TabIndex = 12;
            btnGo.Text = "Go";
            btnGo.UseVisualStyleBackColor = true;
            // 
            // LauncherForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 270);
            Controls.Add(btnGo);
            Controls.Add(label5);
            Controls.Add(cbFormulas);
            Controls.Add(lblError);
            Name = "LauncherForm";
            Text = "LauncherForm";
            FormClosing += LauncherForm_FormClosing;
            Load += LauncherForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblError;
        private ComboBox cbFormulas;
        private Label label5;
        private Button btnGo;
    }
}