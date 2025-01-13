namespace InventoryManagement.Domain.Entities;

public class Product : EntityBase
{
    public Product() : base()
    {
        Available = true;
    }

    public Item? Item { get; set; }
    public Guid ItemId { get; private set; }
    public Guid? ConsumptionId { get; private set; }
    public string PartNumber { get; private set; } = null!;
    public decimal Price { get; private set; }
    public bool Available { get; private set; }
}