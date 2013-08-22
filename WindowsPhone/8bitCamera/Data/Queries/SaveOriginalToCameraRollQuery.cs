namespace EightBitCamera.Data.Queries
{
    public class SaveOriginalToCameraRollQuery : SettingsQuery<bool>
    {
        public override bool Get()
        {
            var settingString = Get(SettingsKeys.SaveOriginalToCameraRoll);
            bool saveToCameraRoll;
            if (!bool.TryParse(settingString, out saveToCameraRoll))
            {
                return false;
            }

            return saveToCameraRoll;
        }
    }
}