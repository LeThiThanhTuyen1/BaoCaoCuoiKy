using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InventoryManagement.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Contact { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseID { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}
