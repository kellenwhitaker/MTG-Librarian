
using RestSharp;
using Newtonsoft.Json;
using System;

namespace ThirdPartyAPI
{
    public static class TCGPlayer
    {
        private static string BearerToken = "";

        private static string GetPublicKey()
        {
            return "";
        }

        private static string GetPrivateKey()
        {
            return "";
        }

        public static string GetBearerToken()
        {
            if (BearerToken == "")
            {
                string publicKey = GetPublicKey();
                string privateKey = GetPrivateKey();
                if (publicKey != "" && privateKey != "")
                {
                    var client = new RestClient("https://api.tcgplayer.com");
                    var request = new RestRequest("token", Method.POST);
                    request.AddParameter("grant_type", "client_credentials");
                    request.AddParameter("client_id", publicKey);
                    request.AddParameter("client_secret", privateKey);
                    string responseContent = client.Execute(request).Content;
                    var responseObject = JsonConvert.DeserializeObject<BearerTokenResponseObject>(responseContent);
                    BearerToken = responseObject.access_token;
                }
                else
                    throw new NotImplementedException();
            }
            return BearerToken;
        }

        private class BearerTokenResponseObject
        {
            public string access_token;
            public string token_type;
            public int expires_in;
            public string userName;
            public string issued;
            public string expires;
        }    
    }
}
