using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Utilities;

namespace RetroMikeMiningTools.Common
{
    public static class Coins
    {
        public static List<Coin> AsicCoinList
        {
            get
            {
                return new List<Coin>(){
                    new Coin(){ Ticker = "DOGE", Name = "Dogecoin (DOGE)"},
                    new Coin(){ Ticker = "LTC", Name = "Litcoin (LTC)" },
                    new Coin(){ Ticker = "Nicehash-Scrypt", Name = "Nicehash-Scrypt"},
                    new Coin(){ Ticker = "DGB", Name = "DGB-Scrypt (DGB)"},
                    new Coin(){ Ticker = "EMC2", Name = "Einsteinium (EMC2)"},
                    new Coin(){ Ticker = "XVG", Name = "Verge-Scrypt (XVG)"},
                    new Coin(){ Ticker = "XMY", Name = "Myriad-Scrypt (XMY)"},
                    new Coin(){ Ticker = "VIA", Name = "Viacoin (VIA)"},
                    new Coin(){ Ticker = "SC", Name = "Sia (SC)"},
                    new Coin(){ Ticker = "HNS", Name = "Handshake (HNS)"},
                    new Coin(){ Ticker = "LBC", Name = "LBRY (LBC)"},
                    new Coin(){ Ticker = "CKB", Name = "Nervos (CKB)"},
                    new Coin(){ Ticker = "Nicehash-LBRY", Name = "Nicehash-Lbry"},
                    new Coin(){ Ticker = "Nicehash-Eaglesong", Name = "Nicehash-Eaglesong"},
                    new Coin(){ Ticker = "Nicehash-Handshake", Name = "Nicehash-Handshake"},
                };
            }
        }

        public static List<Coin> ZergAlgoList
        {
            get
            {
                var result = new List<Coin>();
                result = result.Concat(ZergUtilities.GetZergAlgos()).ToList();
                result.Add(new Coin() { Algorithm="k12", Name="k12" });
                return result;
            }
        }

        public static List<Coin> CoinList
        {
            get
            {
                List<Coin> result = new List<Coin>();
                result.AddRange(ZergUtilities.GetZergAlgos());
                result.AddRange(ProhashingUtilities.GetAlgos());
                result.AddRange(MiningDutchUtilities.GetAlgos());

                foreach (Coin coin in WhatToMineUtilities.GetCoinList("https://whattomine.com/"))
                {
                    var existingRecord = result.Where(x => x.Ticker.Equals(coin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingRecord == null)
                    {
                        result.Add(coin);
                    }
                }

                foreach (Coin coin in WhatToMineUtilities.GetCoinList("https://whattomine.com/asic"))
                {
                    var existingRecord = result.Where(x => x.Ticker.Equals(coin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingRecord == null)
                    {
                        result.Add(coin);
                    }
                }

                foreach (Coin coin in WhatToMineUtilities.GetIndividualCoinList())
                {
                    var existingRecord = result.Where(x => x.Ticker.Equals(coin.Ticker, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingRecord == null)
                    {
                        result.Add(coin);
                    }
                }

                foreach (Coin coin in ZergUtilities.GetZergCoins())
                {
                    if (coin.Ticker != null)
                    {
                        var tickerKey = coin.Ticker.Replace("ZergProvider-", String.Empty);
                        var existingRecord = result.Where(x => x.Ticker.Equals(tickerKey, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (existingRecord == null)
                        {
                            result.Add(coin);
                        }
                    }
                }

                result.Add(new Coin() { Ticker = "RDX", Name = "Radiant (RDX)" });
                result = result.OrderBy(x => x.Name).ToList();
                return result.Distinct().ToList();
            }
        }
    }
}
