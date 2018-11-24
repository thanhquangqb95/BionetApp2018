namespace BionetUpdate
{
    partial class FrmBionetUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBionetUpdate));
            this.label1 = new System.Windows.Forms.Label();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnSync = new DevExpress.XtraEditors.SimpleButton();
            this.progressBarControlDownload = new DevExpress.XtraEditors.ProgressBarControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControlDownload.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(38, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(390, 22);
            this.label1.TabIndex = 19;
            this.label1.Text = "Cập nhật phiên bản BionetSangLocSoSinh";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl1.Location = new System.Drawing.Point(136, 102);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(191, 13);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "Copyright © 2018 Bionet Việt Nam";
            // 
            // btnSync
            // 
            this.btnSync.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSync.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.btnSync.Appearance.Options.UseFont = true;
            this.btnSync.Appearance.Options.UseForeColor = true;
            this.btnSync.ImageOptions.Image = global::BionetUpdate.Properties.Resources.recycle;
            this.btnSync.Location = new System.Drawing.Point(70, 70);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(147, 26);
            this.btnSync.TabIndex = 22;
            this.btnSync.Text = "Cập nhật";
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // progressBarControlDownload
            // 
            this.progressBarControlDownload.Location = new System.Drawing.Point(15, 34);
            this.progressBarControlDownload.Name = "progressBarControlDownload";
            this.progressBarControlDownload.Size = new System.Drawing.Size(453, 30);
            this.progressBarControlDownload.TabIndex = 23;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.ForeColor = System.Drawing.Color.Firebrick;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Appearance.Options.UseForeColor = true;
            this.simpleButton1.ImageOptions.Image = global::BionetUpdate.Properties.Resources.x_button;
            this.simpleButton1.Location = new System.Drawing.Point(261, 70);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(147, 26);
            this.simpleButton1.TabIndex = 24;
            this.simpleButton1.Text = "Hủy";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmBionetUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImage = global::BionetUpdate.Properties.Resources._6478051_abstract_background;
            this.ClientSize = new System.Drawing.Size(480, 119);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.progressBarControlDownload);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmBionetUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cập nhật phiên bản";
            this.Load += new System.EventHandler(this.FrmBionetUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControlDownload.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton btnSync;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControlDownload;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}

