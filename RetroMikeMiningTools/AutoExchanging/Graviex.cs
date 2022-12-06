using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.AutoExchanging
{
    public static class Graviex
    {
        public static async void Process(ExchangeConfig exchange)
        {
            if (String.IsNullOrEmpty(exchange?.ApiKey) || String.IsNullOrEmpty(exchange?.ApiSecret))
            {
                Common.Logger.Log(String.Format("Unable to execute Auto Exchanging Job for Graviex due to missing API Keys"), Enums.LogType.AutoExchanging, exchange.Username);
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
            var markets = GraviexExchangeApiUtilities.GetMarkets();
            var excludedCoins = exchange.ExcludedSourceCoins ?? new List<Coin>();
            var balances = GraviexExchangeApiUtilities.GetWalletBalances(exchange).Where(x => x.Balance > 0.00m).ToList();
            if (balances != null && balances.Count > 0)
            {
                foreach (var balance in balances.Where(x => x.Balance > 0.00m))
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
                            if (tradingMarket != null)
                            {
                                var rate = 0.00m;
                                if (tradingCurrencyStep)
                                {
                                    rate = Convert.ToDecimal(tradingMarket.Ask);
                                }
                                else
                                {
                                    rate = Convert.ToDecimal(tradingMarket.Bid);
                                }

                                var orderResult = GraviexExchangeApiUtilities.SubmitTradeOrder(tradingCurrencyStep, tradingMarket.MarketName, balance.Balance, exchange, rate, Convert.ToDecimal(tradingMarket.MinTradeSize), tradingMarket.Fee);
                                if (!String.IsNullOrEmpty(orderResult))
                                {
                                    Common.Logger.Log(orderResult, LogType.AutoExchanging, exchange.Username);
                                }
                            }
                        }
                    }
                }
            }

            if (exchange.AutoWithdrawl && !String.IsNullOrEmpty(exchange.AutoWithdrawlAddress) && exchange.AutoWithdrawlCurrency != null)
            {
                var balanceRecord = GraviexExchangeApiUtilities.GetWalletBalances(exchange).Where(x => x.Ticker.Equals(exchange.AutoWithdrawlCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (balanceRecord != null)
                {
                    if (balanceRecord.Balance > exchange.AutoWithdrawlMin)
                    {
                       GraviexExchangeApiUtilities.Withdraw(exchange, Convert.ToString(balanceRecord.Balance-exchange.WithdrawlFee ?? 0.00m));
                    }
                }
            }
        }
    }
}
