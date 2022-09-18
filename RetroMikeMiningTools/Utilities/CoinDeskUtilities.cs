using Newtonsoft.Json;
using RetroMikeMiningTools.DAO;
using RestSharp;

namespace RetroMikeMiningTools.Utilities
{
    public static class CoinDeskUtilities
    {
        public static double GetBtcPrice()
        {
            var result = 0.00;
            var coreConfig = CoreConfigDAO.GetCoreConfig();
            if (!String.IsNullOrEmpty(coreConfig.CoinDeskApi))
            {
                RestClient btcRestClient = new RestClient(coreConfig.CoinDeskApi);
                RestRequest btcRestRequest = new RestRequest("");
                dynamic btcMarketData = JsonConvert.DeserializeObject(btcRestClient.Get(btcRestRequest).Content);
                result = Convert.ToDouble(btcMarketData?.bpi?.USD?.rate?.Value ?? 0.01);
            }
            return result;
        }
    }
}
