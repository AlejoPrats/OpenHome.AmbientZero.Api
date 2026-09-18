#!/bin/bash

SECONDS_UNTIL_REBOOT=$(echo "$1" | tr -d '"')

# Kill previous scheduled reboot if it exists
sudo systemctl stop safe_reboot.service 2>/dev/null
sudo systemctl stop safe_reboot.timer 2>/dev/null

# Schedule new reboot
sudo systemd-run \
    --unit=safe_reboot \
    --on-active="$SECONDS_UNTIL_REBOOT" \
    /sbin/reboot -f
