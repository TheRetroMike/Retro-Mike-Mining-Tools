using RetroMikeMiningTools.DO;

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
                    new Coin(){ Ticker = "Nicehash-Lbry", Name = "Nicehash-Lbry"},
                    new Coin(){ Ticker = "Nicehash-Eaglesong", Name = "Nicehash-Eaglesong"},
                    new Coin(){ Ticker = "Nicehash-Handshake", Name = "Nicehash-Handshake"},
                };
            }
        }

        public static List<Coin> CoinList
        {
            get
            {
                return new List<Coin>() {
                    new Coin(){ Ticker = "KAS", Name = "Kaspa (KAS)"},
                    new Coin(){ Ticker = "VTC", Name = "Vertcoin (VTC)"},
                    new Coin(){ Ticker = "Nicehash-KawPow", Name = "Nicehash-KawPow"},
                    new Coin(){ Ticker = "ZANO", Name = "Zano (ZANO)"},
                    new Coin(){ Ticker = "NEOX", Name = "Neoxa (NEOX)"},
                    new Coin(){ Ticker = "RVN", Name = "Ravencoin (RVN)"},
                    new Coin(){ Ticker = "Nicehash-BeamV3", Name = "Nicehash-BeamV3"},
                    new Coin(){ Ticker = "FTC", Name = "Feathercoin (FTC)"},
                    new Coin(){ Ticker = "BEAM", Name = "Beam (BEAM)"},
                    new Coin(){ Ticker = "RYO", Name = "Ryo (RYO)"},
                    new Coin(){ Ticker = "FIRO", Name = "Firo (FIRO)"},
                    new Coin(){ Ticker = "CCX", Name = "Conceal (CCX)"},
                    new Coin(){ Ticker = "TZC", Name = "Trezarcoin (TZC)"},
                    new Coin(){ Ticker = "BLOC", Name = "Bloc.money (BLOC)"},
                    new Coin(){ Ticker = "XHV", Name = "Haven Protocol (XHV)"},
                    new Coin(){ Ticker = "Nicehash-NeoScrypt", Name = "Nicehash-NeoScrypt"},
                    new Coin(){ Ticker = "SERO", Name = "Sero (SERO)"},
                    new Coin(){ Ticker = "CTXC", Name = "Cortex (CTXC)"},
                    new Coin(){ Ticker = "QKC", Name = "Quark Chain (QKC)"},
                    new Coin(){ Ticker = "XWP", Name = "Swap (XWP)"},
                    new Coin(){ Ticker = "GBX", Name = "Go Byte (GBX)"},
                    new Coin(){ Ticker = "Nicehash-Autolykos", Name = "Nicehash-Autolykos"},
                    new Coin(){ Ticker = "AION", Name = "Aion (AION)"},
                    new Coin(){ Ticker = "Nicehash-ZelHash", Name = "Nicehash-ZelHash"},
                    new Coin(){ Ticker = "UBQ", Name = "Ubiq (UBQ)"},
                    new Coin(){ Ticker = "FLUX", Name = "Flux (FLUX)"},
                    new Coin(){ Ticker = "GRIN", Name = "Grin (GRIN)"},
                    new Coin(){ Ticker = "BTCZ", Name = "Bitcoin Z (BTCZ)"},
                    new Coin(){ Ticker = "Nicehash-CukooCycle", Name = "Nicehash-CukooCycle"},
                    new Coin(){ Ticker = "AE", Name = "Aeternity (AE)"},
                    new Coin(){ Ticker = "GLINK", Name = "Gemlink (GLINK)"},
                    new Coin(){ Ticker = "TUBE", Name = "BitTube Cash (TUBE)"},
                    new Coin(){ Ticker = "Nicehash-Etchash", Name = "Nicehash-Etchash"},
                    new Coin(){ Ticker = "ERG", Name = "Ergo (ERG)"},
                    new Coin(){ Ticker = "ETC", Name = "Ethereum Classic (ETC)"},
                    new Coin(){ Ticker = "Nicehash-Cuckatoo32", Name = "Nicehash-Cuckatoo32"},
                    new Coin(){ Ticker = "BTG", Name = "Bitcoin Gold (BTG)"},
                    new Coin(){ Ticker = "Nicehash-Zhash", Name = "Nicehash-Zhash"},
                    new Coin(){ Ticker = "XEQ", Name = "Equilibria (XEQ)"},
                    new Coin(){ Ticker = "ETHO", Name = "ETHO Protocol (ETHO)"},
                    new Coin(){ Ticker = "MSR", Name = "Masari (MSR)"},
                    new Coin(){ Ticker = "EXP", Name = "Expanse (EXP)"},
                    new Coin(){ Ticker = "CFX", Name = "Conflux (CFX)"},
                    new Coin(){ Ticker = "SIN", Name = "Sinovate (SIN)"},
                    new Coin(){ Ticker = "CLO", Name = "Callisto (CLO)"},
                    new Coin(){ Ticker = "QRL", Name = "Quantum R L (QRL)"},
                    new Coin(){ Ticker = "XMR", Name = "Monero (XMR)"},
                    new Coin(){ Ticker = "EGEM", Name = "EtherGem (EGEM)"},
                    new Coin(){ Ticker = "Nicehash-Octopus", Name = "Nicehash-Octopus"},
                    new Coin(){ Ticker = "Nicehash-Ethash", Name = "Nicehash-Ethash"},
                    new Coin(){ Ticker = "ETHW", Name = "EthereumPoW (ETHW)"},
                };
            }
        }
    }
}
