namespace EightBitCamera.Data.Commands
{
    public class SaveLocationCommand : SettingsCommand<SaveLocations>
    {
        public override void Set(SaveLocations value)
        {
            Set(SettingsKeys.SaveLocation, value);
        }
    }
}