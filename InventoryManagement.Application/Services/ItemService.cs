using AutoMapper;
using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.DTOs.Responses;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.Items.Commands;
using InventoryManagement.Application.Items.Queries;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Services;

public class ItemService(IMediator mediator, IMapper mapper) : IItemService
{
    public async Task<ResultResponse<ItemResponse>> AddAsync(CreateItemRequest request)
    {
        ResultResponse<ItemResponse> resultResponseService = new();

        var item = mapper.Map<Item>(request);

        var itemCommand = new CreateItemCommand(item);

        var resultResponseCommand = await mediator.Send(itemCommand);

        if (resultResponseCommand.Success)
        {
            var itemResponse = mapper.Map<ItemResponse>(resultResponseCommand.Data);
            resultResponseService.SetData(itemResponse);
        }
        else
            resultResponseService.SetValidationErrors(resultResponseCommand.StatusCode, resultResponseCommand.ErrorMessage, resultResponseCommand.ErrorsDto);

        return resultResponseService;
    }

    public async Task<ResultResponse<ItemResponse>> DeleteByIdAsync(Guid id)
    {
        ResultResponse<ItemResponse> resultResponseService = new();

        var itemCommand = new DeleteItemByIdCommand(id);

        var resultResponseCommand = await mediator.Send(itemCommand);

        if (resultResponseCommand.Success)
        {
            var itemResponse = mapper.Map<ItemResponse>(resultResponseCommand.Data);
            resultResponseService.SetData(itemResponse);
        }
        else
            resultResponseService.SetErrorMessage(resultResponseCommand.StatusCode, resultResponseCommand.ErrorMessage);

        return resultResponseService;
    }

    public async Task<ResultResponse<IEnumerable<ItemResponse>>> GetAllAsync()
    {
        ResultResponse<IEnumerable<ItemResponse>> resultResponseService = new();

        var itemQuery = new GetAllItemQuery();

        var resultResponseQuery = await mediator.Send(itemQuery);

        if (resultResponseQuery.Success)
        {
            var itemResponse = mapper.Map<IEnumerable<ItemResponse>>(resultResponseQuery.Data);
            resultResponseService.SetData(itemResponse);
        }
        else
            resultResponseService.SetErrorMessage(resultResponseQuery.StatusCode, resultResponseQuery.ErrorMessage);

        return resultResponseService;
    }

    public async Task<ResultResponse<ItemResponse>> GetById(Guid id)
    {
        ResultResponse<ItemResponse> resultResponseService = new();

        var itemQuery = new GetItemByIdQuery(id);

        var resultResponseQuery = await mediator.Send(itemQuery);

        if (resultResponseQuery.Success)
        {
            var itemResponse = mapper.Map<ItemResponse>(resultResponseQuery.Data);
            resultResponseService.SetData(itemResponse);
        }
        else
            resultResponseService.SetErrorMessage(resultResponseQuery.StatusCode, resultResponseQuery.ErrorMessage);

        return resultResponseService;
    }

    public async Task<ResultResponse<ItemResponse>> UpdateAsync(UpdateItemRequest request)
    {
        ResultResponse<ItemResponse> resultResponseService = new();

        var item = mapper.Map<Item>(request);

        var itemCommand = new UpdateItemCommand(item);

        var resultResponseCommand = await mediator.Send(itemCommand);
        
        if (resultResponseCommand.Success)
        {
            var itemResponse = mapper.Map<ItemResponse>(resultResponseCommand.Data);
            resultResponseService.SetData(itemResponse);
        }            
        else
            resultResponseService.SetValidationErrors(resultResponseCommand.StatusCode, resultResponseCommand.ErrorMessage, resultResponseCommand.ErrorsDto);

        return resultResponseService;
    }
}