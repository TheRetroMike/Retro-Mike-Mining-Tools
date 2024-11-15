using CryptoExchange.Net.CommonObjects;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using System.Dynamic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RetroMikeMiningTools.Utilities
{
    public static class XeggexUtilities
    {
        public static List<Coin> GetTickers()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient(Constants.XEGGEX_API_BASE_PATH);
            var request = new RestRequest("/asset/getlist", Method.Get);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null)
            {
                foreach (var item in responseContent)
                {
                    result.Add(new Coin() { Exchange = Enums.Exchange.Xeggex, Ticker = item.ticker.ToString(), Name = String.Format("{0} ({1})", item.name.ToString(), item.ticker.ToString()) });
                }
            }
            return result;
        }

        public static List<ExchangeMarket> GetMarkets()
        {
            List<ExchangeMarket> markets = new List<ExchangeMarket>();

            var client = new RestClient(Constants.XEGGEX_API_BASE_PATH);
            var request = new RestRequest("/market/getlist", Method.Get);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null)
            {
                foreach (var item in responseContent)
                {
                    var symbol = item.symbol;
                    var primaryTicker = item.primaryTicker;
                    var secondaryTicker = symbol.ToString().Split('/')[1];
                    var ask = item.bestAsk;
                    var bid = item.bestBid;
                    var qtyDecimalPlaces = Convert.ToInt32(item.quantityDecimals);

                    markets.Add(new ExchangeMarket()
                    {
                        MarketName = Convert.ToString(symbol),
                        BaseCurrency = Convert.ToString(primaryTicker),
                        MarketCurrency = Convert.ToString(secondaryTicker),
                        MinTradeSize = qtyDecimalPlaces>0 ? String.Format("0.{0}1", String.Empty.PadRight(qtyDecimalPlaces-1,'0')) : "1",
                        Ask = ask,
                        Bid = bid
                    });
                }
            }
            return markets;
        }

        public static List<ExchangeBalance> GetBalances(ExchangeConfig exchange)
        {
            var balances = new List<ExchangeBalance>();
            var client = new RestClient(Constants.XEGGEX_API_BASE_PATH);
            client.Authenticator = new HttpBasicAuthenticator(exchange.ApiKey, exchange.ApiSecret);
            var request = new RestRequest("/balances");
            var response = client.Get(request);
            dynamic responseData = JsonConvert.DeserializeObject(response.Content);
            if (responseData != null)
            {
                foreach (var item in responseData)
                {
                    balances.Add(new ExchangeBalance() { Ticker = item.asset, Balance = Convert.ToDecimal(item.available), BalanceDisplayVal = item.available });
                }
            }
            return balances;
        }

        public static string PlaceTradeOrder(ExchangeConfig exchange, bool isSellOrder, string baseCurrency, string marketCurrency, decimal balance, decimal rate, string marketName)
        {
            var result = String.Empty;
            var orderClient = new RestClient(Constants.XEGGEX_API_BASE_PATH);
            orderClient.Authenticator = new HttpBasicAuthenticator(exchange.ApiKey, exchange.ApiSecret);
            var orderRequest = new RestRequest("/createorder", Method.Post);
            orderRequest.AddParameter("userProvidedId", "");
            orderRequest.AddParameter("symbol", String.Format("{0}/{1}", baseCurrency, marketCurrency));
            orderRequest.AddParameter("side", !isSellOrder ? "buy" : "sell");
            orderRequest.AddParameter("type", "market");
            orderRequest.AddParameter("quantity", balance);
            orderRequest.AddParameter("price", rate);
            orderRequest.AddParameter("strictValidate", false);
            var orderResponse = orderClient.Post(orderRequest);
            dynamic orderResponseData = JsonConvert.DeserializeObject(orderResponse.Content);
            var id = Convert.ToString(orderResponseData.id);
            if (!String.IsNullOrEmpty(id))
            {
                if (isSellOrder)
                {
                    result = String.Format("Created Sell Order on Xeggex for {0}", marketName);
                }
                else
                {
                    result = String.Format("Created Buy Order on Xeggex for {0}", marketName);
                }
            }

            return result;
        }
        public static async Task<string> Withdraw(ExchangeConfig exchange, decimal amount)
        {
            var result = String.Empty;
            try
            {
                var client = new RestClient(Constants.XEGGEX_API_BASE_PATH);
                client.Authenticator = new HttpBasicAuthenticator(exchange.ApiKey, exchange.ApiSecret);
                var request = new RestRequest("/createwithdrawal", Method.Post);
                request.AddParameter("ticker", exchange.AutoWithdrawlCurrency.Ticker);
                request.AddParameter("quantity", amount - (exchange.WithdrawlFee ?? 0.00m));
                request.AddParameter("address", exchange.AutoWithdrawlAddress);
                request.AddParameter("paymentid", String.Empty);
                
                var response = client.Post(request);
                dynamic responseData = JsonConvert.DeserializeObject(response.Content);
                var id = Convert.ToString(responseData.id);
                if (!String.IsNullOrEmpty(id))
                {
                    Logger.Log(String.Format("Auto Withdrawl Submitted on Xeggex for {0} {1}", amount - (exchange.WithdrawlFee ?? 0.00m), exchange.AutoWithdrawlCurrency.Ticker), LogType.AutoExchanging, exchange.Username);
                }
            }
            catch (Exception ex) { }
            return result;
        }
    }
}