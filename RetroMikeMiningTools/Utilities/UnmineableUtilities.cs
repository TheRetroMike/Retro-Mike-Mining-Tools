using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class UnmineableUtilities
    {
        public static void RefreshPoolData()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://api.unminable.com/v4/pool");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent.data != null && responseContent.data.pools != null)
            {
                foreach (var item in responseContent.data.pools)
                {
                    string algoName = Convert.ToString(item.Name);

                    var marketRateClient = new RestClient("https://api.unminable.com/v3/calculate/reward");
                    var marketRateRequest = new RestRequest("");
                    marketRateRequest.AddParameter("algo", algoName);
                    marketRateRequest.AddParameter("coin", "BTC");
                    marketRateRequest.AddParameter("mh", 1000000000000); //always getting rate in TH/S because unminable returns 0 for low hashrate calcs
                    var marketRateResponse = marketRateClient.Post(marketRateRequest);
                    dynamic marketRateResponseContent = JsonConvert.DeserializeObject(marketRateResponse.Content);
                    var estimate = "0.00"; //TODO: additional post call to calculate estimate
                    if (marketRateResponseContent != null)
                    {
                        estimate = Convert.ToString(Convert.ToDecimal(marketRateResponseContent.per_day)/ 1000000000000);
                    }

                    
                    var mhFactor = 1;
                    if (!result.Any(x => x.Algorithm.Equals(algoName)))
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = "Unmineable-" + algoName, Ticker = "Unmineable-" + algoName, BtcRevenue = Convert.ToString(estimate), HashRateFactor = Convert.ToString(mhFactor) });
                    }
                }
            }

            using (var writer = new StreamWriter("Unmineable.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }

        }

        public static List<Coin> GetAlgos()
        {
            if (!File.Exists("Unmineable.dat"))
            {
                RefreshPoolData();
            }

            return JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("Unmineable.dat"));
        }

        public static List<Coin> GetAlgos(List<Coin> wtmCoins)
        {
            List<Coin> result = new List<Coin>();
            if (!File.Exists("Unmineable.dat"))
            {
                RefreshPoolData();
            }
            var coins = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("Unmineable.dat"));
            foreach (var item in coins)
            {
                var algoName = item.Algorithm;
                if (!result.Any(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)))
                {
                    var wtmCoin = wtmCoins.Where(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (wtmCoin != null)
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = algoName, Ticker = "Unmineable-" + algoName, PowerConsumption = wtmCoin.PowerConsumption, HashRate = wtmCoin.HashRate });
                    }
                    else
                    {
                        result.Add(new Coin() { Algorithm = algoName, Name = algoName, Ticker = "Unmineable-" + algoName });
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
            if (!File.Exists("Unmineable.dat"))
            {
                RefreshPoolData();
            }
            var coins = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("Unmineable.dat"));
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
