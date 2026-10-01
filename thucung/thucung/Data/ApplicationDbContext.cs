using Microsoft.EntityFrameworkCore;
using thucung.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaikhoanModel> TaiKhoan { get; set; }
    public DbSet<ThucungModel> ThuCung { get; set; }
    public DbSet<KhachhangModel> KhachHang { get; set; }
    public DbSet<NhanVienModel> NhanVien { get; set; }
    public DbSet<ThuocModel> Thuoc { get; set; }
    public DbSet<PhieukhamModel> PhieuKham { get; set; }
         public DbSet<HoaDonModel> HoaDon { get; set; }
}
