using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace linkedin.Droid
{
    [Activity(Label = "linkedin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        protected override void OnNewIntent(Android.Content.Intent intent)
        {
            base.OnNewIntent(intent);

            
            if (intent.Data != null)
            {
                HandleLinkedInAuthenticationResult(intent.Data);
            }
        }

        private async void HandleLinkedInAuthenticationResult(Android.Net.Uri uri)
        {
            var code = uri.GetQueryParameter("code");

            if (!string.IsNullOrEmpty(code))
            {
                var accessToken = await GetAccessToken(code);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    
                }
                else
                {
                    
                }
            }
            else
            {
                
            }
        }

        private async Task<string> GetAccessToken(string code)
        {
            

            return ""; 
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
