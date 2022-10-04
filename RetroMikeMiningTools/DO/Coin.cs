namespace RetroMikeMiningTools.DO
{
    public class Coin
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Algorithm { get; set; }
        public string CoinRevenue { get; set; }
        public string BtcRevenue { get; set; }
        public string PowerConsumption { get; set; }
        public string WhatToMineEndpoint { get; set; }
        public string HashRate { get; set; }
        public Enums.Exchange Exchange { get; set; }
        public string? SecondaryTicker { get; set; }
        public int? WtmId { get; set; }
        public string HashRateFactor { get; set; }
        public string Provider { get; set; }
    }
}
