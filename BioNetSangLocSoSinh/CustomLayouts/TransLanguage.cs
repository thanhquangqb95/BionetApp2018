using BioNetBLL;
using BioNetModel;
using BioNetModel.Data;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BioNetSangLocSoSinh.CustomLayouts
{
    public partial class TransLanguage
    {
        public static long? idFO;

        public static void AddItemCT(Control.ControlCollection con, long? idfo)
        {
            List<PSMenuItem> MenuAddItem = new List<PSMenuItem>();
            idFO = idfo;
            foreach (Control item in con)
            {
                if ((!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.Text)) &&
                    !(item.GetType() == typeof(DateEdit) || item.GetType() == typeof(PictureEdit) || item.GetType() == typeof(PictureBox) || item.Name.Equals("cbbLanguage")))
                {
                    PSMenuItem ctmenu = new PSMenuItem
                    {
                        ItemName = item.Name,
                        VN = item.Text,
                        ItemType = item.GetType().ToString(),
                    };
                    MenuAddItem.Add(ctmenu);
                }
                if (item.GetType() == typeof(RadioGroup))
                {
                    RadioGroup ra = (RadioGroup)item;
                    for (int i = 0; i < ra.Properties.Items.Count; i++)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = ra.Name + ".Item." + i,
                            VN = ra.Properties.Items[i].ToString(),
                            ItemType = item.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                }
                else if (item.GetType() == typeof(ComboBoxEdit) || item.GetType() == typeof(ImageComboBoxEdit))
                {
                    if (!item.Equals("cbbLanguage"))
                    {
                        ComboBoxEdit com = (ComboBoxEdit)item;
                        for (int i = 0; i < com.Properties.Items.Count; i++)
                        {
                            PSMenuItem ctmenu1 = new PSMenuItem
                            {
                                ItemName = com.Name + ".Item." + i,
                                VN = com.Properties.Items[i].ToString(),
                                ItemType = item.GetType().ToString(),
                            };
                            MenuAddItem.Add(ctmenu1);
                        }
                    }
                }
                else if (item.GetType() == typeof(DevExpress.XtraGrid.GridControl))
                {
                    DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)item;
                    foreach (var ctgv in gc.Views)
                    {
                        GridView gv = (GridView)ctgv;
                        foreach (DevExpress.XtraGrid.Columns.GridColumn col in gv.Columns)
                        {
                            PSMenuItem ctmenu1 = new PSMenuItem
                            {
                                ItemName = col.Name,
                                VN = col.Caption,
                                ItemType = item.GetType().ToString(),
                            };
                            MenuAddItem.Add(ctmenu1);
                        }
                    }
                }
                else if (item.GetType() == typeof(RibbonStatusBar))
                {
                    RibbonStatusBar ba = (RibbonStatusBar)item;
                    var it = ba.ItemLinks.ToList();
                    foreach (var i in it)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = i.Item.Name,
                            VN = i.Caption,
                            ItemType = item.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                }
                else if (item.GetType() == typeof(RibbonControl))
                {
                    RibbonControl ba = (RibbonControl)item;
                    var it = ba.Items.ToList();
                    foreach (var i in it)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = i.Name,
                            VN = i.Caption,
                            ItemType = i.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                    var page = ba.Pages.ToList();
                    foreach (var pa in page)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = pa.Name,
                            VN = pa.Text,
                            ItemType = pa.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                }
                else if (item.GetType() == typeof(LookUpEdit))
                {
                    PSMenuItem ctmenu = new PSMenuItem
                    {
                        ItemName = item.Name,
                        VN = item.Text,
                        ItemType = item.GetType().ToString(),
                    };
                    MenuAddItem.Add(ctmenu);
                }
                else
                {
                    AddControl(item);
                }
            }
            BioNet_Bus.AddMenuItem(MenuAddItem, idFO);
        }

        public static void AddControl(Control minicontrol)
        {
            List<PSMenuItem> MenuAddItem = new List<PSMenuItem>();
            foreach (Control item in minicontrol.Controls)
            {
                if ((!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.Text)) &&
                     !(item.GetType() == typeof(DateEdit) || item.GetType() == typeof(PictureEdit) || item.GetType() == typeof(PictureBox) || item.Name.Equals("cbbLanguage")))
                {
                    {
                        PSMenuItem ctmenu = new PSMenuItem
                        {
                            ItemName = item.Name,
                            VN = item.Text,
                            ItemType = item.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu);
                    }
                }
                if (item.GetType() == typeof(RadioGroup))
                {
                    string a = item.GetType().ToString();
                    RadioGroup ra = (RadioGroup)item;
                    for (int i = 0; i < ra.Properties.Items.Count; i++)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = ra.Name + ".Item." + i,
                            VN = ra.Properties.Items[i].ToString(),
                            ItemType = item.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                }
                else if (item.GetType() == typeof(ComboBoxEdit) || item.GetType() == typeof(ImageComboBoxEdit))
                {
                    string a = item.GetType().ToString();
                    ComboBoxEdit com = (ComboBoxEdit)item;
                    for (int i = 0; i < com.Properties.Items.Count; i++)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = com.Name + ".Item." + i,
                            VN = com.Properties.Items[i].ToString(),
                            ItemType = item.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                }
                else if (item.GetType() == typeof(DevExpress.XtraGrid.GridControl))
                {
                    DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)item;
                    if (gc != null)
                    {
                        foreach (var ctgv in gc.Views)
                        {
                            GridView gv = (GridView)ctgv;
                            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gv.Columns)
                            {
                                PSMenuItem ctmenu1 = new PSMenuItem
                                {
                                    ItemName = col.Name,
                                    VN = col.Caption,
                                    ItemType = item.GetType().ToString(),
                                };
                                MenuAddItem.Add(ctmenu1);
                            }
                        }
                    }
                }
                else if (item.GetType() == typeof(RibbonStatusBar))
                {
                    RibbonStatusBar ba = (RibbonStatusBar)item;
                    var it = ba.ItemLinks.ToList();
                    foreach (var i in it)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = i.Item.Name,
                            VN = i.Caption,
                            ItemType = item.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                }
                else if (item.GetType() == typeof(RibbonControl))
                {
                    RibbonControl ba = (RibbonControl)item;
                    var it = ba.Items.ToList();
                    foreach (var i in it)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = i.Name,
                            VN = i.Caption,
                            ItemType = i.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                    var page = ba.Pages.ToList();
                    foreach (var pa in page)
                    {
                        PSMenuItem ctmenu1 = new PSMenuItem
                        {
                            ItemName = pa.Name,
                            VN = pa.Text,
                            ItemType = pa.GetType().ToString(),
                        };
                        MenuAddItem.Add(ctmenu1);
                    }
                }
                else if (item.GetType() == typeof(LookUpEdit))
                {
                    PSMenuItem ctmenu = new PSMenuItem
                    {
                        ItemName = item.Name,
                        VN = item.Text,
                        ItemType = item.GetType().ToString(),
                    };
                    MenuAddItem.Add(ctmenu);
                }
                else
                {
                    AddControl(item);
                }
                BioNet_Bus.AddMenuItem(MenuAddItem, idFO);
            }
        }

        public static List<PSMenuTrans> MenuItem = new List<PSMenuTrans>();

        public static void Trans(Control.ControlCollection con, long? idfo)
        {
            try
            {
                idFO = idfo;
                MenuItem = BioNet_Bus.Trans(idFO);
                if (MenuItem.Count != 0)
                {
                    foreach (Control item in con)
                    {
                        PSMenuTrans meit = MenuItem.FirstOrDefault(x => x.ItemName.Equals(item.Name));
                        if (item.GetType() == typeof(RadioGroup))
                        {
                            RadioGroup ra = (RadioGroup)item;
                            for (int i = 0; i < ra.Properties.Items.Count; i++)
                            {
                                PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(item.Name + ".Item." + i));
                                if (me != null)
                                {
                                    ra.Properties.Items[i].Description = me.Trans;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ComboBoxEdit))
                        {
                            ComboBoxEdit com = (ComboBoxEdit)item;
                            ComboBoxItemCollection coll = com.Properties.Items;
                            int dem = com.Properties.Items.Count;
                            com.Properties.Items.Clear();
                            for (int i = 0; i < dem; i++)
                            {
                                PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(item.Name + ".Item." + i));
                                if (me != null)
                                {
                                    com.Properties.Items.Add(new ComboBoxItem(me.Trans));
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ImageComboBoxEdit))
                        {
                            ImageComboBoxEdit com = (ImageComboBoxEdit)item;
                            ImageComboBoxItemCollection coll = com.Properties.Items;
                            int dem = com.Properties.Items.Count;
                            com.Properties.Items.Clear();
                            for (int i = 0; i < dem; i++)
                            {
                                PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(item.Name + ".Item." + i));
                                if (me != null)
                                {
                                    com.Properties.Items.Add(new ImageComboBoxItem(me.Trans));
                                }
                            }
                        }
                        else if (item.GetType() == typeof(DevExpress.XtraGrid.GridControl))
                        {
                            DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)item;

                            foreach (var ctgv in gc.Views)
                            {
                                GridView gv = (GridView)ctgv;
                                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gv.Columns)
                                {
                                    PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(col.Name));
                                    if (me != null)
                                    {
                                        col.Caption = me.Trans;
                                    }
                                }
                            }
                        }
                        else if (item.GetType() == typeof(RibbonStatusBar))
                        {
                            string a = item.GetType().ToString();
                            RibbonStatusBar ba = (RibbonStatusBar)item;
                            var it = ba.ItemLinks.ToList();
                            foreach (var i in it)
                            {
                                PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(i.GalleryBarItemName));
                                if (me != null)
                                {
                                    i.Caption = me.Trans;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(RibbonControl))
                        {
                            string a = item.GetType().ToString();
                            RibbonControl ba = (RibbonControl)item;
                            var it = ba.Items.ToList();
                            foreach (var i in it)
                            {
                                PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(i.Name));
                                if (me != null)
                                {
                                    i.Caption = me.Trans;
                                }
                            }
                            var page = ba.Pages.ToList();
                            foreach (var pa in page)
                            {
                                PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(pa.Name));
                                if (me != null)
                                {
                                    pa.Text = me.Trans;
                                }

                            }
                        }
                        else
                        {
                            if (meit != null)
                            {
                                item.Text = meit.Trans;
                            }
                            TransControl(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        public static void TransControl(Control minicontrol)
        {
            try
            {
                foreach (Control item in minicontrol.Controls)
                {
                    PSMenuTrans meit = MenuItem.FirstOrDefault(x => x.ItemName.Equals(item.Name));
                    if (item.GetType() == typeof(RadioGroup))
                    {
                        RadioGroup ra = (RadioGroup)item;
                        for (int i = 0; i < ra.Properties.Items.Count; i++)
                        {
                            PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(item.Name + ".Item." + i));
                            if (me != null)
                            {
                                ra.Properties.Items[i].Description = me.Trans;
                            }
                        }
                    }
                    else if (item.GetType() == typeof(ComboBoxEdit))
                    {
                        ComboBoxEdit com = (ComboBoxEdit)item;
                        ComboBoxItemCollection coll = com.Properties.Items;
                        int dem = com.Properties.Items.Count;
                        com.Properties.Items.Clear();
                        for (int i = 0; i < dem; i++)
                        {
                            PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(item.Name + ".Item." + i));
                            if (me != null)
                            {
                                com.Properties.Items.Add(new ComboBoxItem(me.Trans));
                            }
                        }
                    }
                    else if (item.GetType() == typeof(ImageComboBoxEdit))
                    {
                        ImageComboBoxEdit com = (ImageComboBoxEdit)item;
                        ImageComboBoxItemCollection coll = com.Properties.Items;
                        int dem = com.Properties.Items.Count;
                        com.Properties.Items.Clear();
                        for (int i = 0; i < dem; i++)
                        {
                            PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(item.Name + ".Item." + i));
                            if (me != null)
                            {
                                com.Properties.Items.Add(new ImageComboBoxItem(me.Trans));
                            }
                        }
                    }
                    else if (item.GetType() == typeof(DevExpress.XtraGrid.GridControl))
                    {
                        DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)item;

                        foreach (var ctgv in gc.Views)
                        {
                            GridView gv = (GridView)ctgv;
                            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gv.Columns)
                            {
                                PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(col.Name));
                                if (me != null)
                                {
                                    col.Caption = me.Trans;
                                }
                            }
                        }
                    }
                    else if (item.GetType() == typeof(RibbonStatusBar))
                    {
                        string a = item.GetType().ToString();
                        RibbonStatusBar ba = (RibbonStatusBar)item;
                        var it = ba.ItemLinks.ToList();
                        foreach (var i in it)
                        {
                            PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(i.GalleryBarItemName));
                            if (me != null)
                            {
                                i.Caption = me.Trans;
                            }
                        }
                    }
                    else if (item.GetType() == typeof(RibbonControl))
                    {
                        string a = item.GetType().ToString();
                        RibbonControl ba = (RibbonControl)item;
                        var it = ba.Items.ToList();
                        foreach (var i in it)
                        {
                            PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(i.Name));
                            if (me != null)
                            {
                                i.Caption = me.Trans;
                            }
                        }
                        var page = ba.Pages.ToList();
                        foreach (var pa in page)
                        {
                            PSMenuTrans me = MenuItem.FirstOrDefault(x => x.ItemName.Equals(pa.Name));
                            if (me != null)
                            {
                                pa.Text = me.Trans;
                            }
                           
                        }
                    }
                    else
                    {
                        if (meit != null)
                        {
                            item.Text = meit.Trans;
                        }
                        TransControl(item);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
