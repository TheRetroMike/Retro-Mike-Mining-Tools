using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Utilities
{
    public static class GraviexExchangeApiUtilities
    {
        public static List<Coin> GetTickers()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient(Constants.GRAVIEX_API_BASE_PATH);
            var request = new RestRequest("/tickers.json", Method.Get);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null)
            {
                foreach (var item in responseContent)
                {
                    foreach (var subItem in item)
                    {
                        if (!result.Any(x => x.Ticker== subItem.base_unit.ToString().ToUpper()))
                        {
                            //result.Add(new Coin() { Exchange = Enums.Exchange.Graviex, Ticker = subItem.base_unit.ToString().ToUpper(), Name = subItem.base_unit.ToString().ToUpper() });
                        }

                        if (!result.Any(x => x.Ticker == subItem.quote_unit.ToString().ToUpper()))
                        {
                            //result.Add(new Coin() { Exchange = Enums.Exchange.Graviex, Ticker = subItem.quote_unit.ToString().ToUpper(), Name = subItem.quote_unit.ToString().ToUpper() });
                        }
                    }
                }
            }
            return result;
        }

        public static List<ExchangeMarket> GetMarkets()
        {
            List<ExchangeMarket> markets = new List<ExchangeMarket>();
            var client = new RestClient(Constants.GRAVIEX_API_BASE_PATH);
            var request = new RestRequest("/tickers.json", Method.Get);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null)
            {
                foreach (var item in responseContent)
                {
                    var marketName = item.Path;
                    foreach (var subItem in item)
                    {
                        markets.Add(new ExchangeMarket()
                        {
                            BaseCurrency = Convert.ToString(subItem.quote_unit.ToString()),
                            MarketCurrency = Convert.ToString(subItem.base_unit.ToString()),
                            MarketName = Convert.ToString(marketName),
                            MinTradeSize = Convert.ToString(subItem.base_min.ToString()),
                            Fee = Convert.ToString(subItem.base_fee.ToString()),
                            Ask = Convert.ToString(subItem.buy.ToString()),
                            Bid = Convert.ToString(subItem.sell.ToString()),
                        });
                    }
                }
            }
            return markets;
        }

        public static List<ExchangeBalance> GetWalletBalances(ExchangeConfig exchangeConfig)
        {
            List<ExchangeBalance> result = new List<ExchangeBalance>();

            RestClient client = new RestClient(Constants.GRAVIEX_API_BASE_PATH);
            var resourcePrefix = "/webapi/v3";
            var resource = "/members/me";

            var nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var queryString = String.Format("access_key={0}&tonce={1}", exchangeConfig.ApiKey, nonce);
            var message = String.Format("GET|{0}{1}|{2}", resourcePrefix, resource, queryString);
            var signature = HashingUtilities.SHA256_ComputeHash(message, exchangeConfig.ApiSecret);
            var apiEndpoint = String.Format("{0}?{1}&signature={2}", resource, queryString, signature);

            RestRequest request = new RestRequest(apiEndpoint, Method.Get);
            var response = client.Get(request);
            dynamic responseData = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseData.accounts_filtered)
            {
                var currency = item.currency.Value.ToString().ToUpper();
                var balance = Convert.ToDecimal(item.balance.Value);
                if (balance > 0.00m)
                {
                    result.Add(new ExchangeBalance() { Ticker = currency, Balance = balance, BalanceDisplayVal = decimal.Parse(balance.ToString(), System.Globalization.NumberStyles.Float).ToString() });
                }
            }
            return result;
        }

        public static string SubmitTradeOrder(bool isBuyOrder, string market, decimal balance, ExchangeConfig exchange, decimal rate, decimal minTradeSize, string tradingFee)
        {
            string result = String.Empty;
            var method = "";
            if (isBuyOrder)
            {
                method = "sell";
                balance = MathUtilities.RoundDown(balance, 5);
            }
            else
            {
                method = "buy";
                var tradingFeeVal = decimal.Parse(tradingFee.TrimEnd(new char[] { '%', ' ' })) / 100M;
                balance = MathUtilities.RoundDown((balance * (1m - tradingFeeVal)) / rate, 5);
            }
            var tradeAmount = rate * balance;
            if (tradeAmount > Convert.ToDecimal(minTradeSize))
            {
                var nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                RestClient client = new RestClient(Constants.GRAVIEX_API_BASE_PATH);
                var resourcePrefix = "/webapi/v3";
                var resource = "/orders";
                var queryString = String.Format("access_key={0}&market={2}&price={3}&side={4}&tonce={1}&volume={5}", exchange.ApiKey, nonce, market, rate, method, balance);
                var message = String.Format("POST|{0}{1}|{2}", resourcePrefix, resource, queryString);
                var signature = HashingUtilities.SHA256_ComputeHash(message, exchange.ApiSecret);
                var apiEndpoint = String.Format("{0}?{1}&signature={2}", resource, queryString, signature);
                RestRequest request = new RestRequest(apiEndpoint, Method.Get);
                try
                {
                    var response = client.Post(request);
                    dynamic responseData = JsonConvert.DeserializeObject(response.Content);
                    result = String.Format("Created {2} Order on {0} for {1}", exchange.Exchange, market, method);
                }
                catch { } //Graviex has custom rules in place (i.e., subsequent orders, min pegged to USD and BTC. So we ignore errors.
            }
            return result;
        }

        public static void Withdraw(ExchangeConfig exchange, string quantity)
        {
            var nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            RestClient client = new RestClient(Constants.GRAVIEX_API_BASE_PATH);
            var resourcePrefix = "/webapi/v3";
            var resource = "/orders";
            var queryString = String.Format("access_key={0}&currency={2}&fund_uid={3}&sum={4}&tonce={1}", exchange.ApiKey, nonce, exchange.AutoWithdrawlCurrency.Ticker.ToLower(), exchange.AutoWithdrawlAddress, quantity);
            var message = String.Format("POST|{0}{1}|{2}", resourcePrefix, resource, queryString);
            var signature = HashingUtilities.SHA256_ComputeHash(message, exchange.ApiSecret);
            var apiEndpoint = String.Format("{0}?{1}&signature={2}", resource, queryString, signature);
            RestRequest request = new RestRequest(apiEndpoint, Method.Get);
            try
            {
                var response = client.Post(request);
                dynamic responseData = JsonConvert.DeserializeObject(response.Content);
            }
            catch { }
        }
    }
}
