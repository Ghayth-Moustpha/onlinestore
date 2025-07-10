using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderDbContext _context;

    public OrderController(OrderDbContext context) => _context = context;

    [HttpGet]
    public IActionResult GetAll() => Ok(_context.Orders.ToList());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var order = _context.Orders.Find(id);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public IActionResult Create(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Order o)
    {
        var order = _context.Orders.Find(id);
        if (order == null) return NotFound();
        order.ProductId = o.ProductId;
        order.UserId = o.UserId;
        order.Quantity = o.Quantity;
        _context.SaveChanges();
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null) return NotFound();
        _context.Orders.Remove(order);
        _context.SaveChanges();
        return NoContent();
    }
}
