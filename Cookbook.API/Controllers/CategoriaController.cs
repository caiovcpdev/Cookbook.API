using Cookbook.API.Models.Requests.Categoria;
using Cookbook.API.Repositories;
using Cookbook.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;   
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var categorias = await _categoriaService.ListarAsync();
            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            return Ok(categoria);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarCategoriaRequest request)
        {
            var categoria = await _categoriaService.CriarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = categoria.Id }, categoria);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarCategoriaRequest request)
        {
            await _categoriaService.AtualizarAsync(id, request);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            await _categoriaService.ExcluirAsync(id);
            return NoContent();
        }
    }
}
