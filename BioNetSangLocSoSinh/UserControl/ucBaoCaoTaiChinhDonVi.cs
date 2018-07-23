using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BioNetBLL;
using System.Text.RegularExpressions;

namespace BioNetSangLocSoSinh.UserControl
{
    public partial class ucBaoCaoTaiChinhDonVi : DevExpress.XtraEditors.XtraUserControl
    {
        public ucBaoCaoTaiChinhDonVi()
        {
            InitializeComponent();
        }

        private void btnLayDuLieu_Click(object sender, EventArgs e)
        {
            LoadDuLieu();
        }
        private void LoadDuLieu()
        {
            string MaDonVi = String.Empty;

            if (this.txtDonVi.EditValue.ToString() == "all")
            {
                if (this.txtChiCuc.EditValue.ToString() == "all")
                {
                    this.lblTenDonVi.Text = "Thông kê toàn bộ trung tâm";
                    MaDonVi = "all";
                }
                else
                {
                    this.lblTenDonVi.Text = "Thông kê chi cục " + this.txtChiCuc.Text.ToString();
                    MaDonVi = this.txtChiCuc.EditValue.ToString();
                }
            }
            else
            {
                this.lblTenDonVi.Text = "Thông kê đơn vị " + this.txtDonVi.Text.ToString();
                MaDonVi = this.txtDonVi.EditValue.ToString();
            }
             var kq= BioNetBLL.BioNet_Bus.GetBaoCaoTaiChinh(MaDonVi, this.dllNgay.tungay.Value.Date, this.dllNgay.denngay.Value.Date);
            this.GCBaoCaoTaiChinh.DataSource = kq;


        }

        private void lblTenDonVi_Click(object sender, EventArgs e)
        {

        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (this.GVBaoCaoTaiChinh.RowCount > 0)
            {
                this.ExportDataToExcelFile();
            }
            else
            {
                XtraMessageBox.Show("Không có dữ liệu, vui lòng lấy dữ liệu lại và kiểm tra điều kiện lọc.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportDataToExcelFile()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            // ofd.Multiselect = false;
            ofd.Filter = "Excel File(*.xlsx)|*.xlsx";
            string TenDonVi =convertToUnSign3(this.lblTenDonVi.Text);
            ofd.FileName = "BaoCaoTaiChinh_" + TenDonVi +"_"+ this.dllNgay.tungay.Value.Date.ToString("ddMMyyyy") +"_"+ this.dllNgay.denngay.Value.Date.ToString("ddMMyyyy")+ ".xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {

                        this.GVBaoCaoTaiChinh.ExportToXlsx(ofd.FileName);
                        //this.GCBaoCaoTuyChon.ExportToXlsx(ofd.FileName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể lưu file này! Vui lòng chọn đường dẫn khác.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        private void ucBaoCaoTaiChinhDonVi_Load(object sender, EventArgs e)
        {
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.txtDonVi.EditValue = "all";
            this.txtChiCuc.EditValue = "all";
            this.repositoryItemGridLookUpEditMaGoi.DataSource = BioNet_Bus.GetDanhsachGoiDichVuChung(); ;
            this.repositoryItemGridLookUpEditMaGoi.DisplayMember = "TenGoiDichVuChung";
            this.repositoryItemGridLookUpEditMaGoi.ValueMember = "IDGoiDichVuChung";
            this.LoadDuLieu();
        }

        private void txtChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.txtDonVi.EditValue = "all";
            }
            catch { }
        }
    }
}
