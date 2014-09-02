namespace LedVestVideoRenderer
{
    partial class LedVestVideoRenderer
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtAviFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textSaveAs = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textMaxBrightness = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chbxSmoothen = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chbxDuplicate = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chbxTwoFrames = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbIndexFile = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(522, 91);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // txtAviFileName
            // 
            this.txtAviFileName.Location = new System.Drawing.Point(143, 93);
            this.txtAviFileName.Name = "txtAviFileName";
            this.txtAviFileName.Size = new System.Drawing.Size(373, 20);
            this.txtAviFileName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Video File ";
            // 
            // textSaveAs
            // 
            this.textSaveAs.Location = new System.Drawing.Point(143, 141);
            this.textSaveAs.Name = "textSaveAs";
            this.textSaveAs.Size = new System.Drawing.Size(373, 20);
            this.textSaveAs.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Save As";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(522, 338);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Render";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(522, 138);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Browse";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Max Brightness";
            // 
            // textMaxBrightness
            // 
            this.textMaxBrightness.Location = new System.Drawing.Point(144, 192);
            this.textMaxBrightness.Name = "textMaxBrightness";
            this.textMaxBrightness.Size = new System.Drawing.Size(100, 20);
            this.textMaxBrightness.TabIndex = 8;
            this.textMaxBrightness.Text = "100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(250, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "0-255";
            // 
            // chbxSmoothen
            // 
            this.chbxSmoothen.AutoSize = true;
            this.chbxSmoothen.Location = new System.Drawing.Point(144, 242);
            this.chbxSmoothen.Name = "chbxSmoothen";
            this.chbxSmoothen.Size = new System.Drawing.Size(15, 14);
            this.chbxSmoothen.TabIndex = 10;
            this.chbxSmoothen.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(82, 242);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Smoothen";
            // 
            // chbxDuplicate
            // 
            this.chbxDuplicate.AutoSize = true;
            this.chbxDuplicate.Location = new System.Drawing.Point(144, 283);
            this.chbxDuplicate.Name = "chbxDuplicate";
            this.chbxDuplicate.Size = new System.Drawing.Size(15, 14);
            this.chbxDuplicate.TabIndex = 12;
            this.chbxDuplicate.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(48, 284);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Duplicate Check";
            // 
            // chbxTwoFrames
            // 
            this.chbxTwoFrames.AutoSize = true;
            this.chbxTwoFrames.Location = new System.Drawing.Point(144, 323);
            this.chbxTwoFrames.Name = "chbxTwoFrames";
            this.chbxTwoFrames.Size = new System.Drawing.Size(15, 14);
            this.chbxTwoFrames.TabIndex = 12;
            this.chbxTwoFrames.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 324);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Get Ever 2nd Frame";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(48, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Select Led File ";
            // 
            // lbIndexFile
            // 
            this.lbIndexFile.FormattingEnabled = true;
            this.lbIndexFile.Location = new System.Drawing.Point(144, 46);
            this.lbIndexFile.Name = "lbIndexFile";
            this.lbIndexFile.Size = new System.Drawing.Size(372, 21);
            this.lbIndexFile.TabIndex = 17;
            // 
            // LedVestVideoRenderer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 377);
            this.Controls.Add(this.lbIndexFile);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chbxTwoFrames);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chbxDuplicate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chbxSmoothen);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textMaxBrightness);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textSaveAs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAviFileName);
            this.Controls.Add(this.button1);
            this.Name = "LedVestVideoRenderer";
            this.Text = "LED VEST VIDEO RENDERER";
            this.Load += new System.EventHandler(this.LedVestVideoRenderer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtAviFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textSaveAs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textMaxBrightness;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chbxSmoothen;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chbxDuplicate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chbxTwoFrames;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox lbIndexFile;
    }
}

