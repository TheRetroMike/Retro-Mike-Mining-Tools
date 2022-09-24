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
        //public static int? selectedWorkerId;
        public static HiveOsRigConfig? selectedWorker;
        public static IList<Flightsheet>? flightsheets;

        //private readonly ILogger<ProfitSwitchingModel> _logger;

        //public ProfitSwitchingModel(ILogger<ProfitSwitchingModel> logger)
        //{
        //    _logger = logger;
        //}

        public void OnGet()
        {
            if (rigs == null)
            {
                rigs = HiveRigDAO.GetRecords();
            }

            if (flightsheets == null)
            {
                var coreConfig = CoreConfigDAO.GetCoreConfig();
                if (coreConfig != null && !String.IsNullOrEmpty(coreConfig.HiveApiKey) && !String.IsNullOrEmpty(coreConfig.HiveFarmID))
                {
                    flightsheets = HiveUtilities.GetAllFlightsheets(coreConfig.HiveApiKey, coreConfig.HiveFarmID);
                }
            }
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
            return new JsonResult(Coins.CoinList.OrderBy(x => x.Name));
        }

        public JsonResult OnGetMasterFlightsheetList([DataSourceRequest] DataSourceRequest request)
        {
            var coreConfig = CoreConfigDAO.GetCoreConfig();
            if (coreConfig != null && !String.IsNullOrEmpty(coreConfig.HiveApiKey) && !String.IsNullOrEmpty(coreConfig.HiveFarmID))
            {
                flightsheets = HiveUtilities.GetAllFlightsheets(coreConfig.HiveApiKey, coreConfig.HiveFarmID);
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

        public JsonResult OnPostCreateCoins([DataSourceRequest] DataSourceRequest request, HiveOsRigCoinConfig record)
        {
            HiveOsRigCoinConfig existingRecord = null;
            if (selectedWorker != null)
            {
                var existingRecords = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList()).Where(x => x.Ticker.Equals(record.Ticker, StringComparison.OrdinalIgnoreCase)).ToList();
                if (existingRecords == null || existingRecords?.Count == 0)
                {
                    record.WorkerId = selectedWorker.Id;
                    HiveRigCoinDAO.AddRecord(record);
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList());
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroyCoins([DataSourceRequest] DataSourceRequest request, HiveOsRigCoinConfig record)
        {
            if (selectedWorker != null)
            {
                HiveRigCoinDAO.DeleteRecord(record);
                coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList());
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdateCoins([DataSourceRequest] DataSourceRequest request, HiveOsRigCoinConfig record)
        {
            if (selectedWorker != null)
            {
                HiveRigCoinDAO.UpdateRecord(record);
                coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList());
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, HiveOsRigConfig rig)
        {
            var existingRecord = HiveRigDAO.GetRecord(rig?.Name);
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
            rigs = HiveRigDAO.GetRecords();
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
                    if (existingRecord == null)
                    {
                        worker.DonationAmount = Constants.DEFAULT_DONATION;
                        worker.Enabled = false;
                        HiveRigDAO.AddRig(worker);
                    }
                }
            }
            rigs = HiveRigDAO.GetRecords();
            return new JsonResult("Hive OS Rigs Imported");
        }

        public JsonResult OnPostHiveOsRowSelect(HiveOsRigConfig workerId)
        {
            //bind coins: HiveOsRigCoinConfig
            //selectedWorkerId = workerId.Id;
            selectedWorker = workerId;
            coins = HiveRigCoinDAO.GetRecords(workerId.Id, flightsheets?.ToList());
            return new JsonResult("Data Bound");
        }

        public JsonResult OnPostImportCoins()
        {
            if (selectedWorker != null)
            {
                if (!String.IsNullOrEmpty(selectedWorker.WhatToMineEndpoint))
                {
                    var wtmCoins = WhatToMineUtilities.GetCoinList(selectedWorker.WhatToMineEndpoint);
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList());
                    foreach (var coin in wtmCoins)
                    {
                        if (coins.Where(x => x.Ticker.Equals(coin.Ticker,StringComparison.OrdinalIgnoreCase)).FirstOrDefault() == null)
                        {
                            try
                            {
                                HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                {
                                    Ticker = coin.Ticker,
                                    Enabled = false,
                                    WorkerId = selectedWorker.Id
                                });
                            }
                            catch(Exception ex)
                            {
                                Thread.Sleep(1000);
                                HiveRigCoinDAO.AddRecord(new HiveOsRigCoinConfig()
                                {
                                    Ticker = coin.Ticker,
                                    Enabled = false,
                                    WorkerId = selectedWorker.Id
                                });
                            }
                        }
                    }
                    coins = HiveRigCoinDAO.GetRecords(selectedWorker.Id, flightsheets?.ToList());
                }
            }
            return new JsonResult("Rig Coins Imported");
        }
    }
}