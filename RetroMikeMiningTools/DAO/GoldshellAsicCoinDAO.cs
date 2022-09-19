using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.DO;

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
                    CoinAlgo = coinConfig.CoinAlgo
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

        public static List<GoldshellAsicCoinConfig> GetRecords(int workerId)
        {
            List<GoldshellAsicCoinConfig> result = new List<GoldshellAsicCoinConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GoldshellAsicCoinConfig>(tableName);
                result = table.FindAll().Where(x => x.WorkerId == workerId).ToList();
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
