using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class ProhashingUtilities
    {
        public static void RefreshProhashingData()
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
                    var estimate = coinData.estimate_current.Value;
                    var factor = coinData.mbtc_mh_factor.Value;
                    if (!result.Any(x => x.Algorithm.Equals(algoName)))
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = "Prohashing-" + algoName, Ticker = "Prohashing-" + algoName, BtcRevenue = estimate, HashRateFactor = factor });
                    }
                }
            }

            using (var writer = new StreamWriter("Prohashing.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }

        }

        public static List<Coin> GetAlgos()
        {
            if (!File.Exists("Prohashing.dat"))
            {
                RefreshProhashingData();
            }

            return JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("Prohashing.dat"));
        }

        public static List<Coin> GetAlgos(List<Coin> wtmCoins)
        {
            List<Coin> result = new List<Coin>();
            var data = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("Prohashing.dat"));

            foreach (var item in data)
            {
                if (!result.Any(x => x.Algorithm.Equals(item.Algorithm, StringComparison.OrdinalIgnoreCase)))
                {
                    var wtmCoin = wtmCoins.Where(x => x.Algorithm.Equals(item.Algorithm, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (wtmCoin != null)
                    {
                        result.Add(new Coin() { Algorithm = item.Algorithm, Name = item.Algorithm, Ticker = "Prohashing-" + item.Algorithm, PowerConsumption = wtmCoin.PowerConsumption, HashRate = wtmCoin.HashRate });
                    }
                    else
                    {
                        result.Add(new Coin() { Algorithm = item.Algorithm, Name = item.Algorithm, Ticker = "Prohashing-" + item.Algorithm });
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
            var data = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("Prohashing.dat"));

            foreach (var item in data)
            {
                if (!item.Algorithm.Equals("token", StringComparison.OrdinalIgnoreCase))
                {
                    var estimate = item.BtcRevenue;
                    var mhFactor = item.HashRateFactor;

                    var existingCoin = result.Where(x => x.Algo.Equals(item.Algorithm, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingCoin != null)
                    {
                        var existingCalculatedRevenue = Convert.ToDecimal(existingCoin.Estimate);
                        var newCalculatedRevenue = Convert.ToDecimal(estimate);
                        if (newCalculatedRevenue > existingCalculatedRevenue)
                        {
                            result.Remove(existingCoin);
                            result.Add(new AlgoData() { Algo = item.Algorithm, Estimate = Convert.ToString(estimate), MhFactor = Convert.ToString(mhFactor) });
                        }
                    }
                    else
                    {
                        result.Add(new AlgoData() { Algo = Convert.ToString(item.Algorithm), Estimate = Convert.ToString(estimate), MhFactor = Convert.ToString(mhFactor) });
                    }
                }
            }
            return result;
        }
    }
}