using System.IO;
using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Commands
{
    public class TwitterUserCommand
    {
        public void Set(TwitterUser twitterUser)
        {
            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = file.OpenFile(SettingsKeys.TwitterUser.ToString(), FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(twitterUser.AccessToken);
                        writer.WriteLine(twitterUser.AccessTokenSecret);
                        writer.WriteLine(twitterUser.UserId);
                        writer.WriteLine(twitterUser.ScreenName);
                    }
                }
            }
        }
    }
}