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
using DevExpress.XtraGrid.Views.Grid;
using BioNetBLL;
using BioNetModel.Data;
using BioNetModel;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmKhoiPhucCTChatLuongMau : DevExpress.XtraEditors.XtraForm
    {
        public FrmKhoiPhucCTChatLuongMau()
        {
            InitializeComponent();
        }

        private void btnKhoiPhucCTchatLuongMau_Click(object sender, EventArgs e)
        {
            SuaPhieu();
            AddItemForm();
        }
        private void SuaPhieu()
        {

            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn là khôi phục chất lượng mẫu đã chọn", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    var res = BioNet_Bus.RestoreCTChatLuongMau();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Khôi phục dữ liệu lỗi - " + ex, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                }
            }
            else if (dialogResult == DialogResult.No) { return; }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                this.GCDSCTDanhGiaChatLuongMau.DataSource = BioNet_Bus.GetCTChatLuogMau(this.txtMaPhieu.Text.Trim(), this.txtDonVi.EditValue.ToString(), this.txtChiCuc.EditValue.ToString(), this.dateNgayBD.DateTime, this.dateNgayKetThuc.DateTime);
                if (this.GVDSCTDanhGiaChatLuongMau.DataRowCount == 0)
                {
                    MessageBox.Show("Không có dữ liệu phiếu kết quả cần tìm", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                }
                else
                {
                }
            }
            catch
            {

            }

        }

        private void FrmKhoiPhucCTChatLuongMau_Load(object sender, EventArgs e)
        {
            this.GCDSCTDanhGiaChatLuongMau.DataSource = null;
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.txtDonVi.EditValue = "all";
            this.txtChiCuc.EditValue = "all";
            this.dateNgayBD.EditValue = DateTime.Now;
            this.dateNgayKetThuc.EditValue = DateTime.Now;
            this.btnTimKiem.Enabled = true;
        }
      
        private void GVDSCTDanhGiaChatLuongMau_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (this.GVDSCTDanhGiaChatLuongMau.RowCount > 0)
                {
                    if (this.GVDSCTDanhGiaChatLuongMau.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVDSCTDanhGiaChatLuongMau.GetRowCellValue(this.GVDSCTDanhGiaChatLuongMau.FocusedRowHandle, this.col_MaPhieu).ToString();
                        string maDonVi = this.GVDSCTDanhGiaChatLuongMau.GetRowCellValue(this.GVDSCTDanhGiaChatLuongMau.FocusedRowHandle, this.col_IDDonVi).ToString();
                        string maTiepNhan = this.GVDSCTDanhGiaChatLuongMau.GetRowCellValue(this.GVDSCTDanhGiaChatLuongMau.FocusedRowHandle, this.col_MaTiepNhan).ToString();
                        var phieu = BioNet_Bus.GetThongTinPhieu(maPhieu, maDonVi);
                        if (phieu != null)
                        {
                            this.radioDanhGia.SelectedIndex = (phieu.isKhongDat ?? false) == false ? 0 : 1;
                            phieu.lstLyDoKhongDat = BioNet_Bus.GetChiTietDanhGiaMạuKhongDatTrenPhieu(maPhieu, maTiepNhan);
                            this.checkedListBoxLydoKhongDat.DataSource = phieu.lstLyDoKhongDat;
                        }
                    }
                }
            }
            catch
            {

            }           
        }

        private void AddItemForm()
        {
            PSMenuForm fo = new PSMenuForm
            {
                NameForm = this.Name,
                Capiton = this.Text,
            };
            BioNet_Bus.AddMenuForm(fo);
            long? idfo = BioNet_Bus.GetMenuIDForm(this.Name);
            CustomLayouts.TransLanguage.AddItemCT(this.Controls, idfo);
        }
    }
}