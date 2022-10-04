using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class MiningDutchUtilities
    {
        public static void RefreshMiningDutchData()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://www.mining-dutch.nl/api/status/");
            var request = new RestRequest();
            var response = client.Get(request);
            if (response.Content == "{\"message\":\"Only 1 request every 10 seconds allowed\"}")
            {
                return;
            }
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);

            foreach (var item in responseContent)
            {
                foreach (var coinData in item)
                {
                    var algoName = coinData.name.Value;
                    var estimate = coinData.estimate_current.Value;
                    var mhFactor = coinData.mbtc_mh_factor.Value;
                    if (!result.Any(x => x.Algorithm.Equals(algoName)))
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = "MiningDutch-" + algoName, Ticker = "MiningDutch-" + algoName, BtcRevenue = Convert.ToString(estimate), HashRateFactor = Convert.ToString(mhFactor) });
                    }
                }
            }

            using (var writer = new StreamWriter("MiningDutch.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }

        }

        public static List<Coin> GetAlgos()
        {
            if (!File.Exists("MiningDutch.dat"))
            {
                RefreshMiningDutchData();
            }

            return JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("MiningDutch.dat"));
        }

        public static List<Coin> GetAlgos(List<Coin> wtmCoins)
        {
            List<Coin> result = new List<Coin>();
            if (!File.Exists("MiningDutch.dat"))
            {
                RefreshMiningDutchData();
            }
            var coins = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("MiningDutch.dat"));
            foreach (var item in coins)
            {
                var algoName = item.Algorithm;
                if (!result.Any(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)))
                {
                    var wtmCoin = wtmCoins.Where(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (wtmCoin != null)
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = algoName, Ticker = "MiningDutch-" + algoName, PowerConsumption = wtmCoin.PowerConsumption, HashRate = wtmCoin.HashRate });
                    }
                    else
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = algoName, Ticker = "MiningDutch-" + algoName });
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
            if (!File.Exists("MiningDutch.dat"))
            {
                RefreshMiningDutchData();
            }
            var coins = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("MiningDutch.dat"));
            foreach (var item in coins)
            {
                string algoName = item.Algorithm;
                if (!algoName.Equals("token", StringComparison.OrdinalIgnoreCase))
                {
                    var estimate = item.BtcRevenue;
                    var mhFactor = item.HashRateFactor;

                    var existingCoin = result.Where(x => x.Algo.Equals(algoName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingCoin != null)
                    {
                        var existingCalculatedRevenue = Convert.ToDecimal(existingCoin.Estimate);
                        var newCalculatedRevenue = Convert.ToDecimal(estimate);
                        if (newCalculatedRevenue > existingCalculatedRevenue)
                        {
                            result.Remove(existingCoin);
                            result.Add(new AlgoData() { Algo = algoName, Estimate = Convert.ToString(estimate), MhFactor = Convert.ToString(mhFactor) });
                        }
                    }
                    else
                    {
                        result.Add(new AlgoData() { Algo = Convert.ToString(algoName), Estimate = Convert.ToString(estimate), MhFactor = Convert.ToString(mhFactor) });
                    }
                }
            }
           
            return result;
        }
    }
}
