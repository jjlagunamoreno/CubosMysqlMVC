using Microsoft.EntityFrameworkCore;
using PracticaCubosMVC.Models;

namespace PracticaCubosMVC.Data
{
    public class CubosContext : DbContext
    {
        public CubosContext(DbContextOptions<CubosContext> options) : base(options) { }

        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<Compra> Compras { get; set; }
    }
}
