using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Utilities;
using System.Web;

namespace RetroMikeMiningTools.DAO
{
    public static class AsicCoinDAO
    {
        private static readonly string tableName = "AsicCoinConfig";

        public static void AddRecord(AsicCoinConfig coinConfig)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<AsicCoinConfig>(tableName);
                table.Insert(new AsicCoinConfig()
                {
                    Ticker = coinConfig.Ticker,
                    Pool = coinConfig.Pool,
                    PoolUser = coinConfig.PoolUser,
                    PoolPassword = coinConfig.PoolPassword,
                    Enabled = coinConfig.Enabled,
                    WorkerId = coinConfig.WorkerId,
                    Groups = coinConfig.Groups,
                    Username = coinConfig.Username,
                    HashRateMH = coinConfig.HashRateMH,
                    Power = coinConfig.Power,
                    Algo = coinConfig.Algo,
                    OverrideEndpoint = coinConfig.OverrideEndpoint,
                    PrimaryProvider = coinConfig.PrimaryProvider,
                });
            }
        }

        public static AsicCoinConfig? GetRecord(int id)
        {
            AsicCoinConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<AsicCoinConfig>(tableName);
                result = table.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static List<AsicCoinConfig> GetRecords(int workerId, CoreConfig config, bool isUi, bool forceProfit, bool isMultiUser)
        {
            List<AsicCoinConfig> result = new List<AsicCoinConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<AsicCoinConfig>(tableName);
                result = table.FindAll().Where(x => x.WorkerId == workerId).ToList();
            }
            if (!isUi || (isUi && !isMultiUser && config.UiCoinPriceCalculation) || forceProfit)
            {
                var btcPrice = CoinDeskUtilities.GetBtcPrice();
                List<Coin> wtmCoins = new List<Coin>();
                var rigRecord = HiveRigDAO.GetRecord(workerId);
                var additionalPower = rigRecord?.AdditionalPower ?? 0;
                decimal powerPrice = config.DefaultPowerPrice;
                var wtmEndPoint = rigRecord?.WhatToMineEndpoint;
                if (wtmEndPoint != null)
                {
                    wtmCoins = WhatToMineUtilities.GetCoinList(wtmEndPoint);
                }
                var zergAlgoData = ZergUtilities.GetZergAlgoData();
                var zergCoinData = ZergUtilities.GetZergCoins();
                var zpoolAlgoData = ZpoolUtilities.GetAlgoData();
                var proHashingAlgoData = ProhashingUtilities.GetAlgoData();
                var miningDutchAlgoData = MiningDutchUtilities.GetAlgoData();
                var unmineableAlgoData = UnmineableUtilities.GetAlgoData();

                foreach (var item in result)
                {
                    var combinedRevenue = 0.00m;

                    //Primary Coin
                    var primaryRevenue = 0.00m;
                    if (
                        !String.IsNullOrEmpty(wtmEndPoint) &&
                        !item.Ticker.StartsWith("Zerg-") &&
                        !item.Ticker.StartsWith("Zpool-") &&
                        !item.Ticker.StartsWith("Prohashing-") &&
                        !item.Ticker.StartsWith("MiningDutch-") &&
                        !item.Ticker.StartsWith("Unmineable-") &&
                        !item.Ticker.StartsWith("ZergProvider-") &&
                        !item.Ticker.StartsWith("ZpoolProvider-")
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
                        if (algo != null && !algo.Algo.Equals("token", StringComparison.OrdinalIgnoreCase))
                        {
                            var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate) / (Convert.ToDecimal(algo.MhFactor) / 1000);
                            var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                            var btcRevenue = mBtcRevenue / 1000;
                            primaryRevenue = btcRevenue;
                        }
                    }
                    else if (item.Ticker.StartsWith("Zpool-"))
                    {
                        var algo = zpoolAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (algo != null && !algo.Algo.Equals("token", StringComparison.OrdinalIgnoreCase))
                        {
                            var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate) / (Convert.ToDecimal(algo.MhFactor) / 1000);
                            var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                            var btcRevenue = mBtcRevenue / 1000;
                            primaryRevenue = btcRevenue;
                        }
                    }
                    else if (item.Ticker.StartsWith("Unmineable-"))
                    {
                        var algo = unmineableAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (algo != null && !algo.Algo.Equals("token", StringComparison.OrdinalIgnoreCase))
                        {
                            var btcPerMhAmount = Convert.ToDecimal(algo.Estimate) * (Convert.ToDecimal(1000000));
                            var btcRevenue = item.HashRateMH * btcPerMhAmount;
                            //var btcRevenue = mBtcRevenue / 1000;
                            primaryRevenue = btcRevenue;
                        }
                    }
                    else if (item.Ticker.StartsWith("ZergProvider-"))
                    {
                        var zergCoin = zergCoinData.Where(x => x.Ticker.Equals(item.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (zergCoin != null)
                        {
                            if (zergCoin.HashRateFactor != "False")
                            {
                                var mBtcPerMhAmount = Convert.ToDecimal(zergCoin.BtcRevenue) / (Convert.ToDecimal(zergCoin.HashRateFactor) / 1000);
                                var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                                var btcRevenue = mBtcRevenue / 1000;
                                primaryRevenue = btcRevenue;
                            }
                        }
                    }
                    else if (item.Ticker.StartsWith("Prohashing-"))
                    {
                        var algo = proHashingAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (algo != null)
                        {
                            var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate);
                            var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                            var btcRevenue = mBtcRevenue;// / 1000;
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
                            var btcRevenue = mBtcRevenue / 1;//1000;
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

                    var dailyPowerCost = 24 * ((Convert.ToDouble(item.Power + additionalPower) / 1000) * Convert.ToDouble(powerPrice));
                    decimal dailyRevenue = Convert.ToDecimal(combinedRevenue) * Convert.ToDecimal(btcPrice);
                    decimal dailyProfit = Convert.ToDecimal(dailyRevenue) - Convert.ToDecimal(dailyPowerCost);
                    item.Profit = Convert.ToDecimal(dailyProfit);
                }
            }
            return result;
        }

        public static void DeleteRecord(AsicCoinConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<AsicCoinConfig>(tableName);
                table.Delete(record.Id);
            }
        }

        public static void DeleteRecords(int workerId)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<AsicCoinConfig>(tableName);
                table.DeleteMany(x => x.WorkerId == workerId);
            }
        }

        public static void UpdateRecord(AsicCoinConfig record)
        {
            var existingRecord = GetRecord(record.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<AsicCoinConfig>(tableName);
                if (existingRecord != null)
                {
                    existingRecord.Ticker = record.Ticker;
                    existingRecord.Pool = record.Pool;
                    existingRecord.PoolUser = record.PoolUser;
                    existingRecord.PoolPassword = record.PoolPassword;
                    existingRecord.Enabled = record.Enabled;
                    existingRecord.Groups = record.Groups;
                    existingRecord.HashRateMH = record.HashRateMH;
                    existingRecord.Power = record.Power;
                    existingRecord.Algo = record.Algo;
                    existingRecord.OverrideEndpoint = record.OverrideEndpoint;
                    existingRecord.PrimaryProvider = record.PrimaryProvider;
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
