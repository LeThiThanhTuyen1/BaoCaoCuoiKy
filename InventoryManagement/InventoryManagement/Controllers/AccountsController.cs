using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Models;
using InventoryManagement.Data;
using InventoryManagement.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers
{
    public class AccountsController : Controller
    {
        private readonly InventoryContext _context;
        private readonly AccountService _accountService;

        public AccountsController(InventoryContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = _accountService.Authenticate(username, password);
            if (account == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                // Thiết lập các thuộc tính xác thực khác nếu cần
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Store user information in session
            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetString("Role", account.Role);

            // Redirect to Home page after successful login
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Accounts");
        }

        // GET: Accounts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            string roleFilter,
            string currentRoleFilter,
            int? pageNumber)
        {
            // Sorting parameters
            ViewData["CurrentSort"] = sortOrder;
            ViewData["UsernameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewData["PasswordSortParm"] = sortOrder == "password" ? "password_desc" : "password";
            ViewData["ManagerSortParm"] = sortOrder == "manager" ? "manager_desc" : "manager";
            ViewData["RoleSortParm"] = sortOrder == "role" ? "role_desc" : "role";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentRoleFilter"] = roleFilter;

            // Paging and filtering logic
            if (searchString != null || roleFilter != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
                roleFilter = currentRoleFilter;
            }

            // Fetch accounts with related managers
            var accountsContext = _context.Accounts.Include(a => a.Manager).AsQueryable();

            // Apply search filter
            if (!String.IsNullOrEmpty(searchString))
            {
                accountsContext = accountsContext.Where(a => a.Username.Contains(searchString));
            }

            // Apply role filter
            if (!String.IsNullOrEmpty(roleFilter))
            {
                accountsContext = accountsContext.Where(a => a.Role == roleFilter);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "username_desc":
                    accountsContext = accountsContext.OrderByDescending(a => a.Username);
                    break;
                case "password_desc":
                    accountsContext = accountsContext.OrderByDescending(a => a.Password);
                    break;
                case "manager_desc":
                    accountsContext = accountsContext.OrderByDescending(a => a.Manager.Name);
                    break;
                case "role":
                    accountsContext = accountsContext.OrderBy(a => a.Role);
                    break;
                case "role_desc":
                    accountsContext = accountsContext.OrderByDescending(a => a.Role);
                    break;
                default:
                    accountsContext = accountsContext.OrderBy(a => a.Username);
                    break;
            }

            // Define a list of roles
            var roles = new List<string> { "Admin", "Manager" }; // Example roles, replace with your actual roles
            var roleSelectList = new SelectList(roles);

            // Add the roles to ViewData
            ViewData["RoleList"] = roleSelectList;

            // Define page size and return paginated list
            int pageSize = 8;
            return View(await PaginatedList<Account>.CreateAsync(accountsContext.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Admin")]
        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Manager)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }


        [Authorize(Roles = "Admin")]
        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "Name");
            ViewData["Role"] = new SelectList(_context.Accounts.Select(a => a.Role).Distinct().ToList());
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,Password,ManagerId,Role")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "Name", account.ManagerId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        [Authorize(Roles = "Admin")]
        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "Name", account.ManagerId);
            ViewData["Role"] = new SelectList(_context.Accounts.Select(a => a.Role).Distinct().ToList(), account.Role);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Username,Password,ManagerId,Role")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
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
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "Name", account.ManagerId);
            return View(account);
        }
        // GET: Accounts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Manager)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
       
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}
