using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class ZergUtilities
    {
        public static List<Coin> GetZergAlgos()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestSharp.RestClient("https://zergpool.com/api/currencies");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseContent)
            {
                var coinContainerName = item.Name;
                foreach (var coinData in item)
                {
                    var algoName = coinData.algo.Value;
                    if (!result.Any(x => x.Algorithm.Equals(algoName)))
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = algoName });
                    }
                }
            }
            return result;
        }

        public static List<ZergAlgo> GetZergAlgoData()
        {
            List<ZergAlgo> result = new List<ZergAlgo>();
            var client = new RestSharp.RestClient("https://zergpool.com/api/currencies");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseContent)
            {
                var coinContainerName = item.Name;
                foreach (var coinData in item)
                {
                    string algoName = coinData.algo.Value;
                    if (!algoName.Equals("token", StringComparison.OrdinalIgnoreCase))
                    {
                        var estimate = coinData.estimate_current.Value;
                        var mhFactor = coinData.mbtc_mh_factor.Value;

                        var existingCoin = result.Where(x => x.Algo.Equals(algoName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (existingCoin != null)
                        {
                            var existingCalculatedRevenue = Convert.ToDecimal(existingCoin.Estimate) / Convert.ToDecimal(existingCoin.MhFactor);
                            var newCalculatedRevenue = Convert.ToDecimal(estimate) / Convert.ToDecimal(mhFactor);
                            if (newCalculatedRevenue > existingCalculatedRevenue)
                            {
                                result.Remove(existingCoin);
                                result.Add(new ZergAlgo() { Algo = algoName, Estimate = estimate, MhFactor = mhFactor });
                            }
                        }
                        else
                        {
                            result.Add(new ZergAlgo() { Algo = algoName, Estimate = estimate, MhFactor = mhFactor });
                        }
                    }
                }
            }
            return result;
        }
    }
}
