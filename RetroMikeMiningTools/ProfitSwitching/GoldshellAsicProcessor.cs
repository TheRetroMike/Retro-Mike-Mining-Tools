﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Enums;
using RetroMikeMiningTools.Utilities;
using System.Web;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace RetroMikeMiningTools.ProfitSwitching
{
    public static class GoldshellAsicProcessor
    {
        public static void Process(CoreConfig config, string? platformName)
        {
            if (platformName==null)
            {
                platformName = String.Empty;
            }
            var asics = DAO.GoldshellAsicDAO.GetRecords(false).Where(x => x.Enabled && !String.IsNullOrEmpty(x.ApiBasePath) && !String.IsNullOrEmpty(x.ApiPassword)).ToList();
            if (asics != null && asics.Count > 0)
            {
                foreach (var asic in asics)
                {
                    ProcessAsic(config, asic, platformName, false);
                }
            }
        }

        public static void ProcessAsic(CoreConfig config, GoldshellAsicConfig asic, string? platformName, bool donationRun)
        {
            Common.Logger.Log(String.Format("Executing Goldshell ASIC Profit Switching Job for Device: {0}", asic.Name), Enums.LogType.System);
            decimal threshold = 0.00m;
            if (!String.IsNullOrEmpty(config.CoinDifferenceThreshold))
            {
                threshold = decimal.Parse(config.CoinDifferenceThreshold.TrimEnd(new char[] { '%', ' ' })) / 100M;
            }
            var powerPrice = config.DefaultPowerPrice;
            if (!String.IsNullOrEmpty(asic.WhatToMineEndpoint))
            {
                powerPrice = Convert.ToDecimal(HttpUtility.ParseQueryString(new Uri(HttpUtility.UrlDecode(asic.WhatToMineEndpoint)).Query).Get("factor[cost]"));
            }

            var configuredCoins = GoldshellAsicCoinDAO.GetRecords(asic.Id, false, true).Where(x => x.Enabled);
            var currentPool = GoldshellUtilities.GetCurrentPool(asic);
            var coin = configuredCoins?.Where(x => x.PoolUrl.Equals(currentPool?.Pool, StringComparison.OrdinalIgnoreCase) && x.PoolUser.Equals(currentPool?.PoolUser, StringComparison.OrdinalIgnoreCase) && x.PoolPassword.Equals(currentPool?.PoolPassword, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault();
            var currentCoin = coin?.Ticker;
            var currentSecondaryCoin = coin?.SecondaryTicker;
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
                            stagedCoins.Add(new StagedCoin() { Ticker = configuredCoin.Ticker, SecondaryTicker = configuredCoin.SecondaryTicker, Amount = dailyProfit });
                            break;
                        case MiningMode.CoinStacking:
                            stagedCoins.Add(new StagedCoin() { Ticker = configuredCoin.Ticker, Amount = Convert.ToDouble(configuredCoin.CoinRevenue) });
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
                var newTopCoinSecondaryTicker = stagedCoins.Aggregate((x, y) => x.Amount > y.Amount ? x : y).SecondaryTicker;

                var bestCoin = new GoldshellAsicCoinConfig();
                if (newTopCoinSecondaryTicker != null)
                {
                    bestCoin = GoldshellAsicCoinDAO.GetRecords(asic.Id, false, true).Where(x => x.Ticker.Equals(newTopCoinTicker, StringComparison.OrdinalIgnoreCase) && x.SecondaryTicker.Equals(newTopCoinSecondaryTicker, StringComparison.OrdinalIgnoreCase) && x.Enabled).FirstOrDefault();
                }
                else if (newTopCoinTicker != null)
                {
                    bestCoin = GoldshellAsicCoinDAO.GetRecords(asic.Id, false, true).Where(x => x.Ticker.Equals(newTopCoinTicker, StringComparison.OrdinalIgnoreCase) && x.Enabled).FirstOrDefault();
                }

                if(donationRun)
                {
                    var bestCoinTicker = bestCoin.Ticker;
                    var bestCoinSecondaryTicker = bestCoin.SecondaryTicker;
                    var donationData = Constants.DONATION_FLIGHTSHEET_DATA.Where(x => x.Ticker.Equals(bestCoinTicker, StringComparison.OrdinalIgnoreCase) || x.Ticker.Equals(bestCoinSecondaryTicker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (donationData != null)
                    {
                        bestCoin.PoolUrl = donationData.AsicPool;
                        bestCoin.PoolUser = donationData.AsicWallet;
                        bestCoin.PoolPassword = donationData.AsicPassword;
                    }
                }

                var newPriceMin = (currentCoinPrice * Convert.ToDouble(threshold)) + currentCoinPrice;
                if (currentCoinPrice < 0.00)
                {
                    newPriceMin = -(currentCoinPrice * Convert.ToDouble(threshold)) + currentCoinPrice;
                }
                if (newPriceMin == null || newCoinBestPrice > newPriceMin || donationRun)
                {
                    new DriverManager().SetUpDriver(new ChromeConfig() { });
                    var options = new ChromeOptions();

                    if (!String.IsNullOrEmpty(platformName) && platformName.Equals(Constants.PLATFORM_RPI))
                    {
                        options.BinaryLocation = "chromium-browswer";
                    }
                    else if (
                        !String.IsNullOrEmpty(platformName) &&
                        platformName.Equals(Constants.PLATFORM_DOCKER_AMD64) ||
                        platformName.Equals(Constants.PLATFORM_DOCKER_AMD64_V2) ||
                        platformName.Equals(Constants.PLATFORM_DOCKER_AMD64_V3) ||
                        platformName.Equals(Constants.PLATFORM_DOCKER_ARM64)
                        )
                    {
                        options.BinaryLocation = "/usr/bin/chromium";
                    }
                    options.AddArgument("--headless");
                    options.AddArgument("--window-size=1920,1080");
                    options.AddArgument("--disable-extensions");
                    options.AddArgument("--no-sandbox");
                    options.AddArgument("--disable-gpu");
                    options.AddArgument("--start-maximized");
                    options.AddArgument("--disable-dev-shm-usage");
                    //options.BinaryLocation = "chromium-browswer";
                    var driver = new ChromeDriver(options);
                    driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 30);
                    driver.Navigate().GoToUrl(String.Format("{0}/#/login", asic.ApiBasePath));
                    Thread.Sleep(1000);
                    try
                    {
                        var passwordField = driver.FindElement(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-application--wrap']/main[@class='v-main grey lighten-4']/div[@class='v-main__wrap']/div[@class='login px-4 my-5']/div[@class='row']/div[@class='col-sm-6 col-md-6 col-lg-6 col-xl-6 col-12']/div[@class='v-card v-sheet v-sheet--outlined theme--light']/div[@class='v-card__text']/div[@class='v-input theme--light v-text-field v-text-field--is-booted v-text-field--placeholder']/div[@class='v-input__control']/div[@class='v-input__slot']/div[@class='v-text-field__slot']/input"));
                        if (passwordField != null)
                        {
                            passwordField.SendKeys(asic.ApiPassword);
                            var loginButton = driver.FindElement(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-application--wrap']/main[@class='v-main grey lighten-4']/div[@class='v-main__wrap']/div[@class='login px-4 my-5']/div[@class='row']/div[@class='col-sm-6 col-md-6 col-lg-6 col-xl-6 col-12']/div[@class='v-card v-sheet v-sheet--outlined theme--light']/div[@class='v-card__actions']/button[@class='v-btn v-btn--text theme--light v-size--default primary--text']/span[@class='v-btn__content']"));
                            loginButton.Click();
                            Thread.Sleep(1000);
                            driver.Navigate().GoToUrl(String.Format("{0}/#/config", asic.ApiBasePath));
                            Thread.Sleep(1000);
                            try
                            {
                                var deleteButtons = driver.FindElements(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-application--wrap']/main[@class='v-main grey lighten-4']/div[@class='v-main__wrap']/div[@class='miner-setting px-4 my-5']/div[@class='row no-gutters justify-center']/div[@class='col col-12']/div[@class='v-card v-sheet v-sheet--outlined theme--light']/div[@class='v-card__text']/div/span/div[@class='v-list-item theme--light']/button[@class='v-btn v-btn--outlined theme--light v-size--small error--text']/span[@class='v-btn__content']"));
                                if (deleteButtons != null)
                                {
                                    for (int i = 0; i < deleteButtons.Count(); i++)
                                    {
                                        var deleteButton = driver.FindElement(By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-application--wrap']/main[@class='v-main grey lighten-4']/div[@class='v-main__wrap']/div[@class='miner-setting px-4 my-5']/div[@class='row no-gutters justify-center']/div[@class='col col-12']/div[@class='v-card v-sheet v-sheet--outlined theme--light']/div[@class='v-card__text']/div/span/div[@class='v-list-item theme--light'][1]/button[@class='v-btn v-btn--outlined theme--light v-size--small error--text']/span[@class='v-btn__content']"));
                                        deleteButton.Click();
                                        Thread.Sleep(1000);
                                        var popupConfirmation = driver.FindElement(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-dialog__content v-dialog__content--active']/div[@class='v-dialog v-dialog--active v-dialog--persistent']/div[@class='v-card v-sheet theme--light']/div[@class='v-card__actions']/button[@class='v-btn v-btn--text theme--light v-size--default primary--text']/span[@class='v-btn__content']"));
                                        popupConfirmation.Click();
                                        Thread.Sleep(1000);
                                    }
                                }

                                if (!String.IsNullOrEmpty(bestCoin.CoinAlgo))
                                {
                                    var algoDropDown = driver.FindElement(By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-application--wrap']/main[@class='v-main grey lighten-4']/div[@class='v-main__wrap']/div[@class='miner-setting px-4 my-5']/div[@class='row no-gutters justify-center']/div[@class='col col-12']/div[@class='mt-4 v-card v-sheet v-sheet--outlined theme--light'][2]/div[@class='v-card__text']/div[@class='row']/div[@class='col-sm-8 col-md-6 col-lg-4 col-xl-4 col-12']/div[@class='v-input v-input--is-label-active v-input--is-dirty theme--light v-text-field v-text-field--is-booted v-select']/div[@class='v-input__control']/div[@class='v-input__slot']/div[@class='v-select__slot']/div[@class='v-select__selections']/div[@class='v-select__selection v-select__selection--comma']"));
                                    var currentAlgo = algoDropDown.Text;
                                    if (currentAlgo != bestCoin.CoinAlgo)
                                    {
                                        var algoClicker = driver.FindElement(By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-application--wrap']/main[@class='v-main grey lighten-4']/div[@class='v-main__wrap']/div[@class='miner-setting px-4 my-5']/div[@class='row no-gutters justify-center']/div[@class='col col-12']/div[@class='mt-4 v-card v-sheet v-sheet--outlined theme--light'][2]/div[@class='v-card__text']/div[@class='row']/div[@class='col-sm-8 col-md-6 col-lg-4 col-xl-4 col-12']/div[@class='v-input v-input--is-label-active v-input--is-dirty theme--light v-text-field v-text-field--is-booted v-select']/div[@class='v-input__control']/div[@class='v-input__slot']/div[@class='v-select__slot']/div[@class='v-input__append-inner']/div[@class='v-input__icon v-input__icon--append']/i[@class='v-icon notranslate mdi mdi-menu-down theme--light']"));
                                        algoClicker.Click();
                                        Thread.Sleep(1000);
                                        var algoSelector = driver.FindElement(By.XPath(String.Format("//div[contains(text(), \"{0}\")]", bestCoin.CoinAlgo)));
                                        algoSelector.Click();
                                        Thread.Sleep(1000);
                                        var apply = driver.FindElement(By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-application--wrap']/main[@class='v-main grey lighten-4']/div[@class='v-main__wrap']/div[@class='miner-setting px-4 my-5']/div[@class='row no-gutters justify-center']/div[@class='col col-12']/div[@class='mt-4 v-card v-sheet v-sheet--outlined theme--light'][2]/div[@class='v-card__actions']/button"));
                                        apply.Click();
                                        Thread.Sleep(1000);
                                    }
                                }

                                var newPoolButton = driver.FindElement(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-application--wrap']/main[@class='v-main grey lighten-4']/div[@class='v-main__wrap']/div[@class='miner-setting px-4 my-5']/div[@class='row no-gutters justify-center']/div[@class='col col-12']/div[@class='v-card v-sheet v-sheet--outlined theme--light']/div[@class='v-card__title']/button[@class='v-btn v-btn--text theme--light v-size--default primary--text']"));
                                if (newPoolButton != null)
                                {
                                    newPoolButton.Click();
                                    Thread.Sleep(1000);
                                    try
                                    {
                                        var poolUrlField = driver.FindElement(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-dialog__content v-dialog__content--active']/div[@class='v-dialog v-dialog--active v-dialog--persistent']/form[@class='v-form']/div[@class='v-card v-sheet theme--light']/div[@class='v-card__text']/div[@class='container']/div[@class='row']/div[@class='col col-12'][2]/div[@class='v-input theme--light v-text-field v-text-field--is-booted v-text-field--placeholder']/div[@class='v-input__control']/div[@class='v-input__slot']/div[@class='v-text-field__slot']/input"));
                                        var poolUserField = driver.FindElement(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-dialog__content v-dialog__content--active']/div[@class='v-dialog v-dialog--active v-dialog--persistent']/form[@class='v-form']/div[@class='v-card v-sheet theme--light']/div[@class='v-card__text']/div[@class='container']/div[@class='row']/div[@class='col col-12'][3]/div[@class='v-input theme--light v-text-field v-text-field--is-booted']/div[@class='v-input__control']/div[@class='v-input__slot']/div[@class='v-text-field__slot']/input"));
                                        var poolPasswordField = driver.FindElement(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-dialog__content v-dialog__content--active']/div[@class='v-dialog v-dialog--active v-dialog--persistent']/form[@class='v-form']/div[@class='v-card v-sheet theme--light']/div[@class='v-card__text']/div[@class='container']/div[@class='row']/div[@class='col col-12'][4]/div[@class='v-input theme--light v-text-field v-text-field--is-booted v-text-field--placeholder']/div[@class='v-input__control']/div[@class='v-input__slot']/div[@class='v-text-field__slot']/input"));
                                        var poolApplyButton = driver.FindElement(OpenQA.Selenium.By.XPath("/html/body/div[@id='app']/div[@id='app']/div[@class='v-dialog__content v-dialog__content--active']/div[@class='v-dialog v-dialog--active v-dialog--persistent']/form[@class='v-form']/div[@class='v-card v-sheet theme--light']/div[@class='v-card__actions']/button[@class='v-btn v-btn--text theme--light v-size--default blue--text text--darken-1'][2]/span[@class='v-btn__content']"));

                                        if (poolUrlField != null && poolUserField != null && poolPasswordField != null && poolApplyButton != null)
                                        {
                                            poolUrlField.SendKeys(bestCoin.PoolUrl);
                                            poolUserField.SendKeys(bestCoin.PoolUser);
                                            poolPasswordField.SendKeys(bestCoin.PoolPassword);
                                            poolApplyButton.Click();
                                            Thread.Sleep(1000);
                                            if (donationRun)
                                            {
                                                Logger.Log(String.Format("{0} Goldshell Donation / Dev Fee Started", asic.Name), Enums.LogType.ProfitSwitching);
                                            }
                                            else
                                            {
                                                if (asic.MiningMode == MiningMode.Profit || asic.MiningMode == MiningMode.DiversificationByProfit)
                                                {
                                                    Common.Logger.Log(String.Format("{0} Goldshell pool config updated for coin {1}. Projected Profit: ${2}", asic.Name, bestCoin.Ticker, Math.Round(newCoinBestPrice, 2)), Enums.LogType.ProfitSwitching);
                                                }

                                                if (asic.MiningMode == MiningMode.CoinStacking || asic.MiningMode == MiningMode.DiversificationByStacking)
                                                {
                                                    Common.Logger.Log(String.Format("{0} Goldshell pool config updated for coin {1}. Projected Profit: {2} {1}", asic.Name, bestCoin.Ticker, newCoinBestPrice), Enums.LogType.ProfitSwitching);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Common.Logger.Log(String.Format("Unable to update {0} Goldshell miner config. Please set manually", asic.Name), Enums.LogType.ProfitSwitching);
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Logger.Log(String.Format("Unable to connect to Goldshell Device: {0}", asic.Name), Enums.LogType.System);
                    }
                    finally
                    {
                        driver.Close();
                    }
                }
            }
        }
    }
}
