using InventoryManagement.Application.DTOs.Base;

namespace InventoryManagement.Application.DTOs.Responses;

public class ItemResponse : ItemBaseDto
{
    public Guid Id { get; set; }
}