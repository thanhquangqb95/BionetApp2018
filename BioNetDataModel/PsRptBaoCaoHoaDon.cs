using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PsRptBaoCaoHoaDon
    {
        public string MaDVCS { get; set; }
        public string TenDVCS { get; set; }
        public string DiaChiDVCS { get; set; }
        public List<PsRptBaoCaoHoaDonCT> PsRptBaoCaoHoaDonCT { get; set; }
    }
}
