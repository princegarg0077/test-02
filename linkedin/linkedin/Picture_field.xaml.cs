using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace linkedin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Picture_field : ContentPage
    {
        private const string ClientId = "86k0w3ugbg1jb1";
        private const string ClientSecret = "Lg4ZNHMwaswhCPKg";
        private const string RedirectUri = Constants.RedirectUri;
        private const string AuthorizationUrl = "https://www.linkedin.com/oauth/v2/authorization";

        public Picture_field()
        {
            InitializeComponent();
        }

        private async void ChoosePictureButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var action = await DisplayActionSheet("Select Image Source", "Cancel", null, "Take Photo", "Choose Photo");

                if (action == "Take Photo")
                {
                    var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                    {
                        Title = "Take a photo"
                    });

                    if (photo != null)
                    {
                        var stream = await photo.OpenReadAsync();
                        ChosenImage.Source = ImageSource.FromStream(() => stream);
                    }
                }
                else if (action == "Choose Photo")
                {
                    var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                    {
                        Title = "Choose a photo"
                    });

                    if (result != null)
                    {
                        ChosenImage.Source = ImageSource.FromStream(() => result.OpenReadAsync().Result);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error picking photo: " + ex.Message);
            }
        }

        private bool _isSubmitting = false;

        private async void Submit_button(object sender, EventArgs e)
        {
            try
            {
                if (_isSubmitting)
                {
                    return;
                }

                _isSubmitting = true;

                var code = await GetAuthorizationCode();

                if (code != null)
                {
                    var accessToken = await GetAccessToken(code);

                    if (accessToken != null)
                    {
                        var caption = CaptionEditor.Text;

                        if (!string.IsNullOrEmpty(caption))
                        {
                            var postData = new Dictionary<string, string>
                    {
                        { "content", caption }
                    };

                            var success = await PostToLinkedIn(accessToken, postData);

                            if (success)
                            {
                                await DisplayAlert("Success", "Post created successfully", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Error", "Failed to create post", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "Caption is empty", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to obtain access token", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Failed to obtain authorization code", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error posting to LinkedIn: {ex.Message}");
            }
            finally
            {
                _isSubmitting = false;
            }
        }



        private async Task<string> GetAuthorizationCode()
        {
            try
            {
                var state = Guid.NewGuid().ToString("N");
                var authorizationUrl = $"{AuthorizationUrl}?response_type=code&client_id={ClientId}&redirect_uri={RedirectUri}";
                var callbackUrl = new Uri(RedirectUri);

                var result = await WebAuthenticator.AuthenticateAsync(new Uri(authorizationUrl), callbackUrl);

                if (result != null && !string.IsNullOrEmpty(result.Properties["code"]))
                {
                    var code = result.Properties["code"];
                    Console.WriteLine($"Authorization code retrieved: {code}");

                    return code;
                }
                else
                {
                    Console.WriteLine("Authorization code not found in authentication result.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting authorization code: {ex.Message}");
                return null;
            }
        }

        private async Task<string> GetAccessToken(string code)
        {
            try
            {
                var client = new HttpClient();

                var requestBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", ClientId),
                    new KeyValuePair<string, string>("client_secret", ClientSecret),
                    new KeyValuePair<string, string>("redirect_uri", RedirectUri)
                });

                var response = await client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", requestBody);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(json);
                return tokenResponse.AccessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting access token: {ex.Message}");
                return null;
            }
        }

        private async Task<string> UploadImageAndGetUrl()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Choose a photo"
                });

                if (result != null)
                {
                    var imageUrl = await UploadImageToServer(result);
                    return imageUrl;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
        }

        private async Task<string> UploadImageToServer(FileResult imageFile)
        {
            try
            {
                if (imageFile != null)
                {
                    var stream = await imageFile.OpenReadAsync();
                    byte[] imageData = new byte[stream.Length];
                    await stream.ReadAsync(imageData, 0, (int)stream.Length);
                    stream.Close();
                    return "https://example.com/uploaded-image.jpg";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image to server: {ex.Message}");
                return null; 
            }
        }

        private async Task<bool> PostToLinkedIn(string accessToken, Dictionary<string, string> postData)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var url = "https://api.linkedin.com/v2/shares";
                var json = JsonConvert.SerializeObject(postData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error posting to LinkedIn: {ex.Message}");
                return false;
            }
        }

        private void TextEditor(object sender, TextChangedEventArgs e)
        {           
            var newText = e.NewTextValue;
        }

        private class AccessTokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }
    }
}
