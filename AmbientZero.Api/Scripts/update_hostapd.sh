#!/bin/bash

HOSTAPD_CONF="/etc/hostapd/hostapd.conf"

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

echo "Updating hostapd configuration..."

# Rebuild hostapd.conf from scratch
tee "$HOSTAPD_CONF" >/dev/null <<EOF
country_code=$NEW_COUNTRY
interface=wlan0
ssid=$NEW_SSID
hw_mode=g
channel=$NEW_CHANNEL
macaddr_acl=0
auth_algs=1
ignore_broadcast_ssid=0
EOF

# Security mode
if [[ "$NEW_SECURITY" -eq 1 ]]; then
    tee -a "$HOSTAPD_CONF" >/dev/null <<EOF
wpa=2
wpa_passphrase=$NEW_PASSWORD
wpa_key_mgmt=WPA-PSK
rsn_pairwise=CCMP
EOF
else
    echo "Configuring OPEN network (no WPA2)"
fi

echo "Restarting hostapd..."
systemctl restart hostapd

echo "Done."
