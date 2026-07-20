using Cookbook.API.Models.Requests.Passo;
using FluentValidation;

namespace Cookbook.API.Validators.Passo
{
    public class PassoRequestValidator : AbstractValidator<PassoRequest>
    {
        public PassoRequestValidator()
        {
            RuleFor(x => x.Ordem)
                .GreaterThan(0).WithMessage("A ordem do passo deve ser maior que zero.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do passo é obrigatória.")
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");
        }
    }
}
