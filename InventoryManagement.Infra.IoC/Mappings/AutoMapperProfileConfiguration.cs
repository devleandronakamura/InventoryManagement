using AutoMapper;
using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.DTOs.Responses;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Infra.IoC.Mappings;

public class AutoMapperProfileConfiguration : Profile
{
    public AutoMapperProfileConfiguration()
    {
        CreateMap<Item, CreateItemRequest>().ReverseMap();
        CreateMap<Item, UpdateItemRequest>().ReverseMap();
        CreateMap<Item, ItemResponse>().ReverseMap();
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Item.Description))
            .ReverseMap();

        CreateMap<Product, CreateProductRequest>().ReverseMap();
        CreateMap<Product, UpdateProductRequest>().ReverseMap();

        CreateMap<Consumption, RegisterConsumptionRequest>().ReverseMap();
        CreateMap<Consumption, ConsumptionResponse>().ReverseMap();
    }
}