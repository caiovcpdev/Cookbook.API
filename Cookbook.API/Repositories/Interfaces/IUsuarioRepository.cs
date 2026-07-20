using Cookbook.API.Models.Entities;

namespace Cookbook.API.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<int> CriarAsync(Usuario usuario);
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<Usuario?> ObterPorEmailAsync(string email);
    }
}
