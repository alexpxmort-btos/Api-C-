using Xunit;
using Moq;
using ProductsApi.Repositories;
using ProductsApi.Controllers;
using ProductsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProductsApi.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async void Get_ReturnsAllProducts()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Product> { new Product { Id = 1, Name = "Teste" } });

            var controller = new ProductsController(mockRepo.Object);
            var result = await controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(products);
        }
    }
}
