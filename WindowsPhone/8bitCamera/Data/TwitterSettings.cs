namespace EightBitCamera.Data
{
    public class TwitterSettings
    {
        public const string ConsumerKey = "0Z5ENxmtKve6taZPtog2Q";
        public const string ConsumerSecret = "aXdNTUWAa7ELvG3A3NtkNBEaei2zX2iP9p7WRri8k";
        
        public const string RequestTokenUri = "https://api.twitter.com/oauth/request_token";
        public const string AuthorizeUri = "https://api.twitter.com/oauth/authorize";
        public const string AccessTokenUri = "https://api.twitter.com/oauth/access_token";
        public const string CallbackUri = "http://www.google.com";   // we've mentioned Google.com as our callback URL.
        public const string OAuthVersion = "1.0a";
    }
}