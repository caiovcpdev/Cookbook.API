using Cookbook.API.Models.Entities;

namespace Cookbook.API.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> ListarAsync();
        Task<Categoria?> ObterPorIdAsync(int id);
        Task<Categoria?> ObterPorNomeAsync(string nome);
        Task<int> CriarAsync(Categoria categoria);
        Task AtualizarAsync(Categoria categoria);
        Task ExcluirAsync(int id);
    }
}
