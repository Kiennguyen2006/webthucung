using System.ComponentModel.DataAnnotations;

namespace thucung.Models
{
    public class HoaDonModel
    {
        [Key]
        public string MaHoaDon { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng chọn mã phiếu khám!")]
        public string MaPhieuKham { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập ngày lập!")]
        public DateTime? NgayLap { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tổng tiền!")]
        public decimal? TongTien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ghi chú!")]
        public string GhiChu { get; set; } = "";
    }
}