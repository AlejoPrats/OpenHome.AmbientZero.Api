#!/bin/bash

API_URL="http://localhost:5000/System/Health"
DB="/opt/api/ambientZero.db"

# Get kernel boot time (epoch seconds)
kernel_boot=$(grep btime /proc/stat | awk '{print $2}')

# Poll until API responds
while true; do
    if curl -s --max-time 1 "$API_URL" > /dev/null; then
        api_ready=$(date +%s)
        startup_duration=$((api_ready - kernel_boot))

        sudo -u ambientzero sqlite3 "$DB" \
            "UPDATE applicationSetting SET SettingValue='$startup_duration' WHERE SettingName = 'BootTime';"
        exit 0
    fi
    sleep 0.1
done
