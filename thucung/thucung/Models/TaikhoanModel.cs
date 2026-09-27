using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class TaikhoanModel
    {
        [Key]
        public string MaTaiKhoan { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
