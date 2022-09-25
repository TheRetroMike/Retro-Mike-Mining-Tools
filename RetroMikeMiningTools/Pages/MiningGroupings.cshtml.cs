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

        public MiningGroupingsModel(IConfiguration configuration)
        {
            systemConfiguration = configuration;
        }

        public void OnGet()
        {
            if (data == null)
            {
                data = MiningGroupDAO.GetRecords();
            }
            if (systemConfiguration != null)
            {
                var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                if (hostPlatform != null)
                {
                    ViewData["Platform"] = hostPlatform;
                }
            }
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, GroupConfig record)
        {
            var existingRecord = MiningGroupDAO.GetRecord(record?.Name);
            if (existingRecord == null)
            {
                MiningGroupDAO.AddMiningGroup(record);
                data = MiningGroupDAO.GetRecords();
            }

            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, GroupConfig record)
        {
            MiningGroupDAO.DeleteRecord(record);
            data = MiningGroupDAO.GetRecords();
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, GroupConfig record)
        {
            MiningGroupDAO.UpdateMiningGroup(record);
            data = MiningGroupDAO.GetRecords();
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

    }
}
