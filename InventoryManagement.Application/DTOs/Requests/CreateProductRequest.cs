namespace InventoryManagement.Application.DTOs.Requests;

public class CreateProductRequest
{
    public Guid ItemId { get; set; }
    public string PartNumber { get; set; } = null!;
    public decimal Price { get; set; }
}