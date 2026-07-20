using Cookbook.API.Models.Requests.Usuario;
using Cookbook.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _usuarioService.LoginAsync(request);
            return Ok(response);
        }
    }
}
