using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class TaikhoanModel
    {
        [Key]
        public string MaTaiKhoan { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; }
    }
}
