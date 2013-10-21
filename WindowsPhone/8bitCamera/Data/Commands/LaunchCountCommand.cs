namespace EightBitCamera.Data.Commands
{
    public class LaunchCountCommand : SettingsCommand<int>
    {
        public override void Set(int value)
        {
            Set(SettingsKeys.LaunchCount, value);
        }
    }
}