using System;

namespace BlockChain.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string? Pengirim { get; set; }
        public string? Penerima { get; set; }
        public string? Pesan { get; set; }
        public DateTime Tanggal { get; set; }
        public bool IsMe { get; set; } // Untuk identifikasi posisi pesan di kanan atau kiri
    }
}
