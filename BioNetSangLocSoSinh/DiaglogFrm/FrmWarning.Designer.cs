namespace BioNetSangLocSoSinh.DiaglogFrm
{
    partial class FrmWarning
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtErros = new System.Windows.Forms.RichTextBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblTry = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit1.EditValue = global::BioNetSangLocSoSinh.Properties.Resources.warning1;
            this.pictureEdit1.Location = new System.Drawing.Point(93, 3);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(98, 70);
            this.pictureEdit1.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.Black;
            this.lblHeader.Location = new System.Drawing.Point(90, 76);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(101, 25);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Warning";
            // 
            // txtErros
            // 
            this.txtErros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtErros.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtErros.Enabled = false;
            this.txtErros.Location = new System.Drawing.Point(20, 115);
            this.txtErros.Name = "txtErros";
            this.txtErros.ReadOnly = true;
            this.txtErros.Size = new System.Drawing.Size(245, 102);
            this.txtErros.TabIndex = 2;
            this.txtErros.Text = "";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(47, 269);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(193, 16);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "@Copyright 2018 Bionet Việt Nam";
            // 
            // lblTry
            // 
            this.lblTry.AutoSize = true;
            this.lblTry.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTry.ForeColor = System.Drawing.Color.IndianRed;
            this.lblTry.Location = new System.Drawing.Point(81, 231);
            this.lblTry.Name = "lblTry";
            this.lblTry.Size = new System.Drawing.Size(110, 25);
            this.lblTry.TabIndex = 4;
            this.lblTry.Text = "Try again";
            this.lblTry.Click += new System.EventHandler(this.lblTry_Click);
            // 
            // FrmWarning
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 296);
            this.ControlBox = false;
            this.Controls.Add(this.lblTry);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtErros);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.pictureEdit1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmWarning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmWarning_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.RichTextBox txtErros;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Label lblTry;
    }
}
