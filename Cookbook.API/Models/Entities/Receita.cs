using Cookbook.API.Models.Enum;

namespace Cookbook.API.Models.Entities
{
    public class Receita
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int TempoPreparo { get; set; }
        public int Porcoes { get; set; }
        public DificuldadeReceita Dificuldade { get; set; } = DificuldadeReceita.Facil;
        public string? Imagem { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
