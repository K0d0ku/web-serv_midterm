# web-service development midterm task  
## repo created for my midterm for my subject in - Web services development  
  
it is unknown if the project will be continued or abandoned if there wont be any further tasks after midterm but either way will be archived after the semester   

## the process  
you can see the process of making this project here in the : [the_process.md](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/the_process.md) `(it is mandatory to check)`  

## the project is made with the following requirements:  
![requirements](https://github.com/K0d0ku/web-serv_midterm/blob/master/%23images_and_files/requirements.png)  

the packages and the .Net version are also listed in the [KuroApi.csproj](https://github.com/K0d0ku/web-serv_midterm/blob/master/KuroApi.csproj)
```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.10" />
    <PackageReference Include="Microsoft.AspNetCore.Identity" Version="2.3.1" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.10" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.10">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.10" />
    <PackageReference Include="Microsoft.Extensions.Configuration.EnvironmentVariables" Version="9.0.10" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.10" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.4" />
    <PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
    <PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
    <PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
    <PackageReference Include="Serilog.Sinks.Seq" Version="9.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="9.0.6" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.14.0" />
  </ItemGroup>

</Project>
```

## and used the following tools and packages:

- net9.0
- PostGres17, Pgadmin, DataGrip(optional)
- Postman
  
Created a postgres Database called **KuroApiDb**

## NuGet packages used:
  
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
