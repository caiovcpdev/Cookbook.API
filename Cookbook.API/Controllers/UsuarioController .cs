using Cookbook.API.Helpers.CurrentUser;
using Cookbook.API.Models.Requests.Usuario;
using Cookbook.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ICurrentUserService _currentUserService;
        public UsuarioController(IUsuarioService usuarioService, ICurrentUserService currentUserService)
        {
            _usuarioService = usuarioService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request)
        {
            var usuario = await _usuarioService.CriarAsync(request);

            return CreatedAtAction(nameof(Criar), new { id = usuario.Id }, usuario);
        }

        [Authorize]
        [HttpGet("perfil")]
        public async Task<IActionResult> ObterPerfil()
        {
            var usuarioId = _currentUserService.ObterUsuarioId();
            var usuario = await _usuarioService.ObterPorIdAsync(usuarioId);

            return Ok(usuario);
        }
    }
}
