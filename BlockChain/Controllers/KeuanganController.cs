using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using BlockChain.Models;

namespace BlockChain.Controllers
{
    public class KeuanganController : Controller
    {
        public IActionResult Keuanganowner()
        {
            // Data dummy untuk ditampilkan di view
            var transaksiList = new List<TransaksiKeuangan>
            {
                new TransaksiKeuangan
                {
                    Id = "EX19866",
                    Tanggal = new DateTime(2025, 5, 5),
                    Deskripsi = "Produk Bahan",
                    Kategori = "Bahan Pangan",
                    Jumlah = 2450000,
                    IsPemasukan = true
                },
                new TransaksiKeuangan
                {
                    Id = "EX19852",
                    Tanggal = new DateTime(2025, 5, 5),
                    Deskripsi = "Produk Bahan",
                    Kategori = "Bahan Pangan",
                    Jumlah = 650000,
                    IsPemasukan = false
                }
            };

            return View(transaksiList);
        }
    }
}
