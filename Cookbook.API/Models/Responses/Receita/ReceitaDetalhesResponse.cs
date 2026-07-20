using Cookbook.API.Models.Enum;
using Cookbook.API.Models.Responses.Ingrediente;
using Cookbook.API.Models.Responses.Passo;

namespace Cookbook.API.Models.Responses.Receita
{
    public class ReceitaDetalhesResponse
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
        public List<IngredienteResponse> Ingredientes { get; set; } = new();
        public List<PassoResponse> Passos { get; set; } = new();
    }
}
