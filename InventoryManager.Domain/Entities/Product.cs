using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string NameOfProduct { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    //public int CategoryId { get; set; }

    // public Category Category { get; set; }




}