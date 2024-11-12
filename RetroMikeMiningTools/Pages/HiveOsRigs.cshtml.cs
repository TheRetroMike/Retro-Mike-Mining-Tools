using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class HiveOsRigsModel : PageModel
    {
        public static IList<HiveOsRigConfig>? rigs;
        public static IList<HiveOsRigCoinConfig> coins;
        public static IList<ZergAlgoConfig> zergAlgos;
        //public static int? selectedWorkerId;
        public static HiveOsRigConfig? selectedWorker;
        public static IList<Flightsheet>? flightsheets;
        private static IConfiguration systemConfiguration;
        public static bool multiUserMode = false;
        private static string? username;
        public bool IsMultiUser { get; set; }
        public static CoreConfig Config { get; set; }

        public HiveOsRigsModel(IConfiguration configuration)
        {
            systemConfiguration = configuration;
        }

        public void OnGet()
        {
            if (systemConfiguration != null)
            {
                var multiUserModeConfig = systemConfiguration.GetValue<string>(Constants.MULTI_USER_MODE);
                if (!String.IsNullOrEmpty(multiUserModeConfig) && multiUserModeConfig == "true")
                {
                    username = User?.Identity?.Name;
                    multiUserMode = true;
                    IsMultiUser = true;
                    ViewData["MultiUser"] = true;
                }

                var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                if (hostPlatform != null)
                {
                    ViewData["Platform"] = hostPlatform;
                }
            }

            if (multiUserMode)
            {
                Config = CoreConfigDAO.GetCoreConfig(username);
                if (Config==null && !String.IsNullOrEmpty(username))
                {
                    CoreConfigDAO.InitialConfiguration(username);
                    Config = CoreConfigDAO.GetCoreConfig(username);
                }
                rigs = HiveRigDAO.GetRecords(Config, true, username, true).ToList();
            }
            else
            {
                Config = CoreConfigDAO.GetCoreConfig();
                rigs = HiveRigDAO.GetRecords(Config, true, null, false);
            }

            var coreConfig = CoreConfigDAO.GetCoreConfig();
            if (multiUserMode)
            {
                coreConfig = CoreConfigDAO.GetCoreConfig(username);
            }
            if (coreConfig != null && !String.IsNullOrEmpty(coreConfig.HiveApiKey) && !String.IsNullOrEmpty(coreConfig.HiveFarmID))
            {
                flightsheets = HiveUtilities.GetAllFlightsheets(coreConfig.HiveApiKey, coreConfig.HiveFarmID);
            }
        }

        public JsonResult OnGetMasterGroupList([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<LinkedGroup>();
            var groups = DAO.MiningGroupDAO.GetRecords();
            if (multiUserMode)
            {
                groups = MiningGroupDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
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
            return new JsonResult(Coins.CoinList.OrderBy(x => x.Name));
        }

        //public JsonResult OnGetMasterZpoolAlgoList([DataSourceRequest] DataSourceRequest request)
        //{
        //    return new JsonResult(Coins.ZpoolAlgoList.OrderBy(x => x.Name));
        //}

        public JsonResult OnGetMasterFlightsheetList([DataSourceRequest] DataSourceRequest request)
        {
            var coreConfig = CoreConfigDAO.GetCoreConfig();
            if (multiUserMode)
            {
                coreConfig = CoreConfigDAO.GetCoreConfig(username);
            }
            if (coreConfig != null && !String.IsNullOrEmpty(coreConfig.HiveApiKey) && !String.IsNullOrEmpty(coreConfig.HiveFarmID))
            {
                flightsheets = HiveUtilities.GetAllFlightsheets(coreConfig.HiveApiKey, coreConfig.HiveFarmID);
                flightsheets.Add(new Flightsheet() { Id = 0, Name = "" });
                return new JsonResult(flightsheets?.OrderBy(x => x.Name));
            }
            else
            {
                return new JsonResult(new List<Flightsheet>());
            }
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(rigs.ToDataSourceResult(request));
        }

        public JsonResult OnPostReadCoins([DataSourceRequest] DataSourceRequest request)
        {
            if (coins != null)
            {
                return new JsonResult(coins.ToDataSourceResult(request));
            }
            return new JsonResult(String.Empty);
        }

        public JsonResult OnPostReadZergAlgos([DataSourceRequest] DataSourceRequest request)
        {
            if (coins != null)
            {
                return new JsonResult(zergAlgos.ToDataSourceResult(request));
            }
            return new JsonResult(String.Empty);
        }

        public JsonResult OnPostCreateCoins([DataSourceRequest] DataSourceRequest request, HiveOsRigCoinConfig record)
        {
            HiveOsRigCoinConfig existingRecord = null;
            if (selectedWorker != null)
            {
                if (multiUserMode)
                {
                    record.Username = username;
                }
                record.WorkerId = selectedWorker.Id;

                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("Zerg-"))
                {
                    record.Algo = record.Ticker.Remove(0, 5);
                }
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("Zpool-"))
                {
                    record.Algo = record.Ticker.Remove(0, 6);
                }
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("Prohashing-"))
                {
                    record.Algo = record.Ticker.Remove(0, 11);
                }
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("MiningDutch-"))
                {
                    record.Algo = record.Ticker.Remove(0, 12);
                }
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("Unmineable-"))
                {
                    record.Algo = record.Ticker.Remove(0, 11);
                }
                if (record.Ticker != null && record.Ticker.StartsWith("ZergProvider-"))
                {
                    //record.Ticker = record.Ticker.Remove(0, 13);
                    record.PrimaryProvider = "ZergProvider";
                }

                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("Zerg-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 5);
                }
                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("Zpool-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 6);
                }
                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("Prohashing-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 11);
                }
                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("MiningDutch-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 12);
                }
                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("Unmineable-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 11);
                }
                if (record.SecondaryTicker != null && record.SecondaryTicker.StartsWith("ZergProvider-"))
                {
                    //record.SecondaryTicker = record.SecondaryTicker.Remove(0, 13);
                    record.SecondaryProvider = "ZergProvider";
                }

                HiveRigCoinDAO.AddRecord(record);
                
                if (multiUserMode)
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, true).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, false);
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostCreateZergAlgos([DataSourceRequest] DataSourceRequest request, ZergAlgoConfig record)
        {
            ZergAlgoConfig existingRecord = null;
            if (selectedWorker != null)
            {
                var existingRecords = ZergAlgoDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList()).Where(x => x.Algo.Equals(record.Algo, StringComparison.OrdinalIgnoreCase)).ToList();
                if (multiUserMode)
                {
                    record.Username = username;
                    existingRecords = ZergAlgoDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList()).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && x.Algo.Equals(record.Algo, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                if (existingRecords == null || existingRecords?.Count == 0)
                {
                    record.WorkerId = selectedWorker.Id;
                    ZergAlgoDAO.AddRecord(record);
                    zergAlgos = ZergAlgoDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList());
                    if (multiUserMode)
                    {
                        zergAlgos = ZergAlgoDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList()).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroyCoins([DataSourceRequest] DataSourceRequest request, HiveOsRigCoinConfig record)
        {
            if (selectedWorker != null)
            {
                HiveRigCoinDAO.DeleteRecord(record);
                
                if (multiUserMode)
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, multiUserMode).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, false);
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroyZergAlgos([DataSourceRequest] DataSourceRequest request, ZergAlgoConfig record)
        {
            if (selectedWorker != null)
            {
                ZergAlgoDAO.DeleteRecord(record);
                zergAlgos = ZergAlgoDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList());
                if(multiUserMode)
                {
                    zergAlgos = ZergAlgoDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList()).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdateCoins([DataSourceRequest] DataSourceRequest request, HiveOsRigCoinConfig record)
        {
            if (selectedWorker != null)
            {
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("Zerg-"))
                {
                    record.Algo = record.Ticker.Remove(0, 5);
                }
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("Zpool-"))
                {
                    record.Algo = record.Ticker.Remove(0, 6);
                }
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("Prohashing-"))
                {
                    record.Algo = record.Ticker.Remove(0, 11);
                }
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("MiningDutch-"))
                {
                    record.Algo = record.Ticker.Remove(0, 12);
                }
                if (record.Ticker != null && record.Algo == null && record.Ticker.StartsWith("Unmineable-"))
                {
                    record.Algo = record.Ticker.Remove(0, 11);
                }
                if (record.Ticker != null && record.Ticker.StartsWith("ZergProvider-"))
                {
                    record.PrimaryProvider = "ZergProvider";
                }

                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("Zerg-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 5);
                }
                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("Zpool-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 6);
                }
                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("Prohashing-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 11);
                }
                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("MiningDutch-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 12);
                }
                if (record.SecondaryTicker != null && record.SecondaryAlgo == null && record.SecondaryTicker.StartsWith("Unmineable-"))
                {
                    record.SecondaryAlgo = record.SecondaryTicker.Remove(0, 11);
                }
                if (record.SecondaryTicker != null && record.SecondaryTicker.StartsWith("ZergProvider-"))
                {
                    //record.SecondaryTicker = record.SecondaryTicker.Remove(0, 13);
                    record.SecondaryProvider = "ZergProvider";
                }
                HiveRigCoinDAO.UpdateRecord(record);
                
                if (multiUserMode)
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, true).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, false);
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdateZergAlgos([DataSourceRequest] DataSourceRequest request, ZergAlgoConfig record)
        {
            if (selectedWorker != null)
            {
                ZergAlgoDAO.UpdateRecord(record);
                zergAlgos = ZergAlgoDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList());
                if (multiUserMode)
                {
                    zergAlgos = ZergAlgoDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList()).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, HiveOsRigConfig rig)
        {
            var existingRecord = HiveRigDAO.GetRecord(rig?.Name);
            if (multiUserMode)
            {
                rig.Username = username;
                rig.DonationAmount = Constants.MULTI_USER_MODE_DONATION;
                existingRecord = HiveRigDAO.GetRecord(rig?.Name, username);
            }
            else
            {
                rig.DonationAmount = Constants.DEFAULT_DONATION;
            }
            if (existingRecord == null)
            {
                if (!String.IsNullOrEmpty(rig?.WhatToMineEndpoint))
                {
                    var uri = new Uri(rig.WhatToMineEndpoint);
                    if (uri != null && uri.AbsolutePath.ToLower() == "/coins")
                    {
                        var includePort = false;
                        if ((uri.Scheme.ToLower() == "https" && uri.Port != 443) || (uri.Scheme.ToLower() == "http" && uri.Port != 80))
                        {
                            includePort = true;
                        }

                        if (includePort)
                        {
                            rig.WhatToMineEndpoint = String.Format("{0}://{1}:{2}{3}{4}", uri.Scheme, uri.Host, uri.Port, "/coins.json", uri.Query);
                        }
                        else
                        {
                            rig.WhatToMineEndpoint = String.Format("{0}://{1}{2}{3}", uri.Scheme, uri.Host, "/coins.json", uri.Query);
                        }
                    }
                }
                HiveRigDAO.AddRig(rig);
                existingRecord = HiveRigDAO.GetRecord(rig?.Name);
                if (multiUserMode)
                {
                    existingRecord = HiveRigDAO.GetRecord(rig?.Name, username);
                }
                rigs.Add(existingRecord);
            }

            return new JsonResult(new[] { existingRecord }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, HiveOsRigConfig record)
        {
            var currentRecord = HiveRigDAO.GetRecord(record.Id);
            if (!String.IsNullOrEmpty(record?.WhatToMineEndpoint) && currentRecord?.WhatToMineEndpoint != record?.WhatToMineEndpoint)
            {
                var uri = new Uri(record.WhatToMineEndpoint);
                if (uri != null && uri.AbsolutePath.ToLower() == "/coins")
                {
                    var includePort = false;
                    if ((uri.Scheme.ToLower() == "https" && uri.Port != 443) || (uri.Scheme.ToLower() == "http" && uri.Port != 80))
                    {
                        includePort = true;
                    }
                    if (includePort)
                    {
                        record.WhatToMineEndpoint = String.Format("{0}://{1}:{2}{3}{4}", uri.Scheme, uri.Host, uri.Port, "/coins.json", uri.Query);
                    }
                    else
                    {
                        record.WhatToMineEndpoint = String.Format("{0}://{1}{2}{3}", uri.Scheme, uri.Host, "/coins.json", uri.Query);
                    }
                }
                
            }
            HiveRigDAO.UpdateRecord(record);

            if (multiUserMode)
            {
                rigs = HiveRigDAO.GetRecords(Config, true, username, true).ToList();
            }
            else
            {
                rigs = HiveRigDAO.GetRecords(Config, true, null, false);
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, HiveOsRigConfig rig)
        {
            HiveRigDAO.DeleteRecord(rig);
            rigs.Remove(rigs.First(x => x.Id == rig.Id));
            return new JsonResult(new[] { rig }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostImportHiveRigs()
        {
            var coreConfig = CoreConfigDAO.GetCoreConfig();
            if (multiUserMode)
            {
                coreConfig = CoreConfigDAO.GetCoreConfig(username);
            }
            if (String.IsNullOrEmpty(coreConfig?.HiveFarmID) || String.IsNullOrEmpty(coreConfig?.HiveApiKey))
            {
                return new JsonResult("Hive OS API Key and Farm ID are missing. Please set them on the Core Configuration Page");
            }
            var workers = HiveUtilities.GetWorkers(coreConfig.HiveApiKey, coreConfig.HiveFarmID);
            if (workers != null&& workers.Count > 0)
            {
                foreach (var worker in workers)
                {
                    var existingRecord = HiveRigDAO.GetRecord(worker?.Name);
                    if (multiUserMode)
                    {
                        existingRecord = HiveRigDAO.GetRecord(worker?.Name, username);
                    }
                    if (existingRecord == null)
                    {
                        if (multiUserMode)
                        {
                            worker.DonationAmount = Constants.MULTI_USER_MODE_DONATION;
                        }
                        else
                        {
                            worker.DonationAmount = Constants.DEFAULT_DONATION;
                        }
                        worker.Enabled = false;
                        worker.Username = username;
                        HiveRigDAO.AddRig(worker);
                    }
                }
            }
            
            if (multiUserMode)
            {
                rigs = HiveRigDAO.GetRecords(Config, true, username, true).ToList();
            }
            else
            {
                rigs = HiveRigDAO.GetRecords(Config, true, null, false);
            }
            return new JsonResult("Hive OS Rigs Imported");
        }

        public JsonResult OnPostCloneRig(int workerId)
        {
            var selectedRig = HiveRigDAO.GetRecord(workerId);
            var coins = HiveRigCoinDAO.GetRecords(workerId, flightsheets?.ToList(), Config, false, false, multiUserMode);
            if (selectedRig != null)
            {
                selectedRig.Id = 0;
                selectedRig.Name = String.Format("{0}_COPY", selectedRig.Name);
                var newId = HiveRigDAO.AddRig(selectedRig);
                
                if (coins != null)
                {
                    foreach (var coin in coins)
                    {
                        coin.Id = 0;
                        coin.WorkerId = newId;
                        HiveRigCoinDAO.AddRecord(coin);
                    }
                }
            }
            return new JsonResult("Cloned");
        }

        public JsonResult OnPostHiveOsRowSelect(HiveOsRigConfig workerId)
        {
            if (Config == null)
            {
                Config = new CoreConfig();
            }
            selectedWorker = workerId;
            

            if (multiUserMode)
            {
                coins = HiveRigCoinDAO.GetRecords(workerId.Id, flightsheets?.ToList(), Config, true, false, true).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                zergAlgos = ZergAlgoDAO.GetRecords(workerId.Id, flightsheets?.ToList()).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                coins = HiveRigCoinDAO.GetRecords(workerId.Id, flightsheets?.ToList(), Config, true, false, false);
                zergAlgos = ZergAlgoDAO.GetRecords(workerId.Id, flightsheets?.ToList());
            }

            return new JsonResult("Data Bound");
        }

        public JsonResult OnPostRefreshHashrates()
        {
            if (selectedWorker != null)
            {
                if (!String.IsNullOrEmpty(selectedWorker.WhatToMineEndpoint))
                {
                    var wtmCoins = WhatToMineUtilities.GetCoinList(selectedWorker.WhatToMineEndpoint);
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, true);
                    foreach (var coin in wtmCoins)
                    {
                        var coinRecord = coins.Where(x => x.Ticker.Equals(coin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (coinRecord != null)
                        {
                            coinRecord.Power = Convert.ToDecimal(coin.PowerConsumption);
                            if (!String.IsNullOrEmpty(coin.HashRate))
                            {
                                coinRecord.HashRateMH = Convert.ToDecimal(decimal.Parse(coin.HashRate, System.Globalization.NumberStyles.Float));
                            }
                            
                            HiveRigCoinDAO.UpdateRecord(coinRecord);
                        }
                    }
                }
            }
            return new JsonResult("Coins Updated");
        }

        public JsonResult OnPostImportCoins()
        {
            if (Config == null)
            {
                Config = CoreConfigDAO.GetCoreConfig();
            }
            if (selectedWorker != null)
            {
                if (!String.IsNullOrEmpty(selectedWorker.WhatToMineEndpoint))
                {
                    var wtmCoins = WhatToMineUtilities.GetCoinList(selectedWorker.WhatToMineEndpoint);
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, true);
                    foreach (var coin in wtmCoins)
                    {
                        if (String.IsNullOrEmpty(coin.HashRate))
                        {
                            coin.HashRate = "0";
                        }
                        if (coins.Where(x => x.Ticker.Equals(coin.Ticker,StringComparison.OrdinalIgnoreCase)).FirstOrDefault() == null)
                        {
                            try
                            {
                                if (multiUserMode)
                                {
                                    HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                    {
                                        Ticker = coin.Ticker,
                                        Enabled = false,
                                        WorkerId = selectedWorker.Id,
                                        Username = username,
                                        Algo = coin.Algorithm,
                                        HashRateMH = decimal.Parse(coin.HashRate, System.Globalization.NumberStyles.Float),
                                        Power = Convert.ToDecimal(coin.PowerConsumption),
                                    });
                                }
                                else
                                {
                                    HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                    {
                                        Ticker = coin.Ticker,
                                        Enabled = false,
                                        WorkerId = selectedWorker.Id,
                                        Algo = coin.Algorithm,
                                        HashRateMH = Convert.ToDecimal(coin.HashRate),
                                        Power = Convert.ToDecimal(coin.PowerConsumption)
                                    });
                                }
                            }
                            catch(Exception ex)
                            {
                                Thread.Sleep(1000);
                                if (multiUserMode)
                                {
                                    HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                    {
                                        Ticker = coin.Ticker,
                                        Enabled = false,
                                        WorkerId = selectedWorker.Id,
                                        Username = username,
                                        Algo = coin.Algorithm,
                                        HashRateMH = Convert.ToDecimal(coin.HashRate),
                                        Power = Convert.ToDecimal(coin.PowerConsumption)
                                    });
                                }
                                else
                                {
                                    HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                    {
                                        Ticker = coin.Ticker,
                                        Enabled = false,
                                        WorkerId = selectedWorker.Id,
                                        Algo = coin.Algorithm,
                                        HashRateMH = decimal.Parse(coin.HashRate, System.Globalization.NumberStyles.Float),
                                        Power = Convert.ToDecimal(coin.PowerConsumption)
                                    });
                                }
                            }
                        }
                    }
                }

                List<Coin> zergCoins;
                //Import Zerg Algo's
                if (selectedWorker.WhatToMineEndpoint != null)
                {
                    zergCoins = ZergUtilities.GetZergAlgos(WhatToMineUtilities.GetCoinList(selectedWorker.WhatToMineEndpoint));
                }
                else
                {
                    zergCoins = ZergUtilities.GetZergAlgos().ToList();
                }
                if (zergCoins != null)
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, true);
                    foreach (var zergCoin in zergCoins)
                    {
                        if (coins.Where(x => x.Ticker.Equals(zergCoin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() == null)
                        {
                            if (multiUserMode)
                            {
                                HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                {
                                    Ticker = zergCoin.Ticker,
                                    Enabled = false,
                                    WorkerId = selectedWorker.Id,
                                    Username = username,
                                    Power = Convert.ToDecimal(zergCoin.PowerConsumption),
                                    HashRateMH = Convert.ToDecimal(zergCoin.HashRate),
                                    Algo = zergCoin.Algorithm
                                });
                            }
                            else
                            {
                                HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                {
                                    Ticker = zergCoin.Ticker,
                                    Enabled = false,
                                    WorkerId = selectedWorker.Id,
                                    Power = Convert.ToDecimal(zergCoin.PowerConsumption),
                                    HashRateMH = Convert.ToDecimal(zergCoin.HashRate),
                                    Algo = zergCoin.Algorithm
                                });
                            }
                        }
                    }
                }

                //Import Prohashing Algo's
                List<Coin> proHashingCoins;
                //Import Zerg Algo's
                if (selectedWorker.WhatToMineEndpoint != null)
                {
                    proHashingCoins = ProhashingUtilities.GetAlgos(WhatToMineUtilities.GetCoinList(selectedWorker.WhatToMineEndpoint));
                }
                else
                {
                    proHashingCoins = ProhashingUtilities.GetAlgos().ToList();
                }
                if (proHashingCoins != null)
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, true);
                    foreach (var proHashingCoin in proHashingCoins)
                    {
                        if (coins.Where(x => x.Ticker.Equals(proHashingCoin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() == null)
                        {
                            if (multiUserMode)
                            {
                                HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                {
                                    Ticker = proHashingCoin.Ticker,
                                    Enabled = false,
                                    WorkerId = selectedWorker.Id,
                                    Username = username,
                                    Power = Convert.ToDecimal(proHashingCoin.PowerConsumption),
                                    HashRateMH = Convert.ToDecimal(proHashingCoin.HashRate),
                                    Algo = proHashingCoin.Algorithm
                                });
                            }
                            else
                            {
                                HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                {
                                    Ticker = proHashingCoin.Ticker,
                                    Enabled = false,
                                    WorkerId = selectedWorker.Id,
                                    Power = Convert.ToDecimal(proHashingCoin.PowerConsumption),
                                    HashRateMH = Convert.ToDecimal(proHashingCoin.HashRate),
                                    Algo = proHashingCoin.Algorithm
                                });
                            }
                        }
                    }
                }

                
                if (multiUserMode)
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, true).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList(), Config, true, false, false);
                }
            }
            return new JsonResult("Rig Coins Imported");
        }
    }
}