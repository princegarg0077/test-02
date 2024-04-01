using Xamarin.Forms;

namespace linkedin
{
    public class LinkedInProfilePage : ContentPage
    {
        public LinkedInProfilePage()
        //{
        //    var linkedInProfilePage = new LinkedInProfilePage("https://www.linkedin.com/in/example");
        //    Navigation.PushAsync(linkedInProfilePage);

        //}
        {
            
            HandleLinkedInLoginSuccess();
        }
        private void HandleLinkedInLoginSuccess()
        {
           
            var pictureFieldPage = new Picture_field();
            Navigation.PushAsync(pictureFieldPage);
        }
        public LinkedInProfilePage(string profileUrl)
        {
            var webView = new WebView
            {
                Source = new UrlWebViewSource { Url = profileUrl },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

           
            webView.Navigating += (sender, e) =>
            {
                if (e.Url.StartsWith("https://www.linkedin.com"))
                {
                    
                    return;
                }
                
                e.Cancel = true;
              
                webView.Source = e.Url;
            };

            Content = webView;
        }

    }
}
