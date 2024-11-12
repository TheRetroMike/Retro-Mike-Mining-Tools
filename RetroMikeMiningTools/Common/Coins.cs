using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.Common
{
    public static class Coins
    {
        public static List<Coin> ZergAlgoList
        {
            get
            {
                var result = new List<Coin>();
                result = result.Concat(ZergUtilities.GetZergAlgos()).ToList();
                result.Add(new Coin() { Algorithm="k12", Name="k12" });
                return result;
            }
        }

        public static List<Coin> CoinList
        {
            get
            {
                List<Coin> result = new List<Coin>();
                result.AddRange(ZergUtilities.GetZergAlgos());
                result.AddRange(ZpoolUtilities.GetAlgos());
                result.AddRange(ProhashingUtilities.GetAlgos());
                result.AddRange(MiningDutchUtilities.GetAlgos());
                result.AddRange(UnmineableUtilities.GetAlgos());

                foreach (Coin coin in WhatToMineUtilities.GetCoinList("https://whattomine.com/"))
                {
                    var existingRecord = result.Where(x => x.Ticker.Equals(coin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingRecord == null)
                    {
                        result.Add(coin);
                    }
                }

                foreach (Coin coin in WhatToMineUtilities.GetCoinList("https://whattomine.com/asic"))
                {
                    var existingRecord = result.Where(x => x.Ticker.Equals(coin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingRecord == null)
                    {
                        result.Add(coin);
                    }
                }

                foreach (Coin coin in WhatToMineUtilities.GetIndividualCoinList())
                {
                    var existingRecord = result.Where(x => x.Ticker.Equals(coin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingRecord == null)
                    {
                        result.Add(coin);
                    }
                }

                foreach (Coin coin in ZergUtilities.GetZergCoins())
                {
                    if (coin.Ticker != null)
                    {
                        var tickerKey = coin.Ticker.Replace("ZergProvider-", String.Empty);
                        var existingRecord = result.Where(x => x.Ticker.Equals(tickerKey, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (existingRecord == null)
                        {
                            result.Add(coin);
                        }
                    }
                }

                result = result.OrderBy(x => x.Name).ToList();
                return result.Distinct().ToList();
            }
        }
    }
}
