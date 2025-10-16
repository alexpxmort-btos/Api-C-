using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTO;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.Controllers
{
[ApiController]
[Route("api/products")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly IMapper _mapper;

    public ProductsController(IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseSuccessDTO<List<Product>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var products = await _service.GetAllAsync();
        return Ok(new ApiResponseSuccessDTO<List<Product>>(products));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseSuccessDTO<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponseErrorDTO), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
            return NotFound(new ApiResponseErrorDTO( "Produto com id {id} não encontrado."));

        return Ok(new ApiResponseSuccessDTO<Product>(product));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseSuccessDTO<Product>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponseErrorValidationDTO), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(errors);
        }

        var product = _mapper.Map<Product>(productDTO);
        var created = await _service.AddAsync(product);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, new ApiResponseSuccessDTO<Product>(created));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseSuccessDTO<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponseErrorValidationDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseErrorDTO), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] ProductDTO productDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(errors);
        }

        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound(new ApiResponseErrorDTO( $"Produto com id {id} não encontrado."));

        _mapper.Map(productDto, existing);
        var updated = await _service.UpdateAsync(existing);
        return Ok(new ApiResponseSuccessDTO<Product>(updated));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponseSuccessDTO<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound(new ApiResponseErrorDTO("Produto com id {id} não encontrado." ));

        await _service.DeleteAsync(id);
        return NoContent();
    }
}

}
