#!/usr/bin/env python3
import json
from http.server import BaseHTTPRequestHandler, HTTPServer

STATE_FILE = "/dev/shm/status.json"

class Handler(BaseHTTPRequestHandler):
    def _set_headers(self, status_code=200):
        self.send_response(status_code)
        self.send_header("Content-Type", "application/json")
        
        # 1. ALLOW PORT CHANGE: Tells the browser that Angular can read this data
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Methods", "GET, POST, OPTIONS")
        self.send_header("Access-Control-Allow-Headers", "Content-Type")
        
        # 2. BUST CACHE: Prevents browser from serving old progress numbers
        self.send_header("Cache-Control", "no-cache, no-store, must-revalidate")
        self.send_header("Pragma", "no-cache")
        self.send_header("Expires", "0")
        
        self.end_headers()

    def do_OPTIONS(self):
        # Handle preflight requests safely if your app ever needs to talk back
        self._set_headers(200)

    def do_GET(self):
        try:
            with open(STATE_FILE, "r") as f:
                data = f.read()
        except Exception as e:
            data = json.dumps({"error": str(e)})

        self._set_headers(200)
        self.wfile.write(data.encode())

    def do_POST(self):
        length = int(self.headers.get("Content-Length", 0))
        body = self.rfile.read(length).decode()

        try:
            # Validate JSON before writing to disk
            json.loads(body)
            with open(STATE_FILE, "w") as f:
                f.write(body)
            self._set_headers(200)
        except Exception as e:
            self._set_headers(400)
            self.wfile.write(str(e).encode())

server = HTTPServer(("0.0.0.0", 5000), Handler)
server.serve_forever()
