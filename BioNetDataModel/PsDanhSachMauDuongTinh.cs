using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PsDanhSachMauDuongTinh
    {
        public int STT { get; set; }
        public string MaPhieuL1 { get; set; }
        public string MaPhieuL2 { get; set; }
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public string MaGoiXN { get; set; }
        public string VietTatDV { get; set; }
        public string KetQuaCuoiL1 { get; set; }
        public string KetQua1L1 {get;set;}
        public string KetQua1L2 { get; set; }
        public string KetQuaCuoiL2 { get; set; }
        public string KetQua2L1 { get; set; }
        public string KetQua2L2 { get; set; }
        public DateTime? NgaySinh { get; set; }
        public DateTime? NgayLayMau { get; set; }
        public DateTime? NgayNhanMau { get; set; }
        public string KetLuan { get; set; }
        public string CLMau { get; set; }
    }
}
