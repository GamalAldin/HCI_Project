from pythonosc import osc_server, dispatcher
import socket

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server_address = ("127.0.0.1", 5005)

# Define the markers
MARKERS = {42: "hoodie", 1: "trouser"}

def tuio_handler(address, *args):
    if len(args) >= 6 and args[0] == "set":
        fiducial_id = args[2]
        x, y = args[3], args[4]
        rotation = args[5]

        if fiducial_id in MARKERS:
            message = f"{fiducial_id},{x},{y},{rotation}"
            sock.sendto(message.encode(), server_address)
            print(f"📡 Sent to C#: {message}")

dispatcher = dispatcher.Dispatcher()
dispatcher.map("/tuio/2Dobj", tuio_handler)

server = osc_server.ThreadingOSCUDPServer(("0.0.0.0", 3333), dispatcher)
print("🎯 Listening for TUIO messages on port 3333...")
server.serve_forever()
