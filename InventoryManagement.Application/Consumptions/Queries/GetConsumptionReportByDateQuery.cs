using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.DTOs.Responses;
using MediatR;

namespace InventoryManagement.Application.Consumptions.Queries;

public class GetConsumptionReportByDateQuery(DateTime date) : IRequest<ResultResponse<List<ReportResponse>>>
{
    public DateTime Date = date;
}