#!/bin/bash
CLEAN_TYPE="$1"
RESTART_DELAY="${2:-10}"

if [ -z "$CLEAN_TYPE" ]; then
    echo "Usage: clean_ram.sh <clean_type> [restart_delay]"
    exit 1
fi

sync
echo 3 | sudo tee /proc/sys/vm/drop_caches > /dev/null

if [ "$SKIP_DOWNLOAD" != "Force" ]; then
    systemd-run --collect --no-block --on-active="$RESTART_DELAY" /bin/systemctl restart AmbientZeroApi.service
fi