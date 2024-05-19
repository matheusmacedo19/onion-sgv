using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Onion.SGV.API.Models;
using System.Reflection.Metadata;
using System.Security.Principal;

namespace Onion.SGV.API.Data
{
    public class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "  OnionDb");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
            new { Id = 1, Name = "Celular", Price = 1000.0 },
            new { Id = 2, Name = "Notebook", Price = 3000.0 },
            new { Id = 3, Name = "Televisão", Price = 5000.0 });

            modelBuilder.Entity<Order>()
             .HasOne(e => e.Client)
             .WithMany(e => e.Orders)
             .HasForeignKey(e => e.ClientId);

            modelBuilder.Entity<Order>()
                .HasOne(e => e.Product)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.ProductId);

        
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
