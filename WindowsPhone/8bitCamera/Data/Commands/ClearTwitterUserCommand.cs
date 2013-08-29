using System.IO.IsolatedStorage;

namespace EightBitCamera.Data.Commands
{
    public class ClearTwitterUserCommand
    {
        public void Execute()
        {
            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                file.DeleteFile(SettingsKeys.TwitterUser.ToString());
            }
        }
    }
}