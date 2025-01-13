using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.Helpers;
using InventoryManagement.Application.Validators;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Items.Commands;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ResultResponse<Item>>
{
    private readonly ILogger<CreateItemCommandHandler> _logger;
    private readonly IItemRepository _itemRepository;

    public CreateItemCommandHandler(IItemRepository itemRepository, ILogger<CreateItemCommandHandler> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultResponse<Item>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
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

            var itemDb = await _itemRepository.GetByNameAsync(request.Item.Name);

            if (itemDb is not null)
            {
                var errorMessage = $"Erro: o item (Nome='{request.Item.Name}') já existe.";
                int statusCode = 400;
                _logger.LogInformation("StatusCode='{0}'. MessageError='{1}'.'", statusCode, errorMessage);
                resultResponse.SetErrorMessage(statusCode: statusCode, errorMessage: errorMessage);
                return resultResponse;
            }

            var item = await _itemRepository.AddAsync(request.Item);

            resultResponse.SetData(item);
        }
        catch (Exception ex)
        {
            int statusCode = 500;
            var errorMessage = $"Erro ao inserir o item (Nome='{request.Item.Name}').";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", statusCode, errorMessage, ex.Message);
            resultResponse.SetErrorMessage(statusCode, errorMessage);
        }

        return resultResponse;
    }
}