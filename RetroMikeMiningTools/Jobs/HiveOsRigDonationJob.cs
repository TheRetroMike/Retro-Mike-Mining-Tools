using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;
using Quartz;
using RetroMikeMiningTools.DTO;

namespace RetroMikeMiningTools.Jobs
{
    public class HiveOsRigDonationJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Common.Logger.Log("Executing HiveOS Rig Donation Checking Job", LogType.System);
            bool multiUserMode = context.Trigger.JobDataMap.GetBoolean("multi_user_mode");

            List<CoreConfig> configs = new List<CoreConfig>();
            if (multiUserMode)
            {
                configs = CoreConfigDAO.GetCoreConfigs().Where(x => x.Username != null).ToList();
            }
            else
            {
                configs.Add(CoreConfigDAO.GetCoreConfig());
            }
            foreach (var config in configs)
            {
                if (!String.IsNullOrEmpty(config.HiveApiKey) && !String.IsNullOrEmpty(config.HiveFarmID))
                {
                    var donationFlightsheets = DonationDAO.GetRecords();
                    if (donationFlightsheets != null)
                    {
                        foreach (var item in donationFlightsheets)
                        {
                            var workerCount = HiveUtilities.GetFlightsheetWorkerCount(config.HiveApiKey, config.HiveFarmID, item.TrackingID);
                            if (workerCount != null && workerCount == 0)
                            {
                                HiveUtilities.DeleteFlightsheet(config.HiveApiKey, config.HiveFarmID, Convert.ToInt64(item.TrackingID));
                            }
                        }
                    }

                    foreach (var item in HiveUtilities.GetAllFlightsheets(config.HiveApiKey, config.HiveFarmID).Where(x => x.Name.EndsWith("profit_switcher_donation", StringComparison.OrdinalIgnoreCase)))
                    {
                        var workerCount = HiveUtilities.GetFlightsheetWorkerCount(config.HiveApiKey, config.HiveFarmID, Convert.ToString(item.Id));
                        if (workerCount != null && workerCount == 0)
                        {
                            HiveUtilities.DeleteFlightsheet(config.HiveApiKey, config.HiveFarmID, item.Id);
                        }
                    }

                    foreach (var item in HiveRigDAO.GetRecords().Where(x => x.Enabled && x.EnabledDateTime != null))
                    {
                        if (item.EnabledDateTime <= DateTime.Now.AddHours(-1) && !String.IsNullOrEmpty(item.DonationAmount))
                        {
                            var currentFlightsheet = "";
                            var donationAmount = decimal.Parse(item?.DonationAmount?.TrimEnd(new char[] { '%', ' ' })) / 100M;
                            if (donationAmount > 0.00m)
                            {
                                var currentRecord = HiveRigDAO.GetRecord(item.Id);

                                //currentRecord.DonationRunning = false;
                                //currentRecord.DonationStartTime = DateTime.Parse("2022-09-27 20:25");
                                //currentRecord.DonationEndTime = DateTime.Parse("2022-09-27 21:45");
                                //HiveRigDAO.UpdateRecord(currentRecord);

                                currentRecord = HiveRigDAO.GetRecord(item.Id);
                                if (currentRecord.DonationRunning && (DateTime.Now > currentRecord.DonationEndTime || DateTime.Now < currentRecord.DonationStartTime))
                                {
                                    currentRecord.DonationRunning = false;
                                    HiveRigDAO.UpdateRecord(currentRecord);
                                    currentFlightsheet = HiveUtilities.GetCurrentFlightsheet(currentRecord.HiveWorkerId, config.HiveApiKey, config.HiveFarmID, currentRecord.Name);
                                    ProfitSwitching.HiveOsGpuRigProcessor.Process(currentRecord, config);
                                    Thread.Sleep(10000);
                                    if (!String.IsNullOrEmpty(currentFlightsheet))
                                    {
                                        var isDonationFlightsheet = DonationDAO.GetRecords().Where(x => x.TrackingID == currentFlightsheet).Any();
                                        if (isDonationFlightsheet)
                                        {
                                            try
                                            {
                                                HiveUtilities.DeleteFlightsheet(config.HiveApiKey, config.HiveFarmID, Convert.ToInt64(currentFlightsheet));
                                            }
                                            catch
                                            {
                                                Thread.Sleep(5000);
                                                HiveUtilities.DeleteFlightsheet(config.HiveApiKey, config.HiveFarmID, Convert.ToInt64(currentFlightsheet));
                                            }
                                        }
                                    }

                                }

                                currentRecord = HiveRigDAO.GetRecord(item.Id);
                                if (!currentRecord.DonationRunning && (currentRecord.DonationStartTime == null || currentRecord.DonationEndTime == null))
                                {
                                    if (currentRecord != null)
                                    {
                                        var secondsInDay = 86400;
                                        var donationSecondsInDay = secondsInDay * donationAmount;
                                        var donationSecondsPerRun = donationSecondsInDay / 4; //We run donations every 6 hours
                                        var startTime = DateTime.Now;
                                        var endTime = startTime.AddSeconds(Convert.ToDouble(donationSecondsPerRun));
                                        currentRecord.DonationStartTime = startTime;
                                        currentRecord.DonationEndTime = endTime;
                                        HiveRigDAO.UpdateRecord(currentRecord);
                                    }
                                }

                                currentRecord = HiveRigDAO.GetRecord(item.Id);
                                currentFlightsheet = HiveUtilities.GetCurrentFlightsheet(currentRecord.HiveWorkerId, config.HiveApiKey, config.HiveFarmID, item.Name);

                                if (!currentRecord.DonationRunning && DateTime.Now >= currentRecord.DonationStartTime && DateTime.Now <= currentRecord.DonationEndTime)
                                {
                                    if (!String.IsNullOrEmpty(currentFlightsheet))
                                    {
                                        var donationFlightSheetId = HiveUtilities.CreateDonationFlightsheet(currentFlightsheet, config.HiveApiKey, config.HiveFarmID);
                                        if (!String.IsNullOrEmpty(donationFlightSheetId))
                                        {
                                            HiveUtilities.UpdateFlightSheetID(currentRecord.HiveWorkerId, donationFlightSheetId, "", "", config.HiveApiKey, config.HiveFarmID, currentRecord.Name, true, currentRecord.MiningMode, "coins", currentRecord.Username);
                                            DonationDAO.AddDonationEntry(new DTO.DonationHistory()
                                            {
                                                DateTime = DateTime.Now,
                                                TrackingID = donationFlightSheetId
                                            });
                                            currentRecord.DonationRunning = true;
                                            HiveRigDAO.UpdateRecord(currentRecord);
                                        }
                                    }
                                }

                                currentRecord = HiveRigDAO.GetRecord(item.Id);
                                if (currentRecord.DonationRunning && DateTime.Now >= currentRecord.DonationEndTime)
                                {
                                    var secondsInDay = 86400;
                                    var donationSecondsInDay = secondsInDay * donationAmount;
                                    var donationSecondsPerRun = donationSecondsInDay / 4;
                                    var startTime = DateTime.Now.AddHours(6);
                                    var endTime = startTime.AddSeconds(Convert.ToDouble(donationSecondsPerRun));
                                    currentRecord.DonationStartTime = startTime;
                                    currentRecord.DonationEndTime = endTime;
                                    currentRecord.DonationRunning = false;
                                    HiveRigDAO.UpdateRecord(currentRecord);
                                    ProfitSwitching.HiveOsGpuRigProcessor.Process(currentRecord, config);
                                    Thread.Sleep(new TimeSpan(0, 0, 20));

                                    if (!String.IsNullOrEmpty(currentFlightsheet))
                                    {
                                        var isDonationFlightsheet = DonationDAO.GetRecords().Where(x => x.TrackingID == currentFlightsheet).Any();
                                        if (isDonationFlightsheet)
                                        {
                                            try
                                            {
                                                HiveUtilities.DeleteFlightsheet(config.HiveApiKey, config.HiveFarmID, Convert.ToInt64(currentFlightsheet));
                                            }
                                            catch
                                            {
                                                Thread.Sleep(new TimeSpan(0, 0, 30));
                                                HiveUtilities.DeleteFlightsheet(config.HiveApiKey, config.HiveFarmID, Convert.ToInt64(currentFlightsheet));
                                            }
                                        }
                                    }
                                }
                                currentRecord = HiveRigDAO.GetRecord(item.Id);
                                if (currentRecord.DonationEndTime <= DateTime.Now)
                                {
                                    var secondsInDay = 86400;
                                    var donationSecondsInDay = secondsInDay * donationAmount;
                                    var donationSecondsPerRun = donationSecondsInDay / 4;
                                    var startTime = DateTime.Now.AddHours(6);
                                    var endTime = startTime.AddSeconds(Convert.ToDouble(donationSecondsPerRun));
                                    currentRecord.DonationStartTime = startTime;
                                    currentRecord.DonationEndTime = endTime;
                                    currentRecord.DonationRunning = false;
                                    HiveRigDAO.UpdateRecord(currentRecord);
                                }
                            }
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
