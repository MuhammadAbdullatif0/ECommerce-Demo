using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MU.APi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductRepository repo) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand , string? type , string? sort)
    {
        var products = await repo.GetAllAsync(brand , type , sort);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        if (id <= 0) return BadRequest("Invalid ID");
        var product = await repo.GetByIdAsync(id);
        if (product is null) return NotFound("Not Found Product");
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        if (product == null) return NotFound("Product Not Found");
        repo.CreateProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction(nameof(GetProduct),new {id = product.Id}, product);
        }
        return BadRequest("The is a problem with DB");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id <= 0 || !repo.ProductExists(id)) return BadRequest("Invalid Product");

        repo.UpdateProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("The is a problem with DB");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        if (id <= 0) return BadRequest("Invalid ID");
        var product = await repo.GetByIdAsync(id);
        if (product is null) return NotFound("Not Found Product");

        repo.DeleteProduct(product);
        if (await repo.SaveChangesAsync())
        {
           return NoContent();
        }
        return BadRequest("The is a problem with DB");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }
}

