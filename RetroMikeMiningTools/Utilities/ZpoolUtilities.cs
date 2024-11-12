using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class ZpoolUtilities
    {
        public static void RefreshStatusData()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://www.zpool.ca/api/status");
            var request = new RestRequest();
            var response = client.Get(request);
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
                        result.Add(new Coin() { Algorithm = algoName, Name = "Zpool-" + algoName, Ticker = "Zpool-" + algoName, BtcRevenue = Convert.ToString(estimate), HashRateFactor = Convert.ToString(mhFactor) });
                    }
                }
            }

            using (var writer = new StreamWriter("ZpoolStatus.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }
        }

        public static void RefreshCurrencyData()
        {
            if (!File.Exists("ZpoolStatus.dat"))
            {
                RefreshStatusData();
            }
            var zpoolStatus = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZpoolStatus.dat"));
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://www.zpool.ca/api/currencies");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);

            foreach (var item in responseContent)
            {
                foreach (var coinData in item)
                {
                    var algoName = coinData.algo.Value;
                    var estimate = coinData.estimate.Value;
                    var mhFactor = coinData.mbtc_mh_factor.Value;
                    if (!result.Any(x => x.Algorithm.Equals(algoName)))
                    {
                        if (zpoolStatus != null && zpoolStatus.Where(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)).Any())
                        {
                            var algoRecord = zpoolStatus.Where(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)).First();
                            result.Add(new Coin() { Algorithm = algoName, Name = "Zpool-" + algoName, Ticker = "Zpool-" + algoName, BtcRevenue = Convert.ToString(algoRecord.BtcRevenue), HashRateFactor = Convert.ToString(algoRecord.HashRateFactor) });
                        }
                        else
                        {
                            result.Add(new Coin() { Algorithm = algoName, Name = "Zpool-" + algoName, Ticker = "Zpool-" + algoName, BtcRevenue = Convert.ToString(estimate), HashRateFactor = Convert.ToString(mhFactor) }); ;
                        }
                    }
                }
            }

            using (var writer = new StreamWriter("ZpoolCurrency.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }
        }

        public static void RefreshCoinData()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://www.zpool.ca/api/currencies");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);

            foreach (var item in responseContent)
            {
                var ticker = item.Name;
                foreach (var coinData in item)
                {
                    var coinName = coinData.name.Value;
                    var algo = coinData.algo.Value;
                    var estimate = coinData.estimate.Value;
                    var mhFactor = coinData.mbtc_mh_factor.Value;
                    result.Add(new Coin() { Algorithm = algo, Name = String.Format("{0} ({1})", coinName, ticker), Ticker = String.Format("ZpoolProvider-{0}", ticker), BtcRevenue = Convert.ToString(estimate), HashRateFactor = Convert.ToString(mhFactor) });
                }
            }

            using (var writer = new StreamWriter("ZpoolCoin.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }
        }

        public static List<Coin> GetCoins()
        {
            if (!File.Exists("ZpoolCoin.dat"))
            {
                RefreshCoinData();
            }

            return JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZpoolCoin.dat"));
        }

        public static List<Coin> GetAlgos()
        {
            if (!File.Exists("ZpoolCurrency.dat"))
            {
                RefreshStatusData();
                RefreshCurrencyData();
            }

            return JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZpoolCurrency.dat"));
        }

        public static List<Coin> GetAlgos(List<Coin> wtmCoins)
        {
            List<Coin> result = new List<Coin>();
            if (!File.Exists("ZpoolCurrency.dat"))
            {
                RefreshStatusData();
                RefreshCurrencyData();
            }
            var data = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZpoolCurrency.dat"));
            foreach (var item in data)
            {
                if (!result.Any(x => x.Algorithm.Equals(item.Algorithm, StringComparison.OrdinalIgnoreCase)))
                {
                    var wtmCoin = wtmCoins.Where(x => x.Algorithm.Equals(item.Algorithm, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (wtmCoin != null)
                    {
                        result.Add(new Coin() { Algorithm = item.Algorithm, Name = item.Algorithm, Ticker = "Zpool-" + item.Algorithm, PowerConsumption = wtmCoin.PowerConsumption, HashRate = wtmCoin.HashRate });
                    }
                    else
                    {
                        result.Add(new Coin() { Algorithm = item.Algorithm, Name = item.Algorithm, Ticker = "Zpool-" + item.Algorithm });
                    }
                }
            }

            return result;
        }


        public static List<AlgoData> GetAlgoData()
        {
            List<AlgoData> result = new List<AlgoData>();
            if (!File.Exists("ZpoolCurrency.dat"))
            {
                RefreshStatusData();
                RefreshCurrencyData();
            }

            var zpoolData = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZpoolCurrency.dat"));
            foreach (var item in zpoolData)
            {
                var existingCoin = result.Where(x => x.Algo.Equals(item.Algorithm, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (existingCoin != null)
                {
                    var existingCalculatedRevenue = Convert.ToDecimal(existingCoin.Estimate) / Convert.ToDecimal(existingCoin.MhFactor);
                    var newCalculatedRevenue = Convert.ToDecimal(item.BtcRevenue) / Convert.ToDecimal(item.HashRateFactor);
                    if (newCalculatedRevenue > existingCalculatedRevenue)
                    {
                        result.Remove(existingCoin);
                        result.Add(new AlgoData() { Algo = item.Algorithm, Estimate = item.BtcRevenue, MhFactor = item.HashRateFactor });
                    }
                }
                else
                {
                    result.Add(new AlgoData() { Algo = item.Algorithm, Estimate = item.BtcRevenue, MhFactor = item.HashRateFactor });
                }
            }
            return result;
        }
    }
}
