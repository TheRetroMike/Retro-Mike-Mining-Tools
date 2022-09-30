using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public static class CoreConfigDAO
    {
        private static readonly string tableName = "CoreConfig";
        public static void InitialConfiguration(string? username=null)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                if (username == null)
                {
                    var recordCount = rigExecutionsCollection.Find(x => x.Username == null).Count();
                    if (recordCount == 0)
                    {
                        var result = rigExecutionsCollection.Insert(new CoreConfig()
                        {
                            ProfitSwitchingEnabled = false,
                            AutoExchangingEnabled = false,
                            CoinDifferenceThreshold = "5%",
                            HiveApiKey = null,
                            HiveFarmID = null,
                            ProfitSwitchingCronSchedule = "0 0/15 * 1/1 * ? *",
                            AutoExchangingCronSchedule = "0 0/15 * 1/1 * ? *",
                            Port = 7000,
                            ReleaseType = Enums.ReleaseType.Production,
                            DefaultPowerPrice = 0.10m,
                            UiCoinPriceCalculation = true,
                            UiRigPriceCalculation = true
                        });
                        db.Commit();
                    }
                }
                else
                {
                    rigExecutionsCollection.Insert(new CoreConfig()
                    {
                        Username = username,
                        DefaultPowerPrice = 0.10m,
                        HiveApiKey = null,
                        HiveFarmID = null,
                        CoinDifferenceThreshold = "5%",
                        UiCoinPriceCalculation = true,
                        UiRigPriceCalculation = true
                    });
                }
            }
        }

        public static CoreConfig? GetCoreConfig()
        {
            CoreConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly=true }))
            {
                var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                result = rigExecutionsCollection.FindOne(x => x.Username == null);
            }
            return result;
        }

        public static List<CoreConfig>? GetCoreConfigs()
        {
            List<CoreConfig>? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                result = rigExecutionsCollection.FindAll().ToList();
            }
            return result;
        }

        public static CoreConfig? GetCoreConfig(string username)
        {
            CoreConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                result = rigExecutionsCollection.FindOne(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
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
                existingRecord.ProfitSwitchingCronSchedule = coreConfig.ProfitSwitchingCronSchedule;
                existingRecord.IgnoredVersion = coreConfig.IgnoredVersion;
                existingRecord.DockerHostIp = coreConfig.DockerHostIp;
                existingRecord.Port = coreConfig.Port;
                existingRecord.AutoExchangingCronSchedule = coreConfig.AutoExchangingCronSchedule;
                existingRecord.ReleaseType = coreConfig.ReleaseType;
                existingRecord.DefaultPowerPrice = coreConfig.DefaultPowerPrice;
                existingRecord.UiCoinPriceCalculation = coreConfig.UiCoinPriceCalculation;
                existingRecord.UiRigPriceCalculation = coreConfig.UiRigPriceCalculation;
                using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
                {
                    var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                    rigExecutionsCollection.Update(existingRecord);
                }
            }
        }

        public static void UpdateUserCoreConfig(CoreConfig coreConfig, string username)
        {
            var existingRecord = GetCoreConfig(username);
            if (existingRecord != null)
            {
                existingRecord.CoinDifferenceThreshold = coreConfig.CoinDifferenceThreshold;
                existingRecord.HiveApiKey = coreConfig.HiveApiKey;
                existingRecord.HiveFarmID = coreConfig.HiveFarmID;
                existingRecord.DefaultPowerPrice = coreConfig.DefaultPowerPrice;
                existingRecord.UiCoinPriceCalculation = coreConfig.UiCoinPriceCalculation;
                existingRecord.UiRigPriceCalculation = coreConfig.UiRigPriceCalculation;
                using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
                {
                    var rigExecutionsCollection = db.GetCollection<CoreConfig>(tableName);
                    rigExecutionsCollection.Update(existingRecord);
                }
            }
        }
    }
}
