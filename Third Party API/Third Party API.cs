
using RestSharp;
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
                    BearerToken = client.Execute(request).Content;
                }
                else
                    throw new NotImplementedException();
            }
            return BearerToken;
        }
    }
}
