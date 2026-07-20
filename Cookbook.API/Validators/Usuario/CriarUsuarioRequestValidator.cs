using Cookbook.API.Models.Requests.Usuario;
using FluentValidation;

namespace Cookbook.API.Validators.Usuario
{
    public class CriarUsuarioRequestValidator : AbstractValidator<CriarUsuarioRequest> //Fluent validation ao inves de data annotations, para validação de requisições, é mais flexível e permite criar regras complexas de validação
    {
        public CriarUsuarioRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail informado não é válido.")
                .MaximumLength(200).WithMessage("O e-mail deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
        }
    }
}
