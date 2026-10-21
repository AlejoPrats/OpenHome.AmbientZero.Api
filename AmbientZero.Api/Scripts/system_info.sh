#!/bin/bash

jq -n \
  --arg mem_avail "$(awk '/MemAvailable/ {val=$2/1024; if (val < 1024) printf "%.1f MiB", val; else printf "%.2f GiB", val/1024}' /proc/meminfo)" \
  --arg mem_free "$(awk '/MemFree/ {val=$2/1024; if (val < 1024) printf "%.1f MiB", val; else printf "%.2f GiB", val/1024}' /proc/meminfo)" \
  --arg swap_used "$(awk '/SwapTotal/ {total=$2} /SwapFree/ {free=$2} END {val=(total-free)/1024; if (val < 1024) printf "%.1f MiB", val; else printf "%.2f GiB", val/1024}' /proc/meminfo)" \
  --argjson cpu_temp "$(cat /sys/class/thermal/thermal_zone0/temp 2>/dev/null | awk '{printf "%.1f", $1/1000}')" \
  '{
    "memory": { "available": $mem_avail, "free": $mem_free },
    "swap": { "used": $swap_used },
    "cpu": { "temperature": $cpu_temp }
  }'