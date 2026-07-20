using Cookbook.API.Helpers.CurrentUser;
using Cookbook.API.Helpers.Exceptions;
using Cookbook.API.Models.Entities;
using Cookbook.API.Models.Requests.Ingrediente;
using Cookbook.API.Models.Requests.Passo;
using Cookbook.API.Models.Requests.Receita;
using Cookbook.API.Models.Responses.Ingrediente;
using Cookbook.API.Models.Responses.Passo;
using Cookbook.API.Models.Responses.Receita;
using Cookbook.API.Repositories.Interfaces;
using Cookbook.API.Services.Interfaces;
using System.Runtime.InteropServices;

namespace Cookbook.API.Services
{
    public class ReceitaService : IReceitaService
    {
        private readonly IReceitaRepository _receitaRepository;
        private readonly ICurrentUserService _currentUserService;
        public ReceitaService(IReceitaRepository receitaRepository, ICurrentUserService currentUserService)
        {
            _receitaRepository = receitaRepository;
            _currentUserService = currentUserService;
        }
        public async Task AtualizarAsync(int id, AtualizarReceitaRequest request)
        {

            var receita = await BuscarOuFalharAsync(id);

            GarantirQueEhDono(receita);

            var categoriaExiste = await _receitaRepository.CategoriaExisteAsync(id);


            if (!categoriaExiste)
                throw new DirectoryNotFoundException("Categoria informada não foi encontrada.");

            receita.CategoriaId = request.CategoriaId;
            receita.Nome = request.Nome;
            receita.Descricao = request.Descricao;
            receita.TempoPreparo = request.TempoPreparo;
            receita.Porcoes = request.Porcoes;
            receita.Dificuldade = request.Dificuldade;
            receita.Imagem = request.Imagem;

            await _receitaRepository.AtualizarAsync(receita);
        }
        public async Task<ReceitaDetalhesResponse> CriarAsync(CriarReceitaRequest request)
        {
            var categoriaExiste = await _receitaRepository.CategoriaExisteAsync(request.CategoriaId);

            if (!categoriaExiste)
            {
                throw new NotFoundException("Categoria informada não foi encontrada.");
            }

            var usuarioId = _currentUserService.ObterUsuarioId();

            var receita = new Receita
            {
                UsuarioId = usuarioId,
                CategoriaId = request.CategoriaId,
                Nome = request.Nome,
                Descricao = request.Descricao,
                TempoPreparo = request.TempoPreparo,
                Porcoes = request.Porcoes,
                Dificuldade = request.Dificuldade,
                Imagem = request.Imagem,
                DataCadastro = DateTime.UtcNow
            };

            var ingredientes = request.Ingredientes
                .Select(i => new Ingrediente { Nome = i.Nome, Quantidade = i.Quantidade })
                .ToList();

            var passos = request.Passos
                .Select(p => new Passo { Ordem = p.Ordem, Descricao = p.Descricao })
                .ToList();

            var receitaId = await _receitaRepository.CriarComIngredientesEPassosAsync(receita, ingredientes, passos);
            receita.Id = receitaId;

            return MapearParaDetalhesResponse(receita, ingredientes, passos);
        }
        public async Task ExcluirAsync(int id)
        {
            var receita = await BuscarOuFalharAsync(id);

            GarantirQueEhDono(receita);

            await _receitaRepository.ExcluirAsync(id);
        }
        public async Task<IEnumerable<ReceitaResponse>> ListarAsync()
        {
            var receitas = await _receitaRepository.ListarAsync();
            return receitas.Select(MapearParaResponse);
        }
        public async Task<ReceitaResponse> ObterPorIdAsync(int id)
        {
            var receita = await _receitaRepository.ObterPorIdAsync(id);
            return MapearParaResponse(receita);
        }
        public async Task<ReceitaDetalhesResponse> ObterDetalhesAsync(int id)
        {
            var (receita, ingredientes, passos) = await _receitaRepository.ObterDetalhadoPorIdAsync(id);
            if (receita is null)
            {
                throw new NotFoundException("Receita não encontrada.");
            }

            return MapearParaDetalhesResponse(receita, ingredientes, passos);
        }

        public async Task<IngredienteResponse> AdicionarIngredienteAsync(int receitaId, IngredienteRequest request)
        {
            var receita = await BuscarOuFalharAsync(receitaId);
            GarantirQueEhDono(receita);

            var ingrediente = new Ingrediente
            {
                ReceitaId = receitaId,
                Nome = request.Nome,
                Quantidade = request.Quantidade
            };

            await _receitaRepository.AdicionarIngredienteAsync(ingrediente);

            return new IngredienteResponse
            {
                Id = ingrediente.Id,
                Nome = ingrediente.Nome,
                Quantidade = ingrediente.Quantidade
            };
        }
        public async Task RemoverIngredienteAsync(int receitaId, int ingredienteId)
        {
            var receita = await _receitaRepository.ObterPorIdAsync(receitaId);
            GarantirQueEhDono(receita);

            var ingrediente = await _receitaRepository.ObterIngredientePorIdAsync(ingredienteId);

            if (ingrediente is null || ingrediente.ReceitaId != receitaId)
                throw new NotFoundException("Ingrediente não encontrado nesta receita");

            await _receitaRepository.RemoverIngredienteAsync(ingredienteId);
        }
  
        public async Task<PassoResponse> AdicionarPassoAsync(int receitaId, PassoRequest request)
        {
            var receita = await _receitaRepository.ObterPorIdAsync(receitaId);
            GarantirQueEhDono(receita);

            var passo = new Passo
            {
                ReceitaId = receitaId,
                Ordem = request.Ordem,
                Descricao = request.Descricao

            };
            await _receitaRepository.AdicionarPassoAsync(passo);

            return new PassoResponse
            {
                Id = passo.Id,
                Ordem = passo.Ordem,
                Descricao = passo.Descricao
            };
        }
        public async Task RemoverPassoAsync(int receitaId, int passoId)
        {
            var receita = await BuscarOuFalharAsync(receitaId);
            GarantirQueEhDono(receita);

            var passo = await _receitaRepository.ObterPassoPorIdAsync(passoId);

            if (passo is null || passo.ReceitaId != receitaId)
                throw new NotFoundException("Passo não encontrado nessa receita.");

            await _receitaRepository.RemoverPassoAsync(passoId);
        }

        private void GarantirQueEhDono(Receita receita)
        {
            var usuarioId = _currentUserService.ObterUsuarioId();

            if (receita.UsuarioId != usuarioId)
            {
                throw new ForbiddenException("Você não tem permissão para alterar esta receita.");
            }
        }
        private async Task<Receita> BuscarOuFalharAsync(int id)
        {
            var receita = await _receitaRepository.ObterPorIdAsync(id);

            if (receita is null)
            {
                throw new NotFoundException("Receita não encontrada.");
            }

            return receita;
        }
        private static ReceitaResponse MapearParaResponse(Receita receita)
        {
            return new ReceitaResponse
            {
                Id = receita.Id,
                UsuarioId = receita.UsuarioId,
                CategoriaId = receita.CategoriaId,
                Nome = receita.Nome,
                Descricao = receita.Descricao,
                TempoPreparo = receita.TempoPreparo,
                Porcoes = receita.Porcoes,
                Dificuldade = receita.Dificuldade.ToString(),
                Imagem = receita.Imagem,
                DataCadastro = receita.DataCadastro
            };
        }
        private static ReceitaDetalhesResponse MapearParaDetalhesResponse(
        Receita receita,
        IEnumerable<Ingrediente> ingredientes,
        IEnumerable<Passo> passos)
        {
            return new ReceitaDetalhesResponse
            {
                Id = receita.Id,
                UsuarioId = receita.UsuarioId,
                CategoriaId = receita.CategoriaId,
                Nome = receita.Nome,
                Descricao = receita.Descricao,
                TempoPreparo = receita.TempoPreparo,
                Porcoes = receita.Porcoes,
                Dificuldade = receita.Dificuldade,
                Imagem = receita.Imagem,
                DataCadastro = receita.DataCadastro,
                Ingredientes = ingredientes
                    .Select(i => new IngredienteResponse { Id = i.Id, Nome = i.Nome, Quantidade = i.Quantidade })
                    .ToList(),
                Passos = passos
                    .Select(p => new PassoResponse { Id = p.Id, Ordem = p.Ordem, Descricao = p.Descricao })
                    .ToList()
            };
        }
    }
}
