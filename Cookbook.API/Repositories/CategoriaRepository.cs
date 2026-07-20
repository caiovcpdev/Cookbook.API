using Cookbook.API.Data;
using Cookbook.API.Models.Entities;
using Cookbook.API.Repositories.Interfaces;
using Dapper;

namespace Cookbook.API.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CategoriaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task AtualizarAsync(Categoria categoria)
        {
            const string sql = "UPDATE Categorias SET Nome = @Nome WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { categoria.Id, categoria.Nome });
        }

        public async Task<int> CriarAsync(Categoria categoria)
        {
            const string sql = @"
            INSERT INTO Categorias (Nome)
            OUTPUT INSERTED.Id
            VALUES (@Nome);";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleAsync<int>(sql, new { categoria.Nome });
        }

        public async Task ExcluirAsync(int id)
        {
            const string sql = "DELETE FROM Categorias WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Categoria>> ListarAsync()
        {
            const string sql = "SELECT * FROM Categorias ORDER BY Nome;";
            
            using var connection  = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Categoria>(sql);
        }

        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            const string sql = "SELECT * FROM Categorias WHERE Id = @Id";

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<Categoria>(sql, new { Id = id });
        }

        public async Task<Categoria?> ObterPorNomeAsync(string nome)
        {
            const string sql = "SELECT * FROM Categorias WHERE Nome = @Nome;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Categoria>(sql, new { Nome = nome });
        }
    }
}
