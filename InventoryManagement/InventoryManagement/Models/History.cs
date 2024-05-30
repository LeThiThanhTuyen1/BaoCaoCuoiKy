using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class History
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Action { get; set; } // "Nhập kho" or "Xuất kho"
        
        public DateTime Date { get; set; }

        public int Quantitybegin { get; set; }
        public int Quantity { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseName { get; set; }
    }
}
