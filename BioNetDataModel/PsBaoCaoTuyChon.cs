using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BioNetModel.Data;

namespace BioNetModel
{
    public class PsBaoCaoTuyChon
    {

        //Thông tin phiếu
        public string IDPhieu { get; set; }
        public string MaDVCS { get; set; }
        public string MaXetNghiem { get; set; }
        public string MaKhachHang { get; set; }
        public string GoiXN { get; set; }

        //Thông tin gia đình
        public string TenMe { get; set; }
        public string NamSinhMe { get; set; }
        public string TenCha { get; set; }
        public string NamSinhCha { get; set; }
        public string DiaChiGiaDinh { get; set; }
        public string SDTMe { get; set; }
        public string SDTCha { get; set; }
        public string Para { get; set; }
        public string PPSinh { get; set; }

        //Thông tin đơn vị
        public string NoiLayMau { get; set; }
        public string DiaChiLayMau { get; set; }
        public string NguoiLayMau { get; set; }
        public string SdtNguoiLayMau { get; set; }
        public byte? IDViTriLayMau { get; set; }

        //Thông tin trẻ
        public string TenTre { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string  IDTinhTrangTre{ get; set; }
        public string SLTruyenMau { get; set; }
        public DateTime? NgayTruyenMau { get; set; }
        public string TuoiThai { get; set; }
        public string NoiSinh { get; set; }
        public string IDCheDoDinhDuong { get; set; }
        public string IDDanToc { get; set; }
        public string CanNang { get; set; }
        //Thông tin xét nghiêm
        public DateTime? NgayNhanMau { get; set; }
        public DateTime? NgayXetNghiem { get; set; }

        public DateTime? NgayTraKQ { get; set; }
        public DateTime? NgayLayMau { get; set; }
        public string IDChuongTrinh { get; set; }
        public bool? TinhTrangMau { get; set; }
        public bool? DanhGiaMau { get; set; }
        public string ChiSoXNLai { get; set; }
        public string IDPhieuLan1 { get; set; }
        public string LuuY { get; set; }
        public string GhiChu { get; set; }
        public string KLBinhThuong { get; set; }
        public string KLNguyCoCao { get; set; }
        public string LyDoMauKDat { get; set; }

        public string IDGoiDichVuChung { get; set; }

        public bool? isNguyCoCao { get; set; }

     

        //Thông tin kết quả
        public PsCTKQBaoCaoTuyChon ctkqL1 { get; set; }
        public PsCTKQBaoCaoTuyChon ctkqL2 { get; set; }
        public PsCTKQBaoCaoTuyChon ctkqCuoi { get; set; }
        public PsCTKQBaoCaoTuyChon ctklCuoi { get; set; }

    }
}
