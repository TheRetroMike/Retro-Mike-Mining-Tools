using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Pages
{
    public class CoreModel : PageModel
    {
        [BindProperty]
        public CoreConfig? Settings { get; set; }
        public string? Platform { get; set; }
        public bool PortReadOnly = false;
        
        IHostApplicationLifetime appLifetime;
        private static IConfiguration systemConfiguration;

        public CoreModel(IHostApplicationLifetime hostLifetime, IConfiguration configuration)
        {
            appLifetime = hostLifetime;
            systemConfiguration = configuration;
        }

        
        public void OnGet()
        {
            if (Settings == null)
            {
                Settings = new CoreConfig();
            }
            var currentSettings = CoreConfigDAO.GetCoreConfig();
            if (currentSettings != null)
            {
                Settings = currentSettings;
            }
            if (Platform==null)
            {
                Platform = "General";
            }

            if (systemConfiguration != null)
            {
                var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                if (hostPlatform != null)
                {
                    Platform = hostPlatform;
                    if (Platform.Equals(Constants.PLATFORM_DOCKER, StringComparison.OrdinalIgnoreCase))
                    {
                        PortReadOnly = true;
                    }
                }
            }
        }

        private void RestartServices()
        {
            if (appLifetime != null)
            {
                System.Diagnostics.Process newProcess = new System.Diagnostics.Process();

                if (systemConfiguration != null)
                {
                    var serviceName = systemConfiguration.GetValue<string>(Constants.PARAMETER_SERVICE_NAME);
                    var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                    if (!String.IsNullOrEmpty(serviceName) && !String.IsNullOrEmpty(hostPlatform) && hostPlatform.Equals(Constants.PLATFORM_HIVE_OS, StringComparison.OrdinalIgnoreCase))
                    {
                        newProcess.StartInfo = new System.Diagnostics.ProcessStartInfo()
                        {
                            Arguments = String.Format("{0} {1}", Constants.LINUX_RESTART_SERVICE_CMD, serviceName),
                            FileName = Constants.LINUX_SERVICE_CONTROLLER_CMD
                        };
                        newProcess.Start();
                        return;
                    }
                }
                
                newProcess.StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    WorkingDirectory = Environment.CurrentDirectory,
                    Arguments = Environment.CommandLine,
                    FileName = Environment.ProcessPath
                };
                appLifetime.StopApplication();
                newProcess.Start();
            }
        }

        public void OnPost()
        {
            if (Settings != null)
            {
                
                var oldSettings = CoreConfigDAO.GetCoreConfig();
                CoreConfigDAO.UpdateCoreConfig(Settings);
                ViewData["SettingsSaveMessage"] = "Settings Saved";
                if (
                    oldSettings?.ProfitSwitchingCronSchedule != Settings?.ProfitSwitchingCronSchedule || 
                    oldSettings?.ProfitSwitchingEnabled != Settings.ProfitSwitchingEnabled ||
                    oldSettings?.Port != Settings.Port
                    )
                {
                    RestartServices();
                    ViewData["SettingsSaveMessage"] = "Settings Saved and Services Restarted";
                    ViewData["NewUrl"] = String.Format("{0}://{1}:{2}", Request.Scheme, Request.Host.Host, Settings.Port);
                }
            }
        }

        public JsonResult OnPostHiveKey()
        {
            var currentSettings = CoreConfigDAO.GetCoreConfig();
            var result = currentSettings.HiveApiKey;
            return new JsonResult(result);
        }

        public JsonResult OnPostRestart()
        {
            RestartServices();
            return new JsonResult("");
        }

        public JsonResult OnPostCronCalculator(string cronExpression)
        {
            List<string> result = new List<string>();
            if (!String.IsNullOrEmpty(cronExpression))
            {
                try
                {
                    var cron = new Quartz.CronExpression(cronExpression);
                    if (cron != null)
                    {
                        var checkDt = DateTimeOffset.Now;
                        for (int i = 0; i < 5; i++)
                        {
                            var nextRunTime = cron?.GetNextValidTimeAfter(checkDt);
                            if (nextRunTime != null)
                            {
                                var nextRunTimeLocal = nextRunTime.Value.LocalDateTime;
                                result.Add(nextRunTimeLocal.ToString());
                            }
                            checkDt = nextRunTime.Value.AddSeconds(1);
                        }
                    }
                }
                catch
                {
                    //we don't care about errors since this is realtime calculation
                }
            }
            return new JsonResult(result);
        }
    }
}