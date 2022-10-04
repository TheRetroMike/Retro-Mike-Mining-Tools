using Quartz;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.Jobs
{
    public class RefreshApiDataJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                MiningDutchUtilities.RefreshMiningDutchData();
                ZergUtilities.RefreshZergStatusData();
                ZergUtilities.RefreshZergCurrencyData();
                ZergUtilities.RefreshZergCoinData();
                ProhashingUtilities.RefreshProhashingData();
            }
            catch (Exception ex)
            {
                Common.Logger.Log("Error while trying to refresh API data: " + ex.Message, Enums.LogType.System);
            }
            return Task.CompletedTask;
        }
    }
}
