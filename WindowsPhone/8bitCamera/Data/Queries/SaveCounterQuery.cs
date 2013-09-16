namespace EightBitCamera.Data.Queries
{
    public class SaveCounterQuery : SettingsQuery<int>
    {
        public override int Get()
        {
            int value;
            if (!int.TryParse(Get(SettingsKeys.SaveCounter), out value))
            {
                return 0;
            }

            return value;
        }
    }
}