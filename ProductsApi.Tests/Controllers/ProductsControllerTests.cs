using Xunit;
using Moq;
using ProductsApi.Services;
using ProductsApi.Controllers;
using ProductsApi.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace ProductsApi.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task Get_ReturnsSuccessTrueAndProductList()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Teste" }
            };

            mockService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(products);

            var controller = new ProductsController(mockService.Object, mockMapper.Object);

            var result = await controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var json = JsonSerializer.Serialize(okResult.Value);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Assert.True(root.GetProperty("success").GetBoolean());

            var data = root.GetProperty("data").Deserialize<List<Product>>();
            Assert.NotNull(data);
            Assert.Single(data);
            Assert.Equal("Teste", data[0].Name);
        }
    }
}
