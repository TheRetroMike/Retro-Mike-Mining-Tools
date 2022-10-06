using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Jobs;
using Quartz;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Common;
using Microsoft.AspNetCore.Authentication.Cookies;

if (!Directory.Exists("db"))
{
    Directory.CreateDirectory("db");
    if (File.Exists("retromikeminingtools.db"))
    {
        File.Copy("retromikeminingtools.db", RetroMikeMiningTools.Common.Constants.DB_FILE);
    }
}


CancellationTokenSource cancelTokenSource = new System.Threading.CancellationTokenSource();

CoreConfigDAO.InitialConfiguration();
LogDAO.InitialConfiguration();

try
{
    RetroMikeMiningTools.Utilities.MiningDutchUtilities.RefreshMiningDutchData();
    RetroMikeMiningTools.Utilities.ZergUtilities.RefreshZergStatusData();
    RetroMikeMiningTools.Utilities.ZergUtilities.RefreshZergCurrencyData();
    RetroMikeMiningTools.Utilities.ZergUtilities.RefreshZergCoinData();
    RetroMikeMiningTools.Utilities.ProhashingUtilities.RefreshProhashingData();
}
catch { }

var coreConfig = CoreConfigDAO.GetCoreConfig();

var builder = WebApplication.CreateBuilder(args);
var overridePort = builder.Configuration.GetValue<string>(Constants.OVERRIDE_PORT);
if (!String.IsNullOrEmpty(overridePort))
{
    coreConfig.Port = Convert.ToInt32(overridePort);
    CoreConfigDAO.UpdateCoreConfig(coreConfig);
}
if (coreConfig.Port == 0)
{
    coreConfig.Port = 7000;
    CoreConfigDAO.UpdateCoreConfig(coreConfig);
}

bool multiUserMode = false;
var multiUserModeConfig = builder.Configuration.GetValue<string>(Constants.MULTI_USER_MODE);
if (!String.IsNullOrEmpty(multiUserModeConfig) && multiUserModeConfig == "true")
{
    multiUserMode = true;
    coreConfig.ProfitSwitchingEnabled = true;
    coreConfig.AutoExchangingEnabled = true;
    coreConfig.ProfitSwitchingCronSchedule = "0 0/15 * 1/1 * ? *";
    coreConfig.AutoExchangingCronSchedule = "0 0/15 * 1/1 * ? *";
    CoreConfigDAO.UpdateCoreConfig(coreConfig);
}

builder.WebHost.UseUrls(String.Format("http://0.0.0.0:{0}", Convert.ToString(coreConfig.Port)));
var razorBuilder = builder.Services.AddRazorPages();

if (multiUserMode)
{
    builder.Services.Configure<CookiePolicyOptions>(x =>
    {
        x.CheckConsentNeeded = context => true;
        x.MinimumSameSitePolicy = SameSiteMode.None;
    });
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
    {
        x.LoginPath = "/Login";
    });
    razorBuilder.AddRazorPagesOptions(x =>
     {
         x.Conventions.AuthorizePage("/Index");
         x.Conventions.AuthorizePage("/AutoExchanging");
         x.Conventions.AuthorizePage("/Core");
         x.Conventions.AuthorizePage("/HiveOsRigs");
         x.Conventions.AuthorizePage("/MiningGroupings");
     });
}
razorBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
builder.Services.AddKendo();
builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "RetroMikeMiningToolsScheduler";
    q.SchedulerName = "Retro Mike Mining Tools Scheduler";
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
    q.UseDefaultThreadPool(tp =>
    {
        tp.MaxConcurrency = 10;
    });

    q.ScheduleJob<RefreshApiDataJob>(trigger => trigger
        .WithIdentity("Refresh API Data Trigger")
        .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
        .WithCronSchedule("30 3/15 * 1/1 * ? *")
        .WithDescription("Refresh API Data Trigger")
    );

    if (coreConfig.ProfitSwitchingEnabled && !String.IsNullOrEmpty(coreConfig.ProfitSwitchingCronSchedule))
    {
        //HiveOS ProfitSwitching
        q.ScheduleJob<HiveOsRigProfitSwitchingJob>(trigger => trigger
            .WithIdentity("Profit Switching Trigger")
            .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
            .WithCronSchedule(coreConfig?.ProfitSwitchingCronSchedule ?? "0 0/1 * 1/1 * ? *")
            .WithDescription("Profit Switching Trigger")
            .UsingJobData("platform_name", builder.Configuration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME))
            .UsingJobData("multi_user_mode", multiUserMode)
        );

        //Donation Job
        q.ScheduleJob<HiveOsRigDonationJob>(trigger => trigger
                .WithIdentity("Donation Trigger")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
                .WithCronSchedule("30 0/1 * 1/1 * ? *")
                .WithDescription("Donation Trigger")
                .UsingJobData("multi_user_mode", multiUserMode)
            );
    }

    if (coreConfig.AutoExchangingEnabled && !String.IsNullOrEmpty(coreConfig.AutoExchangingCronSchedule))
    {
        //Auto Exchanging
        q.ScheduleJob<AutoExchangingJob>(trigger => trigger
            .WithIdentity("Auto Exchanging Trigger")
            .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
            .WithCronSchedule(coreConfig?.AutoExchangingCronSchedule ?? "0 0/1 * 1/1 * ? *")
            .WithDescription("Auto Exchanging Trigger")
            .UsingJobData("multi_user_mode", multiUserMode)
        );
    }
});

builder.Services.AddQuartzServer(options =>
{
    options.AwaitApplicationStarted = true;
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
if (multiUserMode)
{
    app.UseAuthentication();
}

app.UseAuthorization();
app.MapRazorPages();
app.Lifetime.ApplicationStarted.Register(() => { RetroMikeMiningTools.Common.Logger.Log("Application Started", LogType.System); RetroMikeMiningTools.Utilities.AnalyticUtilities.Startup(); });
app.Lifetime.ApplicationStopped.Register(() => { RetroMikeMiningTools.Common.Logger.Log("Application Shutdown", LogType.System); });
app.Run();
