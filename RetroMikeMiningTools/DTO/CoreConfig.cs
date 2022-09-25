using RetroMikeMiningTools.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class CoreConfig
    {
        public int Id { get; set; }
        [DataType(DataType.Password)]
        public string? HiveApiKey { get; set; }
        public string? HiveFarmID { get; set; }
        public string? CoinDifferenceThreshold { get; set; }
        public bool ProfitSwitchingEnabled { get; set; }
        public bool AutoExchangingEnabled { get; set; }
        public string? ProfitSwitchingCronSchedule { get; set; }
        public string? IgnoredVersion { get; set; }
        public int Port { get; set; }
        public string? DockerHostIp { get; set; }
        public string? AutoExchangingCronSchedule { get; set; }
        public ReleaseType ReleaseType { get; set; }
        public decimal DefaultPowerPrice { get; set; }
    }
}
