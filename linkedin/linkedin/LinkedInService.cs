using System.Threading.Tasks;
using System;
using Xamarin.Essentials;

public class LinkedInService
{
    private const string ClientId = "your-linkedin-client-id";
    private const string RedirectUri = "https://localhost/callback";
    private const string Scope = "r_liteprofile r_emailaddress";

    public async Task<string> LoginAndGetAccessToken()
    {
        var authUrl = $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={ClientId}&redirect_uri={RedirectUri}&scope={Scope}";

        var result = await WebAuthenticator.AuthenticateAsync(new Uri(authUrl), new Uri(RedirectUri));

        return result?.AccessToken;
    }


}
