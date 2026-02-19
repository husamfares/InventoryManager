
using InventoryManager.Application.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]

public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken ct)
    {
        var products = await _productRepository.GetAllAsync(ct);
        return Ok(products);
    }



    [HttpGet("{id:guid}")]//defines a route parameter called id. ASP.NET model binding extracts the value from the URL, validates it as a GUID, and injects it into the method parameter.
    public async Task<IActionResult> GetById(Guid id , CancellationToken ct)
    {
        //Console.WriteLine($"GetById endpoint hit. id={id}");
        var product = await _productRepository.GetByIdAsync(id , ct);
        return product is null ? NotFound() : Ok(product);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]CreateProductRequest request , CancellationToken ct)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var newId = await _productRepository.CreateAsync(product , ct);

        return CreatedAtAction //return 201 Created: A new resource was successfully created
        (
            nameof(GetById),
            new {id = newId},
            new {id = newId}
        );

    }


    [HttpPut ("{id:guid}")]
    public async Task<IActionResult> UpdateStockAsync(Guid id ,[FromBody] UpdateStockRequest request , CancellationToken ct)
    {
        var update = await _productRepository.UpdateStockAsync(id , request.StockQuantity , ct);
        return update ? NoContent() : NotFound();
    }
    
}