using System;
using System.ComponentModel.DataAnnotations;
namespace InventoryManagement.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Contact { get; set; }
    }
}

