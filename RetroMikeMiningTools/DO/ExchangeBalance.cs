using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DO
{
    public class ExchangeBalance
    {
        public string Ticker { get; set; }
        public decimal Balance { get; set; }
        public string BalanceDisplayVal { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal UsdDisplayVal { get; set; }
    }
}
