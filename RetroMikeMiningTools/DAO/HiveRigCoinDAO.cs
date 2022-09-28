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

        public static List<HiveOsRigCoinConfig> GetRecords(int workerId, List<Flightsheet> flightsheets, CoreConfig config)
        {
            var btcPrice = CoinDeskUtilities.GetBtcPrice();
            List<Coin> wtmCoins = new List<Coin>();
            var zergAlgoData = ZergUtilities.GetZergAlgoData();
            string powerPrice = String.Empty;
            var wtmEndPoint = HiveRigDAO.GetRecord(workerId)?.WhatToMineEndpoint;
            if (wtmEndPoint != null)
            {
                powerPrice = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndPoint)).Query).Get("factor[cost]");
                wtmCoins = WhatToMineUtilities.GetCoinList(wtmEndPoint);
            }
            

            List<HiveOsRigCoinConfig> result = new List<HiveOsRigCoinConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<HiveOsRigCoinConfig>(tableName);
                result = table.FindAll().Where(x => x.WorkerId == workerId).ToList();
            }
            foreach (var item in result)
            {
                if (item.Flightsheet != null)
                {
                    item.FlightsheetName = flightsheets?.Where(x => x.Id == item.Flightsheet)?.FirstOrDefault()?.Name;
                }

                if (!String.IsNullOrEmpty(wtmEndPoint) && !item.Ticker.StartsWith("Zerg-"))
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
                                    powerPrice = Convert.ToString(groupingRecord.PowerCost);
                                }
                            }
                        }
                        

                        var dailyPowerCost = 24 * ((Convert.ToDouble(coin.PowerConsumption) / 1000) * Convert.ToDouble(powerPrice));
                        var dailyRevenue = Convert.ToDouble(coin.BtcRevenue) * Convert.ToDouble(btcPrice);
                        var dailyProfit = dailyRevenue - dailyPowerCost;
                        item.Profit = Convert.ToDecimal(dailyProfit);
                        item.CoinRevenue = Convert.ToDecimal(coin.CoinRevenue);
                    }
                }
                else if(item.Ticker.StartsWith("Zerg-"))
                {
                    var algo = zergAlgoData.Where(x => x.Algo.Equals(item.Algo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (algo != null)
                    {
                        var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate) / (Convert.ToDecimal(algo.MhFactor) / 1000);
                        var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                        var btcRevenue = mBtcRevenue / 1000;

                        decimal dailyPowerCost = 24 * (Convert.ToDecimal(item.Power) / 1000m) * Convert.ToDecimal(config.DefaultPowerPrice);
                        decimal dailyRevenue = Convert.ToDecimal(btcRevenue) * Convert.ToDecimal(btcPrice);
                        decimal dailyProfit = Convert.ToDecimal(dailyRevenue) - Convert.ToDecimal(dailyPowerCost);
                        item.Profit = Convert.ToDecimal(dailyProfit);
                    }
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
