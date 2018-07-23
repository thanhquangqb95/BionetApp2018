using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PsBaoCaoTaiChinh
    {
        public long? STT { get; set; }
        public string IDPhieu { get; set; }
        public string MaBenhNhan { get; set; }
        public string MotherName { get; set; }
        public string TenBenhNhan { get; set; }
        public DateTime? NgayGioSinh { get; set; }
        public DateTime? NgayGioLayMau { get; set; }
        public DateTime? NgayNhanMau { get; set; }
        public DateTime? NgayCoKQ { get; set; }
        public byte? TrangThaiMau { get; set; }
        public string MaGoiXN { get; set; }
        public string TenGoiDichVuChung { get; set; }
        public decimal? DonGia { get; set; }
        public string GhiChu { get; set; }
        public string MaDVCS { get; set; }
        public string MaChiCuc { get; set; }
        public string TenDVCS { get; set; }
        public string TenChiCuc { get; set; }
        public string DiaChiDVCS { get; set; }

    }
}
