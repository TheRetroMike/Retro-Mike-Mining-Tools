using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class AsicCoinConfig
    {
        public AsicCoinConfig()
        {
            this.Groups = new List<LinkedGroup>();
        }

        [Key]
        public int Id { get; set; }

        public int WorkerId { get; set; }

        [UIHint("MiningCoinEditor")]
        public string Ticker { get; set; }

        public string? Pool { get; set; }
        public string? PoolUser { get; set; }
        public string? PoolPassword { get; set; }

        public bool Enabled { get; set; }

        public double? DesiredBalance { get; set; }

        [UIHint("GroupListEditor")]
        public List<LinkedGroup>? Groups { get; set; }

        [DataType(DataType.Currency)]
        public decimal Profit { get; set; }

        public string? Username { get; set; }

        public decimal HashRateMH { get; set; }
        
        public decimal Power { get; set; }
        
        public string Algo { get; set; }
        
        public decimal CoinRevenue { get; set; }

        public string? OverrideEndpoint { get; set; }
        
        public string? PrimaryProvider { get; set; }

    }
}
