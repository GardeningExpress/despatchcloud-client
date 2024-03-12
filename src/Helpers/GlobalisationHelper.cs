
using System.Globalization;

namespace GardeningExpress.DespatchCloudClient.Helpers
{
    public static class GlobalisationHelper
    {

        public static string GetCountryNameFromIsoTwoCode(string isoTwoCode)
        {
            try
            {
                var regionInfo = new RegionInfo(isoTwoCode);
                string countryName = regionInfo.EnglishName;
                return countryName;
            }
            catch
            {
                return null;
            }
        }
    }

}
