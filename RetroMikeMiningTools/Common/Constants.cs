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
        public static readonly string PLATFORM_NICEHASH_OS = "nicehash_os";
        public static readonly string PLATFORM_LINUX = "linux";
        public static readonly string PLATFORM_DOCKER_ARM64 = "Docker-linux/arm64";
        public static readonly string PLATFORM_DOCKER_AMD64 = "Docker-linux/amd64";
        public static readonly string PLATFORM_DOCKER_AMD64_V2 = "Docker-linux/amd64/v2";
        public static readonly string PLATFORM_DOCKER_AMD64_V3 = "Docker-linux/amd64/v3";
        public static readonly string PLATFORM_RPI = "raspberry_pi";

        public static readonly string PARAMETER_SERVICE_NAME = "service_name";
        public static readonly string PARAMETER_PLATFORM_NAME = "platform_name";
        public static readonly string MULTI_USER_MODE = "multi_user_mode";
        public static readonly string FLUX_MODE = "flux_mode";
        public static readonly string PARAMETER_MAX_USER_COUNT = "max_user_count";
        public static readonly string OVERRIDE_PORT = "override_port";
        public static readonly string RESET_DEVFEE = "reset_devfee";
        public static readonly string MULTI_USER_MODE_DONATION = "5%";

        public static readonly string TX_BIT_API_BASE_PATH = "https://api.txbit.io/api";
        public static readonly string TX_BIT_TRADE_FEE = "0.2%";
        
        public static readonly string TRADE_OGRE_API_BASE_PATH = "https://tradeogre.com/api/v1";
        public static readonly string TRADE_OGRE_TRADE_FEE = "0.2%";

        public static readonly string SOUTH_XCHANGE_TRADE_FEE = "0.3%";

        public static readonly string GRAVIEX_API_BASE_PATH = "https://graviex.net/webapi/v3";

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

                    new FlightsheetRecord(){Wallet="0x3F7fD3A4Db915b7a3261D0F84ec034657a4D18CB",Ticker="ETC"},
                    new FlightsheetRecord(){Wallet="GMNMKa92U8shcG5PmTVXeE2xgfBZy1PUJ2",Ticker="NEOX"},
                    new FlightsheetRecord(){Wallet="RRNju1AqFNTXkhwDnjLdEqQev9SJhfJ4cT",Ticker="RVN"},
                    new FlightsheetRecord(){Wallet="t1VgKU62C6psbGikgjCb7u7Kfv258hMiRqq",Ticker="FLUX"},
                    new FlightsheetRecord(){Wallet="AJ1C7FbZaaB6TNGpkBBBm46v4tz2ESR7Cc",Ticker="BTG"},
                    new FlightsheetRecord(){Wallet="t1bSYLj22UaSziDstTH9DFQ7z8fvqXSgoKB",Ticker="BTCZ"},
                    new FlightsheetRecord(){Wallet="vtc1qk38z8d038545jqf0zc9e8ypkz36v7u7xu8xykz",Ticker="VTC"},
                    new FlightsheetRecord(){Wallet="SX3D3rynfFYgQGQQgtr8gt6oCYPeFYArwh",Ticker="SIN"},
                    new FlightsheetRecord(){Wallet="87EntRscnvhbcgYFHHLjvxHjEfkeUA5ocEQRFbXRxCrthnTMJWAhv6aHBDQmhRthsdaWnawJvudtrTKx14D3fFV5JAaeGe8",Ticker="XMR"},
                    new FlightsheetRecord(){Wallet="9fusLPsxkMQNb1pYKN5TCT1Dbbv2x1SnvHr6wdJa2fxWUriD419",Ticker="ERG"},
                    new FlightsheetRecord(){Wallet="RYoNsBiFU6iYi8rqkmyE9c4SftzYzWPCGA3XvcXbGuBYcqDQJWe8wp8NEwNicFyzZgKTSjCjnpuXTitwn6VdBcFZEFXLcTgQy6gYsqRN4UVRvR",Ticker="RYO"},
                    new FlightsheetRecord(){Wallet="ccx7aoNYpGb7sndJtEDWvCBQhPAy9mC8QW5KWuCx8J1FJrDcDrER1XYA9LGtggrR7ZC4KfQmQ2uRN47L9WypBbNLAeq2Q4Q9WN.d027693e1a7b69639583a48fb0f4826c5d63d4cc9526beede3c109f5aac34835",Ticker="CCX"},
                    new FlightsheetRecord(){Wallet="Tsz59oveWtic6h1PHmiQwBRaJznK7a4QcEWqxPJRG5UgXFWLrsZPQou5CayqMEKenZSH1ocBLcREZY5MqpnpPmMw1Hmcq2a1LE",Ticker="XEQ"},
                    new FlightsheetRecord(){Wallet="abLoc7JNzYXijnKnPf7tSFUNSWBuwKrmUPMevvPkH4jc3b1K9LmS76DKpPamgQ5AYAC2CW9dJfTJ91AnXHYDNXAKRqPx5ZrtR49.cc9260956d08b7deab9098c130750fc90d9da4cd48c21f0de627f9914bc49293",Ticker="BLOC"},
                    new FlightsheetRecord(){Wallet="hvs1Mv7guY3PgQdo2PjRGk7tvVdneNxtL5SFkkYi9SPNNCrKUEkUVXd1sCFAR4wcW8fMzxAzQfhcggHCQxrTuVCL7bnX27aWeo",Ticker="XHV"},
                    new FlightsheetRecord(){Wallet="iZ2CFBHEsbjPYCWHns75LFT5NwNLc2i9hPTG1SwJjonyBnBUGCtdCdpKwBbo6KdZgH1Azg9vNcyLoBXBLEaz3HADLhncV4uBH9C2Ey4kyDSF",Ticker="ZANO"},
                    new FlightsheetRecord(){Wallet="369b749038e86e92f6971da94b6ed5b2b09bc742f2ab8fe95a752d2b103668b57ef",Ticker="BEAM"},
                    new FlightsheetRecord(){Wallet="5t5mEm254JNJ9HqRjY9vCiTE8aZALHX3v8TqhyQ3TTF9VHKZQXkRYjPDweT9kK4rJw7dDLtZXGjav2z9y24vXCdRbybmW2eTRpM15NQZSa",Ticker="MSR"},
                    new FlightsheetRecord(){Wallet="kaspa:qrf5628359yc3p5yflz460g24khanaecjd32xs3qmlryawtth3neumrjskhx7",Ticker="KAS"},
                    new FlightsheetRecord(){Wallet="RY59k1zNMGd3iK4TSk5ruLCk8rno1G66iu", Ticker="VRSC"},
                    new FlightsheetRecord(){Wallet="s1dPNXs4tV5rHBZGbbf5kC22zEXpXQ87gHK", Ticker="YEC"},
                    new FlightsheetRecord(){Wallet="M95ULQ4AUNERJ2aAH8Zq3dk2L6RK1xMnY2", Ticker="MEWC"},
                    new FlightsheetRecord(){Wallet="1MECmayTrsUyk6fUqegcVPtyHNK4htwJsh",Ticker="RXD"},
                    new FlightsheetRecord(){Wallet="XnXP9jM2Lx97BDfKQfEN6KbFmL3hGYpz1Udfbx84KXg6VoBbpf1raJHTJ1fX5vUgo2Vhnm56FYhciS72Frs4XHc41QPrJzjtu", Ticker="AEON"},
                    new FlightsheetRecord(){Wallet="bc1q2jwrfzv08km7pxds5m659yyzams5d6cglms7km",Ticker="OBTC"},
                    new FlightsheetRecord(){Wallet="grin1pv5ecu8mdv4kq9nyvclmmmmh7e539vpzswk4q0v3xtght79lq3ysprl7hm", Ticker="GRIN"},
                    new FlightsheetRecord(){Wallet="nexa:nqtsq5g5s2rp8e0v3vatjnqjwjxjn4v0qskdcltmhwywtss6", Ticker="NEXA"},
                    new FlightsheetRecord(){Wallet="Xwo5uSVAyHtaLVd12zTygAKSC48DukpVdCQi7vqc5WsYXtpzJySMFXtDu7Vy2GqWC9cMtfePPu4ZeAB8BsPC9AwZ1PmLmbXur.b85de8b9a2619e53cee581cf5d05b050e4200c9221d6c9da260bfcbe8a1e0f2e", Ticker="DNX"},
                    new FlightsheetRecord(){Wallet="deroi1qyzlxxgq2weyqlxg5u4tkng2lf5rktwanqhse2hwm577ps22zv2x2q9pvfz92xe2kc98kqr5n38s4wl7lp", Ticker="DERO"},

                    new FlightsheetRecord(){Wallet="Eg59kTJ2dm79KraqgiEVqdAWAGv3LdRLfo",Ticker="EVR"},
                    new FlightsheetRecord(){Wallet="PFNwaWcq3v6rNEk4TYdtErGKLeNpXewHiV",Ticker="PGN"},
                    new FlightsheetRecord(){Wallet="1CTF5ZAzrSV2omBcZdeTV8CPyTz9R6DAEtzxNqUf6mkWw", Ticker="ALPH" },

                    //new FlightsheetRecord(){Wallet="",Ticker="NOVO"},
                    //new FlightsheetRecord(){Wallet="", Ticker="NIM"},
                    //new FlightsheetRecord(){Wallet="",Ticker="AION"},
                    //new FlightsheetRecord(){Wallet="",Ticker="AE"},
                    //new FlightsheetRecord(){Wallet="",Ticker="CTXC"},
                    //new FlightsheetRecord(){Wallet="",Ticker="SERO"},
                    //new FlightsheetRecord(){Wallet="",Ticker="QKC"},
                    //new FlightsheetRecord(){Wallet="cfx:",Ticker="CFX"},
                    //new FlightsheetRecord(){Wallet="",Ticker="UBQ"},
                    //new FlightsheetRecord(){Wallet="",Ticker="FIRO"},
                    //new FlightsheetRecord(){Wallet="",Ticker="ETHW" },
                    //new FlightsheetRecord(){Wallet="",Ticker="ETHF"},
                    //new FlightsheetRecord(){Wallet="",Ticker="ETHO"},
                    //new FlightsheetRecord(){Wallet="",Ticker="EXP"},
                    //new FlightsheetRecord(){Wallet="",Ticker="CLO"},
                    //new FlightsheetRecord(){Wallet="",Ticker="EGEM"},

                    new FlightsheetRecord(){Wallet="DJm56pVkWqQj26A2UFeafwFw1bY632trLY",Ticker="DOGE", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="ltc1qgpks9d0tjfucx9x9wqu4j099hqxfkygmug84qm",Ticker="LTC", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="dgb1q6k7sxkwku0nn9tch8vu4arxg63q7l70hwsw2cp",Ticker="DGB-Scrypt", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="DQRhSWGMSv2SJ4SrZWQg8bYGmAojrzTvKV",Ticker="XVG-Scrypt", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="ERz6GRmPbtB2HumzxMVGftwnVcr2Et4kxX",Ticker="VIA", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="bDAWf87BcCGzJK89xeLSiYVBYy29d95StQ",Ticker="LBC", AsicPool="stratum+tcp://lbry.mine.zergpool.com:3334", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="ckb1qzda0cr08m85hc8jlnfp3zer7xulejywt49kt2rr0vthywaa50xwsq0qp84vf0eha9hu2p04vs54s5vzqusph3crlx2en",Ticker="CKB", AsicPool="stratum+tcp://mining.viabtc.io:3001", Password="123", AsicWallet="05sonicblue.donation"},
                    new FlightsheetRecord(){Wallet="ac8c2c19bac533c51fd4771a95d8bde73357e0efcd8c78416a377dbee33ead7daa7e0455b2f8",Ticker="SC", AsicPool="stratum+tcp://us-east.siamining.com:3333", AsicPassword="x", AsicWallet="ac8c2c19bac533c51fd4771a95d8bde73357e0efcd8c78416a377dbee33ead7daa7e0455b2f8.donation" },
                    new FlightsheetRecord(){Wallet="hs1q08xlzh9xv62fjjw7a3keen2mp99pqxzchqxl88",Ticker="HNS", AsicPool="stratum+tcp://mining.viabtc.io:3008", AsicPassword="123", AsicWallet="05sonicblue.donation"},
                    new FlightsheetRecord(){Wallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078",Ticker="Zerg-Scrypt", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="05sonicblue",Ticker="Prohashing-Scrypt", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="05sonicblue",Ticker="MiningDutch-Scrypt", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="3BAocmSZNqmiCkrLsPjYmbTMwwfp7aV29U",Ticker="Nicehash-Scrypt", AsicPool="stratum+tcp://scrypt.mine.zergpool.com:3433", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="bDAWf87BcCGzJK89xeLSiYVBYy29d95StQ",Ticker="Zerg-lbry", AsicPool="stratum+tcp://lbry.mine.zergpool.com:3334", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                    new FlightsheetRecord(){Wallet="bDAWf87BcCGzJK89xeLSiYVBYy29d95StQ",Ticker="MiningDutch-lbry", AsicPool="stratum+tcp://lbry.mine.zergpool.com:3334", AsicPassword="c=BTC,ID=donation", AsicWallet="bc1q6m0r7u2ux542grhrp0zqtv4k6hl0ql99ev5078"},
                };
            }
        }

        public static List<Promos> PROMO_DATA
        {
            get
            {
                return new List<Promos>()
                {
                    new Promos(){ Code="52454450414e44414d494e494e47", DonationPercentage="0%", CutoffDate=DateTime.Parse("2024-01-01")},
                    new Promos(){ Code="4e4554574f524b42495453", DonationPercentage="0%", CutoffDate=DateTime.Parse("2024-01-01")},
                    new Promos(){ Code="4d49534649544d494e494e47", DonationPercentage="0%", CutoffDate=DateTime.Parse("2024-01-01")},
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
