using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interfaces;

public interface IConsumptionRepository
{
    Task<IEnumerable<(Guid Id, string Product, decimal AveragePrice, int Quantity)>> GetByDateAsync(DateTime date);
    Task<Consumption> RegisterProductsIdsAsync(Consumption consumption, List<Guid> productsIds);
}