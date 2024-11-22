using CoinEx.Net.Clients;
using CoinEx.Net.Objects;
using CryptoExchange.Net.Authentication;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;

namespace RetroMikeMiningTools.Utilities
{
    public static class CoinExUtilities
    {
        public static async Task<List<Coin>> GetTickers()
        {
            List<Coin> result = new List<Coin>();
            var client = new CoinExRestClient(x =>
            {
                x.RequestTimeout = TimeSpan.FromSeconds(60);
            });
            var coinData = await client.SpotApi.ExchangeData.GetSymbolInfoAsync("");
            if (coinData != null && coinData.Data != null)
            {
                foreach (var item in coinData.Data)
                {
                    var tradingCurrencyName = item.Value.TradingName;
                    var pricingCurrencyName = item.Value.PricingName;
                    if (!result.Any(x => x.Ticker.Equals(tradingCurrencyName,StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Add(new Coin() { Ticker = tradingCurrencyName, Exchange = Enums.Exchange.CoinEx, Name = tradingCurrencyName });
                    }
                    if (!result.Any(x => x.Ticker.Equals(pricingCurrencyName, StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Add(new Coin() { Ticker = pricingCurrencyName, Exchange = Enums.Exchange.CoinEx, Name = pricingCurrencyName });
                    }
                }
            } 
            return result;
        }

        public static async Task<List<ExchangeBalance>> GetBalances(ExchangeConfig exchange)
        {
            List<ExchangeBalance> result = new List<ExchangeBalance>();
            var client = new CoinExRestClient(x =>
            {
                x.ApiCredentials = new ApiCredentials(exchange.ApiKey, exchange.ApiSecret);
                x.RequestTimeout = TimeSpan.FromSeconds(60);
            });
            var markets = await client.SpotApi.ExchangeData.GetSymbolInfoAsync();
            var balances = await client.SpotApiV2.Account.GetBalancesAsync();
            if (balances != null && balances.Data != null && balances.Data.Count() > 0)
            {
                foreach (var item in balances.Data)
                {
                    var usdtAmount = 0.00;
                    //var currencyRates = await client.SpotApi.ExchangeData.GetTickersAsync();
                    var tickerMarketDirect = markets.Data.Where(x => x.Value.TradingName.Equals(item.Asset, StringComparison.OrdinalIgnoreCase) && x.Value.PricingName.Equals("USDT", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (item.Available > 0)
                    {
                        result.Add(new ExchangeBalance() { Balance = item.Available, Ticker = item.Asset, BalanceDisplayVal = item.Available.ToString() });
                    }
                }
            }
            return result;
        }
    }
}
