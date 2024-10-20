namespace CalcmasterFractal
{
    partial class FractalInterfaceTests
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
            lbPtrStatus = new Label();
            btnDestroyMap = new Button();
            btnAdd = new Button();
            tbSum = new TextBox();
            label3 = new Label();
            tbY = new TextBox();
            label2 = new Label();
            tbX = new TextBox();
            label1 = new Label();
            label4 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // lbPtrStatus
            // 
            lbPtrStatus.AutoSize = true;
            lbPtrStatus.Location = new Point(108, 167);
            lbPtrStatus.Name = "lbPtrStatus";
            lbPtrStatus.Size = new Size(38, 15);
            lbPtrStatus.TabIndex = 17;
            lbPtrStatus.Text = "label4";
            // 
            // btnDestroyMap
            // 
            btnDestroyMap.Location = new Point(18, 163);
            btnDestroyMap.Name = "btnDestroyMap";
            btnDestroyMap.Size = new Size(75, 23);
            btnDestroyMap.TabIndex = 16;
            btnDestroyMap.Text = "Destroy";
            btnDestroyMap.UseVisualStyleBackColor = true;
            btnDestroyMap.Click += btnDestroyMap_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(165, 98);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 15;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // tbSum
            // 
            tbSum.Location = new Point(59, 99);
            tbSum.Name = "tbSum";
            tbSum.Size = new Size(100, 23);
            tbSum.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 102);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 13;
            label3.Text = "SUM:";
            // 
            // tbY
            // 
            tbY.Location = new Point(59, 70);
            tbY.Name = "tbY";
            tbY.Size = new Size(100, 23);
            tbY.TabIndex = 12;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(36, 73);
            label2.Name = "label2";
            label2.Size = new Size(17, 15);
            label2.TabIndex = 11;
            label2.Text = "Y:";
            // 
            // tbX
            // 
            tbX.Location = new Point(59, 41);
            tbX.Name = "tbX";
            tbX.Size = new Size(100, 23);
            tbX.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 44);
            label1.Name = "label1";
            label1.Size = new Size(17, 15);
            label1.TabIndex = 9;
            label1.Text = "X:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 9);
            label4.Name = "label4";
            label4.Size = new Size(311, 15);
            label4.TabIndex = 18;
            label4.Text = "Tests for CalcmasterFractalDll Interface, FractalInterface.cs";
            // 
            // button1
            // 
            button1.Location = new Point(368, 44);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 19;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // FractalInterfaceTests
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(lbPtrStatus);
            Controls.Add(btnDestroyMap);
            Controls.Add(btnAdd);
            Controls.Add(tbSum);
            Controls.Add(label3);
            Controls.Add(tbY);
            Controls.Add(label2);
            Controls.Add(tbX);
            Controls.Add(label1);
            Name = "FractalInterfaceTests";
            Text = "FractalInterfaceTests";
            FormClosing += FractalInterfaceTests_FormClosing;
            Load += FractalInterfaceTests_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbPtrStatus;
        private Button btnDestroyMap;
        private Button btnAdd;
        private TextBox tbSum;
        private Label label3;
        private TextBox tbY;
        private Label label2;
        private TextBox tbX;
        private Label label1;
        private Label label4;
        private Button button1;
    }
}