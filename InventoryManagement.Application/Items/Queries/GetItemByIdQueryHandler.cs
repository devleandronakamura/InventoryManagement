using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Items.Queries;

internal class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ResultResponse<Item>>
{
    private readonly ILogger<GetItemByIdQueryHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public GetItemByIdQueryHandler(IItemRepository itemRepository, ILogger<GetItemByIdQueryHandler> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<Item>> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        ResultResponse<Item> resultResponse = new();

        try
        {
            var item = await _itemRepository.GetByIdAsync(request.Id);

            if (item is null)
            {
                var errorMessage = $"Erro: o item (Id='{request.Id}') não existe.";
                int statusCode = 400;
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode: statusCode, errorMessage: errorMessage);
                return resultResponse;
            }

            resultResponse.SetData(item);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Erro ao buscar o item por (Id='{request.Id}').";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}