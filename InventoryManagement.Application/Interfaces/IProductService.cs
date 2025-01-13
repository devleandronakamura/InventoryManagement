using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.DTOs.Responses;

namespace InventoryManagement.Application.Interfaces;

public interface IProductService
{
    Task<ResultResponse<IEnumerable<ProductResponse>>> GetAllAsync();
    Task<ResultResponse<ProductResponse>> GetById(Guid id);
    Task<ResultResponse<ProductResponse>> AddAsync(CreateProductRequest request);
    Task<ResultResponse<ProductResponse>> UpdateAsync(UpdateProductRequest request);
    Task<ResultResponse<ProductResponse>> DeleteByIdAsync(Guid id);
}