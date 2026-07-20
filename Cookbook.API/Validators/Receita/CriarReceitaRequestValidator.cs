using Cookbook.API.Models.Requests.Receita;
using Cookbook.API.Validators.Ingrediente;
using Cookbook.API.Validators.Passo;
using FluentValidation;

namespace Cookbook.API.Validators.Receita
{
    public class CriarReceitaRequestValidator : AbstractValidator<CriarReceitaRequest>
    {
        public CriarReceitaRequestValidator()
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

            RuleFor(x => x.Dificuldade)
                .IsInEnum()
                .WithMessage("Informe uma dificuldade válida.");

            RuleFor(x => x.Ingredientes)
                .NotEmpty().WithMessage("A receita precisa ter pelo menos um ingrediente.");

            RuleForEach(x => x.Ingredientes)
                .SetValidator(new IngredienteRequestValidator());

            RuleFor(x => x.Passos)
                .NotEmpty().WithMessage("A receita precisa ter pelo menos um passo.");

            RuleForEach(x => x.Passos)
                .SetValidator(new PassoRequestValidator());
        }
    }
}
