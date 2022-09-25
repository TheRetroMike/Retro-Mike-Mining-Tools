using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class AutoExchangingModel : PageModel
    {
        public static IList<ExchangeConfig>? data;
        private static IConfiguration systemConfiguration;

        public AutoExchangingModel(IConfiguration configuration)
        {
            systemConfiguration = configuration;
        }

        public void OnGet()
        {
            if (data == null)
            {
                data = ExchangeDAO.GetRecords();
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
        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, ExchangeConfig record)
        {
            ExchangeDAO.AddRecord(record);
            data = ExchangeDAO.GetRecords();
            
            return new JsonResult(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, ExchangeConfig record)
        {
            ExchangeDAO.DeleteRecord(record);
            data = ExchangeDAO.GetRecords();
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, ExchangeConfig record)
        {
            ExchangeDAO.UpdateRecord(record);
            data = ExchangeDAO.GetRecords();
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnGetMasterCoinList([DataSourceRequest] DataSourceRequest request, int exchangeId)
        {
            return new JsonResult(Exchanges.ExchangeCoins.Where(x => x.Exchange == (Enums.Exchange)exchangeId));
        }
    }
}
