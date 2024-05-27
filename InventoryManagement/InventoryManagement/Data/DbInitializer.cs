using System;
using System.Linq;
using InventoryManagement.Models;

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
                return; // DB has been seeded
            }

            var warehouses = new Warehouse[]
            {
                new Warehouse { Name = "Kho A", Location = "An Dương Vương, Nguyễn Văn Cừ, Quy Nhơn" },
                new Warehouse { Name = "Kho B", Location = "45 Phường Nhơn Trãi, Huyện An Nhơn, Bình Định" },
                new Warehouse { Name = "Kho C", Location = "Quốc Lộ 1, Phường Nhơn Hưng, Huyện An Nhơn, Bình Định" }
            };
            foreach (var warehouse in warehouses)
            {
                context.Warehouses.Add(warehouse);
            }
            context.SaveChanges(); // Save warehouses to generate IDs

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
            context.SaveChanges(); // Save suppliers to generate IDs

            var managers = new Manager[]
            {
                new Manager { Name = "Nguyễn Văn An", Address = "An Dương Vương, Nguyễn Văn Cừ, Quy Nhơn", Contact = "0868425266", WarehouseID = warehouses[0].WarehouseID },
                new Manager { Name = "Manager B", Address = "Address for Manager B", Contact = "Contact for Manager B", WarehouseID = warehouses[1].WarehouseID }
            };
            foreach (var manager in managers)
            {
                context.Managers.Add(manager);
            }
            context.SaveChanges(); // Save managers to generate IDs

            var accounts = new Account[]
            {
                new Account { Username = "manager", Password = "manager", Role = "Manager", ManagerId = managers[0].ManagerId },
                new Account { Username = "manager", Password = "manager", Role = "Manager", ManagerId = managers[1].ManagerId }
            };
            foreach (var account in accounts)
            {
                context.Accounts.Add(account);
            }
            context.SaveChanges(); // Save accounts to generate IDs

            var products = new Product[]
            {
                new Product
                {
                    ProductID = "P001",
                    Name = "Gỗ Sồi",
                    Description = "Gỗ Sồi chất lượng cao, xuất xứ từ châu Âu",
                    Price = 5000000,
                    Quantity = 100,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[0].SupplierId,
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Product
                {
                    ProductID = "P002",
                    Name = "Gỗ Tràm",
                    Description = "Gỗ Tràm tự nhiên, màu sắc đẹp",
                    Price = 3500000,
                    Quantity = 200,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[1].SupplierId,
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Product
                {
                    ProductID = "P003",
                    Name = "Gỗ Lim",
                    Description = "Gỗ Lim chắc chắn, bền bỉ",
                    Price = 7000000,
                    Quantity = 150,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[2].SupplierId,
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Product
                {
                    ProductID = "P004",
                    Name = "Gỗ Thông",
                    Description = "Gỗ Thông nhẹ, dễ thi công",
                    Price = 4000000,
                    Quantity = 180,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[3].SupplierId,
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Product
                {
                    ProductID = "P005",
                    Name = "Gỗ Hương",
                    Description = "Gỗ Hương có mùi thơm tự nhiên",
                    Price = 8000000,
                    Quantity = 120,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[4].SupplierId,
                    WarehouseID = warehouses[2].WarehouseID
                },
                new Product
                {
                    ProductID = "P006",
                    Name = "Gỗ Dẻ",
                    Description = "Gỗ Dẻ bền chắc, dễ gia công",
                    Price = 5500000,
                    Quantity = 80,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[0].SupplierId,
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Product
                {
                    ProductID = "P007",
                    Name = "Gỗ Xoan",
                    Description = "Gỗ Xoan chịu nước tốt, bền màu",
                    Price = 3200000,
                    Quantity = 220,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[1].SupplierId,
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Product
                {
                    ProductID = "P008",
                    Name = "Gỗ Pơ Mu",
                    Description = "Gỗ Pơ Mu chịu nhiệt, ít cong vênh",
                    Price = 7500000,
                    Quantity = 130,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[2].SupplierId,
                    WarehouseID = warehouses[2].WarehouseID
                },
                new Product
                {
                    ProductID = "P009",
                    Name = "Gỗ Bạch Đàn",
                    Description = "Gỗ Bạch Đàn vân gỗ đẹp, độ bền cao",
                    Price = 4200000,
                    Quantity = 190,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[3].SupplierId,
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Product
                {
                    ProductID = "P010",
                    Name = "Gỗ Tần Bì",
                    Description = "Gỗ Tần Bì có vân gỗ đẹp, độ bền cao",
                    Price = 8200000,
                    Quantity = 110,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[4].SupplierId,
                    WarehouseID = warehouses[2].WarehouseID
                },
                new Product
                {
                    ProductID = "P011",
                    Name = "Gỗ Mít",
                    Description = "Gỗ Mít có màu vàng đẹp, dễ chế tác",
                    Price = 3000000,
                    Quantity = 240,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[1].SupplierId,
                    WarehouseID = warehouses[2].WarehouseID
                },
                new Product
                {
                    ProductID = "P012",
                    Name = "Gỗ Chò",
                    Description = "Gỗ Chò có màu sắc đẹp, dễ chế tác",
                    Price = 7200000,
                    Quantity = 140,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[2].SupplierId,
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Product
                {
                    ProductID = "P013",
                    Name = "Gỗ Gụ",
                    Description = "Gỗ Gụ có màu đẹp, rất bền",
                    Price = 4300000,
                    Quantity = 170,
                                        EntryDate = DateTime.Now,
                    SupplierId = suppliers[3].SupplierId,
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Product
                {
                    ProductID = "P014",
                    Name = "Gỗ Trắc",
                    Description = "Gỗ Trắc có độ bền cao, màu đẹp",
                    Price = 8500000,
                    Quantity = 100,
                    EntryDate = DateTime.Now,
                    SupplierId = suppliers[4].SupplierId,
                    WarehouseID = warehouses[1].WarehouseID
                }
            };
            foreach (var product in products)
            {
                context.Products.Add(product);
            }
            context.SaveChanges();
        }
    }
}

