using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Products.Queries;

public class GetAllProductQuery : IRequest<ResultResponse<IEnumerable<Product>>>;