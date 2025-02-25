# Table of Contents
  * [Summary](#retro-mike-mining-tools)
  * [Features](#features)
  * [Profit Switching](#profit-switching)
    + [Mining Modes](#mining-modes)
    + [Supported Coins](#supported-coins)
    + [Pinned Coin](#pinned-coin)
  * [Auto Exchanging](#auto-exchanging)
    + [Configuration](#configuration)
  * [Docker Installer](#docker-installer)
  * [Screenshots](#screenshots)
    + [Dashboard](#dashboard)
    + [Mining Groups](#mining-groups)
    + [Hive OS Rigs](#hive-os-rigs)
    + [Auto Exchanging](#auto-exchanging-1)
  * [Videos](#videos)  
  * [Support Me](#support-me)
  * [Support](#support)
  * [Roadmap](#roadmap)
  * [Release Notes](#release-notes)


# Retro Mike Mining Tools

Overview Video: https://youtu.be/Yp5bNBCuqQs

This is a web and service based app that can connect to your Hive OS account (or ASIC's on your local network), profit switch your mining rigs based on WhatToMine calculations, and auto-exchange coins on your exchange accounts, using a configuration that you setup. 

This app was designed to be installed on a server on a Hive OS Rig on your network (or on any server on your network that is always running). If you are unable to host yourself due to network restrictions, there is a hosted version that can be used.

## Features
- Web based management and monitoring dashboard 
- Hive OS Rig Profit Switching
- IceRiver Profit Switching when running the PBFarmer OC Firmware
- Built-In Scheduler that gives you full control over how frequently it tries to profit switch
- Auto Exchanging of coins utilizing your own exchange accounts
- Calculations for any combination of up to 2 coins (i.e., GPU+CPU rig or just a mixed GPU rig)

## Profit Switching
One of the most popular and powerful features is the ability to have the application automatically switch coins that you are mining. There are several modes available. While profit switching mode is the most popular, there are also coin stacking and coin diversification modes as well

### Mining Modes
The following mining modes are supported and can be set on each rig / ASIC  
- Profit - Mine whichever coin / algo has the best current profitability based on current difficulty, network hashrates, coin price, and power consumption
- CoinStacking - Mine whichever coin will net you the most coins based on current difficulty and network hashrates
- DiversificationByProfit - Mine whichever coin has the best current profitability based on current difficulty, network hashrates, coin price, and power consumption until you have obtained a desired amount of the coin and then move onto the next most profitable coin. 
- DiversificationByCoinStacking - Mine whichever coin will net you the most coins based on current difficulty and network hashrates until you have obtained a desired amount of the coin and then move onto the next most netable coin
- ZergPoolAlgoProfitBasis - Auto switch algo's based on Zerg Pool real-time calculations. Your flightsheets can be for any pool, but the calculations will be based off of Zerg Pool.

### Supported Coins
- Any coin listed on WhatToMine under the GPU or ASIC sections
- All algo's on ZergPool
- All algo's on ZPool
- All algo's on Nicehash that are also listed on WhatToMine
- All algo's on Prohashing
- All algo's on Mining-Dutch
- All algo's on Unmineable

### Pinned Coin
You can pin a coin per rig if you want that coin to over-ride current profitability and always mine. This can be useful for maintaining your configuration, but wanting to mine something like a newly released coin to accumulate as many coins as possible early-on.

### Additional Power
When configuring your Hive OS Rig, you can specify an "Additional / Misc Power" in watts. The power cost will then be calculated and factored into profitability of each coin. This is usefull if you have a A/C Unit, Window Fan, Box Fan, or additional items hooked up to your rig and you want to deduct that cost from profitability figures

### Coin Configuration
- When configuring a coin, if it's a Prohashing or Zerg coin, then set the Hashrate in the MH equivalent and set the system power. Make sure your power cost is set in the Core Configuration screen
- When configuring a coin from WTM, you can either have it use the generalized WTM Endpoint from the rig config, or set the individual WTM endpoints for the primary and secondary coins. If you do only override endpoints, you don't need to set one at the rig level
- Always set the power to the total system power
- When using override, leave hashrate blank as it will be overwritten with what is defined in the WTM Override Endpoints

### Dev Fee
There is no Dev Fee as of version 3.0.0.0. If you are running an older version, please upgrade.

## Auto Exchanging
Another powerful feature is the ability to have the application auto-exchange your mined crypto into the coins you wish to hold. If you are mining to an exchange, this is a great way to mine and exchange at minimal fees. The following exchanges are currently supported

- CoinEx
- Kucoin
- TradeOgre
- Xeggex

### Configuration
- Destination - This is the ultimate coin that you want to exchange into.
- Exclusions - These are coins that you don't want to auto exchange from
- Trading Pair - This is the coin that may be needed to trade through to get to your destination coin. i.e., you should set this to BTC if you needed to go ETH -> BTC -> LTC. In most cases, this would either be BTC or USDT, since those are the most common pricing currencies on exchanges
- Api Key - API Key you can get from your account settings on the exchange
- Api Secret - API Secret Key you can get from your account settings on the exchange
- Passphrase - Some exchanges, like Kucoin, also require a passphrase in addition to your API Keys
- Auto Move To Trading Account - For exchanges that have a main and trading account, like Kucoin, setting this on will auto move your assets from your main account into your trading account prior to executing the auto exchanging
- Auto Withdrawl - Enables / Disables the auto withdrawl feature. This is dependent on the Enabled flag also being set.
- Auto Withdrawl Address - Address to auto withdraw to
- Withdrawl Coin - Coin on the exchange to auto withdraw
- Auto Withdrawl Min - Minimum amount need to auto withdraw selected coin
- Withdrawl Fee - Fee amount to deduct from the withdrawl amount. This is needed for some exchanges.
- Enabled - Enables the auto exchanging feature.

### Auto Withdrawls
The system can auto withdraw your wallet balance to a defined wallet address once a threshold is met. This is supported by most exchanges. Most exchanges will auto-detect the fee, but for some exchanges, you may need to indicate the withdrawl fee

Notes:
- TradeOgre's API doesn't support auto-withdrawls at this time.
- Kucoin requires IP Whitelist setup in order to auto withdrawl
- CoinEx requires the withdrawl address to be whitelisted in the API section of your profile

## Docker Installer
#### Parameters
- Volume
    - /app/db: This mapped volume is where your db will be stored. Make sure it's mapped so that it persists across container restarts
- Environment
    - TZ: This is optional and can be set to your local timezone. If you don't include it, it will default to GMT

You can install the mining tools via a docker container. 

You may specify the "TZ" environment variable to have the container use your local timezone instead of GMT.
i.e.: 
```
sudo docker pull theretromike/miningtools:single_user && sudo docker run -d --name RetroMikeMiningTools -p 8080:7000 -e TZ=America/Chicago -v /volume0/retromikeminingtools:/app/db --restart always theretromike/miningtools:single_user
```

### Single-User Mode (Preferred)
You can install the normal app in single user mode via docker with the following command.
```
sudo docker pull theretromike/miningtools:single_user && sudo docker run -d --name RetroMikeMiningTools -p 8080:7000 -v /volume0/retromikeminingtools:/app/db --restart always theretromike/miningtools:single_user
```

### Multi-User Mode (No SSL)
You can install the hosted version in multi user mode via docker with the following command. This should only be used if you need login capabilities
```
sudo docker pull theretromike/miningtools:multi_user && sudo docker run -d --name RetroMikeMiningTools -p 8080:7000 -v /volume0/retromikeminingtools:/app/db --restart always theretromike/miningtools:multi_user
```

### Multi-User Mode (SSL)
You can install the hosted version in multi user mode via docker with the following command. This should only be used if you need login capabilities and SSL
```
sudo docker pull theretromike/miningtools:multi_user && sudo docker run -d -it --name RetroMikeMiningTools -p 9090:7000 -p 9091:7001 -v /volume0/retromikeminingtools-multi:/app/db -v /volume0/certs:/certs --restart on-failure:3 --entrypoint /bin/sh theretromike/miningtools:multi_user -c "dotnet RetroMikeMiningTools.dll --platform_name=Docker-linux/amd64 --multi_user_mode=true --cert=/certs/xxx.pfx --cert_pwd=xxx --max_user_count=100"
```

## Screenshots
### Dashboard
![image](https://user-images.githubusercontent.com/1271856/191880384-08ca2b4a-d584-4bb2-a5c2-b3b479b355f0.png)

### Mining Groups
![image](https://user-images.githubusercontent.com/1271856/191880556-b12059a5-5cca-4736-b0ac-1f62d2553c79.png)

### Hive OS Rigs
![image](https://user-images.githubusercontent.com/1271856/191880644-9307603d-93f6-44b9-9df4-bd0a89cd3ce3.png)

### Auto Exchanging
![image](https://user-images.githubusercontent.com/1271856/191880955-688ceabe-7ba7-4490-8e44-541e682b8d08.png)

## Videos
- Overview: https://youtu.be/Yp5bNBCuqQs

## Support Me
If you would like to support me:
- Youtube Channel: https://www.youtube.com/retromikecrypto
- Amazon Affiliate Link: https://amzn.to/3hIOvhP
- Ebay Affiliate Link: https://ebay.us/jvqlUf
- Donate in Crypto:
    - BTC
```31q6x9Vp9J2BJ8rTnW4F8aP744CEAScFN5```
    - BCH
```bitcoincash:qzczcn98zusq77fk6jq744xu0u8jlrd6su930qk7x7```
    - LTC
```MAhooUHqeTdhJoMEjbgTwFSqhZmbzyko83```
    - DOGE
```DSQLL3m5B1BwZa7jaNnoYaXHVeT9cAHUvd```
    - ETH / POL / BNB
```0xde6b4E548d71459Af5041dA71883AEA62426e68E```
    - KAS
```kaspa:qzs36kutqphrqzwnl34zd36wqtsr97dvy6np83ugqac75zjgvsy7qgk4yr722```
    - SOL
```AsHA1y22XnYf3SwP6g5iGSvJhMcSGaBtEQbzcSccPkJ4```

## Support
This is an open-source project and isn't officially supported, but if you have questions you can ask them on Discord and possibly get support from a community member: https://discord.gg/WSefezcjg3

## Roadmap
TBD
