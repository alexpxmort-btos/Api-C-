using ProductsApi.Models;
using ProductsApi.Repositories;

namespace ProductsApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
             _repository = repository;
        }

        public async Task<List<Product>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Product?> GetByIdAsync(int id) =>
            await _repository.GetByIdAsync(id);

        public async Task<Product> AddAsync(Product product)
        {
            
            return await _repository.AddAsync(product);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            return await _repository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
           return await _repository.DeleteAsync(id);
        }
    }
}
