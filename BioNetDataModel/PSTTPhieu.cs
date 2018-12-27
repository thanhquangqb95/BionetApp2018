using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BioNetModel.Data;

namespace BioNetModel
{
    public class PSTTPhieu
    {
        public string MaPhieu { get; set; }
        public string MaPhieu1 { get; set; }
        public string MaDonVi { get; set; }
        public string MaNV { get; set; }
        public string MaBenhNhan { get; set; }
        public string MaTiepNhan { get; set; }
        public string MaChiDinh { get; set; }
        public string MaGoiXN { get; set; }
        public decimal? DonGiaGoi { get; set; }
        public bool? isDaNhapLieu { get; set; }
        public PSPhieuSangLoc Phieu { get; set; }
        public PSPatient Benhnhan { get; set; }
        public PSTiepNhan TiepNhan { get; set; }
        public PSChiDinhDichVu ChiDinh { get; set; }
        public List<PSChiTietDanhGiaChatLuong> lstLyDoKhongDat { get; set; }
        public List<PsDichVu> lstChiDinhDichVu { get; set; }
    }
}
