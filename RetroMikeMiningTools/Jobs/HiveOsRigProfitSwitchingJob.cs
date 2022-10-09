using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;
using Quartz;
using System.Web;
using RetroMikeMiningTools.ProfitSwitching;
using RetroMikeMiningTools.Common;

namespace RetroMikeMiningTools.Jobs
{
    public class HiveOsRigProfitSwitchingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Common.Logger.Log("Executing HiveOS Rig Profit Switching Job", LogType.System);
            string? platformName = context.Trigger.JobDataMap.GetString("platform_name") ?? String.Empty;
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
                if (config != null)
                {
                    config.UiRigPriceCalculation = false;
                    if (String.IsNullOrEmpty(config.HiveApiKey) || String.IsNullOrEmpty(config.HiveFarmID))
                    {
                        Common.Logger.Log("Skipping Hive OS Rig Profit Switching because there is no Hive API Key and/or Farm ID configured", LogType.System);
                    }
                    else
                    {
                        var rigs = HiveRigDAO.GetRecords(config, true, config.Username).Where(x => x.Enabled);
                        if (rigs != null && rigs.Count() > 0)
                        {
                            foreach (var rig in rigs.Where(x => x.Enabled))
                            {
                                if (!rig.DonationRunning && (
                                    rig.MiningMode == MiningMode.Profit ||
                                    rig.MiningMode == MiningMode.CoinStacking ||
                                    rig.MiningMode == MiningMode.DiversificationByProfit ||
                                    rig.MiningMode == MiningMode.DiversificationByStacking
                                    )
                                )
                                {
                                    HiveOsGpuRigProcessor.Process(rig, config);
                                }
                                else if (!rig.DonationRunning && rig.MiningMode == MiningMode.ZergPoolAlgoProfitBasis)
                                {
                                    ZergAlgoProcessor.Process(rig, config);
                                }
                            }
                        }
                    }

                    if (!multiUserMode && !platformName.Equals(Constants.PLATFORM_DOCKER_ARM64, StringComparison.OrdinalIgnoreCase))
                    {
                        Common.Logger.Log("Executing Goldshell ASIC Profit Switching Job", LogType.System);
                        GoldshellAsicProcessor.Process(config, platformName);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
