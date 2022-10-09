using Newtonsoft.Json;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RestSharp;
using System.Web;

namespace RetroMikeMiningTools.Utilities
{
    public static class WhatToMineUtilities
    {
        public static List<Coin> GetCoinList(string wtmEndpoint)
        {
            List<Coin> result = new List<Coin>();
            try
            {
                RestClient client = new RestClient(wtmEndpoint);
                RestRequest request = new RestRequest("");
                var response = client.Get(request);
                dynamic whatToMineResponseContent = JsonConvert.DeserializeObject(response.Content);
                foreach (var coin in whatToMineResponseContent?.coins)
                {
                    var ticker = coin?.First?.tag?.Value;
                    var name = coin?.Name;
                    if (ticker != null && (ticker.ToString().ToUpper() == "NICEHASH" || ticker.ToString().ToUpper() == "DGB" || ticker.ToString().ToUpper() == "XMY" || ticker.ToString().ToUpper() == "XVG"))
                    {
                        ticker = name;
                    }
                    var algorithm = coin?.First?.algorithm?.Value;
                    var powerConsumption = "0.00";
                    var hashRate = "";
                    switch (algorithm?.ToUpper())
                    {
                        case "AUTOLYKOS":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[al_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[al_hr]");
                            break;
                        case "BEAMHASHIII":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqb_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqb_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "CORTEX":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cx_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cx_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "CRYPTONIGHTFASTV2":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cnf_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cnf_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "CRYPTONIGHTGPU":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cng_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cng_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "CRYPTONIGHTHAVEN":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cnh_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cnh_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "CUCKAROO29S":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cr29_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cr29_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "CUCKATOO31":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ct31_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ct31_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "CUCKATOO32":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ct32_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ct32_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "CUCKOOCYCLE":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cc_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cc_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "EQUIHASH (210,9)":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqa_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqa_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "EQUIHASHZERO":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqz_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqz_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "ETCHASH":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_hr]");
                            break;
                        case "ETHASH":
                            if (ticker == "ETH" || ticker == "NICEHASH-Ethash")
                            {
                                powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eth_p]");
                                hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eth_hr]");
                            }
                            else
                            {
                                powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_p]");
                                hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_hr]");
                            }
                            break;
                        case "FIROPOW":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[fpw_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[fpw_hr]");
                            break;
                        case "KAWPOW":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[kpw_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[kpw_hr]");
                            break;
                        case "KHEAVYHASH":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[hh_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[hh_hr]");
                            break;
                        case "NEOSCRYPT":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ns_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqz_hr]")) * 0.001); //H -> KH 
                            break;
                        case "OCTOPUS":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ops_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ops_hr]");
                            break;
                        case "PROGPOW":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ppw_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ppw_hr]");
                            break;
                        case "PROGPOWZ":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ppw_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ppw_hr]");
                            break;
                        case "RANDOMX":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[rmx_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[rmx_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "UBQHASH":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_hr]");
                            break;
                        case "VERTHASH":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[vh_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[vh_hr]");
                            break;
                        case "X25X":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[x25x_p]");
                            hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[x25x_hr]");
                            break;
                        case "ZELHASH":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[zlh_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[zlh_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "ZHASH":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[zh_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[zh_hr]")) * 0.000001); //H -> MH 
                            break;
                        case "SCRYPT":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[scrypt_power]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[scrypt_hash_rate]")) * 1000); //GH -> MH 
                            break;
                        case "EAGLESONG":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[esg_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[esh_hr]")) * 1000); //GH -> MH 
                            break;
                        case "LBRY":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[lbry_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[lbry_hr]")) * 1000); //GH -> MH 
                            break;
                        case "HANDSHAKE":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[hk_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[hk_hr]")) * 1000000); //TH -> MH 
                            break;
                        case "SIA":
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[sia_p]");
                            hashRate = Convert.ToString(Convert.ToDouble(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[sia_hr]")) * 1000000); //TH -> MH 
                            break;
                        default:
                            break;
                    }
                    result.Add(
                        new Coin()
                        {
                            Name = String.Format("{0} ({1})", name, ticker),
                            Ticker = ticker,
                            Algorithm = coin?.First?.algorithm?.Value,
                            BtcRevenue = coin?.First?.btc_revenue?.Value,
                            CoinRevenue = coin?.First?.estimated_rewards?.Value,
                            PowerConsumption = powerConsumption,
                            HashRate = hashRate
                        });
                }
            }
            catch { }
            return result;
        }

        public static List<Coin> GetIndividualCoinList()
        {
            if (!File.Exists("WtmCoins.dat"))
            {
                RefreshIndividualCoinList();
            }
            return JsonConvert.DeserializeObject<List<Coin>>(File.ReadAllText("WtmCoins.dat"));
        }

        public static void RefreshIndividualCoinList()
        {
            List<Coin> result = new List<Coin>();
            try
            {
                RestClient client = new RestClient("https://whattomine.com/calculators.json");
                RestRequest request = new RestRequest("");
                var response = client.Get(request);
                dynamic whatToMineResponseContent = JsonConvert.DeserializeObject(response.Content);
                foreach (var coin in whatToMineResponseContent?.coins)
                {
                    var name = coin?.Name;
                    foreach (var item in coin)
                    {
                        var ticker = item?.tag?.Value;
                        var id = item?.id?.Value;
                        var algorithm = item?.algorithm?.Value;
                        var status = item?.status?.Value;
                        if (status != null && status.ToString().Equals("Active", StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(new Coin()
                            {
                                Name = String.Format("{0} ({1})", name, ticker),
                                Ticker = ticker,
                                Algorithm = algorithm,
                                WtmId = Convert.ToInt32(id)
                            });
                        }
                    }
                }
                using (var writer = new StreamWriter("WtmCoins.dat", false))
                {
                    writer.Write(JsonConvert.SerializeObject(result));
                }
            }
            catch { }
        }

        public static Coin GetIndividualCoin(int id, string hashrate, string power, string powerCost)
        {
            try
            {
                var wtmEndpoint = String.Format("https://whattomine.com/coins/{0}.json?hr={1}p={2}&fee=0.0&cost={3}&cost_currency=USD", id, hashrate, power, powerCost);
                RestClient client = new RestClient(wtmEndpoint);
                RestRequest request = new RestRequest("");
                var response = client.Get(request);
                dynamic whatToMineResponseContent = JsonConvert.DeserializeObject(response.Content);
                if (whatToMineResponseContent != null)
                {
                    var ticker = whatToMineResponseContent.tag?.Value;
                    var name = whatToMineResponseContent?.name?.Value;
                    var algo = whatToMineResponseContent?.algorithm?.Value;
                    var btcRevenue = whatToMineResponseContent?.btc_revenue?.Value;
                    var coinRevenue = whatToMineResponseContent?.estimated_rewards?.Value;
                    var powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("p");
                    var hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("hr");
                    return new Coin()
                    {
                        Name = String.Format("{0} ({1})", name, ticker),
                        Ticker = ticker,
                        Algorithm = algo,
                        BtcRevenue = btcRevenue,
                        CoinRevenue = coinRevenue,
                        PowerConsumption = powerConsumption,
                        HashRate = hashRate
                    };
                }
            }
            catch { }
            return new Coin();
        }

        public static Coin GetIndividualCoin(string wtmEndpoint)
        {
            try
            {
                RestClient client = new RestClient(wtmEndpoint);
                RestRequest request = new RestRequest("");
                var response = client.Get(request);
                dynamic whatToMineResponseContent = JsonConvert.DeserializeObject(response.Content);
                if (whatToMineResponseContent != null)
                {
                    var ticker = whatToMineResponseContent.tag?.Value;
                    var name = whatToMineResponseContent?.name?.Value;
                    var algo = whatToMineResponseContent?.algorithm?.Value;
                    var btcRevenue = whatToMineResponseContent?.btc_revenue?.Value;
                    var coinRevenue = whatToMineResponseContent?.estimated_rewards?.Value;
                    var powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("p");
                    var hashRate = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("hr");
                    return new Coin()
                    {
                        Name = String.Format("{0} ({1})", name, ticker),
                        Ticker = ticker,
                        Algorithm = algo,
                        BtcRevenue = btcRevenue,
                        CoinRevenue = coinRevenue,
                        PowerConsumption = powerConsumption,
                        HashRate = hashRate
                    };
                }
            }
            catch { }
            return new Coin();
        }
    }
}
