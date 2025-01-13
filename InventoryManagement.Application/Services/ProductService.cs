using AutoMapper;
using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.DTOs.Responses;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.Items.Commands;
using InventoryManagement.Application.Products.Commands;
using InventoryManagement.Application.Products.Queries;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Services;

public class ProductService(IMediator mediator, IMapper mapper) : IProductService
{
    public async Task<ResultResponse<ProductResponse>> AddAsync(CreateProductRequest request)
    {
        ResultResponse<ProductResponse> resultResponseService = new();

        var product = mapper.Map<Product>(request);

        var productCommand = new CreateProductCommand(product);

        var resultResponseCommand = await mediator.Send(productCommand);

        if (resultResponseCommand.Success)
        {
            var productResponse = mapper.Map<ProductResponse>(resultResponseCommand.Data);
            resultResponseService.SetData(productResponse);
        }
        else
            resultResponseService.SetValidationErrors(resultResponseCommand.StatusCode, resultResponseCommand.ErrorMessage, resultResponseCommand.ErrorsDto);

        return resultResponseService;
    }

    public async Task<ResultResponse<ProductResponse>> DeleteByIdAsync(Guid id)
    {
        ResultResponse<ProductResponse> resultResponseService = new();

        var productCommand = new DeleteProductByIdCommand(id);

        var resultResponseCommand = await mediator.Send(productCommand);

        if (resultResponseCommand.Success)
        {
            var productResponse = mapper.Map<ProductResponse>(resultResponseCommand.Data);
            resultResponseService.SetData(productResponse);
        }
        else
            resultResponseService.SetErrorMessage(resultResponseCommand.StatusCode, resultResponseCommand.ErrorMessage);

        return resultResponseService;
    }

    public async Task<ResultResponse<IEnumerable<ProductResponse>>> GetAllAsync()
    {
        ResultResponse<IEnumerable<ProductResponse>> resultResponseService = new();

        var productQuery = new GetAllProductQuery();

        var resultResponseQuery = await mediator.Send(productQuery);

        if (resultResponseQuery.Success)
        {
            var productResponse = mapper.Map<IEnumerable<ProductResponse>>(resultResponseQuery.Data);
            resultResponseService.SetData(productResponse);
        }
        else
            resultResponseService.SetErrorMessage(resultResponseQuery.StatusCode, resultResponseQuery.ErrorMessage);

        return resultResponseService;
    }

    public async Task<ResultResponse<ProductResponse>> GetById(Guid id)
    {
        ResultResponse<ProductResponse> resultResponseService = new();

        var productQuery = new GetProductByIdQuery(id);

        var resultResponseQuery = await mediator.Send(productQuery);

        if (resultResponseQuery.Success)
        {
            var productResponse = mapper.Map<ProductResponse>(resultResponseQuery.Data);
            resultResponseService.SetData(productResponse);
        }
        else
            resultResponseService.SetErrorMessage(resultResponseQuery.StatusCode, resultResponseQuery.ErrorMessage);

        return resultResponseService;
    }

    public async Task<ResultResponse<ProductResponse>> UpdateAsync(UpdateProductRequest request)
    {
        ResultResponse<ProductResponse> resultResponseService = new();

        var product = mapper.Map<Product>(request);

        var productCommand = new UpdateProductCommand(product);

        var resultResponseCommand = await mediator.Send(productCommand);

        if (resultResponseCommand.Success)
        {
            var productResponse = mapper.Map<ProductResponse>(resultResponseCommand.Data);
            resultResponseService.SetData(productResponse);
        }
        else
            resultResponseService.SetValidationErrors(resultResponseCommand.StatusCode, resultResponseCommand.ErrorMessage, resultResponseCommand.ErrorsDto);

        return resultResponseService;
    }
}