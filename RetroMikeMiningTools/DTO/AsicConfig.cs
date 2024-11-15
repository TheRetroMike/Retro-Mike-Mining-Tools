using RetroMikeMiningTools.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class AsicConfig
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }

        [UIHint("AsicTypeEditor")]
        public AsicType AsicType { get; set; }

        [UIHint("MiningModeEditor")]
        public MiningMode MiningMode { get; set; }

        public string? DeviceIP { get; set; }

        [DataType(DataType.Currency)]
        public decimal Profit { get; set; }

        public decimal AdditionalPower { get; set; }

        public bool Enabled { get; set; }

        public string? Username { get; set; }

        [UIHint("MiningCoinEditor")]
        public string? PinnedTicker { get; set; }

        [UIHint("SmartPlugEditor")]
        public SmartPlugType SmartPlugType { get; set; }
        public string? SmartPlugHost { get; set; }

        [DataType(DataType.Currency)]
        public decimal? RigMinProfit { get; set; }

        [DataType(DataType.Password)]
        public string? ApiKey { get; set; }
    }
}
