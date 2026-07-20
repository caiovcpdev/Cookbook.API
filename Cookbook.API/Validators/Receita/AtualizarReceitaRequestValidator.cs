using Cookbook.API.Models.Requests.Receita;
using FluentValidation;

namespace Cookbook.API.Validators.Receita
{
    public class AtualizarReceitaRequestValidator : AbstractValidator<AtualizarReceitaRequest>
    {
        public AtualizarReceitaRequestValidator()
        {
            RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome da receita é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Descricao)
                .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.");

            RuleFor(x => x.CategoriaId)
                .GreaterThan(0).WithMessage("Informe uma categoria válida.");

            RuleFor(x => x.TempoPreparo)
                .GreaterThan(0).WithMessage("O tempo de preparo deve ser maior que zero.");

            RuleFor(x => x.Porcoes)
                .GreaterThan(0).WithMessage("A quantidade de porções deve ser maior que zero.");
        }
    }
}
