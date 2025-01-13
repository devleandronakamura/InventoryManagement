using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.Helpers;
using InventoryManagement.Application.Validators;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResultResponse<Product>>
{
    private readonly ILogger<UpdateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IItemRepository _itemRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository, IItemRepository itemRepository, ILogger<UpdateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _itemRepository = itemRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        ResultResponse<Product> resultResponse = new();
        var validator = new ProductValidator();

        try
        {
            var validationResult = validator.Validate(request.Product);

            if (!validationResult.IsValid)
            {
                resultResponse.SetValidationErrors(statusCode: 400, errorMessage: "Erro de validação.", errorsDto: ValidationResultHelper.ToErrorsDto(validationResult));
                return resultResponse;
            }

            var productDb = await _productRepository.GetByIdAsync(request.Product.Id);

            if (productDb is null)
            {
                int statusCode = 400;
                var errorMessage = $"Erro: o produto (Id='{request.Product.Id}') não existe.";
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode, errorMessage);
                return resultResponse;
            }

            productDb = await _productRepository.GetByIdAndPartNumberAsync(request.Product.Id, request.Product.PartNumber);

            if (productDb is not null)
            {
                int statusCode = 400;
                var errorMessage = $"Erro: o produto (PartNumber='{request.Product.PartNumber}') já existe.";
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode, errorMessage);
                return resultResponse;
            }

            var item = await _itemRepository.GetByIdAsync(request.Product.ItemId);

            if (item is null)
            {
                var errorMessage = $"Erro: o item (Id='{request.Product.ItemId}') não existe.";
                int statusCode = 400;
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode: statusCode, errorMessage: errorMessage);
                return resultResponse;
            }

            request.Product.SetUpdatedAt();

            var product = await _productRepository.UpdateAsync(request.Product);
            resultResponse.SetData(product);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Ocorreu um erro inesperado ao atualizar o produto onde (Id={request.Product.Id}).";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}