namespace InventoryManagement.Application.DTOs.Responses;

public class ConsumptionResponse
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public int Quantity { get; set; }
}