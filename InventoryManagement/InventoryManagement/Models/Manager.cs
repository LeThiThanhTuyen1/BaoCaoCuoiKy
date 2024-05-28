using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InventoryManagement.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "Tên không được để trống.")]
        [MaxLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự.")]
        [Display(Name = "Tên")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        [MaxLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự.")]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Liên hệ không được để trống.")]
        [MaxLength(200, ErrorMessage = "Liên hệ không được vượt quá 200 ký tự.")]
        [Display(Name = "Liên hệ")]
        public string? Contact { get; set; }

        [ForeignKey("Warehouse")]
        [Display(Name = "Kho")]
        public int WarehouseID { get; set; }
        [Display(Name = "Kho")]
        public Warehouse? Warehouse { get; set; }
    }
}
