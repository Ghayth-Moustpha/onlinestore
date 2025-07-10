using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Data;
using UserService.Models;
using UsersService.Models;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _config;

    public UserController(UserDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return Ok(user);
    }

    [HttpPost("login")]
    public IActionResult Login(User credentials)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Username == credentials.Username && u.Password == credentials.Password);

        if (user == null) return Unauthorized("Invalid credentials");

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(_context.Users.ToList());
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [Authorize]
    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, User updatedUser)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        user.Username = updatedUser.Username;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;

        _context.SaveChanges();
        return Ok(user);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        _context.SaveChanges();
        return NoContent();
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes("your_secret_key_123456");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
