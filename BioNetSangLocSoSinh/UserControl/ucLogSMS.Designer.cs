namespace BioNetSangLocSoSinh.UserControl
{
    partial class ucLogSMS
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.rtbStatus = new System.Windows.Forms.RichTextBox();
            this.tấtCảSốToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tấtCảToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sốMẹToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sốĐiệnThoạiChaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thoátToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tấtCảSốToolStripMenuItem,
            this.thoátToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(838, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // rtbStatus
            // 
            this.rtbStatus.BackColor = System.Drawing.SystemColors.ControlText;
            this.rtbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbStatus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.rtbStatus.Location = new System.Drawing.Point(0, 24);
            this.rtbStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbStatus.Name = "rtbStatus";
            this.rtbStatus.Size = new System.Drawing.Size(838, 525);
            this.rtbStatus.TabIndex = 4;
            this.rtbStatus.Text = "";
            // 
            // tấtCảSốToolStripMenuItem
            // 
            this.tấtCảSốToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tấtCảToolStripMenuItem,
            this.sốMẹToolStripMenuItem,
            this.sốĐiệnThoạiChaToolStripMenuItem});
            this.tấtCảSốToolStripMenuItem.Name = "tấtCảSốToolStripMenuItem";
            this.tấtCảSốToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.tấtCảSốToolStripMenuItem.Text = "Chọn số";
            // 
            // tấtCảToolStripMenuItem
            // 
            this.tấtCảToolStripMenuItem.Name = "tấtCảToolStripMenuItem";
            this.tấtCảToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.tấtCảToolStripMenuItem.Text = "Tất cả";
            // 
            // sốMẹToolStripMenuItem
            // 
            this.sốMẹToolStripMenuItem.Name = "sốMẹToolStripMenuItem";
            this.sốMẹToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sốMẹToolStripMenuItem.Text = "Số điện thoại mẹ";
            this.sốMẹToolStripMenuItem.Click += new System.EventHandler(this.sốMẹToolStripMenuItem_Click);
            // 
            // sốĐiệnThoạiChaToolStripMenuItem
            // 
            this.sốĐiệnThoạiChaToolStripMenuItem.Name = "sốĐiệnThoạiChaToolStripMenuItem";
            this.sốĐiệnThoạiChaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sốĐiệnThoạiChaToolStripMenuItem.Text = "Số điện thoại cha";
            // 
            // thoátToolStripMenuItem
            // 
            this.thoátToolStripMenuItem.Name = "thoátToolStripMenuItem";
            this.thoátToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.thoátToolStripMenuItem.Text = "Thoát";
            this.thoátToolStripMenuItem.Click += new System.EventHandler(this.thoátToolStripMenuItem_Click);
            // 
            // ucLogSMS
            // 
            this.Appearance.BackColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 549);
            this.Controls.Add(this.rtbStatus);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ucLogSMS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lịch sử gửi tin";
            this.Load += new System.EventHandler(this.ucLogSMS_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tấtCảSốToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tấtCảToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sốMẹToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sốĐiệnThoạiChaToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rtbStatus;
        private System.Windows.Forms.ToolStripMenuItem thoátToolStripMenuItem;
    }
}
