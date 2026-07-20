namespace Cookbook.API.Models.Entities
{
    public class Ingrediente
    {
        public int Id { get; set; }
        public int ReceitaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Quantidade { get; set; } = string.Empty;
    }
}
