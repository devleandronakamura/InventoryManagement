using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultResponse = await productService.GetAllAsync();

        if (resultResponse.StatusCode == 400)
            return Ok(resultResponse.ErrorMessage);

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return Ok(resultResponse.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var resultResponse = await productService.GetById(id);

        if (resultResponse.StatusCode == 400)
            return Ok(resultResponse.ErrorMessage);

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return Ok(resultResponse.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateProductRequest request)
    {
        var resultResponse = await productService.AddAsync(request);

        if (resultResponse.StatusCode == 400)
        {
            if (resultResponse?.ErrorsDto?.Errors.Count > 0)
                return Ok(resultResponse.ErrorsDto);
            else
                return Ok(resultResponse?.ErrorMessage);
        }

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = resultResponse?.Data?.Id }, resultResponse?.Data);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductRequest request)
    {
        var resultResponse = await productService.UpdateAsync(request);

        if (resultResponse.StatusCode == 400)
        {
            if (resultResponse?.ErrorsDto?.Errors.Count > 0)
                return Ok(resultResponse.ErrorsDto);
            else
                return Ok(resultResponse?.ErrorMessage);
        }

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var resultResponse = await productService.DeleteByIdAsync(id);

        if (resultResponse.StatusCode == 400)
            return Ok(resultResponse.ErrorMessage);

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return NoContent();
    }
}