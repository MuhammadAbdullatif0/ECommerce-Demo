using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

using MU.APi.RequestHelper;

namespace MU.APi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);
        var products = await repo.ListAsync(spec);
        var count = await repo.CountAsync(spec);
        var paging = new Pagination<Product>(specParams.PageIndex, specParams.PageSize , count , products);
        return Ok(paging);
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
        repo.Add(product);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct),new {id = product.Id}, product);
        }
        return BadRequest("The is a problem with DB");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id <= 0 || !repo.Exists(id)) return BadRequest("Invalid Product");

        repo.Update(product);
        if (await repo.SaveAllAsync())
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

        repo.Remove(product);
        if (await repo.SaveAllAsync())
        {
           return NoContent();
        }
        return BadRequest("The is a problem with DB");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        var brands = await repo.ListAsync<string>(spec);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await repo.ListAsync<string>(spec);
        return Ok(types);
    }
}

