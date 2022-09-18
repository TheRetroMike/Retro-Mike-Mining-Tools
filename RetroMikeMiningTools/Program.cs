using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Jobs;
using Quartz;

CancellationTokenSource cancelTokenSource = new System.Threading.CancellationTokenSource();

CoreConfigDAO.InitialConfiguration();
LogDAO.InitialConfiguration();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
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

    var coreConfig = CoreConfigDAO.GetCoreConfig();

    if (coreConfig.ProfitSwitchingEnabled && !String.IsNullOrEmpty(coreConfig.ProfitSwitchingCronSchedule))
    {
        q.ScheduleJob<HiveOsRigProfitSwitchingJob>(trigger => trigger
            .WithIdentity("Hive OS Rig Profit Switching Trigger")
            .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
            .WithCronSchedule(coreConfig?.ProfitSwitchingCronSchedule ?? "0 0/1 * 1/1 * ? *")
            .WithDescription("HiveOS Rig Profit Switching Trigger")
        );

        q.ScheduleJob<HiveOsRigDonationJob>(trigger => trigger
                .WithIdentity("Hive OS Rig Donation Trigger")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
                .WithCronSchedule("30 0/1 * 1/1 * ? *")
                .WithDescription("Hive OS Rig Donation Trigger")
            );
    }
    
});

// ASP.NET Core hosting
builder.Services.AddQuartzServer(options =>
{
    options.AwaitApplicationStarted = true;
    // when shutting down we want jobs to complete gracefully
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
app.UseAuthorization();
app.MapRazorPages();
app.Lifetime.ApplicationStarted.Register(() => { RetroMikeMiningTools.Common.Logger.Log("Application Started", LogType.System); });
app.Lifetime.ApplicationStopped.Register(() => { RetroMikeMiningTools.Common.Logger.Log("Application Shutdown", LogType.System); });
app.Run();
