using RestSharp;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class ApiUtilities
    {
        public static void ReportDevFeeProcessing(string hiveFlightSheetId, string farmId, string hiveApiKey)
        {
            var flightSheetWallets = HiveUtilities.GetFlightsheetWallets(farmId, hiveFlightSheetId, hiveApiKey);
            if (flightSheetWallets != null && flightSheetWallets.Keys != null && flightSheetWallets.Keys.Count > 0)
            {
                foreach (var item in flightSheetWallets.Keys)
                {
                    var coin = item;
                    var address = flightSheetWallets[coin];

                    ApiDevFeeData requestData = new ApiDevFeeData()
                    {
                        Address = address,
                        EntryDateTime = DateTime.UtcNow,
                        Ticker = coin
                    };

                    var client = new RestClient("https://api.retromike.net/api/MiningTools");
                    var request = new RestRequest();
                    request.AddJsonBody(requestData);
                    client.Post(request);
                }
            }
        }
    }
}
