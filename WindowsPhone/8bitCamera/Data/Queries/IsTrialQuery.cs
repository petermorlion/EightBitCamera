using Microsoft.Phone.Marketplace;

namespace EightBitCamera.Data.Queries
{
    public class IsTrialQuery : SettingsQuery<bool>
    {
        public override bool Get()
        {
#if DEBUG
            return true;
#endif
            return new LicenseInformation().IsTrial();
        }
    }
}