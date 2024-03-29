﻿using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Utilities;


namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class IndexModel : PageModel
    {
        public static IList<LogEntry>? data;
        private static IConfiguration systemConfiguration;
        private static IWebHostEnvironment systemEnvironment;
        private static IHostApplicationLifetime appHost;
        private static bool multiUserMode = false;
        private static string? username;
        public bool IsMultiUser { get; set; }


        public IndexModel(IConfiguration configuration, IWebHostEnvironment env, IHostApplicationLifetime hostLifetime)
        {
            systemConfiguration = configuration;
            systemEnvironment = env;
            appHost = hostLifetime;
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
                    IsMultiUser = true;
                }

                var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                if (hostPlatform != null)
                {
                    ViewData["Platform"] = hostPlatform;
                }
            }

            if (multiUserMode && username != "admin")
            {
                data = LogDAO.GetLogs()?.Where(x => (x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))?.ToList();
            }
            else
            {
                data = LogDAO.GetLogs();
            }
        }

        public async Task<JsonResult> OnPostUpgradeCheck()
        {
            if(multiUserMode && username != "admin")
            {
                return new JsonResult("");
            }
            var releaseType = CoreConfigDAO.GetCoreConfig().ReleaseType;
            var updateInfo = new UpdateInfo();

            var assemblyVersion = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version;
            var client = new GitHubClient(new ProductHeaderValue("retro-mike-mining-tools"));
            Release latestRelease = null;
            if (releaseType == Enums.ReleaseType.Development)
            {
                latestRelease = await client.Repository.Release.GetLatest("TheRetroMike", "Retro-Mike-Mining-Tools-Dev");
            }
            else
            {
                latestRelease = await client.Repository.Release.GetLatest("TheRetroMike", "Retro-Mike-Mining-Tools");
            }

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
            if (multiUserMode && username != "admin")
            {
                return new JsonResult("");
            }
            if (!String.IsNullOrEmpty(version))
            {
                var config = CoreConfigDAO.GetCoreConfig();
                config.IgnoredVersion = version;
                CoreConfigDAO.UpdateCoreConfig(config);
            }
            return new JsonResult(String.Empty);
        }

        public async Task<JsonResult> OnPostExecuteUpgrade()
        {
            if (multiUserMode && username != "admin")
            {
                return new JsonResult("");
            }
            var releaseType = CoreConfigDAO.GetCoreConfig().ReleaseType;
            var client = new GitHubClient(new ProductHeaderValue("retro-mike-mining-tools"));
            Release latestRelease = null;
            if (releaseType == Enums.ReleaseType.Development)
            {
                latestRelease = await client.Repository.Release.GetLatest("TheRetroMike", "Retro-Mike-Mining-Tools-Dev");
            }
            else
            {
                latestRelease = await client.Repository.Release.GetLatest("TheRetroMike", "Retro-Mike-Mining-Tools");
            }
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
                        using (System.IO.Compression.ZipArchive zip = System.IO.Compression.ZipFile.OpenRead(localZipFile))
                        {
                            zip.ExtractToDirectory(destinationFolder, true);
                        }
                        System.IO.File.Delete(localZipFile);



                        if (appHost != null)
                        {
                            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                            System.Diagnostics.Process newProcess = new System.Diagnostics.Process();

                            if (systemConfiguration != null)
                            {
                                var serviceName = systemConfiguration.GetValue<string>(Constants.PARAMETER_SERVICE_NAME);
                                var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                                if (
                                        !String.IsNullOrEmpty(serviceName) && 
                                        (
                                            !String.IsNullOrEmpty(hostPlatform) && hostPlatform.Equals(Constants.PLATFORM_HIVE_OS, StringComparison.OrdinalIgnoreCase) ||
                                            !String.IsNullOrEmpty(hostPlatform) && hostPlatform.Equals(Constants.PLATFORM_NICEHASH_OS, StringComparison.OrdinalIgnoreCase) ||
                                            !String.IsNullOrEmpty(hostPlatform) && hostPlatform.Equals(Constants.PLATFORM_LINUX, StringComparison.OrdinalIgnoreCase)
                                        )
                                    )
                                {
                                    newProcess.StartInfo = new System.Diagnostics.ProcessStartInfo()
                                    {
                                        Arguments = String.Format("{0} {1}", Constants.LINUX_RESTART_SERVICE_CMD, serviceName),
                                        FileName = Constants.LINUX_SERVICE_CONTROLLER_CMD
                                    };
                                    newProcess.Start();
                                    return new JsonResult(String.Empty);
                                }
                            }

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

        public JsonResult OnPostClearLogs()
        {
            LogDAO.PurgeLogs(username);
            return new JsonResult("Logs Purged");
        }

            public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, LogEntry record)
        {
            LogDAO.DeleteRecord(record);
            if (multiUserMode && username != "admin")
            {
                data = LogDAO.GetLogs()?.Where(x => (x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))?.ToList();
            }
            else
            {
                data = LogDAO.GetLogs();
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostRefreshLogs()
        {
            var newData = LogDAO.GetLogs();
            if (multiUserMode && username != "admin")
            {
                newData = LogDAO.GetLogs()?.Where(x => (x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))?.ToList();
            }
            if (newData.Count != data?.Count)
            {
                data = newData;
                return new JsonResult("New Data");
            }
            return new JsonResult("");
        }
    }
}