namespace InventoryManagement.Application.DTOs.Responses;

public class ReportResponse
{
    public string Product { get; set; } = null!;
    public decimal AveragePrice { get; set; }
    public int Quantity { get; set; }
}