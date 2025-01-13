using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Items.Queries;

public class GetAllItemQuery : IRequest<ResultResponse<IEnumerable<Item>>>;