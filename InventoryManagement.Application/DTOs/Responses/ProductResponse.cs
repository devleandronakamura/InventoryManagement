using InventoryManagement.Application.DTOs.Base;

namespace InventoryManagement.Application.DTOs.Responses;

public class ProductResponse : ItemBaseDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string PartNumber { get; set; } = null!;
    public decimal Price { get; set; }
    public bool Available { get; set; }
}