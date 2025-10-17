using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KuroApi.Models;
using KuroApi.Repositories;

namespace KuroApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ICustomerRepository repository, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger;
        }

        // C
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_repository.ExistsByEmail(customer.Email))
                return BadRequest(new { Message = "Email already registered." });

            if (string.IsNullOrEmpty(customer.Role))
                customer.Role = "Customer"; // default for now

            _repository.Add(customer);

            _logger.LogInformation("Customer registered successfully: {CustomerId}", customer.Id);
            return Ok(new { Message = "Registration successful", CustomerId = customer.Id });
        }

        // login & JWT 
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Customer login)
        {
            var customer = _repository.GetAll()
                .FirstOrDefault(c => c.Email == login.Email && c.Password == login.Password);

            if (customer == null)
                return Unauthorized(new { Message = "Invalid credentials" });

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.Email),
                new Claim(ClaimTypes.Role, customer.Role),
                new Claim("Id", customer.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Login successful for email {Email}", login.Email);
            return Ok(new
            {
                Token = tokenString,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
                Role = customer.Role
            });
        }

        // R (admin only)
        [HttpGet("customers")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllCustomers()
        {
            _logger.LogInformation("Fetching all customers");
            var customers = _repository.GetAll();
            return Ok(customers);
        }

        // R id relevant
        [HttpGet("customers/{id}")]
        [Authorize] 
        public IActionResult GetCustomerById(int id)
        {
            var customer = _repository.GetById(id);
            if (customer == null)
                return NotFound(new { Message = "Customer not found" });

            var userId = User.FindFirst("Id")?.Value;
            if (!User.IsInRole("Admin") && userId != customer.Id.ToString())
                return Forbid();

            return Ok(customer);
        }

        // U id relevant
        [HttpPut("customers/{id}")]
        [Authorize] 
        public IActionResult UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            var customer = _repository.GetById(id);
            if (customer == null)
                return NotFound(new { Message = "Customer not found" });

            var userId = User.FindFirst("Id")?.Value;
            if (!User.IsInRole("Admin") && userId != customer.Id.ToString())
                return Forbid();

            customer.Nickname = updatedCustomer.Nickname;
            customer.Email = updatedCustomer.Email;
            customer.Password = updatedCustomer.Password;

            _repository.Update(customer);
            _logger.LogInformation("Customer with ID {CustomerId} updated successfully", id);

            return Ok(new { Message = "Customer updated successfully" });
        }

        // D id relevant
        [HttpDelete("customers/{id}")]
        [Authorize(Roles = "Admin")] 
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _repository.GetById(id);
            if (customer == null)
                return NotFound(new { Message = "Customer not found" });

            _repository.Delete(id);
            _logger.LogInformation("Customer with ID {CustomerId} deleted successfully", id);

            return Ok(new { Message = "Customer deleted successfully" });
        }
    }
}
