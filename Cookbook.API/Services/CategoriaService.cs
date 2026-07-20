using Cookbook.API.Helpers.Exceptions;
using Cookbook.API.Models.Entities;
using Cookbook.API.Models.Requests.Categoria;
using Cookbook.API.Models.Responses.Categoria;
using Cookbook.API.Repositories.Interfaces;
using Cookbook.API.Services.Interfaces;

namespace Cookbook.API.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
                _categoriaRepository = categoriaRepository;
        }
        public async Task AtualizarAsync(int id, AtualizarCategoriaRequest request)
        {
            var categoria = await BuscarOuFalharAsync(id);

            var existente = await _categoriaRepository.ObterPorNomeAsync(request.Nome);

            if (existente is not null && existente.Id != id)
                throw new ConflictException("Já existe uma categoria com esse nome.");
            
            categoria.Nome = request.Nome;
            await _categoriaRepository.AtualizarAsync(categoria);
        }

        public async Task<CategoriaResponse> CriarAsync(CriarCategoriaRequest request)
        {
            var existente = await _categoriaRepository.ObterPorNomeAsync(request.Nome);

            if (existente is not null)
            {
                throw new ConflictException("Já existe uma categoria com esse nome.");
            }
            
            var categoria = new Categoria { Nome = request.Nome };
            var id = await _categoriaRepository.CriarAsync(categoria);

            return MapearParaResponse(categoria);
        }

        public async Task ExcluirAsync(int id)
        {
            await BuscarOuFalharAsync(id);
            await _categoriaRepository.ExcluirAsync(id);
        }

        public async Task<IEnumerable<CategoriaResponse>> ListarAsync()
        {
            var categoria = await _categoriaRepository.ListarAsync();
            return categoria.Select(MapearParaResponse);
        }

        public async Task<CategoriaResponse> ObterPorIdAsync(int id)
        {
            var categoria = await BuscarOuFalharAsync(id);
            return MapearParaResponse(categoria);
        }
        private async Task<Categoria> BuscarOuFalharAsync(int id)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(id);

            if (categoria is null)
            {
                throw new NotFoundException("Categoria não encontrada.");
            }

            return categoria;
        }
        private static CategoriaResponse MapearParaResponse(Categoria categoria)
        {
            return new CategoriaResponse
            {
                Id = categoria.Id,
                Nome = categoria.Nome
            };
        }
    }
}
