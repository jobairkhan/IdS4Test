using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityProvider
{
    public static class InMemoryConfiguration
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser{ SubjectId = "6ed26693-b0a1-497e-aa14-7b880536920f", Username = "orders.tatum@gmail.com", Password = "mypassword",
                    Claims = new List<Claim>
                    {
                        new Claim("family_name", "tatum")
                    }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("teachers_test_planner", "Mein Testplaner")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentyResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "teachers_test_client",
                    ClientSecrets = new[]{ new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new []{ "teachers_test_planner" }
                }
            };
        }
    }
}
