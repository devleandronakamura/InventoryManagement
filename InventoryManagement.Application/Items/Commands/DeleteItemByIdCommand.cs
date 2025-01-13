using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Items.Commands;

public class DeleteItemByIdCommand(Guid id) : IRequest<ResultResponse<Item>>
{
    public Guid Id = id;
}