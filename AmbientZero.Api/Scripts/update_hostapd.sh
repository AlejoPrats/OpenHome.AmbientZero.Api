#!/bin/bash
# Modern NetworkManager drop-in handler for application-driven AP updates

CON_NAME="AmbientZero"

NEW_SSID="$1"
NEW_CHANNEL="$2"
NEW_COUNTRY="$3"
NEW_PASSWORD="$4"
NEW_SECURITY="$5"   # 0=open, 1=wpa2

# Validate SSID
if [[ -z "$NEW_SSID" ]]; then
    echo "Error: SSID cannot be empty"
    exit 1
fi

# Validate channel (1–11 for 2.4GHz)
if ! [[ "$NEW_CHANNEL" =~ ^[1-9]$|^1[0-1]$ ]]; then
    echo "Error: Channel must be between 1 and 11"
    exit 1
fi

# Validate country code (2 letters)
if ! [[ "$NEW_COUNTRY" =~ ^[A-Z]{2}$ ]]; then
    echo "Error: Country code must be 2 uppercase letters"
    exit 1
fi

echo "Updating wireless regulatory domain (Country Code)..."
# In modern RPi OS, the country code is managed globally by regulatory domains
sudo iw reg set "$NEW_COUNTRY" || true

echo "Updating NetworkManager profile '$CON_NAME' settings..."

# 1. Update basic infrastructure parameters
sudo nmcli connection modify "$CON_NAME" \
    802-11-wireless.ssid "$NEW_SSID" \
    802-11-wireless.channel "$NEW_CHANNEL"

# 2. Update Security Mode Dynamically
if [[ "$NEW_SECURITY" -eq 1 ]]; then
    echo "Configuring SECURED WPA2 network..."
    sudo nmcli connection modify "$CON_NAME" \
        802-11-wireless-security.key-mgmt wpa-psk \
        802-11-wireless-security.psk "$NEW_PASSWORD" \
        802-11-wireless-security.proto rsn \
        802-11-wireless-security.pairwise ccmp \
        802-11-wireless-security.group ccmp
else
    echo "Configuring OPEN network (no WPA2)..."
    # Cleanly strip out all lingering security keys from the profile context
    sudo nmcli connection modify "$CON_NAME" remove wifi-sec 2>/dev/null || true
    sudo nmcli connection modify "$CON_NAME" remove 802-1x 2>/dev/null || true
fi

echo "Reloading profile and recycling interface..."
sudo nmcli connection reload
sudo nmcli connection down "$CON_NAME" 2>/dev/null || true
sudo nmcli connection up "$CON_NAME"

echo "Done."