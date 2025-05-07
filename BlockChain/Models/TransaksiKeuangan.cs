using System;

namespace BlockChain.Models
{
    public class TransaksiKeuangan
    {
        public required string Id { get; set; }
        public DateTime Tanggal { get; set; }
        public string? Deskripsi { get; set; }
        public string? Kategori { get; set; }
        public decimal Jumlah { get; set; }
        public bool IsPemasukan { get; set; } // true = pemasukan, false = pengeluaran
    }
}
