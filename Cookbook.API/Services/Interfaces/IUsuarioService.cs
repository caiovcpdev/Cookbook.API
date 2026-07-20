using Cookbook.API.Models.Requests.Usuario;
using Cookbook.API.Models.Responses.Usuario;

namespace Cookbook.API.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<UsuarioResponse> ObterPorIdAsync(int id);
    }
}
