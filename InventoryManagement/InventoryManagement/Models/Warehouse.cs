using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }

        [Required(ErrorMessage = "Tên kho không được để trống.")]
        [MaxLength(100, ErrorMessage = "Tên kho không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên kho")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vị trí không được để trống.")]
        [MaxLength(200, ErrorMessage = "Vị trí không được vượt quá 200 ký tự.")]
        [Display(Name = "Vị trí")]
        public string? Location { get; set; }
    }
}
