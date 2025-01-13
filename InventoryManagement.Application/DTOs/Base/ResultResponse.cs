namespace InventoryManagement.Application.DTOs.Base;

public class ResultResponse<T>
{
    public void SetData(T? data)
    {
        Success = true;
        Data = data;
    }

    public void SetErrorMessage(int statusCode, string? errorMessage)
    {
        Success = false;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }

    public void SetValidationErrors(int statusCode, string? errorMessage, ErrorsDto? errorsDto)
    {
        Success = false;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
        ErrorsDto = errorsDto;
    }

    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public ErrorsDto? ErrorsDto { get; set; }
    public T? Data { get; set; }
}

public class ErrorsDto
{
    public ErrorsDto(List<string> errors)
    {
        Errors = errors;
    }

    public List<string> Errors { get; set; } = new();
}