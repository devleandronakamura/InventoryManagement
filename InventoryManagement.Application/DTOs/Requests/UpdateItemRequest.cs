using InventoryManagement.Application.DTOs.Base;

namespace InventoryManagement.Application.DTOs.Requests;

public class UpdateItemRequest : ItemBaseDto
{
    public Guid Id { get; set; }
}