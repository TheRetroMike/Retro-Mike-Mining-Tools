﻿using LiteDB;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;

namespace RetroMikeMiningTools.DAO
{
    public static class LogDAO
    {
        private static readonly string tableName = "Log";

        public static void InitialConfiguration()
        {
            var data = GetLogs();
            if (data==null || data?.Count==0)
            {
                Add(new LogEntry() { LogDateTime = DateTime.Now, LogType = LogType.System, LogMessage = "Application Installed" });
            }
        }

        public static void Add(LogEntry record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<LogEntry>(tableName);
                table.Insert(new LogEntry()
                {
                    LogDateTime = record.LogDateTime,
                    LogMessage = record.LogMessage,
                    LogType = record.LogType,
                    Username = record.Username
                });
            }
        }

        public static void DeleteRecord(LogEntry record)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<LogEntry>(tableName);
                table.Delete(record.Id);
            }
        }

        public static List<LogEntry> GetLogs()
        {
            List<LogEntry> result = new List<LogEntry>();
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = true }))
            {
                var table = db.GetCollection<LogEntry>(tableName);
                result = table.FindAll().ToList();
            }
            return result;
        }

        public static void PurgeLogs(string username)
        {
            using (var db = new LiteDatabase(new ConnectionString { Filename = Constants.DB_FILE, Connection = ConnectionType.Shared, ReadOnly = false }))
            {
                var table = db.GetCollection<LogEntry>(tableName);
                if (!String.IsNullOrEmpty(username) && username!="admin")
                {
                    table.DeleteMany(x => x.Username != null && x.Username == username);
                }
                else
                {
                    table.DeleteAll();
                }
            }
        }
    }
}
