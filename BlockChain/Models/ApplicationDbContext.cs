using Microsoft.EntityFrameworkCore;

namespace BlockChain.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Pengguna> Penggunas { get; set; }  // <-- Tambahkan ini
    }
}
