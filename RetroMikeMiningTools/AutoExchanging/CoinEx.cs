using CoinEx.Net.Clients;
using CoinEx.Net.Objects;
using CryptoExchange.Net.Authentication;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;

namespace RetroMikeMiningTools.AutoExchanging
{
    public static class CoinExExchange
    {
        public static async Task Process(ExchangeConfig exchange)
        {
            if (String.IsNullOrEmpty(exchange?.ApiKey) || String.IsNullOrEmpty(exchange?.ApiSecret))
            {
                Common.Logger.Log(String.Format("Unable to execute Auto Exchanging Job for CoinEx due to missing API Keys"), LogType.AutoExchanging);
            }
            else
            {
                var client = new CoinExClient(new CoinExClientOptions()
                {
                    ApiCredentials = new ApiCredentials(exchange.ApiKey, exchange.ApiSecret),
                    LogLevel = LogLevel.Trace,
                    RequestTimeout = TimeSpan.FromSeconds(60)
                });
                var balances = await client.SpotApi.Account.GetBalancesAsync();
                var markets = await client.SpotApi.ExchangeData.GetSymbolInfoAsync();
                foreach (var balance in balances.Data.Where(x => !exchange.ExcludedSourceCoins.Any(y => y.Ticker==x.Key)))
                {
                    string ticker = balance.Key;
                    var balanceVal = balance.Value.Available;
                    if (balanceVal > 0.00m)
                    {
                        if (ticker != exchange.DestinationCoin?.Ticker && ticker != exchange.TradingPairCurrency?.Ticker)
                        {
                            var tickerMarketDirect = markets.Data.Where(x => x.Value.TradingName == ticker && x.Value.PricingName == exchange.DestinationCoin?.Ticker).FirstOrDefault();
                            if (tickerMarketDirect.Key != null)
                            {
                                var orderOperation = await client.SpotApi.Trading.PlaceOrderAsync(tickerMarketDirect.Key, CoinEx.Net.Enums.OrderSide.Sell, CoinEx.Net.Enums.OrderType.Market, balanceVal);
                                Common.Logger.Log(String.Format("CoinEx: Auto Exchanged {2} {0} for {3} {1}", ticker, exchange.TradingPairCurrency.Ticker, balanceVal, orderOperation?.Data?.QuoteQuantityFilled), LogType.AutoExchanging);
                            }
                            else
                            {
                                var tickerMarketIntermediate = markets.Data.Where(x => x.Value.TradingName == ticker && x.Value.PricingName == exchange.TradingPairCurrency.Ticker).FirstOrDefault();
                                if (tickerMarketIntermediate.Key != null)
                                {
                                    if (balanceVal > tickerMarketIntermediate.Value.MinQuantity)
                                    {
                                        var orderOperation = await client.SpotApi.Trading.PlaceOrderAsync(tickerMarketIntermediate.Key, CoinEx.Net.Enums.OrderSide.Sell, CoinEx.Net.Enums.OrderType.Market, balanceVal);
                                        Common.Logger.Log(String.Format("CoinEx: Auto Exchanged {2} {0} for {3} {1}", ticker, exchange.TradingPairCurrency.Ticker, balanceVal, orderOperation?.Data?.QuoteQuantityFilled), LogType.AutoExchanging);
                                    }
                                }
                            }
                        }
                    }
                }

                balances = await client.SpotApi.Account.GetBalancesAsync();
                var intermediateTradingBalance = balances.Data.Where(x => x.Key == exchange.TradingPairCurrency.Ticker).FirstOrDefault();
                if (intermediateTradingBalance.Key != null)
                {
                    var balanceVal = intermediateTradingBalance.Value.Available;
                    var finalMarket = markets.Data.Where(x => x.Value.TradingName == exchange.DestinationCoin.Ticker && x.Value.PricingName == exchange.TradingPairCurrency.Ticker).FirstOrDefault();
                    if (finalMarket.Key != null)
                    {
                        var orderBook = await client.SpotApi.ExchangeData.GetOrderBookAsync(finalMarket.Key, 1);
                        if (orderBook != null)
                        {
                            var projectedMarketPrice = orderBook?.Data?.Asks?.OrderBy(x => x.Price)?.Take(1)?.FirstOrDefault()?.Price;
                            var convertedBuyAmount = balanceVal / projectedMarketPrice;
                            if (convertedBuyAmount >= finalMarket.Value.MinQuantity)
                            {
                                var orderOperation = await client.SpotApi.Trading.PlaceOrderAsync(finalMarket.Key, CoinEx.Net.Enums.OrderSide.Buy, CoinEx.Net.Enums.OrderType.Market, balanceVal);
                                Common.Logger.Log(String.Format("CoinEx: Auto Exchanged {2} {0} for {3} {1}", exchange.TradingPairCurrency.Ticker, exchange.DestinationCoin.Ticker, orderOperation?.Data?.Quantity, convertedBuyAmount), LogType.AutoExchanging);
                            }
                        }
                    }
                }
            }
        }
    }
}
