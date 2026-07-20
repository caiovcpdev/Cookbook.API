using Cookbook.API.Models.Requests.Ingrediente;
using Cookbook.API.Models.Requests.Passo;
using Cookbook.API.Models.Requests.Receita;
using Cookbook.API.Models.Responses.Ingrediente;
using Cookbook.API.Models.Responses.Passo;
using Cookbook.API.Models.Responses.Receita;

namespace Cookbook.API.Services.Interfaces
{
    public interface IReceitaService
    {
        Task<IEnumerable<ReceitaResponse>> ListarAsync();
        Task<ReceitaDetalhesResponse> ObterDetalhesAsync(int id);
        Task<ReceitaResponse> ObterPorIdAsync(int id);
        Task<ReceitaDetalhesResponse> CriarAsync(CriarReceitaRequest request);
        Task AtualizarAsync(int id, AtualizarReceitaRequest request);
        Task ExcluirAsync(int id);

        Task<IngredienteResponse> AdicionarIngredienteAsync(int receitaId, IngredienteRequest request);
        Task RemoverIngredienteAsync(int receitaId, int ingredienteId);

        Task<PassoResponse> AdicionarPassoAsync(int receitaId, PassoRequest request);
        Task RemoverPassoAsync(int receitaId, int passoId);
    }
}
