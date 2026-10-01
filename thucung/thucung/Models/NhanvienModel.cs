using System;
using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class NhanVienModel
    {
        [Key]
        public string MaNhanVien { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; } = "";

        public DateTime? NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giới tính")]
        public string GioiTinh { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string SoDienThoai { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string DiaChi { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập chuyên môn")]
        public string ChuyenMon { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập bằng cấp")]
        public string BangCap { get; set; } = "";

        public DateTime? NgayVaoLam { get; set; }

        public string Anh { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string MoTa { get; set; } = "";
    }
}