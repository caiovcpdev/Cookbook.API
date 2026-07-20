namespace Cookbook.API.Models.Responses.Usuario
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
    }
}
