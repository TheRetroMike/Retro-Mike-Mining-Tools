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

        [DataType(DataType.Currency)]
        public decimal Profit { get; set; }

        public decimal AdditionalPower { get; set; }

        public bool Enabled { get; set; }

        public string? Username { get; set; }
    }
}
