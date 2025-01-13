namespace InventoryManagement.Domain.Entities;

public abstract class EntityBase
{
    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        Deleted = false;
    }

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.Now;
    }

    public void SetDeleted()
    {
        UpdatedAt = DateTime.Now;
        Deleted = true;
    }

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool Deleted { get; private set; }
}