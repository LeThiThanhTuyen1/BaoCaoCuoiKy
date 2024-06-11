using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement;
using System.IO;
using OfficeOpenXml;
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
    public async Task<IActionResult> ExportToExcel(string searchString, string actionFilter)
    {
        try
        {
            var histories = _context.Histories.AsQueryable();

            // Filter by product name
            if (!String.IsNullOrEmpty(searchString))
            {
                histories = histories.Where(h => h.ProductName.Contains(searchString));
            }

            // Filter by action type
            if (!String.IsNullOrEmpty(actionFilter))
            {
                histories = histories.Where(h => h.Action == actionFilter);
            }

            // Fetch data from the database
            var data = await histories.AsNoTracking().ToListAsync();

            // Create an Excel package
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Only needed for non-commercial use of EPPlus
            using (var excelPackage = new ExcelPackage())
            {
                // Create a new sheet
                var sheet = excelPackage.Workbook.Worksheets.Add("Histories");

                // Set column headers
                sheet.Cells[1, 1].Value = "Product ID";
                sheet.Cells[1, 2].Value = "Product Name";
                sheet.Cells[1, 3].Value = "Action";
                sheet.Cells[1, 4].Value = "Date";
                // Continue with other columns based on the data in History

                // Fill in data rows
                int row = 2;
                foreach (var history in data)
                {
                    sheet.Cells[row, 1].Value = history.ID;
                    sheet.Cells[row, 2].Value = history.ProductName;
                    sheet.Cells[row, 3].Value = history.Action;
                    sheet.Cells[row, 4].Value = history.Date.ToString("yyyy-MM-dd"); // Format the date as needed
                                                                                     // Continue with other columns
                    row++;
                }

                // Save the Excel file to a memory stream
                var stream = new MemoryStream();
                excelPackage.SaveAs(stream);
                stream.Position = 0;

                // Return the Excel file as a FileResult
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Histories.xlsx");
            }
        }
        catch (Exception ex)
        {
            // Log the error and return an error message
            // Example: use a logger to log the error
            // _logger.LogError(ex, "Error exporting to Excel");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

