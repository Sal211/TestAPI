namespace WebApplication1.Models
{
    public class StaticToken
    {
        private static string user = "Sal";
        private static string pasw = "123";
        public static bool Token(string param)
        {
            string userpassword = user + ":" + pasw;
            return (param == userpassword) ? true : false;
        }
    }
}
