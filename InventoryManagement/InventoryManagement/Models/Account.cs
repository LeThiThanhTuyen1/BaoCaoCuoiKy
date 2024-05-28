using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        public Manager? Manager { get; set; }

        public string? Role { get; set; } // Admin or Manager
    }
}
