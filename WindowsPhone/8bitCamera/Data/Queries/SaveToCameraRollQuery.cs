namespace EightBitCamera.Data.Queries
{
    public class SaveToCameraRollQuery : SettingsQuery<bool>
    {
        public override bool Get()
        {
            var settingString = Get(SettingsKeys.SaveToCameraRoll);
            bool saveToCameraRoll;
            if (!bool.TryParse(settingString, out saveToCameraRoll))
            {
                return false;
            }

            return saveToCameraRoll;
        }
    }
}