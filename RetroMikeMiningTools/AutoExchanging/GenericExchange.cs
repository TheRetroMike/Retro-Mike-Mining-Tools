using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.AutoExchanging
{
    public static class GenericExchange
    {
        public static void Process(ExchangeConfig? exchange, string apiBasePath, string tradingFee)
        {
            if (String.IsNullOrEmpty(exchange?.ApiKey) || String.IsNullOrEmpty(exchange?.ApiSecret))
            {
                Common.Logger.Log(String.Format("Unable to execute Auto Exchanging Job due to missing API Keys"), LogType.AutoExchanging, exchange.Username);
            }
            else
            {
                GenericTradeForTradingCurrency(true, exchange, apiBasePath, tradingFee);
                Thread.Sleep(new TimeSpan(0, 0, 5));
                if (exchange.DestinationCoin != exchange.TradingPairCurrency)
                {
                    GenericTradeForTradingCurrency(false, exchange, apiBasePath, tradingFee);
                }
                if (exchange.AutoWithdrawl && !String.IsNullOrEmpty(exchange.AutoWithdrawlAddress) && exchange.AutoWithdrawlCurrency != null)
                {
                    Withdraw(exchange, apiBasePath);
                }
            }
        }

        private static void Withdraw(ExchangeConfig exchange, string apiBasePath)
        {
            var balance = GenericExchangeApiUtilities.GetWalletBalances(apiBasePath, exchange).Where(x => x.Ticker.Equals(exchange.AutoWithdrawlCurrency.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (balance != null && balance.Balance >= exchange.AutoWithdrawlMin)
            {
                if (exchange.WithdrawlFee == null)
                {
                    exchange.WithdrawlFee = 0.00m;
                }
                GenericExchangeApiUtilities.Withdraw(exchange, apiBasePath, exchange.AutoWithdrawlCurrency.Ticker, (balance.Balance).ToString(), exchange.AutoWithdrawlAddress);
                Common.Logger.Log(String.Format("{0}: Auto Withdrawl Submitted", exchange.Exchange.ToString()), LogType.AutoExchanging, exchange.Username);
            }
        }

        private static void GenericTradeForTradingCurrency(bool tradingCurrencyStep, ExchangeConfig exchange, string apiBasePath, string tradingFee)
        {
            var excludedCoins = exchange.ExcludedSourceCoins ?? new List<Coin>();
            var markets = GenericExchangeApiUtilities.GetMarkets(apiBasePath);
            var balances = GenericExchangeApiUtilities.GetWalletBalances(apiBasePath, exchange);
            foreach (var balance in balances.Where(x => x.Balance > 0.00m))
            {
                if (!excludedCoins.Any(x => x.Ticker.Equals(balance.Ticker, StringComparison.OrdinalIgnoreCase)) && !balance.Ticker.Equals(exchange.DestinationCoin.Ticker,StringComparison.OrdinalIgnoreCase) && (!tradingCurrencyStep || (tradingCurrencyStep && !balance.Ticker.Equals(exchange.TradingPairCurrency?.Ticker, StringComparison.OrdinalIgnoreCase))))
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
                            var rate = GenericExchangeApiUtilities.GetExchangeRate(apiBasePath, tradingCurrencyStep, tradingMarket.MarketName);
                            var orderResult = GenericExchangeApiUtilities.SubmitTradeOrder(apiBasePath, tradingCurrencyStep, tradingMarket.MarketName, balance.Balance, exchange, rate, Convert.ToDecimal(tradingMarket.MinTradeSize), tradingFee);
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
