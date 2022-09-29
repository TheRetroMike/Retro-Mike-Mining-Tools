using CoinEx.Net.Clients;
using CoinEx.Net.Objects;
using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace RetroMikeMiningTools.Utilities
{
    public static class SouthXchangeUtilities
    {
        public static List<Coin> GetTickers()
        {
            List<Coin> result = new List<Coin>();

            var client = new RestSharp.RestClient("https://www.southxchange.com/api/v2/markets");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseContent)
            {
                var currency = item[0].Value;
                var tradingCurrency = item[1].Value;
                if (!result.Any(x => x.Ticker.Equals(currency, StringComparison.OrdinalIgnoreCase)))
                {
                    result.Add(new Coin() { Ticker = currency, Exchange = Enums.Exchange.SouthXchange, Name = currency });
                }
                if (!result.Any(x => x.Ticker.Equals(tradingCurrency, StringComparison.OrdinalIgnoreCase)))
                {
                    result.Add(new Coin() { Ticker = tradingCurrency, Exchange = Enums.Exchange.SouthXchange, Name = tradingCurrency });
                }
            }
            return result;
        }

        public static List<ExchangeBalance> GetBalances(ExchangeConfig exchange)
        {
            List<ExchangeBalance> result = new List<ExchangeBalance>();
            dynamic requestObject = new ExpandoObject();
            requestObject.key = exchange.ApiKey;
            requestObject.nonce = DateTime.UtcNow.Ticks;
            string jsonData = JsonConvert.SerializeObject(requestObject);

            RestClient client = new RestClient("https://www.southxchange.com/api/listBalances");
            client.AddDefaultHeader("Hash", HashingUtilities.SHA512_ComputeHash(jsonData, exchange.ApiSecret));
            RestRequest request = new RestRequest("", Method.Post);
            request.AddStringBody(jsonData, DataFormat.Json);
            var response = client.Post(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null)
            {
                foreach (var item in responseContent)
                {
                    var ticker = item.Currency.Value;
                    var balance = item.Available.Value.ToString().ToLower();
                    result.Add(new ExchangeBalance() { Ticker = ticker, Balance= decimal.Parse(balance, System.Globalization.NumberStyles.Float), BalanceDisplayVal = decimal.Parse(balance, System.Globalization.NumberStyles.Float).ToString() });
                }
            }
            return result;
        }

        public static List<ExchangeMarket> GetMarkets()
        {
            List<ExchangeMarket> result = new List<ExchangeMarket>();

            var client = new RestClient("https://www.southxchange.com/api/prices");
            var request = new RestRequest();
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseContent)
            {
                var marketName = item.Market.Value;
                var bid = item.Bid.Value;
                var ask = item.Ask.Value;
                var currency = Convert.ToString(marketName).Split('/')[0];
                var tradingCurrency = Convert.ToString(marketName).Split('/')[1];

                result.Add(new ExchangeMarket()
                {
                    MarketName = Convert.ToString(marketName),
                    BaseCurrency = Convert.ToString(tradingCurrency),
                    MarketCurrency = Convert.ToString(currency),
                    Ask = Convert.ToString(ask),
                    Bid = Convert.ToString(bid)
                });
            }
            return result;
        }

        public static async Task<string> PlaceTradeOrder(ExchangeConfig exchange, bool isBuyOrder, string baseCurrency, string marketCurrency, decimal balance, decimal rate, string marketName)
        {
            var result = String.Empty;
            try
            {
                dynamic requestObject = new ExpandoObject();
                requestObject.amount = balance;
                requestObject.amountInReferenceCurrency = false;
                requestObject.key = exchange.ApiKey;
                requestObject.limitPrice = null;
                requestObject.listingCurrency = marketCurrency;
                requestObject.nonce = DateTime.UtcNow.Ticks;
                requestObject.referenceCurrency = baseCurrency;
                requestObject.type = !isBuyOrder ? "buy" : "sell";
                string jsonData = JsonConvert.SerializeObject(requestObject);
                RestClient client = new RestClient("https://www.southxchange.com/api/placeOrder");
                client.AddDefaultHeader("Hash", HashingUtilities.SHA512_ComputeHash(jsonData, exchange.ApiSecret));
                RestRequest request = new RestRequest("", Method.Post);
                request.AddStringBody(jsonData, DataFormat.Json);
                var response = client.Post(request);
                dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
                if (responseContent != null)
                {
                    result = String.Format("Created {2} Order on South Xchange for {0}-{1}", marketCurrency, baseCurrency, !isBuyOrder ? "Buy" : "Sell");
                }
            }
            catch { }
            return result;
        }
    }
}
