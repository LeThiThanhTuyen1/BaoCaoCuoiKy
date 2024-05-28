
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required]
        [Display(Name = "Tên tài khoản")]
        public string? Username { get; set; }

        [Required]
        [Display(Name = "Mật Khẩu")]
        public string? Password { get; set; }

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
        [Display(Name = "Quản Lý")]
        public Manager? Manager { get; set; }

        [Display(Name = "Vai Trò")]
        public string? Role { get; set; } // Admin or Manager
    }
}
