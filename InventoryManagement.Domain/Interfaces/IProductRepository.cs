using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByPartNumberAsync(string partNumber);
    Task<Product?> GetByIdAndPartNumberAsync(Guid id, string partNumber);
    Task<IEnumerable<Product>> GetByIdAndQuantityAsync(Guid id, int quantity);
    Task RegisterByConsumptionIdAndProductsIdsAsync(Guid consumptionId, List<Guid> productsIds);
}