using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class ThucungModel
    {
        [Key]
        public string MaThuCung { get; set; }
        public string MaKhachHang { get; set; }
        public string TenThuCung { get; set; }
        public string LoaiThuCung { get; set; }
        public string GiongThuCung { get; set; }
        public string GioiTinh { get; set; }
        public string MauLong { get; set; }
        public decimal CanNang { get; set; }
        public string Anh { get; set; }
        public string MoTa { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
