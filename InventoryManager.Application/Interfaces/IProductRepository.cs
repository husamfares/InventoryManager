using InventoryManager.Domain.Entities;

namespace InventoryManager.Application.Interfaces;
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);

    Task<Product?> GetByIdAsync(Guid id , CancellationToken ct = default);

    Task<Guid> CreateAsync(Product product , CancellationToken ct = default);

    Task<bool> UpdateStockAsync(Guid id , int newStockQuantity , CancellationToken ct = default);


}