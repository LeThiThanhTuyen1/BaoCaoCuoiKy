using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Models;
using OfficeOpenXml;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly InventoryContext _context;

        public ProductsController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(
            string searchString,
            string sortOrder,
            string currentFilter,
            int? pageNumber,
            string warehouseID
            )
        {
            // Chuẩn bị danh sách kho cho dropdown
            var warehouses = await _context.Warehouses.Select(w => new SelectListItem
            {
                Value = w.WarehouseID.ToString(),
                Text = w.Name
            }).ToListAsync();
            ViewData["WarehouseList"] = new SelectList(warehouses, "Value", "Text");

            // Xử lý tìm kiếm và bộ lọc hiện tại
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["SupplierSortParm"] = String.IsNullOrEmpty(sortOrder) ? "supplier_desc" : "";
            ViewData["CurrentSort"] = sortOrder;

            // Truy vấn sản phẩm
            var products = from p in _context.Products
                            .Include(p => p.Supplier)
                            .Include(p => p.Warehouse)
                           select p;
            products = products.Where(p => p.Quantity > 0);

            // Lọc theo chuỗi tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }

            // Lọc theo kho
            if (!string.IsNullOrEmpty(warehouseID))
            {
                int warehouseId = int.Parse(warehouseID);
                products = products.Where(p => p.WarehouseID == warehouseId);
            }

            // Logic sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "EntryDate":
                    products = products.OrderBy(s => s.EntryDate);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.EntryDate);
                    break;
                case "Supplier":
                    products = products.OrderBy(s => s.Supplier.Name);
                    break;
                case "supplier_desc":
                    products = products.OrderByDescending(s => s.Supplier.Name);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }

            // Thiết lập WarehouseID trong ViewData để truyền vào view
            ViewData["WarehouseID"] = warehouseID;

            int pageSize = 5;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Phương thức kiểm tra sản phẩm tồn tại
        private async Task<Product> FindProductByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        // Hỗ trợ kiểm tra nha cung cấp
        // Hỗ trợ kiểm tra nhà cung cấp
        private async Task<Product> FindProductByNameAndSupplierAsync(string name, int supplierId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Name == name && p.SupplierId == supplierId);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name");
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "WarehouseID", "Name");
            var product = new Product
            {
                EntryDate = DateTime.Now
            };
            return View(product);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductID,Name,Description,Price,Quantity,EntryDate,SupplierId,WarehouseID")] Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await FindProductByNameAndSupplierAsync(product.Name, product.SupplierId);
                if (existingProduct != null)
                {
                    var initialQuantity = existingProduct.Quantity;
                    existingProduct.Quantity += product.Quantity;
                    _context.Update(existingProduct);

                    // Log the import transaction
                    var supplier = await _context.Suppliers.FindAsync(product.SupplierId);
                    var warehouse = await _context.Warehouses.FindAsync(product.WarehouseID);
                    var history = new History
                    {
                        ProductName = product.Name,
                        Action = "Nhập Hàng",
                        Date = DateTime.Now,
                        Quantitybegin = initialQuantity,
                        Quantity = product.Quantity,
                        SupplierName = supplier.Name,
                        WarehouseName = warehouse.Name
                    };

                    _context.Histories.Add(history);
                }
                else
                {
                    _context.Add(product);

                    // Log the import transaction for a new product
                    var supplier = await _context.Suppliers.FindAsync(product.SupplierId);
                    var warehouse = await _context.Warehouses.FindAsync(product.WarehouseID);
                    var history = new History
                    {
                        ProductName = product.Name,
                        Action = "Nhập Hàng",
                        Date = DateTime.Now,
                        Quantitybegin = 0,
                        Quantity = product.Quantity,
                        SupplierName = supplier.Name,
                        WarehouseName = warehouse.Name
                    };

                    _context.Histories.Add(history);
                }

                await _context.SaveChangesAsync();
                TempData["Message"] = existingProduct != null ? "Sản phẩm của bạn đã có trước đó, đã cập nhật lại số lượng." : "Đã thêm thành công.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "WarehouseID", "Name", product.WarehouseID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "WarehouseID", "Name", product.WarehouseID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductID,Name,Description,Price,Quantity,EntryDate,SupplierId,WarehouseID")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", product.SupplierId);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "WarehouseID", "Name", product.WarehouseID);
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Export
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Export(int ID, int ExportQuantity)
        {
            var product = await _context.Products.FindAsync(ID);
            if (product == null)
            {
                return NotFound();
            }

            if (ExportQuantity > product.Quantity)
            {
                TempData["Message"] = "Vượt quá số lượng hàng có trong kho.";
            }
            else
            {
                // Log initial quantity before export
                int initialQuantity = product.Quantity;

                // Update product quantity
                product.Quantity -= ExportQuantity;
                _context.Update(product);
                await _context.SaveChangesAsync();

                // Log the export transaction
                var supplier = await _context.Suppliers.FindAsync(product.SupplierId);
                var warehouse = await _context.Warehouses.FindAsync(product.WarehouseID);
                var history = new History
                {
                    ProductName = product.Name,
                    Action = "Xuất Hàng",
                    Date = DateTime.Now,
                    Quantitybegin = initialQuantity,
                    Quantity = ExportQuantity,
                    SupplierName = supplier.Name,
                    WarehouseName = warehouse.Name
                };

                _context.Histories.Add(history);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Xuất hàng thành công.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }

        //Thống kê từng nhà cung cấp còn lại bao nhiêu sản phẩm 
        public IActionResult Statistics(string sortOrder)
        {
            // Lấy tổng giá trị hàng tồn kho
            var totalInventoryValue = _context.Products.Sum(p => p.Quantity * p.Price);

            // Lấy thông tin tồn kho theo nhà cung cấp
            var supplierInventory = _context.Products
                .GroupBy(p => p.Supplier.Name)
                .Select(g => new { SupplierName = g.Key, TotalQuantity = g.Sum(p => p.Quantity) });

            // Thêm sắp xếp
            ViewData["SupplierNameSortParam"] = String.IsNullOrEmpty(sortOrder) || sortOrder != "name_desc" ? "name_desc" : "";
            ViewData["QuantitySortParam"] = sortOrder == "quantity_desc" ? "quantity_asc" : "quantity_desc";

            switch (sortOrder)
            {
                case "name_desc":
                    supplierInventory = supplierInventory.OrderByDescending(g => g.SupplierName);
                    break;
                case "quantity_desc":
                    supplierInventory = supplierInventory.OrderByDescending(g => g.TotalQuantity);
                    break;
                case "quantity_asc":
                    supplierInventory = supplierInventory.OrderBy(g => g.TotalQuantity);
                    break;
                default:
                    supplierInventory = supplierInventory.OrderBy(g => g.SupplierName);
                    break;
            }

            ViewData["TotalInventoryValue"] = totalInventoryValue;
            ViewData["SupplierInventory"] = supplierInventory;

            return View();
        }

        //Xem số lượng sản phẩm còn lại của từng nhà cung cấp
        public IActionResult SupplierProducts(string supplierName, string sortOrder)
        {
            var supplierProducts = _context.Products
                .Where(p => p.Supplier.Name == supplierName)
                .Select(p => new { ProductName = p.Name, Quantity = p.Quantity });

            ViewData["SupplierName"] = supplierName;
            ViewData["ProductNameSortParam"] = String.IsNullOrEmpty(sortOrder) || sortOrder != "name_desc" ? "name_desc" : "";
            ViewData["QuantitySortParam"] = sortOrder == "quantity_desc" ? "quantity_asc" : "quantity_desc";

            switch (sortOrder)
            {
                case "name_desc":
                    supplierProducts = supplierProducts.OrderByDescending(p => p.ProductName);
                    break;
                case "quantity_desc":
                    supplierProducts = supplierProducts.OrderByDescending(p => p.Quantity);
                    break;
                case "quantity_asc":
                    supplierProducts = supplierProducts.OrderBy(p => p.Quantity);
                    break;
                default:
                    supplierProducts = supplierProducts.OrderBy(p => p.ProductName);
                    break;
            }

            return View(supplierProducts.ToList());
        }

        public async Task<IActionResult> NStatistics(DateTime? startDate, DateTime? endDate)
        {
            // Kiểm tra và gán giá trị mặc định cho khoảng thời gian nếu không có giá trị
            startDate ??= DateTime.MinValue;
            endDate ??= DateTime.MaxValue;

            // Lấy tổng giá trị hàng tồn kho
            var totalInventoryValue = await _context.Products.SumAsync(p => p.Quantity * p.Price);

            // Lấy tổng số lượng và giá trị hàng nhập kho trong khoảng thời gian cụ thể
            var importTransactions = await _context.Histories
                .Where(h => h.Action == "Nhập Hàng" && h.Date >= startDate && h.Date <= endDate)
                .ToListAsync();

            var totalQuantityImported = importTransactions.Sum(h => h.Quantity);
            var totalValueImported = 0m;

            // Tính tổng giá trị nhập kho
            foreach (var transaction in importTransactions)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == transaction.ProductName);
                if (product != null)
                {
                    totalValueImported += transaction.Quantity * product.Price;
                }
            }

            // Tạo view model và gán giá trị
            var viewModel = new StatisticsViewModel
            {
                TotalInventoryValue = totalInventoryValue,
                TotalQuantityImported = totalQuantityImported,
                TotalValueImported = totalValueImported,
                Histories = importTransactions
            };

            // Truyền view model vào view
            return View(viewModel);
        }

        public async Task<IActionResult> XStatistics(DateTime? startDate, DateTime? endDate)
        {
            // Kiểm tra và gán giá trị mặc định cho khoảng thời gian nếu không có giá trị
            startDate ??= DateTime.MinValue;
            endDate ??= DateTime.MaxValue;

            // Lấy tổng số lượng và giá trị hàng xuất kho trong khoảng thời gian cụ thể
            var exportTransactions = await _context.Histories
                .Where(h => h.Action == "Xuất Hàng" && h.Date >= startDate && h.Date <= endDate)
                .ToListAsync();

            var totalQuantityExported = exportTransactions.Sum(h => h.Quantity);
            var totalValueExported = 0m;

            // Tính tổng giá trị xuất kho
            foreach (var transaction in exportTransactions)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == transaction.ProductName);
                if (product != null)
                {
                    totalValueExported += transaction.Quantity * product.Price;
                }
            }

            // Tạo view model và gán giá trị
            var viewModel = new StatisticsViewModel
            {
                TotalQuantityExported = totalQuantityExported,
                TotalValueExported = totalValueExported,
                Histories = exportTransactions
            };

            // Truyền view model vào view
            return View(viewModel);
        }

        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var products = await _context.Products.ToListAsync();

                // Tạo một package Excel
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var excelPackage = new ExcelPackage())
                {
                    // Tạo một sheet mới
                    var sheet = excelPackage.Workbook.Worksheets.Add("Products");

                    // Đặt tiêu đề cột
                    sheet.Cells[1, 1].Value = "Mã hàng";
                    sheet.Cells[1, 2].Value = "Tên hàng";
                    sheet.Cells[1, 3].Value = "Mô tả";
                    sheet.Cells[1, 4].Value = "Gía cả";
                    sheet.Cells[1, 5].Value = "Số lượng";
                    sheet.Cells[1, 6].Value = "Ngày nhập kho";
                    // Tiếp tục với các cột khác

                    // Đổ dữ liệu vào từng dòng
                    int row = 2;
                    foreach (var product in products)
                    {
                        sheet.Cells[row, 1].Value = product.ID;
                        sheet.Cells[row, 2].Value = product.Name;
                        sheet.Cells[row, 3].Value = product.Description;
                        sheet.Cells[row, 4].Value = product.Price;
                        sheet.Cells[row, 5].Value = product.Quantity;
                        sheet.Cells[row, 6].Value = product.EntryDate.ToString("yyyy-MM-dd");
                        // Tiếp tục với các cột khác
                        row++;
                    }

                    // Lưu file Excel vào memory stream
                    using (var stream = new MemoryStream())
                    {
                        excelPackage.SaveAs(stream);
                        var fileBytes = stream.ToArray(); // Chuyển MemoryStream thành mảng byte

                        // Trả về file Excel như một FileResult
                        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về thông báo lỗi
                // Ví dụ: sử dụng một logger để log lỗi
                // _logger.LogError(ex, "Error exporting to Excel");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

