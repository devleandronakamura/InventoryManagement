using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.Services;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Infra.IoC.DependencyInjection;

public static class ApiDI
{
    public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services)
    {
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IConsumptionService, ConsumptionService>();

        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IConsumptionRepository, ConsumptionRepository>();

        return services;
    }
}