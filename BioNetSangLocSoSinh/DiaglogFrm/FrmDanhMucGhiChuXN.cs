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
using BioNetModel.Data;
using BioNetBLL;
using BioNetModel;
using DevExpress.XtraGrid.Views.Grid;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmDanhMucGhiChuXN : DevExpress.XtraEditors.XtraForm
    {
        public FrmDanhMucGhiChuXN(PsEmployeeLogin psEmployee)
        {
            InitializeComponent();
            employee = psEmployee;
        }
        public PsEmployeeLogin employee = new PsEmployeeLogin();
        public List<PSDanhMucGhiChuXN> ghiChuXn = new List<PSDanhMucGhiChuXN>();
        private void FrmDanhMucGhiChuXN_Load(object sender, EventArgs e)
        {
            LoadDanhSach();
            PSEmployee emp = BioNet_Bus.GetThongTinNhanVien(employee.EmployeeCode);
            if(emp!=null)
            {
                if(emp.PositionCode==1)
                {
                    this.Edit(true);
                }
                else
                {
                    this.Edit(false);
                }
            }
            else
            {
                this.Edit(false);
            }
        }
        private void LoadDanhSach()
        {
            ghiChuXn = BioNet_Bus.GetDanhMucGhiXN();
            GCDMGhiChuXN.DataSource = null;
            GCDMGhiChuXN.DataSource = ghiChuXn;
        }
        private void Edit(bool value)
        {
            if(value)
            {
                col_isDung.OptionsColumn.AllowEdit = true;
                col_VietTat.OptionsColumn.AllowEdit = true;
                col_YNghia.OptionsColumn.AllowEdit = true;
            }
            else
            {
                col_isDung.OptionsColumn.AllowEdit = false;
                col_VietTat.OptionsColumn.AllowEdit = false;
                col_YNghia.OptionsColumn.AllowEdit = false;
            }
           
        }

        private void GVDMGhiChuXN_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                int rowfocus = e.RowHandle;
                if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(rowfocus, col_VietTat))))
                {
                    e.Valid = false;
                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Tên viết tắt không thể để trống.");
                    warning.ShowDialog();
                }
                if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(rowfocus, col_YNghia))))
                {
                    e.Valid = false;
                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Ý nghĩa không để trống.");
                    warning.ShowDialog();
                }
                if (e.Valid)
                {
                    PSDanhMucGhiChuXN ghiChu = new PSDanhMucGhiChuXN();
                    ghiChu.IDRowGhiChuXN =long.Parse(view.GetRowCellValue(rowfocus, col_IDRow).ToString());
                    ghiChu.VietTatGhiChu = view.GetRowCellValue(rowfocus, col_VietTat).ToString();
                    ghiChu.NoiDungGhiChuTruoc = view.GetRowCellValue(rowfocus, col_YNghia).ToString();
                    ghiChu.isSuDung = Boolean.Parse(view.GetRowCellValue(rowfocus, col_isDung).ToString());
                    if (BioNet_Bus.UpdateDanhMucGC(ghiChu))
                    {
                        DiaglogFrm.FrmOK frmOk = new DiaglogFrm.FrmOK("Cập nhật ghi chú thành công");
                        frmOk.ShowDialog();
                    }
                    else
                    {
                        DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Cập nhật ghi chú lỗi.");
                        warning.ShowDialog();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Cập nhật ghi chú lỗi." +ex.ToString());
                warning.ShowDialog();
            }
            LoadDanhSach();
        }

        private void GVDMGhiChuXN_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
           try
            {
               
                GridView view = sender as GridView;
                int rowfocus = e.RowHandle;
                bool valid = true;
                if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(rowfocus, col_VietTat))))
                {
                    view.SetColumnError(col_VietTat, "Tên viết tắt không để trống");
                    valid = false;
                }
                if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(rowfocus, col_YNghia))))
                {
                    view.SetColumnError(col_VietTat, "Ý nghĩa không để trống!");
                    valid = false;
                }
                if (valid)
                {
                    PSDanhMucGhiChuXN ghiChu = new PSDanhMucGhiChuXN();
                    ghiChu.IDRowGhiChuXN = 0;
                    ghiChu.VietTatGhiChu = view.GetRowCellValue(rowfocus, col_VietTat).ToString();
                    ghiChu.NoiDungGhiChuTruoc = view.GetRowCellValue(rowfocus, col_YNghia).ToString();
                    ghiChu.isSuDung = Boolean.Parse(view.GetRowCellValue(rowfocus, col_isDung).ToString());
                    if (BioNet_Bus.AddNewDanhMucGC(ghiChu))
                    {
                        DiaglogFrm.FrmOK frmOk = new DiaglogFrm.FrmOK("Thêm ghi chú thành công");
                        frmOk.ShowDialog();
                    }
                    else
                    {
                        DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Thêm ghi chú lỗi.");
                        warning.ShowDialog();
                    }

                }
            }
            catch (Exception ex)
            {
                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Thêm ghi chú lỗi." + ex.ToString());
                warning.ShowDialog();
            }
            LoadDanhSach();
        }

        private void GVDMGhiChuXN_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Delete )
            {
                try
                {
                    if (MessageBox.Show("Bạn có chắn chắn xóa ghi chú không?", "Cảnh báo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    GridView view = sender as GridView;
                    int rowfocus = view.FocusedRowHandle;
                    PSDanhMucGhiChuXN ghiChu = new PSDanhMucGhiChuXN();
                    ghiChu.IDRowGhiChuXN = long.Parse(view.GetRowCellValue(rowfocus, col_IDRow).ToString());
                    ghiChu.VietTatGhiChu = view.GetRowCellValue(rowfocus, col_VietTat).ToString();
                    ghiChu.NoiDungGhiChuTruoc = view.GetRowCellValue(rowfocus, col_YNghia).ToString();
                    ghiChu.isSuDung = Boolean.Parse(view.GetRowCellValue(rowfocus, col_isDung).ToString());
                    BioNet_Bus.DeleteDanhMucGC(ghiChu);
                    DiaglogFrm.FrmOK frmOk = new DiaglogFrm.FrmOK("Xóa ghi chú thành công");
                    frmOk.ShowDialog();
                }
                catch(Exception ex)
                {
                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Xóa ghi chú lỗi." + ex.ToString());
                    warning.ShowDialog();
                }
                LoadDanhSach();
            }
        }

        private void GVDMGhiChuXN_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            try
            {

                GridView view = sender as GridView;
                int rowfocus = e.RowHandle;
                bool valid = true;
                if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(rowfocus, col_VietTat))))
                {
                    view.SetColumnError(col_VietTat, "Tên viết tắt không để trống");
                    valid = false;
                }
                if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(rowfocus, col_YNghia))))
                {
                    view.SetColumnError(col_VietTat, "Ý nghĩa không để trống!");
                    valid = false;
                }
                if (valid)
                {
                    PSDanhMucGhiChuXN ghiChu = new PSDanhMucGhiChuXN();
                    ghiChu.IDRowGhiChuXN = 0;
                    ghiChu.VietTatGhiChu = view.GetRowCellValue(rowfocus, col_VietTat).ToString();
                    ghiChu.NoiDungGhiChuTruoc = view.GetRowCellValue(rowfocus, col_YNghia).ToString();
                    ghiChu.isSuDung = Boolean.Parse(view.GetRowCellValue(rowfocus, col_isDung).ToString());
                    if (BioNet_Bus.AddNewDanhMucGC(ghiChu))
                    {
                        DiaglogFrm.FrmOK frmOk = new DiaglogFrm.FrmOK("Thêm ghi chú thành công");
                        frmOk.ShowDialog();
                    }
                    else
                    {
                        DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Thêm ghi chú lỗi.");
                        warning.ShowDialog();
                    }

                }
            }
            catch (Exception ex)
            {
                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Thêm ghi chú lỗi." + ex.ToString());
                warning.ShowDialog();
            }
            LoadDanhSach();
        }

        private void GVDMGhiChuXN_RowCellClick(object sender, RowCellClickEventArgs e)
        {

          
        }
    }
}