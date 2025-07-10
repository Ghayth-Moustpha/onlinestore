using Microsoft.AspNetCore.Mvc;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductDbContext _context;

    public ProductController(ProductDbContext context) => _context = context;

    [HttpGet]
    public IActionResult GetAll() => Ok(_context.Products.ToList());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _context.Products.Find(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Product p)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();
        product.Name = p.Name;
        product.Description = p.Description;
        product.Price = p.Price;
        _context.SaveChanges();
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();
        _context.Products.Remove(product);
        _context.SaveChanges();
        return NoContent();
    }
}
