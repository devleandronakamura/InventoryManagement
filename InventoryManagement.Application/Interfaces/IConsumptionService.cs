using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.DTOs.Responses;

namespace InventoryManagement.Application.Interfaces;

public interface IConsumptionService
{
    Task<ResultResponse<ConsumptionResponse>> RegisterAsync(RegisterConsumptionRequest request);
    Task<ResultResponse<IEnumerable<ReportResponse>>> GetByDateAsync(DateTime request);
}