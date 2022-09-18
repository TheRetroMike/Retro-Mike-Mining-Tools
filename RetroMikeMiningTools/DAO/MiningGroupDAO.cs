using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public static class MiningGroupDAO
    {
        private static readonly string tableName = "MiningGroupConfig";

        public static void AddMiningGroup(GroupConfig miningGroup)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<GroupConfig>(tableName);
                var insertResult = table.Insert(new GroupConfig()
                {
                    Name = miningGroup.Name,
                    StartTime = miningGroup.StartTime,
                    EndTime = miningGroup.EndTime,
                    PowerCost = miningGroup.PowerCost,
                    Enabled = miningGroup.Enabled
                });
            }
        }

        public static void UpdateMiningGroup(GroupConfig miningGroup)
        {
            var existingGroup = GetRecord(miningGroup.Id);
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                if (existingGroup != null)
                {
                    existingGroup.Name = miningGroup.Name;
                    existingGroup.Enabled = miningGroup.Enabled;
                    existingGroup.StartTime = miningGroup.StartTime;
                    existingGroup.EndTime = miningGroup.EndTime;
                    existingGroup.PowerCost = miningGroup.PowerCost;
                    var table = db.GetCollection<GroupConfig>(tableName);
                    table.Update(existingGroup);
                }
                else
                {
                    var table = db.GetCollection<GroupConfig>(tableName);
                    table.Insert(new GroupConfig()
                    {
                        Name = miningGroup.Name,
                        StartTime = miningGroup.StartTime,
                        EndTime = miningGroup.EndTime,
                        PowerCost = miningGroup.PowerCost,
                        Enabled = miningGroup.Enabled
                    });
                }

            }
        }

        public static GroupConfig? GetRecord(string groupName)
        {
            GroupConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GroupConfig>(tableName);
                result = table.FindOne(x => x.Name == groupName);
            }
            return result;
        }

        public static GroupConfig? GetRecord(int id)
        {
            GroupConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GroupConfig>(tableName);
                result = table.FindOne(x => x.Id == id);
            }
            return result;
        }

        public static List<GroupConfig> GetRecords()
        {
            List<GroupConfig> result = new List<GroupConfig>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<GroupConfig>(tableName);
                result = table.FindAll().ToList();
            }
            return result;
        }

        public static void DeleteRecord(GroupConfig miningGroup)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<GroupConfig>(tableName);
                table.Delete(miningGroup.Id);
            }
        }
    }
}
