using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Products.Queries;

public class GetProductByIdQuery(Guid id) : IRequest<ResultResponse<Product>>
{
    public Guid Id = id;
}