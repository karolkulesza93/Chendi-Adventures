namespace LevelManager
{
    partial class LevelManager
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
            System.Windows.Forms.NumericUpDown numericUpDown1;
            System.Windows.Forms.NumericUpDown numericUpDown2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelManager));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "GENERATE TEMPLATE";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(numericUpDown2);
            this.panel1.Controls.Add(numericUpDown1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(10, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 54);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(120, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y:";
            // 
            // numericUpDown1
            // 
            numericUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numericUpDown1.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numericUpDown1.ImeMode = System.Windows.Forms.ImeMode.Off;
            numericUpDown1.InterceptArrowKeys = false;
            numericUpDown1.Location = new System.Drawing.Point(37, 26);
            numericUpDown1.Margin = new System.Windows.Forms.Padding(5);
            numericUpDown1.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            numericUpDown1.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.ReadOnly = true;
            numericUpDown1.Size = new System.Drawing.Size(73, 20);
            numericUpDown1.TabIndex = 3;
            numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numericUpDown1.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            numericUpDown1.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            numericUpDown2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numericUpDown2.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numericUpDown2.ImeMode = System.Windows.Forms.ImeMode.Off;
            numericUpDown2.InterceptArrowKeys = false;
            numericUpDown2.Location = new System.Drawing.Point(151, 26);
            numericUpDown2.Margin = new System.Windows.Forms.Padding(5);
            numericUpDown2.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            numericUpDown2.Minimum = new decimal(new int[] {
            17,
            0,
            0,
            0});
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.ReadOnly = true;
            numericUpDown2.Size = new System.Drawing.Size(73, 20);
            numericUpDown2.TabIndex = 4;
            numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numericUpDown2.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            numericUpDown2.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(241, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 41);
            this.button1.TabIndex = 5;
            this.button1.Text = "GENERATE";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // LevelManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(496, 473);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LevelManager";
            this.Text = "LevelManager";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}

