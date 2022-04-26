using System.Text;
using Microsoft.IdentityModel.Tokens;
using VideoTranslate.Shared.Abstractions.Providers;

namespace VideoTranslate.WebAPI.Infrastructure.Providers
{
    public class JwtSigningKeyProvider : IJwtSigningKeyProvider
    {
        public SymmetricSecurityKey GetSymmetricSecurityKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}
