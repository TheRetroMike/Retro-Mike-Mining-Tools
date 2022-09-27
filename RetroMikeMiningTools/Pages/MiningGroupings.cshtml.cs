using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class MiningGroupingsModel : PageModel
    {
        public static IList<GroupConfig>? data;
        private static IConfiguration systemConfiguration;
        private static bool multiUserMode = false;
        private static string? username;

        public MiningGroupingsModel(IConfiguration configuration)
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
                data = MiningGroupDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                data = MiningGroupDAO.GetRecords();
            }

        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, GroupConfig record)
        {
            var existingRecord = MiningGroupDAO.GetRecord(record?.Name, null);
            if (multiUserMode)
            {
                record.Username = username;
                existingRecord = MiningGroupDAO.GetRecord(record?.Name, username);
            }
            if (existingRecord == null)
            {
                MiningGroupDAO.AddMiningGroup(record);
                if (multiUserMode)
                {
                    data = MiningGroupDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    data = MiningGroupDAO.GetRecords();
                }
            }

            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, GroupConfig record)
        {
            MiningGroupDAO.DeleteRecord(record);
            if (multiUserMode)
            {
                data = MiningGroupDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                data = MiningGroupDAO.GetRecords();
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, GroupConfig record)
        {
            MiningGroupDAO.UpdateMiningGroup(record);
            if (multiUserMode)
            {
                data = MiningGroupDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                data = MiningGroupDAO.GetRecords();
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

    }
}
