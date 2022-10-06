using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Utilities;
using System.Web;

namespace RetroMikeMiningTools.DAO
{
    public static class GoldshellAsicCoinDAO
    {
        private static readonly string tableName = "GoldshellAsicCoinConfig";

        public static void AddRecord(GoldshellAsicCoinConfig coinConfig)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<GoldshellAsicCoinConfig>(tableName);
                table.Insert(new GoldshellAsicCoinConfig()
                {
                    Ticker = coinConfig.Ticker,
                    Enabled = coinConfig.Enabled,
                    WorkerId = coinConfig.WorkerId,
                    Groups = coinConfig.Groups,
                    MergedTickers = coinConfig.MergedTickers,
                    CoinWhatToMineEndpoint = coinConfig.CoinWhatToMineEndpoint,
                    PoolPassword = coinConfig.PoolPassword,
                    PoolUrl = coinConfig.PoolUrl,
                    PoolUser = coinConfig.PoolUser,
                    CoinAlgo = coinConfig.CoinAlgo,
                    Algo = coinConfig.Algo,
                    HashRateMH = coinConfig.HashRateMH,
                    OverrideEndpoint = coinConfig.OverrideEndpoint,
                    Power = coinConfig.Power,
                    Profit = coinConfig.Profit,
                    SecondaryAlgo = coinConfig.SecondaryAlgo,
                    SecondaryHashRateMH = coinConfig.SecondaryHashRateMH,
                    SecondaryOverrideEndpoint = coinConfig.SecondaryOverrideEndpoint,
                    SecondaryTicker = coinConfig.SecondaryTicker,
                    PrimaryProvider = coinConfig.PrimaryProvider,
                    SecondaryProvider = coinConfig.SecondaryProvider
                });
            }
        }

        public static GoldshellAsicCoinConfig? GetRecord(int id)
        {
            GoldshellAsicCoinConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GoldshellAsicCoinConfig>(tableName);
                result = table.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static List<GoldshellAsicCoinConfig> GetRecords(int workerId, bool isUi, bool forceProfit = false)
        {
            List<GoldshellAsicCoinConfig> result = new List<GoldshellAsicCoinConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GoldshellAsicCoinConfig>(tableName);
                result = table.FindAll().Where(x => x.WorkerId == workerId).ToList();
                if (result != null)
                {
                    var config = CoreConfigDAO.GetCoreConfig();
                    if (!isUi || (isUi && config.UiCoinPriceCalculation) || forceProfit)
                    {
                        var btcPrice = CoinDeskUtilities.GetBtcPrice();
                        List<Coin> wtmCoins = new List<Coin>();

                        decimal powerPrice = config.DefaultPowerPrice;
                        var wtmEndPoint = GoldshellAsicDAO.GetRecord(workerId)?.WhatToMineEndpoint;
                        if (wtmEndPoint != null)
                        {
                            wtmCoins = WhatToMineUtilities.GetCoinList(wtmEndPoint);
                        }
                        var zergAlgoData = ZergUtilities.GetZergAlgoData();
                        var zergCoinData = ZergUtilities.GetZergCoins();
                        var proHashingAlgoData = ProhashingUtilities.GetAlgoData();
                        var miningDutchAlgoData = MiningDutchUtilities.GetAlgoData();
                        foreach (var item in result)
                        {
                            var combinedRevenue = 0.00m;

                            //Primary Coin
                            var primaryRevenue = 0.00m;
                            if (
                                !String.IsNullOrEmpty(wtmEndPoint) && 
                                !item.Ticker.StartsWith("Zerg-") && 
                                !item.Ticker.StartsWith("Prohashing-") &&
                                !item.Ticker.StartsWith("MiningDutch-") &&
                                (item.PrimaryProvider==null || !item.PrimaryProvider.Equals("ZergProvider", StringComparison.OrdinalIgnoreCase))
                                )
                            {
                                var coin = wtmCoins.Where(x => x.Ticker.Equals(item.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                if (coin != null)
                                {
                                    powerPrice = Convert.ToDecimal(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndPoint)).Query).Get("factor[cost]"));
                                    foreach (var grouping in item.Groups)
                                    {
                                        var groupingRecord = MiningGroupDAO.GetRecord(grouping.Id);
                                        if (groupingRecord != null)
                                        {
                                            if (groupingRecord.PowerCost != null)
                                            {
                                                powerPrice = Convert.ToDecimal(groupingRecord.PowerCost);
                                            }
                                        }
                                    }
                                    primaryRevenue = Convert.ToDecimal(coin.BtcRevenue);
                                    if (!String.IsNullOrEmpty(coin.HashRate))
                                    {
                                        item.HashRateMH = decimal.Parse(coin.HashRate, System.Globalization.NumberStyles.Float);
                                    }

                                    if (!String.IsNullOrEmpty(coin.PowerConsumption) && item.Power == 0)
                                    {
                                        item.Power = Convert.ToDecimal(coin.PowerConsumption);
                                    }
                                }
                            }
                            else if (item.Ticker.StartsWith("Zerg-"))
                            {
                                var algo = zergAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                if (algo != null)
                                {
                                    var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate) / (Convert.ToDecimal(algo.MhFactor) / 1000);
                                    var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                                    var btcRevenue = mBtcRevenue / 1000;
                                    primaryRevenue = btcRevenue;
                                }
                            }
                            else if (item.PrimaryProvider != null && item.PrimaryProvider.Equals("ZergProvider", StringComparison.OrdinalIgnoreCase))
                            {
                                var zergCoin = zergCoinData.Where(x => x.Ticker.Equals(item.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                if (zergCoin != null)
                                {
                                    var mBtcPerMhAmount = Convert.ToDecimal(zergCoin.BtcRevenue) / (Convert.ToDecimal(zergCoin.HashRateFactor) / 1000);
                                    var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                                    var btcRevenue = mBtcRevenue / 1000;
                                    primaryRevenue = btcRevenue;
                                }
                            }
                            else if (item.Ticker.StartsWith("Prohashing-"))
                            {
                                var algo = proHashingAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                if (algo != null)
                                {
                                    var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate);
                                    var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                                    var btcRevenue = mBtcRevenue;
                                    primaryRevenue = btcRevenue;
                                }
                            }
                            else if (item.Ticker.StartsWith("MiningDutch-"))
                            {
                                var algo = miningDutchAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                if (algo != null)
                                {
                                    var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate);
                                    var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                                    var btcRevenue = mBtcRevenue / 1000;
                                    primaryRevenue = btcRevenue;
                                }
                            }

                            if (!String.IsNullOrEmpty(item.OverrideEndpoint))
                            {
                                var coin = WhatToMineUtilities.GetIndividualCoin(item.OverrideEndpoint);
                                if (coin != null)
                                {
                                    foreach (var grouping in item.Groups)
                                    {
                                        var groupingRecord = MiningGroupDAO.GetRecord(grouping.Id);
                                        if (groupingRecord != null)
                                        {
                                            if (groupingRecord.PowerCost != null)
                                            {
                                                powerPrice = Convert.ToDecimal(groupingRecord.PowerCost);
                                            }
                                        }
                                    }

                                    primaryRevenue = Convert.ToDecimal(coin.BtcRevenue);
                                    if (!String.IsNullOrEmpty(coin.HashRate))
                                    {
                                        item.HashRateMH = decimal.Parse(coin.HashRate, System.Globalization.NumberStyles.Float);
                                    }
                                }
                            }

                            combinedRevenue += primaryRevenue;

                            //Secondary Coin
                            var secondaryRevenue = 0.00m;
                            if (item.SecondaryTicker != null)
                            {
                                if (
                                    !String.IsNullOrEmpty(wtmEndPoint) && 
                                    !item.SecondaryTicker.StartsWith("Zerg-") && 
                                    !item.SecondaryTicker.StartsWith("Prohashing-") &&
                                    !item.SecondaryTicker.StartsWith("MiningDutch-") &&
                                    (item.PrimaryProvider == null || !item.PrimaryProvider.Equals("ZergProvider", StringComparison.OrdinalIgnoreCase))
                                    )
                                {
                                    var coin = wtmCoins.Where(x => x.Ticker.Equals(item.SecondaryTicker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    if (coin != null)
                                    {
                                        foreach (var grouping in item.Groups)
                                        {
                                            var groupingRecord = MiningGroupDAO.GetRecord(grouping.Id);
                                            if (groupingRecord != null)
                                            {
                                                if (groupingRecord.PowerCost != null)
                                                {
                                                    powerPrice = Convert.ToDecimal(groupingRecord.PowerCost);
                                                }
                                            }
                                        }
                                        secondaryRevenue = Convert.ToDecimal(coin.BtcRevenue);
                                        if (!String.IsNullOrEmpty(coin.HashRate))
                                        {
                                            item.SecondaryHashRateMH = decimal.Parse(coin.HashRate, System.Globalization.NumberStyles.Float);
                                        }
                                    }
                                }
                                else if (item.SecondaryTicker.StartsWith("Zerg-"))
                                {
                                    var algo = zergAlgoData.Where(x => x.Algo.Equals(item.SecondaryAlgo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    if (algo != null)
                                    {
                                        var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate) / (Convert.ToDecimal(algo.MhFactor) / 1000);
                                        var mBtcRevenue = item.SecondaryHashRateMH * mBtcPerMhAmount;
                                        var btcRevenue = mBtcRevenue / 1000;
                                        secondaryRevenue = Convert.ToDecimal(btcRevenue);
                                    }
                                }
                                else if (item.PrimaryProvider != null && item.PrimaryProvider.Equals("ZergProvider", StringComparison.OrdinalIgnoreCase))
                                {
                                    var zergCoin = zergCoinData.Where(x => x.Ticker.Equals(item.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    if (zergCoin != null)
                                    {
                                        var mBtcPerMhAmount = Convert.ToDecimal(zergCoin.BtcRevenue) / (Convert.ToDecimal(zergCoin.HashRateFactor) / 1000);
                                        var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                                        var btcRevenue = mBtcRevenue / 1000;
                                        primaryRevenue = btcRevenue;
                                    }
                                }
                                else if (item.SecondaryTicker.StartsWith("Prohashing-"))
                                {
                                    var algo = proHashingAlgoData.Where(x => x.Algo.Equals(item.SecondaryAlgo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    if (algo != null)
                                    {
                                        var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate);// / (Convert.ToDecimal(algo.MhFactor) / 1000);
                                        var mBtcRevenue = item.SecondaryHashRateMH * mBtcPerMhAmount;
                                        var btcRevenue = mBtcRevenue;// / 1000;
                                        secondaryRevenue = Convert.ToDecimal(btcRevenue);
                                    }
                                }
                                else if (item.Ticker.StartsWith("MiningDutch-"))
                                {
                                    var algo = miningDutchAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    if (algo != null)
                                    {
                                        var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate);
                                        var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                                        var btcRevenue = mBtcRevenue / 1000;
                                        primaryRevenue = btcRevenue;
                                    }
                                }

                                if (!String.IsNullOrEmpty(item.SecondaryOverrideEndpoint))
                                {
                                    var coin = WhatToMineUtilities.GetIndividualCoin(item.SecondaryOverrideEndpoint);

                                    if (coin != null)
                                    {
                                        foreach (var grouping in item.Groups)
                                        {
                                            var groupingRecord = MiningGroupDAO.GetRecord(grouping.Id);
                                            if (groupingRecord != null)
                                            {
                                                if (groupingRecord.PowerCost != null)
                                                {
                                                    powerPrice = Convert.ToDecimal(groupingRecord.PowerCost);
                                                }
                                            }
                                        }

                                        secondaryRevenue = Convert.ToDecimal(coin.BtcRevenue);
                                        if (!String.IsNullOrEmpty(coin.HashRate))
                                        {
                                            item.SecondaryHashRateMH = decimal.Parse(coin.HashRate, System.Globalization.NumberStyles.Float);
                                        }
                                    }
                                }
                            }
                            combinedRevenue += secondaryRevenue;

                            var dailyPowerCost = 24 * ((Convert.ToDouble(item.Power) / 1000) * Convert.ToDouble(powerPrice));
                            decimal dailyRevenue = Convert.ToDecimal(combinedRevenue) * Convert.ToDecimal(btcPrice);
                            decimal dailyProfit = Convert.ToDecimal(dailyRevenue) - Convert.ToDecimal(dailyPowerCost);
                            item.Profit = Convert.ToDecimal(dailyProfit);
                        }
                    }
                }

            }
            return result;
        }

        public static void DeleteRecord(GoldshellAsicCoinConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<GoldshellAsicCoinConfig>(tableName);
                table.Delete(record.Id);
            }
        }

        public static void UpdateRecord(GoldshellAsicCoinConfig record)
        {
            var existingRecord = GetRecord(record.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<GoldshellAsicCoinConfig>(tableName);
                if (existingRecord != null)
                {
                    existingRecord.Ticker = record.Ticker;
                    existingRecord.Enabled = record.Enabled;
                    existingRecord.Groups = record.Groups;
                    existingRecord.MergedTickers = record.MergedTickers;
                    existingRecord.PoolPassword = record.PoolPassword;
                    existingRecord.PoolUrl = record.PoolUrl;
                    existingRecord.CoinWhatToMineEndpoint = record.CoinWhatToMineEndpoint;
                    existingRecord.PoolUser = record.PoolUser;
                    existingRecord.CoinAlgo = record.CoinAlgo;
                    existingRecord.HashRateMH = record.HashRateMH;
                    existingRecord.OverrideEndpoint = record.OverrideEndpoint;
                    existingRecord.Power = record.Power;
                    existingRecord.Profit = record.Profit;
                    existingRecord.SecondaryAlgo = record.SecondaryAlgo;
                    existingRecord.SecondaryHashRateMH = record.SecondaryHashRateMH;
                    existingRecord.SecondaryOverrideEndpoint = record.SecondaryOverrideEndpoint;
                    existingRecord.SecondaryTicker = record.SecondaryTicker;
                    existingRecord.PrimaryProvider = record.PrimaryProvider;
                    existingRecord.SecondaryProvider = record.SecondaryProvider;
                    table.Update(existingRecord);
                }
                else
                {
                    record.Id = 0;
                    table.Insert(record);
                }
            }
        }
    }
}
