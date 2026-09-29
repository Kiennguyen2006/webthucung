using Microsoft.AspNetCore.Mvc;
using thucung.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace thucung.Controllers
{
    public class QuanlyController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public QuanlyController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Quanly()
        {
            string tentk = TempData["tentk"]?.ToString();

            if (tentk == null || tentk == "")
            {
                return RedirectToAction("Dangnhap", "Kiemtrathongtin");
            }
            var quyen = _context.TaiKhoan
        .Where(x => x.TenDangNhap == tentk)
        .Select(x => x.PhanQuyen) 
        .FirstOrDefault();
            ViewBag.PhanQuyen = quyen;
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
        public IActionResult Themthucung()
        {
            var bangkh = _context.KhachHang.ToList();
            ViewBag.thongtinkhachhang = bangkh;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Themtc(ThucungModel thucung, IFormFile anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhFile.FileName);
                string uploadDir = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhFile.CopyToAsync(stream);
                }
                thucung.Anh = fileName;
            }

            var bangthucung = _context.ThuCung.ToList();

            int so = 0;

            if (bangthucung.Count > 0)
            {
                string maLonNhat = bangthucung
                    .OrderByDescending(x => x.MaThuCung)
                    .First()
                    .MaThuCung;

                so = int.Parse(maLonNhat.Substring(2));
            }

            so++;

            string maThuCungMoi = "TC" + so.ToString("D3");
            thucung.MaThuCung = maThuCungMoi;
            thucung.NgayTao = DateTime.Now;
            ModelState.Remove("MaThuCung");
            ModelState.Remove("Anh");
            ModelState.Remove("MaKhachHang");
            if (ModelState.IsValid)
            {
                _context.ThuCung.Add(thucung);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Content(
                        "CanNang = " + thucung.CanNang +
                        "\nMaThuCung = " + thucung.MaThuCung +
                        "\nMaKhachHang = " + thucung.MaKhachHang +
                        "\nLỗi = " + ex.GetBaseException().Message
                    );
                }
                return RedirectToAction("Thucung", "Quanly");
            }
            else {
                ViewBag.tb = "Vui lòng nhập đủ dữ liệu";
                return View();
            }
            return View();
        }
        public IActionResult Chitiet(string id)
        {
            var dongthongtin = _context.ThuCung
                .FirstOrDefault(x => x.MaThuCung.Equals(id));

            return View(dongthongtin);
        }
        public IActionResult Sua(string id)
        {
            var bangkh = _context.KhachHang.ToList();
            ViewBag.thongtinkhachhang = bangkh;
            var dongthongtin = _context.ThuCung
                .FirstOrDefault(x => x.MaThuCung.Equals(id));

            return View(dongthongtin);
        }
        [HttpPost]
        public async Task<IActionResult> Suatc(ThucungModel thucung, IFormFile anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhFile.FileName);
                string uploadDir = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                string filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhFile.CopyToAsync(stream);
                }

                
                thucung.Anh = fileName;
            }
            ModelState.Remove("MaThuCung");
            ModelState.Remove("Anh");
            if (ModelState.IsValid)
            {
                _context.ThuCung.Update(thucung);
                _context.SaveChanges();
                return RedirectToAction("Thucung", "Quanly");
            }
            return View();
        }
        public IActionResult Xoa(string id)
        {
            var bang = _context.ThuCung.FirstOrDefault(x => x.MaThuCung == id);
            return View(bang);
        }
        [HttpPost]
        public IActionResult Xoatc(ThucungModel thucung )
        {
                var dong = _context.ThuCung.FirstOrDefault(x => x.MaThuCung == thucung.MaThuCung);
            if (dong != null)
            {
                _context.ThuCung.Remove(dong);
                _context.SaveChanges();
            }

            ViewBag.bangthucung = _context.ThuCung.ToList();

            return View("Thucung");
        }
        public IActionResult Khachhang(string timkiem)
        {
            if (timkiem != null && timkiem != "")
            {
                var bangkh = _context.KhachHang.Where(x => x.MaKhachHang.Contains(timkiem) || x.HoTen.Contains(timkiem)).ToList();
                return View(bangkh);
            }
            else
            {
                var bangkh = _context.KhachHang.ToList();
                return View(bangkh);
            }
            return View();
        }
        public IActionResult Themkhachhang()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Themkh(KhachhangModel khachhang)
        {
            var bangkh = _context.KhachHang.ToList();

            int so = 0;

            if (bangkh.Count > 0)
            {
                string maLonNhat = bangkh
                    .OrderByDescending(x => x.MaKhachHang)
                    .First()
                    .MaKhachHang;

                so = int.Parse(maLonNhat.Substring(2));
            }

            so++;

            string maKhachHangMoi = "KH" + so.ToString("D3");
            khachhang.MaKhachHang = maKhachHangMoi;
            khachhang.NgayTao = DateTime.Now;
            ModelState.Remove("MaKhachHang");
            ModelState.Remove("NgayTao");
            if (ModelState.IsValid)
            {
                _context.KhachHang.Add(khachhang);
                _context.SaveChanges();
                return RedirectToAction("Khachhang", "Quanly");
            }
            else
            {
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin và đúng định dạng!";
            }
            return View("Themkhachhang", khachhang);
        }
        public IActionResult Chitetkh(string id)
        {
            var bang = _context.KhachHang.FirstOrDefault(x => x.MaKhachHang == id);
            return View(bang);
        }
        public IActionResult Suakh(string id)
        {
            var dong = _context.KhachHang.FirstOrDefault(x=>x.MaKhachHang==id);
            return View(dong);
        }
        [HttpPost]
        public IActionResult Suakhachhang(KhachhangModel khachhang)
        {
            ModelState.Remove("MaKhachHang");
            ModelState.Remove("NgayTao");
            if (ModelState.IsValid)
            {
                _context.KhachHang.Update(khachhang);
                _context.SaveChanges();
                return RedirectToAction("Khachhang", "Quanly");
            }
            else
            {
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin và đúng định dạng!";
            }
            return View("Suakh", khachhang);
        }
        public IActionResult Xoakh(string id)
        {
            var dong = _context.KhachHang.FirstOrDefault(x => x.MaKhachHang == id);
            return View(dong);
        }
        [HttpPost]
        public IActionResult Xoakhachhang(KhachhangModel khachhang)
        {
            var dong = _context.KhachHang.FirstOrDefault(x => x.MaKhachHang == khachhang.MaKhachHang);
            if ( dong != null &&  dong != null)
            {
                _context.KhachHang.Remove(dong);
                _context.SaveChanges();
                return RedirectToAction("Khachhang", "Quanly");
            }
            return View();
        }
        public IActionResult Nhanvien(string timkiem)
        {
            if (timkiem == null || timkiem == "")
            {
                var bang = _context.NhanVien.ToList();
                return View(bang);
            }
            else
            {
                var bang = _context.NhanVien.Where(x=>x.MaNhanVien.Contains(timkiem) || x.HoTen.Contains(timkiem)).ToList();
                return View(bang);
            }
            return View();
        }
        public IActionResult Themnv()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Themnhanvien(NhanVienModel nhanVien, IFormFile anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhFile.FileName);
                string uploadDir = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhFile.CopyToAsync(stream);
                }

                nhanVien.Anh = fileName;
            }

            var bangnhanvien = _context.NhanVien.ToList();

            int so = 0;

            if (bangnhanvien.Count > 0)
            {
                string maLonNhat = bangnhanvien
                    .OrderByDescending(x => x.MaNhanVien)
                    .First()
                    .MaNhanVien;

                so = int.Parse(maLonNhat.Substring(2));
            }

            so++;

            string maNhanVienMoi = "NV" + so.ToString("D3");

            nhanVien.MaNhanVien = maNhanVienMoi;

            ModelState.Remove("MaNhanVien");
            ModelState.Remove("Anh");

            if (ModelState.IsValid)
            {
                _context.NhanVien.Add(nhanVien);
                _context.SaveChanges();

                return RedirectToAction("Nhanvien", "Quanly");
            }
            else
            {
                ViewBag.tb = "Thông tin chưa được nhập đủ hoặc thông tin không đúng!";
                return View("Themnv", nhanVien);
            }
        }
        public IActionResult Chitietnv(string id)
        {
            var dong = _context.NhanVien.FirstOrDefault(x => x.MaNhanVien == id);
            return View(dong);
        }
        public IActionResult Suanv(string id)
        {
            var dong = _context.NhanVien.FirstOrDefault(x => x.MaNhanVien == id);
            return View(dong);
        }

        [HttpPost]
        public async Task<IActionResult> Suanhanvien(NhanVienModel nhanVien, IFormFile anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhFile.FileName);
                string uploadDir = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhFile.CopyToAsync(stream);
                }

                nhanVien.Anh = fileName;
            }

            ModelState.Remove("MaNhanVien");
            ModelState.Remove("Anh");

            if (ModelState.IsValid)
            {
                _context.NhanVien.Update(nhanVien);
                _context.SaveChanges();

                return RedirectToAction("Nhanvien", "Quanly");
            }
            else
            {
                ViewBag.tb = "Thông tin đang bị trống hoặc dữ liệu sửa không chính xác!!";

                return View("Suanv", nhanVien);
            }
        }
        public IActionResult Xoanv(string id)
        {
            var dong = _context.NhanVien.Find(id);
            return View(dong);
        }
        [HttpPost]
        public IActionResult Xoanhanvien(NhanVienModel nhanvien)
        {
            var dong = _context.NhanVien.Find(nhanvien.MaNhanVien);
            if (dong != null)
            {
                _context.NhanVien.Remove(dong);
                _context.SaveChanges();
                return RedirectToAction("Nhanvien", "Quanly");
            }
            return View();
        }
    }
}
