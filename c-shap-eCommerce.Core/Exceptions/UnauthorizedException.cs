namespace c_shap_eCommerce.Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public static int StatusCode = 401;
        public UnauthorizedException(string message) : base(message)
        {

        }
    }
}