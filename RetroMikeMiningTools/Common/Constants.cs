using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Common
{
    public static class Constants
    {
        public static readonly string DB_FILE = "retromikeminingtools.db";
        public static readonly string DEFAULT_DONATION = "1.00%";
        public static readonly string LINUX_SERVICE_CONTROLLER_CMD = "systemctl";
        public static readonly string LINUX_RESTART_SERVICE_CMD = "restart";
        public static readonly string PLATFORM_HIVE_OS = "restart";

        public static readonly string PARAMETER_SERVICE_NAME = "--service_name";
        public static readonly string PARAMETER_PLATFORM_NAME = "--platform_name";

        public static List<FlightsheetRecord> DONATION_FLIGHTSHEET_DATA
        {
            get
            {
                return new List<FlightsheetRecord>()
                {
                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ethash.mine.zergpool.com:9999",
                        Password="c=BTC",
                        Ticker="NICEHASH-Ethash"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="kawpow.mine.zergpool.com:3638",
                        Password="c=BTC",
                        Ticker="NICEHASH-KawPow"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="equihash125.mine.zergpool.com",
                        Password="c=BTC",
                        Ticker="NICEHASH-ZelHash"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="etchash.mine.zergpool.com:9997",
                        Password="c=BTC",
                        Ticker="NICEHASH-Etchash"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ethash.mine.zergpool.com:9999",
                        Password="c=BTC",
                        Ticker="ETH"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ethash.mine.zergpool.com:9999",
                        Password="c=BTC",
                        Ticker="ETHO"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ethash.mine.zergpool.com:9999",
                        Password="c=BTC",
                        Ticker="EXP"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ethash.mine.zergpool.com:9999",
                        Password="c=BTC",
                        Ticker="CLO"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ethash.mine.zergpool.com:9999",
                        Password="c=BTC",
                        Ticker="QKC"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ethash.mine.zergpool.com:9999",
                        Password="c=BTC",
                        Ticker="EGEM"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="etchash.mine.zergpool.com:9997",
                        Password="c=BTC",
                        Ticker="ETC"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="kawpow.mine.zergpool.com:3638",
                        Password="c=BTC",
                        Ticker="NEOX"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="kawpow.mine.zergpool.com:3638",
                        Password="c=BTC",
                        Ticker="RVN"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="firopow.mine.zergpool.com:3001",
                        Password="c=BTC",
                        Ticker="FIRO"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="equihash125.mine.zergpool.com",
                        Password="c=BTC",
                        Ticker="FLUX"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ubqhash.mine.zergpool.com:9998",
                        Password="c=BTC",
                        Ticker="UBQ"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue.a=octopus",
                        Pool="prohashing.com:3363",
                        Password="",
                        Ticker="CFX"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue.a=octopus",
                        Pool="prohashing.com:3363",
                        Password="",
                        Ticker="NICEHASH-Octopus"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="equihash144.mine.zergpool.com:2146",
                        Password="c=BTC",
                        Ticker="BTG"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="equihash144.mine.zergpool.com:2146",
                        Password="c=BTC",
                        Ticker="BTCZ"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="equihash144.mine.zergpool.com:2146",
                        Password="c=BTC",
                        Ticker="GLINK"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="verthash.mine.zergpool.com:4534",
                        Password="c=BTC",
                        Ticker="VTC"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="verthash.mine.zergpool.com:4534",
                        Password="c=BTC",
                        Ticker="NICEHASH-Zhash"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="cryptonight_gpu.mine.zergpool.com:4445",
                        Password="c=BTC",
                        Ticker="RYO"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="cryptonight_gpu.mine.zergpool.com:4445",
                        Password="c=BTC",
                        Ticker="CCX"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="cryptonight_gpu.mine.zergpool.com:4445",
                        Password="c=BTC",
                        Ticker="XEQ"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="x25x.mine.zergpool.com:3225",
                        Password="c=BTC",
                        Ticker="SIN"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="cryptonight_haven.mine.zergpool.com:4452",
                        Password="c=BTC",
                        Ticker="BLOC"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="cryptonight_haven.mine.zergpool.com:4452",
                        Password="c=BTC",
                        Ticker="XHV"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue",
                        Pool="prohashing.com:3359",
                        Password="a=randomx",
                        Ticker="XMR"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue",
                        Pool="prohashing.com:3359",
                        Password="a=randomx",
                        Ticker="QRL"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue",
                        Pool="prohashing.com:3359",
                        Password="a=randomx",
                        Ticker="NICEHASH-RandomX"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue",
                        Pool="prohashing.com:3338",
                        Password="a=neoscrypt",
                        Ticker="NICEHASH-NeoScrypt"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue",
                        Pool="prohashing.com:3338",
                        Password="a=neoscrypt",
                        Ticker="FTC"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue",
                        Pool="prohashing.com:3338",
                        Password="a=neoscrypt",
                        Ticker="TZC"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue",
                        Pool="prohashing.com:3338",
                        Password="a=neoscrypt",
                        Ticker="GBX"
                    },

                    //Ergo: Waiting for prohashing to activate autolykos
                    //new FlightsheetRecord()
                    //{
                    //    Wallet="",
                    //    Pool="",
                    //    Password="",
                    //    Ticker="ERGO"
                    //},
                    //new FlightsheetRecord()
                    //{
                    //    Wallet="",
                    //    Pool="",
                    //    Password="",
                    //    Ticker="NICEHASH-Autolykos"
                    //},

                    //Excluded Coins
                    //SERO
                    //ZANO
                    //Cortex
                    //Beam
                    //NICEHASH-BeamV3
                    //AE
                    //NICEHASH-CuckooCycle
                    //AION
                    //SWAP
                    //TUBE
                    //GRIN
                    //MWC
                    //NICEHASH-Cuckatoo32
                    //NICEHASH-Cuckatoo31
                    //Masari
                };
            }
        }
    }
}
