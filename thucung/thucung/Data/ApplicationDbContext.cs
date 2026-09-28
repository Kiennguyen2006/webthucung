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
}
