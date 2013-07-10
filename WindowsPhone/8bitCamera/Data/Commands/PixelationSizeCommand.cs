using System.IO;
using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Commands
{
    public class PixelationSizeCommand
    {
        public void Set(int value)
        {
            var key = SettingsKeys.PixelationSize.ToString();

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