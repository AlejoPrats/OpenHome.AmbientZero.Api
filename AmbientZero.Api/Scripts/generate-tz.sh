#!/bin/bash

SOURCE_DIR="/usr/share/zoneinfo"
OUTPUT_FILE="/dev/shm/timezones.json"

if [ ! -d "$SOURCE_DIR" ]; then
    echo "Error: Source directory $SOURCE_DIR does not exist." >&2
    exit 1
fi

# Start JSON Array
echo "[" > "$OUTPUT_FILE"

first=true
while IFS= read -r file; do
    if [ -f "$file" ]; then
        # Extract the system ID (e.g., "Europe/Madrid")
        tz_id="${file#$SOURCE_DIR/}"
        
        # Ask the 'date' command for the offset (+0200) and short name (CEST)
        # Using a temporary environment variable 'TZ' so it doesn't affect the system
        tz_info=$(TZ="$tz_id" date +"%z %Z")
        
        # Split the string out safely
        offset=$(echo "$tz_info" | cut -d' ' -f1)
        abbrev=$(echo "$tz_info" | cut -d' ' -f2)
        
        # Format the numeric offset from "+0200" to "+02:00"
        sign=${offset:0:1}
        hours=${offset:1:2}
        minutes=${offset:3:2}
        formatted_offset="UTC${sign}${hours}:${minutes}"
        
        # Create a clean display name (e.g., "Europe/Madrid" -> "Madrid")
        city_name="${tz_id##*/}"
        city_name="${city_name//_/ }"
        
        # Build the final formal name string
        formal_name="($formatted_offset) $city_name ($abbrev)"
        
        if [ "$first" = true ]; then
            first=false
        else
            echo "," >> "$OUTPUT_FILE"
        fi
        
        # Output exactly to fit your export interface TimeZone { id: string; name: string; }
        echo -n "  { \"id\": \"$tz_id\", \"name\": \"$formal_name\" }" >> "$OUTPUT_FILE"
    fi
# Filter lists out noise files like links, tabs, and duplicate posix trees
done < <(find "$SOURCE_DIR" -type f ! -name "*.tab" ! -path "*/posix*" ! -path "*/right*" ! -name "localtime" ! -name "posixrules" | sort)

# Close JSON Array
echo "" >> "$OUTPUT_FILE"
echo "]" >> "$OUTPUT_FILE"

chmod 644 "$OUTPUT_FILE"