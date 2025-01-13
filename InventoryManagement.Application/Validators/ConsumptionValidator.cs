using FluentValidation;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Validators;

public class ConsumptionValidator : AbstractValidator<Consumption>
{
    public ConsumptionValidator()
    {
        RuleFor(x => x.ItemId)
             .NotEqual(Guid.Empty).WithMessage("O campo 'ItemId' não pode ser um Guid vazio.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("O campo 'Quantity' deve ser maior do que zero.");
    }
}