using Newtonsoft.Json;
using RetroMikeMiningTools.DAO;
using RestSharp;
using RetroMikeMiningTools.Common;

namespace RetroMikeMiningTools.Utilities
{
    public static class CoinDeskUtilities
    {
        public static double GetBtcPrice()
        {
            if (!File.Exists("CoinDeskBtcPrice.dat"))
            {
                RefreshBtcPrice();
            }
            return JsonConvert.DeserializeObject<Double>(File.ReadAllText("CoinDeskBtcPrice.dat"));
        }

        public static void RefreshBtcPrice()
        {
            var result = 0.00;
            var coreConfig = CoreConfigDAO.GetCoreConfig();
            RestClient btcRestClient = new RestClient(Constants.COINDESK_API);
            RestRequest btcRestRequest = new RestRequest("");
            dynamic btcMarketData = JsonConvert.DeserializeObject(btcRestClient.Get(btcRestRequest).Content);
            result = Convert.ToDouble(btcMarketData?.bpi?.USD?.rate?.Value ?? 0.01);
            using (var writer = new StreamWriter("CoinDeskBtcPrice.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }           
        }
    }
}
