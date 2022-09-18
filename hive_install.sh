#!/usr/bin/env bash

SERVICE_NAME="retro_mike_mining_tools"
SERVICE_FILE="/etc/systemd/system/$SERVICE_NAME.service"
APP_PATH="/usr/retro-mike-mining-tools/RetroMikeMiningTools.dll"
LAUNCH_PATH="/usr/bin/dotnet $APP_PATH --urls=http://0.0.0.0:7000 --service_name=$SERVICE_NAME --platform_name=hive_os"

wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-6.0
wget https://github.com/TheRetroMike/Retro-Mike-Mining-Tools/releases/latest/download/RetroMikeMiningTools.zip
unzip -o -d /usr/retro-mike-mining-tools RetroMikeMiningTools.zip
rm RetroMikeMiningTools.zip

if [ -f "$SERVICE_FILE" ] ; then
        IS_ACTIVE=$(systemctl is-active $SERVICE_NAME)
        if [ "$IS_ACTIVE" == "active" ]; then
                systemctl stop $SERVICE_NAME
                systemctl disable $SERVICE_NAME
                systemctl daemon-reload
                systemctl reset-failed
        fi
        sudo rm "$SERVICE_FILE"
fi

cat > /etc/systemd/system/${SERVICE_NAME//'"'/}.service << EOF
[Unit]
Description=Retro Mike Mining Tools
After=network.target

[Service]
WorkingDirectory=/usr/retro-mike-mining-tools
ExecStart=$LAUNCH_PATH
Restart=always

[Install]
WantedBy=multi-user.target
EOF
systemctl daemon-reload
systemctl enable ${SERVICE_NAME//'.service'/}
systemctl start ${SERVICE_NAME//'.service'/}

exit 0
