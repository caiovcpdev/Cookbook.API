using Cookbook.API.Data;
using Cookbook.API.Models.Entities;
using Cookbook.API.Repositories.Interfaces;
using Dapper;

namespace Cookbook.API.Repositories
{
    public class ReceitaRepository : IReceitaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public ReceitaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AtualizarAsync(Receita receita)
        {
            const string sql = @"
                UPDATE Receitas
                SET CategoriaId = @CategoriaId,
                    Nome = @Nome,
                    Descricao = @Descricao,
                    TempoPreparo = @TempoPreparo,
                    Porcoes = @Porcoes,
                    Dificuldade = @Dificuldade,
                    Imagem = @Imagem
                WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(sql, new
            {
                receita.Id,
                receita.CategoriaId,
                receita.Nome,
                receita.Descricao,
                receita.TempoPreparo,
                receita.Porcoes,
                receita.Dificuldade,
                receita.Imagem
            });
        }
        public async Task<bool> CategoriaExisteAsync(int categoriaId)
        {
            const string sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM Categorias WHERE Id = @CategoriaId) THEN 1 ELSE 0 END;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleAsync<bool>(sql, new { CategoriaId = categoriaId });
        }
        public async Task<int> CriarAsync(Receita receita)
        {
            const string sql = @"
                INSERT INTO Receitas
                    (UsuarioId, CategoriaId, Nome, Descricao, TempoPreparo, Porcoes, Dificuldade, Imagem, DataCadastro)
                OUTPUT INSERTED.Id
                VALUES
                    (@UsuarioId, @CategoriaId, @Nome, @Descricao, @TempoPreparo, @Porcoes, @Dificuldade, @Imagem, @DataCadastro);";

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleAsync<int>(sql, new
            {
                receita.UsuarioId,
                receita.CategoriaId,
                receita.Nome,
                receita.Descricao,
                receita.TempoPreparo,
                receita.Porcoes,
                receita.Dificuldade,
                receita.Imagem,
                receita.DataCadastro
            });
        }
        public async Task<int> CriarComIngredientesEPassosAsync(Receita receita, List<Ingrediente> ingredientes, List<Passo> passos)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                const string sqlReceita = @"
                    INSERT INTO Receitas
                        (UsuarioId, CategoriaId, Nome, Descricao, TempoPreparo, Porcoes, Dificuldade, Imagem, DataCadastro)
                    OUTPUT INSERTED.Id
                    VALUES
                        (@UsuarioId, @CategoriaId, @Nome, @Descricao, @TempoPreparo, @Porcoes, @Dificuldade, @Imagem, @DataCadastro);";

                var receitaId = await connection.QuerySingleAsync<int>(sqlReceita, new
                {
                    receita.UsuarioId,
                    receita.CategoriaId,
                    receita.Nome,
                    receita.Descricao,
                    receita.TempoPreparo,
                    receita.Porcoes,
                    receita.Dificuldade,
                    receita.Imagem,
                    receita.DataCadastro
                }, transaction);

                const string sqlIngrediente = @"
                    INSERT INTO Ingredientes (ReceitaId, Nome, Quantidade)
                    VALUES (@ReceitaId, @Nome, @Quantidade);";

                foreach(var ingrediente in ingredientes)
                {
                    ingrediente.ReceitaId = receitaId;
                    await connection.ExecuteAsync(sqlIngrediente, ingrediente, transaction);
                }

                const string sqlPasso = @"
                    INSERT INTO Passos (ReceitaId, Ordem, Descricao)
                    VALUES (@ReceitaId, @Ordem, @Descricao);";

                foreach (var passo in passos)
                {
                    passo.ReceitaId = receitaId;
                    await connection.ExecuteAsync(sqlPasso, passo, transaction);
                }

                transaction.Commit();

                return receitaId;
            }
            catch 
            { 
                transaction.Rollback(); 
                throw;  
            }
        }
        public async Task ExcluirAsync(int id)
        {
            const string sql = "DELETE FROM Receitas WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }
        public async Task<IEnumerable<Receita>> ListarAsync()
        {
            const string sql = "SELECT * FROM Receitas ORDER BY DataCadastro DESC;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Receita>(sql);
        }
        public async Task<Receita?> ObterPorIdAsync(int id)
        {
            const string sql = "SELECT * FROM Receitas WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Receita>(sql, new { Id = id });
        }
        public async Task<(Receita? Receita, IEnumerable<Ingrediente> Ingredientes, IEnumerable<Passo> Passos)> ObterDetalhadoPorIdAsync(int id)
        {
            const string sql = @"
            SELECT * FROM Receitas WHERE Id = @Id;
            SELECT * FROM Ingredientes WHERE ReceitaId = @Id;
            SELECT * FROM Passos WHERE ReceitaId = @Id ORDER BY Ordem;";

            using var connection = _connectionFactory.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });

            var receita = await multi.ReadSingleOrDefaultAsync<Receita>();
            var ingredientes = await multi.ReadAsync<Ingrediente>();
            var passos = await multi.ReadAsync<Passo>();

            return (receita, ingredientes, passos);
        }

        public async Task AdicionarIngredienteAsync(Ingrediente ingrediente)
        {
            const string sql = @"
                INSERT INTO Ingredientes (ReceitaId, Nome, Quantidade)
                OUTPUT INSERTED.Id
                VALUES (@ReceitaId, @Nome, @Quantidade);";

            using var connection = _connectionFactory.CreateConnection();

            ingrediente.Id = await connection.QuerySingleAsync<int>(sql, new
            {
                ingrediente.ReceitaId,
                ingrediente.Nome,
                ingrediente.Quantidade
            });
        }      
        public async Task<Ingrediente?> ObterIngredientePorIdAsync(int id)
        {
            const string sql = "SELECT * FROM Ingredientes WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Ingrediente>(sql, new { Id = id });
        }
        public async Task RemoverIngredienteAsync(int id)
        {
            const string sql = "DELETE FROM Ingredientes WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task AdicionarPassoAsync(Passo passo)
        {
            const string sql = @"
                INSERT INTO Passos (ReceitaId, Ordem, Descricao)
                OUTPUT INSERTED.Id
                VALUES (@ReceitaId, @Ordem, @Descricao);";

            using var connection = _connectionFactory.CreateConnection();

            passo.Id = await connection.QuerySingleAsync<int>(sql, new
            {
                passo.ReceitaId,
                passo.Ordem,
                passo.Descricao
            });
        }
        public async Task<Passo?> ObterPassoPorIdAsync(int id)
        {
            const string sql = "SELECT * FROM Passos WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Passo>(sql, new { Id = id });
        }
        public async Task RemoverPassoAsync(int id)
        {
            const string sql = "DELETE FROM Passos WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
