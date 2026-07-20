using Cookbook.API.Models.Requests.Ingrediente;
using FluentValidation;

namespace Cookbook.API.Validators.Ingrediente
{
    public class IngredienteRequestValidator : AbstractValidator<IngredienteRequest>
    {
        public IngredienteRequestValidator()
        {
            RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do ingrediente é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Quantidade)
                .NotEmpty().WithMessage("A quantidade é obrigatória.")
                .MaximumLength(50).WithMessage("A quantidade deve ter no máximo 50 caracteres.");
        }
    }
}
