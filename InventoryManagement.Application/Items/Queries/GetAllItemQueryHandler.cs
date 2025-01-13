using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Items.Queries;

public class GetAllItemQueryHandler: IRequestHandler<GetAllItemQuery, ResultResponse<IEnumerable<Item>>>
{
    private readonly ILogger<GetAllItemQueryHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public GetAllItemQueryHandler(IItemRepository itemRepository, ILogger<GetAllItemQueryHandler> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<IEnumerable<Item>>> Handle(GetAllItemQuery request, CancellationToken cancellationToken)
    {
        ResultResponse<IEnumerable<Item>> resultResponse = new();

        try
        {
            var items = await _itemRepository.GetAllAsync();
            resultResponse.SetData(items);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = "Erro ao buscar todos os items.";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}