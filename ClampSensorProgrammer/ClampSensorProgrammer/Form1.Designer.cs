namespace ClampSensorProgrammer
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
            this.btnProgram = new System.Windows.Forms.Button();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.txtProgramStatus = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.comboComPorts = new System.Windows.Forms.ComboBox();
            this.btnRescan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnProgram
            // 
            this.btnProgram.Enabled = false;
            this.btnProgram.Location = new System.Drawing.Point(12, 12);
            this.btnProgram.Name = "btnProgram";
            this.btnProgram.Size = new System.Drawing.Size(100, 25);
            this.btnProgram.TabIndex = 0;
            this.btnProgram.Text = "Program and Test";
            this.btnProgram.UseVisualStyleBackColor = true;
            this.btnProgram.Click += new System.EventHandler(this.btnProgram_Click);
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.Location = new System.Drawing.Point(12, 70);
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.Size = new System.Drawing.Size(100, 20);
            this.txtSerialNumber.TabIndex = 1;
            this.txtSerialNumber.Text = "0123456789";
            this.txtSerialNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSerialNumber.TextChanged += new System.EventHandler(this.txtSerialNumber_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(19, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(43, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Status";
            // 
            // txtProgress
            // 
            this.txtProgress.Location = new System.Drawing.Point(12, 122);
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ReadOnly = true;
            this.txtProgress.Size = new System.Drawing.Size(100, 20);
            this.txtProgress.TabIndex = 3;
            this.txtProgress.Text = "IDLE";
            this.txtProgress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtProgress.TextChanged += new System.EventHandler(this.txtProgress_TextChanged);
            // 
            // txtProgramStatus
            // 
            this.txtProgramStatus.Location = new System.Drawing.Point(118, 9);
            this.txtProgramStatus.Multiline = true;
            this.txtProgramStatus.Name = "txtProgramStatus";
            this.txtProgramStatus.ReadOnly = true;
            this.txtProgramStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProgramStatus.Size = new System.Drawing.Size(310, 300);
            this.txtProgramStatus.TabIndex = 5;
            this.txtProgramStatus.TextChanged += new System.EventHandler(this.txtProgramStatus_TextChanged);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 227);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(100, 25);
            this.btnOpen.TabIndex = 6;
            this.btnOpen.Text = "Open COM";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // comboComPorts
            // 
            this.comboComPorts.FormattingEnabled = true;
            this.comboComPorts.Location = new System.Drawing.Point(12, 289);
            this.comboComPorts.Name = "comboComPorts";
            this.comboComPorts.Size = new System.Drawing.Size(100, 21);
            this.comboComPorts.TabIndex = 7;
            this.comboComPorts.SelectedIndexChanged += new System.EventHandler(this.comboComPorts_SelectedIndexChanged);
            // 
            // btnRescan
            // 
            this.btnRescan.Location = new System.Drawing.Point(12, 258);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(100, 25);
            this.btnRescan.TabIndex = 8;
            this.btnRescan.Text = "Rescan";
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 321);
            this.Controls.Add(this.btnRescan);
            this.Controls.Add(this.comboComPorts);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.txtProgramStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProgress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSerialNumber);
            this.Controls.Add(this.btnProgram);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clamp Sensor Programmer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProgram;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.TextBox txtProgramStatus;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ComboBox comboComPorts;
        private System.Windows.Forms.Button btnRescan;
    }
}

