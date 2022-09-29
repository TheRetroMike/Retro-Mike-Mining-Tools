using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Utilities
{
    public static class GenericExchangeApiUtilities
    {
        public static List<Coin> GetTickers(string apiBasePath, Enums.Exchange exchangeId)
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient(apiBasePath);
            var request = new RestRequest("/public/getcurrencies", Method.Get);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null && responseContent.result != null)
            {
                foreach (var item in responseContent.result)
                {
                    result.Add(new Coin() { Exchange = exchangeId, Ticker = item.Currency.ToString(), Name = String.Format("{0} ({1})", item.CurrencyLong.ToString(), item.Currency.ToString()) });
                }
            }
            return result;
        }

        public static List<ExchangeMarket> GetMarkets(string apiBasePath)
        {
            List<ExchangeMarket> markets = new List<ExchangeMarket>();
            var client = new RestClient(apiBasePath);
            var request = new RestRequest("/public/getmarkets", Method.Get);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null && responseContent.result != null)
            {
                foreach (var item in responseContent.result)
                {
                    var active = Convert.ToBoolean(item.IsActive);

                    if (active)
                    {
                        markets.Add(new ExchangeMarket()
                        {
                            BaseCurrency = Convert.ToString(item.BaseCurrency),
                            MarketCurrency = Convert.ToString(item.MarketCurrency),
                            MarketName = Convert.ToString(item.MarketName),
                            MinTradeSize = Convert.ToString(item.MinTradeSize)
                        });
                    }
                }
            }
            return markets;
        }

        public static List<ExchangeBalance> GetWalletBalances(string apiBasePath, ExchangeConfig exchangeConfig)
        {
            List<ExchangeBalance> result = new List<ExchangeBalance>();
            var method = "/account/getbalances";
            var queryString = String.Format("apikey={0}&nonce={1}", exchangeConfig.ApiKey, HashingUtilities.CalculateNonce());
            var apiSignMessage = String.Format("{0}{1}?{2}", apiBasePath, method, queryString);
            var apiSign = HashingUtilities.SHA512_ComputeHash(apiSignMessage, exchangeConfig.ApiSecret);
            var client = new RestClient(apiBasePath);
            var request = new RestRequest(String.Format("{0}?{1}", method, queryString));
            request.AddHeader("apisign", apiSign.ToUpper());
            var response = client.Get(request);
            dynamic responseData = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseData.result)
            {
                var currency = item.Currency.Value;
                var balance = Convert.ToDecimal(item.Available.Value);
                result.Add(new ExchangeBalance() { Ticker = currency, Balance = balance, BalanceDisplayVal = decimal.Parse(item.Available.Value.ToString(), System.Globalization.NumberStyles.Float).ToString() });
            }
            return result;
        }

        public static decimal GetExchangeRate(string apiBasePath, bool isBuyOrder, string market)
        {
            decimal rate = 0m;
            var buyOrderMethod = isBuyOrder ?
                String.Format("/public/getorderbook?market={0}&type=buy", market) :
                String.Format("/public/getorderbook?market={0}&type=sell", market);
            var client = new RestClient(apiBasePath);
            var request = new RestRequest(buyOrderMethod);
            var response = client.Get(request);
            dynamic responseData = JsonConvert.DeserializeObject(response.Content);
            foreach (var buyOrder in responseData.result)
            {
                var buyRate = decimal.Parse(buyOrder.Rate.Value.ToString().ToLower(), System.Globalization.NumberStyles.Float);
                if (isBuyOrder)
                {
                    if (buyRate > rate || rate == 0m)
                    {
                        rate = buyRate;
                    }
                }
                else
                {
                    if (buyRate < rate || rate == 0m)
                    {
                        rate = buyRate;
                    }
                }
            }
            return rate;
        }

        public static string SubmitTradeOrder(string apiBasePath, bool isBuyOrder, string market, decimal balance, ExchangeConfig exchange, decimal rate, decimal minTradeSize, string tradingFee)
        {
            string result = String.Empty;
            var method = "";
            if (isBuyOrder)
            {
                method = "/market/selllimit";
                balance = MathUtilities.RoundDown(balance, 4);
            }
            else
            {
                method = "/market/buylimit";
                var tradingFeeVal = decimal.Parse(tradingFee.TrimEnd(new char[] { '%', ' ' })) / 100M;
                balance = MathUtilities.RoundDown((balance * (1m - tradingFeeVal)) / rate, 4);
            }
            var tradeAmount = rate * balance;
            if (tradeAmount > Convert.ToDecimal(minTradeSize))
            {

                var queryString = String.Format("apikey={0}&market={1}&nonce={2}&quantity={3}&rate={4}", exchange.ApiKey, market, HashingUtilities.CalculateNonce(), balance, rate);
                var apiSignMessage = String.Format("{0}{1}?{2}", apiBasePath, method, queryString);
                var apiSign = HashingUtilities.SHA512_ComputeHash(apiSignMessage, exchange.ApiSecret);
                var client = new RestClient(apiBasePath);
                var request = new RestRequest(String.Format("{0}?{1}", method, queryString));
                request.AddHeader("apisign", apiSign.ToUpper());
                var response = client.Get(request);
                dynamic responseData = JsonConvert.DeserializeObject(response.Content);
                var sellResult = responseData?.result?.uuid;
                result = String.Format("Created Sell Order on {0} for {1}", exchange.Exchange, market);
            }
            return result;
        }
    }
}
