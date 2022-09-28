using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DAO;
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
                        result.Add(new Coin() { Algorithm = algoName, Name = "Zerg-" + algoName, Ticker = "Zerg-" + algoName });
                    }
                }
            }
            return result;
        }

        public static List<Coin> GetZergAlgos(List<Coin> wtmCoins)
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
                    if (!result.Any(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)))
                    {
                        var wtmCoin = wtmCoins.Where(x => x.Algorithm.Equals(algoName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (wtmCoin != null)
                        {
                            result.Add(new Coin() { Algorithm = algoName, Name = algoName, Ticker = "Zerg-" + algoName, PowerConsumption = wtmCoin.PowerConsumption, HashRate = wtmCoin.HashRate });
                        }
                        else
                        {
                            result.Add(new Coin() { Algorithm = algoName, Name = algoName, Ticker = "Zerg-" + algoName });
                        }
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

        public static decimal GetProjectedProfitability(string algo, decimal mhHashrate, decimal power)
        {
            decimal result = 0;
            var btcPrice = CoinDeskUtilities.GetBtcPrice();
            var config = CoreConfigDAO.GetCoreConfig();
            var algoData = GetZergAlgoData().Where(x => x.Algo.Equals(algo)).FirstOrDefault();
            if (algoData != null)
            {
                var mBtcPerMhAmount = Convert.ToDecimal(algoData.Estimate) / (Convert.ToDecimal(algoData.MhFactor) / 1000);
                var mBtcRevenue = mhHashrate * mBtcPerMhAmount;
                var btcRevenue = mBtcRevenue / 1000;

                decimal dailyPowerCost = 24 * (Convert.ToDecimal(power) / 1000m) * Convert.ToDecimal(config?.DefaultPowerPrice ?? 0.10m);
                decimal dailyRevenue = Convert.ToDecimal(btcRevenue) * Convert.ToDecimal(btcPrice);
                decimal dailyProfit = dailyRevenue - dailyPowerCost;

                result = dailyProfit;
            }
            return result;
        }
    }
}
