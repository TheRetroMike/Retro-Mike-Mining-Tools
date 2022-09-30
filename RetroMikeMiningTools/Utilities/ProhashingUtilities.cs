using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class ProhashingUtilities
    {
        public static List<Coin> GetAlgos()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://prohashing.com/api/v1/status");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseContent.data)
            {
                foreach (var coinData in item)
                {
                    var algoName = coinData.name.Value;
                    if (!result.Any(x => x.Algorithm.Equals(algoName)))
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = "Prohashing-" + algoName, Ticker = "Prohashing-" + algoName });
                    }
                }
            }
            return result;
        }

        public static List<Coin> GetAlgos(List<Coin> wtmCoins)
        {
            List<Coin> result = new List<Coin>();
            var client = new RestSharp.RestClient("https://prohashing.com/api/v1/status");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseContent)
            {
                foreach (var coinData in item)
                {
                    var algoName = coinData.name.Value;
                    if (!result.Any(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)))
                    {
                        var wtmCoin = wtmCoins.Where(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (wtmCoin != null)
                        {
                            result.Add(new Coin() { Algorithm = algoName, Name = algoName, Ticker = "Prohashing-" + algoName, PowerConsumption = wtmCoin.PowerConsumption, HashRate = wtmCoin.HashRate });
                        }
                        else
                        {
                            result.Add(new Coin() { Algorithm = algoName, Name = algoName, Ticker = "Prohashing-" + algoName });
                        }
                    }
                }
            }
            return result;
        }

        public static List<AlgoData> GetAlgoData()
        {
            try
            {
                return GetAlgoDataContent();
            }
            catch
            {
                try
                {
                    return GetAlgoDataContent();
                }
                catch { }
            }
            return new List<AlgoData>();
        }

        public static List<AlgoData> GetAlgoDataContent()
        {
            List<AlgoData> result = new List<AlgoData>();
            var client = new RestSharp.RestClient("https://prohashing.com/api/v1/status");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseContent.data)
            {
                foreach (var coinData in item)
                {
                    string algoName = coinData.name.Value;
                    if (!algoName.Equals("token", StringComparison.OrdinalIgnoreCase))
                    {
                        var estimate = coinData.estimate_current.Value;
                        var mhFactor = coinData.mbtc_mh_factor.Value;

                        var existingCoin = result.Where(x => x.Algo.Equals(algoName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (existingCoin != null)
                        {
                            var existingCalculatedRevenue = Convert.ToDecimal(existingCoin.Estimate);
                            var newCalculatedRevenue = Convert.ToDecimal(estimate);
                            if (newCalculatedRevenue > existingCalculatedRevenue)
                            {
                                result.Remove(existingCoin);
                                result.Add(new AlgoData() { Algo = algoName, Estimate = Convert.ToDecimal(estimate), MhFactor = mhFactor });
                            }
                        }
                        else
                        {
                            result.Add(new AlgoData() { Algo = algoName, Estimate = Convert.ToDecimal(estimate), MhFactor = mhFactor });
                        }
                    }
                }
            }
            return result;
        }
    }
}
