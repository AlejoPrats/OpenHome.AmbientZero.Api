#!/usr/bin/env bash

AP_INT="wlan0"
STA_INT="wlan1"

nmcli device wifi rescan ifname "$AP_INT" 2>/dev/null || true
sleep 2

UPLINK_CHAN=$(nmcli -t -f IN-USE,CHAN device wifi list ifname "$STA_INT" 2>/dev/null | grep '^\*:' | cut -d: -f2 || echo "0")

CURRENT_AP_CHAN=$(nmcli -t -f 802-11-wireless.channel connection show "AmbientZero-AP" 2>/dev/null grep '^\*:' | cut -d: -f2 || echo "0")

declare -A CHANNEL_SCORES
for i in {1..11}; do
    CHANNEL_SCORES[$i]=0
done

while read -r CHAN SIGNAL; do
    if [[ -z "$CHAN" || -z "$SIGNAL" ]] || ! [[ "$CHAN" =~ ^[0-9]+$ ]]; then
        continue
    fi

    if [ "$CHAN" -gt 11 ] || [ "$CHAN" -lt 1 ]; then
        continue
    fi

    CHANNEL_SCORES[$CHAN]=$(( CHANNEL_SCORES[$CHAN] + SIGNAL ))
done < <(nmcli -f CHAN,SIGNAL device wifi list ifname "$AP_INT" 2>/dev/null | awk 'NR>1 {print $1, $2}')

BEST_CHAN=1
LOWEST_COST=99999999

for target in {1..11}; do
    direct_noise=${CHANNEL_SCORES[$target]}
    local_cost=$(( direct_noise * 1000 )) 

    bleeding_cost=0
    for scanned in {1..11}; do
        if [ $target -eq $scanned ]; then 
            continue 
        fi
        
        delta=$(( target - scanned ))
        if [ $delta -lt 0 ]; then 
            delta=$(( -delta ))
        fi
        
        neighbor_noise=${CHANNEL_SCORES[$scanned]}
        
        if [ $neighbor_noise -gt 0 ]; then
            if [ $delta -eq 1 ]; then
                bleeding_cost=$(( bleeding_cost + (neighbor_noise * 15) ))
            elif [ $delta -eq 2 ]; then
                bleeding_cost=$(( bleeding_cost + (neighbor_noise * 5) ))
            fi
        fi
    done

    total_cost=$(( local_cost + bleeding_cost ))

    if [ $target -eq 1 ] || [ $target -eq 11 ]; then
        total_cost=$(( total_cost + 150 ))
    fi

    if [ $total_cost -lt $LOWEST_COST ]; then
        LOWEST_COST=$total_cost
        BEST_CHAN=$target
    fi
done

cat << EOF
{
  "activeUplinkChannel": $UPLINK_CHAN,
  "currentAccessPointChannel": $CURRENT_AP_CHAN,
  "recommendedChannel": $BEST_CHAN,
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
