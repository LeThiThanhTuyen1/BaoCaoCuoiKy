using System;
using System.ComponentModel.DataAnnotations;

public class Manager
{
    public int ManagerId { get; set; }

    [Required]
    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Contact { get; set; }

    public string? ManagedWarehouse { get; set; } // Kho mà người quản lý chịu trách nhiệm
}
