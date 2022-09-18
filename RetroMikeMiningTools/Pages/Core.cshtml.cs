using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Pages
{
    public class CoreModel : PageModel
    {
        [BindProperty]
        public CoreConfig? Settings { get; set; }
        
        IHostApplicationLifetime appLifetime;
        public CoreModel(IHostApplicationLifetime hostLifetime)
        {
            appLifetime = hostLifetime;
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
        }

        private void RestartServices()
        {
            if (appLifetime != null)
            {
                System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                System.Diagnostics.Process newProcess = new System.Diagnostics.Process();
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

                if (oldSettings?.ProfitSwitchingCronSchedule != Settings?.ProfitSwitchingCronSchedule)
                {
                    RestartServices();
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