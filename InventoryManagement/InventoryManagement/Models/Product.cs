using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace InventoryManagement.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Display(Name = "Mã hàng")]
        public string? ProductID { get; set; }

        [Required(ErrorMessage = "Tên hàng không được để trống.")]
        [MaxLength(200, ErrorMessage = "Tên hàng không được vượt quá 200 ký tự.")]
        [Display(Name = "Tên hàng")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Mô tả hàng không được để trống.")]
        [MaxLength(200, ErrorMessage = "Mô tả hàng không được vượt quá 200 ký tự.")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "Gía cả hàng không được để trống.")]
        [Display(Name = "Gíá cả")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải là số nguyên không âm.")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }



        [Display(Name = "Ngày nhập kho")]
        public DateTime EntryDate { get; set; }

        [ForeignKey("Supplier")]
        [Display(Name = "Nhà cung cấp")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [Display(Name = "Kho")]
        [ForeignKey("Warehouse")]
        public int WarehouseID { get; set; }
        public Warehouse? Warehouse { get; set; }
    }

}

