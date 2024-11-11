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
            bool multiUserMode = context.Trigger.JobDataMap.GetBoolean("multi_user_mode");

            var exchanges = ExchangeDAO.GetRecords().Where(x => x.Enabled).ToList();
            if (multiUserMode)
            {
                exchanges = ExchangeDAO.GetRecords().Where(x => x.Username != null && x.Enabled).ToList();
            }
            if (exchanges != null && exchanges.Count > 0)
            {
                foreach (var exchange in exchanges)
                {
                    switch (exchange.Exchange)
                    {
                        //case Exchange.TxBit:
                        //    Common.Logger.Log(String.Format("Executing Auto Exchanging Job for TxBit"), LogType.System, exchange.Username);
                        //    GenericExchange.Process(exchange, Constants.TX_BIT_API_BASE_PATH, Constants.TX_BIT_TRADE_FEE);
                        //    break;
                        case Exchange.TradeOgre:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for Trade Ogre"), LogType.System, exchange.Username);
                            TradeOgre.Process(exchange);
                            break;
                        case Exchange.CoinEx:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for CoinEx"), LogType.System, exchange.Username);
                            CoinExExchange.Process(exchange);
                            break;
                        //case Exchange.SouthXchange:
                        //    Common.Logger.Log(String.Format("Executing Auto Exchanging Job for SouthXchange"), LogType.System, exchange.Username);
                        //    SouthXchange.Process(exchange);
                        //    break;
                        case Exchange.Kucoin:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for Kucoin"), LogType.System, exchange.Username);
                            KucoinExchange.Process(exchange);
                            break;
                        //case Exchange.Graviex:
                        //    Common.Logger.Log(String.Format("Executing Auto Exchanging Job for Graviex"), LogType.System, exchange.Username);
                        //    Graviex.Process(exchange);
                        //    break;
                        case Exchange.Xeggex:
                            Common.Logger.Log(String.Format("Executing Auto Exchanging Job for Xeggex"), LogType.System, exchange.Username);
                            Xeggex.Process(exchange);
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
