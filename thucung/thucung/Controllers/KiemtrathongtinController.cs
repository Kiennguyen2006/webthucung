using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using thucung.Models;
namespace thucung.Controllers
{
    public class KiemtrathongtinController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KiemtrathongtinController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dangnhap(string tentk, string matkhau)
        {
            
            TempData["tentk"] = tentk;
            int a = 0;
            string b = "Tên đăng nhập hoặc mật khẩu không tồn tại!!!";
            if (tentk != null && tentk != "" && matkhau != null && matkhau != "")
            {
                var bangtk = _context.TaiKhoan.ToList();
                for (int i = 0; i < bangtk.Count; i++)
                {
                    if (tentk == bangtk[i].TenDangNhap && matkhau == bangtk[i].MatKhau)
                    {
                        a = 1;
                        break;
                    }
                    else
                    {
                        a = 0;
                    }
                }
                if (a == 1)
                {
                    return RedirectToAction("Quanly", "Quanly");
                }
                else
                {
                    return View(model: b);
                }
            }
            return View(model: null);
        }
        public async Task<IActionResult> Khoiphuc(string email)
        {
            if(email=="" || email == null)
            {
                return View();
            }
            int k = 0;
            var bangthongtin = _context.TaiKhoan.ToList();
            for (int i = 0; i < bangthongtin.Count; i++)
            {
                if (bangthongtin[i].Email.Equals(email))
                {
                    k = 1;
                    break;
                }
                else
                {
                    k = 0;
                }
            }
            if (k == 1)
            {
                Random rd = new Random();

                string ma = rd.Next(100000, 1000000).ToString();

                TempData["MaOTP"] = ma;
                TempData["Email"] = email;

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(
                    "Hệ thống quản lý thú cưng",
                    "kiennguyenfa12345678@gmail.com"
                ));

                message.To.Add(new MailboxAddress(
                    "",
                    email
                ));

                message.Subject = "Mã khôi phục mật khẩu";

                message.Body = new TextPart("plain")
                {
                    Text = "Mã khôi phục mật khẩu của bạn là: " + ma +
                           "\n\nMã này dùng để xác nhận khôi phục mật khẩu."
                };

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(
                        "smtp.gmail.com",
                        587,
                        SecureSocketOptions.StartTls
                    );

                    await smtp.AuthenticateAsync(
                        "kiennguyenfa12345678@gmail.com",
                        "xokepjphvempgazk"
                    );

                    await smtp.SendAsync(message);

                    await smtp.DisconnectAsync(true);
                }
                return RedirectToAction("Nhapma");
            }
            else
            {
                ViewBag.tb = "Email bạn nhập không tồn tại để tìm mật khẩu!!!";
            }
            return View();
        }
        public IActionResult Nhapma(string ma)
        {
            if (ma == null || ma == "")
            {
                return View();
            }

            if (ma == TempData["MaOTP"]?.ToString())
            {
                return RedirectToAction("Taolaimk");
            }
            else
            {
                ViewBag.tb = "Mã nhập không chính xác";

                TempData.Keep("MaOTP");
                TempData.Keep("Email");

                return View();
            }
        }
        public IActionResult Taolaimk(string matkhau, string nhaplaimatkhau)
        {
            if (matkhau == null || matkhau == "" || nhaplaimatkhau == null || nhaplaimatkhau == "")
            {
                return View();
            }
            if (matkhau == nhaplaimatkhau)
            {
                var dongmuonsua = _context.TaiKhoan.FirstOrDefault(x => x.Email == TempData["email"]);
                dongmuonsua.MatKhau = matkhau;
                _context.SaveChanges();
                return RedirectToAction("Quanly", "Quanly");
            }
            else
            {
                ViewBag.tb = "Mật khẩu nhập lại không đúng so với mật khẩu trên!!!";
            }
            return View();
        }
        public IActionResult Taotk()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Themtk(TaikhoanModel taikhoan)
        {
            var dongcuoi = _context.TaiKhoan.OrderByDescending(x => x.MaTaiKhoan).FirstOrDefault();

            string mataikhoan;

            if (dongcuoi == null)
            {
                mataikhoan = "TK001";
            }
            else
            {
                int so = int.Parse(dongcuoi.MaTaiKhoan.Substring(2));
                so = so + 1;

                mataikhoan = "TK" + so.ToString("D3");
            }
            var bangtk = _context.TaiKhoan.ToList();
            bool a = false;
            if (ModelState.IsValid)
            {
                for(int i = 0; i < bangtk.Count; i++)
                {
                    if (taikhoan.TenDangNhap == bangtk[i].TenDangNhap)
                    {
                        a = false;
                        break;
                    }
                    else
                    {
                        a = true;
                    }
                }
                if (a == true)
                {
                    taikhoan.MaTaiKhoan = mataikhoan;
                    taikhoan.NgayTao = DateTime.Now;
                    _context.TaiKhoan.Add(taikhoan);
                    _context.SaveChanges();
                    return RedirectToAction("Quanly", "Quanly");
                }
                else
                {
                    ViewBag.tb = "Tên tài khoản đã tồn tại";
                }
            }
            else
            {
                ViewBag.tb = "vui lòng nhập dữ liệu!!!";
            }
            return View("Taotk");
        }

    }
}
