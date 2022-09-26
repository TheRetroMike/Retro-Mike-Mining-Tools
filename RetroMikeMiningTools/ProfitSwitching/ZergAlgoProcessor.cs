using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.ProfitSwitching
{
    public class ZergAlgoProcessor
    {
        public static void Process(HiveOsRigConfig rig, CoreConfig config)
        {
            string currentCoin = String.Empty;
            decimal threshold = 0.00m;
            if (!String.IsNullOrEmpty(config.CoinDifferenceThreshold))
            {
                threshold = decimal.Parse(config.CoinDifferenceThreshold.TrimEnd(new char[] { '%', ' ' })) / 100M;
            }
            var btcPrice = CoinDeskUtilities.GetBtcPrice();
            Common.Logger.Log(String.Format("Executing Zergo Algo Profit Switching Job for Rig: {0}", rig.Name), LogType.System);
            string currentFlightsheet = HiveUtilities.GetCurrentFlightsheet(rig.HiveWorkerId, config.HiveApiKey, config.HiveFarmID, rig.Name);
            var configuredCoins = ZergAlgoDAO.GetRecords(rig.Id, HiveUtilities.GetAllFlightsheets(config.HiveApiKey, config.HiveFarmID)).Where(x => x.Enabled);
            if (!String.IsNullOrEmpty(currentFlightsheet))
            {
                currentCoin = configuredCoins.Where(x => x?.Flightsheet?.ToString() == currentFlightsheet).FirstOrDefault()?.Algo;
            }
            if (configuredCoins != null && configuredCoins.Count() > 0)
            {
                var zergAlgos = ZergUtilities.GetZergAlgoData().Where(x => configuredCoins.Any(y => y.Algo.Equals(x.Algo, StringComparison.OrdinalIgnoreCase)));
                List<StagedCoin> stagedCoins = new List<StagedCoin>();

                foreach (var item in zergAlgos)
                {
                    var configuredCoin = configuredCoins.Where(x => x.Algo.Equals(item.Algo)).FirstOrDefault();
                    if (configuredCoin != null)
                    {
                        bool skipCoin = false;
                        double powerCostOverride = Convert.ToDouble(config.DefaultPowerPrice);
                        if (configuredCoin.Groups != null)
                        {
                            foreach (var grouping in configuredCoin.Groups)
                            {
                                var groupingRecord = MiningGroupDAO.GetRecord(grouping.Id);
                                if (groupingRecord != null)
                                {
                                    if (groupingRecord.StartTime != null && groupingRecord.EndTime != null && Convert.ToDateTime(groupingRecord.StartTime) >= DateTime.Now && Convert.ToDateTime(groupingRecord.EndTime) <= DateTime.Now)
                                    {
                                        skipCoin = true;
                                    }
                                    else if (groupingRecord.PowerCost != null)
                                    {
                                        powerCostOverride = Convert.ToDouble(groupingRecord.PowerCost);
                                    }
                                }
                            }
                        }
                        if (skipCoin)
                        {
                            continue;
                        }

                        var mBtcPerMhAmount = Convert.ToDecimal(item.Estimate) / (Convert.ToDecimal(item.MhFactor) / 1000);
                        var mBtcRevenue = configuredCoin.HashRateMH * mBtcPerMhAmount;
                        var btcRevenue = mBtcRevenue / 1000;

                        decimal dailyPowerCost = 24 * (Convert.ToDecimal(configuredCoin.Power) / 1000m) * Convert.ToDecimal(powerCostOverride);
                        decimal dailyRevenue = Convert.ToDecimal(btcRevenue) * Convert.ToDecimal(btcPrice);
                        decimal dailyProfit = dailyRevenue - dailyPowerCost;

                        stagedCoins.Add(new StagedCoin() { Algorithm = item.Algo, Amount = Convert.ToDouble(dailyProfit) });
                    }
                }

                if (stagedCoins.Count > 0)
                {
                    var currentCoinPrice = stagedCoins.Where(x => x.Algorithm.Equals(currentCoin, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Amount;
                    var newCoinBestPrice = stagedCoins.Max(x => x.Amount);
                    var newTopCoinTicker = stagedCoins.Aggregate((x, y) => x.Amount > y.Amount ? x : y).Algorithm;

                    if (!String.IsNullOrEmpty(rig.PinnedZergAlgo))
                    {
                        newTopCoinTicker = rig.PinnedZergAlgo;
                        var newFlightsheet = configuredCoins.Where(x => x.Algo.Equals(newTopCoinTicker, StringComparison.OrdinalIgnoreCase) && x.Enabled).FirstOrDefault();
                        if (newFlightsheet != null && newFlightsheet.Flightsheet != null && newFlightsheet.Flightsheet.ToString() != currentFlightsheet)
                        {
                            var pinnedProfit = stagedCoins.Where(x => x.Algorithm.Equals(newTopCoinTicker)).FirstOrDefault();
                            if (pinnedProfit != null)
                            {
                                newCoinBestPrice = pinnedProfit.Amount;
                            }
                            else
                            {
                                newCoinBestPrice = 0.00;
                            }
                            HiveUtilities.UpdateFlightSheetID(rig.HiveWorkerId, newFlightsheet.Flightsheet.ToString(), newFlightsheet.FlightsheetName, newCoinBestPrice.ToString(), config.HiveApiKey, config.HiveFarmID, rig.Name, false, rig.MiningMode, newTopCoinTicker);
                        }
                    }
                    else
                    {
                        var newPriceMin = (currentCoinPrice * Convert.ToDouble(threshold)) + currentCoinPrice;
                        if (currentCoinPrice < 0.00)
                        {
                            newPriceMin = -(currentCoinPrice * Convert.ToDouble(threshold)) + currentCoinPrice;
                        }
                        if (newPriceMin == null || newCoinBestPrice > newPriceMin)
                        {
                            var newFlightsheet = configuredCoins.Where(x => x.Algo.Equals(newTopCoinTicker, StringComparison.OrdinalIgnoreCase) && x.Enabled).FirstOrDefault();

                            if (newFlightsheet != null && newFlightsheet.Flightsheet != null && newFlightsheet.Flightsheet.ToString() != currentFlightsheet)
                            {
                                HiveUtilities.UpdateFlightSheetID(rig.HiveWorkerId, newFlightsheet.Flightsheet.ToString(), newFlightsheet.FlightsheetName, newCoinBestPrice.ToString(), config.HiveApiKey, config.HiveFarmID, rig.Name, false, rig.MiningMode, newTopCoinTicker);
                            }
                        }
                    }
                }
            }
        }
    }
}
