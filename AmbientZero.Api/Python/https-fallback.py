import http.server
import ssl

class RedirectHandler(http.server.BaseHTTPRequestHandler):
    def do_GET(self):
        # Tell the browser to cleanly switch protocols to pure HTTP
        self.send_response(301)
        self.send_header('Location', 'http://myproduct.local' + self.path)
        self.end_headers()
        
    def log_message(self, format, *args):
        pass # Suppress logging to save CPU and RAM cycles

server_address = ('0.0.0.0', 443)
httpd = http.server.HTTPServer(server_address, RedirectHandler)

# Wrap the connection in the dummy SSL certificate
context = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)
context.load_cert_chain(certfile='/etc/ssl/certs/redirect.crt', keyfile='/etc/ssl/private/redirect.key')
httpd.socket = context.wrap_socket(httpd.socket, server_side=True)

print("Secure fallback listener running on port 443...")
httpd.serve_forever()
