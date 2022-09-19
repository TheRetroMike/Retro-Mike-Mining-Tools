using RetroMikeMiningTools.DO;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class GoldshellAsicCoinConfig
    {
        public GoldshellAsicCoinConfig()
        {
            this.Groups = new List<LinkedGroup>();
            this.MergedTickers = new List<Coin>();
        }

        [Key]
        public int Id { get; set; }

        public int WorkerId { get; set; }

        [UIHint("AsicMiningCoinEditor")]
        public string Ticker { get; set; }

        public bool Enabled { get; set; }

        [UIHint("GroupListEditor")]
        public List<LinkedGroup>? Groups { get; set; }

        [UIHint("MergeCoinEditor")]
        public List<Coin>? MergedTickers { get; set; }

        public string? PoolUrl { get; set; }
        public string? PoolUser { get; set; }
        public string? PoolPassword { get; set; }
        public string? CoinWhatToMineEndpoint { get; set; }
        public string? CoinAlgo { get; set; }
    }
}
