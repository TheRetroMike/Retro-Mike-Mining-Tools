using CryptoExchange.Net.Authentication;
using Kucoin.Net.Clients;
using Kucoin.Net.Objects;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;

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

        public static async Task<List<ExchangeBalance>> GetBalances(ExchangeConfig exchange)
        {
            List<ExchangeBalance> result = new List<ExchangeBalance>();
            var client = new KucoinClient(new KucoinClientOptions()
            {
                LogLevel = LogLevel.Error,
                RequestTimeout = TimeSpan.FromSeconds(60),
                ApiCredentials = new KucoinApiCredentials(exchange.ApiKey, exchange.ApiSecret, exchange.Passphrase),
            });
            var balances = await client.SpotApi.Account.GetAccountsAsync();
            foreach (var item in balances.Data)
            {
                if (item.Available > 0)
                {
                    result.Add(new ExchangeBalance() { Balance = item.Available, Ticker = item.Asset, BalanceDisplayVal = item.Available.ToString() });
                }
            }
            return result;
        }
    }
}
