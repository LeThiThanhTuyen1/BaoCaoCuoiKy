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
                new Supplier
                {
                    Name = "Công ty TNHH 1 thành viên MCK",
                    Address = "Hoài Nhơn, Quy Nhơn, Bình Định",
                    Contact = "mck_tnhh@gmail.com"
                },
                new Supplier
                {
                    Name = "Công ty TNHH ABC",
                    Address = "123 Đường XYZ, Phường ABC, Quận 1, TP. Hồ Chí Minh",
                    Contact = "contact@abc.com.vn"
                },
                new Supplier
                {
                    Name = "Công ty Cổ phần DEF",
                    Address = "456 Đường DEF, Phường DEF, Quận 2, TP. Đà Nẵng",
                    Contact = "info@defcorp.com.vn"
                },
                new Supplier
                {
                    Name = "Công ty TNHH GHI",
                    Address = "789 Đường GHI, Phường GHI, Quận 3, TP. Hà Nội",
                    Contact = "support@ghi.vn"
                },
                new Supplier
                {
                    Name = "Công ty TNHH JKL",
                    Address = "10 Đường JKL, Phường JKL, Quận 4, TP. Hải Phòng",
                    Contact = "sales@jkl.com.vn"
                }
            };
            foreach (var supplier in suppliers)
            {
                context.Suppliers.Add(supplier);
            }
            context.SaveChanges();

            var managers = new Manager[]
            {
                new Manager { Name = "Nguyễn Văn An", Address = "An Dương Vương, Nguyễn Văn Cừ, Quy Nhơn", Contact = "0868425266", ManagedWarehouse = "Kho A" },
                new Manager { Name = "Manager B", Address = "Address for Manager B", Contact = "Contact for Manager B", ManagedWarehouse = "Kho B" }
            };
            foreach (var manager in managers)
            {
                context.Managers.Add(manager);
            }
            context.SaveChanges();

            var products = new Product[]
            {
                new Product {ProductID = "A01",  Name = "Tủ gỗ thông", Description = "Description for Product 1", Price = 10.5m, Quantity = 100, EntryDate = DateTime.Parse("2024-05-11"), SupplierId = 1, WarehouseType = "Kho A" },
                new Product {ProductID = "A02", Name = "Product 2", Description = "Description for Product 2", Price = 20.75m, Quantity = 50, EntryDate = DateTime.Parse("2024-05-12"), SupplierId = 2, WarehouseType = "Kho B" }
            };
            foreach (var product in products)
            {
                context.Products.Add(product);
            }
            context.SaveChanges();
        }
    }
}
