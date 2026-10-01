using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class TaikhoanModel
    {
        [Key]
        public string MaTaiKhoan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; } = string.Empty;

        public DateTime NgayTao { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập phân quyền")]
        public string PhanQuyen { get; set; } = string.Empty;
    }
}