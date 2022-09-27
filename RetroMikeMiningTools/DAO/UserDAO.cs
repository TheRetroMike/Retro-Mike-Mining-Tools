using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public static class UserDAO
    {
        private static readonly string tableName = "UserConfig";

        public static UserConfig? GetUser(string username)
        {
            UserConfig? result = null;
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<UserConfig>(tableName);
                result = table.FindOne(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }

        public static void AddUser(string username, string hashedPassword)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<UserConfig>(tableName);
                table.Insert(new UserConfig()
                {
                    UserName = username,
                    Password = hashedPassword
                });
            }
        }

    }
}
