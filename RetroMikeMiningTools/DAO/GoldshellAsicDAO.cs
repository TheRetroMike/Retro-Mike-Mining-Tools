using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public static class GoldshellAsicDAO
    {
        private static readonly string tableName = "GoldshellAsicConfig";

        public static void AddRig(GoldshellAsicConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<GoldshellAsicConfig>(tableName);
                table.Insert(new GoldshellAsicConfig()
                {
                    Name = record.Name,
                    DonationAmount = record.DonationAmount,
                    Enabled = record.Enabled,
                    WhatToMineEndpoint = record.WhatToMineEndpoint,
                    MiningMode = record.MiningMode,
                    EnabledDateTime = record.Enabled ? DateTime.Now : null,
                    ApiBasePath = record.ApiBasePath,
                    ApiPassword = record.ApiPassword
                });
            }
        }

        public static GoldshellAsicConfig? GetRecord(string workerName)
        {
            GoldshellAsicConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GoldshellAsicConfig>(tableName);
                result = table.FindOne(x => x.Name.Equals(workerName, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }

        public static GoldshellAsicConfig? GetRecord(int id)
        {
            GoldshellAsicConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GoldshellAsicConfig>(tableName);
                result = table.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static List<GoldshellAsicConfig> GetRecords(bool isUi)
        {
            var config = CoreConfigDAO.GetCoreConfig();
            List<GoldshellAsicConfig> result = new List<GoldshellAsicConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GoldshellAsicConfig>(tableName);
                result = table.FindAll().ToList();
            }

            if (!isUi || (isUi && config.UiRigPriceCalculation))
            {
                foreach (var item in result)
                {
                    var coins = GoldshellAsicCoinDAO.GetRecords(item.Id, isUi, true)?.Where(x => x.Enabled);
                    if (coins != null && coins.Count() > 0)
                    {
                        item.Profit = coins.Max(x => x.Profit);
                    }
                }
            }
            return result;
        }

        public static void DeleteRecord(GoldshellAsicConfig rigConfig)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<HiveOsRigConfig>(tableName);
                table.Delete(rigConfig.Id);
            }
        }

        public static void UpdateRecord(GoldshellAsicConfig record)
        {
            var existingRecord = GetRecord(record.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
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
                    existingRecord.ApiBasePath = record.ApiBasePath;
                    existingRecord.ApiPassword = record.ApiPassword;
                    var table = db.GetCollection<GoldshellAsicConfig>(tableName);
                    table.Update(existingRecord);
                }
                else
                {
                    var table = db.GetCollection<GoldshellAsicConfig>(tableName);
                    table.Insert(new GoldshellAsicConfig()
                    {
                        Name = record.Name,
                        DonationAmount = record.DonationAmount,
                        WhatToMineEndpoint = record.WhatToMineEndpoint,
                        MiningMode = record.MiningMode,
                        Enabled = record.Enabled,
                        EnabledDateTime = record.Enabled ? DateTime.Now : null,
                        ApiBasePath = record.ApiBasePath,
                        ApiPassword = record.ApiPassword
                    });
                }
            }
        }
    }
}
