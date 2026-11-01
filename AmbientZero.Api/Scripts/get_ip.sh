#!/bin/bash
ADAPTER_NAME="$1"

if [ -z "$ADAPTER_NAME" ]; then
        echo "Usage: get_ip.sh <adapter name>"
        exit 1
fi

ip -j -br address | jq --arg target "$ADAPTER_NAME" '.[] | select(.ifname == $target) | {
  adapterName: .ifname,
  adapterStatus: .operstate,
  ipAddress: .addr_info[0].local,
  prefixlen: .addr_info[0].prefixlen
}'