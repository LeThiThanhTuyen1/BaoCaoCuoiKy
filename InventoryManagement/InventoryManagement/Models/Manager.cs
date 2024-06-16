using System;
using System.Collections.Generic;
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

        [RegularExpression(@"^[a-zA-Z0-9àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵđÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴĐ'\-,. ]*$", ErrorMessage = "Địa chỉ chỉ được chứa chữ cái, số, dấu nháy đơn, dấu gạch ngang, dấu phẩy, dấu chấm và khoảng trắng.")]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }


        [Required(ErrorMessage = "Liên hệ không được để trống.")]
        [MinLength(10, ErrorMessage = "Liên hệ phải có ít nhất 10 ký tự.")]
        [MaxLength(20, ErrorMessage = "Liên hệ không được vượt quá 20 ký tự.")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Liên hệ chỉ được chứa số.")]
        [Display(Name = "Liên hệ")]
        public string? Contact { get; set; }

        [ForeignKey("Warehouse")]
        [Display(Name = "Kho")]
        public int WarehouseID { get; set; }

        [Display(Name = "Kho")]
        public Warehouse? Warehouse { get; set; }

        public ICollection<Account>? Account { get; set; }
    }
}
