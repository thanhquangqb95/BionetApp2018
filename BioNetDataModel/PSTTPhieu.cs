using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BioNetModel.Data;

namespace BioNetModel
{
    public class PSTTPhieu
    {
        public PSPhieuSangLoc Phieu { get; set; }
        public PSPatient Benhnhan { get; set; }
        public PSTiepNhan TiepNhan { get; set; }
        public PSChiDinhDichVu ChiDinh { get; set; }
        public List<PSChiTietDanhGiaChatLuong> lstLyDoKhongDat { get; set; }
        public List<PsDichVu> lstChiDinhDichVu { get; set; }
    }
}
