using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public static class CoreConfigDAO
    {
        private static readonly string tableName = "CoreConfig";
        public static void InitialConfiguration()
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                var recordCount = rigExecutionsCollection.Count();
                if (recordCount == 0)
                {
                    var result = rigExecutionsCollection.Insert(new CoreConfig()
                    {
                        ProfitSwitchingEnabled = false,
                        AutoExchangingEnabled = false,
                        CoinDeskApi = "https://api.coindesk.com/v1/bpi/currentprice.json",
                        CoinDifferenceThreshold = "5%",
                        HiveApiKey = null,
                        HiveFarmID = null,
                        ProfitSwitchingCronSchedule = "0 0/1 * 1/1 * ? *",
                        AutoExchangingCronSchedule = "0 0/1 * 1/1 * ? *",
                        Port =7000
                    });
                    db.Commit();
                }
            }
        }

        public static CoreConfig? GetCoreConfig()
        {
            CoreConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly=true }))
            {
                var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                var recordCount = rigExecutionsCollection.Count();
                if (recordCount == 1)
                {
                    result = rigExecutionsCollection.FindAll().FirstOrDefault();
                }
            }
            return result;
        }

        public static void UpdateCoreConfig(CoreConfig coreConfig)
        {
            var existingRecord = GetCoreConfig();
            if (existingRecord != null)
            {
                existingRecord.AutoExchangingEnabled = coreConfig.AutoExchangingEnabled;
                existingRecord.CoinDifferenceThreshold = coreConfig.CoinDifferenceThreshold;
                existingRecord.ProfitSwitchingEnabled = coreConfig.ProfitSwitchingEnabled;
                existingRecord.HiveApiKey = coreConfig.HiveApiKey;
                existingRecord.HiveFarmID = coreConfig.HiveFarmID;
                existingRecord.CoinDeskApi = coreConfig.CoinDeskApi;
                existingRecord.ProfitSwitchingCronSchedule = coreConfig.ProfitSwitchingCronSchedule;
                existingRecord.IgnoredVersion = coreConfig.IgnoredVersion;
                existingRecord.DockerHostIp = coreConfig.DockerHostIp;
                existingRecord.Port = coreConfig.Port;
                existingRecord.AutoExchangingCronSchedule = coreConfig.AutoExchangingCronSchedule;
                using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
                {
                    var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                    rigExecutionsCollection.Update(existingRecord);
                }
            }
        }
    }
}
