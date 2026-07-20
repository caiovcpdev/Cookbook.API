using BCrypt.Net;
using Cookbook.API.Helpers.Exceptions;
using Cookbook.API.Helpers.Jwt;
using Cookbook.API.Models.Entities;
using Cookbook.API.Models.Requests.Usuario;
using Cookbook.API.Models.Responses.Usuario;
using Cookbook.API.Repositories.Interfaces;
using Cookbook.API.Services.Interfaces;

namespace Cookbook.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public UsuarioService(IUsuarioRepository usuarioRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _usuarioRepository = usuarioRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request)
        {
            var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(request.Email);

            if (usuarioExistente is not null)
                throw new ConflictException("Já existe um usuário cadastrado com essse e-mail.");
            
            var senhaHash  = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            //Aplicando fuso horário diretamente no banco.
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            var fusoDataCadastro = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = senhaHash,
                DataCadastro = fusoDataCadastro
            };

            var id = await _usuarioRepository.CriarAsync(usuario);

            return new UsuarioResponse 
            { 
                Id = id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                FotoPerfil = usuario.FotoPerfil,
                DataCadastro = usuario.DataCadastro
            };
            throw new NotImplementedException();
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                throw new UnauthorizedAccessException("E-mail ou senha inválidos");

            var (token, expiraEm) = _jwtTokenGenerator.GerarToken(usuario);

            return new LoginResponse { Token = token, ExpiraEm = expiraEm };
        }

        public async Task<UsuarioResponse> ObterPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario is null)
                throw new NotFoundException("Usuário não encontrado.");

            return new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                FotoPerfil = usuario.FotoPerfil,
                DataCadastro = usuario.DataCadastro
            };
        }
    }
}
