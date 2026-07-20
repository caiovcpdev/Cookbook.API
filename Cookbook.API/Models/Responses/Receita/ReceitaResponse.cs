namespace Cookbook.API.Models.Responses.Receita
{
    public class ReceitaResponse
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int TempoPreparo { get; set; }
        public int Porcoes { get; set; }
        public string Dificuldade { get; set; } = string.Empty;
        public string? Imagem { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
