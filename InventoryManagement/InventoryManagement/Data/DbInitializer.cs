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
                new Manager
                {
                    Name = "Nguyễn Văn An",
                    Address = "An Dương Vương, Nguyễn Văn Cừ, Quy Nhơn",
                    Contact = "0868425266",
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Manager
                {
                    Name = "Trần Thị Hoa",
                    Address = "123 Lý Thường Kiệt, Hà Nội",
                    Contact = "0987654321",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Phạm Văn Tùng",
                    Address = "789 Trần Hưng Đạo, Đà Nẵng",
                    Contact = "0905678901",
                    WarehouseID = warehouses[2].WarehouseID
                },
                new Manager
                {
                    Name = "Lê Thị Lan",
                    Address = "456 Nguyễn Huệ, Thành phố Hồ Chí Minh",
                    Contact = "0903456789",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Nguyễn Đức Anh",
                    Address = "234 Lê Lợi, Huế",
                    Contact = "0912345678",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Trần Minh Ngọc",
                    Address = "567 Hàng Bông, Hà Nội",
                    Contact = "0989876543",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Phạm Thị Hải",
                    Address = "890 Lê Duẩn, Đà Nẵng",
                    Contact = "0901234567",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Võ Văn Minh",
                    Address = "111 Phan Đình Phùng, Thành phố Hồ Chí Minh",
                    Contact = "0909876543",
                    WarehouseID = warehouses[2].WarehouseID
                },
                new Manager
                {
                    Name = "Nguyễn Thị Hương",
                    Address = "222 Nguyễn Thái Học, Vinh",
                    Contact = "0908765432",
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Manager
                {
                    Name = "Lê Văn Đức",
                    Address = "333 Trần Phú, Nha Trang",
                    Contact = "0907654321",
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Manager
                {
                    Name = "Trần Thị Lan Anh",
                    Address = "444 Hoàng Diệu, Hải Phòng",
                    Contact = "0906543210",
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Manager
                {
                    Name = "Phạm Quốc Tuấn",
                    Address = "555 Nguyễn Chí Thanh, Đà Lạt",
                    Contact = "0905432109",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Nguyễn Thị Hải Yến",
                    Address = "666 Ngô Quyền, Hạ Long",
                    Contact = "0904321098",
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Manager
                {
                    Name = "Lê Hoàng Anh",
                    Address = "777 Lê Lợi, Vũng Tàu",
                    Contact = "0903210987",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Trần Văn Phương",
                    Address = "888 Trường Chinh, Cần Thơ",
                    Contact = "0902109876",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Phạm Thị Lan Anh",
                    Address = "999 Lê Hồng Phong, Rạch Giá",
                    Contact = "0901098765",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Nguyễn Văn Minh",
                    Address = "101 Nguyễn Tất Thành, Hải Dương",
                    Contact = "0900987654",
                    WarehouseID = warehouses[1].WarehouseID
                },
                new Manager
                {
                    Name = "Lê Thị Thanh Huyền",
                    Address = "202 Hùng Vương, Thái Nguyên",
                    Contact = "0899876543",
                    WarehouseID = warehouses[2].WarehouseID
                },
                new Manager
                {
                    Name = "Trần Quang Huy",
                    Address = "303 Lý Thường Kiệt, Buôn Ma Thuột",
                    Contact = "0898765432",
                    WarehouseID = warehouses[0].WarehouseID
                },
                new Manager
                {
                    Name = "Phạm Thanh Bình",
                    Address = "404 Nguyễn Trãi, Bạc Liêu",
                    Contact = "0897654321",
                    WarehouseID = warehouses[1].WarehouseID
                }
            };
            foreach (var manager in managers)
            {
                context.Managers.Add(manager);
            }
            context.SaveChanges(); // Save managers to generate IDs

            var accounts = new Account[]
            {
                new Account
                {
                    Username = "admin",
                    Password = "admin",
                    Role = "Admin",
                    ManagerId = managers[0].ManagerId
                },
                new Account
                {
                    Username = "manager2",
                    Password = "manager2",
                    Role = "Manager",
                    ManagerId = managers[1].ManagerId
                },
                new Account
                {
                    Username = "admin",
                    Password = "admin",
                    Role = "Admin",
                    ManagerId = managers[2].ManagerId
                },
                new Account
                {
                    Username = "manager3",
                    Password = "manager3",
                    Role = "Manager",
                    ManagerId = managers[3].ManagerId
                },
                new Account
                {
                    Username = "manager4",
                    Password = "manager4",
                    Role = "Manager",
                    ManagerId = managers[4].ManagerId
                }
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