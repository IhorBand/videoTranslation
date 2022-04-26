using Microsoft.IdentityModel.Tokens;

namespace VideoTranslate.Shared.Abstractions.Providers
{
    public interface IJwtSigningKeyProvider
    {
        SymmetricSecurityKey GetSymmetricSecurityKey(string secret);
    }
}
