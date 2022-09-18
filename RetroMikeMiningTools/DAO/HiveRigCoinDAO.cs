using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.DO;

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
