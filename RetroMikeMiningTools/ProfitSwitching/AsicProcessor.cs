﻿using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;
using System.Web;

namespace RetroMikeMiningTools.ProfitSwitching
{
    public static class AsicProcessor
    {
        public static async void Process(AsicConfig asic, CoreConfig config)
        {
            try
            {
                switch (asic.AsicType)
                {
                    case AsicType.IceRiver_PBFarmer:
                        if(String.IsNullOrEmpty(asic.DeviceIP) || String.IsNullOrEmpty(asic.ApiKey))
                        {
                            Common.Logger.Log(String.Format("Device IP Address and API Key needed for processing IceRiver ASIC: {0}", asic.Name), LogType.ProfitSwitching);
                            return;
                        }
                        break;
                    default:
                        break;
                }
                string currentCoin = String.Empty;
                decimal threshold = 0.00m;
                if (!String.IsNullOrEmpty(config.CoinDifferenceThreshold))
                {
                    threshold = decimal.Parse(config.CoinDifferenceThreshold.TrimEnd(new char[] { '%', ' ' })) / 100M;
                }
                var btcPrice = CoinDeskUtilities.GetBtcPrice();
                Common.Logger.Log(String.Format("Executing ASIC Profit Switching Job for ASIC: {0}", asic.Name), LogType.System, asic.Username);
                string currentPool = "";
                string currentPoolUser = "";
                string currentPoolPassword = "";

                switch (asic.AsicType)
                {
                    case AsicType.IceRiver_PBFarmer:
                        //TODO: Get current connected pool info
                        var iceRiverPool = IceRiverUtilities.GetCurrentPool(asic.DeviceIP, asic.ApiKey);
                        if (iceRiverPool != null)
                        {
                            currentPool = iceRiverPool.Pool;
                            currentPoolUser = iceRiverPool.PoolUser;
                            currentPoolPassword = iceRiverPool.PoolPassword;
                        }
                        break;
                    default:
                        break;
                }

                //string currentFlightsheet = HiveUtilities.GetCurrentFlightsheet(rig.HiveWorkerId, config.HiveApiKey, config.HiveFarmID, rig.Name);
                var powerPrice = config.DefaultPowerPrice;
                var configuredCoins = AsicCoinDAO.GetRecords(asic.Id, config, false, true, false).Where(x => x.Enabled);
                if (!String.IsNullOrEmpty(currentPool))
                {
                    var currentConfigged = configuredCoins.Where(x => x.Pool==currentPool && x.PoolUser==currentPoolUser && x.PoolPassword==currentPoolPassword).FirstOrDefault();
                    if (currentConfigged != null)
                    {
                        currentCoin = currentConfigged.Ticker;
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
                        switch (asic.MiningMode)
                        {
                            case MiningMode.Profit:
                                stagedCoins.Add(new StagedCoin() { Ticker = configuredCoin.Ticker, Amount = dailyProfit });
                                break;
                            case MiningMode.CoinStacking:
                                stagedCoins.Add(new StagedCoin() { Ticker = configuredCoin.Ticker, Amount = Convert.ToDouble(configuredCoin.CoinRevenue) });
                                break;
                            default:
                                break;
                        }
                    }
                    double? currentCoinPrice = null;
                    if (currentCoin != null)
                    {
                        currentCoinPrice = stagedCoins.Where(x => x.Ticker.Equals(currentCoin, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Amount;
                    }

                    var newCoinBestPrice = stagedCoins.Max(x => x.Amount);
                    var newTopCoinTicker = stagedCoins.Aggregate((x, y) => x.Amount > y.Amount ? x : y).Ticker;



                    if (!String.IsNullOrEmpty(asic.PinnedTicker))
                    {
                        newTopCoinTicker = asic.PinnedTicker;
                        var newPool = configuredCoins.Where(x => x.Ticker.Equals(newTopCoinTicker, StringComparison.OrdinalIgnoreCase) && x.Enabled).FirstOrDefault();
                        if (newPool != null && newPool.Pool != currentPool)
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
                            switch (asic.AsicType)
                            {
                                case AsicType.IceRiver_PBFarmer:
                                    IceRiverUtilities.ReplacePools(asic.DeviceIP, asic.ApiKey, newPool);
                                    Common.Logger.Log(String.Format("Profit Switched Pool on ASIC {0} to {1}", asic.Name, newPool.Ticker), LogType.System, asic.Username);
                                    break;
                                default:
                                    break;
                            }
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
                            var newPool = configuredCoins.Where(x => x.Ticker.Equals(newTopCoinTicker, StringComparison.OrdinalIgnoreCase) && x.Enabled).FirstOrDefault();

                            if (newPool != null && newPool.Pool != currentPool)
                            {
                                switch (asic.AsicType)
                                {
                                    case AsicType.IceRiver_PBFarmer:
                                        IceRiverUtilities.ReplacePools(asic.DeviceIP, asic.ApiKey, newPool);
                                        Common.Logger.Log(String.Format("Profit Switched Pool on ASIC {0} to {1}", asic.Name, newPool.Ticker), LogType.System, asic.Username);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }


                    if (asic.SmartPlugType != SmartPlugType.None && asic.RigMinProfit != null && asic.SmartPlugHost != null)
                    {
                        //Check to see if we should power off rig
                        if (newCoinBestPrice < Convert.ToDouble(asic.RigMinProfit))
                        {
                            switch (asic.SmartPlugType)
                            {
                                case SmartPlugType.Kasa:
                                    using (var kasaClient = new Kasa.KasaOutlet(asic.SmartPlugHost))
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
                            switch (asic.SmartPlugType)
                            {
                                case SmartPlugType.Kasa:
                                    using (var kasaClient = new Kasa.KasaOutlet(asic.SmartPlugHost))
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
                Common.Logger.Log(String.Format("Error While Trying To Run Profit Switching Routine for ASIC: {0}", asic.Name), LogType.System, asic.Username);
            }
        }
    }
}
