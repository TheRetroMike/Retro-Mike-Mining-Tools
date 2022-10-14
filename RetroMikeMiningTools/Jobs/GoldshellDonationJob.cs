using Quartz;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.Jobs
{
    public class GoldshellDonationJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            bool multiUserMode = context.Trigger.JobDataMap.GetBoolean("multi_user_mode");
            string platformName = context.Trigger.JobDataMap.GetString("platform_name");
            if (multiUserMode)
            {
                return Task.CompletedTask;
            }
            else
            {
                var config = CoreConfigDAO.GetCoreConfig();
                foreach (var item in GoldshellAsicDAO.GetRecords(false).Where(x => x.Enabled))
                {
                    if (item.EnabledDateTime == null)
                    {
                        item.EnabledDateTime = DateTime.Now;
                        GoldshellAsicDAO.UpdateRecord(item);
                    }
                    if (item.EnabledDateTime <= DateTime.Now.AddHours(-1) && !String.IsNullOrEmpty(item.DonationAmount))
                    {
                        var donationAmount = decimal.Parse(item?.DonationAmount?.TrimEnd(new char[] { '%', ' ' })) / 100M;
                        if(donationAmount < 0.01m)
                        {
                            donationAmount = 0.01m;
                        }
                        if (donationAmount > 0.00m)
                        {
                            var currentRecord = GoldshellAsicDAO.GetRecord(item.Id);
                            if (currentRecord.DonationRunning && (DateTime.Now > currentRecord.DonationEndTime || DateTime.Now < currentRecord.DonationStartTime))
                            {
                                currentRecord.DonationRunning = false;
                                GoldshellAsicDAO.UpdateRecord(currentRecord);
                                ProfitSwitching.GoldshellAsicProcessor.ProcessAsic(config, currentRecord, platformName, false);
                            }

                            currentRecord = GoldshellAsicDAO.GetRecord(item.Id);
                            if (!currentRecord.DonationRunning && (currentRecord.DonationStartTime == null || currentRecord.DonationEndTime == null))
                            {
                                var secondsInDay = 86400;
                                var donationSecondsInDay = secondsInDay * donationAmount;
                                var donationSecondsPerRun = donationSecondsInDay; //Now running donations once per day instead of small amounts every 6 hours
                                var startTime = DateTime.Now;
                                var endTime = startTime.AddSeconds(Convert.ToDouble(donationSecondsPerRun));
                                currentRecord.DonationStartTime = startTime;
                                currentRecord.DonationEndTime = endTime;
                                GoldshellAsicDAO.UpdateRecord(currentRecord);
                            }
                            currentRecord = GoldshellAsicDAO.GetRecord(item.Id);
                            var currentPool = GoldshellUtilities.GetCurrentPool(currentRecord);
                            if (!currentRecord.DonationRunning && DateTime.Now >= currentRecord.DonationStartTime && DateTime.Now <= currentRecord.DonationEndTime)
                            {
                                if (currentPool != null)
                                {
                                    ProfitSwitching.GoldshellAsicProcessor.ProcessAsic(config, currentRecord, platformName, true);
                                    DonationDAO.AddDonationEntry(new DTO.DonationHistory()
                                    {
                                        DateTime = DateTime.Now,
                                        TrackingID = Guid.NewGuid().ToString()
                                    });
                                    Common.Logger.Push(String.Format("Donation Started: {0}", item.Name));
                                    currentRecord.DonationRunning = true;
                                    GoldshellAsicDAO.UpdateRecord(currentRecord);
                                }
                            }
                            currentRecord = GoldshellAsicDAO.GetRecord(item.Id);
                            if (currentRecord.DonationRunning && DateTime.Now >= currentRecord.DonationEndTime)
                            {
                                var secondsInDay = 86400;
                                var donationSecondsInDay = secondsInDay * donationAmount;
                                var donationSecondsPerRun = donationSecondsInDay;
                                var startTime = DateTime.Now.AddHours(23);
                                var endTime = startTime.AddSeconds(Convert.ToDouble(donationSecondsPerRun));
                                currentRecord.DonationStartTime = startTime;
                                currentRecord.DonationEndTime = endTime;
                                currentRecord.DonationRunning = false;
                                GoldshellAsicDAO.UpdateRecord(currentRecord);
                                ProfitSwitching.GoldshellAsicProcessor.ProcessAsic(config, currentRecord, platformName, false);
                            }
                            currentRecord = GoldshellAsicDAO.GetRecord(item.Id);
                            if (currentRecord.DonationEndTime <= DateTime.Now)
                            {
                                var secondsInDay = 86400;
                                var donationSecondsInDay = secondsInDay * donationAmount;
                                var donationSecondsPerRun = donationSecondsInDay;
                                var startTime = DateTime.Now.AddHours(23);
                                var endTime = startTime.AddSeconds(Convert.ToDouble(donationSecondsPerRun));
                                currentRecord.DonationStartTime = startTime;
                                currentRecord.DonationEndTime = endTime;
                                currentRecord.DonationRunning = false;
                                GoldshellAsicDAO.UpdateRecord(currentRecord);
                            }
                        }
                    }
                    
                }
            }
            return Task.CompletedTask;
        }
    }
}
