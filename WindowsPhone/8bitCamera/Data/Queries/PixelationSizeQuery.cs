namespace EightBitCamera.Data.Queries
{
    public class PixelationSizeQuery : IsolatedStorageFileQuery<int>
    {
        public override int Get()
        {
            var settingString = Get(SettingsKeys.PixelationSize);
            int setting;
            if (!int.TryParse(settingString, out setting))
            {
                return 6;
            }

            return setting;
        }
    }
}