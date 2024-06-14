using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers
{
    public class ManagersController : Controller
    {
        private readonly InventoryContext _context;

        public ManagersController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Managers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber,
            int? warehouseFilter)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var inventoryContext = _context.Managers.Include(m => m.Warehouse).AsQueryable();


            if (!String.IsNullOrEmpty(searchString))
            {
                inventoryContext = inventoryContext.Where(s => s.Name.Contains(searchString)
                                               || s.Address.Contains(searchString));
            }

            if (warehouseFilter.HasValue && warehouseFilter.Value != 0)
            {
                inventoryContext = inventoryContext.Where(m => m.WarehouseID == warehouseFilter.Value);
            }
            var warehouses = await _context.Warehouses.ToListAsync();
            SelectList warehouseSelectList;
            if (warehouses != null && warehouses.Any())
            {
                warehouseSelectList = new SelectList(warehouses, "WarehouseID", "Name");
            }
            else
            {
                warehouseSelectList = new SelectList(new List<Warehouse>(), "WarehouseID", "Name");
            }

            ViewData["WarehouseList"] = warehouseSelectList;
            ViewData["WarehouseFilter"] = warehouseFilter;

            switch (sortOrder)
            {
                case "name_desc":
                    inventoryContext = inventoryContext.OrderByDescending(s => s.Name);
                    break;
                default:
                    inventoryContext = inventoryContext.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Manager>.CreateAsync(inventoryContext.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Managers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(m => m.Warehouse)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // GET: Managers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "WarehouseID", "Name");
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ManagerId,Name,Address,Contact,WarehouseID")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manager);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo mới người quản lý thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "WarehouseID", "Name", manager.WarehouseID);
            return View(manager);
        }

        // GET: Managers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "WarehouseID", "Name", manager.WarehouseID);
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ManagerId,Name,Address,Contact,WarehouseID")] Manager manager)
        {
            if (id != manager.ManagerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa người quản lý thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.ManagerId))
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
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "WarehouseID", "Name", manager.WarehouseID);
            return View(manager);
        }

        // GET: Managers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(m => m.Warehouse)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // POST: Managers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager != null)
            {
                _context.Managers.Remove(manager);
                TempData["SuccessMessage"] = "Xóa người quản lý thành công!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool ManagerExists(int id)
        {
            return _context.Managers.Any(e => e.ManagerId == id);
        }
    }
}
