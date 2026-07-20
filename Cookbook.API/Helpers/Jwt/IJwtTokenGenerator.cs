using Cookbook.API.Models.Entities;

namespace Cookbook.API.Helpers.Jwt
{
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime ExpiraEm) GerarToken(Usuario usuario);
    }
}
