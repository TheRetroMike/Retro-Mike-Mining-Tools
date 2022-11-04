#!/bin/bash
apt-get update -y
apt-get install ipmitool -y
wget -O /QuantaS51GFanControl.sh https://raw.githubusercontent.com/TheRetroMike/Retro-Mike-Mining-Tools/main/NodeUtilities/QuantaS51GFanControl.sh
chmod +x /QuantaS51GFanControl.sh

cat >/etc/systemd/system/s51gfancontrol.service<<'EOF'
[Unit]
Description=Start Retro Mike's Quanta S51G fan control service
StartLimitIntervalSec=0

[Service]
Type=idle
ExecStart=/QuantaS51GFanControl.sh

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable s51gfancontrol
systemctl start s51gfancontrol
