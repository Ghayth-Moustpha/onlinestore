using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using ProductService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add DB
builder.Services.AddDbContext<PaymentDbContext>(opt =>
    opt.UseSqlite("Data Source=payments.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure DB created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
