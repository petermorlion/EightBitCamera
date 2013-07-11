namespace EightBitCamera.Data.Commands
{
    public class SaveLocationCommand : IsolatedStorageFileCommand<SaveLocations>
    {
        public override void Set(SaveLocations value)
        {
            Set(SettingsKeys.SaveLocation, value);
        }
    }
}