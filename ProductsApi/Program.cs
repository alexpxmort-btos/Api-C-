using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Repositories;
using ProductsApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IProductService, ProductService>();



// Adiciona Controllers
builder.Services.AddControllers();

// Habilita Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilita CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value.Errors.Select(err => new 
            { 
                field = kvp.Key.ToLower(), 
                message = err.ErrorMessage 
            }))
            .ToList();

        return new BadRequestObjectResult(new 
        {
            errors
        });
    };
});


builder.Services.AddAutoMapper(typeof(ProductProfile));

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1");
});


app.UseCors("AllowAngular");
app.MapControllers();

app.Run();
