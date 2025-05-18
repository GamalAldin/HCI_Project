# from threading import Thread
# from tuio_controller import start_tuio_server, receive_username
# from mediapipe_overlay import start_mediapipe_overlay
# import time

# if __name__ == "__main__":
#     print("[System] Starting Virtual Fitting Room...")

#     # ✅ Start the username listener first
#     Thread(target=receive_username, daemon=True).start()

#     # ✅ Start the TUIO marker listener
#     Thread(target=start_tuio_server, daemon=True).start()

#     # (Optional) Delay to let servers bind before MediaPipe starts
#     time.sleep(1)

#     # ✅ Start the MediaPipe overlay (runs in foreground)
#     start_mediapipe_overlay()
from threading import Thread
from tuio_controller import start_tuio_server, receive_username
from mediapipe_overlay import start_mediapipe_overlay
from face_recognition_login import face_login_server, set_login_callback
import time

def on_face_login_success():
    print("[System] ✅ FaceID login complete. Starting MediaPipe overlay...")
    Thread(target=start_mediapipe_overlay, daemon=True).start()

if __name__ == "__main__":
    print("[System] Starting Virtual Fitting Room...")

    # Register the callback to start MediaPipe only after login
    set_login_callback(on_face_login_success)

    # Start TUIO + username listeners
    Thread(target=receive_username, daemon=True).start()
    Thread(target=start_tuio_server, daemon=True).start()

    # Run FaceID login server in foreground (it blocks)
    face_login_server()


