using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using BlockChain.Models;

namespace BlockChain.Controllers
{
    public class PenggunaController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PenggunaController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public IActionResult Pengguna()
        {
            var penggunaList = GetAllUsers();
            return View(penggunaList);
        }

        [HttpPost]
        public IActionResult Tambah(Pengguna pengguna)
        {

            Console.WriteLine($"[DEBUG] Diterima: {pengguna.Username} - {pengguna.Role}");

            if (pengguna.Role == "Distributor" && string.IsNullOrEmpty(pengguna.NamaToko))
            {
                ModelState.AddModelError("NamaToko", "Nama Toko wajib diisi untuk Distributor.");
            }

            if (!ModelState.IsValid) return View("Pengguna", GetAllUsers());

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Pengguna (Username, NamaToko, Password, Role) VALUES (@Username, @NamaToko, @Password, @Role)", conn);
                cmd.Parameters.AddWithValue("@Username", pengguna.Username);
                cmd.Parameters.AddWithValue("@NamaToko", (object)pengguna.NamaToko ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Password", pengguna.Password);
                cmd.Parameters.AddWithValue("@Role", pengguna.Role);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Pengguna");
        }

        [HttpPost]
        public IActionResult Edit(Pengguna pengguna)
        {
            if (pengguna.Role == "Distributor" && string.IsNullOrEmpty(pengguna.NamaToko))
            {
                ModelState.AddModelError("NamaToko", "Nama Toko wajib diisi untuk Distributor.");
            }

            if (!ModelState.IsValid) return View("Pengguna", GetAllUsers());

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Pengguna SET Username=@Username, NamaToko=@NamaToko, Password=@Password, Role=@Role WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", pengguna.Id);
                cmd.Parameters.AddWithValue("@Username", pengguna.Username);
                cmd.Parameters.AddWithValue("@NamaToko", (object)pengguna.NamaToko ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Password", pengguna.Password);
                cmd.Parameters.AddWithValue("@Role", pengguna.Role);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Pengguna");
        }

        [HttpPost]
        public IActionResult Hapus(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Pengguna WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Pengguna");
        }

        private List<Pengguna> GetAllUsers()
        {
            var penggunaList = new List<Pengguna>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Pengguna", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    penggunaList.Add(new Pengguna
                    {
                        Id = (int)reader["Id"],
                        Username = reader["Username"].ToString() ?? "",
                        NamaToko = reader["NamaToko"]?.ToString() ?? "",
                        Password = reader["Password"].ToString() ?? "",
                        Role = reader["Role"].ToString() ?? "",
                        Email = reader["Email"].ToString() ?? ""
                    });
                }
            }
            return penggunaList;
        }
    }
}
