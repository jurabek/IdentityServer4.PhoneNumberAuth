using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.PhoneNumberAuth.Constants;

namespace IdentityServer4.PhoneNumberAuth.Configuration
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource ("myapi", "My Api") { UserClaims = { JwtClaimTypes.Role, JwtClaimTypes.PhoneNumber } }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "phone_number_authentication",
                    AllowedGrantTypes = new List<string> { AuthConstants.GrantType.PhoneNumberToken},
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "myapi"
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}
