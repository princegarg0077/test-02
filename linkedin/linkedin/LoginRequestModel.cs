using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace linkedin
{
    public class LoginRequestModel
    {
        [JsonProperty("grant_type")]
        public string Grant_type { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("client_id")]
        public string Client_id { get; set; }

        [JsonProperty("client_secret")]
        public string Client_secret { get; set; }

        [JsonProperty("redirect_uri")]
        public string Redirect_uri { get; set; }
    }

    public class AccessTokenResponse
    {
        public string Access_token { get; set; }
    
    }
}
