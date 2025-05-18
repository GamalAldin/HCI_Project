# face_recognition_login.py
import face_recognition
import cv2
import os
import socket

USERS_DIR = r"D:/UNI/HCI/Projecthci/Project/VirtualFittingRoom/bin/Debug/net8.0-windows/users"
PORT = 9004
login_callback = None

def set_login_callback(callback):
    global login_callback
    login_callback = callback

def load_user_faces():
    face_encodings = {}
    for user in os.listdir(USERS_DIR):
        user_folder = os.path.join(USERS_DIR, user)
        photo_path = os.path.join(user_folder, "photo.jpg")
        if os.path.exists(photo_path):
            image = face_recognition.load_image_file(photo_path)
            encodings = face_recognition.face_encodings(image)
            if encodings:
                face_encodings[user] = encodings[0]
    return face_encodings

def recognize_face(known_faces):
    cap = cv2.VideoCapture(0)
    print("[FaceID] Camera started. Press ESC to cancel...")

    matched_user = None

    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break

        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        faces = face_recognition.face_locations(rgb_frame)
        encodings = face_recognition.face_encodings(rgb_frame, faces)

        for face_encoding in encodings:
            for username, known_encoding in known_faces.items():
                match = face_recognition.compare_faces([known_encoding], face_encoding, tolerance=0.45)
                if match[0]:
                    matched_user = username
                    print(f"[FaceID] Match found: {username}")
                    break
            if matched_user:
                break

        cv2.imshow("Face Login", frame)
        if matched_user or cv2.waitKey(1) == 27:
            break

    cap.release()
    cv2.destroyAllWindows()
    return matched_user

def face_login_server():
    known_faces = load_user_faces()
    print("[FaceID] Waiting for login request...")

    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind(("0.0.0.0", PORT))
    server.listen()

    while True:
        conn, addr = server.accept()
        print("[FaceID] Login trigger received.")
        user = recognize_face(known_faces)

        if user and login_callback:
            login_callback()  # Trigger MediaPipe start

        conn.sendall((user if user else "UNKNOWN").encode())
        conn.close()

