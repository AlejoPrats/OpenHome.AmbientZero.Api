#!/bin/bash
set -e

ZIP_URL="$1"
SKIP_DOWNLOAD="${2:-false}"
RESTART_DELAY="${3:-10}"
TMP_ZIP="/tmp/api.zip"

if [ -z "$ZIP_URL" ]; then
    echo "Usage: update_api.sh <zip_url> [skip_download] [restart_delay_seconds]"
    exit 1
fi

if [ "$SKIP_DOWNLOAD" != "True" ]; then
    wget -O "$TMP_ZIP" "$ZIP_URL"
fi

unzip -o "$TMP_ZIP" -d /opt/api
rm "$TMP_ZIP"

sudo chown -R ambientzero:ambientzero /opt/api

sudo chown root:root /opt/api/Scripts/*.sh
sudo chmod 750 /opt/api/Scripts
sudo chmod 750 /opt/api/Scripts/*.sh

# Schedule delayed restart so API can finish responding
systemd-run --on-active="$RESTART_DELAY" /bin/systemctl restart AmbientZeroApi.service
