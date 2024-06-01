using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement;

public class HistoryController : Controller
{
    private readonly InventoryContext _context;

    public HistoryController(InventoryContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber, string actionFilter)
    {
        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentSort"] = sortOrder;
        ViewData["CurrentActionFilter"] = actionFilter;

        // Retrieve the list of actions for the dropdown
        var actionList = await _context.Histories.Select(h => h.Action).Distinct().ToListAsync();
        ViewData["ActionList"] = new SelectList(actionList);

        var histories = from h in _context.Histories
                        select h;

        if (!String.IsNullOrEmpty(searchString))
        {
            histories = histories.Where(h => h.ProductName.Contains(searchString));
        }

        if (!String.IsNullOrEmpty(actionFilter))
        {
            histories = histories.Where(h => h.Action == actionFilter);
        }

        switch (sortOrder)
        {
            case "name_desc":
                histories = histories.OrderByDescending(h => h.ProductName);
                break;
            case "date":
                histories = histories.OrderBy(h => h.Date);
                break;
            case "date_desc":
                histories = histories.OrderByDescending(h => h.Date);
                break;
            default:
                histories = histories.OrderByDescending(h => h.Date); // Default to sorting by date descending
                break;
        }

        int pageSize = 5; // Set the page size to 5
        return View(await PaginatedList<History>.CreateAsync(histories.AsNoTracking(), pageNumber ?? 1, pageSize));
    }
}
