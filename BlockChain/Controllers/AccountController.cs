using Microsoft.AspNetCore.Mvc;
using BlockChain.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlockChain.Services;
using BlockChain.Models.ViewModels;

namespace BlockChain.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly IEmailSender _emailSender;

        public AccountController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context, IEmailSender emailSender)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _emailSender = emailSender; // Dependency injection for email service
        }

        // GET: Halaman Login
        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Dashboard", "Owner");
            }
            return View();
        }

        // POST: Proses Login
        [HttpPost]
        public async Task<IActionResult> Login(string? Username, string Password)
        {
            Username = Username?.Trim().ToLower();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == Username);

            if (user == null)
            {
                ViewBag.Error = "Akun tidak ditemukan.";
                return View();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.KataSandi, Password);

            if (result != PasswordVerificationResult.Success)
            {
                ViewBag.Error = "Kata sandi salah.";
                return View();
            }

            // Simpan data user ke session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("NamaToko", user.NamaToko ?? "");

            return RedirectToAction("Dashboard", "Owner");
        }


        // GET: Halaman Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Proses Register
        // POST: Proses Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUserByUsername = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);
            if (existingUserByUsername != null)
            {
                ModelState.AddModelError("Username", "Username sudah digunakan.");
                return View(model);
            }

            var existingUserByEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUserByEmail != null)
            {
                ModelState.AddModelError("Email", "Email sudah digunakan.");
                return View(model);
            }

            // Upload logo
            string? uniqueFileName = null;
            if (model.LogoFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "logos");
                Directory.CreateDirectory(uploadsFolder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.LogoFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.LogoFile.CopyToAsync(fileStream);
                }
            }

            // Buat User
            var user = new User
            {
                NamaToko = model.NamaToko ?? "",
                Username = model.Username ?? "",
                Email = model.Email ?? "",
                Kategori = model.Kategori ?? "", // Ini kategori makanan
                LogoPath = uniqueFileName,
                KataSandi = ""
            };

            user.KataSandi = _passwordHasher.HashPassword(user, model.KataSandi ?? "");
            _context.Users.Add(user);

            // Buat entri di tabel Pengguna
            var pengguna = new Pengguna
            {
                Nama = model.NamaToko ?? model.Username ?? "",
                Username = model.Username ?? "",
                Email = model.Email ?? "",
                Role = "Distributor", // ini role, bukan kategori
                NamaToko = model.NamaToko ?? "",
                Password = model.KataSandi ?? ""
            };
            
            _context.Penggunas.Add(pengguna);

            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }



        public IActionResult Success()
        {
            return View();
        }

        // GET: Halaman Lupa Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Proses Lupa Password
        // POST: Proses Lupa Password
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string? username, string? email)
        {
            // Cek apakah username atau email kosong
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Username dan email tidak boleh kosong.");
                return View();
            }

            // Cari pengguna berdasarkan username dan email
            username = username?.Trim().ToLower();
            email = email?.Trim().ToLower();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username && u.Email.ToLower() == email);



            if (user == null)
            {
                // Jika kombinasi username dan email tidak ditemukan
                ViewBag.Error = "Username atau email tidak ditemukan.";
                return View();
            }

            // Generate a verification code (6 digit)
            string verificationCode = GenerateVerificationCode();

            // Kirim email verifikasi dengan kode
            string subject = "Kode Verifikasi Reset Password";
            string message = $@"
            <!DOCTYPE html>
            <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <p>Halo <strong>{user.Username}</strong>,</p>
                    <p>Kami menerima permintaan untuk mengatur ulang kata sandi akun Anda.</p>
                    <p>Kode verifikasi Anda adalah:</p>
                    <h2 style='color: #2E86C1;'>{verificationCode}</h2>
                    <p>Jangan berikan kode ini kepada siapa pun. Kode ini hanya berlaku selama 30 menit.</p>
                    <br />
                    <p>Terima kasih,</p>
                    <p><strong>Tim Blockchain</strong></p>
                  </body>
             </html>";

            if (email !=  null)
            {
                await _emailSender.SendEmailAsync(email, subject, message);

                // Simpan kode verifikasi di session
                HttpContext.Session.SetString("VerificationCode", verificationCode);

                HttpContext.Session.SetString("Email", email); // Simpan email untuk digunakan di halaman verifikasi dan reset password

                HttpContext.Session.SetString("Username", user.Username); // tambahkan ini
            }
           
            Console.WriteLine($"Kode Verifikasi Disimpan di Session: {verificationCode}");

            // Redirect ke halaman verifikasi
            return RedirectToAction("VerifyCode");
        }


        private string GenerateVerificationCode()
        {
            // Anda bisa menggunakan logika untuk membuat kode verifikasi secara acak
            // Misalnya, kode 6 digit acak
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }


        // GET: Halaman Verifikasi Kode
        // GET: Halaman Verifikasi Kode
        [HttpGet]
        public IActionResult VerifyCode()
        {
            ViewBag.Email = HttpContext.Session.GetString("Email"); // Ambil email dari session
            ViewBag.ExpectedCode = HttpContext.Session.GetString("VerificationCode");
            return View();
        }

        // POST: Proses Verifikasi Kode
        // POST: Proses Verifikasi Kode
        [HttpPost]
        public IActionResult VerifyCode(string[] CodeDigit)
        {
            // Gabungkan semua digit menjadi satu string
            string code = string.Join("", CodeDigit);
            Console.WriteLine($"Kode yang Dimasukkan: {code}");

            // Ambil kode verifikasi yang disimpan di session
            string expectedCode = HttpContext.Session.GetString("VerificationCode") ?? "";
            Console.WriteLine($"Kode yang Diharapkan: {expectedCode}");

            if (code == expectedCode)
            {
                // Jika kode benar, redirect ke halaman reset password
                return RedirectToAction("ResetPassword", new { email = HttpContext.Session.GetString("Email") });
            }

            // Jika kode salah
            ViewBag.Message = "Kode verifikasi salah.";
            return View();
        }


        // GET: Halaman Reset Password
        // GET: Halaman Reset Password
        // GET: Halaman Reset Password
        [HttpGet]
        public IActionResult ResetPassword()
        {
            string email = HttpContext.Session.GetString("Email") ?? "";
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        // POST: Proses Reset Password
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Email = model.Email;
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase));
            
            if (user == null)
            {
                ViewBag.Message = "Pengguna tidak ditemukan.";
                ViewBag.Email = model.Email;
                return View(model);
            }

            var passParam = model.NewPassword;

            if(passParam != null)
                user.KataSandi = _passwordHasher.HashPassword(user, passParam);
            
            await _context.SaveChangesAsync();

            // Clear session setelah reset password untuk memastikan pengguna tidak otomatis login
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }




        // GET: Logout
        [HttpGet]
        public IActionResult Logout()
        {
            // Hapus seluruh sesi user
            HttpContext.Session.Clear();

            // Gunakan Response.Redirect untuk menghindari kembali ke halaman sebelumnya
            Response.Redirect("/Account/Login");

            return new EmptyResult();
        }
    }
}
