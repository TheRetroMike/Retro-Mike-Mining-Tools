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
  * [Roadmap](#roadmap)
  * [Release Notes](#release-notes)


# Retro Mike Mining Tools
This is a web and service based app that can connect to your Hive OS account or Goldshell ASIC's, profit switch your mining rigs based on WhatToMine calculations, and auto-exchange coins on your exchange accounts, using a configuration that you setup. 

This app was designed to be installed on a server on a Hive OS Rig on your network (or on any server on your network that is always running). If you are unable to host yourself due to network restrictions, there is a hosted version that can be used.

## Features
- Web based management and monitoring dashboard 
- Hive OS GPU Rig Profit Switching
- Goldshell ASIC Profit Switching and auto algo switching
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
- All algo's on Nicehash that are also listed on WhatToMine
- All algo's on Prohashing
- All algo's on Mining-Dutch

### Pinned Coin
You can pin a coin per rig if you want that coin to over-ride current profitability and always mine. This can be useful for maintaining your configuration, but wanted to mine something like a newly released coin to accumulate as many coins as possible early-on.

### Coin Configuration
- When configuring a coin, if it's a Prohashing or Zerg coin, then set the Hashrate in the MH equivalent and set the system power. Make sure your power cost is set in the Core Configuration screen
- When configuring a coin from WTM, you can either have it use the generalized WTM Endpoint from the rig config, or set the individual WTM endpoints for the primary and secondary coins. If you do only override endpoints, you don't need to set one at the rig level
- Always set the power to the total system power
- When using override, leave hashrate blank as it will be overwritten with what is defined in the WTM Override Endpoints

### Donation / Dev Fee
There is a Dev Fee of 1%. The system will auto generate a profit switching flightsheet that matches your settings, but points to the DEV wallet. To ensures system stability. To prevent frequent hopping, the process runs once per day for approx 15 minutes and then switch back to your wallet.

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

### Flux
For Flux, please use the RetroMikeMiningTools App or just use the hosted environment at https://retromike.net

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

### Goldshell ASICs
![image](https://user-images.githubusercontent.com/1271856/191880727-3ad72a5d-7039-4bd4-b553-a37298cc7870.png)

### Auto Exchanging
![image](https://user-images.githubusercontent.com/1271856/191880955-688ceabe-7ba7-4490-8e44-541e682b8d08.png)

## Hosted Version
https://retromike.net

## Support Me
If you would like to support me:
- Youtube Channel: https://www.youtube.com/retromikecrypto
- Hosted Version: https://retromike.net
- Just run the profit switcher

## Support
This is an open-source project and isn't officially supported, but if you have questions you can ask them on Discord and possibly get support from a community member: https://discord.gg/HsjJPCP2hp

## Roadmap
- Add Additional / Misc power on rig and auto-detect in profit on each coin
- Auto Update
- Manually Apply Flightsheet
- Manually Apply Goldshell ASIC Config
- Exbitron Exchange
- Graviex Exchange
- 6Block Exchange
- Dove Wallet / BTX Exchange
- Hotbit Exchange
- Altmarkets Exchange
- Asymetrex Exchange
- SafeTrade Exchange
- Garlix Exchange
- iPollo G1 Mini Asic Profit Switching
- Login Page for Single User Mode


## Release Notes

v2.5.9

Added check to the profit switching and donation jobs to make sure the Hive API is working and that the API Key is valid.
This will make sure that when user multi user mode that profit switching stops if the user inactivates the API key in the hive dashboard

Also added SSL Cert configuration for Multi-User Mode.

----------------------------------------------

v2.5.8

- Optimized WhatToMine calls from the Rig and Goldshell Pages
- Various Performance Optimizations
- Removed Profit Viewing on the Multi User Mode UI due to WTM DDoS Implementation
- Updated Donation / Dev Fee to process only once per day instead of 4 times per day
- Added Max User Count Config for Multi User Mode

Note: Donation processing will now run in one block every 24 hours instead of a tiny block every 6 hours.
In addition, donation will use the same pool and miner config with the only difference being the wallet address.
This should help with any pool hopping monitoring and custom device specific flightsheets

----------------------------------------------

v2.5.7

- Added Smart Plug support for the TP-Link KASA line of smart plugs. Tested with a KP115. If profitability drops below your defined threshold, then the app can turn off the smart plug. It will automatically turn it back on whenever profitability goes above your defined minimum. You can have one KASA device configured per rig. Be sure to give it a static IP and then set the IP in the Smart Plug Host field

Note that this is currently implemented for single plugs and doesn't work with the KASA power strips yet. The KP115 I used for testing is rated for 1800 watts. Be cautious and mindful of the wattage of your rig when using this.

----------------------------------------------

v2.5.6

- Performance Optimizations for all APIs
- Donation processing Optimizations

----------------------------------------------

v2.5.4

- Improved Performance of Hive OS Rigs page when there are a large number of users
- Added WTM coin caching to limit WTM calls

----------------------------------------------

v2.5.3

Added Coindesk API Caching and fixed issue with Hive OS Rigs page not loading properly on multi-user mode
This release is mainly an attempt to handle a large number of users on the hosted platform

----------------------------------------------

v2.5.2

Removed Log Purging for Multi User Mode

----------------------------------------------

v2.5.1

Fixed Conflicting Coin Listings

----------------------------------------------

v2.5.0

- Added New Coin Providers and Comprehensive List of Coins
- Added Primary and Secondary Coins for determining dual mining profitability
- Added ability to specify an over-ride WTM json url for primary and secondary coins. If not specified, it will try to determine profitability by hashrate and using available coin providers. This can be used both on Hive Rigs and Goldshell ASICs
- Added Purge Log Feature to purge all log entries
- Added Mining-Dutch
- Revamped Donation Process

----------------------------------------------

v2.4.0

- Added view of all balances currently on each configured exchange
- Fixed ZergPool Donation Processing
- Fixed Nicehash Donation Processing
- Fixed ProHashing Donation Processing

----------------------------------------------

v2.3.1

Fixed issue where if a rig didn't have a WhatToMine endpoint, it wasn't processing the Zerg and/or Prohashing profit switching routine

----------------------------------------------

v2.3.0

- Added ZergPool into the core Profit Switching Mode
- Added Prohashing into the core Profit Switching Mode
With this release Zergpool has been integration into the base ProfitSwitching mode and so the extra Zerg mining mode will be removed in a future release.

----------------------------------------------

v2.2.1

Fixed donation calculation for imported rigs

----------------------------------------------

v2.2.0

Added multi-user support for a hosted environment. No need to apply this upgrade. This is being released for a Community Portal. Links in Discord and YouTube soon.

----------------------------------------------

v2.1.1

Quick fix to an issue where if a coin is pinned and isn't on WhatToMine, it was causing the coin grid not to show

----------------------------------------------

v2.1.0

- Added Real-Time Profit calculations to Coin and Algo grids
- Fixed calculation of Zerg Algo where it was off by a multiple of 1000. This doesn't impact differences between coins, just is a fix to reflect accurate values when looking at the grids or on the logs
- Fixed issue where Zerg Algo changes weren't being logged in the Dashboard view

----------------------------------------------

v2.0.0

- Added ZergPool algo profit switching option. Just set the mode and then define the algo's, your hashrate, and power per algo and it will profit switch based on real-time ZergPool calculations
- Updated amd64 Docker Image
- Added arm64 Docker Image with core functionality
- Added amd64v2 Docker Image with full functionality
- Added amd64v3 Docker Image with full functionality

----------------------------------------------

v1.6.0

- Added Auto Exchanging for SouthXchange
- Fixed issue where you couldn't clear the pinned coin once set
- Added Raspberry Pi Installer Script

----------------------------------------------

v1.5.0

- Added Kucoin for auto exchanging.
- Added an option for a pinned coin per rig. This will force that coin to be mined regardless of profitability. i.e., if you want to mine Radiant or a new coin, set that coin and it will over-ride the WTM calculations

----------------------------------------------

v1.4.1

- Fixed several Nicehash Algo Tickers. If they should empty on your coin list, just click edit and reselect them.
- Removed case sensitivity requirement for coins since there are some differences between WTM, Hive OS, and Exchanges

----------------------------------------------

v1.4.0

- Added Auto Exchanging for TxBit
- Added Auto Exchanging for TradeOgre
- Added Auto Exchanging for CoinEx
- Added process to cleanup donation flightsheets

----------------------------------------------

v1.3.2

- Fixed a case sensitivity issue with flightsheets between WTM and Hive. Nicehash-ZelHash should work now
- Adjusted projected profits to indicate the appropriate currency ($ vs coin)
- Added core code for auto exchanging (will be fully implemented in a future release)

----------------------------------------------

v1.3.1

Patch Fix for Dev Fee. If you still notice excess dev fee processing, you can set it to 0.00%.

----------------------------------------------

v1.3.0

- Added Docker Image
- Added Nicehash-Octopus to the algo list
- Added Docker specific config options

----------------------------------------------

v1.2.1

Minor release for fixing Goldshell ASIC processing

----------------------------------------------

v1.2.0

- Added Goldshell ASIC Profit Switching
- Added Configurable Port for Web UI

----------------------------------------------

v1.0.2

- Added an easy installer script for hive os that will setup the tools as a native service
- Replaced all alert messages with toast notifications

----------------------------------------------

v1.0.1

Added an update check and the ability to execute the update from within the web ui

----------------------------------------------

v1.0.0

Initial release of the Retro Mike Mining Tools.

This includes Core Hive OS Profit Switching capabilities.
