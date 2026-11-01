#!/usr/bin/env bash
# Stateless Hardware Executor for Dynamic AP Channel Selection

TARGET_CHANNEL="$1"
CON_NAME="AmbientZero"

# Validate input integrity safely before invoking network layer changes
if ! [[ "$TARGET_CHANNEL" =~ ^[1-9]$|^1[0-1]$ ]]; then
    echo "Error: Target channel parameter must be an integer between 1 and 11"
    exit 1
fi

echo "[+] Executing radio migration profile update to Channel $TARGET_CHANNEL..."

# Update settings cleanly inside the NetworkManager configuration layer
sudo nmcli connection modify "$CON_NAME" \
    802-11-wireless.band bg \
    802-11-wireless.channel "$TARGET_CHANNEL"

# Apply changes instantly to wlan0 without interrupting wlan1 active sessions
sudo nmcli connection reload
sudo nmcli connection down "$CON_NAME" 2>/dev/null || true
sudo nmcli connection up "$CON_NAME" 2>/dev/null || true

echo "[+] Hardware migration complete."
