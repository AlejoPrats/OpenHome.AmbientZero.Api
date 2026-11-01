#!/bin/bash
set -e

DB="/opt/api/ambientZero.db"

# --- Read DB seeds ---
REMOTE_DEVICE_ID=$(sqlite3 "$DB" "SELECT SettingValue FROM applicationSetting WHERE SettingName='RemoteDeviceId';")
UPDATE_ENDPOINT=$(sqlite3 "$DB" "SELECT SettingValue FROM applicationSetting WHERE SettingName='UpdateEndpoint';")
ROOT_PROJECT_URL=$(sqlite3 "$DB" "SELECT SettingValue FROM applicationSetting WHERE SettingName='RootProjectURL';")

# --- Assemble final URL ---
FINAL_URL="${ROOT_PROJECT_URL}/${UPDATE_ENDPOINT}?id=${REMOTE_DEVICE_ID}"
echo $FINAL_URL
echo $ROOT_PROJECT_URL
echo $REMOTE_DEVICE_ID
echo $UPDATE_ENDPOINT
# --- Fetch remote JSON (array of modules) ---
JSON=$(curl -s "$FINAL_URL")

# --- Iterate over each module entry ---
echo "$JSON" | jq -c '.[]' | while read -r ITEM; do
    NAME=$(echo "$ITEM" | jq -r '.name')
    VERSION=$(echo "$ITEM" | jq -r '.version')
    BINARY_URL=$(echo "$ITEM" | jq -r '.binaryUrl')

    # Update SQLite for this module
    sqlite3 "$DB" <<EOF
UPDATE version
SET LocalVersion='$VERSION'
WHERE ModuleName='$NAME';
EOF
done