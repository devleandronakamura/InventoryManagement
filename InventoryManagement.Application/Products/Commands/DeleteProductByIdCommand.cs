using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Products.Commands;

public class DeleteProductByIdCommand(Guid id) : IRequest<ResultResponse<Product>>
{
    public Guid Id = id;
}