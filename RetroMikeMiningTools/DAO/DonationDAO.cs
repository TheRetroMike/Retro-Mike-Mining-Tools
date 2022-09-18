using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.DAO
{
    public class DonationDAO
    {
        private static readonly string tableName = "DonationHistory";

        public static void AddDonationEntry(DonationHistory record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<DonationHistory>(tableName);
                table.Insert(record);
                db.Commit();
            }
        }

        public static List<DonationHistory> GetRecords()
        {
            List<DonationHistory> result = new List<DonationHistory>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<DonationHistory>(tableName);
                result = table.FindAll().ToList();
            }
            return result;
        }
    }
}
