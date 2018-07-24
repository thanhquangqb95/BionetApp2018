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
using DevExpress.XtraGrid.Views.Grid;
using BioNetBLL;
using BioNetModel;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmDMLanguage : DevExpress.XtraEditors.XtraForm
    {
        public FrmDMLanguage()
        {
            InitializeComponent();
        }
        public static long? idFo;
        public static List<PSMenuForm> fo = new List<PSMenuForm>();
        private void FrmDMLanguage_Load(object sender, EventArgs e)
        {          
            fo = BioNetBLL.BioNet_Bus.GetMenuForm();
            GCForm.DataSource = fo;
            AddItemForm();
        }

        private void GVForm_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (this.GVForm.RowCount > 0)
                {
                    if (this.GVForm.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVForm.GetRowCellValue(this.GVForm.FocusedRowHandle, this.col_Form).ToString();
                         idFo = long.Parse(maPhieu);
                        GCItem.DataSource = BioNetBLL.BioNet_Bus.TransAll(idFo); 
                    }
                }
            }
            catch { }          
        }    

        private void GVItem_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                int rowfocus = e.RowHandle;
                if (e.Valid)
                {
                    PSMenuTrans item = new PSMenuTrans();
                    item.IDItem = Convert.ToInt16(GVItem.GetRowCellValue(e.RowHandle, col_IDItem).ToString());
                    item.ItemName = GVItem.GetRowCellValue(e.RowHandle, col_ITemName).ToString();
                    item.VN = GVItem.GetRowCellValue(e.RowHandle, col_ItemVN).ToString();
                    item.Trans= GVItem.GetRowCellValue(e.RowHandle, col_ItemEN).ToString();
                    // PSMenuItem idold = BioBLL.GetMenuItemById(item.IDItem);
                    PsReponse reponse= BioBLL.UpdateMenuItemById(item);
                    if (reponse.Result)
                    {
                       // XtraMessageBox.Show("Cập nhật đơn vị cơ sở thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("Cập nhật từ điển thất bại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //}
                    fo = BioNetBLL.BioNet_Bus.GetMenuForm();
                    GCForm.DataSource = fo;
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
            CustomLayouts.TransLanguage.Trans(this.Controls, idfo);
        }

        
    }
}