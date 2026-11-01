#!/bin/bash

if [ -z "$1" ]; then
    echo "Error: Missing mode argument (e.g., import or export)"
    exit 1
fi

MODE="$1"

sudo systemctl stop AmbientZeroApi.service

sleep 2

sync
echo 3 | sudo tee /proc/sys/vm/drop_caches > /dev/null

python3 /opt/api/Python/micro-server.py &
PROGRESS_PID=$!

sleep 1

cd /opt/api && dotnet AmbientZero.Api.dll --$MODE && cd -
EXIT_CODE=$?

sleep 25

kill $PROGRESS_PID 2>/dev/null

sync
echo 3 | sudo tee /proc/sys/vm/drop_caches > /dev/null
sleep 1

sudo systemctl start AmbientZeroApi.service

exit $EXIT_CODE
