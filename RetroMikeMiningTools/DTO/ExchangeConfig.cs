using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class ExchangeConfig
    {
        [Key]
        public int Id { get; set; }

        [UIHint("ExchangeEditor")]
        public Enums.Exchange Exchange { get; set; }
        
        public  bool Enabled { get; set; }
        
        [DataType(DataType.Password)]
        public string? ApiKey { get; set; }

        [DataType(DataType.Password)]
        public string? ApiSecret { get; set; }

        [DataType(DataType.Password)]
        public string? Passphrase { get; set; }

        [UIHint("ExchangeCoinEditor")]

        public Coin DestinationCoin { get; set; }
        
        [UIHint("ExchangeMultiCoinEditor")]
        public List<Coin>? ExcludedSourceCoins { get; set; }

        [UIHint("ExchangeCoinEditor")]
        public Coin? TradingPairCurrency { get; set; }

        public bool AutoMoveToTradingAccount { get; set; }
        public string? Username { get; set; }
        public bool AutoWithdrawl { get; set; }

        [UIHint("ExchangeCoinEditor")]
        public Coin? AutoWithdrawlCurrency { get; set; }
        public string? AutoWithdrawlAddress { get; set; }
        public decimal? AutoWithdrawlMin { get; set; }
        public decimal? WithdrawlFee { get; set; }
    }
}
