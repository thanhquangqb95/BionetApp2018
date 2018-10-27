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
using DevExpress.XtraGrid.Views.Grid;
using BioNetModel.Data;
using System.Data.Linq;
using BioNetSangLocSoSinh.DiaglogFrm;
using System.IO;
using DevExpress.XtraGrid.Columns;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmDMDonViCoSo : DevExpress.XtraEditors.XtraForm
    {
        private string codeGenOld = string.Empty;
        public FrmDMDonViCoSo()
        {
            InitializeComponent();
        }

        private void FrmDMDonViCoSo_Load(object sender, EventArgs e)
        {
            this.repositoryItemLookUpEdit_ChiCuc.DataSource = BioBLL.GetListChiCuc();
            this.repositoryItemLookUpEdit_ChiCuc.ValueMember = "MaChiCuc";
            this.repositoryItemLookUpEdit_ChiCuc.DisplayMember = "TenChiCuc";
            DataTable dtkq = new DataTable();
            dtkq.Columns.Add("id", typeof(int));
            dtkq.Columns.Add("name", typeof(string));
            dtkq.Rows.Add(1, "Theo trung tâm");
            dtkq.Rows.Add(2, "Theo trung tâm kiểu 2");
            dtkq.Rows.Add(3, "Theo đơn vị");
            dtkq.Rows.Add(4, "Theo đơn vị kiểu 2");
            dtkq.Rows.Add(5, "Theo đơn vị kiểu 3");
            this.repositoryItemLookUpEdit_KieuTraKetQua.DataSource = dtkq;
            this.repositoryItemLookUpEdit_KieuTraKetQua.DisplayMember = "name";
            this.repositoryItemLookUpEdit_KieuTraKetQua.ValueMember = "id";
            this.gridControl_DonViCoSo.DataSource = BioBLL.GetListDVCS();
            AddItemForm();
        }

        private void gridView_DonViCoSo_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                int rowfocus = e.RowHandle;
                if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(rowfocus, col_th_TenDVCS))))
                {
                    e.Valid = false;
                    view.SetColumnError(col_th_TenDVCS, "Tên đơn vị cơ sở không được để trống!");
                }
                if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(rowfocus, col_th_MaChiCuc))))
                {
                    e.Valid = false;
                    view.SetColumnError(col_th_MaChiCuc, "Chi cục không được để trống!");
                }
                if (e.Valid)
                {
                    byte[] byteNull = ASCIIEncoding.ASCII.GetBytes("");
                    PSDanhMucDonViCoSo donVi = new PSDanhMucDonViCoSo();
                    if (string.IsNullOrEmpty(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "RowIDDVCS").ToString()))
                        donVi.RowIDDVCS = 0;
                    else
                    donVi.RowIDDVCS = Convert.ToInt16(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "RowIDDVCS").ToString());
                    donVi.MaDVCS = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "MaDVCS").ToString();
                    donVi.TenDVCS = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "TenDVCS").ToString();
                    donVi.DiaChiDVCS = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "DiaChiDVCS").ToString();                   
                    if(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "SDTCS")==null)
                    {
                        donVi.SDTCS = string.Empty;
                    }
                    else
                    {
                        donVi.SDTCS = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "SDTCS").ToString();
                    }
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "VietTatDV") == null)
                    {
                        donVi.VietTatDV = string.Empty;
                    }
                    else
                    {
                        donVi.VietTatDV = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "VietTatDV").ToString();
                    }
                    // donVi.Email = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, col_th_Email).ToString();
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "EmailTC")==null)
                    {
                        donVi.EmailTC = string.Empty;
                    }
                    else
                    {
                        donVi.EmailTC = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "EmailTC").ToString();
                    }
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "Email") == null)
                    {
                        donVi.Email = string.Empty;
                    }
                    else
                    {
                        donVi.Email = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "Email").ToString();
                    }

                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "Logo")==null)
                        donVi.Logo = new Binary(byteNull);
                    else
                        donVi.Logo = (Binary)gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "Logo");
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "ChuKiDonVi")==null)
                        donVi.ChuKiDonVi = new Binary(byteNull);
                    else
                        donVi.ChuKiDonVi = (Binary)gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "ChuKiDonVi");
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "HeaderReport")==null)
                        donVi.HeaderReport = new Binary(byteNull);
                    else
                        donVi.HeaderReport = (Binary)gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "HeaderReport");
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "Stt")==null)
                        donVi.Stt = 0;
                    else
                        donVi.Stt = Convert.ToInt16(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "Stt").ToString());
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "isLocked")==null)
                        donVi.isLocked = false;
                    else
                        donVi.isLocked = Convert.ToBoolean(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "isLocked").ToString());
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "isGuiMailTC")==null)
                        donVi.isGuiMailTC = false;
                    else
                        donVi.isGuiMailTC = Convert.ToBoolean(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "isGuiMailTC").ToString());
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "isChoPhepGuiSMS") == null)
                        donVi.isChoPhepGuiSMS = false;
                    else
                        donVi.isChoPhepGuiSMS = Convert.ToBoolean(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "isChoPhepGuiSMS").ToString());

                    donVi.MaChiCuc = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "MaChiCuc").ToString();
                    if (gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "KieuTraKetQua")==null)
                        donVi.KieuTraKetQua = 1;
                    else
                        donVi.KieuTraKetQua = int.Parse(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "KieuTraKetQua").ToString());
                    if(gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "TenBacSiDaiDien")==null)
                    {
                        donVi.TenBacSiDaiDien = string.Empty;
                    }
                    else
                    donVi.TenBacSiDaiDien = gridView_DonViCoSo.GetRowCellValue(e.RowHandle, "TenBacSiDaiDien").ToString();             
                    donVi.isDongBo = false;

                        PSDanhMucDonViCoSo dvOld = BioBLL.GetDonViCoSoById(donVi.RowIDDVCS);
                        if (BioBLL.UpdDonViCS(donVi))
                        {
                            PSDanhMucDonViCoSo dvNew = BioBLL.GetDonViCoSoById(donVi.RowIDDVCS);
                            if (dvOld.MaChiCuc != donVi.MaChiCuc)
                                XtraMessageBox.Show("Thay đổi mã đơn vị " + donVi.MaDVCS + " thành " + dvNew.MaDVCS+ " thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            XtraMessageBox.Show("Cập nhật đơn vị cơ sở thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show("Cập nhật đơn vị cơ sở thất bại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    //}
                    this.gridControl_DonViCoSo.DataSource = BioBLL.GetListDVCS();
                }
            }
            catch { XtraMessageBox.Show("Thao tác thất bại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private bool CheckCodeExist(string maDonViCS)
        {
            if (!BioBLL.CheckExistCodeDVCS(maDonViCS))
            {
                XtraMessageBox.Show("Mã đơn vị cơ sở đã tồn tại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private string InputForm(string codeGen)
        {
            FrmInputCode frm = new FrmInputCode(codeGen.Substring(5, 3), codeGen.Substring(0, 5), 3);
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
                codeGen = frm.name + frm.code;
            return codeGen;
        }

        private void gridControl_DonViCoSo_ProcessGridKey(object sender, KeyEventArgs e)
        {
        }

        private void repositoryItemPictureEdit_logo_Click(object sender, EventArgs e)
        {
            fileLogo.ShowHelp = true;
            fileLogo.FileName = string.Empty;
            fileLogo.Filter = "Images (*.jpg)|*.jpg|All Files(*.*)|*.*";
            fileLogo.ShowDialog();
        }

        private void repositoryItemPictureEdit_header_Click(object sender, EventArgs e)
        {
            fileHeader.ShowHelp = true;
            fileHeader.FileName = string.Empty;
            fileHeader.Filter = "Images (*.jpg)|*.jpg|All Files(*.*)|*.*";
            fileHeader.ShowDialog();
        }

        private void fileLogo_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                var fileBytes = File.ReadAllBytes(fileLogo.FileName);
                var image = new Binary(fileBytes);
                gridView_DonViCoSo.SetFocusedRowCellValue(col_th_Logo, image);
            }
            catch { }
        }

        private void fileHeader_FileOk(object sender, CancelEventArgs e)
        {
            try
            {

                var fileBytes = File.ReadAllBytes(fileHeader.FileName);
                var image = new Binary(fileBytes);
                gridView_DonViCoSo.SetFocusedRowCellValue(col_th_HeaderReport, image);
            }
            catch { }
        }

        private void gridView_DonViCoSo_RowCellClick(object sender, RowCellClickEventArgs e)
        {
           
        }

        private void repositoryItemPictureEditChuKiDonVi_Click(object sender, EventArgs e)
        {
            fileChuKiDonVi.ShowHelp = true;
            fileChuKiDonVi.FileName = string.Empty;
            fileChuKiDonVi.Filter = "Images (*.jpg)|*.jpg|All Files(*.*)|*.*";
            fileChuKiDonVi.ShowDialog();
        }

        private void fileChuKiDonVi_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                var fileBytes = File.ReadAllBytes(fileChuKiDonVi.FileName);
                var image = new Binary(fileBytes);
                gridView_DonViCoSo.SetFocusedRowCellValue(col_ChuKiDonVi, image);
            }
            catch { }
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
    }
}