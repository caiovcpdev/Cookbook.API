using Cookbook.API.Models.Requests.Ingrediente;
using Cookbook.API.Models.Requests.Passo;
using Cookbook.API.Models.Requests.Receita;
using Cookbook.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceitaController : ControllerBase
    {
        private readonly IReceitaService _receitaService;
        public ReceitaController(IReceitaService receitaService )
        {
            _receitaService = receitaService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var receitas = await _receitaService.ListarAsync();
            return Ok(receitas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            //var receita = await _receitaService.ObterPorIdAsync(id);
            var receita = await _receitaService.ObterDetalhesAsync(id);
            return Ok(receita);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarReceitaRequest request)
        {
            var receita = await _receitaService.CriarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = receita.Id }, receita);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarReceitaRequest request)
        {
            await _receitaService.AtualizarAsync(id, request);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            await _receitaService.ExcluirAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{receitaId:int}/ingredientes")]
        public async Task<IActionResult> AdicionarIngrediente(int receitaId, [FromBody] IngredienteRequest request)
        {
            var ingrediente = await _receitaService.AdicionarIngredienteAsync(receitaId, request);
            return CreatedAtAction(nameof(ObterPorId), new { id = receitaId }, ingrediente);
        }

        [Authorize]
        [HttpDelete("{receitaId:int}/ingredientes/{ingredienteId:int}")]
        public async Task<IActionResult> RemoverIngrediente(int receitaId, int ingredienteId)
        {
            await _receitaService.RemoverIngredienteAsync(receitaId, ingredienteId);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{receitaId:int}/passos")]
        public async Task<IActionResult> AdicionarPasso(int receitaId, [FromBody] PassoRequest request)
        {
            var passo = await _receitaService.AdicionarPassoAsync(receitaId, request);
            return CreatedAtAction(nameof(ObterPorId), new { id = receitaId }, passo);
        }

        [Authorize]
        [HttpDelete("{receitaId:int}/passos/{passoId:int}")]
        public async Task<IActionResult> RemoverPasso(int receitaId, int passoId)
        {
            await _receitaService.RemoverPassoAsync(receitaId, passoId);
            return NoContent();
        }
    }
}
