using RetroMikeMiningTools.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class LogEntry
    {
        [Key]
        public int Id { get; set; }
        public DateTime LogDateTime { get; set; }
        public string LogMessage { get; set; }
        public LogType LogType { get; set; }
    }
}
