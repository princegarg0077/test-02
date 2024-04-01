using Android.App;
using Android.Content;
using Android.Content.PM;
using Xamarin.Essentials;

[Activity(Name = "com.companyname.linkedin.CallbackActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataSchemes = new[] { "https" },
    DataPath = "/auth/linkedin/callback")]
public class CallbackActivity 
{
}