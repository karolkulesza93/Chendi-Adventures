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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelManager));
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.pGenerateTemplate = new System.Windows.Forms.Panel();
            this.lGenerateStatus = new System.Windows.Forms.Label();
            this.bGenerate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pGenerateLevel = new System.Windows.Forms.Panel();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.trampolinesAvailable = new System.Windows.Forms.CheckBox();
            this.spikeballsAvailable = new System.Windows.Forms.CheckBox();
            this.shopAvailable = new System.Windows.Forms.CheckBox();
            this.teleportsAvailable = new System.Windows.Forms.CheckBox();
            this.goldenDoorAvailable = new System.Windows.Forms.CheckBox();
            this.silverDoorAvailable = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.blowtorchersAvailable = new System.Windows.Forms.CheckBox();
            this.spikesAvailable = new System.Windows.Forms.CheckBox();
            this.crushersAvailable = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.wizardsAvailable = new System.Windows.Forms.CheckBox();
            this.ghostsAvailable = new System.Windows.Forms.CheckBox();
            this.archersAvailable = new System.Windows.Forms.CheckBox();
            this.knightsAvailable = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lGenerateLevelStatus = new System.Windows.Forms.Label();
            this.bLevelGenerate = new System.Windows.Forms.Button();
            this.nudLevelHeight = new System.Windows.Forms.NumericUpDown();
            this.nudLevelWidth = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.pGenerateTemplate.SuspendLayout();
            this.pGenerateLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevelHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevelWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // nudWidth
            // 
            this.nudWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudWidth.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudWidth.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudWidth.InterceptArrowKeys = false;
            this.nudWidth.Location = new System.Drawing.Point(37, 26);
            this.nudWidth.Margin = new System.Windows.Forms.Padding(5);
            this.nudWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudWidth.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.ReadOnly = true;
            this.nudWidth.Size = new System.Drawing.Size(73, 20);
            this.nudWidth.TabIndex = 3;
            this.nudWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudWidth.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudWidth.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudWidth.ValueChanged += new System.EventHandler(this.nudTemplateSize_ValueChange);
            // 
            // nudHeight
            // 
            this.nudHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudHeight.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudHeight.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudHeight.InterceptArrowKeys = false;
            this.nudHeight.Location = new System.Drawing.Point(151, 26);
            this.nudHeight.Margin = new System.Windows.Forms.Padding(5);
            this.nudHeight.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            17,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.ReadOnly = true;
            this.nudHeight.Size = new System.Drawing.Size(73, 20);
            this.nudHeight.TabIndex = 4;
            this.nudHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudHeight.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudHeight.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudTemplateSize_ValueChange);
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
            // pGenerateTemplate
            // 
            this.pGenerateTemplate.BackColor = System.Drawing.Color.Transparent;
            this.pGenerateTemplate.Controls.Add(this.lGenerateStatus);
            this.pGenerateTemplate.Controls.Add(this.bGenerate);
            this.pGenerateTemplate.Controls.Add(this.nudHeight);
            this.pGenerateTemplate.Controls.Add(this.nudWidth);
            this.pGenerateTemplate.Controls.Add(this.label3);
            this.pGenerateTemplate.Controls.Add(this.label2);
            this.pGenerateTemplate.Controls.Add(this.label1);
            this.pGenerateTemplate.Location = new System.Drawing.Point(10, 12);
            this.pGenerateTemplate.Name = "pGenerateTemplate";
            this.pGenerateTemplate.Size = new System.Drawing.Size(474, 53);
            this.pGenerateTemplate.TabIndex = 1;
            // 
            // lGenerateStatus
            // 
            this.lGenerateStatus.BackColor = System.Drawing.Color.Transparent;
            this.lGenerateStatus.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lGenerateStatus.ForeColor = System.Drawing.Color.Lime;
            this.lGenerateStatus.Location = new System.Drawing.Point(243, 31);
            this.lGenerateStatus.Margin = new System.Windows.Forms.Padding(5);
            this.lGenerateStatus.Name = "lGenerateStatus";
            this.lGenerateStatus.Size = new System.Drawing.Size(228, 13);
            this.lGenerateStatus.TabIndex = 6;
            this.lGenerateStatus.Text = "000000";
            this.lGenerateStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lGenerateStatus.Visible = false;
            // 
            // bGenerate
            // 
            this.bGenerate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bGenerate.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bGenerate.Location = new System.Drawing.Point(241, 3);
            this.bGenerate.Name = "bGenerate";
            this.bGenerate.Size = new System.Drawing.Size(230, 20);
            this.bGenerate.TabIndex = 5;
            this.bGenerate.Text = "GENERATE";
            this.bGenerate.UseVisualStyleBackColor = true;
            this.bGenerate.Click += new System.EventHandler(this.bGenerate_Click);
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
            // pGenerateLevel
            // 
            this.pGenerateLevel.BackColor = System.Drawing.Color.Transparent;
            this.pGenerateLevel.Controls.Add(this.tbName);
            this.pGenerateLevel.Controls.Add(this.label11);
            this.pGenerateLevel.Controls.Add(this.trampolinesAvailable);
            this.pGenerateLevel.Controls.Add(this.spikeballsAvailable);
            this.pGenerateLevel.Controls.Add(this.shopAvailable);
            this.pGenerateLevel.Controls.Add(this.teleportsAvailable);
            this.pGenerateLevel.Controls.Add(this.goldenDoorAvailable);
            this.pGenerateLevel.Controls.Add(this.silverDoorAvailable);
            this.pGenerateLevel.Controls.Add(this.label10);
            this.pGenerateLevel.Controls.Add(this.blowtorchersAvailable);
            this.pGenerateLevel.Controls.Add(this.spikesAvailable);
            this.pGenerateLevel.Controls.Add(this.crushersAvailable);
            this.pGenerateLevel.Controls.Add(this.label9);
            this.pGenerateLevel.Controls.Add(this.wizardsAvailable);
            this.pGenerateLevel.Controls.Add(this.ghostsAvailable);
            this.pGenerateLevel.Controls.Add(this.archersAvailable);
            this.pGenerateLevel.Controls.Add(this.knightsAvailable);
            this.pGenerateLevel.Controls.Add(this.label8);
            this.pGenerateLevel.Controls.Add(this.lGenerateLevelStatus);
            this.pGenerateLevel.Controls.Add(this.bLevelGenerate);
            this.pGenerateLevel.Controls.Add(this.nudLevelHeight);
            this.pGenerateLevel.Controls.Add(this.nudLevelWidth);
            this.pGenerateLevel.Controls.Add(this.label5);
            this.pGenerateLevel.Controls.Add(this.label6);
            this.pGenerateLevel.Controls.Add(this.label7);
            this.pGenerateLevel.Location = new System.Drawing.Point(10, 71);
            this.pGenerateLevel.Name = "pGenerateLevel";
            this.pGenerateLevel.Size = new System.Drawing.Size(474, 205);
            this.pGenerateLevel.TabIndex = 7;
            // 
            // tbName
            // 
            this.tbName.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbName.Location = new System.Drawing.Point(76, 53);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(395, 20);
            this.tbName.TabIndex = 24;
            this.tbName.Text = "NONE";
            this.tbName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(5, 56);
            this.label11.Margin = new System.Windows.Forms.Padding(5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "NAME:";
            // 
            // trampolinesAvailable
            // 
            this.trampolinesAvailable.AutoSize = true;
            this.trampolinesAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trampolinesAvailable.ForeColor = System.Drawing.Color.White;
            this.trampolinesAvailable.Location = new System.Drawing.Point(305, 183);
            this.trampolinesAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.trampolinesAvailable.Name = "trampolinesAvailable";
            this.trampolinesAvailable.Size = new System.Drawing.Size(169, 17);
            this.trampolinesAvailable.TabIndex = 22;
            this.trampolinesAvailable.Text = "TRAMPOLINES";
            this.trampolinesAvailable.UseVisualStyleBackColor = true;
            // 
            // spikeballsAvailable
            // 
            this.spikeballsAvailable.AutoSize = true;
            this.spikeballsAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spikeballsAvailable.ForeColor = System.Drawing.Color.White;
            this.spikeballsAvailable.Location = new System.Drawing.Point(305, 168);
            this.spikeballsAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.spikeballsAvailable.Name = "spikeballsAvailable";
            this.spikeballsAvailable.Size = new System.Drawing.Size(156, 17);
            this.spikeballsAvailable.TabIndex = 21;
            this.spikeballsAvailable.Text = "SPIKEBALLS";
            this.spikeballsAvailable.UseVisualStyleBackColor = true;
            // 
            // shopAvailable
            // 
            this.shopAvailable.AutoSize = true;
            this.shopAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shopAvailable.ForeColor = System.Drawing.Color.White;
            this.shopAvailable.Location = new System.Drawing.Point(305, 153);
            this.shopAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.shopAvailable.Name = "shopAvailable";
            this.shopAvailable.Size = new System.Drawing.Size(78, 17);
            this.shopAvailable.TabIndex = 20;
            this.shopAvailable.Text = "SHOP";
            this.shopAvailable.UseVisualStyleBackColor = true;
            // 
            // teleportsAvailable
            // 
            this.teleportsAvailable.AutoSize = true;
            this.teleportsAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teleportsAvailable.ForeColor = System.Drawing.Color.White;
            this.teleportsAvailable.Location = new System.Drawing.Point(305, 138);
            this.teleportsAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.teleportsAvailable.Name = "teleportsAvailable";
            this.teleportsAvailable.Size = new System.Drawing.Size(143, 17);
            this.teleportsAvailable.TabIndex = 19;
            this.teleportsAvailable.Text = "TELEPORTS";
            this.teleportsAvailable.UseVisualStyleBackColor = true;
            // 
            // goldenDoorAvailable
            // 
            this.goldenDoorAvailable.AutoSize = true;
            this.goldenDoorAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.goldenDoorAvailable.ForeColor = System.Drawing.Color.White;
            this.goldenDoorAvailable.Location = new System.Drawing.Point(305, 123);
            this.goldenDoorAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.goldenDoorAvailable.Name = "goldenDoorAvailable";
            this.goldenDoorAvailable.Size = new System.Drawing.Size(169, 17);
            this.goldenDoorAvailable.TabIndex = 18;
            this.goldenDoorAvailable.Text = "GOLDEN DOOR";
            this.goldenDoorAvailable.UseVisualStyleBackColor = true;
            // 
            // silverDoorAvailable
            // 
            this.silverDoorAvailable.AutoSize = true;
            this.silverDoorAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.silverDoorAvailable.ForeColor = System.Drawing.Color.White;
            this.silverDoorAvailable.Location = new System.Drawing.Point(305, 108);
            this.silverDoorAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.silverDoorAvailable.Name = "silverDoorAvailable";
            this.silverDoorAvailable.Size = new System.Drawing.Size(169, 17);
            this.silverDoorAvailable.TabIndex = 17;
            this.silverDoorAvailable.Text = "SILVER DOOR";
            this.silverDoorAvailable.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(302, 87);
            this.label10.Margin = new System.Windows.Forms.Padding(5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "SPECIALS:";
            // 
            // blowtorchersAvailable
            // 
            this.blowtorchersAvailable.AutoSize = true;
            this.blowtorchersAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blowtorchersAvailable.ForeColor = System.Drawing.Color.White;
            this.blowtorchersAvailable.Location = new System.Drawing.Point(133, 140);
            this.blowtorchersAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.blowtorchersAvailable.Name = "blowtorchersAvailable";
            this.blowtorchersAvailable.Size = new System.Drawing.Size(169, 17);
            this.blowtorchersAvailable.TabIndex = 15;
            this.blowtorchersAvailable.Text = "BLOWTORCHES";
            this.blowtorchersAvailable.UseVisualStyleBackColor = true;
            // 
            // spikesAvailable
            // 
            this.spikesAvailable.AutoSize = true;
            this.spikesAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spikesAvailable.ForeColor = System.Drawing.Color.White;
            this.spikesAvailable.Location = new System.Drawing.Point(133, 125);
            this.spikesAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.spikesAvailable.Name = "spikesAvailable";
            this.spikesAvailable.Size = new System.Drawing.Size(104, 17);
            this.spikesAvailable.TabIndex = 14;
            this.spikesAvailable.Text = "SPIKES";
            this.spikesAvailable.UseVisualStyleBackColor = true;
            // 
            // crushersAvailable
            // 
            this.crushersAvailable.AutoSize = true;
            this.crushersAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crushersAvailable.ForeColor = System.Drawing.Color.White;
            this.crushersAvailable.Location = new System.Drawing.Point(133, 110);
            this.crushersAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.crushersAvailable.Name = "crushersAvailable";
            this.crushersAvailable.Size = new System.Drawing.Size(130, 17);
            this.crushersAvailable.TabIndex = 13;
            this.crushersAvailable.Text = "CRUSHERS";
            this.crushersAvailable.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(130, 89);
            this.label9.Margin = new System.Windows.Forms.Padding(5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "TRAPS:";
            // 
            // wizardsAvailable
            // 
            this.wizardsAvailable.AutoSize = true;
            this.wizardsAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardsAvailable.ForeColor = System.Drawing.Color.White;
            this.wizardsAvailable.Location = new System.Drawing.Point(8, 155);
            this.wizardsAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.wizardsAvailable.Name = "wizardsAvailable";
            this.wizardsAvailable.Size = new System.Drawing.Size(117, 17);
            this.wizardsAvailable.TabIndex = 11;
            this.wizardsAvailable.Text = "WIZARDS";
            this.wizardsAvailable.UseVisualStyleBackColor = true;
            // 
            // ghostsAvailable
            // 
            this.ghostsAvailable.AutoSize = true;
            this.ghostsAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ghostsAvailable.ForeColor = System.Drawing.Color.White;
            this.ghostsAvailable.Location = new System.Drawing.Point(8, 140);
            this.ghostsAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.ghostsAvailable.Name = "ghostsAvailable";
            this.ghostsAvailable.Size = new System.Drawing.Size(104, 17);
            this.ghostsAvailable.TabIndex = 10;
            this.ghostsAvailable.Text = "GHOSTS";
            this.ghostsAvailable.UseVisualStyleBackColor = true;
            // 
            // archersAvailable
            // 
            this.archersAvailable.AutoSize = true;
            this.archersAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.archersAvailable.ForeColor = System.Drawing.Color.White;
            this.archersAvailable.Location = new System.Drawing.Point(8, 125);
            this.archersAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.archersAvailable.Name = "archersAvailable";
            this.archersAvailable.Size = new System.Drawing.Size(117, 17);
            this.archersAvailable.TabIndex = 9;
            this.archersAvailable.Text = "ARCHERS";
            this.archersAvailable.UseVisualStyleBackColor = true;
            // 
            // knightsAvailable
            // 
            this.knightsAvailable.AutoSize = true;
            this.knightsAvailable.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.knightsAvailable.ForeColor = System.Drawing.Color.White;
            this.knightsAvailable.Location = new System.Drawing.Point(8, 110);
            this.knightsAvailable.Margin = new System.Windows.Forms.Padding(5);
            this.knightsAvailable.Name = "knightsAvailable";
            this.knightsAvailable.Size = new System.Drawing.Size(117, 17);
            this.knightsAvailable.TabIndex = 8;
            this.knightsAvailable.Text = "KNIGHTS";
            this.knightsAvailable.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(5, 89);
            this.label8.Margin = new System.Windows.Forms.Padding(5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "MONSTERS:";
            // 
            // lGenerateLevelStatus
            // 
            this.lGenerateLevelStatus.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lGenerateLevelStatus.ForeColor = System.Drawing.Color.Lime;
            this.lGenerateLevelStatus.Location = new System.Drawing.Point(243, 31);
            this.lGenerateLevelStatus.Margin = new System.Windows.Forms.Padding(5);
            this.lGenerateLevelStatus.Name = "lGenerateLevelStatus";
            this.lGenerateLevelStatus.Size = new System.Drawing.Size(228, 13);
            this.lGenerateLevelStatus.TabIndex = 6;
            this.lGenerateLevelStatus.Text = "000000";
            this.lGenerateLevelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lGenerateLevelStatus.Visible = false;
            // 
            // bLevelGenerate
            // 
            this.bLevelGenerate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bLevelGenerate.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLevelGenerate.Location = new System.Drawing.Point(241, 3);
            this.bLevelGenerate.Name = "bLevelGenerate";
            this.bLevelGenerate.Size = new System.Drawing.Size(230, 20);
            this.bLevelGenerate.TabIndex = 5;
            this.bLevelGenerate.Text = "GENERATE";
            this.bLevelGenerate.UseVisualStyleBackColor = true;
            this.bLevelGenerate.Click += new System.EventHandler(this.bLevelGenerate_Click);
            // 
            // nudLevelHeight
            // 
            this.nudLevelHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudLevelHeight.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLevelHeight.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudLevelHeight.InterceptArrowKeys = false;
            this.nudLevelHeight.Location = new System.Drawing.Point(151, 26);
            this.nudLevelHeight.Margin = new System.Windows.Forms.Padding(5);
            this.nudLevelHeight.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudLevelHeight.Minimum = new decimal(new int[] {
            17,
            0,
            0,
            0});
            this.nudLevelHeight.Name = "nudLevelHeight";
            this.nudLevelHeight.ReadOnly = true;
            this.nudLevelHeight.Size = new System.Drawing.Size(73, 20);
            this.nudLevelHeight.TabIndex = 4;
            this.nudLevelHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLevelHeight.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudLevelHeight.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            this.nudLevelHeight.ValueChanged += new System.EventHandler(this.nudLevelSize_ValueChange);
            // 
            // nudLevelWidth
            // 
            this.nudLevelWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudLevelWidth.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLevelWidth.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudLevelWidth.InterceptArrowKeys = false;
            this.nudLevelWidth.Location = new System.Drawing.Point(37, 26);
            this.nudLevelWidth.Margin = new System.Windows.Forms.Padding(5);
            this.nudLevelWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudLevelWidth.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudLevelWidth.Name = "nudLevelWidth";
            this.nudLevelWidth.ReadOnly = true;
            this.nudLevelWidth.Size = new System.Drawing.Size(73, 20);
            this.nudLevelWidth.TabIndex = 3;
            this.nudLevelWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLevelWidth.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudLevelWidth.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudLevelWidth.ValueChanged += new System.EventHandler(this.nudLevelSize_ValueChange);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(120, 28);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Y:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 28);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "X:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Nintendo NES Font", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(5, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "GENERATE LEVEL";
            // 
            // LevelManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(496, 284);
            this.Controls.Add(this.pGenerateLevel);
            this.Controls.Add(this.pGenerateTemplate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LevelManager";
            this.Text = "LevelManager";
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.pGenerateTemplate.ResumeLayout(false);
            this.pGenerateTemplate.PerformLayout();
            this.pGenerateLevel.ResumeLayout(false);
            this.pGenerateLevel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevelHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevelWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pGenerateTemplate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bGenerate;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label lGenerateStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel pGenerateLevel;
        private System.Windows.Forms.Label lGenerateLevelStatus;
        private System.Windows.Forms.Button bLevelGenerate;
        private System.Windows.Forms.NumericUpDown nudLevelHeight;
        private System.Windows.Forms.NumericUpDown nudLevelWidth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox ghostsAvailable;
        private System.Windows.Forms.CheckBox archersAvailable;
        private System.Windows.Forms.CheckBox knightsAvailable;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox trampolinesAvailable;
        private System.Windows.Forms.CheckBox spikeballsAvailable;
        private System.Windows.Forms.CheckBox shopAvailable;
        private System.Windows.Forms.CheckBox teleportsAvailable;
        private System.Windows.Forms.CheckBox goldenDoorAvailable;
        private System.Windows.Forms.CheckBox silverDoorAvailable;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox blowtorchersAvailable;
        private System.Windows.Forms.CheckBox spikesAvailable;
        private System.Windows.Forms.CheckBox crushersAvailable;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox wizardsAvailable;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label11;
    }
}

