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
- Profit - Mine whichever coin has the best current profitability based on current difficulty, network hashrates, coin price, and power consumption
- CoinStacking - Mine whichever coin will net you the most coins based on current difficulty and network hashrates
- DiversificationByProfit - Mine whichever coin has the best current profitability based on current difficulty, network hashrates, coin price, and power consumption until you have obtained a desired amount of the coin and then move onto the next most profitable coin. 
- DiversificationByCoinStacking - Mine whichever coin will net you the most coins based on current difficulty and network hashrates until you have obtained a desired amount of the coin and then move onto the next most netable coin

## Auto Exchanging
Another powerful feature is the ability to have the application auto-exchange your mined crypto into the coins you wish to hold. If you are mining to an exchange, this is a great way to mine and exchange at minimal fees. The following exchanges are currently supported
- TxBit
- TradeOgre
- CoinEx

## Hive OS Installer
Install Video / Walkthrough: https://youtu.be/A3J7Ax6jtlk

```
wget -O - https://raw.githubusercontent.com/TheRetroMike/Retro-Mike-Mining-Tools/main/hive_install.sh | bash
```

## Docker Installer
```
sudo docker pull theretromike/miningtools && sudo docker run -d --name RetroMikeMiningTools -p 8080:7000 -v /volume0/retromikeminingtools:/app/db --restart always theretromike/miningtools
```

## Support Me
If you would like to support me:
- Youtube Channel: https://www.youtube.com/retromikecrypto
- Community Mining Pool: https://retromike.net
- Just run the profit switcher with any donation amount

## Support
Discord: https://discord.gg/HsjJPCP2hp
