using Cookbook.API.Data;
using Cookbook.API.Repositories.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IUsuarioRepository _usuarioRepository;

    public HealthController(IDbConnectionFactory connectionFactory, IUsuarioRepository usuarioRepository)
    {
        _connectionFactory = connectionFactory;
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet("db")]
    public async Task<IActionResult> CheckDatabase()
    {
        using var connection = _connectionFactory.CreateConnection();

        var result = await connection.QuerySingleAsync<int>("SELECT 1");

        return Ok(new { status = "ok", result });
    }
}