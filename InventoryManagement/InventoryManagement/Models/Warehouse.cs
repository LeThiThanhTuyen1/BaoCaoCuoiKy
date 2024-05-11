using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Warehouse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public Manager? Manager { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
