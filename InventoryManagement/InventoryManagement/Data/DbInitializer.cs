using System;
using System.Linq;
using InventoryManagement.Models;
using InventoryManagement.Data;

namespace InventoryManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InventoryContext context)
        {
            context.Database.EnsureCreated();

            // Look for any products.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var suppliers = new Supplier[]
            {
                new Supplier { Name = "Supplier A", Address = "Address for Supplier A", Contact = "Contact for Supplier A" },
                new Supplier { Name = "Supplier B", Address = "Address for Supplier B", Contact = "Contact for Supplier B" }
            };
            foreach (var supplier in suppliers)
            {
                context.Suppliers.Add(supplier);
            }
            context.SaveChanges();

            var managers = new Manager[]
            {
                new Manager { Name = "Manager A", Address = "Address for Manager A", Contact = "Contact for Manager A", ManagedWarehouse = "Kho A" },
                new Manager { Name = "Manager B", Address = "Address for Manager B", Contact = "Contact for Manager B", ManagedWarehouse = "Kho B" }
            };
            foreach (var manager in managers)
            {
                context.Managers.Add(manager);
            }
            context.SaveChanges();

            var products = new Product[]
            {
                new Product { Name = "Product 1", Description = "Description for Product 1", Price = 10.5m, Quantity = 100, EntryDate = DateTime.Parse("2024-05-11"), SupplierId = 1, WarehouseType = "Kho A" },
                new Product { Name = "Product 2", Description = "Description for Product 2", Price = 20.75m, Quantity = 50, EntryDate = DateTime.Parse("2024-05-12"), SupplierId = 2, WarehouseType = "Kho B" }
            };
            foreach (var product in products)
            {
                context.Products.Add(product);
            }
            context.SaveChanges();
        }
    }
}
