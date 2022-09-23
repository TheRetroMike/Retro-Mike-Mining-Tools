using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;
using Quartz;
using System.Web;
using RetroMikeMiningTools.ProfitSwitching;

namespace RetroMikeMiningTools.Jobs
{
    public class HiveOsRigProfitSwitchingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var config = CoreConfigDAO.GetCoreConfig();
            if (config != null && config.ProfitSwitchingEnabled)
            {
                if (String.IsNullOrEmpty(config.HiveApiKey) || String.IsNullOrEmpty(config.HiveFarmID))
                {
                    Common.Logger.Log("Skipping Hive OS Rig Profit Switching because there isno Hive API Key and/or Farm ID configured", LogType.System);
                }
                else
                {
                    Common.Logger.Log("Executing HiveOS Rig Profit Switching Job", LogType.System);
                    var rigs = HiveRigDAO.GetRecords().Where(x => x.Enabled);
                    if (rigs != null && rigs.Count() > 0)
                    {
                        foreach (var rig in rigs.Where(x => x.Enabled && !String.IsNullOrEmpty(x.WhatToMineEndpoint)))
                        {
                            if (!rig.DonationRunning)
                            {
                                HiveOsGpuRigProcessor.Process(rig, config);
                            }                        
                        }
                    }
                }

                Common.Logger.Log("Executing Goldshell ASIC Profit Switching Job", LogType.System);
                GoldshellAsicProcessor.Process(config);
            }

            return Task.CompletedTask;
        }
    }
}
