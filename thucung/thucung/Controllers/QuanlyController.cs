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
            string? tentk = TempData["tentk"]?.ToString();

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
                ViewBag.bangthucung = _context.ThuCung
                    .Where(x => x.MaThuCung == timkiem || x.TenThuCung.Contains(timkiem))
                    .ToList();
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
            else
            {
                var bangkh = _context.KhachHang.ToList();
                ViewBag.thongtinkhachhang = bangkh;
                ViewBag.tb = "Vui lòng nhập đủ dữ liệu";
                return View("Themthucung",thucung);
            }
        }

        public IActionResult Chitiet(string id)
        {
            var dongthongtin = _context.ThuCung
                .FirstOrDefault(x => x.MaThuCung == id);

            return View(dongthongtin);
        }

        public IActionResult Sua(string id)
        {
            var bangkh = _context.KhachHang.ToList();
            ViewBag.thongtinkhachhang = bangkh;

            var dongthongtin = _context.ThuCung
                .FirstOrDefault(x => x.MaThuCung == id);

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
            else
            {
                var bangkh = _context.KhachHang.ToList();

                ViewBag.thongtinkhachhang = bangkh;
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin!";

                return View("Sua", thucung);
            }
        }

        public IActionResult Xoa(string id)
        {
            var bang = _context.ThuCung
                .FirstOrDefault(x => x.MaThuCung == id);

            return View(bang);
        }

        [HttpPost]
        public IActionResult Xoatc(ThucungModel thucung)
        {
            var dong = _context.ThuCung
                .FirstOrDefault(x => x.MaThuCung == thucung.MaThuCung);

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
                var bangkh = _context.KhachHang
                    .Where(x => x.MaKhachHang.Contains(timkiem) || x.HoTen.Contains(timkiem))
                    .ToList();

                return View(bangkh);
            }
            else
            {
                var bangkh = _context.KhachHang.ToList();

                return View(bangkh);
            }
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
                var bang = _context.KhachHang.ToList();

                ViewBag.thongtinkhachhang = bang;
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin và đúng định dạng!";
            }

            return View("Themkhachhang", khachhang);
        }

        public IActionResult Chitietkh(string id)
        {
            var bang = _context.KhachHang
                .FirstOrDefault(x => x.MaKhachHang == id);

            return View(bang);
        }

        public IActionResult Suakh(string id)
        {
            var dong = _context.KhachHang
                .FirstOrDefault(x => x.MaKhachHang == id);

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
                var bangkh = _context.KhachHang.ToList();

                ViewBag.thongtinkhachhang = bangkh;
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin và đúng định dạng!";
            }

            return View("Suakh", khachhang);
        }

        public IActionResult Xoakh(string id)
        {
            var dong = _context.KhachHang
                .FirstOrDefault(x => x.MaKhachHang == id);

            return View(dong);
        }

        [HttpPost]
        public IActionResult Xoakhachhang(KhachhangModel khachhang)
        {
            var dong = _context.KhachHang
                .FirstOrDefault(x => x.MaKhachHang == khachhang.MaKhachHang);

            if (dong != null)
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
                var bang = _context.NhanVien
                    .Where(x => x.MaNhanVien.Contains(timkiem) || x.HoTen.Contains(timkiem))
                    .ToList();

                return View(bang);
            }
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
            var dong = _context.NhanVien
                .FirstOrDefault(x => x.MaNhanVien == id);

            return View(dong);
        }

        public IActionResult Suanv(string id)
        {
            var dong = _context.NhanVien
                .FirstOrDefault(x => x.MaNhanVien == id);

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

        public IActionResult TaiKhoan(string timkiem)
        {
            if (timkiem != null && timkiem != "")
            {
                var bangtk = _context.TaiKhoan
                    .Where(x => x.MaTaiKhoan.Contains(timkiem) || x.TenDangNhap.Contains(timkiem))
                    .ToList();

                return View(bangtk);
            }
            else
            {
                var bangtk = _context.TaiKhoan.ToList();

                return View(bangtk);
            }
        }

        public IActionResult Chitiettk(string id)
        {
            if (id == null || id == "")
            {
                ViewBag.tb = "Mã chi tiết không tồn tại!!";

                return View("Taikhoan");
            }
            else
            {
                var dong = _context.TaiKhoan
                    .FirstOrDefault(x => x.MaTaiKhoan == id);

                if (dong == null)
                {
                    ViewBag.tb = "Tài khoản không tồn tại!!";

                    return View("TaiKhoan");
                }

                return View(dong);
            }
        }

        public IActionResult Suatk(string id)
        {
            var dong = _context.TaiKhoan.Find(id);

            return View(dong);
        }

        [HttpPost]
        public IActionResult Suataikhoan(TaikhoanModel taikhoan)
        {
            if (ModelState.IsValid)
            {
                _context.TaiKhoan.Update(taikhoan);
                _context.SaveChanges();

                return RedirectToAction("TaiKhoan", "Quanly");
            }
            else
            {
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin hoặc nhập đúng định dạng!!";

                return View("Suatk", taikhoan);
            }
        }

        public IActionResult Xoatk(string id)
        {
            var dong = _context.TaiKhoan.Find(id);

            return View(dong);
        }

        [HttpPost]
        public IActionResult Xoataikhoan(TaikhoanModel taikhoan)
        {
            var dong = _context.TaiKhoan.Find(taikhoan.MaTaiKhoan);

            if (dong != null)
            {
                _context.TaiKhoan.Remove(dong);
                _context.SaveChanges();

                return RedirectToAction("TaiKhoan", "Quanly");
            }
            else
            {
                ViewBag.tb = "Mã tài khoản không tồn tại!!";

                return View("Xoatk", taikhoan);
            }
        }

        public IActionResult Thuoc(string timkiem)
        {
            if (timkiem != null && timkiem != "")
            {
                var bang = _context.Thuoc
                    .Where(x => x.MaThuoc.Contains(timkiem) || x.TenThuoc.Contains(timkiem))
                    .ToList();

                return View(bang);
            }
            else
            {
                var bang = _context.Thuoc.ToList();

                return View(bang);
            }
        }

        public IActionResult Themthuoc()
        {
            var bang = _context.Thuoc.ToList();

            string mathuoc = "TH001";

            if (bang.Count > 0)
            {
                var dong = bang
                    .OrderByDescending(x => x.MaThuoc)
                    .FirstOrDefault();

                if (dong != null && dong.MaThuoc != null && dong.MaThuoc != "")
                {
                    string phanso = dong.MaThuoc.Substring(2);

                    int so = int.Parse(phanso);

                    so = so + 1;

                    mathuoc = "TH" + so.ToString("D3");
                }
            }

            var thuoc = new ThuocModel();

            thuoc.MaThuoc = mathuoc;

            return View(thuoc);
        }

        [HttpPost]
        public IActionResult Themthuocxl(ThuocModel thuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Thuoc.Add(thuoc);
                _context.SaveChanges();

                return RedirectToAction("Thuoc", "Quanly");
            }
            else
            {
                ViewBag.tb = "Vui lòng nhập đủ thông tin và đúng định dạng!!!";

                return View("Themthuoc", thuoc);
            }
        }

        public IActionResult Suathuoc(string id)
        {
            var dong = _context.Thuoc.Find(id);

            return View(dong);
        }

        [HttpPost]
        public IActionResult Suathuocxl(ThuocModel thuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Thuoc.Update(thuoc);
                _context.SaveChanges();

                return RedirectToAction("Thuoc", "Quanly");
            }
            else
            {
                ViewBag.tb = "vui lòng nhập đầy đủ thông tin và chính xác!!!";

                return View("Suathuoc", thuoc);
            }
        }

        public IActionResult Chitietthuoc(string id)
        {
            var dong = _context.Thuoc.Find(id);

            return View(dong);
        }

        public IActionResult Xoathuoc(string id)
        {
            var dong = _context.Thuoc.Find(id);

            return View(dong);
        }

        [HttpPost]
        public IActionResult Xoathuocxl(ThuocModel thuoc)
        {
            var dong = _context.Thuoc.Find(thuoc.MaThuoc);

            if (dong != null)
            {
                _context.Thuoc.Remove(dong);
                _context.SaveChanges();

                return RedirectToAction("Thuoc", "Quanly");
            }
            else
            {
                ViewBag.tb = "Mã xoá không tồn tại!!!";

                return View("Xoathuocxl", thuoc);
            }
        }

        public IActionResult Phieukham(string timkiem)
        {
            if (timkiem != null && timkiem != "")
            {
                var bang = _context.PhieuKham
                    .Where(x =>
                        x.MaPhieuKham.Contains(timkiem) ||
                        x.MaKhachHang.Contains(timkiem) ||
                        x.MaNhanVien.Contains(timkiem) ||
                        x.BenhTinh.Contains(timkiem))
                    .ToList();

                return View(bang);
            }
            else
            {
                var bang = _context.PhieuKham.ToList();

                return View(bang);
            }
        }

        public IActionResult Themphieukham()
        {
            var bangkh = _context.KhachHang.ToList();
            var bangtc = _context.ThuCung.ToList();
            var bangnv = _context.NhanVien.ToList();

            ViewBag.kh = bangkh;
            ViewBag.tc = bangtc;
            ViewBag.nv = bangnv;

            return View();
        }

        [HttpPost]
        public IActionResult Themphieukhamxl(PhieukhamModel phieukham)
        {
            ModelState.Remove("MaPhieuKham");

            var bang = _context.PhieuKham.ToList();

            string maphieukham = "PK001";

            if (bang.Count > 0)
            {
                var dong = bang
                    .OrderByDescending(x => x.MaPhieuKham)
                    .FirstOrDefault();

                if (dong != null && dong.MaPhieuKham != null && dong.MaPhieuKham != "")
                {
                    string phanso = dong.MaPhieuKham.Substring(2);

                    int so = int.Parse(phanso);

                    so = so + 1;

                    maphieukham = "PK" + so.ToString("D3");
                }
            }

            phieukham.MaPhieuKham = maphieukham;

            if (ModelState.IsValid)
            {
                _context.PhieuKham.Add(phieukham);
                _context.SaveChanges();

                return RedirectToAction("Phieukham", "Quanly");
            }
            else
            {
                var bangkh = _context.KhachHang.ToList();
                var bangtc = _context.ThuCung.ToList();
                var bangnv = _context.NhanVien.ToList();

                ViewBag.kh = bangkh;
                ViewBag.tc = bangtc;
                ViewBag.nv = bangnv;

                ViewBag.tb = "vui lòng nhập đầy đủ thông tin và chọn nội dung";

                return View("Themphieukham", phieukham);
            }
        }

        public IActionResult Suaphieukham(string id)
        {
            var bangkh = _context.KhachHang.ToList();
            var bangtc = _context.ThuCung.ToList();
            var bangnv = _context.NhanVien.ToList();

            ViewBag.kh = bangkh;
            ViewBag.tc = bangtc;
            ViewBag.nv = bangnv;

            var thongtincu = _context.PhieuKham.Find(id);

            return View(thongtincu);
        }

        [HttpPost]
        public IActionResult Suaphieukhamxl(PhieukhamModel phieukham)
        {
            ModelState.Remove("MaPhieuKham");

            if (ModelState.IsValid)
            {
                _context.PhieuKham.Update(phieukham);
                _context.SaveChanges();

                return RedirectToAction("Phieukham", "Quanly");
            }
            else
            {
                var bangkh = _context.KhachHang.ToList();
                var bangtc = _context.ThuCung.ToList();
                var bangnv = _context.NhanVien.ToList();

                ViewBag.kh = bangkh;
                ViewBag.tc = bangtc;
                ViewBag.nv = bangnv;
                ViewBag.tb = "vui lòng nhập đầy đủ thông tin và chọn nội dung";

                return View("Suaphieukham", phieukham);
            }
        }

        public IActionResult Chitietphieukham(string id)
        {
            var thongtincu = _context.PhieuKham.Find(id);

            return View(thongtincu);
        }

        public IActionResult Xoaphieukham(string id)
        {
            var thongtincu = _context.PhieuKham.Find(id);

            return View(thongtincu);
        }

        [HttpPost]
        public IActionResult Xoaphieukhamxl(PhieukhamModel phieukham)
        {
            var dongxoa = _context.PhieuKham.Find(phieukham.MaPhieuKham);

            if (dongxoa != null)
            {
                _context.PhieuKham.Remove(dongxoa);
                _context.SaveChanges();

                return RedirectToAction("Phieukham", "Quanly");
            }
            else
            {
                ViewBag.tb = "mã phiêu khám không tồn tại!!!";

                return View("Xoaphieukham", phieukham);
            }
        }
        public IActionResult Hoadon(string timkiem)
        {
            if(timkiem!=null && timkiem!="")
            {
                var bang=_context.HoaDon.Where(x => x.MaHoaDon.Contains(timkiem) || x.MaPhieuKham.Contains(timkiem)).ToList();
                ViewBag.banghoadon = bang;
                return View();
            }
            else
            {
                var bang = _context.HoaDon.ToList();
                ViewBag.banghoadon = bang;
                return View(bang);
            }
        }
        public IActionResult Themhoadon()
        {
            var bang = _context.PhieuKham.ToList();
            ViewBag.thongtin= bang;
            return View();
        }
        [HttpPost]
        public IActionResult Themhd(HoaDonModel hoaDon)
        {
            string mahoadon = "";

            var hoadoncu = _context.HoaDon
                .OrderByDescending(x => x.MaHoaDon)
                .FirstOrDefault();

            if (hoadoncu != null)
            {
                string phanchu = "";
                string phanso = "";

                foreach (char kytu in hoadoncu.MaHoaDon)
                {
                    if (char.IsLetter(kytu))
                    {
                        phanchu = phanchu + kytu;
                    }
                    else
                    {
                        phanso = phanso + kytu;
                    }
                }

                int so = int.Parse(phanso);
                so = so + 1;

                mahoadon = phanchu + so.ToString("D3");
            }
            ModelState.Remove("MaHoaDon");
            hoaDon.MaHoaDon = mahoadon;
            if (ModelState.IsValid)
            {
                _context.HoaDon.Add(hoaDon);
                _context.SaveChanges();
                return RedirectToAction("Hoadon", "Quanly");
            }
            else
            {
                var bang = _context.PhieuKham.ToList();
                ViewBag.thongtin= bang;
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin!";
                return View("Themhoadon", hoaDon);
            }
        }
        public IActionResult Chitiethoadon(string id)
        {
            var bang = _context.HoaDon.Find(id);
            ViewBag.thongtincu = bang;
            return View();
        }
        public IActionResult Suahoadon(string id)
        {
            var bang = _context.PhieuKham.ToList();
            ViewBag.thongtin= bang;
            var thongtincu = _context.HoaDon.Find(id);
            ViewBag.cu = thongtincu;
            return View();
        }
        [HttpPost]
        public IActionResult Suahd(HoaDonModel hoaDon)
        {
            ModelState.Remove("MaHoaDon");
            if (ModelState.IsValid)
            {
                _context.HoaDon.Update(hoaDon);
                _context.SaveChanges();
                return RedirectToAction("Hoadon", "Quanly");
            }
            else
            {
                var bang = _context.PhieuKham.ToList();
                ViewBag.thongtin = bang;
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin!";
                return View("Suahoadon", hoaDon);
            }
        }
        public IActionResult Xoahoadon(string id)
        {
            var bang = _context.HoaDon.Find(id);
            ViewBag.bang = bang;
            return View();
        }
        [HttpPost]
        public IActionResult Xoahd(HoaDonModel hoaDon)
        {
            ModelState.Remove("MaHoaDon");
            var dong = _context.HoaDon.Find(hoaDon.MaHoaDon);
            if (dong!=null)
            {
                _context.HoaDon.Remove(dong);
                _context.SaveChanges();
                return RedirectToAction("Hoadon", "Quanly");
            }
            else
            {
                ViewBag.tb = "Vui lòng nhập đầy đủ thông tin!";
                return View("Xoahoadon", hoaDon);
            }
        }
        
    }
}