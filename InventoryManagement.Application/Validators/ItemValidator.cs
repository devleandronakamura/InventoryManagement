using FluentValidation;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Validators;

public class ItemValidator : AbstractValidator<Item>
{
    public ItemValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O campo 'Nome' não pode ser em branco (campo obrigatório).");
    }
}