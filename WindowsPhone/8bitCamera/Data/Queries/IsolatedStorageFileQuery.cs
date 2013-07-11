using System.IO;
using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Queries
{
    public abstract class IsolatedStorageFileQuery<T>
    {
        public abstract T Get();

        protected string Get(SettingsKeys settingsKey)
        {
            var key = settingsKey.ToString();
            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.FileExists(key))
                {
                    return "";
                }

                using (var stream = file.OpenFile(key, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}