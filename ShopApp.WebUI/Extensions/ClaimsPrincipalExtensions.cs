using System.Security.Claims;

namespace ShopApp.WebUI.Extensions
{
    // Cookie'de tutulan verilerin kontrollerini yapmak için kullanılacak olan metotların toplanacağı class.
    public static class ClaimsPrincipalExtensions
    {
        // this -> bu sayede metot sondan çağırılır.
        // User.IsLogger() tarzında.

        public static bool IsLogged(this ClaimsPrincipal user)
        {
            if (user.Claims.FirstOrDefault(x => x.Type == "id") != null)
                return true;
            else
                return false;
        }

        public static string GetUserFirstName(this ClaimsPrincipal user) 
        {
            return user.Claims.FirstOrDefault(x => x.Type == "firstName")?.Value;
        }

        public static string GetUserLastName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "lastName")?.Value;
        }
    }
}
