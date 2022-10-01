using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.DTO;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class AutoExchangingModel : PageModel
    {
        public static IList<ExchangeConfig>? data;
        public static IList<ExchangeBalance>? balanceData;
        private static IConfiguration systemConfiguration;
        private static bool multiUserMode = false;
        private static string? username;

        public AutoExchangingModel(IConfiguration configuration)
        {
            systemConfiguration = configuration;
        }

        public void OnGet()
        {
            if (systemConfiguration != null)
            {
                var multiUserModeConfig = systemConfiguration.GetValue<string>(Constants.MULTI_USER_MODE);
                if (!String.IsNullOrEmpty(multiUserModeConfig) && multiUserModeConfig == "true")
                {
                    username = User?.Identity?.Name;
                    multiUserMode = true;
                    ViewData["MultiUser"] = true;
                }

                var hostPlatform = systemConfiguration.GetValue<string>(Constants.PARAMETER_PLATFORM_NAME);
                if (hostPlatform != null)
                {
                    ViewData["Platform"] = hostPlatform;
                }
            }

            data = ExchangeDAO.GetRecords();
            if (multiUserMode)
            {
                data = ExchangeDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (balanceData==null)
            {
                balanceData = new List<ExchangeBalance>();
            }
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(data.ToDataSourceResult(request));
        }
        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, ExchangeConfig record)
        {
            if(multiUserMode)
            {
                record.Username = username;
            }
            ExchangeDAO.AddRecord(record);
            data = ExchangeDAO.GetRecords();
            if (multiUserMode)
            {
                data = ExchangeDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return new JsonResult(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, ExchangeConfig record)
        {
            ExchangeDAO.DeleteRecord(record);
            data = ExchangeDAO.GetRecords();
            if (multiUserMode)
            {
                data = ExchangeDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, ExchangeConfig record)
        {
            ExchangeDAO.UpdateRecord(record);
            data = ExchangeDAO.GetRecords();
            if (multiUserMode)
            {
                data = ExchangeDAO.GetRecords().Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return new JsonResult(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnGetMasterCoinList([DataSourceRequest] DataSourceRequest request, int exchangeId)
        {
            return new JsonResult(Exchanges.ExchangeCoins.Where(x => x.Exchange == (Enums.Exchange)exchangeId));
        }

        public JsonResult OnPostReadBalances([DataSourceRequest] DataSourceRequest request)
        {
            return new JsonResult(balanceData.ToDataSourceResult(request));
        }

        public JsonResult OnPostExchangeRowSelect(int exchangeId, string apiKey, string apiSecret, string passphrase)
        {
            balanceData = new List<ExchangeBalance>();
            Enums.Exchange exchange = (Enums.Exchange)exchangeId;

            var exchangeRecord = new ExchangeConfig() { ApiKey = apiKey, ApiSecret = apiSecret, Passphrase = passphrase };

            switch (exchange)
            {
                case Enums.Exchange.TxBit:
                    balanceData = GenericExchangeApiUtilities.GetWalletBalances(Constants.TX_BIT_API_BASE_PATH, exchangeRecord).Where(x => x.Balance > 0.00m).ToList();
                    break;
                case Enums.Exchange.TradeOgre:
                    balanceData = TradeOgreApiUtilities.GetBalances(exchangeRecord).Where(x => x.Balance > 0.00m).ToList();
                    break;
                case Enums.Exchange.CoinEx:
                    balanceData = CoinExUtilities.GetBalances(exchangeRecord).Result;
                    break;
                case Enums.Exchange.SouthXchange:
                    balanceData = SouthXchangeUtilities.GetBalances(exchangeRecord);
                    break;
                case Enums.Exchange.Kucoin:
                    balanceData = KucoinUtilities.GetBalances(exchangeRecord).Result;
                    break;
                default:
                    break;
            }

            if (balanceData != null)
            {
                var coreConfig = CoreConfigDAO.GetCoreConfig();
                if(multiUserMode)
                {
                    coreConfig = CoreConfigDAO.GetCoreConfig(username);
                    
                }
                if (!String.IsNullOrEmpty(coreConfig?.CoinMarketCapApi))
                {
                    List<String> marketList = balanceData.Select(x => x.Ticker).Distinct().ToList();
                    NoobsMuc.Coinmarketcap.Client.CoinmarketcapClient client = new NoobsMuc.Coinmarketcap.Client.CoinmarketcapClient(coreConfig?.CoinMarketCapApi);

                    var marketData = client.GetCurrencyBySymbolList(marketList.ToArray(), "USD");
                    if (marketData != null)
                    {
                        foreach (var item in marketData)
                        {
                            var ticker = item.Symbol;
                            var price = item.Price;
                            var balanceRecord = balanceData.Where(x => x.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (balanceRecord != null)
                            {
                                balanceRecord.UsdDisplayVal = Convert.ToDecimal(price) * balanceRecord.Balance;
                            }
                        }
                    }
                }
            }

            return new JsonResult("Data Bound");
        }
    }
}
