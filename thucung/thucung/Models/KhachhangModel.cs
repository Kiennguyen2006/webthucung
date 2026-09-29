using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class KhachhangModel
    {
        [Key]
        public string MaKhachHang { get; set; }
        public string HoTen{ get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
