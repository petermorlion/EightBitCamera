namespace EightBitCamera.Data.Commands
{
    public class ReviewedCommand : SettingsCommand<bool>
    {
        public override void Set(bool value)
        {
            Set(SettingsKeys.Reviewed, value);
        }
    }
}