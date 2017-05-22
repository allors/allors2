namespace Allors.Server
{
    using System;

    using Microsoft.IdentityModel.Tokens;

    public interface IAuthenticationContext 
    {
        string Issuer { get; }

        string Audience { get; }

        RsaSecurityKey Key { get; }

        DateTime? Expires();
    }
}

