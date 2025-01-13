using AutoMapper;
using InventoryManagement.Application.Consumptions.Commands;
using InventoryManagement.Application.Consumptions.Queries;
using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.DTOs.Responses;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Services;

public class ConsumptionService(IMediator mediator, IMapper mapper) : IConsumptionService
{
    public async Task<ResultResponse<IEnumerable<ReportResponse>>> GetByDateAsync(DateTime request)
    {
        ResultResponse<IEnumerable<ReportResponse>> resultResponseService = new();

        var reportQuery = new GetConsumptionReportByDateQuery(request);

        var resultResponseCommand = await mediator.Send(reportQuery);

        if (resultResponseCommand.Success)
        {
            resultResponseService.SetData(resultResponseCommand.Data);
        }
        else
            resultResponseService.SetValidationErrors(resultResponseCommand.StatusCode, resultResponseCommand.ErrorMessage, resultResponseCommand.ErrorsDto);

        return resultResponseService;
    }

    public async Task<ResultResponse<ConsumptionResponse>> RegisterAsync(RegisterConsumptionRequest request)
    {
        ResultResponse<ConsumptionResponse> resultResponseService = new();

        var consumption = mapper.Map<Consumption>(request);

        var consumptionCommand = new RegisterConsumptionCommand(consumption);

        var resultResponseCommand = await mediator.Send(consumptionCommand);

        if (resultResponseCommand.Success)
        {
            var consumptionResponse = mapper.Map<ConsumptionResponse>(resultResponseCommand.Data);
            resultResponseService.SetData(consumptionResponse);
        }
        else
            resultResponseService.SetValidationErrors(resultResponseCommand.StatusCode, resultResponseCommand.ErrorMessage, resultResponseCommand.ErrorsDto);

        return resultResponseService;
    }
}