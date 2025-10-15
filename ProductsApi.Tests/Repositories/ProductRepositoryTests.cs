using ProductsApi.Data; 
using ProductsApi.Models;
using ProductsApi.Repositories; 
using Microsoft.EntityFrameworkCore;

using Xunit;


namespace ProductsApi.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // banco único por teste
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            var product = new Product
            {
                Name = "Teste",
                Description = "Produto teste",
                Price = 10,
                Quantity = 5
            };

            var result = await repo.AddAsync(product);

            Assert.NotNull(result);
            Assert.Equal("Teste", result.Name);
            Assert.Equal(1, await context.Products.CountAsync());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            var context = GetDbContext();
            context.Products.Add(new Product { Name = "P1", Price = 1, Quantity = 1 });
            context.Products.Add(new Product { Name = "P2", Price = 2, Quantity = 2 });
            await context.SaveChangesAsync();

            var repo = new ProductRepository(context);
            var result = await repo.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            var context = GetDbContext();
            var product = new Product { Name = "P1", Price = 1, Quantity = 1 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repo = new ProductRepository(context);
            var deleted = await repo.DeleteAsync(product.Id);

            Assert.True(deleted);
            Assert.Empty(await context.Products.ToListAsync());
        }
    }
}
