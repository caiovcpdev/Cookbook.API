using Cookbook.API.Models.Enum;
using Cookbook.API.Models.Requests.Ingrediente;
using Cookbook.API.Models.Requests.Passo;

namespace Cookbook.API.Models.Requests.Receita
{
    public class CriarReceitaRequest
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int TempoPreparo { get; set; }
        public int Porcoes { get; set; }
        public DificuldadeReceita Dificuldade { get; set; } = DificuldadeReceita.Facil;
        public string? Imagem { get; set; }
        public List<IngredienteRequest> Ingredientes { get; set; } = new();
        public List<PassoRequest> Passos { get; set; } = new();
    }
}
