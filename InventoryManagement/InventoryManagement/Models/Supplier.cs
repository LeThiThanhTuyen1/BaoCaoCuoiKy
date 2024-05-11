using System;
using System.ComponentModel.DataAnnotations;
public class Supplier
{
    public int SupplierId { get; set; }

    [Required]
    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Contact { get; set; }
}
