namespace EightBitCamera.Data
{
    public partial class TwitterSettings
    {
        public const string ConsumerKey = "MFszN6wBwlqy8gOS2Tgjw";

        public const string RequestTokenUri = "https://api.twitter.com/oauth/request_token";
        public const string AuthorizeUri = "https://api.twitter.com/oauth/authorize";
        public const string AccessTokenUri = "https://api.twitter.com/oauth/access_token";
        public const string CallbackUri = "http://www.google.com";   // we've mentioned Google.com as our callback URL.
        public const string OAuthVersion = "1.0a";
    }
}