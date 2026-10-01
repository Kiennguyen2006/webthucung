using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thucung.Models
{
    [Table("PhieuKham", Schema = "dbo")]
    public class PhieukhamModel
    {
        [Key]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string MaPhieuKham { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string MaKhachHang { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string MaThuCung { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string MaNhanVien { get; set; } = string.Empty;

        [Required]
        public DateTime NgayKham { get; set; }

        [StringLength(1000)]
        public string? MoTaTrieuChung { get; set; }

        [StringLength(1000)]
        public string? KetQuaKham { get; set; }

        [StringLength(500)]
        public string? BenhTinh { get; set; }

        [StringLength(1000)]
        public string? ThongTinThuoc { get; set; }

        [StringLength(1000)]
        public string? MoTaSuDungDichVu { get; set; }


        [Required]
        public DateTime NgayLap { get; set; }

        [ForeignKey("MaKhachHang")]
        public virtual KhachhangModel? KhachHang { get; set; }

        [ForeignKey("MaThuCung")]
        public virtual ThucungModel? ThuCung { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual NhanVienModel? NhanVien { get; set; }
    }
}