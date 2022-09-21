# Retro Mike Mining Tools
Mining Tools such as Hive OS Profit Switching, Auto Exchanging, and Goldshell auto algo and coin switching

## Features
- Web based management and monitoring dashboard 
- Hive OS GPU Rig Profit Switching
- Goldshell ASIC Profit Switching and auto algo switching

## Hive OS Installer
```
wget -O - https://raw.githubusercontent.com/TheRetroMike/Retro-Mike-Mining-Tools/main/hive_install.sh | bash
```

## Docker Installer
```
sudo docker pull theretromike/miningtools && sudo docker run -d --name RetroMikeMiningTools -p 8080:7000 -v /volume0/retromikeminingtools:/app/db --restart always theretromike/miningtools
```
