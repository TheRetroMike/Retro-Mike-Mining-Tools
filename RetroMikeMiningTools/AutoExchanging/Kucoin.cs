using CoinEx.Net.Clients;
using CoinEx.Net.Objects;
using CryptoExchange.Net.Authentication;
using Kucoin.Net.Enums;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.AutoExchanging
{
    public static class KucoinExchange
    {
        public static async Task Process(ExchangeConfig exchange)
        {
            if (String.IsNullOrEmpty(exchange?.ApiKey) || String.IsNullOrEmpty(exchange?.ApiSecret) || String.IsNullOrEmpty(exchange?.Passphrase))
            {
                Common.Logger.Log(String.Format("Unable to execute Auto Exchanging Job for Kucoin due to missing API Keys and/or Passphrase"), LogType.AutoExchanging, exchange.Username);
            }
            else
            {
                var client = new Kucoin.Net.Clients.KucoinClient(new Kucoin.Net.Objects.KucoinClientOptions()
                {
                    ApiCredentials = new Kucoin.Net.Objects.KucoinApiCredentials(exchange.ApiKey, exchange.ApiSecret, exchange.Passphrase),
                    LogLevel = LogLevel.Trace,
                    RequestTimeout = TimeSpan.FromSeconds(60)
                });
                var balances = await client.SpotApi.Account.GetAccountsAsync();

                if (exchange.AutoMoveToTradingAccount)
                {
                    var mainAccountBalances = balances.Data.Where(x => x.Type == AccountType.Main);
                    foreach (var item in mainAccountBalances)
                    {
                        await client.SpotApi.Account.InnerTransferAsync(item.Asset, AccountType.Main, AccountType.Trade, item.Available);
                    }
                }

                balances = await client.SpotApi.Account.GetAccountsAsync();
                var markets = await client.SpotApi.ExchangeData.GetSymbolsAsync();
                foreach (var balance in balances.Data.Where(x => x.Type == AccountType.Trade && !exchange.ExcludedSourceCoins.Any(y => y.Ticker.Equals(x.Asset, StringComparison.OrdinalIgnoreCase))))
                {
                    string ticker = balance.Asset;
                    var balanceVal = balance.Available;
                    if (balanceVal > 0.00m)
                    {
                        if (ticker != exchange.DestinationCoin?.Ticker && ticker != exchange.TradingPairCurrency?.Ticker)
                        {
                            var tickerMarketDirect = markets.Data.Where(x => x.BaseAsset.Equals(ticker, StringComparison.OrdinalIgnoreCase) && x.QuoteAsset.Equals(exchange.DestinationCoin?.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (tickerMarketDirect?.BaseAsset != null)
                            {
                                var orderOperation = await client.SpotApi.Trading.PlaceOrderAsync(tickerMarketDirect.Symbol, Kucoin.Net.Enums.OrderSide.Sell, Kucoin.Net.Enums.NewOrderType.Market, balanceVal);
                                Common.Logger.Log(String.Format("Kucoin: Auto Exchanged {0} for {1}", ticker, exchange.TradingPairCurrency.Ticker), LogType.AutoExchanging, exchange.Username);
                            }
                            else
                            {
                                var tickerMarketIntermediate = markets.Data.Where(x => x.BaseAsset.Equals(ticker, StringComparison.OrdinalIgnoreCase) && x.QuoteAsset.Equals(exchange.TradingPairCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                if (tickerMarketIntermediate?.BaseAsset != null)
                                {
                                    if (balanceVal > tickerMarketIntermediate.BaseMinQuantity)
                                    {
                                        var orderOperation = await client.SpotApi.Trading.PlaceOrderAsync(tickerMarketIntermediate.Symbol, Kucoin.Net.Enums.OrderSide.Sell, Kucoin.Net.Enums.NewOrderType.Market, balanceVal);
                                        Common.Logger.Log(String.Format("Kucoin: Auto Exchanged {0} for {1}", ticker, exchange.TradingPairCurrency.Ticker), LogType.AutoExchanging, exchange.Username);
                                    }
                                }
                            }
                        }
                    }
                }

                balances = await client.SpotApi.Account.GetAccountsAsync();
                var intermediateTradingBalance = balances.Data.Where(x => x.Type == AccountType.Trade && x.Asset.Equals(exchange.TradingPairCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (intermediateTradingBalance?.Asset != null)
                {
                    var balanceVal = intermediateTradingBalance.Available;
                    var finalMarket = markets.Data.Where(x => x.BaseAsset.Equals(exchange.DestinationCoin.Ticker, StringComparison.OrdinalIgnoreCase) && x.QuoteAsset.Equals(exchange.TradingPairCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (finalMarket?.BaseAsset != null)
                    {
                        var orderBook = await client.SpotApi.ExchangeData.GetAggregatedPartialOrderBookAsync(finalMarket.Symbol, 20);
                        if (orderBook != null)
                        {
                            var projectedMarketPrice = orderBook?.Data?.Asks?.OrderBy(x => x.Price)?.Take(1)?.FirstOrDefault()?.Price;
                            var convertedBuyAmount = balanceVal / projectedMarketPrice;
                            convertedBuyAmount = MathUtilities.RoundDown(Convert.ToDecimal(convertedBuyAmount), MathUtilities.GetDecimalPlaces(Convert.ToDecimal(finalMarket.BaseIncrement)));
                            if (convertedBuyAmount >= finalMarket.BaseMinQuantity)
                            {
                                
                                var orderOperation = await client.SpotApi.Trading.PlaceOrderAsync(finalMarket.Symbol, OrderSide.Buy, NewOrderType.Market, convertedBuyAmount);
                                Common.Logger.Log(String.Format("Kucoin: Auto Exchanged {0} for {1}", exchange.TradingPairCurrency.Ticker, exchange.DestinationCoin.Ticker), LogType.AutoExchanging, exchange.Username);
                            }
                        }
                    }
                }

                if (exchange.AutoWithdrawl && !String.IsNullOrEmpty(exchange.AutoWithdrawlAddress) && exchange.AutoWithdrawlCurrency != null)
                {
                    balances = await client.SpotApi.Account.GetAccountsAsync();
                    var balanceRecord = balances.Data.Where(x => x.Asset.Equals(exchange.AutoWithdrawlCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (balanceRecord != null)
                    {
                        if (balanceRecord.Available > exchange.AutoWithdrawlMin)
                        {
                            var withdrawlAmount = Convert.ToDecimal(balanceRecord.Available - (exchange.WithdrawlFee ?? 0.00m));
                            await client.SpotApi.Account.WithdrawAsync(balanceRecord.Asset, exchange.AutoWithdrawlAddress, withdrawlAmount);
                            Common.Logger.Log("Kucoin: Auto Withdrawl Submitted", LogType.AutoExchanging, exchange.Username);
                        }
                    }
                }
            }
        }
    }
}
