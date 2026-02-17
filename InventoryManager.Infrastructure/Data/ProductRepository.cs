namespace InventoryManager.Infrastructure.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;
using InventoryManager.Domain.Entities;
using InventoryManager.Application.Interfaces;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection is not configured.");
    }


    public Task<Guid> CreateAsync(Product product, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        const string sql = @"Select id, name, price, stock_quantity, created_at, updated_at
                            From Products
                            ORDER By Created_at DESC;";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(ct); // re explain for what this CancellationToken process

        await using var command = new NpgsqlCommand(sql , conn);

        await using var reader = await command.ExecuteReaderAsync(ct); // and explain the await and async and using concept


        var result = new List<Product>();
        while (await reader.ReadAsync(ct))
        {
            result.Add(new Product
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                StockQuantity = reader.GetInt32(3),
                CreatedAt = reader.GetFieldValue<DateTimeOffset>(4),
                UpdatedAt = reader.GetFieldValue<DateTimeOffset>(5)
                
            });
        }

        return result;


    }


    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateStockAsync(Guid id, int newStockQuantity, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}