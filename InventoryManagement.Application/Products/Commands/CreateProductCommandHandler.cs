using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.Helpers;
using InventoryManagement.Application.Validators;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResultResponse<Product>>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IItemRepository _itemRepository;

    public CreateProductCommandHandler(IProductRepository productRepository, IItemRepository itemRepository, ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _itemRepository = itemRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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

            var productPartNumber = await _productRepository.GetByPartNumberAsync(request.Product.PartNumber);

            if (productPartNumber is not null)
            {
                var errorMessage = $"Erro: o produto (PartNumber='{request.Product.PartNumber}') já existe.";
                int statusCode = 400;
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode: statusCode, errorMessage: errorMessage);
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

            _ = await _productRepository.AddAsync(request.Product);

            var product = await _productRepository.GetByIdAsync(request.Product.Id);

            resultResponse.SetData(product);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Erro ao inserir o produto (PartNumber='{request.Product.PartNumber}').";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}