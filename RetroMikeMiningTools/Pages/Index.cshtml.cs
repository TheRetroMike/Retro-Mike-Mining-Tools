using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Utilities;
using System.IO.Compression;
using System.Net;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class IndexModel : PageModel
    {
        public static IList<LogEntry>? data;
        private static IConfiguration systemConfiguration;
        private static IWebHostEnvironment systemEnvironment;
        private static IHostApplicationLifetime appHost;

        public IndexModel(IConfiguration configuration, IWebHostEnvironment env, IHostApplicationLifetime hostLifetime)
        {
            systemConfiguration = configuration;
            systemEnvironment = env;
            appHost = hostLifetime;
        }

        public void OnGet()
        {
            if (data == null)
            {
                data = LogDAO.GetLogs();
            }
        }

        public async Task<JsonResult> OnPostUpgradeCheck()
        {
            
            var updateInfo = new UpdateInfo();

            var assemblyVersion = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version;
            var client = new GitHubClient(new ProductHeaderValue("retro-mike-mining-tools"));
            var latestRelease = await client.Repository.Release.GetLatest("TheRetroMike", "Retro-Mike-Mining-Tools");
            var githubVersion = Version.Parse(latestRelease.TagName.Remove(0, 1));
            if (githubVersion != null && githubVersion > assemblyVersion)
            {
                var config = CoreConfigDAO.GetCoreConfig();
                if (config?.IgnoredVersion != latestRelease.TagName)
                {
                    if (latestRelease.Assets.Any(x => x.Name == "RetroMikeMiningTools.zip"))
                    {
                        updateInfo.Version = latestRelease.TagName;
                        updateInfo.Description = Markdig.Markdown.ToHtml(latestRelease.Body);
                        updateInfo.Name = latestRelease.Name;
                    }
                }
            }
            return new JsonResult(updateInfo);
        }

        public JsonResult OnPostIgnoreUpgrade(string version)
        {
            if (!String.IsNullOrEmpty(version))
            {
                var config = CoreConfigDAO.GetCoreConfig();
                config.IgnoredVersion = version;
                CoreConfigDAO.UpdateCoreConfig(config);
            }
            return new JsonResult(String.Empty);
        }

        public async Task<JsonResult> OnPostExecuteUpgrade(string version)
        {
            var client = new GitHubClient(new ProductHeaderValue("retro-mike-mining-tools"));
            var latestRelease = await client.Repository.Release.GetLatest("TheRetroMike", "Retro-Mike-Mining-Tools");
            if (latestRelease != null)
            {
                var installerAsset = latestRelease?.Assets?.Where(x => x.Name == "RetroMikeMiningTools.zip")?.FirstOrDefault();
                if (installerAsset != null)
                {
                    var installerRemoteZip = installerAsset.BrowserDownloadUrl;
                    if (!String.IsNullOrEmpty(installerRemoteZip) && systemEnvironment != null)
                    {
                        var appFilesDirectory = systemEnvironment.ContentRootPath;
                        string localZipFile = Path.Combine(appFilesDirectory, installerAsset.Name);
                        string destinationFolder = appFilesDirectory;
                        await WebUtilities.DownloadFile(installerRemoteZip, appFilesDirectory, installerAsset.Name);
                        using (ZipArchive zip = ZipFile.OpenRead(localZipFile))
                        {
                            zip.ExtractToDirectory(destinationFolder, true);
                        }
                        System.IO.File.Delete(localZipFile);
                        if (appHost != null)
                        {
                            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                            System.Diagnostics.Process newProcess = new System.Diagnostics.Process();
                            newProcess.StartInfo = new System.Diagnostics.ProcessStartInfo()
                            {
                                WorkingDirectory = Environment.CurrentDirectory,
                                Arguments = Environment.CommandLine,
                                FileName = Environment.ProcessPath
                            };
                            appHost.StopApplication();
                            newProcess.Start();
                        }
                    }
                }
                
            }
            return new JsonResult(String.Empty);
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