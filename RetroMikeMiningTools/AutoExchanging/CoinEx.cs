using CoinEx.Net.Clients;
using CoinEx.Net.Objects;
using CoinEx.Net.Objects.Options;
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
                Common.Logger.Log(String.Format("Unable to execute Auto Exchanging Job for CoinEx due to missing API Keys"), LogType.AutoExchanging, exchange.Username);
            }
            else
            {
                var client = new CoinExRestClient(x =>
                {

                    x.ApiCredentials = new ApiCredentials(exchange.ApiKey, exchange.ApiSecret);
                    x.RequestTimeout = TimeSpan.FromSeconds(60);
                });

                var balances = await client.SpotApiV2.Account.GetBalancesAsync();
                var markets = await client.SpotApi.ExchangeData.GetSymbolInfoAsync();
                foreach (var balance in balances.Data.Where(x => exchange.ExcludedSourceCoins == null || !exchange.ExcludedSourceCoins.Any(y => y.Ticker.Equals(x.Asset, StringComparison.OrdinalIgnoreCase))))
                {
                    string ticker = balance.Asset;
                    var balanceVal = balance.Available;
                    if (balanceVal > 0.00m)
                    {
                        if (ticker != exchange.DestinationCoin?.Ticker && ticker != exchange.TradingPairCurrency?.Ticker)
                        {
                            var tickerMarketDirect = markets.Data.Where(x => x.Value.TradingName.Equals(ticker,StringComparison.OrdinalIgnoreCase) && x.Value.PricingName.Equals(exchange.DestinationCoin?.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (tickerMarketDirect.Key != null)
                            {
                                var orderOperation = await client.SpotApiV2.Trading.PlaceOrderAsync(tickerMarketDirect.Key, CoinEx.Net.Enums.AccountType.Spot, CoinEx.Net.Enums.OrderSide.Sell, CoinEx.Net.Enums.OrderTypeV2.Market, balanceVal);
                                Common.Logger.Log(String.Format("CoinEx: Auto Exchanged {2} {0} for {3} {1}", ticker, exchange.TradingPairCurrency.Ticker, balanceVal, orderOperation?.Data?.QuantityFilled), LogType.AutoExchanging, exchange.Username);
                            }
                            else
                            {
                                var tickerMarketIntermediate = markets.Data.Where(x => x.Value.TradingName.Equals(ticker, StringComparison.OrdinalIgnoreCase) && x.Value.PricingName.Equals(exchange.TradingPairCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                if (tickerMarketIntermediate.Key != null)
                                {
                                    if (balanceVal > tickerMarketIntermediate.Value.MinQuantity)
                                    {
                                        var orderOperation = await client.SpotApiV2.Trading.PlaceOrderAsync(tickerMarketIntermediate.Key, CoinEx.Net.Enums.AccountType.Spot, CoinEx.Net.Enums.OrderSide.Sell, CoinEx.Net.Enums.OrderTypeV2.Market, balanceVal);
                                        Common.Logger.Log(String.Format("CoinEx: Auto Exchanged {2} {0} for {3} {1}", ticker, exchange.TradingPairCurrency.Ticker, balanceVal, orderOperation?.Data?.QuantityFilled), LogType.AutoExchanging, exchange.Username);
                                    }
                                }
                            }
                        }
                    }
                }

                balances = await client.SpotApiV2.Account.GetBalancesAsync();
                var intermediateTradingBalance = balances.Data.Where(x => x.Asset.Equals(exchange.TradingPairCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (intermediateTradingBalance != null && intermediateTradingBalance.Asset != null)
                {
                    var balanceVal = intermediateTradingBalance.Available;
                    var finalMarket = markets.Data.Where(x => x.Value.TradingName.Equals(exchange.DestinationCoin.Ticker,StringComparison.OrdinalIgnoreCase) && x.Value.PricingName.Equals(exchange.TradingPairCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
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
                                Common.Logger.Log(String.Format("CoinEx: Auto Exchanged {2} {0} for {3} {1}", exchange.TradingPairCurrency.Ticker, exchange.DestinationCoin.Ticker, orderOperation?.Data?.Quantity, convertedBuyAmount), LogType.AutoExchanging, exchange.Username);
                            }
                        }
                    }
                }

                if (exchange.AutoWithdrawl && !String.IsNullOrEmpty(exchange.AutoWithdrawlAddress) && exchange.AutoWithdrawlCurrency != null)
                {
                    balances = await client.SpotApiV2.Account.GetBalancesAsync();
                    if (balances.Data.Where(x => x.Asset==exchange.AutoWithdrawlCurrency.Ticker).FirstOrDefault() != null)
                    {
                        var balanceRecord = balances.Data.Where(x => x.Asset.Equals(exchange.AutoWithdrawlCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (balanceRecord.Available > exchange.AutoWithdrawlMin)
                        {
                            var withdrawlAmount = Convert.ToDecimal(balanceRecord.Available - (exchange.WithdrawlFee ?? 0.00m));
                            var withdrawOperation = await client.SpotApiV2.Account.WithdrawAsync(balanceRecord.Asset, withdrawlAmount, exchange.AutoWithdrawlAddress);
                            if (withdrawOperation != null && withdrawOperation.Success)
                            {
                                Common.Logger.Log(String.Format("CoinEx: Auto Withdrawl Submitted for {0} {1}", withdrawlAmount, balanceRecord.Asset), LogType.AutoExchanging, exchange.Username);
                            }
                            else if(withdrawOperation != null && !withdrawOperation.Success)
                            {
                                Common.Logger.Log("CoinEx: Withdraw Error: " + withdrawOperation.Error, LogType.AutoExchanging, exchange.Username);
                            }
                        }
                    }
                }
            }
        }
    }
}
