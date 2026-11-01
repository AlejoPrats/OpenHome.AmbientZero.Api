#!/bin/bash
set -e

ZIP_URL="$1"
SKIP_DOWNLOAD="${2:-false}"
TMP_ZIP="/tmp/web.zip"

if [ -z "$ZIP_URL" ]; then
    echo "Usage: update_web.sh <zip_url> [skip_download]"
    exit 1
fi

if [ "$SKIP_DOWNLOAD" != "True" ]; then
    wget -O "$TMP_ZIP" "$ZIP_URL"
fi

unzip -o "$TMP_ZIP" -d /opt/web
rm "$TMP_ZIP"