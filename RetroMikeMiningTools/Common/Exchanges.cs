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
            exchangeData = exchangeData.Concat(Utilities.GenericExchangeApiUtilities.GetTickers(Constants.TX_BIT_API_BASE_PATH, Enums.Exchange.TxBit)).ToList(); //TxBit
            exchangeData = exchangeData.Concat(Utilities.TradeOgreApiUtilities.GetTickers()).ToList(); //Trade Ogre
            exchangeData = exchangeData.Concat(await Utilities.CoinExUtilities.GetTickers()).ToList(); //Coin Ex
            exchangeData = exchangeData.Concat(Utilities.SouthXchangeUtilities.GetTickers()).ToList(); //SouthXchange
            exchangeData = exchangeData.Concat(await Utilities.KucoinUtilities.GetTickers()).ToList(); //Coin Ex
        }
    }
}
