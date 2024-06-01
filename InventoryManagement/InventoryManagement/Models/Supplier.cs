using System;
using System.ComponentModel.DataAnnotations;
namespace InventoryManagement.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }

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
        public ICollection<Product>? Products { get; set; }

    }
}

