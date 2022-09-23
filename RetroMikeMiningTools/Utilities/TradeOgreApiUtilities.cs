using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using System.Dynamic;

namespace RetroMikeMiningTools.Utilities
{
    public static class TradeOgreApiUtilities
    {
        public static List<Coin> GetTickers()
        {
            List<Coin> result = new List<Coin>();
            var client = new RestClient(Constants.TRADE_OGRE_API_BASE_PATH);
            var request = new RestRequest("/markets", Method.Get);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null)
            {
                foreach (var item in responseContent)
                {
                    foreach (var market in item)
                    {
                        var marketName = market.Name;
                        string[] marketSegments = marketName.Split('-');
                        if (marketSegments.Length == 2)
                        {
                            var ticker = marketSegments[1];
                            if (!result.Any(x => x.Ticker == ticker))
                            {
                                result.Add(new Coin() { Exchange = Enums.Exchange.TradeOgre, Ticker = ticker, Name = ticker });
                            }
                            ticker = marketSegments[0];
                            if (!result.Any(x => x.Ticker == ticker))
                            {
                                result.Add(new Coin() { Exchange = Enums.Exchange.TradeOgre, Ticker = ticker, Name = ticker });
                            }
                        }
                    }

                    //result.Add(new Coin() { ExchangeId = Constants.EXCHANGE_TRADE_OGRE, Ticker = item.Currency.ToString(), Name = String.Format("{0} ({1})", item.CurrencyLong.ToString(), item.Currency.ToString()) });
                }
            }
            return result;
        }

        public static List<ExchangeMarket> GetMarkets()
        {
            List<ExchangeMarket> markets = new List<ExchangeMarket>();

            var client = new RestClient(Constants.TRADE_OGRE_API_BASE_PATH);
            var request = new RestRequest("/markets", Method.Get);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            if (responseContent != null)
            {
                foreach (var item in responseContent)
                {
                    foreach (var marketContainer in item)
                    {
                        var ask = "0.00";
                        var bid = "0.00";
                        foreach (var data in marketContainer)
                        {
                            ask = data.ask;
                            bid = data.bid;
                        }
                        var minTrade = "0.00";
                        switch (Convert.ToString(marketContainer?.Name?.ToString()?.Split('-')[0]))
                        {
                            case "BTC":
                                minTrade = "0.00005";
                                break;
                            case "LTC":
                                minTrade = "0.01";
                                break;
                            case "USDT":
                                minTrade = "1.00";
                                break;
                            default:
                                break;
                        }

                        markets.Add(new ExchangeMarket()
                        {
                            MarketName = Convert.ToString(marketContainer?.Name),
                            BaseCurrency = Convert.ToString(marketContainer?.Name?.ToString()?.Split('-')[0]),
                            MarketCurrency = Convert.ToString(marketContainer?.Name?.ToString()?.Split('-')[1]),
                            MinTradeSize = minTrade,
                            Ask = ask,
                            Bid = bid
                        });
                    }
                }
            }
            return markets;
        }

        public static List<ExchangeBalance> GetBalances(ExchangeConfig exchange)
        {
            var balances = new List<ExchangeBalance>();
            var method = "/account/balances";
            var client = new RestClient(Constants.TRADE_OGRE_API_BASE_PATH);
            client.Authenticator = new HttpBasicAuthenticator(exchange.ApiKey, exchange.ApiSecret);
            var request = new RestRequest(method);
            var response = client.Get(request);
            dynamic responseData = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseData.balances)
            {
                balances.Add(new ExchangeBalance() { Ticker = item.Name, Balance = Convert.ToDecimal(item.Value) });
            }
            return balances;
        }

        public static string PlaceTradeOrder(ExchangeConfig exchange, bool isBuyOrder, string baseCurrency, string marketCurrency, decimal balance, decimal rate, string marketName)
        {
            var result = String.Empty;
            var method = String.Empty;
            if (isBuyOrder)
            {
                method = "/order/sell";
            }
            else
            {
                method = "/order/buy";
            }

            dynamic requestData = new ExpandoObject();
            requestData.market = String.Format("{0}-{1}", baseCurrency, marketCurrency);
            requestData.quantity = balance;
            requestData.price = rate;


            var orderClient = new RestClient(Constants.TRADE_OGRE_API_BASE_PATH);
            orderClient.Authenticator = new HttpBasicAuthenticator(exchange.ApiKey, exchange.ApiSecret);
            var orderRequest = new RestRequest(method, Method.Post);
            orderRequest.AddParameter("market", String.Format("{0}-{1}", baseCurrency, marketCurrency));
            orderRequest.AddParameter("quantity", balance);
            orderRequest.AddParameter("price", rate);
            var orderResponse = orderClient.Post(orderRequest);
            dynamic orderResponseData = JsonConvert.DeserializeObject(orderResponse.Content);
            result = String.Format("Created Sell Order on Trade Ogre for {0}", marketName);
            return result;
        }
    }
}