namespace Cookbook.API.Models.Requests.Passo
{
    public class PassoRequest
    {
        public int Ordem { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
