using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace InventoryManagement.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string ProductID { get; set; } // Fixed: Removed '?'

        [Required]
        public string Name { get; set; } // Fixed: Removed '?'

        public string Description { get; set; } // Fixed: Removed '?'

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime EntryDate { get; set; }
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; } // Fixed: Renamed from Supplier
        public Supplier Supplier { get; set; }
        
        [ForeignKey("Warehouse")]
        public int WarehouseID { get; set; } // Fixed: Renamed from Warehouse
        public Warehouse Warehouse { get; set; }
    }

}

