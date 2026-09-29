using System;
using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class NhanVienModel
    {
        [Key]
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string ChuyenMon { get; set; }
        public string BangCap { get; set; }
        public DateTime? NgayVaoLam { get; set; }
        public string Anh { get; set; }
        public string MoTa { get; set; }
    }
}