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
        public bool edit;
        private void FrmDanhMucGhiChuXN_Load(object sender, EventArgs e)
        {
            this.groupControlAdd.Visible = false;
            LoadDanhSach();
            PSEmployee emp = BioNet_Bus.GetThongTinNhanVien(employee.EmployeeCode);
            if(emp!=null)
            {
                if(emp.PositionCode==1)
                {
                    this.Edit(true);
                    edit = true;
                }
                else
                {
                    this.Edit(false);
                    edit = false;
                }
            }
            else
            {
                this.Edit(false);
                edit = false;
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
                this.btnAdd.Enabled = true;
            }
            else
            {
                col_isDung.OptionsColumn.AllowEdit = false;
                col_VietTat.OptionsColumn.AllowEdit = false;
                col_YNghia.OptionsColumn.AllowEdit = false;
                this.btnAdd.Enabled = false;
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
                    ghiChu.VietTatGhiChu = view.GetRowCellValue(rowfocus, col_VietTat).ToString().ToUpper();
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
                view.SetRowCellValue(e.RowHandle, col_IDRow, 0);
                view.SetRowCellValue(e.RowHandle, col_VietTat, view.GetRowCellValue(e.RowHandle, col_VietTat).ToString());
                view.SetRowCellValue(e.RowHandle, col_YNghia, view.GetRowCellValue(e.RowHandle, col_VietTat).ToString());
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

            if(e.KeyCode==Keys.Delete && edit == true)
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

        
        private void GVDMGhiChuXN_RowCellClick(object sender, RowCellClickEventArgs e)
        {

          
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.groupControlAdd.Visible = true;
            this.txtVietTat.ResetText();
            this.txtYNghia.ResetText();
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAddOk_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtVietTat.Text) && !string.IsNullOrEmpty(txtYNghia.Text))
                {
                    PSDanhMucGhiChuXN ghiChu = new PSDanhMucGhiChuXN();
                    ghiChu.IDRowGhiChuXN = 0;
                    ghiChu.VietTatGhiChu = txtVietTat.Text.ToUpper();
                    ghiChu.NoiDungGhiChuTruoc = txtYNghia.Text;
                    ghiChu.isSuDung = true;
                    if (BioNet_Bus.AddNewDanhMucGC(ghiChu))
                    {
                        DiaglogFrm.FrmOK frmOk = new DiaglogFrm.FrmOK("Thêm ghi chú thành công");
                        frmOk.ShowDialog();
                        this.groupControlAdd.Hide();
                        this.LoadDanhSach();
                    }
                    else
                    {
                        DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Thêm ghi chú lỗi.");
                        warning.ShowDialog();
                    }
                }
                else
                {
                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Không được để trống viết tắt và ý nghĩa.");
                    warning.ShowDialog();
                }
            }
            catch
            {
                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Thêm ghi chú lỗi.");
                warning.ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.groupControlAdd.Visible = false;
        }
    }
}