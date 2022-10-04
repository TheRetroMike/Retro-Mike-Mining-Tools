using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class ZergUtilities
    {
        public static void RefreshZergStatusData()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://zergpool.com/api/status");
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
                        result.Add(new Coin() { Algorithm = algoName, Name = "Zerg-" + algoName, Ticker = "Zerg-" + algoName, BtcRevenue = Convert.ToString(estimate), HashRateFactor = Convert.ToString(mhFactor) });
                    }
                }
            }

            using (var writer = new StreamWriter("ZergStatus.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }
        }

        public static void RefreshZergCurrencyData()
        {
            if (!File.Exists("ZergStatus.dat"))
            {
                RefreshZergStatusData();
            }
            var zergStatus = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZergStatus.dat"));
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://zergpool.com/api/currencies");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);

            foreach (var item in responseContent)
            {
                foreach (var coinData in item)
                {
                    var algoName = coinData.algo.Value;
                    var estimate = coinData.estimate_current.Value;
                    var mhFactor = coinData.mbtc_mh_factor.Value;
                    if (!result.Any(x => x.Algorithm.Equals(algoName)))
                    {
                        if (zergStatus != null && zergStatus.Where(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)).Any())
                        {
                            var algoRecord = zergStatus.Where(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)).First();
                            result.Add(new Coin() { Algorithm = algoName, Name = "Zerg-" + algoName, Ticker = "Zerg-" + algoName, BtcRevenue = Convert.ToString(algoRecord.BtcRevenue), HashRateFactor = Convert.ToString(algoRecord.HashRateFactor) });
                        }
                        else
                        {
                            result.Add(new Coin() { Algorithm = algoName, Name = "Zerg-" + algoName, Ticker = "Zerg-" + algoName, BtcRevenue = Convert.ToString(estimate), HashRateFactor = Convert.ToString(mhFactor) }); ;
                        }
                    }
                }
            }

            using (var writer = new StreamWriter("ZergCurrency.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }
        }

        public static void RefreshZergCoinData()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient("https://zergpool.com/api/currencies");
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
                    var estimate = coinData.estimate_current.Value;
                    var mhFactor = coinData.mbtc_mh_factor.Value;
                    result.Add(new Coin() { Algorithm = algo, Name = String.Format("{0} ({1})", coinName, ticker), Ticker = String.Format("ZergProvider-{0}", ticker), BtcRevenue = Convert.ToString(estimate), HashRateFactor = Convert.ToString(mhFactor) });
                }
            }

            using (var writer = new StreamWriter("ZergCoin.dat", false))
            {
                writer.Write(JsonConvert.SerializeObject(result));
            }
        }

        public static List<Coin> GetZergCoins()
        {
            if (!File.Exists("ZergCoin.dat"))
            {
                RefreshZergCoinData();
            }

            return JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZergCoin.dat"));
        }

        public static List<Coin> GetZergAlgos()
        {
            if (!File.Exists("ZergCurrency.dat"))
            {
                RefreshZergStatusData();
                RefreshZergCurrencyData();
            }

            return JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZergCurrency.dat"));
        }

        public static List<Coin> GetZergAlgos(List<Coin> wtmCoins)
        {
            List<Coin> result = new List<Coin>();
            if (!File.Exists("ZergCurrency.dat"))
            {
                RefreshZergStatusData();
                RefreshZergCurrencyData();
            }
            var data = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZergCurrency.dat"));
            foreach (var item in data)
            {
                if (!result.Any(x => x.Algorithm.Equals(item.Algorithm, StringComparison.OrdinalIgnoreCase)))
                {
                    var wtmCoin = wtmCoins.Where(x => x.Algorithm.Equals(item.Algorithm, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (wtmCoin != null)
                    {
                        result.Add(new Coin() { Algorithm = item.Algorithm, Name = item.Algorithm, Ticker = "Zerg-" + item.Algorithm, PowerConsumption = wtmCoin.PowerConsumption, HashRate = wtmCoin.HashRate });
                    }
                    else
                    {
                        result.Add(new Coin() { Algorithm = item.Algorithm, Name = item.Algorithm, Ticker = "Zerg-" + item.Algorithm });
                    }
                }
            }

            return result;
        }


        public static List<AlgoData> GetZergAlgoData()
        {
            List<AlgoData> result = new List<AlgoData>();
            if (!File.Exists("ZergCurrency.dat"))
            {
                RefreshZergStatusData();
                RefreshZergCurrencyData();
            }

            var zergData = JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("ZergCurrency.dat"));
            foreach (var item in zergData)
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
