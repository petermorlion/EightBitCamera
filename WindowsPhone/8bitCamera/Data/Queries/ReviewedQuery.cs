namespace EightBitCamera.Data.Queries
{
    public class ReviewedQuery : SettingsQuery<bool>
    {
        public override bool Get()
        {
            var settingString = Get(SettingsKeys.Reviewed);
            bool setting;
            if (!bool.TryParse(settingString, out setting))
            {
                return false;
            }

            return setting;
        }
    }
}