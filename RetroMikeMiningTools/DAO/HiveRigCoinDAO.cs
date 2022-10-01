using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Utilities;
using System.Web;

namespace RetroMikeMiningTools.DAO
{
    public static class HiveRigCoinDAO
    {
        private static readonly string tableName = "HiveRigCoinConfig";

        public static void AddRecord(HiveOsRigCoinConfig coinConfig)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<HiveOsRigCoinConfig>(tableName);
                table.Insert(new HiveOsRigCoinConfig()
                {
                    Ticker = coinConfig.Ticker,
                    Flightsheet = coinConfig.Flightsheet,
                    Enabled = coinConfig.Enabled,
                    WorkerId = coinConfig.WorkerId,
                    Groups = coinConfig.Groups,
                    Username = coinConfig.Username,
                    HashRateMH = coinConfig.HashRateMH,
                    Power = coinConfig.Power,
                    Algo = coinConfig.Algo,
                    SecondaryTicker = coinConfig.SecondaryTicker,
                    SecondaryHashRateMH = coinConfig.SecondaryHashRateMH,
                    SecondaryAlgo = coinConfig.SecondaryAlgo
                });
            }
        }

        public static HiveOsRigCoinConfig? GetRecord(int id)
        {
            HiveOsRigCoinConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<HiveOsRigCoinConfig>(tableName);
                result = table.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static List<HiveOsRigCoinConfig> GetRecords(int workerId, List<Flightsheet> flightsheets, CoreConfig config, bool isUi, bool forceProfit = false)
        {
            List<HiveOsRigCoinConfig> result = new List<HiveOsRigCoinConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<HiveOsRigCoinConfig>(tableName);
                result = table.FindAll().Where(x => x.WorkerId == workerId).ToList();
            }
            if (!isUi || (isUi && config.UiCoinPriceCalculation) || forceProfit)
            {
                var btcPrice = CoinDeskUtilities.GetBtcPrice();
                List<Coin> wtmCoins = new List<Coin>();

                decimal powerPrice = 0.00m;
                var wtmEndPoint = HiveRigDAO.GetRecord(workerId)?.WhatToMineEndpoint;
                if (wtmEndPoint != null)
                {
                    powerPrice = Convert.ToDecimal(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndPoint)).Query).Get("factor[cost]"));
                    wtmCoins = WhatToMineUtilities.GetCoinList(wtmEndPoint);
                }
                var zergAlgoData = ZergUtilities.GetZergAlgoData();
                var proHashingAlgoData = ProhashingUtilities.GetAlgoData();

                foreach (var item in result)
                {
                    if (item.Flightsheet != null)
                    {
                        item.FlightsheetName = flightsheets?.Where(x => x.Id == item.Flightsheet)?.FirstOrDefault()?.Name;
                    }

                    var combinedRevenue = 0.00m;
                    var dailyPowerCost = 24 * ((Convert.ToDouble(item.Power) / 1000) * Convert.ToDouble(powerPrice));

                    //Primary Coin
                    if (!String.IsNullOrEmpty(wtmEndPoint) && !item.Ticker.StartsWith("Zerg-") && !item.Ticker.StartsWith("Prohashing-"))
                    {
                        var coin = wtmCoins.Where(x => x.Ticker.Equals(item.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
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
                            combinedRevenue += Convert.ToDecimal(coin.BtcRevenue);
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
                            combinedRevenue += btcRevenue;
                        }
                    }
                    else if (item.Ticker.StartsWith("Prohashing-"))
                    {
                        var algo = proHashingAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (algo != null)
                        {
                            var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate);// / (Convert.ToDecimal(algo.MhFactor) / 1000);
                            var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                            var btcRevenue = mBtcRevenue;// / 1000;
                            combinedRevenue += btcRevenue;
                        }
                    }

                    //Secondary Coin
                    if (item.SecondaryTicker != null)
                    {
                        if (!String.IsNullOrEmpty(wtmEndPoint) && !item.SecondaryTicker.StartsWith("Zerg-") && !item.SecondaryTicker.StartsWith("Prohashing-"))
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
                                combinedRevenue += Convert.ToDecimal(coin.BtcRevenue);
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
                                combinedRevenue += Convert.ToDecimal(btcRevenue);
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
                                combinedRevenue += Convert.ToDecimal(btcRevenue);
                            }
                        }
                    }

                    decimal dailyRevenue = Convert.ToDecimal(combinedRevenue) * Convert.ToDecimal(btcPrice);
                    decimal dailyProfit = Convert.ToDecimal(dailyRevenue) - Convert.ToDecimal(dailyPowerCost);
                    item.Profit = Convert.ToDecimal(dailyProfit);
                }
            }
            return result;
        }

        public static void DeleteRecord(HiveOsRigCoinConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<HiveOsRigCoinConfig>(tableName);
                table.Delete(record.Id);
            }
        }

        public static void UpdateRecord(HiveOsRigCoinConfig record)
        {
            var existingRecord = GetRecord(record.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<HiveOsRigCoinConfig>(tableName);
                if (existingRecord != null)
                {
                    existingRecord.Ticker = record.Ticker;
                    existingRecord.Flightsheet = record.Flightsheet;
                    existingRecord.Enabled = record.Enabled;
                    existingRecord.Groups = record.Groups;
                    existingRecord.HashRateMH = record.HashRateMH;
                    existingRecord.Power = record.Power;
                    existingRecord.Algo = record.Algo;
                    existingRecord.SecondaryHashRateMH = record.SecondaryHashRateMH;
                    existingRecord.SecondaryTicker = record.SecondaryTicker;
                    existingRecord.SecondaryAlgo = record.SecondaryAlgo;
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
