using Quartz;
using RetroMikeMiningTools.AutoExchanging;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.Enums;

namespace RetroMikeMiningTools.Jobs
{
    public class AutoExchangingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var exchanges = ExchangeDAO.GetRecords().Where(x => x.Enabled).ToList();
            if (exchanges != null && exchanges.Count > 0)
            {
                
                foreach (var exchange in exchanges)
                {
                    switch (exchange.Exchange)
                    {
                        case Exchange.TxBit:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for TxBit"), LogType.System);
                            GenericExchange.Process(exchange, Constants.TX_BIT_API_BASE_PATH, Constants.TX_BIT_TRADE_FEE);

                            break;
                        case Exchange.TradeOgre:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for Trade Ogre"), LogType.System);
                            TradeOgre.Process(exchange);
                            break;
                        case Exchange.CoinEx:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for CoinEx"), LogType.System);
                            CoinExExchange.Process(exchange);
                            break;
                        case Exchange.SouthXchange:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for SouthXchange"), LogType.System);
                            SouthXchange.Process(exchange);
                            break;
                        case Exchange.Kucoin:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for Kucoin"), LogType.System);
                            KucoinExchange.Process(exchange);
                            break;
                        default:
                            break;
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
