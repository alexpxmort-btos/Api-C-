using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Adiciona DbContext em memória
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

// Registra Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();



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
            .Where(e => e.Value.Errors.Count > 0)
            .ToDictionary(
                e => e.Key.ToLower(), // <-- lowercase
                e => e.Value.Errors.Select(err => err.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new { errors });
    };
});

builder.Services.AddAutoMapper(typeof(ProductProfile));

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1");
});


// app.UseHttpsRedirection(); // opcional
app.UseCors("AllowAngular");
app.MapControllers();

app.Run();
