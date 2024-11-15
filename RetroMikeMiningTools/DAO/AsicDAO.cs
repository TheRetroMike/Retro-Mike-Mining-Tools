using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public static class AsicDAO
    {
        private static readonly string tableName = "AsicConfig";

        public static void Add(AsicConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<AsicConfig>(tableName);
                var insertResult = table.Insert(new AsicConfig()
                {
                    Name = record.Name,
                    AsicType = record.AsicType,
                    DeviceIP = record.DeviceIP,
                    AdditionalPower = record.AdditionalPower,
                    MiningMode = record.MiningMode,
                    Enabled = record.Enabled,
                    PinnedTicker = record.PinnedTicker,
                    RigMinProfit = record.RigMinProfit,
                    SmartPlugHost = record.SmartPlugHost,
                    SmartPlugType = record.SmartPlugType,
                    ApiKey = record.ApiKey,
                    Username = record.Username
                });
            }
        }

        public static void Update(AsicConfig record)
        {
            var existingGroup = GetRecord(record.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                if (existingGroup != null)
                {
                    existingGroup.Name = record.Name;
                    existingGroup.AsicType = record.AsicType;
                    existingGroup.DeviceIP = record.DeviceIP;
                    existingGroup.Enabled = record.Enabled;
                    existingGroup.MiningMode = record.MiningMode;
                    existingGroup.AdditionalPower = record.AdditionalPower;
                    existingGroup.RigMinProfit = record.RigMinProfit;
                    existingGroup.SmartPlugHost = record.SmartPlugHost;
                    existingGroup.SmartPlugType = record. SmartPlugType;
                    existingGroup.PinnedTicker = record.PinnedTicker;
                    existingGroup.ApiKey = record.ApiKey;
                    existingGroup.Username = record.Username;
                    var table = db.GetCollection<AsicConfig>(tableName);
                    table.Update(existingGroup);
                }
                else
                {
                    var table = db.GetCollection<AsicConfig>(tableName);
                    table.Insert(new AsicConfig()
                    {
                        Name = record.Name,
                        AsicType = record.AsicType,
                        DeviceIP = record.DeviceIP,
                        Enabled = record.Enabled,
                        MiningMode = record.MiningMode,
                        AdditionalPower = record.AdditionalPower,
                        RigMinProfit = record.RigMinProfit,
                        SmartPlugHost = record.SmartPlugHost,
                        SmartPlugType = record.SmartPlugType,
                        PinnedTicker = record.PinnedTicker,
                        ApiKey = record.ApiKey,
                        Username = record.Username
                    });
                }
            }
        }

        public static AsicConfig? GetRecord(string name, string username)
        {
            AsicConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<AsicConfig>(tableName);
                if (username == null)
                {
                    result = table.FindOne(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    result = table.FindOne(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                }
                
            }
            return result;
        }

        public static AsicConfig? GetRecord(int id)
        {
            AsicConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<AsicConfig>(tableName);
                result = table.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static List<AsicConfig> GetRecords()
        {
            List<AsicConfig> result = new List<AsicConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<AsicConfig>(tableName);
                result = table.FindAll().ToList();
            }
            return result;
        }

        public static List<AsicConfig> GetRecords(CoreConfig config, bool isUi, string? username, bool isMultiUser)
        {
            List<AsicConfig> result = new List<AsicConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var rigExecutionsCollection = db.GetCollection<AsicConfig>(tableName);


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
                        var coins = AsicCoinDAO.GetRecords(item.Id, config, isUi, true, isMultiUser)?.Where(x => x.Enabled);
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

        public static void DeleteRecord(AsicConfig record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<AsicConfig>(tableName);
                table.Delete(record.Id);
            }
        }
    }
}
