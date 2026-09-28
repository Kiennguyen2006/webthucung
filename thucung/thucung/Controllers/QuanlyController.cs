using Microsoft.AspNetCore.Mvc;

namespace thucung.Controllers
{
    public class QuanlyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanlyController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Quanly()
        {
            string tentk = TempData["tentk"].ToString();
            ViewBag.tentaikhoan = tentk;
            return View();
        }

        public IActionResult Thucung(string timkiem)
        {
            if (timkiem == null || timkiem == "")
            {
                ViewBag.bangthucung = _context.ThuCung.ToList();
            }
            else
            {
                ViewBag.bangthucung = _context.ThuCung.Where(x => x.MaThuCung == timkiem || x.TenThuCung.Contains(timkiem)).ToList();
            }
            return View();
        }
    }
}
