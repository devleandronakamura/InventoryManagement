using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController(IItemService itemService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultResponse = await itemService.GetAllAsync();

        if (resultResponse.StatusCode == 400)
            return Ok(resultResponse.ErrorMessage);

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return Ok(resultResponse.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var resultResponse = await itemService.GetById(id);

        if (resultResponse.StatusCode == 400)
            return Ok(resultResponse.ErrorMessage);

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return Ok(resultResponse.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateItemRequest request)
    {
        var resultResponse = await itemService.AddAsync(request);

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
    public async Task<IActionResult> Update(UpdateItemRequest request)
    {
        var resultResponse = await itemService.UpdateAsync(request);

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
        var resultResponse = await itemService.DeleteByIdAsync(id);

        if (resultResponse.StatusCode == 400)
            return Ok(resultResponse.ErrorMessage);

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return NoContent();
    }
}