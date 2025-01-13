using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.Helpers;
using InventoryManagement.Application.Validators;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Consumptions.Commands;

internal class RegisterConsumptionCommandHandler : IRequestHandler<RegisterConsumptionCommand, ResultResponse<Consumption>>
{
    private readonly ILogger<RegisterConsumptionCommandHandler> _logger;
    private readonly IConsumptionRepository _consumptionRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IProductRepository _productRepository;

    public RegisterConsumptionCommandHandler(
        IConsumptionRepository consumptionRepository, 
        IItemRepository itemRepository,
        IProductRepository productRepository,
        ILogger<RegisterConsumptionCommandHandler> logger)
    {
        _consumptionRepository = consumptionRepository;
        _productRepository = productRepository;
        _itemRepository = itemRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<Consumption>> Handle(RegisterConsumptionCommand request, CancellationToken cancellationToken)
    {
        ResultResponse<Consumption> resultResponse = new();
        var validator = new ConsumptionValidator();

        try
        {
            var validationResult = validator.Validate(request.Consumption);

            if (!validationResult.IsValid)
            {
                resultResponse.SetValidationErrors(statusCode: 400, errorMessage: "Erro de validação.", errorsDto: ValidationResultHelper.ToErrorsDto(validationResult));
                return resultResponse;
            }

            var item = await _itemRepository.GetByIdAsync(request.Consumption.ItemId);

            if (item is null)
            {
                var errorMessage = $"Erro: o item (Id='{request.Consumption.ItemId}') não existe.";
                int statusCode = 400;
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode: statusCode, errorMessage: errorMessage);
                return resultResponse;
            }

            var products = await _productRepository.GetByIdAndQuantityAsync(request.Consumption.ItemId, request.Consumption.Quantity);

            var totalAvailableProducts = products.ToList().Count;

            if (totalAvailableProducts == 0)
            {
                var errorMessage = $"Erro: saldo indisponível do item.";
                int statusCode = 400;
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode: statusCode, errorMessage: errorMessage);
                return resultResponse;
            }
            else if (totalAvailableProducts < request.Consumption.Quantity)
            {
                var errorMessage = $"Erro: não existe disponível (Quantidade='{request.Consumption.Quantity}') no estoque do item.";
                int statusCode = 400;
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode: statusCode, errorMessage: errorMessage);
                return resultResponse;
            }

            var consumption = await _consumptionRepository.RegisterProductsIdsAsync(request.Consumption, products.Select(x => x.Id).ToList());

            resultResponse.SetData(consumption);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Erro ao inserir o consumo (ItemId='{request.Consumption.ItemId}' e Quantidade='{request.Consumption.Quantity}').";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}