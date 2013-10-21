namespace EightBitCamera.Data.Queries
{
    public class LaunchCountQuery : SettingsQuery<int>
    {
        public override int Get()
        {
            var settingString = Get(SettingsKeys.LaunchCount);
            int setting;
            if (!int.TryParse(settingString, out setting))
            {
                return 0;
            }

            return 9;
        }
    }
}