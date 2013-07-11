using System.IO;
using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Commands
{
    public abstract class SettingsCommand<T>
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
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;

                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }

                    }
                }
            }
        }
    }
}