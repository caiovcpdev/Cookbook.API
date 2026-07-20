using Cookbook.API.Models.Entities;
using Cookbook.API.Models.Requests.Categoria;
using FluentValidation;

namespace Cookbook.API.Validators.Categoria
{
    public class CriarCategoriaRequestValidator : AbstractValidator<CriarCategoriaRequest>
    {
        public CriarCategoriaRequestValidator()
        {
            RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
        }
    }
}
