using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.AutoExchanging
{
    public static class Xeggex
    {
        public static void Process(ExchangeConfig exchange)
        {
            if (String.IsNullOrEmpty(exchange?.ApiKey) || String.IsNullOrEmpty(exchange?.ApiSecret))
            {
                Common.Logger.Log(String.Format("Unable to execute Auto Exchanging Job for Xeggex due to missing API Keys"), LogType.AutoExchanging, exchange.Username);
            }
            else
            {
                XeggexTradeForTradingCurrency(exchange, true);
                Thread.Sleep(new TimeSpan(0, 0, 5));
                if (exchange.DestinationCoin != exchange.TradingPairCurrency)
                {
                    XeggexTradeForTradingCurrency(exchange, false);
                }
            }
        }

        private static void XeggexTradeForTradingCurrency(ExchangeConfig exchange, bool tradingCurrencyStep)
        {
            var markets = XeggexUtilities.GetMarkets();
            var excludedCoins = exchange.ExcludedSourceCoins ?? new List<Coin>();
            var balances = XeggexUtilities.GetBalances(exchange).Where(x => x.Balance > 0.00m).ToList();
            if (balances != null && balances.Count > 0)
            {
                foreach (var balance in balances)
                {
                    if (!excludedCoins.Any(x => x.Ticker.Equals(balance.Ticker, StringComparison.OrdinalIgnoreCase)) && !balance.Ticker.Equals(exchange.DestinationCoin.Ticker, StringComparison.OrdinalIgnoreCase) && (!tradingCurrencyStep || (tradingCurrencyStep && !balance.Ticker.Equals(exchange.TradingPairCurrency?.Ticker,StringComparison.OrdinalIgnoreCase))))
                    {
                        var currencyMarkets = new List<ExchangeMarket>();
                        if (tradingCurrencyStep)
                        {
                            currencyMarkets = markets.Where(x => x.MarketCurrency.Equals(exchange.TradingPairCurrency.Ticker, StringComparison.OrdinalIgnoreCase) && x.BaseCurrency.Equals(balance.Ticker, StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                        else
                        {
                            currencyMarkets = markets.Where(x => x.MarketCurrency.Equals(balance.Ticker, StringComparison.OrdinalIgnoreCase) && x.BaseCurrency.Equals(exchange.DestinationCoin.Ticker, StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                        if (currencyMarkets != null && currencyMarkets.Count == 1)
                        {
                            var tradingMarket = currencyMarkets.FirstOrDefault();
                            {
                                if (tradingMarket != null)
                                {
                                    decimal rate = 0m;
                                    if (tradingCurrencyStep)
                                    {
                                        rate = Convert.ToDecimal(tradingMarket.Bid);
                                        balance.Balance = MathUtilities.RoundDown(Convert.ToDecimal(balance.Balance), 10);
                                    }
                                    else
                                    {
                                        rate = Convert.ToDecimal(tradingMarket.Ask);
                                        var tradingFeeVal = decimal.Parse(Constants.XEGGEX_TRADE_FEE.TrimEnd(new char[] { '%', ' ' })) / 100M;
                                        balance.Balance = MathUtilities.RoundDown((balance.Balance * (1m - tradingFeeVal)) / rate, 4);
                                    }

                                    var tradeAmount = rate * balance.Balance;

                                    if (balance.Balance > Convert.ToDecimal(tradingMarket.MinTradeSize))
                                    {
                                        var orderResult = XeggexUtilities.PlaceTradeOrder(exchange, tradingCurrencyStep, tradingMarket.BaseCurrency, tradingMarket.MarketCurrency, balance.Balance, rate, tradingMarket.MarketName);
                                        if (!String.IsNullOrEmpty(orderResult))
                                        {
                                            Common.Logger.Log(orderResult, LogType.AutoExchanging, exchange.Username);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (exchange.AutoWithdrawl && !String.IsNullOrEmpty(exchange.AutoWithdrawlAddress) && exchange.AutoWithdrawlCurrency != null)
            {
                var balanceRecord = XeggexUtilities.GetBalances(exchange).Where(x => x.Ticker.Equals(exchange.AutoWithdrawlCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (balanceRecord != null)
                {
                    if (balanceRecord.Balance > exchange.AutoWithdrawlMin)
                    {
                        XeggexUtilities.Withdraw(exchange, balanceRecord.Balance);
                    }
                }
            }
        }
    }
}