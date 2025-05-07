using Microsoft.AspNetCore.Mvc;
using BlockChain.Models;
using System;
using System.Collections.Generic;

namespace BlockChain.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat/Chat
        public IActionResult Chat(int userId)
        {
            // Simulasi daftar pesan - bisa diganti dengan ambil dari database berdasarkan userId
            var pesan = new List<ChatMessage>();

            if (userId == 1)
            {
                pesan = new List<ChatMessage>
                {
                    new ChatMessage { Pengirim = "PT. Blalala", Penerima = "Me", Pesan = "Halo, ada yang bisa dibantu?", Tanggal = DateTime.Now.AddMinutes(-10), IsMe = false },
                    new ChatMessage { Pengirim = "Me", Penerima = "PT. Blalala", Pesan = "Ya, saya butuh informasi stok.", Tanggal = DateTime.Now.AddMinutes(-5), IsMe = true }
                };
            }
            else if (userId == 2)
            {
                pesan = new List<ChatMessage>
                {
                    new ChatMessage { Pengirim = "PT. Blulala", Penerima = "Me", Pesan = "Selamat pagi!", Tanggal = DateTime.Now.AddHours(-1), IsMe = false },
                    new ChatMessage { Pengirim = "Me", Penerima = "PT. Blulala", Pesan = "Pagi, saya ingin tanya harga terbaru.", Tanggal = DateTime.Now.AddMinutes(-30), IsMe = true }
                };
            }
            else
            {
                pesan = new List<ChatMessage>
                {
                    new ChatMessage { Pengirim = "PT. Generic", Penerima = "Me", Pesan = "Ini contoh pesan default.", Tanggal = DateTime.Now, IsMe = false }
                };
            }

            // Kirim data ke View
            return View(pesan);
        }
    }
}
