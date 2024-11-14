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
    public class AsicsModel : PageModel
    {
        public static IList<AsicConfig>? asics;
        public static IList<AsicCoinConfig> coins;
        public static AsicConfig? selectedWorker;
        private static IConfiguration systemConfiguration;
        private static bool multiUserMode = false;
        private static string? username;
        public static CoreConfig Config { get; set; }
        public bool IsMultiUser { get; set; }

        public AsicsModel(IConfiguration configuration)
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
                if (Config == null && !String.IsNullOrEmpty(username))
                {
                    CoreConfigDAO.InitialConfiguration(username);
                    Config = CoreConfigDAO.GetCoreConfig(username);
                }
                asics = AsicDAO.GetRecords(Config, true, username, true).ToList();
            }
            else
            {
                Config = CoreConfigDAO.GetCoreConfig();
                asics = AsicDAO.GetRecords(Config, true, null, false);
            }

            var coreConfig = CoreConfigDAO.GetCoreConfig();
            if (multiUserMode)
            {
                coreConfig = CoreConfigDAO.GetCoreConfig(username);
            }
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(asics.ToDataSourceResult(request));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, AsicConfig record)
        {
            var existingRecord = AsicDAO.GetRecord(record?.Name, null);
            if (multiUserMode)
            {
                record.Username = username;
                existingRecord = AsicDAO.GetRecord(record?.Name, username);
            }
            if (existingRecord == null)
            {
                AsicDAO.Add(record);
                if (multiUserMode)
                {
                    asics = AsicDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    asics = AsicDAO.GetRecords();
                }
            }

            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, AsicConfig record)
        {
            AsicDAO.DeleteRecord(record);
            if (multiUserMode)
            {
                asics = AsicDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                asics = AsicDAO.GetRecords();
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, AsicConfig record)
        {
            AsicDAO.Update(record);
            if (multiUserMode)
            {
                asics = AsicDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                asics = AsicDAO.GetRecords();
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostReadCoins([DataSourceRequest] DataSourceRequest request)
        {
            if (coins != null)
            {
                return new JsonResult(coins.ToDataSourceResult(request));
            }
            return new JsonResult(String.Empty);
        }

        public JsonResult OnPostCreateCoins([DataSourceRequest] DataSourceRequest request, AsicCoinConfig record)
        {
            AsicCoinConfig existingRecord = null;
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

                AsicCoinDAO.AddRecord(record);

                if (multiUserMode)
                {
                    coins = AsicCoinDAO.GetRecords(selectedWorker.Id, Config, true, false, true).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    coins = AsicCoinDAO.GetRecords(selectedWorker.Id, Config, true, false, false);
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroyCoins([DataSourceRequest] DataSourceRequest request, AsicCoinConfig record)
        {
            if (selectedWorker != null)
            {
                AsicCoinDAO.DeleteRecord(record);

                if (multiUserMode)
                {
                    coins = AsicCoinDAO.GetRecords(selectedWorker.Id, Config, true, false, multiUserMode).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    coins = AsicCoinDAO.GetRecords(selectedWorker.Id, Config, true, false, false);
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdateCoins([DataSourceRequest] DataSourceRequest request, AsicCoinConfig record)
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
                AsicCoinDAO.UpdateRecord(record);

                if (multiUserMode)
                {
                    coins = AsicCoinDAO.GetRecords(selectedWorker.Id, Config, true, false, true).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    coins = AsicCoinDAO.GetRecords(selectedWorker.Id, Config, true, false, false);
                }
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostAsicRowSelect(AsicConfig workerId)
        {
            if (Config == null)
            {
                Config = new CoreConfig();
            }
            selectedWorker = workerId;


            if (multiUserMode)
            {
                coins = AsicCoinDAO.GetRecords(workerId.Id, Config, true, false, true).Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                coins = AsicCoinDAO.GetRecords(workerId.Id, Config, true, false, false);
            }

            return new JsonResult("Data Bound");
        }

    }
}
