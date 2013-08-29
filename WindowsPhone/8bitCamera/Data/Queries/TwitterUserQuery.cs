using System.IO;
using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Queries
{
    public class TwitterUserQuery
    {
        public TwitterUser Get()
        {
            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.FileExists(SettingsKeys.TwitterUser.ToString()))
                {
                    return null;
                }

                using (var stream = file.OpenFile(SettingsKeys.TwitterUser.ToString(), FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return new TwitterUser
                                   {
                                       AccessToken = reader.ReadLine(),
                                       AccessTokenSecret = reader.ReadLine(),
                                       UserId = reader.ReadLine(),
                                       ScreenName = reader.ReadLine()
                                   };
                    }
                }
            }
        }
    }
}