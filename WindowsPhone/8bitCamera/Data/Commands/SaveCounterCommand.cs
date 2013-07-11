namespace EightBitCamera.Data.Commands
{
    public class SaveCounterCommand : SettingsCommand<int>
    {
        public override void Set(int value)
        {
            Set(SettingsKeys.SaveCounter, value);
        }
    }
}