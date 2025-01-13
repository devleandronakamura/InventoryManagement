using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Items.Queries;

internal class GetItemByIdQuery(Guid id) : IRequest<ResultResponse<Item>>
{
    public Guid Id = id;
}