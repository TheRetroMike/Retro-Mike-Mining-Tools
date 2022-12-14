#!/bin/bash
while true
do
  CPUTEMP=$(echo "scale=0; $(sort -nr /sys/class/hwmon/hwmon1/{temp1_input,temp2_input,temp3_input} | head -n1) / 1000" | bc)
  if [[ $CPUTEMP -gt 80 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x60
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x60
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x60
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x60
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x60
  elif [[ $CPUTEMP -gt 75 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x56
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x56
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x56
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x56
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x56
  elif [[ $CPUTEMP -gt 70 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x52
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x52
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x52
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x52
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x52
  elif [[ $CPUTEMP -gt 65 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x48
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x48
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x48
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x48
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x48
  elif [[ $CPUTEMP -gt 60 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x44
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x44
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x44
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x44
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x44
  elif [[ $CPUTEMP -gt 55 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x40
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x40
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x40
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x40
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x40
  elif [[ $CPUTEMP -gt 50 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x36
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x36
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x36
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x36
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x36
  elif [[ $CPUTEMP -gt 45 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x32
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x32
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x32
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x32
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x32
  elif [[ $CPUTEMP -gt 40 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x28
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x28
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x28
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x28
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x28
  elif [[ $CPUTEMP -gt 35 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x24
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x24
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x24
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x24
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x24
  elif [[ $CPUTEMP -gt 30 ]]; then
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x20
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x20
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x20
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x20
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x20
  else
    ipmitool raw 0x30 0x39 0x01 0x0 0x0 0x16
    ipmitool raw 0x30 0x39 0x01 0x0 0x1 0x16
    ipmitool raw 0x30 0x39 0x01 0x0 0x2 0x16
    ipmitool raw 0x30 0x39 0x01 0x0 0x3 0x16
    ipmitool raw 0x30 0x39 0x01 0x0 0x4 0x16
  fi
  sleep 30
done
