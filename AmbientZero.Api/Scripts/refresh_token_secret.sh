RANDOM_KEY=$(tr -dc 'A-Za-z0-9!#$%&()*+,-.:;=?@^_~' </dev/urandom | head -c 64)

tmpfile=$(mktemp)
jq --arg key "$RANDOM_KEY" '.Secrets.TokenEncryptionKey = $key' /opt/api/appsettings.json > "$tmpfile" && mv "$tmpfile" /opt/api/appsettings.json

systemctl restart AmbientZeroApi.service