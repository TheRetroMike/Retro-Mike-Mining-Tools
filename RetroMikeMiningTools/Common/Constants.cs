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
                    new FlightsheetRecord(){Wallet="0xD733458DbE3cECC238d89a98769ccadCcB5EA9D4",Ticker="ETHW" },
                    new FlightsheetRecord(){Wallet="0xD733458DbE3cECC238d89a98769ccadCcB5EA9D4",Ticker="ETHF"},
                    new FlightsheetRecord(){Wallet="0x1a2a13a009E8953b3c7a652a2A904711353Ee998",Ticker="ETHO"},
                    new FlightsheetRecord(){Wallet="0xD60AA0669837EaDA9993734405b76d6859B88D32",Ticker="EXP"},
                    new FlightsheetRecord(){Wallet="0x8E3b1651182665397674f036C0e269F9aF521bF3",Ticker="CLO"},
                    new FlightsheetRecord(){Wallet="0xC951767591258C225ec784085fD461e687631030",Ticker="EGEM"},
                    new FlightsheetRecord(){Wallet="0x012D0487a454D0b6A7042fe946A3061b5fA9A621",Ticker="ETC"},
                    new FlightsheetRecord(){Wallet="GS83o1Hfqv2phjKkeFFHS5SArT7NZ116xv",Ticker="NEOX"},
                    new FlightsheetRecord(){Wallet="RLHvQnYgrzkxSBNHKSj3Gc8aSCQox64A8p",Ticker="RVN"},
                    new FlightsheetRecord(){Wallet="aPccbfhaScMRW1QHm3FQoJwKEfHHXXRuTM",Ticker="FIRO"},
                    new FlightsheetRecord(){Wallet="t1UtLLc5YEVjyxp3ynhZ3JtuHvb9J2JvPLQ",Ticker="FLUX"},
                    new FlightsheetRecord(){Wallet="0xa7ec22f6760658DD654949653c9C620522F99544",Ticker="UBQ"},
                    new FlightsheetRecord(){Wallet="cfx:aastu8x337k77622hdeg9kze27byaxk76jbpnky4a6",Ticker="CFX"},
                    new FlightsheetRecord(){Wallet="0xd5dc9f433702bef90c79d910fe3dea45aa1fbb5300000000",Ticker="QKC"},
                    new FlightsheetRecord(){Wallet="AGoFR5WjmZ3m8z7L2yMi9EouZe3wAHtaDu",Ticker="BTG"},
                    new FlightsheetRecord(){Wallet="t1bSYLj22UaSziDstTH9DFQ7z8fvqXSgoKB",Ticker="BTCZ"},
                    new FlightsheetRecord(){Wallet="3Ha3UvMnb4mPcN42aFDE2EvVnz5bMEpwQJ",Ticker="VTC"},
                    new FlightsheetRecord(){Wallet="RYoNsBiFU6iYi8rqkmyE9c4SftzYzWPCGA3XvcXbGuBYcqDQJWe8wp8NEwNicFyzZgKTSjCjnpuXTitwn6VdBcFZEFXLcTgQy6gYsqRN4UVRvR",Ticker="RYO"},
                    new FlightsheetRecord(){Wallet="ccx7aoNYpGb7sndJtEDWvCBQhPAy9mC8QW5KWuCx8J1FJrDcDrER1XYA9LGtggrR7ZC4KfQmQ2uRN47L9WypBbNLAeq2Q4Q9WN.d027693e1a7b69639583a48fb0f4826c5d63d4cc9526beede3c109f5aac34835",Ticker="CCX"},
                    new FlightsheetRecord(){Wallet="Tsz59oveWtic6h1PHmiQwBRaJznK7a4QcEWqxPJRG5UgXFWLrsZPQou5CayqMEKenZSH1ocBLcREZY5MqpnpPmMw1Hmcq2a1LE",Ticker="XEQ"},
                    new FlightsheetRecord(){Wallet="SX3D3rynfFYgQGQQgtr8gt6oCYPeFYArwh",Ticker="SIN"},
                    new FlightsheetRecord(){Wallet="abLoc7JNzYXijnKnPf7tSFUNSWBuwKrmUPMevvPkH4jc3b1K9LmS76DKpPamgQ5AYAC2CW9dJfTJ91AnXHYDNXAKRqPx5ZrtR49+cc9260956d08b7deab9098c130750fc90d9da4cd48c21f0de627f9914bc49293",Ticker="BLOC"},
                    new FlightsheetRecord(){Wallet="hvs1Mv7guY3PgQdo2PjRGk7tvVdneNxtL5SFkkYi9SPNNCrKUEkUVXd1sCFAR4wcW8fMzxAzQfhcggHCQxrTuVCL7bnX27aWeo",Ticker="XHV"},
                    new FlightsheetRecord(){Wallet="88uHtaQX8kL3CMxXTxGGFaTaxumGMef1HBoVdkPfxDyhXKY8p1zHWLiiewrc9s25q3boTSuJGZHgBhMry2yad5RS9X2LApt",Ticker="XMR"},
                    new FlightsheetRecord(){Wallet="DJm56pVkWqQj26A2UFeafwFw1bY632trLY",Ticker="DOGE"},
                    new FlightsheetRecord(){Wallet="ltc1qgpks9d0tjfucx9x9wqu4j099hqxfkygmug84qm",Ticker="LTC"},
                    new FlightsheetRecord(){Wallet="dgb1q6k7sxkwku0nn9tch8vu4arxg63q7l70hwsw2cp",Ticker="DGB"},
                    new FlightsheetRecord(){Wallet="DQRhSWGMSv2SJ4SrZWQg8bYGmAojrzTvKV",Ticker="XVG"},
                    new FlightsheetRecord(){Wallet="ERz6GRmPbtB2HumzxMVGftwnVcr2Et4kxX",Ticker="VIA"},
                    new FlightsheetRecord(){Wallet="9eya64Qm8iz7vv6SsCF6sFJWoAZDouY59j56e7K2CaLvJoHAqzr",Ticker="ERG"},
                    new FlightsheetRecord(){Wallet="24yE4a6tCnpwcrJ5jGyBtk1kuKmqUqKi78swU1MYN3EZjWh66CFAHzGvh1G4GsFxx7YoYSaY3vtp8WfF9dSyV32CFYhVRtWqHZ5AnGFAwFWzSNmcASfpvXNjFyKKQKaj3eDG",Ticker="SERO"},
                    new FlightsheetRecord(){Wallet="iZ2CFBHEsbjPYCWHns75LFT5NwNLc2i9hPTG1SwJjonyBnBUGCtdCdpKwBbo6KdZgH1Azg9vNcyLoBXBLEaz3HADLhncV4uBH9C2Ey4kyDSF",Ticker="ZANO"},
                    new FlightsheetRecord(){Wallet="0xd18c26638fcc3f2e9b806c8959f86438c5f4461d",Ticker="CTXC"},
                    new FlightsheetRecord(){Wallet="1514be666eb4ac2155183d32cb512ed689c47952090defd91e208b228d430949d92",Ticker="BEAM"},
                    new FlightsheetRecord(){Wallet="ak_JoojsyCeTQ5kAKSvoAKFG7s3PUQHhH4BpJNQcQB3y66cMacsb",Ticker="AE"},
                    new FlightsheetRecord(){Wallet="0xa0b29e3faa57056e97f163001f00b583cd497b674ae3cdbfc92cedcf5d12dc7b",Ticker="AION"},
                    new FlightsheetRecord(){Wallet="5t5mEm254JNJ9HqRjY9vCiTE8aZALHX3v8TqhyQ3TTF9VHKZQXkRYjPDweT9kK4rJw7dDLtZXGjav2z9y24vXCdRbybmW2eTRpM15NQZSa",Ticker="MSR"},
                    new FlightsheetRecord(){Wallet="kaspa:qp68tc8esnku9tsszq4m6z4zev3e2qtrdmc7cy76fjakkzu4luy5klhjl6pkd",Ticker="KAS"},
                    new FlightsheetRecord(){Wallet="ac8c2c19bac533c51fd4771a95d8bde73357e0efcd8c78416a377dbee33ead7daa7e0455b2f8",Ticker="SC"},
                    new FlightsheetRecord(){Wallet="hs1q08xlzh9xv62fjjw7a3keen2mp99pqxzchqxl88",Ticker="HNS"},
                    new FlightsheetRecord(){Wallet="bDAWf87BcCGzJK89xeLSiYVBYy29d95StQ",Ticker="LBC"},
                    new FlightsheetRecord(){Wallet="ckb1qzda0cr08m85hc8jlnfp3zer7xulejywt49kt2rr0vthywaa50xwsq0qp84vf0eha9hu2p04vs54s5vzqusph3crlx2en",Ticker="CKB"},
                    new FlightsheetRecord(){Wallet="1J1gPv9KGNLRrx5J57kJdVsSoEZX8ws25i",Ticker="RDX"},
                    new FlightsheetRecord(){Wallet="NQ191CXUABTDPDXLT5UMNU5PYL66936YVUVF", Ticker="NIM"},
                    new FlightsheetRecord(){Wallet="XnXP9jM2Lx97BDfKQfEN6KbFmL3hGYpz1Udfbx84KXg6VoBbpf1raJHTJ1fX5vUgo2Vhnm56FYhciS72Frs4XHc41QPrJzjtu", Ticker="AEON"}
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
