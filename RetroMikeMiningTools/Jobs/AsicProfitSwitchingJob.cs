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
    public class AsicProfitSwitchingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Common.Logger.Log("Executing ASIC Profit Switching Job", LogType.System);
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
                    var asics = AsicDAO.GetRecords(config, true, config.Username, true).Where(x => x.Enabled);
                    if (asics != null && asics.Count() > 0)
                    {
                        foreach (var asic in asics.Where(x => x.Enabled))
                        {
                            if (
                                    asic.MiningMode == MiningMode.Profit ||
                                    asic.MiningMode == MiningMode.CoinStacking ||
                                    asic.MiningMode == MiningMode.DiversificationByProfit ||
                                    asic.MiningMode == MiningMode.DiversificationByStacking
                                )
                            {
                                AsicProcessor.Process(asic, config);
                            }
                            else if (asic.MiningMode == MiningMode.ZergPoolAlgoProfitBasis)
                            {
                                //ZergAlgoProcessor.Process(rig, config);
                            }
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
