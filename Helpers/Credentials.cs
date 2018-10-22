using System;
using System.Net.Http.Headers;
using System.Text;

namespace Spotify.Helpers
{
    public static class Credentials
    {
        public static readonly string RedirectUri = "https://localhost:5001/Home/Callback";
        public static readonly string ClientId = "8ad6eff46429447490ecd14a8b20b7b9";
        public static AuthenticationHeaderValue ClientAuth => new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:8ba4ed3964e943538d9540c66906c0b2")));
    }
}