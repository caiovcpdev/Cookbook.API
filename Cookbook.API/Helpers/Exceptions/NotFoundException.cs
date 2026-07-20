namespace Cookbook.API.Helpers.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string mensagem) : base(mensagem)
        {
        }
    }
}
