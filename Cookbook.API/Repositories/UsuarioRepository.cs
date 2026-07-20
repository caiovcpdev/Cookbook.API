using Cookbook.API.Data;
using Cookbook.API.Models.Entities;
using Cookbook.API.Repositories.Interfaces;
using Dapper;

namespace Cookbook.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UsuarioRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<int> CriarAsync(Usuario usuario)
        {
            const string sql = @"
            INSERT INTO Usuarios (Nome, Email, SenhaHash, FotoPerfil, DataCadastro)
            OUTPUT INSERTED.Id
            VALUES (@Nome, @Email, @SenhaHash, @FotoPerfil, @DataCadastro);";

            using var connection = _connectionFactory.CreateConnection();

            var novoId = await connection.QuerySingleAsync<int>(sql, new
            {
                usuario.Nome,
                usuario.Email,
                usuario.SenhaHash,
                usuario.FotoPerfil,
                usuario.DataCadastro
            });

            return novoId;
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            const string sql = "SELECT * FROM Usuarios WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<Usuario>(sql, new { Id = id });
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            const string sql = "SELECT * FROM Usuarios WHERE Email = @Email;";

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<Usuario>(sql, new { Email = email });
        }
    }
}
