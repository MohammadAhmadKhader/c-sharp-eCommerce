namespace c_sharp_eCommerce.Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public static int StatusCode = 401;
        public UnauthorizedException(string message) : base(message)
        {

        }
    }
}
