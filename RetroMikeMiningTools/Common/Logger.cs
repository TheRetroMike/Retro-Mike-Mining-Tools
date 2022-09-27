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

        public static void Log(string message, LogType logType, string username)
        {
            LogDAO.Add(new LogEntry()
            {
                LogMessage = message,
                LogDateTime = DateTime.Now,
                LogType = logType,
                Username = username
            });
        }

        public static void Push(string message)
        {
            try
            {
                Send(message).Wait();
            }
            catch { }
        }

        public static async Task Send(string message)
        {
            var client = new PushoverNet.PushoverClient(Constants.PUSHOVER_APP_KEY);
            await client.SendAsync(Constants.PUSHOVER_USER_KEY, message, "Retro Mike Mining Tools");
        }
    }
}
