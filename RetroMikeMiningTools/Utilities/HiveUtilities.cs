﻿using Newtonsoft.Json;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RestSharp;
using System.Dynamic;
using System.Net;

namespace RetroMikeMiningTools.Utilities
{
    public static class HiveUtilities
    {
        public static List<HiveOsRigConfig> GetWorkers(string hiveApiKey, string farmId)
        {
            List<HiveOsRigConfig> result = new List<HiveOsRigConfig>();
            RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
            RestRequest request = new RestRequest(String.Format("/farms/{0}/workers/preview", farmId));
            request.AddHeader("Authorization", "Bearer " + hiveApiKey);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response?.Content);
            if (!String.IsNullOrEmpty(response?.Content))
            {
                
                foreach (var item in responseContent?.data)
                {
                    result.Add(new HiveOsRigConfig()
                    {
                        Name = item.name.Value,
                        HiveWorkerId = item.id.Value.ToString(),
                    });
                }
            }
            
            return result;
        }

        public static List<Flightsheet> GetAllFlightsheets(string hiveApiKey, string farmId)
        {
            List<Flightsheet> result = new List<Flightsheet>();
            try
            {
                RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
                RestRequest request = new RestRequest(String.Format("/farms/{0}/fs", farmId));
                request.AddHeader("Authorization", "Bearer " + hiveApiKey);
                var response = client.Get(request);
                dynamic responseContent = JsonConvert.DeserializeObject(response?.Content);
                if (!String.IsNullOrEmpty(response?.Content))
                {

                    foreach (var item in responseContent?.data)
                    {
                        var flightSheetName = item?.name?.Value;
                        if (!String.IsNullOrEmpty(flightSheetName))
                        {
                            result.Add(new Flightsheet()
                            {
                                Name = flightSheetName,
                                Id = item.id.Value
                            });
                        }
                    }
                }
            }
            catch { }
            return result;
        }



        public static void DeleteFlightsheet(string hiveApiKey, string farmId, long? flightSheetId)
        {
            try
            {
                RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
                RestRequest request = new RestRequest(String.Format("/farms/{0}/fs/{1}", farmId, flightSheetId));

                request.AddHeader("Authorization", "Bearer " + hiveApiKey);
                var response = client.Delete(request);
            }
            catch { }
        }

        public static string GetFlightsheetWalletID(string flightSheetName, string ticker, string hiveApiKey, string farmId)
        {
            string result = "";
            RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
            RestRequest request = new RestRequest(String.Format("/farms/{0}/fs", farmId));
            request.AddHeader("Authorization", "Bearer " + hiveApiKey);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            foreach (var item in responseContent.data)
            {
                var fsName = item.name;
                if (!String.IsNullOrEmpty(fsName?.Value) && fsName?.Value == flightSheetName)
                {
                    foreach (var flightSheetLine in item?.items)
                    {
                        if (flightSheetLine?.coin == ticker)
                        {
                            return flightSheetLine.wal_id;
                        }
                    }
                }
            }
            return result;
        }

        public static long? GetFlightsheetWorkerCount(string hiveApiKey, string farmId, string flightSheetId)
        {
            try
            {
                RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
                RestRequest request = new RestRequest(String.Format("/farms/{0}/fs/{1}", farmId, flightSheetId));
                request.AddHeader("Authorization", "Bearer " + hiveApiKey);
                var response = client.Get(request);
                dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
                if (responseContent != null && responseContent.workers_count != null)
                {
                    return Convert.ToInt64(responseContent?.workers_count);
                }
            }
            catch { }
            return null;
        }

        public static string GetWalletBalance(string walletId, string hiveApiKey, string farmId)
        {
            RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
            RestRequest request = new RestRequest(String.Format("/farms/{0}/wallets/{1}", farmId, walletId));
            request.AddHeader("Authorization", "Bearer " + hiveApiKey);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            var balanceVal = responseContent?.balance?.value;
            return balanceVal;
        }

        public static string GetWalletAddress(string walletId, string hiveApiKey, string farmId)
        {
            RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
            RestRequest request = new RestRequest(String.Format("/farms/{0}/wallets/{1}", farmId, walletId));
            request.AddHeader("Authorization", "Bearer " + hiveApiKey);
            var response = client.Get(request);
            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            var address = responseContent?.wal?.Value;
            return address;
        }

        public static string GetCurrentFlightsheet(string workerId, string hiveApiKey, string farmId, string workerName)
        {
            string result = "0";
            try
            {
                if (String.IsNullOrEmpty(workerId) && !String.IsNullOrEmpty(workerName))
                {
                    workerId = GetWorkers(hiveApiKey, farmId).Where(x => x.Name == workerName)?.FirstOrDefault()?.HiveWorkerId;
                }
                RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
                RestRequest request = new RestRequest(String.Format("/farms/{0}/workers/{1}", farmId, workerId));
                request.AddHeader("Authorization", "Bearer " + hiveApiKey);
                var response = client.Get(request);
                dynamic responseContent = JsonConvert.DeserializeObject(response?.Content);
                if (!String.IsNullOrEmpty(response?.Content))
                {
                    result = responseContent.flight_sheet?.id;
                }
            }
            catch { }
            return result;
        }

        public static Dictionary<string,string> GetFlightsheetWallets(string farmId, string flightSheetId, string hiveApiKey)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
            RestRequest request = new RestRequest(String.Format("/farms/{0}/fs/{1}", farmId, flightSheetId));
            request.AddHeader("Authorization", "Bearer " + hiveApiKey);
            var response = client.Get(request);
            if (response != null)
            {
                dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
                for (int i = 0; i < responseContent.items.Count; i++)
                {
                    string coin = responseContent.items[i].coin.Value;
                    if (
                        coin.StartsWith("Zerg-", StringComparison.OrdinalIgnoreCase) ||
                        coin.StartsWith("Nicehash-", StringComparison.OrdinalIgnoreCase) ||
                        coin.StartsWith("Prohashing-", StringComparison.OrdinalIgnoreCase) ||
                        coin.StartsWith("MiningDutch-", StringComparison.OrdinalIgnoreCase)
                        )
                    {
                        string walletId = Convert.ToString(responseContent.items[i].wal_id);
                        string walletAddress = GetWalletAddress(walletId, hiveApiKey, farmId);
                        if (!String.IsNullOrEmpty(walletAddress))
                        {
                            result.Add(coin, walletAddress);
                        }
                    }
                }
            }
            return result;
        }

        public static void UpdateFlightSheetID(string workerId, string flightSheetId, string flightSheetName, string profit, string hiveApiKey, string farmId, string workerName, bool donation, MiningMode miningMode, string ticker, string username)
        {
            try
            {
                if (String.IsNullOrEmpty(workerId) && !String.IsNullOrEmpty(workerName))
                {
                    workerId = GetWorkers(hiveApiKey, farmId).Where(x => x.Name == workerName)?.FirstOrDefault()?.HiveWorkerId;
                }
                if (!String.IsNullOrEmpty(workerId))
                {
                    RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
                    RestRequest request = new RestRequest(String.Format("/farms/{0}/workers/{1}", farmId, workerId));
                    request.AddHeader("Authorization", "Bearer " + hiveApiKey);
                    var requestBody = new HiveWorkerPatchRequest() { fs_id = Convert.ToInt32(flightSheetId) };
                    request.AddJsonBody(requestBody);
                    var response = client.Patch(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (donation)
                        {
                            Common.Logger.Log(String.Format("Flightsheet Updated on {0} to Dev Fee / Donation. ", workerName), LogType.ProfitSwitching, username);
                        }
                        else
                        {
                            if (miningMode == MiningMode.Profit || miningMode == MiningMode.DiversificationByProfit)
                            {
                                Common.Logger.Log(String.Format("Flightsheet Updated on {2} to {0}. Estimated Current Profit: ${1}", flightSheetName, Math.Round(Convert.ToDouble(profit), 2), workerName), LogType.ProfitSwitching, username);
                            }
                            if (miningMode == MiningMode.CoinStacking || miningMode == MiningMode.DiversificationByStacking)
                            {
                                Common.Logger.Log(String.Format("Flightsheet Updated on {2} to {0}. Estimated Current Amount: {1} {3}", flightSheetName, Math.Round(Convert.ToDouble(profit), 2), workerName, ticker), LogType.ProfitSwitching, username);
                            }
                            if (miningMode == MiningMode.ZergPoolAlgoProfitBasis)
                            {
                                Common.Logger.Log(String.Format("Flightsheet Updated on {2} to {0}. Estimated Current Profit: ${1}", flightSheetName, Math.Round(Convert.ToDouble(profit), 2), workerName), LogType.ProfitSwitching, username);
                            }
                        }
                    }
                    else
                    {
                        Common.Logger.Log("Flightsheet Failed to Update", LogType.ProfitSwitching, username);
                    }
                }
                else
                {
                    Common.Logger.Log(String.Format("Unable to determine worker id for rig {0}. Please check Rig name or use import function", workerName), LogType.ProfitSwitching, username);
                }
            }
            catch { }
        }

        public static string CreateDonationFlightsheet(string flightSheetId, string hiveApiKey, string farmId)
        {
            string result = "";
            string debuggingData = String.Empty;
            string responseDebuggingData = String.Empty;
            try
            {
                RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
                RestRequest request = new RestRequest(String.Format("/farms/{0}/fs/{1}", farmId, flightSheetId));
                request.AddHeader("Authorization", "Bearer " + hiveApiKey);
                var response = client.Get(request);
                if (response != null)
                {
                    try
                    {

                        dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
                        responseDebuggingData = response.Content;
                        dynamic body = new ExpandoObject();
                        if (responseContent.name != null && Convert.ToString(responseContent.name).EndsWith("_profit_switcher_donation", StringComparison.OrdinalIgnoreCase))
                        {
                            return "0";
                        }
                        body.name = String.Format("{0}_profit_switcher_donation", responseContent.name ?? "");
                        body.items = new ExpandoObject[responseContent.items.Count];

                        for (int i = 0; i < responseContent.items.Count; i++)
                        {
                            string coin = responseContent.items[i].coin.Value;
                            var donationRecord = Constants.DONATION_FLIGHTSHEET_DATA.Where(x => x.Ticker.Equals(coin, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (
                                donationRecord != null ||
                                coin.StartsWith("Zerg-", StringComparison.OrdinalIgnoreCase) ||
                                coin.StartsWith("Nicehash-", StringComparison.OrdinalIgnoreCase) ||
                                coin.StartsWith("Prohashing-", StringComparison.OrdinalIgnoreCase) ||
                                coin.StartsWith("MiningDutch-", StringComparison.OrdinalIgnoreCase)
                                )
                            {
                                body.items[i] = new ExpandoObject();
                                body.items[i].coin = responseContent.items[i].coin.Value;

                                if (responseContent.items[i].pool != null)
                                {
                                    body.items[i].pool = responseContent.items[i].pool.Value;
                                }

                                if (responseContent.items[i].pool_geo != null && responseContent.items[i].pool_geo.Count > 0)
                                {
                                    body.items[i].pool_geo = new List<String>();

                                    foreach (var item in responseContent.items[i].pool_geo)
                                    {
                                        body.items[i].pool_geo.Add(item.Value);
                                    }
                                }

                                if (responseContent.items[i].pool_urls != null && responseContent.items[i].pool_urls.Count > 0)
                                {
                                    body.items[i].pool_urls = new List<String>();

                                    foreach (var item in responseContent.items[i].pool_urls)
                                    {
                                        body.items[i].pool_urls.Add(item.Value);
                                    }
                                }


                                body.items[i].pool_ssl = responseContent.items[i].pool_ssl.Value;
                                body.items[i].wal_id = responseContent.items[i].wal_id.Value;
                                body.items[i].dpool_ssl = responseContent.items[i].dpool_ssl.Value;
                                if (responseContent.items[i].dpool_urls != null && responseContent.items[i].dpool_urls.Count > 0)
                                {
                                    body.items[i].dpool_urls = new List<String>();

                                    foreach (var item in responseContent.items[i].dpool_urls)
                                    {
                                        body.items[i].dpool_urls.Add(item.Value);
                                    }
                                }
                                body.items[i].miner = responseContent.items[i].miner.Value;
                                body.items[i].miner_config = new ExpandoObject();


                                if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.url?.Value))
                                {
                                    body.items[i].miner_config.url = responseContent.items[i].miner_config?.url?.Value;
                                }

                                if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.port?.Value))
                                {
                                    body.items[i].miner_config.port = responseContent.items[i].miner_config?.port?.Value;
                                }

                                if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.server?.Value))
                                {
                                    body.items[i].miner_config.server = responseContent.items[i].miner_config?.server?.Value;
                                }

                                if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.algo?.Value))
                                {
                                    body.items[i].miner_config.algo = responseContent.items[i].miner_config.algo.Value;
                                }

                                if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.worker?.Value))
                                {
                                    body.items[i].miner_config.worker = responseContent.items[i].miner_config.worker.Value;
                                }

                                if (!String.IsNullOrEmpty(responseContent.items[i]?.miner_config?.user_config?.Value))
                                {
                                    body.items[i].miner_config.user_config = responseContent.items[i].miner_config.user_config.Value;
                                }

                                if (coin.StartsWith("Zerg-", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.pass?.Value))
                                    {
                                        body.items[i].miner_config.pass = "c=BTC";
                                    }
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.template?.Value))
                                    {
                                        body.items[i].miner_config.template = "bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa";
                                    }
                                }
                                else if (coin.StartsWith("Nicehash-", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.pass?.Value))
                                    {
                                        body.items[i].miner_config.pass = "x";
                                    }
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.template?.Value))
                                    {
                                        body.items[i].miner_config.template = "3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%";
                                    }
                                }
                                else if (coin.StartsWith("Prohashing-", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.pass?.Value))
                                    {
                                        body.items[i].miner_config.pass = responseContent.items[i].miner_config?.pass?.Value;
                                    }
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.template?.Value))
                                    {
                                        body.items[i].miner_config.template = "05sonicblue";
                                    }
                                }
                                else if (coin.StartsWith("MiningDutch-", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.pass?.Value))
                                    {
                                        body.items[i].miner_config.pass = responseContent.items[i].miner_config?.pass?.Value;
                                    }
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.template?.Value))
                                    {
                                        body.items[i].miner_config.template = "05sonicblue";
                                    }
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.url?.Value))
                                    {
                                        string url = Convert.ToString(responseContent.items[i].miner_config?.url?.Value);
                                        if (
                                            url.Contains("viabtc", StringComparison.OrdinalIgnoreCase) ||
                                            url.Contains("antpool", StringComparison.OrdinalIgnoreCase)
                                            )
                                        {
                                            donationRecord.Wallet = "05sonicblue";
                                        }
                                        if (
                                            url.Contains("f2pool", StringComparison.OrdinalIgnoreCase)
                                            )
                                        {
                                            donationRecord.Wallet = "michaeldlesk";
                                        }
                                        if (url.Contains("litecoinpool", StringComparison.OrdinalIgnoreCase))
                                        {
                                            donationRecord.Wallet = "michaeldlesk.donation";
                                            donationRecord.Password = "x";
                                        }
                                        body.items[i].miner_config.url = url;
                                    }
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.pass?.Value))
                                    {
                                        if (!String.IsNullOrEmpty(donationRecord.Password))
                                        {
                                            body.items[i].miner_config.pass = donationRecord.Password;
                                        }
                                        else
                                        {
                                            body.items[i].miner_config.pass = responseContent.items[i].miner_config?.pass?.Value;
                                        }
                                    }
                                    if (!String.IsNullOrEmpty(responseContent.items[i].miner_config?.template?.Value))
                                    {
                                        body.items[i].miner_config.template = donationRecord.Wallet;
                                    }
                                }
                            }
                            else
                            {
                                Common.Logger.Push("Coin in FS Not Configured: " + coin);
                            }
                        }

                        client = new RestClient("https://api2.hiveos.farm/api/v2");
                        request = new RestRequest(String.Format("/farms/{0}/fs", farmId));
                        request.AddHeader("Authorization", "Bearer " + hiveApiKey);
                        string jsonData = JsonConvert.SerializeObject(body);
                        debuggingData = jsonData;
                        request.AddStringBody(jsonData, DataFormat.Json);
                        response = client.Post(request);
                        responseContent = JsonConvert.DeserializeObject(response.Content);
                        result = responseContent?.id?.Value?.ToString();
                        //Common.Logger.Push("Donation FS Created: " + debuggingData);
                    }
                    catch (Exception ex)
                    {
                        //Logger.Push(String.Format("Error while trying to create donation flightsheet: {0} - {1} - Version: {2} - Existing Config: {3}", ex.Message, debuggingData, System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version, responseDebuggingData));
                    }
                }
            }
            catch { }
            return result;
        }

        public static bool ValidateApiKey(string hiveApiKey, string farmId)
        {
            bool result = false;
            try
            {
                RestClient client = new RestClient("https://api2.hiveos.farm/api/v2");
                RestRequest request = new RestRequest("/account");
                request.AddHeader("Authorization", "Bearer " + hiveApiKey);
                var response = client.Get(request);
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    result = true;
                }
            }
            catch(Exception ex)
            {

            }
            return result;
        }
    }
}
