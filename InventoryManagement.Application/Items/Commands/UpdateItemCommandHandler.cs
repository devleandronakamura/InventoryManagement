using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.Helpers;
using InventoryManagement.Application.Validators;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Items.Commands;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ResultResponse<Item>>
{
    private readonly ILogger<UpdateItemCommandHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public UpdateItemCommandHandler(IItemRepository itemRepository, ILogger<UpdateItemCommandHandler> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<Item>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        ResultResponse<Item> resultResponse = new();
        var validator = new ItemValidator();

        try
        {
            var validationResult = validator.Validate(request.Item);

            if (!validationResult.IsValid)
            {
                resultResponse.SetValidationErrors(statusCode: 400, errorMessage: "Erro de validação.", errorsDto: ValidationResultHelper.ToErrorsDto(validationResult));
                return resultResponse;
            }

            var itemDb = await _itemRepository.GetByIdAsync(request.Item.Id);

            if (itemDb is null)
            {
                int statusCode = 400;
                var errorMessage = $"Erro: o item (Id='{request.Item.Id}') não existe.";
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode, errorMessage);
                return resultResponse;
            }

            itemDb = await _itemRepository.GetByIdAndNameAsync(request.Item.Id, request.Item.Name);

            if (itemDb is not null)
            {
                int statusCode = 400;
                var errorMessage = $"Erro: o item (Nome='{request.Item.Name}') já existe.";
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode, errorMessage);
                return resultResponse;
            }

            request.Item.SetUpdatedAt();

            var item = await _itemRepository.UpdateAsync(request.Item);
            resultResponse.SetData(item);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Ocorreu um erro inesperado ao atualizar o item onde (Id={request.Item.Id}).";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}