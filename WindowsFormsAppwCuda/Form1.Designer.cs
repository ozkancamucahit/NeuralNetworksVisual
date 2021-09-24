
namespace WindowsFormsAppwCuda
{
    partial class Form1
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
            this.btn_addWCuda = new System.Windows.Forms.Button();
            this.btn_addWcpu = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cb_useCuda = new System.Windows.Forms.CheckBox();
            this.pbCartesianBox = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.lblClassSamples = new System.Windows.Forms.Label();
            this.comboxInputs = new System.Windows.Forms.ComboBox();
            this.lblNoOfClasses = new System.Windows.Forms.Label();
            this.comboxNoOfClasses = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCartesianBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_addWCuda
            // 
            this.btn_addWCuda.Location = new System.Drawing.Point(566, 264);
            this.btn_addWCuda.Name = "btn_addWCuda";
            this.btn_addWCuda.Size = new System.Drawing.Size(93, 23);
            this.btn_addWCuda.TabIndex = 0;
            this.btn_addWCuda.Text = "add W cuda";
            this.btn_addWCuda.UseVisualStyleBackColor = true;
            this.btn_addWCuda.Click += new System.EventHandler(this.btn_addWCuda_Click);
            // 
            // btn_addWcpu
            // 
            this.btn_addWcpu.Location = new System.Drawing.Point(566, 224);
            this.btn_addWcpu.Name = "btn_addWcpu";
            this.btn_addWcpu.Size = new System.Drawing.Size(118, 23);
            this.btn_addWcpu.TabIndex = 1;
            this.btn_addWcpu.Text = "addWCPU";
            this.btn_addWcpu.UseVisualStyleBackColor = true;
            this.btn_addWcpu.Click += new System.EventHandler(this.btn_addWcpu_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1054, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nNToolStripMenuItem,
            this.loadWeightsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // nNToolStripMenuItem
            // 
            this.nNToolStripMenuItem.Name = "nNToolStripMenuItem";
            this.nNToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.nNToolStripMenuItem.Text = "NN";
            // 
            // loadWeightsToolStripMenuItem
            // 
            this.loadWeightsToolStripMenuItem.Name = "loadWeightsToolStripMenuItem";
            this.loadWeightsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.loadWeightsToolStripMenuItem.Text = "LoadWeights";
            // 
            // cb_useCuda
            // 
            this.cb_useCuda.AutoSize = true;
            this.cb_useCuda.Location = new System.Drawing.Point(566, 306);
            this.cb_useCuda.Name = "cb_useCuda";
            this.cb_useCuda.Size = new System.Drawing.Size(80, 17);
            this.cb_useCuda.TabIndex = 3;
            this.cb_useCuda.Text = "checkBox1";
            this.cb_useCuda.UseVisualStyleBackColor = true;
            this.cb_useCuda.CheckedChanged += new System.EventHandler(this.cb_useCuda_CheckedChanged);
            // 
            // pbCartesianBox
            // 
            this.pbCartesianBox.BackColor = System.Drawing.SystemColors.Control;
            this.pbCartesianBox.Location = new System.Drawing.Point(13, 27);
            this.pbCartesianBox.Name = "pbCartesianBox";
            this.pbCartesianBox.Size = new System.Drawing.Size(547, 411);
            this.pbCartesianBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCartesianBox.TabIndex = 4;
            this.pbCartesianBox.TabStop = false;
            this.pbCartesianBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCartesianBox_Paint);
            this.pbCartesianBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbCartesianBox_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.lblClassSamples);
            this.groupBox1.Controls.Add(this.comboxInputs);
            this.groupBox1.Controls.Add(this.lblNoOfClasses);
            this.groupBox1.Controls.Add(this.comboxNoOfClasses);
            this.groupBox1.Location = new System.Drawing.Point(566, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 135);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(88, 106);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "SET";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // lblClassSamples
            // 
            this.lblClassSamples.AutoSize = true;
            this.lblClassSamples.Location = new System.Drawing.Point(7, 65);
            this.lblClassSamples.Name = "lblClassSamples";
            this.lblClassSamples.Size = new System.Drawing.Size(92, 13);
            this.lblClassSamples.TabIndex = 3;
            this.lblClassSamples.Text = "Inputs For Class #";
            // 
            // comboxInputs
            // 
            this.comboxInputs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxInputs.Enabled = false;
            this.comboxInputs.FormattingEnabled = true;
            this.comboxInputs.Location = new System.Drawing.Point(139, 62);
            this.comboxInputs.Name = "comboxInputs";
            this.comboxInputs.Size = new System.Drawing.Size(121, 21);
            this.comboxInputs.TabIndex = 2;
            // 
            // lblNoOfClasses
            // 
            this.lblNoOfClasses.AutoSize = true;
            this.lblNoOfClasses.Location = new System.Drawing.Point(7, 22);
            this.lblNoOfClasses.Name = "lblNoOfClasses";
            this.lblNoOfClasses.Size = new System.Drawing.Size(97, 13);
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
            this.comboxNoOfClasses.Location = new System.Drawing.Point(139, 19);
            this.comboxNoOfClasses.Name = "comboxNoOfClasses";
            this.comboxNoOfClasses.Size = new System.Drawing.Size(121, 21);
            this.comboxNoOfClasses.TabIndex = 0;
            this.comboxNoOfClasses.SelectedIndexChanged += new System.EventHandler(this.comboxNoOfClasses_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1054, 663);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbCartesianBox);
            this.Controls.Add(this.cb_useCuda);
            this.Controls.Add(this.btn_addWcpu);
            this.Controls.Add(this.btn_addWCuda);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Neural Networks";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCartesianBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_addWCuda;
        private System.Windows.Forms.Button btn_addWcpu;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWeightsToolStripMenuItem;
        private System.Windows.Forms.CheckBox cb_useCuda;
        private System.Windows.Forms.PictureBox pbCartesianBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboxNoOfClasses;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Label lblClassSamples;
        private System.Windows.Forms.ComboBox comboxInputs;
        private System.Windows.Forms.Label lblNoOfClasses;
    }
}

