using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    Task<Item?> GetByNameAsync(string name);
    Task<Item?> GetByIdAndNameAsync(Guid id, string name);
}