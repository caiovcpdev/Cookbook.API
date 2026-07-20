namespace Cookbook.API.Models.Entities
{
    public class Passo
    {
        public int Id { get; set; }
        public int ReceitaId { get; set; }
        public int Ordem { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
