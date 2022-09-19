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
            RestClient client = new RestClient(wtmEndpoint);
            RestRequest request = new RestRequest("");
            var response = client.Get(request);
            dynamic whatToMineResponseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var coin in whatToMineResponseContent?.coins)
            {
                var ticker = coin?.First?.tag?.Value;
                var name = coin?.Name;
                if (ticker != null && ticker.ToString().ToUpper() == "NICEHASH")
                {
                    ticker = name;
                }
                var algorithm = coin?.First?.algorithm?.Value;
                var powerConsumption = "0.00";
                switch (algorithm?.ToUpper())
                {
                    case "AUTOLYKOS":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[al_p]");
                        break;
                    case "BEAMHASHIII":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqb_p]");
                        break;
                    case "CORTEX":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cx_p]");
                        break;
                    case "CRYPTONIGHTFASTV2":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cnf_p]");
                        break;
                    case "CRYPTONIGHTGPU":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cng_p]");
                        break;
                    case "CRYPTONIGHTHAVEN":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cnh_p]");
                        break;
                    case "CUCKAROO29S":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cr29_p]");
                        break;
                    case "CUCKATOO31":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ct31_p]");
                        break;
                    case "CUCKATOO32":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ct32_p]");
                        break;
                    case "CUCKOOCYCLE":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[cc_p]");
                        break;
                    case "EQUIHASH (210,9)":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqa_p]");
                        break;
                    case "EQUIHASHZERO":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eqz_p]");
                        break;
                    case "ETCHASH":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_p]");
                        break;
                    case "ETHASH":
                        if (ticker == "ETH" || ticker == "NICEHASH-Ethash")
                        {
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[eth_p]");
                        }
                        else
                        {
                            powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_p]");
                        }
                        break;
                    case "FIROPOW":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[fpw_p]");
                        break;
                    case "KAWPOW":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[kpw_p]");
                        break;
                    case "KHEAVYHASH":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[hh_p]");
                        break;
                    case "NEOSCRYPT":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ns_p]");
                        break;
                    case "OCTOPUS":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ops_p]");
                        break;
                    case "PROGPOW":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ppw_p]");
                        break;
                    case "PROGPOWZ":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[ppw_p]");
                        break;
                    case "RANDOMX":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[rmx_p]");
                        break;
                    case "UBQHASH":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[e4g_p]");
                        break;
                    case "VERTHASH":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[vh_p]");
                        break;
                    case "X25X":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[x25x_p]");
                        break;
                    case "ZELHASH":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[zlh_p]");
                        break;
                    case "ZHASH":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[zh_p]");
                        break;
                    case "SCRYPT":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[scrypt_power]");
                        break;
                    case "EAGLESONG":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[esg_p]");
                        break;
                    case "LBRY":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[lbry_p]");
                        break;
                    case "HANDSHAKE":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[hk_p]");
                        break;
                    case "SIA":
                        powerConsumption = HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(wtmEndpoint)).Query).Get("factor[sia_p]");
                        break;
                    default:
                        break;
                }
                result.Add(
                    new Coin()
                    {
                        Name = name,
                        Ticker = ticker,
                        Algorithm = coin?.First?.algorithm?.Value,
                        BtcRevenue = coin?.First?.btc_revenue?.Value,
                        CoinRevenue = coin?.First?.estimated_rewards?.Value,
                        PowerConsumption = powerConsumption
                    });
            }
            return result;
        }
    }
}
