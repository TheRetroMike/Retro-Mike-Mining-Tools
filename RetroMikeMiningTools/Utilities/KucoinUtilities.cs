using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Utilities
{
    public class KucoinUtilities
    {
        public static async Task<List<Coin>> GetTickers()
        {
            List<Coin> result = new List<Coin>();
            var client = new Kucoin.Net.Clients.KucoinClient(new Kucoin.Net.Objects.KucoinClientOptions()
            {
                LogLevel = LogLevel.Error,
                RequestTimeout = TimeSpan.FromSeconds(60)
            });
            var coinData = await client.SpotApi.ExchangeData.GetSymbolsAsync(null);
            if (coinData != null && coinData.Data != null)
            {
                foreach (var item in coinData.Data)
                {
                    var tradingCurrencyName = item.BaseAsset;
                    var pricingCurrencyName = item.QuoteAsset;
                    if (!result.Any(x => x.Ticker.Equals(tradingCurrencyName, StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Add(new Coin() { Ticker = tradingCurrencyName, Exchange = Enums.Exchange.Kucoin, Name = tradingCurrencyName });
                    }
                    if (!result.Any(x => x.Ticker.Equals(pricingCurrencyName, StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Add(new Coin() { Ticker = pricingCurrencyName, Exchange = Enums.Exchange.Kucoin, Name = pricingCurrencyName });
                    }
                }
            }
            return result;
        }
    }
}
