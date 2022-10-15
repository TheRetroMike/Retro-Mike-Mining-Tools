using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;
using System.Web;

namespace RetroMikeMiningTools.ProfitSwitching
{
    public static class HiveOsGpuRigProcessor
    {
        public static async void Process(HiveOsRigConfig rig, CoreConfig config)
        {
            try
            {
                string currentCoin = String.Empty;
                string currentSecondaryCoin = String.Empty;
                decimal threshold = 0.00m;
                if (!String.IsNullOrEmpty(config.CoinDifferenceThreshold))
                {
                    threshold = decimal.Parse(config.CoinDifferenceThreshold.TrimEnd(new char[] { '%', ' ' })) / 100M;
                }
                var btcPrice = CoinDeskUtilities.GetBtcPrice();
                Common.Logger.Log(String.Format("Executing HiveOS Rig Profit Switching Job for Rig: {0}", rig.Name), LogType.System, rig.Username);
                string currentFlightsheet = HiveUtilities.GetCurrentFlightsheet(rig.HiveWorkerId, config.HiveApiKey, config.HiveFarmID, rig.Name);
                var powerPrice = config.DefaultPowerPrice;
                if (!String.IsNullOrEmpty(rig.WhatToMineEndpoint))
                {
                    powerPrice = Convert.ToDecimal(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(rig.WhatToMineEndpoint)).Query).Get("factor[cost]"));
                }
                var configuredCoins = HiveRigCoinDAO.GetRecords(rig.Id, HiveUtilities.GetAllFlightsheets(config.HiveApiKey, config.HiveFarmID), config, false, true, false).Where(x => x.Enabled);
                if (!String.IsNullOrEmpty(currentFlightsheet))
                {
                    var currentConfigged = configuredCoins.Where(x => x?.Flightsheet?.ToString() == currentFlightsheet).FirstOrDefault();
                    if (currentConfigged != null)
                    {
                        currentCoin = currentConfigged.Ticker;
                        currentSecondaryCoin = currentConfigged.SecondaryTicker;
                    }
                }
                if (configuredCoins != null && configuredCoins.Count() > 0)
                {
                    List<StagedCoin> stagedCoins = new List<StagedCoin>();

                    foreach (var configuredCoin in configuredCoins)
                    {
                        bool skipCoin = false;
                        double powerCostOverride = Convert.ToDouble(powerPrice);
                        foreach (var grouping in configuredCoin.Groups)
                        {
                            var groupingRecord = MiningGroupDAO.GetRecord(grouping.Id);
                            if (groupingRecord != null)
                            {
                                if (groupingRecord.StartTime != null && groupingRecord.EndTime != null && Convert.ToDateTime(groupingRecord.StartTime) >= DateTime.Now && Convert.ToDateTime(groupingRecord.EndTime) <= DateTime.Now)
                                {
                                    skipCoin = true;
                                }
                            }
                        }
                        if (skipCoin)
                        {
                            continue;
                        }
                        string walletId = String.Empty;
                        string walletBalance = String.Empty;
                        var dailyProfit = Convert.ToDouble(configuredCoin.Profit);
                        switch (rig.MiningMode)
                        {
                            case MiningMode.Profit:
                                stagedCoins.Add(new StagedCoin() { Ticker = configuredCoin.Ticker, SecondaryTicker = configuredCoin.SecondaryTicker, Amount = dailyProfit });
                                break;
                            case MiningMode.CoinStacking:
                                stagedCoins.Add(new StagedCoin() { Ticker = configuredCoin.Ticker, Amount = Convert.ToDouble(configuredCoin.CoinRevenue) });
                                break;
                            case MiningMode.DiversificationByProfit:
                                walletId = HiveUtilities.GetFlightsheetWalletID(configuredCoin?.Flightsheet?.ToString(), configuredCoin.Ticker, config.HiveApiKey, config.HiveFarmID);
                                walletBalance = HiveUtilities.GetWalletBalance(walletId, config.HiveApiKey, config.HiveFarmID);
                                if (walletBalance != null && Convert.ToDouble(walletBalance) < Convert.ToDouble(configuredCoin.DesiredBalance ?? 0.00))
                                {
                                    stagedCoins.Add(new StagedCoin() { Ticker = configuredCoin.Ticker, Amount = dailyProfit });
                                }
                                break;
                            case MiningMode.DiversificationByStacking:
                                walletId = HiveUtilities.GetFlightsheetWalletID(configuredCoin?.Flightsheet?.ToString(), configuredCoin.Ticker, config.HiveApiKey, config.HiveFarmID);
                                walletBalance = HiveUtilities.GetWalletBalance(walletId, config.HiveApiKey, config.HiveFarmID);
                                if (walletBalance == null || (walletBalance != null && Convert.ToDouble(walletBalance) < Convert.ToDouble(configuredCoin.DesiredBalance ?? 0.00)))
                                {
                                    stagedCoins.Add(new StagedCoin() { Ticker = configuredCoin.Ticker, Amount = Convert.ToDouble(configuredCoin.CoinRevenue) });
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    double? currentCoinPrice = null;
                    if (currentCoin != null && currentSecondaryCoin != null)
                    {
                        currentCoinPrice = stagedCoins.Where(x => x.Ticker.Equals(currentCoin, StringComparison.OrdinalIgnoreCase) && (x.SecondaryTicker == null || x.SecondaryTicker.Equals(currentSecondaryCoin, StringComparison.OrdinalIgnoreCase))).FirstOrDefault()?.Amount;
                    }
                    else if (currentCoin != null)
                    {
                        currentCoinPrice = stagedCoins.Where(x => x.Ticker.Equals(currentCoin, StringComparison.OrdinalIgnoreCase) && x.SecondaryTicker == null).FirstOrDefault()?.Amount;
                    }

                    var newCoinBestPrice = stagedCoins.Max(x => x.Amount);
                    var newTopCoinTicker = stagedCoins.Aggregate((x, y) => x.Amount > y.Amount ? x : y).Ticker;



                    if (!String.IsNullOrEmpty(rig.PinnedTicker))
                    {
                        newTopCoinTicker = rig.PinnedTicker;
                        var newFlightsheet = configuredCoins.Where(x => x.Ticker.Equals(newTopCoinTicker, StringComparison.OrdinalIgnoreCase) && x.Enabled).FirstOrDefault();
                        if (newFlightsheet != null && newFlightsheet.Flightsheet != null && newFlightsheet.Flightsheet.ToString() != currentFlightsheet)
                        {
                            var pinnedProfit = stagedCoins.Where(x => x.Ticker.Equals(newTopCoinTicker)).FirstOrDefault();
                            if (pinnedProfit != null)
                            {
                                newCoinBestPrice = pinnedProfit.Amount;
                            }
                            else
                            {
                                newCoinBestPrice = 0.00;
                            }
                            HiveUtilities.UpdateFlightSheetID(rig.HiveWorkerId, newFlightsheet.Flightsheet.ToString(), newFlightsheet.FlightsheetName, newCoinBestPrice.ToString(), config.HiveApiKey, config.HiveFarmID, rig.Name, false, rig.MiningMode, newTopCoinTicker, rig.Username);
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
                            var newFlightsheet = configuredCoins.Where(x => x.Ticker.Equals(newTopCoinTicker, StringComparison.OrdinalIgnoreCase) && x.Enabled).FirstOrDefault();

                            if (newFlightsheet != null && newFlightsheet.Flightsheet != null && newFlightsheet.Flightsheet.ToString() != currentFlightsheet)
                            {
                                HiveUtilities.UpdateFlightSheetID(rig.HiveWorkerId, newFlightsheet.Flightsheet.ToString(), newFlightsheet.FlightsheetName, newCoinBestPrice.ToString(), config.HiveApiKey, config.HiveFarmID, rig.Name, false, rig.MiningMode, newTopCoinTicker, rig.Username);
                            }
                        }
                    }


                    if (rig.SmartPlugType != null && rig.RigMinProfit != null && rig.SmartPlugHost != null)
                    {
                        //Check to see if we should power off rig
                        if (newCoinBestPrice < Convert.ToDouble(rig.RigMinProfit))
                        {
                            switch (rig.SmartPlugType)
                            {
                                case SmartPlugType.Kasa:
                                    using (var kasaClient = new Kasa.KasaOutlet(rig.SmartPlugHost))
                                    {
                                        var isAlreadyOn = await kasaClient.System.IsOutletOn();
                                        if (isAlreadyOn)
                                        {
                                            await kasaClient.System.SetOutletOn(false);
                                        }
                                    }
                                    break;
                                case SmartPlugType.None:
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            //Let's make sure the plug is on and if not, then turn it on
                            switch (rig.SmartPlugType)
                            {
                                case SmartPlugType.Kasa:
                                    using (var kasaClient = new Kasa.KasaOutlet(rig.SmartPlugHost))
                                    {
                                        var isAlreadyOn = await kasaClient.System.IsOutletOn();
                                        if (!isAlreadyOn)
                                        {
                                            await kasaClient.System.SetOutletOn(true);
                                        }
                                    }
                                    break;
                                case SmartPlugType.None:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Common.Logger.Log(String.Format("Error While Trying To Run Profit Switching Routine for Rig: {0}", rig.Name), LogType.System, rig.Username);
            }
        }
    }
}
