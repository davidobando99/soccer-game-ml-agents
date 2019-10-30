namespace ClienteUDP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtDatos = new System.Windows.Forms.TextBox();
            this.butRead = new System.Windows.Forms.Button();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.axShockwaveFlash1 = new AxShockwaveFlashObjects.AxShockwaveFlash();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDatos
            // 
            this.txtDatos.Location = new System.Drawing.Point(87, 1);
            this.txtDatos.Name = "txtDatos";
            this.txtDatos.Size = new System.Drawing.Size(302, 20);
            this.txtDatos.TabIndex = 0;
            this.txtDatos.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // butRead
            // 
            this.butRead.Location = new System.Drawing.Point(136, 304);
            this.butRead.Name = "butRead";
            this.butRead.Size = new System.Drawing.Size(91, 36);
            this.butRead.TabIndex = 2;
            this.butRead.Text = "read";
            this.butRead.UseVisualStyleBackColor = true;
            this.butRead.Click += new System.EventHandler(this.butRead_Click);
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(30, 46);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(315, 233);
            this.axWindowsMediaPlayer1.TabIndex = 3;
            // 
            // axShockwaveFlash1
            // 
            this.axShockwaveFlash1.Enabled = true;
            this.axShockwaveFlash1.Location = new System.Drawing.Point(361, 46);
            this.axShockwaveFlash1.Name = "axShockwaveFlash1";
            this.axShockwaveFlash1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axShockwaveFlash1.OcxState")));
            this.axShockwaveFlash1.Size = new System.Drawing.Size(278, 233);
            this.axShockwaveFlash1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 379);
            this.Controls.Add(this.axShockwaveFlash1);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.butRead);
            this.Controls.Add(this.txtDatos);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDatos;
        private System.Windows.Forms.Button butRead;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash1;
    }
}