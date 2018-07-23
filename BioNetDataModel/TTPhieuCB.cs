using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class TTPhieuCB
    {
        public int TongSoPhieu { get; set; }
        public int PhieuThuMoi { get; set; }
        public int PhieuThuLai { get; set; }
        public int Nam { get; set; }
        public int Nu { get; set; }
        public int GTKhac { get; set; }
        public int PPSinhThuong { get; set; }
        public int PPSinhMo { get; set; }
        public int PPSinhKhac { get; set; }
        public int MauDat { get; set; }
        public int MauKoDat { get; set; }
        public int TyleSinhConThu3 { get; set; }
        public int TuoiMeDuoi18 { get; set; }
        public int TuoiMeTu18den35 { get; set; }
        public int TuoiMeTren35 { get; set; }
        public int SoMauCanThuLai { get; set; }
        public int SoMauDaThuLai { get; set; }
        public int SoMauChuaThuLai { get; set; }
        public List<rptBaoCaoSLPhieu> slphieu { get; set; }
        public List<PsThongKe> thongkebenh { get; set; }
        public List<PsThongKe> thongkeDGMau { get; set; }
        public List<PsThongKe> thongkeCTrinh { get; set; }
        public PsBaoCaoCoBanCT baocaocobanct { get; set; }

    }
}
