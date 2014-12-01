using Microsoft.Phone.Marketplace;

namespace EightBitCamera.Data.Queries
{
    public class ApplicationModeQuery : SettingsQuery<ApplicationMode>
    {
        public override ApplicationMode Get()
        {
#if DEBUG
            return ApplicationMode.Ads;
#endif
            var isTrial =  new LicenseInformation().IsTrial();
            var saveCounter = new SaveCounterQuery().Get();

            if (!isTrial)
            {
                return ApplicationMode.Full;
            }
            else
            {
                if (saveCounter > 10)
                {
                    return ApplicationMode.Ads;
                }
            }

            return ApplicationMode.Trial;
        }
    }
}