using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Commands;

internal class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, ResultResponse<Product>>
{
    private readonly ILogger<DeleteProductByIdCommandHandler> _logger;
    private readonly IProductRepository _productRepository;

    public DeleteProductByIdCommandHandler(IProductRepository productRepository, ILogger<DeleteProductByIdCommandHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<Product>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        ResultResponse<Product> resultResponse = new();

        try
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product is null)
            {
                int statusCode = 400;
                var errorMessage = $"Erro: o produto (Id='{request.Id}') não existe.";
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode, errorMessage);
                return resultResponse;
            }

            product.SetDeleted();

            _ = await _productRepository.DeleteAsync(product);
            resultResponse.SetData(product);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Ocorreu um erro inesperado ao excluir o produto onde (Id={request.Id}).";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}