﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace InventoryManagement.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string? ProductID { get; set; } 

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Now;

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; } 
        public Supplier? Supplier { get; set; }
        
        [ForeignKey("Warehouse")]
        public int WarehouseID { get; set; } 
        public Warehouse? Warehouse { get; set; }
    }

}

