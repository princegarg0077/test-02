using Newtonsoft.Json.Linq;
using Xamarin.Auth;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace linkedin
{
    public partial class linkedinHM : ContentPage
    {
        OAuth2Authenticator authenticator;
        string authorizationCode;
        public string Authcode;

        public linkedinHM()
        {
            InitializeComponent();
            webView.Navigated += webView_Navigated;
        }

        private void Login_button(object sender, EventArgs e)
        {
            webView.Source = "";
            webView.Source = "https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=86k0w3ugbg1jb1&redirect_uri=https://dev.example.com/auth/linkedin/callback&state=foobar&scope=r_basicprofile%20w_member_social";
        }

        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            try
            {
                string url = e.Url;
                string baseUrl = "https://dev.example.com/auth/linkedin/callback?code=";
                if (url.StartsWith(baseUrl))
                {
                    int codeIndex = url.IndexOf("code=") + "code=".Length;
                    int endIndex = url.IndexOf("&", codeIndex);
                    if (endIndex == -1)
                        endIndex = url.Length;

                    string code = url.Substring(codeIndex, endIndex - codeIndex);
                    Authcode = code;
                    webView.IsVisible = false;
                    await FetchLinkedInApi(code);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error navigating webview: " + ex.Message);
            }
        }

        public LoginRequestModel CreateLoginRequest()
        {
            var data = new LoginRequestModel();
            data.Client_id = "861skvw1er3nqc";
            data.Client_secret = "AKOxaMj9veAwA3eJ";
            data.Grant_type = "authorization_code";
            data.Code = Authcode;
            return data;
        }

        private async Task FetchLinkedInApi(string code)
        {
            try
            {
                var client = new HttpClient();
                var url = $"https://www.linkedin.com/oauth/v2/accessToken?code={code}&grant_type=authorization_code&client_id=86k0w3ugbg1jb1&client_secret=Lg4ZNHMwaswhCPKg&redirect_uri=https://dev.example.com/auth/linkedin/callback";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var response = await client.SendAsync(request);

               //var json=JsonConvert.SerializeObject(response);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching LinkedIn API: " + ex.Message);
            }
        }

        //private async Task FetchUserProfile(string accessToken)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        //        var url = "https://randomuser.me/api";
        //        var response = await client.GetAsync(url);
        //        response.EnsureSuccessStatusCode();

        //        var content = await response.Content.ReadAsStringAsync();
        //        var userProfile = JObject.Parse(content);


        //        Console.WriteLine("User Profile:");
        //        Console.WriteLine(userProfile);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error fetching user profile: " + ex.Message);
        //    }
        //}
        private async void webView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            try
            {
                string url = e.Url;
                string baseUrl = "https://dev.example.com/auth/linkedin/callback?code=";
                if (url.StartsWith(baseUrl))
                {
                    int codeIndex = url.IndexOf("code=") + "code=".Length;
                    int endIndex = url.IndexOf("&", codeIndex);
                    if (endIndex == -1)
                        endIndex = url.Length;

                    string code = url.Substring(codeIndex, endIndex - codeIndex);
                    Authcode = code;
                    
                    await FetchLinkedInApi(code);

                    await Navigation.PushAsync(page: new LinkedInProfilePage());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error navigating webview: " + ex.Message);
            }
        }


        private void Picture_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Picture_field());

        }

        
        public async Task FetchUserProfile(string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var url = "https://randomuser.me/api"; 
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var userProfile = JObject.Parse(content);

                Console.WriteLine("User Profile:");
                Console.WriteLine(userProfile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching user profile: " + ex.Message);
            }
        }
        public async Task<string> FetchAccessToken(string code)
        {
            try
            {
                var client = new HttpClient();
                var url = $"https://www.linkedin.com/oauth/v2/accessToken?code={code}&grant_type=authorization_code&client_id=86k0w3ugbg1jb1&client_secret=Lg4ZNHMwaswhCPKg&redirect_uri=https://dev.example.com/auth/linkedin/callback";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var accessToken = JObject.Parse(content)["access_token"].ToString();
                return accessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching access token: {ex.Message}");
                return null;
            }
        }



    }
}
