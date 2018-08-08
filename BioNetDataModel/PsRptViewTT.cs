﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PsRptViewTT
    {
        public string TenTre { get; set; }
        public string NgaySinh { get; set; }
        public string CanNang { get; set; }
        public string GioiTinh { get; set; }
        public string TuoiThai { get; set; }
        public string TenMe { get; set; }
        public string TenCha { get; set; }
        public string DienThoaiMe { get; set; }
        public string DienThoaiCha { get; set; }
        public string NSCha { get; set; }
        public string NSMe { get; set; }
        public string DiaChiTre { get; set; }
        public string MaDonVi { get; set; }
        public string TenDonVi { get; set; }
        public string DiaChiDonVi { get; set; }
        public string MaPhieu { get; set; }
        public string MaKhachHang { get; set; }
        public string MaXetNghiem { get; set; }
        public string NgayThuMau { get; set; }
        public string NgayNhanMau { get; set; }
        public string NgayXetNghiem { get; set; }
        public string KetLuanBinhThuong { get; set; }
        public string KetLuanNguyCoCao { get; set; }
        public string GhiChu { get; set; }
        public string NgayCoKQ { get; set; }
        public string Para { get; set; }
        public string PPSinh { get; set; }
        public string CTSangLoc { get; set; }
        public string NguoiLayMau { get; set; }
        public string LyDoKoDat { get; set; }
        public string GoiXN { get; set; }
        public string CLuongMau { get; set; }
        public List<PsRPTTraKetQuaSangLocChiTiet> chitietKetQua { get; set; }
    }
}