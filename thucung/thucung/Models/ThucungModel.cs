using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class ThucungModel
    {
        [Key]
        public string MaThuCung { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        public string MaKhachHang { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập tên thú cưng")]
        public string TenThuCung { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập loại thú cưng")]
        public string LoaiThuCung { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập giống thú cưng")]
        public string GiongThuCung { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập giới tính")]
        public string GioiTinh { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập màu lông")]
        public string MauLong { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập cân nặng")]
        public decimal CanNang { get; set; }

        public string Anh { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string MoTa { get; set; } = "";

        public DateTime NgayTao { get; set; }
    }
}