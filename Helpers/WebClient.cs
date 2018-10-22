using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Spotify.Helpers
{
    public static class WebClient
    {
        public static readonly HttpClient Client = new HttpClient();

        public static async Task<JObject> PostJson(string url, JObject body, AuthenticationHeaderValue authenticationHeader = null)
        {
            // Setup response type
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Setup request body
            var requestContent = new List<KeyValuePair<string, string>>();
            if (body != null && body.HasValues)
                foreach (var entry in body)
                    requestContent.Add(new KeyValuePair<string, string>(entry.Key, entry.Value.ToString()));

            // Add authorization header
            if (authenticationHeader != null)
                Client.DefaultRequestHeaders.Authorization = authenticationHeader;
            
            using (var response = await Client.PostAsync(url, new FormUrlEncodedContent(requestContent)))
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (!responseContent.StartsWith("{"))
                    throw new Exception($"Unable to parse respons as JSON:{Environment.NewLine}{responseContent}");
                return JObject.Parse(responseContent);
            }
        }
    }
}