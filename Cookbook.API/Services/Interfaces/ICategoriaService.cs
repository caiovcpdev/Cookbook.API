using Cookbook.API.Models.Requests.Categoria;
using Cookbook.API.Models.Responses.Categoria;

namespace Cookbook.API.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaResponse>> ListarAsync();
        Task<CategoriaResponse> ObterPorIdAsync(int id);
        Task<CategoriaResponse> CriarAsync(CriarCategoriaRequest request);
        Task AtualizarAsync(int id, AtualizarCategoriaRequest request);
        Task ExcluirAsync(int id);
    }
}
