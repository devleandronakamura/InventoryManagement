using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.DTOs.Responses;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Consumptions.Queries;

internal class GetConsumptionReportByDateQueryHandler : IRequestHandler<GetConsumptionReportByDateQuery, ResultResponse<List<ReportResponse>>>
{
    private readonly IConsumptionRepository _consumptionRepository;
    private readonly ILogger<GetConsumptionReportByDateQueryHandler> _logger;

    public GetConsumptionReportByDateQueryHandler(IConsumptionRepository consumptionRepository, ILogger<GetConsumptionReportByDateQueryHandler> logger)
    {
        _consumptionRepository = consumptionRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<List<ReportResponse>>> Handle(GetConsumptionReportByDateQuery request, CancellationToken cancellationToken)
    {
        ResultResponse<List<ReportResponse>> resultResponse = new();

        var reportResponse = new List<ReportResponse>();

        try
        {
            var consumptions = await _consumptionRepository.GetByDateAsync(request.Date);

            foreach (var consumption in consumptions)
            {
                reportResponse.Add(new ReportResponse()
                {
                    Product = consumption.Product,
                    AveragePrice = consumption.AveragePrice,
                    Quantity = consumption.Quantity
                });
            }

            resultResponse.SetData(reportResponse);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Erro ao buscar o relatório de consumo (Data='{request.Date}').";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}