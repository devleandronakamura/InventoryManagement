namespace InventoryManagement.Application.DTOs.Requests;

public class RegisterConsumptionRequest
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}