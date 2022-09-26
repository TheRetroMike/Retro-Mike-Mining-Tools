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
                    Groups = coinConfig.Groups
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

        public static List<HiveOsRigCoinConfig> GetRecords(int workerId, List<Flightsheet> flightsheets)
        {
            var btcPrice = CoinDeskUtilities.GetBtcPrice();
            List<Coin> coins = new List<Coin>();
            string powerPrice = String.Empty;
            var wtmEndPoint = DAO.HiveRigDAO.GetRecord(workerId)?.WhatToMineEndpoint;
            if (wtmEndPoint != null)
            {
                powerPrice = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndPoint)).Query).Get("factor[cost]");
                coins = WhatToMineUtilities.GetCoinList(wtmEndPoint);
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

                if (!String.IsNullOrEmpty(wtmEndPoint))
                {
                    var coin = coins.Where(x => x.Ticker.Equals(item.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (coin != null)
                    {
                        var dailyPowerCost = 24 * ((Convert.ToDouble(coin.PowerConsumption) / 1000) * Convert.ToDouble(powerPrice));
                        var dailyRevenue = Convert.ToDouble(coin.BtcRevenue) * Convert.ToDouble(btcPrice);
                        var dailyProfit = dailyRevenue - dailyPowerCost;
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
