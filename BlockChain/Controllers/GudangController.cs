using BlockChain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlockChain.Controllers
{
    public class GudangController : Controller
    {
        public IActionResult Gudangowner()
        {
            var data = new List<ProdukGudang>
            {
                new ProdukGudang
                {
                    KodeProduk = "6544246890",
                    NamaProduk = "Tepung",
                    KategoriProduk = "Bahan Makanan",
                    Stok = 120,
                    TanggalMasuk = new DateTime(2025, 3, 26),
                    TanggalExpired = new DateTime(2026, 3, 29)
                },
                new ProdukGudang
                {
                    KodeProduk = "6544246839",
                    NamaProduk = "Mentega",
                    KategoriProduk = "Bahan Makanan",
                    Stok = 75,
                    TanggalMasuk = new DateTime(2025, 3, 26),
                    TanggalExpired = new DateTime(2026, 3, 29)
                }
            };

            return View(data);
        }
    }
}
