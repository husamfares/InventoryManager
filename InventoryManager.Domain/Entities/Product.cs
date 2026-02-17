using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity {get; set;}

    public DateTimeOffset CreatedAt {get; set;}

    public DateTimeOffset UpdatedAt {get; set;}




}