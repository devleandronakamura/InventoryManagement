namespace InventoryManagement.Domain.Entities;

public class Consumption : EntityBase
{
    public Item? Item { get; private set; }
    public Guid ItemId { get; private set; }
    public Product? Product { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
}