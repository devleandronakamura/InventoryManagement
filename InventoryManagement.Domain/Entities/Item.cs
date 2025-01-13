namespace InventoryManagement.Domain.Entities;

public class Item : EntityBase
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
}