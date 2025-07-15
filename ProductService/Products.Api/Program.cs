using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Products.Api.Extensions;
using Products.Api.Middlewares;
using Products.Application.Interfaces;
using Products.Application.Services;
using Products.Application.Validators;
using Products.Domain.Interfaces;
using Products.Infrastructure.Data;
using Products.Infrastructure.Repositories;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

//read path from appsettings.json

var filePath = builder.Configuration["Serilog:WriteTo:1:Args:path"];

//Configure Serilog to read from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();   //Plug Serilog into the app host

// Add services to the container.
var defaultConnection = builder.Configuration.GetConnectionString("ProductsDb");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(defaultConnection));

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<ProductCreateRequestDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();
builder.Services.AddCustomCorsPolicies();
builder.Services.AddCustomRateLiming(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>(name: "ef-dbcontext", tags: new[] { "db", "ef" });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
app.MapHealthChecks("/health");
app.UseHttpsRedirection();

// Register CORS before authentication/authorization
app.UseCors("AllowSpecificOrigin");
app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }