namespace EightBitCamera.Data.Commands
{
    public class SaveToCameraRollCommand : SettingsCommand<bool>
    {
        public override void Set(bool value)
        {
            Set(SettingsKeys.SaveToCameraRoll, value);
        }
    }
}