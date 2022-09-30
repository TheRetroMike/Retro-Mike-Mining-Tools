# Table of Contents
  * [Summary](#retro-mike-mining-tools)
  * [Features](#features)
  * [Profit Switching](#profit-switching)
    + [Mining Modes](#mining-modes)
    + [Supported Coins](#supported-coins)
    + [Pinned Coin](#pinned-coin)
  * [Auto Exchanging](#auto-exchanging)
    + [Configuration](#configuration)
  * [Hive OS Installer](#hive-os-installer)
  * [Raspberry Pi Installer](#raspberry-pi-installer)
  * [Docker Installer](#docker-installer)
  * [Screenshots](#screenshots)
    + [Dashboard](#dashboard)
    + [Mining Groups](#mining-groups)
    + [Hive OS Rigs](#hive-os-rigs)
    + [Goldshell ASIC's](#goldshell-asics)
    + [Auto Exchanging](#auto-exchanging-1)
  * [Support Me](#support-me)
  * [Support](#support)




# Retro Mike Mining Tools
This is a web and service based app that can connect to your Hive OS account or Goldshell ASIC's, profit switch your mining rigs based on WhatToMine calculations, and auto-exchange coins on your exchange accounts, using a configuration that you setup. 

## Features
- Web based management and monitoring dashboard 
- Hive OS GPU Rig Profit Switching
- Goldshell ASIC Profit Switching and auto algo switching
- Built-In Scheduler that gives you full control over how frequently it tries to profit switch
- Auto Exchanging of coins utilizing your own exchange accounts

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
- All algo's on Nicehash that are also listed on WhatToMine
- All algo's on Prohashing

### Pinned Coin
You can pin a coin per rig if you want that coin to over-ride current profitability and always mine. This can be useful for maintaining your configuration, but wanted to mine something like a newly released coin to accumulate as many coins as possible early-on.

## Auto Exchanging
Another powerful feature is the ability to have the application auto-exchange your mined crypto into the coins you wish to hold. If you are mining to an exchange, this is a great way to mine and exchange at minimal fees. The following exchanges are currently supported

- CoinEx
- Kucoin
- SouthXchange
- TradeOgre
- TxBit

### Configuration
- Destination - This is the ultimate coin that you want to exchange into.
- Exclusions - These are coins that you don't want to auto exchange from
- Trading Pair - This is the coin that may be needed to trade through to get to your destination coin. i.e., you should set this to BTC if you needed to go ETH -> BTC -> LTC. In most cases, this would either be BTC or USDT, since those are the most common pricing currencies on exchanges
- Api Key - API Key you can get from your account settings on the exchange
- Api Secret - API Secret Key you can get from your account settings on the exchange
- Passphrase - Some exchanges, like Kucoin, also require a passphrase in addition to your API Keys
- Auto Move To Trading Account - For exchanges that have a main and trading account, like Kucoin, setting this on will auto move your assets from your main account into your trading account prior to executing the auto exchanging

## Hive OS Installer
Install Video / Walkthrough: https://youtu.be/A3J7Ax6jtlk

```
wget -O - https://raw.githubusercontent.com/TheRetroMike/Retro-Mike-Mining-Tools/main/hive_install.sh | bash
```

## Raspberry Pi Installer
This may work on other ARM platforms utilizing debian based linux like Ubuntu, but has only been tested on a Raspberry Pi 4 running 64-Bit Ubuntu

If you are using a raspberry pi, this is the preferred install method as it gives you access to the full functionality

```
wget -O - https://raw.githubusercontent.com/TheRetroMike/Retro-Mike-Mining-Tools/main/rpi_install.sh | bash
```

## Docker Installer
You can install the mining tools via a docker container. 

All functionality is available on x64 systems. 

If you are running an ARM-based device, such as a raspberry pi, you won't have access to the Goldshell ASIC functionality, but everything else will be fully functional

```
sudo docker pull theretromike/miningtools && sudo docker run -d --name RetroMikeMiningTools -p 8080:7000 -v /volume0/retromikeminingtools:/app/db --restart always theretromike/miningtools
```

## Screenshots
### Dashboard
![image](https://user-images.githubusercontent.com/1271856/191880384-08ca2b4a-d584-4bb2-a5c2-b3b479b355f0.png)

### Mining Groups
![image](https://user-images.githubusercontent.com/1271856/191880556-b12059a5-5cca-4736-b0ac-1f62d2553c79.png)

### Hive OS Rigs
![image](https://user-images.githubusercontent.com/1271856/191880644-9307603d-93f6-44b9-9df4-bd0a89cd3ce3.png)

### Goldshell ASICs
![image](https://user-images.githubusercontent.com/1271856/191880727-3ad72a5d-7039-4bd4-b553-a37298cc7870.png)

### Auto Exchanging
![image](https://user-images.githubusercontent.com/1271856/191880955-688ceabe-7ba7-4490-8e44-541e682b8d08.png)

## Hosted Version
http://tools.retromike.net/

## Support Me
If you would like to support me:
- Youtube Channel: https://www.youtube.com/retromikecrypto
- Community Mining Pool: https://retromike.net
- Just run the profit switcher with any donation amount

## Support
This is an open-source project and isn't officially supported, but if you have questions you can ask them on Discord: https://discord.gg/HsjJPCP2hp
