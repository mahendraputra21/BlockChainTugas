namespace BlockChain.Models
{
    public class ProdukGudang
    {
        public required string KodeProduk { get; set; }
        public string? NamaProduk { get; set; }
        public string? KategoriProduk { get; set; }
        public int Stok { get; set; }

        public DateTime TanggalMasuk { get; set; }
        public DateTime TanggalExpired { get; set; }
    }
}
