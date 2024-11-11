using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Common
{
    public static class Exchanges
    {
        private static List<Coin> exchangeData;
        public static List<Coin> ExchangeCoins
        {
            get
            {
                GetExchangeCoins().Wait();
                return exchangeData.OrderBy(x => x.Name).ToList();
            }
        }

        public static async Task GetExchangeCoins()
        {
            exchangeData = new List<Coin>();
            try
            {
                exchangeData = exchangeData.Concat(Utilities.TradeOgreApiUtilities.GetTickers()).ToList(); //Trade Ogre
            }
            catch { }

            try
            {
                exchangeData = exchangeData.Concat(await Utilities.CoinExUtilities.GetTickers()).ToList(); //Coin Ex
            }
            catch { }

            try
            {
                exchangeData = exchangeData.Concat(await Utilities.KucoinUtilities.GetTickers()).ToList(); //Coin Ex
            }
            catch { }

            try
            {
                exchangeData = exchangeData.Concat(Utilities.XeggexUtilities.GetTickers()).ToList();
            }
            catch { }
        }
    }
}
