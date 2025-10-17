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

CRUD  
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
![Crud - Update]()  
![Crud - Update]()  
![Crud - Update]()  

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
![Crud - Delete]()  

### 1.1. Implementation of validation (DataAnnotation, FluentAPI) - ✅  

2. Implementation of Dependency Injection  
2.1. Logging (Seq, Serilog) - ✅  
2.2. Repository Pattern - ✅  
4. API testing  
3.1. Using HTTPClient (or its analogues) in the WEB part (possibly ASP.NET Core or other solutions) - ✅  
3.2. Using Postman or other analogues - ✅  
5. Implementation of API authorization (JWT or other options) - ✅  
6. Implementation of Authentication and Authorization in an application using Identity - ✅  
