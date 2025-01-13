using InventoryManagement.Application.DTOs.Requests;
using InventoryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsumptionController(IConsumptionService consumptionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add(RegisterConsumptionRequest request)
    {
        var resultResponse = await consumptionService.RegisterAsync(request);

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

    [HttpGet("report/{date}")]
    public async Task<IActionResult> GetById(DateTime date)
    {
        var resultResponse = await consumptionService.GetByDateAsync(date);

        if (resultResponse.StatusCode == 400)
            return Ok(resultResponse.ErrorMessage);

        if (resultResponse.StatusCode == 500)
            return StatusCode(500, resultResponse.ErrorMessage);

        return Ok(resultResponse.Data);
    }
}