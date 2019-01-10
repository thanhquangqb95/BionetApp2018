using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BioNetBLL;
using BioNetModel.Data;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmDMGoiDichVuChiTiet : DevExpress.XtraEditors.XtraForm
    {
        private string idServicePackage = string.Empty;
        private string Nhom = string.Empty;
        private DataTable dtGoiDV = new DataTable();
        private class PSDanhMucDichVuChon
        {
            public  PSDanhMucDichVu PSDanhMucDichVu { get; set; }
            public  bool? Check { get; set; }
        }
        private List<PSDanhMucDichVuChon> lstDV = new List<PSDanhMucDichVuChon>();
        public FrmDMGoiDichVuChiTiet()
        {
            InitializeComponent();
        }

        private void FrmDMGoiDichVuChiTiet_Load(object sender, EventArgs e)
        {
            gridControl_GoiDichVuChung.DataSource = BioBLL.GetListGoiDichVuChung();
            repositoryItemGridLookUpEditKyThuat.DataSource = BioBLL.GetDanhMucKyThuatXNs();


            //AddItemForm();
        }

        private void gridView_GoiDichVuChung_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            this.idServicePackage = gridView_GoiDichVuChung.GetRowCellValue(gridView_GoiDichVuChung.FocusedRowHandle, col_th_IDGoiDichVuChung).ToString();
            Nhom = gridView_GoiDichVuChung.GetRowCellValue(gridView_GoiDichVuChung.FocusedRowHandle, col_th_GroupTraKQ).ToString();
            CheckService(idServicePackage,Nhom);
        }

        private void CheckService(string id,string Nhom)
        {
            lstDV = new List<PSDanhMucDichVuChon>();
            if (string.IsNullOrEmpty(Nhom))
            {
                Nhom = "1";
            }
            var lst = BioBLL.GetListDichVuTheoNhom(int.Parse(Nhom));
            foreach (var ls in lst)
            {
                PSDanhMucDichVuChon dv = new PSDanhMucDichVuChon();
                dv.PSDanhMucDichVu = ls;
                dv.Check = false;
                lstDV.Add(dv);
            }
            List<string> lstService = new List<string>();
            lstService = BioBLL.GetListServicePackageByIDGoi(id).Select(x => x.IDDichVu).ToList();
            foreach (var row in this.lstDV)
            {
                if (lstService.FirstOrDefault(x=>x.Equals(row.PSDanhMucDichVu.IDDichVu.ToString()))!=null)
                    row.Check = true;
                else
                    row.Check = false;
            }
            gridControl_DichVu.DataSource = lstDV;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(this.idServicePackage))
            {
                XtraMessageBox.Show("Chưa chọn gói dịch vụ!", "Bệnh viện điện tử .NET", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<string> lstService = new List<string>();
            foreach (DataRow row in this.dtGoiDV.Rows)
            {
                if (Convert.ToBoolean(row["Check"]))
                    lstService.Add(row["IDDichVu"].ToString());
            }
            if(BioBLL.UpdDetailServicePackage(this.idServicePackage, lstService))
            {
                XtraMessageBox.Show("Cập nhật chi tiết gói dịch vụ thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                XtraMessageBox.Show("Cập nhật chi tiết gói dịch vụ thất bại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CheckService(this.idServicePackage,this.Nhom);
            }
        }

        private void btnLuuSTT_Click(object sender, EventArgs e)
        {
            try {
              
                List<PSDanhMucGoiDichVuChung> dt = (List<PSDanhMucGoiDichVuChung>)gridControl_GoiDichVuChung.DataSource;
                bool kq = BioBLL.UpdateGoiDV(dt);
                if(kq==true)
                {
                    XtraMessageBox.Show("Cập nhật số thứ tự thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(kq==false)
                {
                    XtraMessageBox.Show("Cập nhật số thứ tự thất bại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            { }               
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
            CustomLayouts.TransLanguage.Trans(this.Controls, idfo);
        }

        private void gridView_DichVu_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}