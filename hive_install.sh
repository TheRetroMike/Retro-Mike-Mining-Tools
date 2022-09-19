#!/usr/bin/env bash

SERVICE_NAME="retro_mike_mining_tools"
SERVICE_FILE="/etc/systemd/system/$SERVICE_NAME.service"
APP_PATH="/usr/retro-mike-mining-tools/RetroMikeMiningTools.dll"
APP_FOLDER="/usr/retro-mike-mining-tools"
LAUNCH_PATH="/usr/bin/dotnet $APP_PATH --urls=http://0.0.0.0:7002 --service_name=$SERVICE_NAME --platform_name=hive_os"
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

if [ -f "$CHROME_DRIVER" ]; then
    echo "Chromedriver already installed"
else
    cd "$APP_FOLDER"
    apt-get install -y libappindicator1 fonts-liberation
    wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
    apt-get -f install
    dpkg --configure -a
    dpkg -i google-chrome*.deb
    wget https://chromedriver.storage.googleapis.com/105.0.5195.52/chromedriver_linux64.zip
    unzip chromedriver_linux64.zip
    rm chromedriver_linux64.zip
    rm google-chrome-stable_current_amd64.deb
fi

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

exit 0