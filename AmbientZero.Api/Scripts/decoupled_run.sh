#!/bin/bash
TARGET_SCRIPT="$1" # $1 = The absolute path to the script you want to run
SCRIPT_ARGS=("${@:2}") # ${@:2} = Every other argument passed after it (forwarded completely)

if [ -z "$TARGET_SCRIPT" ]; then
        echo "Usage: decoupled_run.sh <target script>"
        exit 1
fi

sudo systemd-run \
    --unit=offline_execution \
    --on-active=2s \
    --timer-property=AccuracySec=100ms \
    "$TARGET_SCRIPT" "${SCRIPT_ARGS[@]}"
