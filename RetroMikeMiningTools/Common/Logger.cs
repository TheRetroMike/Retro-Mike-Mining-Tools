using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;

namespace RetroMikeMiningTools.Common
{
    public static class Logger
    {
        public static void Log(string message, LogType logType)
        {
            LogDAO.Add(new LogEntry()
            {
                LogMessage = message,
                LogDateTime = DateTime.Now,
                LogType = logType
            });
        }
    }
}
