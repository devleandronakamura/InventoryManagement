using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Queries;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, ResultResponse<IEnumerable<Product>>>
{
    private readonly ILogger<GetAllProductQueryHandler> _logger;
    private readonly IProductRepository _productRepository;

    public GetAllProductQueryHandler(IProductRepository productRepository, ILogger<GetAllProductQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<IEnumerable<Product>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        ResultResponse<IEnumerable<Product>> resultResponse = new();

        try
        {
            var products = await _productRepository.GetAllAsync();
            resultResponse.SetData(products);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = "Erro ao buscar todos os produtos.";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}