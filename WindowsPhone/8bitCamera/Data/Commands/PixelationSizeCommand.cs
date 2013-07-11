namespace EightBitCamera.Data.Commands
{
    public class PixelationSizeCommand : SettingsCommand<int>
    {
        public override void Set(int value)
        {
            Set(SettingsKeys.PixelationSize, value);
        }
    }
}