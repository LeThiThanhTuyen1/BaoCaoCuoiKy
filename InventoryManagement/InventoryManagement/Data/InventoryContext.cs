using Microsoft.EntityFrameworkCore;
using InventoryManagement.Models;

public class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
    {
    }

    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<History> Histories { get; set; } // Add this line

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manager>()
            .HasOne(m => m.Warehouse)
            .WithMany(w => w.Managers)
            .HasForeignKey(m => m.WarehouseID);

        modelBuilder.Entity<Account>()
            .HasOne(a => a.Manager)
            .WithMany(m => m.Account)
            .HasForeignKey(a => a.ManagerId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Warehouse)
            .WithMany(w => w.Products)
            .HasForeignKey(p => p.WarehouseID);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        // Configure the History entity
        modelBuilder.Entity<History>()
            .Property(h => h.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<History>()
            .Property(h => h.Action)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<History>()
            .Property(h => h.Date)
            .IsRequired();

        modelBuilder.Entity<History>()
            .Property(h => h.Quantity)
            .IsRequired();

        modelBuilder.Entity<History>()
            .Property(h => h.SupplierName)
            .HasMaxLength(100);

        modelBuilder.Entity<History>()
            .Property(h => h.WarehouseName)
            .HasMaxLength(100);
    }
}
