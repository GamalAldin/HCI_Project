import cv2
import mediapipe as mp
import socket
from tuio_controller import send_to_csharp
import threading
import numpy as np
from PIL import Image
from ultralytics import YOLO

yolo_model = YOLO("yolov8n.pt")

selected_image_path = None


def receive_clothing_path(host='0.0.0.0', port=9002):
    global selected_image_path
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((host, port))
    server.listen()
    print(f"[Overlay] Listening for image path on {host}:{port}...")
    while True:
        conn, addr = server.accept()
        data = conn.recv(1024).decode().strip()
        if data:
            selected_image_path = data
            print(f"[Overlay] Received clothing image: {selected_image_path}")

def overlay_transparent(background, overlay, x, y):
    """ Overlays a transparent PNG onto the frame at position (x, y) """
    h, w = overlay.shape[:2]
    if x + w > background.shape[1] or y + h > background.shape[0]:
        return background  # Skip if out of bounds

    # Split out alpha channel
    b, g, r, a = cv2.split(overlay)
    overlay_rgb = cv2.merge((b, g, r))
    mask = cv2.merge((a, a, a)) / 255.0

    # Region of interest
    roi = background[y:y+h, x:x+w]
    blended = roi * (1 - mask) + overlay_rgb * mask
    background[y:y+h, x:x+w] = blended.astype(np.uint8)
    return background

def detect_tie(frame):
    results = yolo_model(frame)
    for r in results:
        for box in r.boxes:
            cls_id = int(box.cls[0])
            class_name = yolo_model.model.names[cls_id]
            if class_name.lower() == "tie":
                return True
    return False

def overlay_clothes():
    global selected_image_path
    mp_pose = mp.solutions.pose
    cap = cv2.VideoCapture(0)  # External camera
    isreturn=False
    with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                break

            # Detect if a tie is already present using YOLO
            if (not isreturn):
                tie_present = detect_tie(frame)
                isreturn=True
            if(tie_present):
                send_to_csharp("return")
            image_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            results = pose.process(image_rgb)

            if results.pose_landmarks and selected_image_path and not tie_present:
                try:
                    landmarks = results.pose_landmarks.landmark
                    h, w, _ = frame.shape

                    l_sh = (int(landmarks[mp_pose.PoseLandmark.LEFT_SHOULDER].x * w),
                            int(landmarks[mp_pose.PoseLandmark.LEFT_SHOULDER].y * h))
                    r_sh = (int(landmarks[mp_pose.PoseLandmark.RIGHT_SHOULDER].x * w),
                            int(landmarks[mp_pose.PoseLandmark.RIGHT_SHOULDER].y * h))
                    l_hip = (int(landmarks[mp_pose.PoseLandmark.LEFT_HIP].x * w),
                             int(landmarks[mp_pose.PoseLandmark.LEFT_HIP].y * h))
                    r_hip = (int(landmarks[mp_pose.PoseLandmark.RIGHT_HIP].x * w),
                             int(landmarks[mp_pose.PoseLandmark.RIGHT_HIP].y * h))

                    center_x = (l_sh[0] + r_sh[0]) // 2
                    top_y = min(l_sh[1], r_sh[1]) // 2
                    bottom_y = max(l_hip[1], r_hip[1])
                    width = int(np.linalg.norm(np.array(l_sh) - np.array(r_sh)) * 0.5)  # Ties are narrower
                    height = int((bottom_y - top_y) * 0.6)  # Focus around the chest

                    cloth_pil = Image.open(selected_image_path).convert("RGBA")
                    resized_cloth = cloth_pil.resize((width, height), Image.LANCZOS)

                    offset = 30
                    shirt_x = center_x - width // 2
                    shirt_y = top_y + offset

                    frame_pil = Image.fromarray(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)).convert("RGBA")
                    frame_pil.paste(resized_cloth, (shirt_x, shirt_y), resized_cloth)

                    frame = cv2.cvtColor(np.array(frame_pil), cv2.COLOR_RGBA2BGR)

                except Exception as e:
                    print("Overlay error:", e)

            cv2.imshow("Virtual Fitting Room", frame)
            if cv2.waitKey(1) == 27:
                break
def start_mediapipe_overlay():
    threading.Thread(target=receive_clothing_path, daemon=True).start()
    overlay_clothes()
