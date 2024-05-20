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
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.ClientId)
                .IsRequired(false);
           
            modelBuilder.Entity<Order>()
                .HasOne(e => e.Client)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.ClientId)
                .IsRequired(false);

            modelBuilder.Entity<Order>()
                .HasOne(e => e.Product)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.ProductId)
                .IsRequired(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Orders)
                .WithOne(e => e.Product)
                .HasForeignKey(e => e.ProductId)
                .IsRequired(false);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
