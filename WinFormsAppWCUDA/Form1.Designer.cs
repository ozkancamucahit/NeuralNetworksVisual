﻿
namespace WinFormsAppWCUDA
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbCartesianBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.lblClassSamples = new System.Windows.Forms.Label();
            this.comboxInputs = new System.Windows.Forms.ComboBox();
            this.lblNoOfClasses = new System.Windows.Forms.Label();
            this.comboxNoOfClasses = new System.Windows.Forms.ComboBox();
            this.btn_addWcpu = new System.Windows.Forms.Button();
            this.btn_addWCuda = new System.Windows.Forms.Button();
            this.lboxSamples = new System.Windows.Forms.ListBox();
            this.lblInputText = new System.Windows.Forms.Label();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbCartesianBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbCartesianBox
            // 
            this.pbCartesianBox.BackColor = System.Drawing.Color.White;
            this.pbCartesianBox.Location = new System.Drawing.Point(12, 35);
            this.pbCartesianBox.Name = "pbCartesianBox";
            this.pbCartesianBox.Size = new System.Drawing.Size(481, 355);
            this.pbCartesianBox.TabIndex = 0;
            this.pbCartesianBox.TabStop = false;
            this.pbCartesianBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pbCartesianBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(899, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nNToolStripMenuItem,
            this.loadWeightsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // nNToolStripMenuItem
            // 
            this.nNToolStripMenuItem.Name = "nNToolStripMenuItem";
            this.nNToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.nNToolStripMenuItem.Text = "NN";
            // 
            // loadWeightsToolStripMenuItem
            // 
            this.loadWeightsToolStripMenuItem.Name = "loadWeightsToolStripMenuItem";
            this.loadWeightsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.loadWeightsToolStripMenuItem.Text = "Load Weights";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_reset);
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.lblClassSamples);
            this.groupBox1.Controls.Add(this.comboxInputs);
            this.groupBox1.Controls.Add(this.lblNoOfClasses);
            this.groupBox1.Controls.Add(this.comboxNoOfClasses);
            this.groupBox1.Location = new System.Drawing.Point(499, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 134);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // btn_reset
            // 
            this.btn_reset.Enabled = false;
            this.btn_reset.Location = new System.Drawing.Point(6, 105);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(49, 23);
            this.btn_reset.TabIndex = 5;
            this.btn_reset.Text = "RESET";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(119, 105);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "SET";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // lblClassSamples
            // 
            this.lblClassSamples.AutoSize = true;
            this.lblClassSamples.Location = new System.Drawing.Point(6, 68);
            this.lblClassSamples.Name = "lblClassSamples";
            this.lblClassSamples.Size = new System.Drawing.Size(100, 15);
            this.lblClassSamples.TabIndex = 3;
            this.lblClassSamples.Text = "Inputs For Class #";
            // 
            // comboxInputs
            // 
            this.comboxInputs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxInputs.Enabled = false;
            this.comboxInputs.FormattingEnabled = true;
            this.comboxInputs.Items.AddRange(new object[] {
            "2",
            "3",
            "4"});
            this.comboxInputs.Location = new System.Drawing.Point(119, 65);
            this.comboxInputs.Name = "comboxInputs";
            this.comboxInputs.Size = new System.Drawing.Size(106, 23);
            this.comboxInputs.TabIndex = 2;
            // 
            // lblNoOfClasses
            // 
            this.lblNoOfClasses.AutoSize = true;
            this.lblNoOfClasses.Location = new System.Drawing.Point(6, 25);
            this.lblNoOfClasses.Name = "lblNoOfClasses";
            this.lblNoOfClasses.Size = new System.Drawing.Size(108, 15);
            this.lblNoOfClasses.TabIndex = 1;
            this.lblNoOfClasses.Text = "Number Of Classes";
            // 
            // comboxNoOfClasses
            // 
            this.comboxNoOfClasses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxNoOfClasses.FormattingEnabled = true;
            this.comboxNoOfClasses.Items.AddRange(new object[] {
            "2",
            "3",
            "4"});
            this.comboxNoOfClasses.Location = new System.Drawing.Point(119, 22);
            this.comboxNoOfClasses.Name = "comboxNoOfClasses";
            this.comboxNoOfClasses.Size = new System.Drawing.Size(106, 23);
            this.comboxNoOfClasses.TabIndex = 0;
            this.comboxNoOfClasses.SelectedIndexChanged += new System.EventHandler(this.comboxNoOfClasses_SelectedIndexChanged);
            // 
            // btn_addWcpu
            // 
            this.btn_addWcpu.Location = new System.Drawing.Point(748, 71);
            this.btn_addWcpu.Name = "btn_addWcpu";
            this.btn_addWcpu.Size = new System.Drawing.Size(106, 23);
            this.btn_addWcpu.TabIndex = 3;
            this.btn_addWcpu.Text = "addWCPU";
            this.btn_addWcpu.UseVisualStyleBackColor = true;
            // 
            // btn_addWCuda
            // 
            this.btn_addWCuda.Location = new System.Drawing.Point(748, 101);
            this.btn_addWCuda.Name = "btn_addWCuda";
            this.btn_addWCuda.Size = new System.Drawing.Size(106, 23);
            this.btn_addWCuda.TabIndex = 4;
            this.btn_addWCuda.Text = "add W cuda";
            this.btn_addWCuda.UseVisualStyleBackColor = true;
            this.btn_addWCuda.Click += new System.EventHandler(this.btn_addWCuda_Click);
            // 
            // lboxSamples
            // 
            this.lboxSamples.Enabled = false;
            this.lboxSamples.FormattingEnabled = true;
            this.lboxSamples.ItemHeight = 15;
            this.lboxSamples.Location = new System.Drawing.Point(499, 208);
            this.lboxSamples.Name = "lboxSamples";
            this.lboxSamples.Size = new System.Drawing.Size(139, 94);
            this.lboxSamples.TabIndex = 5;
            this.lboxSamples.SelectedIndexChanged += new System.EventHandler(this.lboxSamples_SelectedIndexChanged);
            // 
            // lblInputText
            // 
            this.lblInputText.AutoSize = true;
            this.lblInputText.Location = new System.Drawing.Point(499, 316);
            this.lblInputText.Name = "lblInputText";
            this.lblInputText.Size = new System.Drawing.Size(0, 15);
            this.lblInputText.TabIndex = 6;
            this.lblInputText.Visible = false;
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Enabled = false;
            this.btnRemoveSelected.Location = new System.Drawing.Point(579, 308);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(59, 23);
            this.btnRemoveSelected.TabIndex = 7;
            this.btnRemoveSelected.Text = "Remove";
            this.toolTip1.SetToolTip(this.btnRemoveSelected, "Remove Selected Sample");
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnRemoveSelected_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(899, 467);
            this.Controls.Add(this.btnRemoveSelected);
            this.Controls.Add(this.lblInputText);
            this.Controls.Add(this.lboxSamples);
            this.Controls.Add(this.btn_addWCuda);
            this.Controls.Add(this.btn_addWcpu);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbCartesianBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Neural Networks";
            ((System.ComponentModel.ISupportInitialize)(this.pbCartesianBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCartesianBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWeightsToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboxNoOfClasses;
        private System.Windows.Forms.Label lblClassSamples;
        private System.Windows.Forms.ComboBox comboxInputs;
        private System.Windows.Forms.Label lblNoOfClasses;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btn_addWcpu;
        private System.Windows.Forms.Button btn_addWCuda;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.ListBox lboxSamples;
        private System.Windows.Forms.Label lblInputText;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

