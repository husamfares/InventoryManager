
using InventoryManager.Application.Interfaces;
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
    
}