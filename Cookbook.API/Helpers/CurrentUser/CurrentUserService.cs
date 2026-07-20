using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cookbook.API.Helpers.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        public int ObterUsuarioId()
        {
            //var idClaim = _contextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var idClaim = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var usuarioId))
                throw new UnauthorizedAccessException("Não foi possível identificar o usuário autenticado.");
            
            return usuarioId;
        }
    }
}
