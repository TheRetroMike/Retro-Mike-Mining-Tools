using CoinEx.Net.Clients;
using CoinEx.Net.Objects;
using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public static class CoinExUtilities
    {
        public static async Task<List<Coin>> GetTickers()
        {
            List<Coin> result = new List<Coin>();
            var client = new CoinExClient(new CoinExClientOptions()
            {
                LogLevel = LogLevel.Error,
                RequestTimeout = TimeSpan.FromSeconds(60)
            });
            var coinData = await client.SpotApi.ExchangeData.GetSymbolInfoAsync("");
            if (coinData != null && coinData.Data != null)
            {
                foreach (var item in coinData.Data)
                {
                    var tradingCurrencyName = item.Value.TradingName;
                    var pricingCurrencyName = item.Value.PricingName;
                    if (!result.Any(x => x.Ticker==tradingCurrencyName))
                    {
                        result.Add(new Coin() { Ticker = tradingCurrencyName, Exchange = Enums.Exchange.CoinEx, Name = tradingCurrencyName });
                    }
                    if (!result.Any(x => x.Ticker == pricingCurrencyName))
                    {
                        result.Add(new Coin() { Ticker = pricingCurrencyName, Exchange = Enums.Exchange.CoinEx, Name = pricingCurrencyName });
                    }
                }
            } 
            return result;
        }
    }
}
