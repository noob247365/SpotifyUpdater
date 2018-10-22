using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Spotify.Helpers
{
    public static class Account
    {
        public static void Login(this Controller controller, string callbackCode)
        {
            // Get the auth and refresh tokens
            var tokenRequest = new JObject()
            {
                { "grant_type", "authorization_code" },
                { "code", callbackCode },
                { "redirect_uri", Credentials.RedirectUri }
            };
            var tokenResponse = WebClient.PostJson("https://accounts.spotify.com/api/token", tokenRequest, Credentials.ClientAuth).Result;

            // Check for error
            if (tokenResponse.TryGetValue("error", out JToken error))
            {
                string errorDescription;
                if (tokenResponse.TryGetValue("error_description", out JToken error_description))
                    errorDescription = error_description.ToString();
                else
                    errorDescription = "Unknown error";
                throw new Exception($"Unable to login ({error.ToString()}): {errorDescription}");
            }

            // Ensure the requested properties are present
            if (!tokenResponse.TryGetValue("access_token", out JToken access_token) ||
                    !tokenResponse.TryGetValue("refresh_token", out JToken refresh_token))
                throw new Exception($"Unable to login (missing information): {tokenResponse}");
            long expiresIn = 3600;
            if (tokenResponse.TryGetValue("expires_in", out JToken expires_in))
                expiresIn = expires_in.ToObject<long>();

            // Setup the cookie
            var accessTokenOptions = new CookieOptions();
            accessTokenOptions.Expires = DateTime.Now + TimeSpan.FromSeconds(expiresIn / 2);
            accessTokenOptions.HttpOnly = false;
            accessTokenOptions.Secure = true;
            controller.HttpContext.Response.Cookies.Append("access_token", access_token.ToString(), accessTokenOptions);

            // Setup the session
            controller.HttpContext.Session.SetString("access_token", access_token.ToString());
            controller.HttpContext.Session.SetString("expire_time", DateTime.Now + TimeSpan.FromSeconds(expiresIn).ToString("O"));
            controller.HttpContext.Session.SetString("refresh_token", refresh_token.ToString());
        }
    }
}