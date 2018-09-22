using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BioNetBLL;
using BioNetModel.Data;
using DevExpress.XtraGrid.Views.Grid;
using BioNetModel;
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmKhoiPhucDongBo : DevExpress.XtraEditors.XtraForm
    {
        public FrmKhoiPhucDongBo()
        {
            InitializeComponent();
        }

        private List<PSDanhMucDonViCoSo> lstDonViResponsitory = new List<PSDanhMucDonViCoSo>();
        private void txtMaPhieu_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadListDS();
        }
        private void LoadListDS()
        {
            bool kq;
            if (cbbTrangThaiPhieu.SelectedIndex==0)
            {
                kq = false;
            }
            else
                kq = true;

            GCDanhSachChuaDongBo.DataSource = BioNet_Bus.GetFullPhieuSangLoc(this.dateNgayBD.DateTime, this.dateNgayKetThuc.DateTime, this.txtDonVi.EditValue.ToString(),this.txtChiCuc.EditValue.ToString(),false,kq); ;
            GCDanhSachDaDongBo.DataSource = BioNet_Bus.GetFullPhieuSangLoc(this.dateNgayBD.DateTime, this.dateNgayKetThuc.DateTime, this.txtDonVi.EditValue.ToString(), this.txtChiCuc.EditValue.ToString(), true,kq);
        }
        private void LoadLookDonViGC()
        {
            this.lstDonViResponsitory.Clear();
            this.lstDonViResponsitory = BioNet_Bus.GetDanhSachDonViCoSo();
            this.repositoryItemLookUpEdit1.DataSource = BioNet_Bus.GetDanhSachDonViCoSo();
            this.repositoryItemLookUpDonVu_GCDanhSachCho.DataSource = BioNet_Bus.GetDanhSachDonViCoSo();
        }
        private void FrmKhoiPhucDongBo_Load(object sender, EventArgs e)
        {
            cbbTrangThaiPhieu.SelectedIndex = 0;
            this.GCDanhSachChuaDongBo.DataSource = null;
            this.GCDanhSachDaDongBo.DataSource = null;
            LoadLookDonViGC();
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.txtChiCuc.EditValue = "all";
            this.txtDonVi.EditValue = "all";
            this.dateNgayBD.EditValue = DateTime.Now;
            this.dateNgayKetThuc.EditValue = DateTime.Now;
            this.btnTimKiem.Enabled = true;
        }

        private void txtChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = txtChiCuc.EditValue.ToString();
                this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
            }
            catch { }
        }

       

        private void btnHoanDongBo_Click(object sender, EventArgs e)
        {
            PsReponse res = new PsReponse();
            SplashScreenManager.ShowForm(this, typeof(WaitingformLoadDongBo), true, true, false);
            res.Result = true;
            int[] lstChecked = this.GVDanhSachDaDongBo.GetSelectedRows();
            foreach (var index in lstChecked)
            {
                if (index >= 0)
                {
                    string IDPhieu= this.GVDanhSachDaDongBo.GetRowCellValue(index, this.col_maPhieu_GCDanhSachDaDB).ToString();
                    string IDCoSo = this.GVDanhSachDaDongBo.GetRowCellValue(index, this.col_maDonVi__GCDanhSachĐaB).ToString();
                    string MaBN = this.GVDanhSachDaDongBo.GetRowCellValue(index, this.col_maBenhNhan_DaDongBo).ToString();
                    PsReponse rese= BioNet_Bus.HoanDongBo(IDPhieu, IDCoSo, MaBN);
                   if(rese.Result==false)
                    {
                        res.Result = false;
                    }
                }
            }
            SplashScreenManager.CloseForm();
            if (res.Result == true)
            {
                XtraMessageBox.Show("Hoàn đồng bộ thành công", "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListDS();
            }
            else
            {
                XtraMessageBox.Show("Hoàn đồng bộ lỗi" + res.StringError, "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGanMaKhachHang_Click(object sender, EventArgs e)
        {
            PsReponse reponse= BioNet_Bus.UpdateMaKhachHang();
            if(reponse.Result)
            {
                XtraMessageBox.Show("khôi phục mã khách hàn thành công", "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                XtraMessageBox.Show("khôi phục mã khách hàn thất bại", "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}