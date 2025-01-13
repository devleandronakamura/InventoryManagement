namespace InventoryManagement.Application.DTOs.Requests;

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string PartNumber { get; set; } = null!;
    public decimal Price { get; set; }
}