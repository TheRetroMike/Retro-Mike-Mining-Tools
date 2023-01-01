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
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="ETHW" },
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="ETHF"},
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="ETHO"},
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="EXP"},
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="CLO"},
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="EGEM"},
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="ETC"},
                    new FlightsheetRecord(){Wallet="GSVvWv2WNcLY7v9g9ajtxvBdtJF5eYD5DF",Ticker="NEOX"},
                    new FlightsheetRecord(){Wallet="RHFmjSnmyFAYJe9ZK8uAbYzcJfzQmeQs3N",Ticker="RVN"},
                    new FlightsheetRecord(){Wallet="a8h3PqM7bDMpJBDTrNEBGJyv9mfAPwnEbn",Ticker="FIRO"},
                    new FlightsheetRecord(){Wallet="t1RrBfGKdLk9ZqGqFnPjAdqmKo4itxXmUso",Ticker="FLUX"},
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="UBQ"},
                    new FlightsheetRecord(){Wallet="cfx:0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="CFX"},
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="QKC"},
                    new FlightsheetRecord(){Wallet="GRpW54ESMGyGK75emua9vo1JTaKfCndLXp",Ticker="BTG"},
                    new FlightsheetRecord(){Wallet="t1RrBfGKdLk9ZqGqFnPjAdqmKo4itxXmUso",Ticker="BTCZ"},
                    new FlightsheetRecord(){Wallet="VhyQZe4wnEGBJQgWakZfvvzGEDpnf2nJxi",Ticker="VTC"},
                    new FlightsheetRecord(){Wallet="SVGagmge6nZAkwZpPPu83voyCBmEtFbbHp",Ticker="SIN"},
                    new FlightsheetRecord(){Wallet="43xbC5JRd2nGBbhyFphRDeEaVf762EhuweCHTXuUKvAMQYMJjvgNP4Y5uNe7BhgcLNeEgHihxB31ZM3pagzZ8p6hPg1PkZY",Ticker="XMR"},
                    new FlightsheetRecord(){Wallet="9hNq9HmYDCvrM7QHLGMdrBKdrS1C1V7JC9Hhz2iKmD326doz9pL",Ticker="ERG"},
                    new FlightsheetRecord(){Wallet="RYoLsg58s1ZSndWXskYuJN8ES4763dWgrVddAgohAWiUWcUnPk3pwczb4t9yaMRn1ibr1iPZKtZz4hJ7yyoz2rpzYNbUTwhXGL6",Ticker="RYO"},
                    new FlightsheetRecord(){Wallet="ccx7FncKReNgdrg6hupLdScavLbJDAQj3eP6F8rJwjTS7qK5N9EKHQoY5yDheh5dGJeouCugiUDd1JnwKpNXK9Ft9w7mmsWRSM",Ticker="CCX"},
                    new FlightsheetRecord(){Wallet="Tvz1PooTeDJ9854nqBSNbQZe8wZsc2vEx1DBCGQeUXvo9GcXFrrH2V9RWSPXYDeTHPMv2ASEkytbxWnvbT5MvrKC1mHLCXUkx",Ticker="XEQ"},
                    new FlightsheetRecord(){Wallet="abLocB84uxkL9ct2U4uVNE9A5kzV6k9KcgUuPuXUKLvK4PyskroAoTTL4sAnwueowWXDNnFxnKkEYEHuL1M3S52mgGHZiu1h7ue",Ticker="BLOC"},
                    new FlightsheetRecord(){Wallet="hvxy27z5sxnWQ9zjsSMvhUKiWGRW9DyEhANgQ5S6wbqzHtLyUNBxQobbH2omWigMiPiAQn9YAYmW1EW6SkAf7ZZL3EpK7jswgD",Ticker="XHV"},
                    new FlightsheetRecord(){Wallet="acaR31MyP4Tbhdq4wtRgogsWbwAVa3uZrjjt89bYBZvhp1tJ9dnTfDjWg2UMcVYJcGJdeUFKmhDDkidSmWNX5KaFp7Vvb2eBXaREirVWaFzpxhvRdM5z7W3xpupgofVip6z",Ticker="SERO"},
                    new FlightsheetRecord(){Wallet="ZxCFd3Lo78M2W34imfg3hXSRttPxLqLQYAH4pgVu6ggQU5ZGkL5p3VjYVoLqU6EPJTgzFLdEeyiBZWZ3CZeB58yf2C3bLvPxr",Ticker="ZANO"},
                    new FlightsheetRecord(){Wallet="0x3aaf2E19C1b7A14228b150CB4449a7BDc2Fb1f94",Ticker="CTXC"},
                    new FlightsheetRecord(){Wallet="9DfkirZYqjTSuHBGYKxb3ifi5PrL7KmkYQYDQb16QAxcZpotW8DeF8ECbp5EWCe7xfWSZyCsiowv2iVVjS3yy5bkZW7YKsdeJdaD2QSNxhVHyBmsYGoye4qt8owKUaqQHRZwUQWY7UzbH9cotsy8tUYWqBSDS7FtHHvjGm7vM2DmBhgPgyPbFmZbjWCqDVfJ6tKKuTADxHZVekxc3ghSWmuRWRYapjuQ6bhtjJLPC2zdgBjLz6PeLj6DXYtbKn6MZVZXKm5bJ3MxnoyvwhNRDba29DGBKZUirz6xwEejKVJFpGDyyTcY9V8o27EaTHevbktFsjhgjGX5T4tCcAiT867Uzm5zueRBSPDpNakT8dWteVctTnvnryFyoSm1fNsnNUCUufSLtUqN2VcLZaWcHrF4Gdq3tPrHF5E9UgJeYVXhszvmyiQuPeRpLQS6qwaWybgeWcsX76Pzqnvz4k",Ticker="BEAM"},
                    new FlightsheetRecord(){Wallet="ak_2WHmY5kB5X3nwUKQd5bJYGTtEaanXCp7u31fYzzdZEEBHao1mc",Ticker="AE"},
                    new FlightsheetRecord(){Wallet="0xa0ac3e53f22482b85d0834f9233422759f25b1a27294e5866cdcce515b631646",Ticker="AION"},
                    new FlightsheetRecord(){Wallet="5iKD4gbCS1ND3gQ4PtazLhgV8bYU6GXLYKxPTJURK4xiF9U33UfdfQ8gHZdd5oJY9nECwuL8Ezt9TX21b2m6RW8fUGv4Dhy",Ticker="MSR"},
                    new FlightsheetRecord(){Wallet="kaspa:qrmw7ap2a2qyenqwsuguv3yx052wgawf6cg8ag57l6auaw0rjj9kwzyhd0sfy",Ticker="KAS"},
                    new FlightsheetRecord(){Wallet="RKndbQJ2CQBSVz2nh587boizfV8dEeJeud", Ticker="VRSC"},
                    new FlightsheetRecord(){Wallet="ys1pgrdn0e9pyhc72s4rrzjutxdtuv2fjlfu367c2gvugre7xs4h28kqw4ymc9mnl7smer2wls0hgh", Ticker="YEC"},
                    new FlightsheetRecord(){Wallet="MA5rj8T523MKmM4Gou3D91pPBkhfEgvsMU", Ticker="MEWC"},
                    new FlightsheetRecord(){Wallet="1Kdt4HqAmVfT6t5w6EtBMLGwMZ76Zb1hzd",Ticker="RDX"},
                    new FlightsheetRecord(){Wallet="1Kdt4HqAmVfT6t5w6EtBMLGwMZ76Zb1hzd",Ticker="RXD"},
                    new FlightsheetRecord(){Wallet="NQ48 1X83 A5EQ BGMX 0UT3 YKQX C1CJ JP9T 37LU", Ticker="NIM"},
                    new FlightsheetRecord(){Wallet="Wmu1FBx1a96ezsJD24eyuWcCHXcJe15FgJ4ihipQyQoHMuT5C2DfnXBcWvu2ebnx8bU6DiwvEnJ4qWdoC3PrL4MQ1tytT4ETs", Ticker="AEON"},
                    new FlightsheetRecord(){Wallet="bc1qp4j2cqxj29ec9vfl3jnd0t8zxczn37dm4effwm",Ticker="OBTC"},
                    new FlightsheetRecord(){Wallet="EZCVMN2ZRMtQP1YddkgTZjB3wJtK2oPyw6",Ticker="EVR"},
                    new FlightsheetRecord(){Wallet="1GikdAT4gkWGVt2wTEAQkLorphhQDMs9yn",Ticker="NOVO"},
                    new FlightsheetRecord(){Wallet="grin1yqh66h3cwaqsaspp6h7px2ycpe3yysdca57zv6ghsekzl0vq6grqh4zllt", Ticker="GRIN"},


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
                    //new Promos(){ Code="484f4c494441593232", DonationPercentage="0.00%", CutoffDate=DateTime.Parse("2023-01-01")},
                    //new Promos(){ Code="474f4c44454e4841574b", DonationPercentage="0%", CutoffDate=DateTime.Parse("2023-04-15")}
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
