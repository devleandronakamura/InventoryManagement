using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Items.Commands;

internal class DeleteItemByIdCommandHandler : IRequestHandler<DeleteItemByIdCommand, ResultResponse<Item>>
{
    private readonly ILogger<DeleteItemByIdCommandHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public DeleteItemByIdCommandHandler(IItemRepository itemRepository, ILogger<DeleteItemByIdCommandHandler> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<Item>> Handle(DeleteItemByIdCommand request, CancellationToken cancellationToken)
    {
        ResultResponse<Item> resultResponse = new();

        try
        {
            var item = await _itemRepository.GetByIdAsync(request.Id);

            if (item is null)
            {
                int statusCode = 400;
                var errorMessage = $"Erro: o item (Id='{request.Id}') não existe.";
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode, errorMessage);
                return resultResponse;
            }

            item.SetDeleted();

            _ = await _itemRepository.DeleteAsync(item);
            resultResponse.SetData(item);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Ocorreu um erro inesperado ao excluir o item onde (Id={request.Id}).";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}