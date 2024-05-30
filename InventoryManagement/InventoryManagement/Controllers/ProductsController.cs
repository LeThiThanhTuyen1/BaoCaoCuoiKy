using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Models;

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
            int? pageNumber)
        {
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

            var products = from p in _context.Products
                            .Include(p => p.Supplier)
                            .Include(p => p.Warehouse)
                           select p;
            products = products.Where(p => p.Quantity > 0);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }

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
     
        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductID,Name,Description,Price,Quantity,EntryDate,SupplierId,WarehouseID")] Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await FindProductByNameAndSupplierAsync(product.Name, product.SupplierId);
                if (existingProduct != null)
                {
                    existingProduct.Quantity += product.Quantity;
                    _context.Update(existingProduct);
                }
                else
                {
                    _context.Add(product);
                }

                await _context.SaveChangesAsync();

                // Log the import transaction
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
                TempData["Message"] = "Vượt quá số lượng Hàng có trong kho.";
            }
            else
            {
                product.Quantity -= ExportQuantity;
                _context.Update(product);
                await _context.SaveChangesAsync();

                // Log the export transaction
                var supplier = await _context.Suppliers.FindAsync(product.SupplierId);
                var warehouse = await _context.Warehouses.FindAsync(product.WarehouseID);
                var history = new History
                {
                    ProductName = product.Name,
                    Action = "Xuất Kho",
                    Date = DateTime.Now,
                    Quantitybegin = product.Quantity,
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
    }
}
