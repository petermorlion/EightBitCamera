namespace EightBitCamera.Data
{
    public partial class TwitterSettings
    {
        public const string ConsumerKey = "MFszN6wBwlqy8gOS2Tgjw";

        public const string TwitterOAuthUri = "https://api.twitter.com/oauth";
        public const string RequestTokenPath = "/request_token";
        public const string AuthorizePath = "/authorize";
        public const string AccessTokenPath = "/access_token";
        public const string CallbackUri = "http://www.google.com";   // we've mentioned Google.com as our callback URL.
        public const string OAuthVersion = "1.0a";
    }
}