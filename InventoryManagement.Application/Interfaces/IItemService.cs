using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.DTOs.Responses;

namespace InventoryManagement.Application.Interfaces;

public interface IItemService
{
    Task<ResultResponse<IEnumerable<ItemResponse>>> GetAllAsync();
    Task<ResultResponse<ItemResponse>> GetById(Guid id);
    Task<ResultResponse<ItemResponse>> AddAsync(CreateItemRequest request);
    Task<ResultResponse<ItemResponse>> UpdateAsync(UpdateItemRequest request);
    Task<ResultResponse<ItemResponse>> DeleteByIdAsync(Guid id);
}