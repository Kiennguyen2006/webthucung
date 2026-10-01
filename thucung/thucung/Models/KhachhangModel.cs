using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class KhachhangModel
    {
        [Key]
        public string MaKhachHang { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập giới tính")]
        public string GioiTinh { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string SoDienThoai { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string DiaChi { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập ghi chú")]
        public string GhiChu { get; set; } = "";

        public DateTime NgayTao { get; set; }
    }
}