# Node Utilities
### Quanta S51G-1UL Fan Control
This service can be installed on a linux system to allow full fan curve control via ipmi. Make sure ipmitool is already installed. This will optimize hair flow and noise levels based on system load / cpu temp and will auto adjust all 5 fans between 25% and 93%. 

My recommended fan is the Artic S4028-6K. You can get the 5 Pack from here: https://amzn.to/3DyZUYS

#### Install
```
wget -O - https://raw.githubusercontent.com/TheRetroMike/Retro-Mike-Mining-Tools/main/NodeUtilities/QuantaS51GFanControlInstaller.sh | bash
```
