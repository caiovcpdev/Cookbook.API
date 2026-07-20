namespace Cookbook.API.Helpers.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string mensagem) : base(mensagem)
        {
        }
    }
}
