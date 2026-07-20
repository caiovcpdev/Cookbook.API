namespace Cookbook.API.Models.Responses.Usuario
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
