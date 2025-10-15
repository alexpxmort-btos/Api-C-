using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTO;
using ProductsApi.Models;
using ProductsApi.Repositories;

namespace ProductsApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;


        public ProductsController(IProductRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET /api/products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _repository.GetAllAsync();
            return Ok(new
            {
                success = true,
                data = products
            });
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Produto com id {id} não encontrado."
                });

            return Ok(new
            {
                success = true,
                data = product
            });
        }

        // POST /api/products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO productDTO)
        {
          if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            var product = _mapper.Map<Product>(productDTO);

            // Adiciona no repositório
            var created = await _repository.AddAsync(product);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, new
            {
                success = true,
                data = created
            });
        }

        // PUT /api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDTO productDto)
        {

               if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
      

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Produto com id {id} não encontrado."
                });

           
           _mapper.Map(productDto, existing);

            var updated = await _repository.UpdateAsync(existing);
            return Ok(new
            {
                success = true,
                data = updated
            });
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Produto com id {id} não encontrado."
                });

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
