using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thucung.Models
{
    [Table("Thuoc", Schema = "dbo")]
    public class ThuocModel
    {
        [Key]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string MaThuoc { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TenThuoc { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string DonViTinh { get; set; } = string.Empty;

        [Required]
        public int SoLuongTon { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }

        [Required]
        public DateTime HanSuDung { get; set; }

        [StringLength(500)]
        public string? MoTa { get; set; }

        [Required]
        public bool TrangThai { get; set; }
    }
}