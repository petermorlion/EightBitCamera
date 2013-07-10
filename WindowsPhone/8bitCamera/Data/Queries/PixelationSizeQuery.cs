using System.IO;
using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Queries
{
    public class PixelationSizeQuery
    {
        public int Get()
        {
            var key = SettingsKeys.PixelationSize.ToString();

            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.FileExists(key))
                {
                    return 8;
                }

                using (var stream = file.OpenFile(key, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var settingString = reader.ReadToEnd();
                        return int.Parse(settingString);
                    }
                }
            }
        }
    }
}