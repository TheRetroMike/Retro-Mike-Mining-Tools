using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class IndexModel : PageModel
    {
        public static IList<LogEntry>? data;
        public void OnGet()
        {
            if (data == null)
            {
                data = LogDAO.GetLogs();
            }

            //UpgradeCheck().Wait();
        }

        private async Task UpgradeCheck()
        {
            var currentVersion = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version.ToString();
            var client = new GitHubClient(new ProductHeaderValue("retro-mike-mining-tools"));
            var latestRelease = await client.Repository.Release.GetLatest("TheRetroMike", "retro-mike-mining-tools");
            var latestReleaseVersion = latestRelease.TagName;
            var releaseNotes = latestRelease.Body;
            var zipFile = latestRelease.ZipballUrl;
            if (currentVersion != latestReleaseVersion)
            {

            }
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, GroupConfig record)
        {
            MiningGroupDAO.DeleteRecord(record);
            data = LogDAO.GetLogs();
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostRefreshLogs()
        {
            var newData = LogDAO.GetLogs();
            if (newData.Count != data?.Count)
            {
                data = newData;
                return new JsonResult("New Data");
            }
            return new JsonResult("");
        }
    }
}