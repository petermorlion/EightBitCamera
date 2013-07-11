namespace EightBitCamera.Data.Commands
{
    public class PixelationSizeCommand : IsolatedStorageFileCommand<int>
    {
        public override void Set(int value)
        {
            Set(SettingsKeys.PixelationSize, value);
        }
    }
}