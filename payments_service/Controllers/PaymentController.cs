using Microsoft.AspNetCore.Mvc;
using PaymentService.Data;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentDbContext _context;

    public PaymentController(PaymentDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_context.Payments.ToList());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var payment = _context.Payments.Find(id);
        return payment == null ? NotFound() : Ok(payment);
    }

    [HttpPost]
    public IActionResult Create(Payment payment)
    {
        _context.Payments.Add(payment);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Payment p)
    {
        var payment = _context.Payments.Find(id);
        if (payment == null) return NotFound();

        payment.Amount = p.Amount;
        payment.Status = p.Status;
        _context.SaveChanges();
        return Ok(payment);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var payment = _context.Payments.Find(id);
        if (payment == null) return NotFound();

        _context.Payments.Remove(payment);
        _context.SaveChanges();
        return NoContent();
    }
}
