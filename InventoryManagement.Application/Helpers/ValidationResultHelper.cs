using FluentValidation.Results;
using InventoryManagement.Application.DTOs.Base;

namespace InventoryManagement.Application.Helpers;

public static class ValidationResultHelper
{
    public static ErrorsDto ToErrorsDto(ValidationResult validationResult)
    {
        var errors = new List<string>();

        foreach (var error in validationResult.Errors)
            errors.Add(error.ErrorMessage);

        return new ErrorsDto(errors);
    }
}