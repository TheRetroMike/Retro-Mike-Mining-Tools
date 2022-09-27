using RetroMikeMiningTools.DO;

namespace RetroMikeMiningTools.Common
{
    public static class Constants
    {
        public static readonly string DB_FILE = "db/retromikeminingtools.db";
        public static readonly string DEFAULT_DONATION = "1.00%";
        public static readonly string LINUX_SERVICE_CONTROLLER_CMD = "systemctl";
        public static readonly string LINUX_RESTART_SERVICE_CMD = "restart";
        public static readonly string PLATFORM_HIVE_OS = "hive_os";
        public static readonly string PLATFORM_DOCKER_ARM64 = "Docker-linux/arm64";
        public static readonly string PLATFORM_DOCKER_AMD64 = "Docker-linux/amd64";
        public static readonly string PLATFORM_DOCKER_AMD64_V2 = "Docker-linux/amd64/v2";
        public static readonly string PLATFORM_DOCKER_AMD64_V3 = "Docker-linux/amd64/v3";
        public static readonly string PLATFORM_RPI = "raspberry_pi";

        public static readonly string PARAMETER_SERVICE_NAME = "service_name";
        public static readonly string PARAMETER_PLATFORM_NAME = "platform_name";
        public static readonly string MULTI_USER_MODE = "multi_user_mode";
        public static readonly string OVERRIDE_PORT = "override_port";
        public static readonly string MULTI_USER_MODE_DONATION = "5%";

        public static readonly string TX_BIT_API_BASE_PATH = "https://api.txbit.io/api";
        public static readonly string TX_BIT_TRADE_FEE = "0.2%";
        
        public static readonly string TRADE_OGRE_API_BASE_PATH = "https://tradeogre.com/api/v1";
        public static readonly string TRADE_OGRE_TRADE_FEE = "0.2%";

        public static readonly string SOUTH_XCHANGE_TRADE_FEE = "0.3%";

        public static readonly string PUSHOVER_APP_KEY = "aycs6oxrntbbifbrdkozr1ntpvimgh";
        public static readonly string PUSHOVER_USER_KEY = "udi31rrph9qz6t6qcuwgta4j3cocwm";
        public static readonly string IP_API = "http://ip-api.com/json";

        public static readonly string COINDESK_API = "https://api.coindesk.com/v1/bpi/currentprice.json";

        public static List<FlightsheetRecord> DONATION_FLIGHTSHEET_DATA
        {
            get
            {
                return new List<FlightsheetRecord>()
                {
                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="daggerhashimoto.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Ethash"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="kawpow.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-KawPow"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="zelhash.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-ZelHash"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="etchash.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Etchash"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="octopus.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Octopus"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="zhash.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Zhash"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="randomxmonero.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-RandomX"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="neoscrypt.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-NeoScrypt"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="scrypt.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Scrypt"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="autolykos.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Autolykos"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="beamv3.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-BeamV3"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="cuckoocycle.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-CuckooCycle"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="stratum+tcp://grincuckatoo32.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Cuckatoo32"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="stratum+tcp://grincuckatoo31.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Cuckatoo31"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="stratum+tcp://handshake.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Handshake"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="stratum+tcp://eaglesong.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-Eaglesong"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="stratum+tcp://lbry.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="NICEHASH-LBRY"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="ethash.mine.zergpool.com:9999",
                        Password="c=BTC",
                        Ticker="ETHW"
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

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="scrypt.mine.zergpool.com:3433",
                        Password="c=BTC",
                        Ticker="DOGE"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="scrypt.mine.zergpool.com:3433",
                        Password="c=BTC",
                        Ticker="LTC"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="scrypt.mine.zergpool.com:3433",
                        Password="c=BTC",
                        Ticker="DGB"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="scrypt.mine.zergpool.com:3433",
                        Password="c=BTC",
                        Ticker="EMC2"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="scrypt.mine.zergpool.com:3433",
                        Password="c=BTC",
                        Ticker="XVG"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="scrypt.mine.zergpool.com:3433",
                        Password="c=BTC",
                        Ticker="XMY"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="bc1q9cmz2u5ced5fyyavstfyj28pnngwh5pn0vn7aa",
                        Pool="scrypt.mine.zergpool.com:3433",
                        Password="c=BTC",
                        Ticker="VIA"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue",
                        Pool="prohashing.com:3364",
                        Password="a=autolykos",
                        Ticker="ERGO"
                    },

                    new FlightsheetRecord()
                    {
                        Wallet="24yE4a6tCnpwcrJ5jGyBtk1kuKmqUqKi78swU1MYN3EZjWh66CFAHzGvh1G4GsFxx7YoYSaY3vtp8WfF9dSyV32CFYhVRtWqHZ5AnGFAwFWzSNmcASfpvXNjFyKKQKaj3eDG",
                        Pool="pool2.sero.cash:8808",
                        Password="x",
                        Ticker="SERO"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="iZ2CFBHEsbjPYCWHns75LFT5NwNLc2i9hPTG1SwJjonyBnBUGCtdCdpKwBbo6KdZgH1Azg9vNcyLoBXBLEaz3HADLhncV4uBH9C2Ey4kyDSF",
                        Pool="zano.luckypool.io:8877",
                        Password="x",
                        Ticker="ZANO"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="0xd18c26638fcc3f2e9b806c8959f86438c5f4461d",
                        Pool="ctxc.2miners.com:2222",
                        Password="x",
                        Ticker="CTXC"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="1514be666eb4ac2155183d32cb512ed689c47952090defd91e208b228d430949d92",
                        Pool="us-beam.2miners.com:5252",
                        Password="x",
                        Ticker="BEAM"
                    },
                    
                    new FlightsheetRecord()
                    {
                        Wallet="ak_JoojsyCeTQ5kAKSvoAKFG7s3PUQHhH4BpJNQcQB3y66cMacsb",
                        Pool="ae.2miners.com:4040",
                        Password="x",
                        Ticker="AE"
                    },
                    
                    new FlightsheetRecord()
                    {
                        Wallet="0xa0b29e3faa57056e97f163001f00b583cd497b674ae3cdbfc92cedcf5d12dc7b",
                        Pool="pool.woolypooly.com:33333",
                        Password="x",
                        Ticker="AION"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="5t5mEm254JNJ9HqRjY9vCiTE8aZALHX3v8TqhyQ3TTF9VHKZQXkRYjPDweT9kK4rJw7dDLtZXGjav2z9y24vXCdRbybmW2eTRpM15NQZSa",
                        Pool="pool.hashvault.pro:80",
                        Password="x",
                        Ticker="MSR"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="kaspa:qp68tc8esnku9tsszq4m6z4zev3e2qtrdmc7cy76fjakkzu4luy5klhjl6pkd",
                        Pool="pool.woolypooly.com:3112",
                        Password="x",
                        Ticker="KAS"
                    },
                    
                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="stratum+tcp://grincuckatoo32.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="GRIN"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U.%WORKER_NAME%",
                        Pool="stratum+tcp://grincuckatoo31.auto.nicehash.com:9200",
                        Password="x",
                        Ticker="MWC"
                    },
                    
                    new FlightsheetRecord()
                    {
                        Wallet="ac8c2c19bac533c51fd4771a95d8bde73357e0efcd8c78416a377dbee33ead7daa7e0455b2f8.%WORKER_NAME%",
                        Pool="us-east.siamining.com:3333",
                        Password="x",
                        Ticker="SC"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue.%WORKER_NAME%",
                        Pool="mining.viabtc.io:3008",
                        Password="123",
                        Ticker="HNS"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue.%WORKER_NAME%",
                        Pool="mining.viabtc.io:3011",
                        Password="123",
                        Ticker="LBC"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="05sonicblue.%WORKER_NAME%",
                        Pool="mining.viabtc.io:3001",
                        Password="123",
                        Ticker="CKB"
                    },
                    new FlightsheetRecord()
                    {
                        Wallet="1J1gPv9KGNLRrx5J57kJdVsSoEZX8ws25i",
                        Pool="stratum-na.rplant.xyz:17086",
                        Password="",
                        Ticker="RDX"
                    },
                };
            }
        }

        public static Dictionary<string, string> GOLDSHELL_ASIC_DEFAULT_WTM
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {"Goldshell-MiniDOGEPro","https://whattomine.com/asic.json?factor%5Bsha256_hr%5D=3140.0&factor%5Bsha256_p%5D=200.0&scryptf=true&factor%5Bscrypt_hash_rate%5D=0.205&factor%5Bscrypt_power%5D=220.0&factor%5Bx11_hr%5D=11000.0&factor%5Bx11_p%5D=0.0&factor%5Bsia_hr%5D=900.0&factor%5Bsia_p%5D=200.0&factor%5Bqk_hr%5D=11000.0&factor%5Bqk_p%5D=0.0&factor%5Bqb_hr%5D=11000.0&factor%5Bqb_p%5D=0.0&factor%5Bmg_hr%5D=11.0&factor%5Bmg_p%5D=0.0&factor%5Bsk_hr%5D=11.0&factor%5Bsk_p%5D=0.0&factor%5Blbry_hr%5D=180.0&factor%5Blbry_p%5D=162.0&factor%5Bbk14_hr%5D=52000.0&factor%5Bbk14_p%5D=2200.0&factor%5Bcn_hr%5D=360.0&factor%5Bcn_p%5D=720.0&factor%5Bcst_hr%5D=13.9&factor%5Bcst_p%5D=62.0&factor%5Beq_hr%5D=14.0&factor%5Beq_p%5D=300.0&factor%5Blrev2_hr%5D=13.0&factor%5Blrev2_p%5D=1100.0&factor%5Bbcd_hr%5D=185.0&factor%5Bbcd_p%5D=670.0&factor%5Bl2z_hr%5D=62.0&factor%5Bl2z_p%5D=670.0&factor%5Bkec_hr%5D=29.0&factor%5Bkec_p%5D=430.0&factor%5Bgro_hr%5D=11.0&factor%5Bgro_p%5D=0.0&factor%5Besg_hr%5D=1050.0&factor%5Besg_p%5D=215.0&factor%5Bct31_hr%5D=126.0&factor%5Bct31_p%5D=2800.0&factor%5Bct32_hr%5D=36.0&factor%5Bct32_p%5D=2800.0&factor%5Bkd_hr%5D=40.2&factor%5Bkd_p%5D=3350.0&factor%5Bhk_hr%5D=4.3&factor%5Bhk_p%5D=3250.0&factor%5Bcost%5D=0.1&factor%5Bcost_currency%5D=USD&sort=Profit&volume=0&revenue=current&factor%5Bexchanges%5D%5B%5D=&factor%5Bexchanges%5D%5B%5D=binance&factor%5Bexchanges%5D%5B%5D=bitfinex&factor%5Bexchanges%5D%5B%5D=bitforex&factor%5Bexchanges%5D%5B%5D=bittrex&factor%5Bexchanges%5D%5B%5D=coinex&factor%5Bexchanges%5D%5B%5D=exmo&factor%5Bexchanges%5D%5B%5D=gate&factor%5Bexchanges%5D%5B%5D=graviex&factor%5Bexchanges%5D%5B%5D=hitbtc&factor%5Bexchanges%5D%5B%5D=ogre&factor%5Bexchanges%5D%5B%5D=poloniex&factor%5Bexchanges%5D%5B%5D=stex&dataset=Main" },
                    {"Goldshell-CKBox", "https://whattomine.com/asic.json?factor%5Bsha256_hr%5D=3140.0&factor%5Bsha256_p%5D=200.0&factor%5Bscrypt_hash_rate%5D=0.205&factor%5Bscrypt_power%5D=220.0&factor%5Bx11_hr%5D=11000.0&factor%5Bx11_p%5D=0.0&factor%5Bsia_hr%5D=900.0&factor%5Bsia_p%5D=200.0&factor%5Bqk_hr%5D=11000.0&factor%5Bqk_p%5D=0.0&factor%5Bqb_hr%5D=11000.0&factor%5Bqb_p%5D=0.0&factor%5Bmg_hr%5D=11.0&factor%5Bmg_p%5D=0.0&factor%5Bsk_hr%5D=11.0&factor%5Bsk_p%5D=0.0&factor%5Blbry_hr%5D=180.0&factor%5Blbry_p%5D=162.0&factor%5Bbk14_hr%5D=52000.0&factor%5Bbk14_p%5D=2200.0&factor%5Bcn_hr%5D=360.0&factor%5Bcn_p%5D=720.0&factor%5Bcst_hr%5D=13.9&factor%5Bcst_p%5D=62.0&factor%5Beq_hr%5D=14.0&factor%5Beq_p%5D=300.0&factor%5Blrev2_hr%5D=13.0&factor%5Blrev2_p%5D=1100.0&factor%5Bbcd_hr%5D=185.0&factor%5Bbcd_p%5D=670.0&factor%5Bl2z_hr%5D=62.0&factor%5Bl2z_p%5D=670.0&factor%5Bkec_hr%5D=29.0&factor%5Bkec_p%5D=430.0&factor%5Bgro_hr%5D=11.0&factor%5Bgro_p%5D=0.0&esg=true&factor%5Besg_hr%5D=1050.0&factor%5Besg_p%5D=215.0&factor%5Bct31_hr%5D=126.0&factor%5Bct31_p%5D=2800.0&factor%5Bct32_hr%5D=36.0&factor%5Bct32_p%5D=2800.0&factor%5Bkd_hr%5D=40.2&factor%5Bkd_p%5D=3350.0&factor%5Bhk_hr%5D=4.3&factor%5Bhk_p%5D=3250.0&factor%5Bcost%5D=0.1&factor%5Bcost_currency%5D=USD&sort=Profit&volume=0&revenue=current&factor%5Bexchanges%5D%5B%5D=&factor%5Bexchanges%5D%5B%5D=binance&factor%5Bexchanges%5D%5B%5D=bitfinex&factor%5Bexchanges%5D%5B%5D=bitforex&factor%5Bexchanges%5D%5B%5D=bittrex&factor%5Bexchanges%5D%5B%5D=coinex&factor%5Bexchanges%5D%5B%5D=exmo&factor%5Bexchanges%5D%5B%5D=gate&factor%5Bexchanges%5D%5B%5D=graviex&factor%5Bexchanges%5D%5B%5D=hitbtc&factor%5Bexchanges%5D%5B%5D=ogre&factor%5Bexchanges%5D%5B%5D=poloniex&factor%5Bexchanges%5D%5B%5D=stex&dataset=Main" },
                    {"Goldshell-LBBox", "https://whattomine.com/asic.json?factor%5Bsha256_hr%5D=3140.0&factor%5Bsha256_p%5D=200.0&factor%5Bscrypt_hash_rate%5D=0.205&factor%5Bscrypt_power%5D=220.0&factor%5Bx11_hr%5D=11000.0&factor%5Bx11_p%5D=0.0&factor%5Bsia_hr%5D=900.0&factor%5Bsia_p%5D=200.0&factor%5Bqk_hr%5D=11000.0&factor%5Bqk_p%5D=0.0&factor%5Bqb_hr%5D=11000.0&factor%5Bqb_p%5D=0.0&factor%5Bmg_hr%5D=11.0&factor%5Bmg_p%5D=0.0&factor%5Bsk_hr%5D=11.0&factor%5Bsk_p%5D=0.0&lbry=true&factor%5Blbry_hr%5D=180.0&factor%5Blbry_p%5D=162.0&factor%5Bbk14_hr%5D=52000.0&factor%5Bbk14_p%5D=2200.0&factor%5Bcn_hr%5D=360.0&factor%5Bcn_p%5D=720.0&factor%5Bcst_hr%5D=13.9&factor%5Bcst_p%5D=62.0&factor%5Beq_hr%5D=14.0&factor%5Beq_p%5D=300.0&factor%5Blrev2_hr%5D=13.0&factor%5Blrev2_p%5D=1100.0&factor%5Bbcd_hr%5D=185.0&factor%5Bbcd_p%5D=670.0&factor%5Bl2z_hr%5D=62.0&factor%5Bl2z_p%5D=670.0&factor%5Bkec_hr%5D=29.0&factor%5Bkec_p%5D=430.0&factor%5Bgro_hr%5D=11.0&factor%5Bgro_p%5D=0.0&factor%5Besg_hr%5D=1050.0&factor%5Besg_p%5D=215.0&factor%5Bct31_hr%5D=126.0&factor%5Bct31_p%5D=2800.0&factor%5Bct32_hr%5D=36.0&factor%5Bct32_p%5D=2800.0&factor%5Bkd_hr%5D=40.2&factor%5Bkd_p%5D=3350.0&factor%5Bhk_hr%5D=4.3&factor%5Bhk_p%5D=3250.0&factor%5Bcost%5D=0.1&factor%5Bcost_currency%5D=USD&sort=Profit&volume=0&revenue=current&factor%5Bexchanges%5D%5B%5D=&factor%5Bexchanges%5D%5B%5D=binance&factor%5Bexchanges%5D%5B%5D=bitfinex&factor%5Bexchanges%5D%5B%5D=bitforex&factor%5Bexchanges%5D%5B%5D=bittrex&factor%5Bexchanges%5D%5B%5D=coinex&factor%5Bexchanges%5D%5B%5D=exmo&factor%5Bexchanges%5D%5B%5D=gate&factor%5Bexchanges%5D%5B%5D=graviex&factor%5Bexchanges%5D%5B%5D=hitbtc&factor%5Bexchanges%5D%5B%5D=ogre&factor%5Bexchanges%5D%5B%5D=poloniex&factor%5Bexchanges%5D%5B%5D=stex&dataset=Main" },
                    {"Goldshell-HSBox", "https://whattomine.com/asic.json?factor%5Bsha256_hr%5D=3140.0&factor%5Bsha256_p%5D=200.0&factor%5Bscrypt_hash_rate%5D=0.205&factor%5Bscrypt_power%5D=220.0&factor%5Bx11_hr%5D=11000.0&factor%5Bx11_p%5D=0.0&sia=true&factor%5Bsia_hr%5D=0.47&factor%5Bsia_p%5D=130.0&factor%5Bqk_hr%5D=11000.0&factor%5Bqk_p%5D=0.0&factor%5Bqb_hr%5D=11000.0&factor%5Bqb_p%5D=0.0&factor%5Bmg_hr%5D=11.0&factor%5Bmg_p%5D=0.0&factor%5Bsk_hr%5D=11.0&factor%5Bsk_p%5D=0.0&factor%5Blbry_hr%5D=180.0&factor%5Blbry_p%5D=162.0&factor%5Bbk14_hr%5D=52000.0&factor%5Bbk14_p%5D=2200.0&factor%5Bcn_hr%5D=360.0&factor%5Bcn_p%5D=720.0&factor%5Bcst_hr%5D=13.9&factor%5Bcst_p%5D=62.0&factor%5Beq_hr%5D=14.0&factor%5Beq_p%5D=300.0&factor%5Blrev2_hr%5D=13.0&factor%5Blrev2_p%5D=1100.0&factor%5Bbcd_hr%5D=185.0&factor%5Bbcd_p%5D=670.0&factor%5Bl2z_hr%5D=62.0&factor%5Bl2z_p%5D=670.0&factor%5Bkec_hr%5D=29.0&factor%5Bkec_p%5D=430.0&factor%5Bgro_hr%5D=11.0&factor%5Bgro_p%5D=0.0&factor%5Besg_hr%5D=1050.0&factor%5Besg_p%5D=215.0&factor%5Bct31_hr%5D=126.0&factor%5Bct31_p%5D=2800.0&factor%5Bct32_hr%5D=36.0&factor%5Bct32_p%5D=2800.0&factor%5Bkd_hr%5D=40.2&factor%5Bkd_p%5D=3350.0&hk=true&factor%5Bhk_hr%5D=0.235&factor%5Bhk_p%5D=230.0&factor%5Bcost%5D=0.1&factor%5Bcost_currency%5D=USD&sort=Profit&volume=0&revenue=current&factor%5Bexchanges%5D%5B%5D=&factor%5Bexchanges%5D%5B%5D=binance&factor%5Bexchanges%5D%5B%5D=bitfinex&factor%5Bexchanges%5D%5B%5D=bitforex&factor%5Bexchanges%5D%5B%5D=bittrex&factor%5Bexchanges%5D%5B%5D=coinex&factor%5Bexchanges%5D%5B%5D=exmo&factor%5Bexchanges%5D%5B%5D=gate&factor%5Bexchanges%5D%5B%5D=graviex&factor%5Bexchanges%5D%5B%5D=hitbtc&factor%5Bexchanges%5D%5B%5D=ogre&factor%5Bexchanges%5D%5B%5D=poloniex&factor%5Bexchanges%5D%5B%5D=stex&dataset=Main" },
                };
            }
        }
    }
}
