using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace RetroMikeMiningTools.AutoExchanging
{
    public static class SouthXchange
    {
        public static async void Process(ExchangeConfig exchange)
        {
            if (String.IsNullOrEmpty(exchange?.ApiKey) || String.IsNullOrEmpty(exchange?.ApiSecret))
            {
                Common.Logger.Log(String.Format("Unable to execute Auto Exchanging Job for South Xchange due to missing API Keys"), Enums.LogType.AutoExchanging, exchange.Username);
            }
            else
            {
                HandleTrading(exchange, true);
                Thread.Sleep(new TimeSpan(0, 0, 5));
                if (exchange.DestinationCoin != exchange.TradingPairCurrency)
                {
                    HandleTrading(exchange, false);
                }
            }
        }

        private static void HandleTrading(ExchangeConfig exchange, bool tradingCurrencyStep)
        {
            var markets = SouthXchangeUtilities.GetMarkets();
            var excludedCoins = exchange.ExcludedSourceCoins ?? new List<Coin>();
            var balances = SouthXchangeUtilities.GetBalances(exchange).Where(x => x.Balance > 0.00m).ToList();
            if (balances != null && balances.Count > 0)
            {
                foreach (var balance in balances)
                {
                    if (!excludedCoins.Any(x => x.Ticker.Equals(balance.Ticker, StringComparison.OrdinalIgnoreCase)) && !balance.Ticker.Equals(exchange.DestinationCoin.Ticker, StringComparison.OrdinalIgnoreCase) && (!tradingCurrencyStep || (tradingCurrencyStep && !balance.Ticker.Equals(exchange.TradingPairCurrency?.Ticker, StringComparison.OrdinalIgnoreCase))))
                    {
                        var currencyMarkets = new List<ExchangeMarket>();
                        if (tradingCurrencyStep)
                        {
                            currencyMarkets = markets.Where(x => x.MarketCurrency.Equals(balance.Ticker, StringComparison.OrdinalIgnoreCase) && x.BaseCurrency.Equals(exchange.TradingPairCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                        else
                        {
                            currencyMarkets = markets.Where(x => x.MarketCurrency.Equals(exchange.DestinationCoin.Ticker, StringComparison.OrdinalIgnoreCase) && x.BaseCurrency.Equals(balance.Ticker, StringComparison.OrdinalIgnoreCase)).ToList();
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
                                        rate = decimal.Parse(tradingMarket.Bid, System.Globalization.NumberStyles.Float);
                                        balance.Balance = MathUtilities.RoundDown(Convert.ToDecimal(balance.Balance), 10);
                                    }
                                    else
                                    {
                                        rate = decimal.Parse(tradingMarket.Ask, System.Globalization.NumberStyles.Float);
                                        var tradingFeeVal = decimal.Parse(Constants.SOUTH_XCHANGE_TRADE_FEE.TrimEnd(new char[] { '%', ' ' })) / 100M;
                                        balance.Balance = MathUtilities.RoundDown((balance.Balance * (1m - tradingFeeVal)) / rate, 4);
                                    }

                                    var tradeAmount = rate * balance.Balance;

                                    if (tradeAmount > Convert.ToDecimal(tradingMarket.MinTradeSize))
                                    {
                                        var orderResult = SouthXchangeUtilities.PlaceTradeOrder(exchange, tradingCurrencyStep, tradingMarket.BaseCurrency, tradingMarket.MarketCurrency, balance.Balance, rate, tradingMarket.MarketName).Result;
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
        }
    }
}
