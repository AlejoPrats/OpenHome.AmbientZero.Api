#!/usr/bin/env bash
# Stateless JSON Wireless Spectrum Scanner for AmbientZero API Integration

AP_INT="wlan0"
STA_INT="wlan1"

# 1. Quietly refresh the kernel neighbor cache mappings
nmcli device wifi rescan ifname "$AP_INT" 2>/dev/null || true
sleep 2

# 2. Dynamically locate what channel wlan1 is currently using to connect to your router
# Returns the active channel integer, or 0 if wlan1 is completely disconnected
UPLINK_CHAN=$(nmcli -t -f IN-USE,CHAN device wifi list ifname "$STA_INT" 2>/dev/null | grep '^\*:' | cut -d: -f2 || echo "0")

# 3. Read the active operational channel of the wlan0 Access Point profile
CURRENT_AP_CHAN=$(nmcli -t -f 802-11-wireless.channel connection show "AmbientZero" 2>/dev/null || echo "1")

# 4. Initialize clean database array bounds for all 11 standard 2.4GHz channels
declare -A CHANNEL_SCORES
for i in {1..11}; do
    CHANNEL_SCORES[$i]=0
done

# 5. Extract fresh environmental footprints directly from the NetworkManager pool
while read -r CHAN SIGNAL; do
    if [[ -z "$CHAN" || -z "$SIGNAL" ]] || [ "$CHAN" -gt 11 ] || [ "$CHAN" -lt 1 ]; then
        continue
    fi
    # Accumulate signal metrics values directly into the target channel indices
    CHANNEL_SCORES[$CHAN]=$(( CHANNEL_SCORES[$CHAN] + SIGNAL ))
done < <(nmcli -f CHAN,SIGNAL device wifi list ifname "$AP_INT" 2>/dev/null | awk 'NR>1 {print $1, $2}')

# 6. Streamout everything as structured, web-ready JSON format
cat << EOF
{
  "activeUplinkChannel": $UPLINK_CHAN,
  "currentAccessPointChannel": $CURRENT_AP_CHAN,
  "channels": [
    {"channel": 1, "rawNoise": ${CHANNEL_SCORES[1]}},
    {"channel": 2, "rawNoise": ${CHANNEL_SCORES[2]}},
    {"channel": 3, "rawNoise": ${CHANNEL_SCORES[3]}},
    {"channel": 4, "rawNoise": ${CHANNEL_SCORES[4]}},
    {"channel": 5, "rawNoise": ${CHANNEL_SCORES[5]}},
    {"channel": 6, "rawNoise": ${CHANNEL_SCORES[6]}},
    {"channel": 7, "rawNoise": ${CHANNEL_SCORES[7]}},
    {"channel": 8, "rawNoise": ${CHANNEL_SCORES[8]}},
    {"channel": 9, "rawNoise": ${CHANNEL_SCORES[9]}},
    {"channel": 10, "rawNoise": ${CHANNEL_SCORES[10]}},
    {"channel": 11, "rawNoise": ${CHANNEL_SCORES[11]}}
  ]
}
EOF