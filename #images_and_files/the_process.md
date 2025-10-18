# web-service development midterm task

## in here i show my process/progress in the making of this project and fulfilling its requirements

Table of contents
- [Requirements](#requirements)
- [Process](process)

## Requirements
i was given a list of requirements to make the project by following it so i can pass my midterm  
the list of requirements are:
![requirements](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/requirements.png)  
and following that list i have fulfilled the needed job to do:
1. Implementation of CRUD operations using the REST architectural style  
1.1. Implementation of validation (DataAnnotation, FluentAPI) - ✅  
2. Implementation of Dependency Injection  
2.1. Logging (Seq, Serilog) - ✅  
2.2. Repository Pattern - ✅  
3. API testing  
3.1. Using HTTPClient (or its analogues) in the WEB part (possibly ASP.NET Core or other solutions) - ✅  
3.2. Using Postman or other analogues - ✅  
4. Implementation of API authorization (JWT or other options) - ✅  
5. Implementation of Authentication and Authorization in an application using Identity - ✅  

## Process

### 1. Implementation of CRUD operations using the REST architectural style  

The API implements full CRUD (Create, Read, Update, Delete) functionality for the Customer resource, following REST conventions.  
Each operation is exposed as a clear, predictable HTTP endpoint under /api/Auth/customers.

| Operation | HTTP Method | Endpoint                   | Description                 | Access Control     |
| --------- | ----------- | -------------------------- | --------------------------- | ------------------ |
| Create    | `POST`      | `/api/Auth/register`       | Register a new customer     | Public (Anonymous) |
| Read All  | `GET`       | `/api/Auth/customers`      | Get a list of all customers | Admin only         |
| Read One  | `GET`       | `/api/Auth/customers/{id}` | Get customer by ID          | Admin or Self      |
| Update    | `PUT`       | `/api/Auth/customers/{id}` | Update customer info        | Admin or Self      |
| Delete    | `DELETE`    | `/api/Auth/customers/{id}` | Delete customer             | Admin only         |  

This controller exposes all CRUD operations through REST endpoints:  
[Create - AuthController.cs](https://github.com/K0d0ku/web-serv_midterm/blob/74ce5e44ff67db98236399b36b331dac2b20ce4a/Controllers/AuthController.cs#L28-L46)  
```
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
```
![Crud - Create](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud1.png)  
![Crud - Create](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrudDb1.png)  
> its accNUpdated after the Update and i didnt have the picture before the update

[Read (all) - AuthController.cs](https://github.com/K0d0ku/web-serv_midterm/blob/74ce5e44ff67db98236399b36b331dac2b20ce4a/Controllers/AuthController.cs#L89-L97)
```
        // R
        [HttpGet("customers")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllCustomers()
        {
            _logger.LogInformation("Fetching all customers");
            var customers = _repository.GetAll();
            return Ok(customers);
        }
```
![Crud - Read(All)](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud2.png)  

[Read (ID) - AuthController.cs](https://github.com/K0d0ku/web-serv_midterm/blob/74ce5e44ff67db98236399b36b331dac2b20ce4a/Controllers/AuthController.cs#L99-L113)  
```
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
```
![Crud - Read(Id)](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud2ByID.png)  

[Update - AuthController.cs](https://github.com/K0d0ku/web-serv_midterm/blob/74ce5e44ff67db98236399b36b331dac2b20ce4a/Controllers/AuthController.cs#L115-L136)  
```
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
```
**Before**  
![Crud - Update](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud3before.png)  
![Crud - Update](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrudDb3Before.png)  
**After**  
![Crud - Update](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud3After.png)  
![Crud - Update](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrudDb3After.png)  

[Delete - AuthController.cs](https://github.com/K0d0ku/web-serv_midterm/blob/74ce5e44ff67db98236399b36b331dac2b20ce4a/Controllers/AuthController.cs#L138-L151)  
```
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
```
**Created Dummy account**  
![Crud - Delete](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud4Dum.png)  
![Crud - Delete](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud4DumDB.png)  
**to delete another account**  
![Crud - Delete](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud4Del.png)  
![Crud - Delete](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud4ChkDB.png)  
![Crud - Delete](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/ApiTestCrud4Chk.png)  
  
#### REST principles applied:
 - Each method corresponds directly to a standard HTTP verb.  
 - URLs are resource-based (/api/Auth/customers).  
 - No unnecessary verbs in URL names.  
 - Proper use of status codes (200, 401, 403, 404, 400).  
 - Role-based access for security.  
  
### 1.1. Implementation of validation (DataAnnotation, FluentAPI)  
The project uses DataAnnotation attributes directly on the [Customer.cs](https://github.com/K0d0ku/web-serv_midterm/blob/master/Models/Customer.cs) model to enforce validation rules at the model level.  
This ensures invalid data is rejected automatically before business logic executes.  
[Customer.cs](https://github.com/K0d0ku/web-serv_midterm/blob/c67fe31b22bd8da1505ffbeda6b870939c785728/Models/Customer.cs#L1-L24)  
```
﻿using System.ComponentModel.DataAnnotations;

namespace KuroApi.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nickname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string Role { get; set; } = "Customer";
    }
}
```
#### Validation rules in place:  
 - Nickname → required, max length 50  
 - Email → required, must be a valid email  
 - Password → required, must be at least 6 characters  
 - Id → marked as primary key

This validation is automatically enforced by the ASP.NET Core model binder —  
if the payload doesn’t match the validation rules, the controller returns 400 Bad Request without entering the repository layer.  
  
### 2. Implementation of Dependency Injection  
 - Dependency Injection (DI): [Program.cs](https://github.com/K0d0ku/web-serv_midterm/blob/master/Program.cs) registers [AppDbContext](https://github.com/K0d0ku/web-serv_midterm/blob/master/Data/AppDbContext.cs), [ICustomerRepository](https://github.com/K0d0ku/web-serv_midterm/blob/master/Repositories/ICustomerRepository.cs) → [CustomerRepository](https://github.com/K0d0ku/web-serv_midterm/blob/master/Repositories/CustomerRepository.cs), HttpClient, Identity, Authentication & Authorization. Controllers receive services through constructor injection ([AuthController](https://github.com/K0d0ku/web-serv_midterm/blob/master/Controllers/AuthController.cs)).  
 - Logging (Serilog): configured at app startup in [Program.cs](https://github.com/K0d0ku/web-serv_midterm/blob/master/Program.cs) (console + rolling file sink). Controllers use injected ILogger<T> to write structured logs.  
 - Repository Pattern: [ICustomerRepository](https://github.com/K0d0ku/web-serv_midterm/blob/master/Repositories/ICustomerRepository.cs) (interface) and [CustomerRepository](https://github.com/K0d0ku/web-serv_midterm/blob/master/Repositories/CustomerRepository.cs) (implementation) encapsulate all EF Core DB access. Controllers call repository methods instead of talking directly to [AppDbContext](https://github.com/K0d0ku/web-serv_midterm/blob/master/Data/AppDbContext.cs).

[AppDbContext.cs](https://github.com/K0d0ku/web-serv_midterm/blob/master/Data/AppDbContext.cs) — EF Core context (where models are registered)  
```
﻿using Microsoft.EntityFrameworkCore;
using KuroApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KuroApi.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
    }
}
```
Configurations in [appsettings.json](https://github.com/K0d0ku/web-serv_midterm/blob/c67fe31b22bd8da1505ffbeda6b870939c785728/appsettings.json#L2-L4) relevant to DI / logging / repository  
```
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=KuroApiDb;Username=postgres;Password=chillen45inda3"
  },
```  

#### Dependency Injection  
[Program.cs](https://github.com/K0d0ku/web-serv_midterm/blob/master/Program.cs)
```
// db context its postgres btw
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// dependency injection
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// HttpClient factory
builder.Services.AddHttpClient();

// identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
```
How controllers consume services (example from [AuthController]((https://github.com/K0d0ku/web-serv_midterm/blob/c67fe31b22bd8da1505ffbeda6b870939c785728/Controllers/AuthController.cs#L15-L26))):  
```
public class AuthController : ControllerBase
{
    private readonly ICustomerRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ICustomerRepository repository,
                          IConfiguration configuration,
                          ILogger<AuthController> logger)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
    }

    // ... controller actions use _repository and _logger
    // rest of the code 
}
```  
  
### 2.1. Logging (Seq, Serilog)  
[Program.cs](https://github.com/K0d0ku/web-serv_midterm/blob/c67fe31b22bd8da1505ffbeda6b870939c785728/Program.cs#L20-L27)  
```
// log using serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
```
 - Creates a global Serilog logger and instructs the host to use Serilog as the logging provider.  
 - Two sinks are configured: console output and daily rolling file logs/log-YYYYMMDD.txt.

How controllers write logs (example snippets from [AuthController](https://github.com/K0d0ku/web-serv_midterm/blob/master/Controllers/AuthController.cs):  
```
_logger.LogInformation("Customer registered successfully: {CustomerId}", customer.Id);
_logger.LogInformation("Login successful for email {Email}", login.Email);
_logger.LogInformation("Fetching all customers");
_logger.LogInformation("Customer with ID {CustomerId} updated successfully", id);
_logger.LogInformation("Customer with ID {CustomerId} deleted successfully", id);
```  
  
### 2.2. Repository Pattern  
Interface: [ICustomerRepository](https://github.com/K0d0ku/web-serv_midterm/blob/master/Repositories/ICustomerRepository.cs)  
```
﻿using KuroApi.Models;
using System.Collections.Generic;

namespace KuroApi.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer GetById(int id);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        bool ExistsByEmail(string email);
    }
}
```
Implementation: [CustomerRepository](https://github.com/K0d0ku/web-serv_midterm/blob/master/Repositories/CustomerRepository.cs)
```
using KuroApi.Data;
using KuroApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace KuroApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAll() => _context.Customers.ToList();

        public Customer GetById(int id) => _context.Customers.Find(id);

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public bool ExistsByEmail(string email) => _context.Customers.Any(c => c.Email == email);
    }
}
```  
What it does is, it keeps the data access separate from controllers and business logic.  
Example usage in [AuthController](https://github.com/K0d0ku/web-serv_midterm/blob/master/Controllers/AuthController.cs)  
```
// check existence
if (_repository.ExistsByEmail(customer.Email))
    return BadRequest(...);

// create
_repository.Add(customer);

// read
var list = _repository.GetAll();

// update
_repository.Update(customer);

// delete
_repository.Delete(id);
```
Also to separate business logic from data access, the API uses the Repository pattern too.  
All database operations for Customer are centralized in:  
[CustomerRepository](https://github.com/K0d0ku/web-serv_midterm/blob/master/Repositories/CustomerRepository.cs)  
```
public IEnumerable<Customer> GetAll() => _context.Customers.ToList();
public Customer GetById(int id) => _context.Customers.Find(id);
public void Add(Customer customer) { ... }
public void Update(Customer customer) { ... }
public void Delete(int id) { ... }
```  
  
### 3. API testing  
### 3.1. Using HTTPClient (or its analogues) in the WEB part (possibly ASP.NET Core or other solutions) - ✅  
### 3.2. Using Postman or other analogues - ✅  

### 4. Implementation of API authorization (JWT or other options) - ✅  

### 5. Implementation of Authentication and Authorization in an application using Identity - ✅  
