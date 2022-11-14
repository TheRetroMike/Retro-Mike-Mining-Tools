using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public static class HiveRigDAO
    {
        private static readonly string tableName = "HiveRigConfig";
        public static void InitialConfiguration()
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var rigExecutionsCollection = db.GetCollection<HiveOsRigConfig>(tableName);
                var recordCount = rigExecutionsCollection.Count();
                if (recordCount == 0)
                {
                    db.Commit();
                }
            }
        }

        public static void AddRig(HiveOsRigConfig rigConfig)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var rigExecutionsCollection = db.GetCollection<HiveOsRigConfig>(tableName);
                rigExecutionsCollection.Insert(new HiveOsRigConfig()
                {
                    Name = rigConfig.Name,
                    DonationAmount = rigConfig.DonationAmount,
                    Enabled = rigConfig.Enabled,
                    WhatToMineEndpoint = rigConfig.WhatToMineEndpoint,
                    MiningMode = rigConfig.MiningMode,
                    EnabledDateTime = rigConfig.Enabled ? DateTime.Now : null,
                    PinnedTicker = rigConfig.PinnedTicker,
                    PinnedZergAlgo = rigConfig.PinnedZergAlgo,
                    Username = rigConfig.Username,
                    SmartPlugType = rigConfig.SmartPlugType,
                    SmartPlugHost = rigConfig.SmartPlugHost,
                    RigMinProfit = rigConfig.RigMinProfit,
                    AdditionalPower = rigConfig.AdditionalPower
                });
            }
        }

        public static HiveOsRigConfig? GetRecord(string workerName)
        {
            HiveOsRigConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var rigExecutionsCollection = db.GetCollection<HiveOsRigConfig>(tableName);
                result = rigExecutionsCollection.FindOne(x => x.Name.Equals(workerName, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }

        public static HiveOsRigConfig? GetRecord(string workerName, string username)
        {
            HiveOsRigConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var rigExecutionsCollection = db.GetCollection<HiveOsRigConfig>(tableName);
                result = rigExecutionsCollection.FindOne(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && x.Name.Equals(workerName, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }

        public static HiveOsRigConfig? GetRecord(int id)
        {
            HiveOsRigConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var rigExecutionsCollection = db.GetCollection<HiveOsRigConfig>(tableName);
                result = rigExecutionsCollection.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static List<HiveOsRigConfig> GetRecords(CoreConfig config, bool isUi, string? username, bool isMultiUser)
        {
            List<HiveOsRigConfig> result = new List<HiveOsRigConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var rigExecutionsCollection = db.GetCollection<HiveOsRigConfig>(tableName);
                

                if (username != null && !String.IsNullOrEmpty(username))
                {
                    result = rigExecutionsCollection.Find(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    result = rigExecutionsCollection.FindAll().ToList();
                }

                if (!isUi || (isUi && !isMultiUser && config.UiRigPriceCalculation))
                {
                    foreach (var item in result)
                    {
                        var coins = HiveRigCoinDAO.GetRecords(item.Id, new List<DO.Flightsheet>(), config, isUi, true, isMultiUser)?.Where(x => x.Enabled);
                        if (coins != null && coins.Count() > 0)
                        {
                            item.Profit = coins.Max(x => x.Profit);
                        }
                    }
                    if (!String.IsNullOrEmpty(username) && !isUi)
                    {
                        Thread.Sleep(250);
                    }
                }
            }
            return result;
        }

        public static void DeleteRecord(HiveOsRigConfig rigConfig)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var rigExecutionsCollection = db.GetCollection<HiveOsRigConfig>(tableName);
                rigExecutionsCollection.Delete(rigConfig.Id);
            }
        }

        public static void UpdateRecord(HiveOsRigConfig record)
        {
            var existingRecord = GetRecord(record.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<HiveOsRigConfig>(tableName);
                if (existingRecord != null)
                {
                    if (!existingRecord.Enabled && record.Enabled)
                    {
                        existingRecord.EnabledDateTime = DateTime.Now;
                    }
                    existingRecord.Name = record.Name;
                    existingRecord.Enabled = record.Enabled;
                    existingRecord.DonationAmount = record.DonationAmount;
                    existingRecord.WhatToMineEndpoint = record.WhatToMineEndpoint;
                    existingRecord.MiningMode = record.MiningMode;
                    existingRecord.DonationStartTime = record.DonationStartTime;
                    existingRecord.DonationEndTime = record.DonationEndTime;
                    existingRecord.DonationRunning = record.DonationRunning;
                    existingRecord.PinnedTicker = record.PinnedTicker;
                    existingRecord.PinnedZergAlgo = record.PinnedZergAlgo;
                    existingRecord.SmartPlugType = record.SmartPlugType;
                    existingRecord.SmartPlugHost = record.SmartPlugHost;
                    existingRecord.RigMinProfit = record.RigMinProfit;
                    existingRecord.AdditionalPower = record.AdditionalPower;
                    
                    table.Update(existingRecord);
                }
                else
                {
                    table.Insert(new HiveOsRigConfig()
                    {
                        Name = record.Name,
                        DonationAmount = record.DonationAmount,
                        WhatToMineEndpoint = record.WhatToMineEndpoint,
                        MiningMode = record.MiningMode,
                        Enabled = record.Enabled,
                        EnabledDateTime = record.Enabled ? DateTime.Now : null,
                        PinnedTicker = record.PinnedTicker,
                        PinnedZergAlgo = record.PinnedZergAlgo,
                        SmartPlugType = record.SmartPlugType,
                        SmartPlugHost = record.SmartPlugHost,
                        RigMinProfit = record.RigMinProfit,
                        AdditionalPower = record.AdditionalPower
                    });
                }
            }
        }
    }
}
