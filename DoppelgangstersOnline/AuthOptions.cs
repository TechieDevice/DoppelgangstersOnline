using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DoppelgangstersOnline
{
    public class AuthOptions
    {
        public const string ISSUER = "DoppelgangersServer";
        public const string AUDIENCE = "DoppelgangersClient";
        const string KEY = "mysupersecret_secretkey!123"; 
        public const int LIFETIME = 120;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
