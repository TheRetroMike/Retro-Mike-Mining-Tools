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
            RestClient btcRestClient = new RestClient("https://min-api.cryptocompare.com/data/price?fsym=BTC&tsyms=USD");
            RestRequest btcRestRequest = new RestRequest("");
            dynamic btcMarketData = JsonConvert.DeserializeObject(btcRestClient.Get(btcRestRequest).Content);
            result = Convert.ToDouble(btcMarketData?.USD?.Value ?? 0.01);
            using (var writer = new StreamWriter("CoinDeskBtcPrice.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }           
        }
    }
}
