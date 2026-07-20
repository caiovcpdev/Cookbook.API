using Cookbook.API.Models.Entities;

namespace Cookbook.API.Repositories.Interfaces
{
    public interface IReceitaRepository
    {
        Task<IEnumerable<Receita>> ListarAsync();
        Task<Receita?> ObterPorIdAsync(int id);

        Task<(Receita? Receita, IEnumerable<Ingrediente> Ingredientes, IEnumerable<Passo> Passos)> ObterDetalhadoPorIdAsync(int id);
        Task<int> CriarComIngredientesEPassosAsync(Receita receita, List<Ingrediente> ingredientes, List<Passo> passos);

        Task<int> CriarAsync(Receita receita);
        Task AtualizarAsync(Receita receita);
        Task ExcluirAsync(int id);
        Task<bool> CategoriaExisteAsync(int categoriaId);

        Task AdicionarIngredienteAsync(Ingrediente ingrediente);
        Task<Ingrediente?> ObterIngredientePorIdAsync(int id);
        Task RemoverIngredienteAsync(int id);

        Task AdicionarPassoAsync(Passo passo);
        Task<Passo?> ObterPassoPorIdAsync(int id);
        Task RemoverPassoAsync(int id);
    }
}
