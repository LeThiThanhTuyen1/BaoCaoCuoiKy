using System.Linq;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services
{
    public class AccountService
    {
        private readonly InventoryContext _context;

        public AccountService(InventoryContext context)
        {
            _context = context;
        }

        public Account Authenticate(string username, string password)
        {
            return _context.Accounts.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
