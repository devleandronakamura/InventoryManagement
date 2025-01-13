using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Items.Commands;

public class UpdateItemCommand(Item item) : IRequest<ResultResponse<Item>>
{
    public Item Item = item;
}