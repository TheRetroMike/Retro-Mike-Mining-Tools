#!/usr/bin/env bash

SERVICE_NAME="retro_mike_mining_tools"
SERVICE_FILE="/etc/systemd/system/$SERVICE_NAME.service"
APP_PATH="/usr/retro-mike-mining-tools/RetroMikeMiningTools.dll"
APP_FOLDER="/usr/retro-mike-mining-tools"
LAUNCH_PATH="/usr/bin/dotnet $APP_PATH --service_name=$SERVICE_NAME --platform_name=hive_os"
CHROME_DRIVER="$APP_FOLDER/chromedriver"

EXE_FOLDER=/usr/profit-switcher/HiveProfitSwitcher/bin/Debug/
CONFIG_PATH="$EXE_FOLDER/HiveProfitSwitcher.exe.config"


wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
apt-get update
apt-get install -y dotnet-sdk-6.0
wget https://github.com/TheRetroMike/Retro-Mike-Mining-Tools/releases/latest/download/RetroMikeMiningTools.zip
unzip  -o -d /usr/retro-mike-mining-tools RetroMikeMiningTools.zip
rm RetroMikeMiningTools.zip

if [ -f "$SERVICE_FILE" ] ; then
        IS_ACTIVE=$(systemctl is-active $SERVICE_NAME)
        if [ "$IS_ACTIVE" == "active" ]; then
                systemctl stop $SERVICE_NAME
                systemctl disable $SERVICE_NAME
                systemctl daemon-reload
                systemctl reset-failed
        fi
        rm "$SERVICE_FILE"
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
SYSTEM_IP=$(ip route get 8.8.8.8 | awk -F"src " 'NR==1{split($2,a," ");print a[1]}')

echo "------------------------------------------------------"
echo "------------------------------------------------------"
echo "------------------------------------------------------"
echo "Retro Mike Mining Tools has been installed. Please navigate to http://$SYSTEM_IP:7000/ to configure." 
echo "------------------------------------------------------"
echo "------------------------------------------------------"
echo "------------------------------------------------------"
exit 0
