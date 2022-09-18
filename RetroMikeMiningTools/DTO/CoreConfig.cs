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
        public string? CoinDeskApi { get; set; }
        public bool ProfitSwitchingEnabled { get; set; }
        public bool AutoExchangingEnabled { get; set; }
        public string? ProfitSwitchingCronSchedule { get; set; }
        public string? IgnoredVersion { get; set; }
    }
}
