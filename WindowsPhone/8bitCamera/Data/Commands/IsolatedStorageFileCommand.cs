using System.IO;
using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Commands
{
    public abstract class IsolatedStorageFileCommand<T>
    {
        public abstract void Set(T value);

        protected void Set(SettingsKeys settingsKey, T value)
        {
            var key = settingsKey.ToString();

            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = file.OpenFile(key, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(value);
                    }
                }
            }
        }
    }
}