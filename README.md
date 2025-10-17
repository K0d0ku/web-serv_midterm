# web-service development midterm task
repo created for my midterm for my subject in - Web services development

it is unknown if the project will be continued or abandoned if there wont be any further tasks after midterm but either way will be archived after the semester 

the process

the postman test data

>[The Postman Test Data]([https://raw.githubusercontent.com/user/repo/branch/file.txt](https://k0d0ku-9187943.postman.co/workspace/15c82ff0-6925-4f2d-8a93-bf09e1252abf/collection/49340916-3aa8bd97-f8b9-4b61-b1a8-19293639d6c3?action=share&source=copy-link&creator=49340916))


the project is made with the following requirements:

and used the following tools and packages:

- net9.0
- PostGres17, Pgadmin, DataGrip(optional)
- Postman

NuGet packages used:
1. Core & EF Core / Database

| Package                                             | Purpose                                    |
| --------------------------------------------------- | ------------------------------------------ |
| `Microsoft.EntityFrameworkCore`                     | Base EF Core functionality                 |
| `Microsoft.EntityFrameworkCore.Design`              | Required for migrations and tooling        |
| `Npgsql.EntityFrameworkCore.PostgreSQL`             | EF Core provider for PostgreSQL            |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | ASP.NET Core Identity support with EF Core |
| `Microsoft.AspNetCore.Identity`                     | Core Identity functionality                |


2. Authentication / JWT

| Package                                         | Purpose                        |
| ----------------------------------------------- | ------------------------------ |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | JWT authentication middleware  |
| `System.IdentityModel.Tokens.Jwt`               | Create and validate JWT tokens |


3. Logging

| Package                          | Purpose                                  |
| -------------------------------- | ---------------------------------------- |
| `Serilog.AspNetCore`             | ASP.NET Core integration for Serilog     |
| `Serilog.Sinks.Console`          | Log output to console                    |
| `Serilog.Sinks.File`             | Log output to rolling files              |
| `Serilog.Sinks.Seq`              | Log output to Seq for structured logging |


4. HTTP / Web API Helpers

| Package                                                         | Purpose                                                      |
| --------------------------------------------------------------- | ------------------------------------------------------------ |
| `Microsoft.AspNetCore.Mvc.NewtonsoftJson`                       | JSON serialization for API endpoints                         |
| `Microsoft.Extensions.Http`                                     | Provides IHttpClientFactory support (used for HttpClient DI) |


5. Tooling / Development

| Package                                     | Purpose                                              |
| ------------------------------------------- | ---------------------------------------------------- |
| `dotnet-ef` (CLI tool, not a NuGet package) | Required for migrations (`dotnet ef migrations add`) |
