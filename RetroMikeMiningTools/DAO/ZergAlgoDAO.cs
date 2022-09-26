using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.DAO
{
    public static class ZergAlgoDAO
    {
        private static readonly string tableName = "ZergAlgoConfig";

        public static void AddRecord(ZergAlgoConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<ZergAlgoConfig>(tableName);
                table.Insert(new ZergAlgoConfig()
                {
                    Algo = record.Algo,
                    Flightsheet = record.Flightsheet,
                    Enabled = record.Enabled,
                    WorkerId = record.WorkerId,
                    Groups = record.Groups,
                    HashRateMH = record.HashRateMH,
                    Power = record.Power
                });
            }
        }

        public static ZergAlgoConfig? GetRecord(int id)
        {
            ZergAlgoConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<ZergAlgoConfig>(tableName);
                result = table.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static List<ZergAlgoConfig> GetRecords(int workerId, List<Flightsheet> flightsheets)
        {
            List<ZergAlgoConfig> result = new List<ZergAlgoConfig>();
            var btcPrice = CoinDeskUtilities.GetBtcPrice();
            var config = CoreConfigDAO.GetCoreConfig();
            var algos = ZergUtilities.GetZergAlgoData();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<ZergAlgoConfig>(tableName);
                result = table.FindAll().Where(x => x.WorkerId == workerId).ToList();
            }
            foreach (var item in result)
            {
                if (item.Flightsheet != null)
                {
                    item.FlightsheetName = flightsheets?.Where(x => x.Id == item.Flightsheet)?.FirstOrDefault()?.Name;
                }
                var algo = algos.Where(x => x.Algo.Equals(item.Algo)).FirstOrDefault();
                if (algo != null)
                {
                    var mBtcPerMhAmount = Convert.ToDecimal(algo.Estimate) / (Convert.ToDecimal(algo.MhFactor) / 1000);
                    var mBtcRevenue = item.HashRateMH * mBtcPerMhAmount;
                    var btcRevenue = mBtcRevenue / 1000;

                    decimal dailyPowerCost = 24 * (Convert.ToDecimal(item.Power) / 1000m) * Convert.ToDecimal(config?.DefaultPowerPrice ?? 0.10m);
                    decimal dailyRevenue = Convert.ToDecimal(btcRevenue) * Convert.ToDecimal(btcPrice);
                    decimal dailyProfit = dailyRevenue - dailyPowerCost;
                    item.Profit = Convert.ToDecimal(dailyProfit);
                }
            }
            return result;
        }

        public static void DeleteRecord(ZergAlgoConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<ZergAlgoConfig>(tableName);
                table.Delete(record.Id);
            }
        }

        public static void UpdateRecord(ZergAlgoConfig record)
        {
            var existingRecord = GetRecord(record.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<ZergAlgoConfig>(tableName);
                if (existingRecord != null)
                {
                    existingRecord.Algo = record.Algo;
                    existingRecord.Flightsheet = record.Flightsheet;
                    existingRecord.Enabled = record.Enabled;
                    existingRecord.Groups = record.Groups;
                    existingRecord.HashRateMH = record.HashRateMH;
                    existingRecord.Power = record.Power;
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
