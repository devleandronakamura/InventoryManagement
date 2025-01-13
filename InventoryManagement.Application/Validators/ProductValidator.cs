using FluentValidation;
using InventoryManagement.Domain.Entities;
namespace InventoryManagement.Application.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.PartNumber)
            .NotEmpty().WithMessage("O campo 'PartNumber' não pode ser em branco (campo obrigatório).");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("O campo 'Price' deve ser maior do que zero.");

        RuleFor(x => x.ItemId)
           .NotEqual(Guid.Empty).WithMessage("O campo 'ItemId' não pode ser um Guid vazio.");
    }
}