using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Products.Commands;

public class UpdateProductCommand(Product product) : IRequest<ResultResponse<Product>>
{
    public Product Product = product;
}