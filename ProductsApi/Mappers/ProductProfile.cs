using AutoMapper;
using ProductsApi.DTO;
using ProductsApi.Models;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductDTO, Product>();
    }
}
