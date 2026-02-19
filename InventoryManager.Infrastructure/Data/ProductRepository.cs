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


    public async Task<Guid> CreateAsync(Product product, CancellationToken ct = default)
    {
        const string sql = @"
        Insert into products (name , price, stock_quantity)
        values (@name , @price , @stock_quantity)
        RETURNING id;
        ";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(ct);

        await using var command = new NpgsqlCommand(sql , conn);
        command.Parameters.AddWithValue("name",product.Name);
        command.Parameters.AddWithValue("price",product.Price);
        command.Parameters.AddWithValue("stock_quantity",product.StockQuantity);

        var NewIdObj = await command.ExecuteScalarAsync(ct); 

        return (Guid)NewIdObj!; 

    }



    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        const string sql = @"Select id, name, price, stock_quantity, created_at, updated_at
                            From Products
                            ORDER By Created_at DESC;";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(ct); 

        await using var command = new NpgsqlCommand(sql , conn);

        await using var reader = await command.ExecuteReaderAsync(ct); 


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


    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        const string sql = @"
            Select id, name, price ,stock_quantity, created_at, updated_at
            From products 
            where id = @id 
            LIMIT 1;
            ";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(ct);

        await using var command = new NpgsqlCommand(sql, conn);
        command.Parameters.AddWithValue("id", id); 

        await using var reader = await command.ExecuteReaderAsync(ct);

        if(!await reader.ReadAsync(ct))
            return null;

            return new Product 
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                StockQuantity = reader.GetInt32(3),
                CreatedAt = reader.GetFieldValue<DateTimeOffset>(4),
                UpdatedAt = reader.GetFieldValue<DateTimeOffset>(5)
            };
        
    }

    public async Task<bool> UpdateStockAsync(Guid id, int newStockQuantity, CancellationToken ct = default)
    {
        const string sql = @"
        Update products
        set stock_quantity = @stock_quantity, updated_at = NOW()
        where id = @id
        And @stock_quantity>=0;
        ";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(ct);

        await using var command = new NpgsqlCommand(sql , conn);
        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("stock_quantity",newStockQuantity);

        var affected = await command.ExecuteNonQueryAsync(ct);

        return affected == 1;
    }
}