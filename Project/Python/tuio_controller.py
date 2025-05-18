from pythonosc.dispatcher import Dispatcher
from pythonosc import osc_server
import socket
import threading
import math
import os

# === Marker Mappings ===
CATEGORY_IDS = {
    14: "hoodies",
    15: "shirts",
    16: "trousers",
    17: "polo",
    18: "dresses"
}

COLOR_IDS = {
    19: "red",
    20: "green",
    21: "blue"
}

# === Constants and State ===
OUTFITS_BASE_PATH = "C:/Users/user/Desktop/HCI Project/VirtualFittingRoom/Python/outfits"
rotation_threshold = math.pi / 2

last_rotation = None
marker2_visible = False
orderid_visible = False
visible_markers = set()
session_to_fiducial = {}
last_order = None
current_username = None
gender = "male"

# === Networking Functions ===

def send_to_csharp(data, port=9001):
    try:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect(("127.0.0.1", port))
            s.sendall(f"{data}\n".encode())
            print(f"[TUIO] Sent to C#: {data}")
    except Exception as e:
        print("[TUIO] Command socket error:", e)

def send_image_path(image_path, port=9002):
    try:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect(("127.0.0.1", port))
            s.sendall(f"{image_path}\n".encode())
            print(f"[TUIO] Sent image path: {image_path}")
    except Exception as e:
        print("[TUIO] Image path socket error:", e)

# === Gesture Processing ===

def normalize_angle(angle):
    return angle % (2 * math.pi)

def angle_difference(current, previous):
    diff = current - previous
    if diff > math.pi:
        diff -= 2 * math.pi
    elif diff < -math.pi:
        diff += 2 * math.pi
    return diff

# === TUIO Handler ===

def tuio_handler(address, *args):
    global last_rotation, marker2_visible, visible_markers, last_order, current_username, orderid_visible, session_to_fiducial

    if len(args) >= 6 and args[0] == "set":
        session_id = args[1]
        fiducial_id = args[2]
        rotation = args[5]

        # Update mapping
        session_to_fiducial[session_id] = fiducial_id
        visible_markers.add(fiducial_id)

        print(f"[DEBUG] TUIO message: {args}")

        # Marker 12: Rotate Menu
        if fiducial_id == 12:
            marker2_visible = False
            rotation = normalize_angle(rotation)
            if last_rotation is None:
                last_rotation = rotation
                return
            delta = angle_difference(rotation, last_rotation)
            if abs(delta) >= rotation_threshold:
                direction = "rotate-right" if delta > 0 else "rotate-left"
                send_to_csharp(direction)
                last_rotation = rotation

        # Marker 13: Select Menu Option
        elif fiducial_id == 13:
            if not marker2_visible:
                marker2_visible = True
                send_to_csharp("select")

        # Direct outfit selection: Category + Color
        category_id = next((id for id in visible_markers if id in CATEGORY_IDS), None)
        color_id = next((id for id in visible_markers if id in COLOR_IDS), None)

        if category_id and color_id:
            category = CATEGORY_IDS[category_id]
            color = COLOR_IDS[color_id]
            last_order = (category, color)
            image_path = os.path.join(OUTFITS_BASE_PATH, gender, category, f"{color}.png")

            if os.path.exists(image_path):
                send_image_path(image_path)
            else:
                print(f"[TUIO] Image not found: {image_path}")

    elif len(args) >= 2 and args[0] == "alive":
        session_ids = set(args[1:])
        visible_markers = {session_to_fiducial[sid] for sid in session_ids if sid in session_to_fiducial}
        print(f"[DEBUG] Visible markers: {visible_markers}")
        print(f"[DEBUG] Current username: {current_username}")
        print(f"[DEBUG] Last order: {last_order}")

        # Marker 22: Place order
        if 22 in visible_markers and last_order and current_username and not orderid_visible:
            category, color = last_order
            send_to_csharp(f"place-order:{category},{color}")
            orderid_visible = True
            print(f"[TUIO] Sending place-order command: {category}, {color}")
        elif 22 not in visible_markers:
            orderid_visible = False

# === Username Listener (Port 9003) ===

def receive_username(port=9003):
    global current_username
    try:
        server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server.bind(("0.0.0.0", port))
        server.listen()
        print(f"[User] Waiting for username on port {port}...")
        while True:
            conn, addr = server.accept()
            data = conn.recv(1024).decode().strip()
            if data:
                current_username = data
                print(f"[User] Logged in as: {current_username}")
    except Exception as e:
        print(f"[User] Error receiving username: {e}")

# === TUIO Server Entry Point ===

def start_tuio_server(host='0.0.0.0', port=3333):
    dispatcher = Dispatcher()
    dispatcher.map("/tuio/2Dobj", tuio_handler)
    server = osc_server.ThreadingOSCUDPServer((host, port), dispatcher)
    print(f"[TUIO] Listening on {host}:{port}...")
    server.serve_forever()

# === Main ===

if __name__ == "__main__":
    threading.Thread(target=receive_username, daemon=True).start()
    start_tuio_server()
