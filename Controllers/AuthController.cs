using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LiveProject_WebAPI_Demo.Data;
using LiveProject_WebAPI_Demo.DTOs;
using LiveProject_WebAPI_Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LiveProject_WebAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);


            if (existingUser != null)
            {
                return BadRequest("Users already exists!!");
            }


            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };


            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return Ok("Registration Successful!!");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email
                                                                     && x.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid Credentials!!");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);


            return Ok(new
            {
                message = "Login Successful!!",
                token = jwtToken
            });

        }
    }
}
