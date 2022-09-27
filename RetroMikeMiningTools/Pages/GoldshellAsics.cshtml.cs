using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Utilities;
using System.Net;
using System.Net.NetworkInformation;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class GoldshellAsicsModel : PageModel
    {
        public static IList<GoldshellAsicConfig>? data;

        public static IList<GoldshellAsicCoinConfig> coins;
        public static GoldshellAsicConfig? selectedWorker;
        private static double? importProgress;
        private static List<String> networkDevices;
        private static IConfiguration systemConfiguration;
        private static CountdownEvent countdown;

        public GoldshellAsicsModel(IConfiguration configuration)
        {
            systemConfiguration = configuration;
        }

        public IActionResult OnGet()
        {
            if (systemConfiguration != null)
            {
                var multiUserModeConfig = systemConfiguration.GetValue<string>(Constants.MULTI_USER_MODE);
                if (!String.IsNullOrEmpty(multiUserModeConfig) && multiUserModeConfig == "true")
                {
                    return RedirectToPage("/Index");
                }
                    var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                if (hostPlatform != null)
                {
                    ViewData["Platform"] = hostPlatform;
                    if (hostPlatform.Equals(Constants.PLATFORM_DOCKER_ARM64))
                    {
                        Response.Redirect("/");
                    }
                }
            }

            if (data == null)
            {
                data = GoldshellAsicDAO.GetRecords();
            }
            if (importProgress == null)
            {
                importProgress = 0;
            }
            return Page();
            
        }

        public JsonResult OnGetMasterGroupList([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<LinkedGroup>();
            var groups = DAO.MiningGroupDAO.GetRecords();
            if (groups != null && groups.Count > 0)
            {
                foreach (var group in groups)
                {
                    result.Add(new LinkedGroup()
                    {
                        Id = group.Id,
                        Name = group.Name
                    });
                }
                
            }
            return new JsonResult(result);
        }

        public JsonResult OnGetMasterCoinList([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(Coins.AsicCoinList.OrderBy(x => x.Name));
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostReadCoins([DataSourceRequest] DataSourceRequest request)
        {
            if (coins != null)
            {
                return new JsonResult(coins.ToDataSourceResult(request));
            }
            return new JsonResult(String.Empty);
        }

        public JsonResult OnPostCreateCoins([DataSourceRequest] DataSourceRequest request, GoldshellAsicCoinConfig record)
        {
            GoldshellAsicCoinConfig existingRecord = null;
            if (selectedWorker != null)
            {
                var existingRecords = GoldshellAsicCoinDAO.GetRecords(selectedWorker.Id).Where(x => x.Ticker.Equals(record.Ticker,StringComparison.OrdinalIgnoreCase)).ToList();
                if (existingRecords == null || existingRecords?.Count == 0)
                {
                    record.WorkerId = selectedWorker.Id;
                    GoldshellAsicCoinDAO.AddRecord(record);
                    coins = GoldshellAsicCoinDAO.GetRecords(selectedWorker.Id);
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroyCoins([DataSourceRequest] DataSourceRequest request, GoldshellAsicCoinConfig record)
        {
            if (selectedWorker != null)
            {
                GoldshellAsicCoinDAO.DeleteRecord(record);
                coins = GoldshellAsicCoinDAO.GetRecords(selectedWorker.Id);
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdateCoins([DataSourceRequest] DataSourceRequest request, GoldshellAsicCoinConfig record)
        {
            if (selectedWorker != null)
            {
                GoldshellAsicCoinDAO.UpdateRecord(record);
                coins = GoldshellAsicCoinDAO.GetRecords(selectedWorker.Id);
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, GoldshellAsicConfig rig)
        {
            var existingRecord = GoldshellAsicDAO.GetRecord(rig?.Name);
            if (existingRecord == null)
            {
                if (!String.IsNullOrEmpty(rig?.WhatToMineEndpoint))
                {
                    var uri = new Uri(rig.WhatToMineEndpoint);
                    if (uri != null && uri.AbsolutePath.ToLower() == "/asic")
                    {
                        var includePort = false;
                        if ((uri.Scheme.ToLower() == "https" && uri.Port != 443) || (uri.Scheme.ToLower() == "http" && uri.Port != 80))
                        {
                            includePort = true;
                        }

                        if (includePort)
                        {
                            rig.WhatToMineEndpoint = String.Format("{0}://{1}:{2}{3}{4}", uri.Scheme, uri.Host, uri.Port, "/asic.json", uri.Query);
                        }
                        else
                        {
                            rig.WhatToMineEndpoint = String.Format("{0}://{1}{2}{3}", uri.Scheme, uri.Host, "/asic.json", uri.Query);
                        }
                    }
                }
                GoldshellAsicDAO.AddRig(rig);
                existingRecord = GoldshellAsicDAO.GetRecord(rig?.Name);
                data.Add(existingRecord);
            }

            return new JsonResult(new[] { existingRecord }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, GoldshellAsicConfig record)
        {
            var currentRecord = GoldshellAsicDAO.GetRecord(record.Id);
            if (!String.IsNullOrEmpty(record?.WhatToMineEndpoint) && currentRecord?.WhatToMineEndpoint != record?.WhatToMineEndpoint)
            {
                var uri = new Uri(record.WhatToMineEndpoint);
                if (uri != null && uri.AbsolutePath.ToLower() == "/asic")
                {
                    var includePort = false;
                    if ((uri.Scheme.ToLower() == "https" && uri.Port != 443) || (uri.Scheme.ToLower() == "http" && uri.Port != 80))
                    {
                        includePort = true;
                    }
                    if (includePort)
                    {
                        record.WhatToMineEndpoint = String.Format("{0}://{1}:{2}{3}{4}", uri.Scheme, uri.Host, uri.Port, "/asic.json", uri.Query);
                    }
                    else
                    {
                        record.WhatToMineEndpoint = String.Format("{0}://{1}{2}{3}", uri.Scheme, uri.Host, "/asic.json", uri.Query);
                    }
                }
                
            }
            GoldshellAsicDAO.UpdateRecord(record);
            data = GoldshellAsicDAO.GetRecords();
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, GoldshellAsicConfig rig)
        {
            GoldshellAsicDAO.DeleteRecord(rig);
            var coinRecords = GoldshellAsicCoinDAO.GetRecords(rig.Id);
            if (coinRecords != null)
            {
                foreach (var coinRecord in coinRecords)
                {
                    GoldshellAsicCoinDAO.DeleteRecord(coinRecord);
                }
            }
            data.Remove(data.First(x => x.Id == rig.Id));
            selectedWorker = null;
            coins = null;
            return new JsonResult(new[] { rig }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostAsicRowSelect(GoldshellAsicConfig workerId)
        {
            selectedWorker = workerId;
            coins = GoldshellAsicCoinDAO.GetRecords(workerId.Id);
            return new JsonResult("Data Bound");
        }

        public JsonResult OnPostImportAsicsProgress()
        {
            return new JsonResult(Math.Round(importProgress.Value, 2));
        }
        public JsonResult OnPostImportAsics()
        {
            try
            {
                importProgress = 0;
                networkDevices = new List<string>();
                countdown = new CountdownEvent(1);

                IPAddress? gateway = null;
                if (systemConfiguration != null)
                {
                    var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                    var coreConfig = CoreConfigDAO.GetCoreConfig();
                    if (!String.IsNullOrEmpty(hostPlatform) && !String.IsNullOrEmpty(coreConfig.DockerHostIp))
                    {
                        gateway = IPAddress.Parse(coreConfig.DockerHostIp);
                    }
                }
                if (gateway == null)
                {
                    gateway = WebUtilities.GetDefaultGateway();
                }
                
                if (gateway != null)
                {

                    var ipv4Gateway = gateway.MapToIPv4().ToString();

                    var ipPrefix = ipv4Gateway.Substring(0, ipv4Gateway.LastIndexOf("."));
                    for (int i = 1; i <= 255; i++)
                    {
                        var ipToCheck = IPAddress.Parse(ipPrefix + String.Format(".{0}", i));
                        Ping pinger = new Ping();
                        pinger.PingCompleted += Pinger_PingCompleted;
                        pinger.SendAsync(ipToCheck, 100, ipToCheck);
                        countdown.AddCount();
                    }

                    countdown.Signal();
                    countdown.Wait();
                }
                if (networkDevices.Count > 0)
                {
                    var progressIncrementer = 50 / networkDevices.Count;
                    foreach (var ip in networkDevices)
                    {
                        var goldshellStatsUrl = String.Format("http://{0}/mcb/status", ip);
                        var client = new RestClient(goldshellStatsUrl);
                        var request = new RestRequest();
                        try
                        {
                            var response = client.Get(request);
                            if (response != null && response.StatusCode == HttpStatusCode.OK)
                            {
                                dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
                                var deviceModel = responseContent?.model.ToString();
                                bool isHnsSiaDevice = false;
                                if (deviceModel != null)
                                {
                                    string? defaultWtmEndpoint = "";
                                    if (Constants.GOLDSHELL_ASIC_DEFAULT_WTM.ContainsKey(deviceModel))
                                    {
                                        defaultWtmEndpoint = Constants.GOLDSHELL_ASIC_DEFAULT_WTM[deviceModel];
                                    }
                                    if (deviceModel != null)
                                    {
                                        if (deviceModel.ToString() == "Goldshell-HSBox")
                                        {
                                            isHnsSiaDevice = true;
                                        }
                                        if (GoldshellAsicDAO.GetRecord(deviceModel) != null)
                                        {
                                            int index = 1;
                                            while (GoldshellAsicDAO.GetRecord(deviceModel) != null)
                                            {
                                                index++;
                                                deviceModel = String.Format("{0}-{1}", deviceModel, index.ToString());
                                            }
                                        }
                                        GoldshellAsicDAO.AddRig(new GoldshellAsicConfig()
                                        {
                                            Name = deviceModel,
                                            ApiBasePath = String.Format("http://{0}", ip),
                                            ApiPassword = "123456789",
                                            WhatToMineEndpoint = defaultWtmEndpoint,
                                            DonationAmount = "1%",
                                            MiningMode = Enums.MiningMode.Profit
                                        });

                                        var newRecord = GoldshellAsicDAO.GetRecord(deviceModel);
                                        if (newRecord != null && !String.IsNullOrEmpty(newRecord.WhatToMineEndpoint))
                                        {
                                            var wtmCoins = WhatToMineUtilities.GetCoinList(newRecord.WhatToMineEndpoint);
                                            foreach (var coin in wtmCoins)
                                            {
                                                try
                                                {
                                                    string? coinAlgo = null;
                                                    if (isHnsSiaDevice && coin.Ticker.ToString().ToUpper() == "SC")
                                                    {
                                                        coinAlgo = "blake2b(SC)";
                                                    }

                                                    if (isHnsSiaDevice && coin.Ticker.ToString().ToUpper() == "HNS")
                                                    {
                                                        coinAlgo = "blake2bsha3(HNS)";
                                                    }

                                                    GoldshellAsicCoinDAO.AddRecord(new GoldshellAsicCoinConfig()
                                                    {
                                                        Ticker = coin.Ticker,
                                                        Enabled = false,
                                                        WorkerId = newRecord.Id,
                                                        CoinAlgo = coinAlgo
                                                    });
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        catch { }
                        finally
                        {
                            importProgress = importProgress + progressIncrementer;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            importProgress = 100;
            data = GoldshellAsicDAO.GetRecords();
            
            return new JsonResult("Goldshell ASIC Import Complete");
        }

        private void Pinger_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            try
            {
                IPAddress ipToCheck = (IPAddress)e.UserState;

                if (e.Reply != null && e.Reply.Status == IPStatus.Success)
                {
                    networkDevices.Add(ipToCheck.ToString());
                }
            }
            finally
            {
                countdown.Signal();
                importProgress = importProgress + 0.2;
            }
        }

        public JsonResult OnPostImportCoins()
        {
            if (selectedWorker != null)
            {
                if (!String.IsNullOrEmpty(selectedWorker.WhatToMineEndpoint))
                {
                    var wtmCoins = WhatToMineUtilities.GetCoinList(selectedWorker.WhatToMineEndpoint);
                    coins = GoldshellAsicCoinDAO.GetRecords(selectedWorker.Id);
                    foreach (var coin in wtmCoins)
                    {
                        if (coins.Where(x => x.Ticker==coin.Ticker).FirstOrDefault() == null)
                        {
                            try
                            {
                                GoldshellAsicCoinDAO.AddRecord(new GoldshellAsicCoinConfig()
                                {
                                    Ticker = coin.Ticker,
                                    Enabled = false,
                                    WorkerId = selectedWorker.Id
                                });
                            }
                            catch(Exception ex)
                            {
                                Thread.Sleep(1000);
                                GoldshellAsicCoinDAO.AddRecord(new GoldshellAsicCoinConfig()
                                {
                                    Ticker = coin.Ticker,
                                    Enabled = false,
                                    WorkerId = selectedWorker.Id
                                });
                            }
                        }
                    }
                    coins = GoldshellAsicCoinDAO.GetRecords(selectedWorker.Id);
                }
            }
            return new JsonResult("Rig Coins Imported");
        }
    }
}