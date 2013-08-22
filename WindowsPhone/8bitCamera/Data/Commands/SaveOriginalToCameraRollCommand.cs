namespace EightBitCamera.Data.Commands
{
    public class SaveOriginalToCameraRollCommand : SettingsCommand<bool>
    {
        public override void Set(bool value)
        {
            Set(SettingsKeys.SaveOriginalToCameraRoll, value);
        }
    }
}