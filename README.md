# IdS4Test

## Token from identity server connection\token is not valid for my API

I am getting a token from my identityserver 4 via url connection/token with a POST request:

Then I copy/paste the value of the access_token key to my API GET request as a header:

**mytokenstring**

eyJhbGciOiJSUzI1NiIsImtpZCI6IkYxMDhCODA2NUNFMTRBOEEwOTZBODUyMkIxQUNBMkFDMTdEQjQwNEEiLCJ0eXAiOiJKV1QiLCJ4NXQiOiI4UWk0Qmx6aFNvb0phb1Vpc2F5aXJCZmJRRW8ifQ.eyJuYmYiOjE1MDg1OTU5MzIsImV4cCI6MTUwODU5OTUzMiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo2NTUzNSIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjY1NTM1L3Jlc291cmNlcyIsInRlYWNoZXJzX3Rlc3RfcGxhbm5lciJdLCJjbGllbnRfaWQiOiJ0ZWFjaGVyc190ZXN0X3BsYW5uZXIiLCJzY29wZSI6WyJ0ZWFjaGVyc190ZXN0X3BsYW5uZXIiXX0.g2x31JcYrXyIavfxCu7UKY3kndznI_gYHJYCxl0dQn3u7l7vWo6qKr13XYMo6P1Lqtu68T2FEXL-5kyS0XwFClpdJE6m13-hfKZsd2QHBmOlgZ2ANwghXW4hfU5nWiwkUACwkP9wfDCULV3oQm5i49L5TQmUiiqcy0TTS2FDBdS5ymFBi1bCKnPh5ErsD8V_4eTqLzxv8CyVkPx2gPd6aBIf_2JNrjrMrrm69kghOHnktVG17KPQhppbIeJO8RP-URiJUJGXIY09yRGVF7YXtkFj-I5QOMvNIAWgUeqNYqH0cuQol9nglA4mtU1MfXtnRoEpRRzGViw7gxJ_-MFadA

Authorization: Bearer **mytokenstring**

What can cause that the token from the identity server is not valid for my API?

I get a 401 error with **POSTMAN**

Looking into the output of the kestrel server I get this:

    Api> fail: Microsoft.AspNetCore.Server.IISIntegration.IISMiddleware[0]
    Api>       'MS-ASPNETCORE-TOKEN' does not match the expected pairing token '52da49ee-6599-483a-b97a-15ced1603005', request rejected.

What do I wrong and what pairing token Guid is that?

 
**API HttpGet:**

header:

Authorization Bearer eyJh...UntilTheEndOfTheString

IdentityServer setup:

    public void ConfigureServices(IServiceCollection services)
    {
        string certificateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certifiateselfsigned.pfx");
    
        var certificate = new X509Certificate2(certificateFilePath, "test");
    
        services.AddIdentityServer()
        .AddSigningCredential(certificate)
            .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
            .AddInMemoryClients(InMemoryConfiguration.GetClients())
            .AddTestUsers(InMemoryConfiguration.GetUsers());
    
        services.AddMvc();
    }

**UPDATE**

  **Api**

    public void ConfigureServices(IServiceCollection services)
    {
    	services.AddAuthentication(options =>
    	{
    		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    	   
    		})
    			.AddIdentityServerAuthentication(opt =>
    		   {
    			   opt.RequireHttpsMetadata = false;
    			   opt.Authority = "http://localhost:65535"; // IdentityProvider => port running IDP on
    			   opt.ApiName = "teachers_test_planner"; // IdentityProvider => api resource name
    		   });
    
    	services.AddMvc();
    }

**IdentityProvider**

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
                ClientId = "teachers_test_planner",
                ClientSecrets = new[]{ new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedScopes = new []{ "teachers_test_planner" }
            }
        };
        }
    }

**UPDATE 2**

You can find the test project here:

https://github.com/LisaTatum/IdentityServer4Test

**UPDATE 3**

As nobody asked how I do the http_post to the connect/token endpoint here it is:

[![enter image description here][1]][1]


  [1]: https://i.stack.imgur.com/JevOd.png



