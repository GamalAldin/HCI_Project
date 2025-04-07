import cv2
import mediapipe as mp
import numpy as np
from PIL import Image
import socket

# Initialize MediaPipe Pose and Hands
mp_drawing = mp.solutions.drawing_utils
mp_pose = mp.solutions.pose
mp_hands = mp.solutions.hands

# Load hoodie images with transparency (ensure they have an alpha channel)
shirt_images = {
    "red": Image.open("hoodies/red.png").convert("RGBA"),
    "blue": Image.open("hoodies/blue.png").convert("RGBA"),
    "green": Image.open("hoodies/green.png").convert("RGBA")
}

# Start video capture
cap = cv2.VideoCapture(0)

# Define socket communication (if you want to communicate with C#)
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server_address = ("127.0.0.1", 5005)

# Initialize MediaPipe Hands for gesture detection
hands = mp_hands.Hands(min_detection_confidence=0.5)

# Variable to store the current hoodie image
hoodie_list = ["red", "blue", "green"]  # List of hoodies to cycle through
current_hoodie_idx = 0  # Start with the red hoodie

# Variable to track pinch gesture state
pinch_detected = False

# Function to calculate distance between two landmarks
def calculate_distance(landmark1, landmark2):
    return np.linalg.norm([landmark1.x - landmark2.x, landmark1.y - landmark2.y])

# Start video capture
with mp_pose.Pose(static_image_mode=False, model_complexity=1, min_detection_confidence=0.5) as pose:
    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break

        h, w, _ = frame.shape
        # Convert frame to RGB for MediaPipe
        image_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

        # Get pose landmarks
        results = pose.process(image_rgb)
        
        # Get hand landmarks for gesture recognition
        hand_results = hands.process(image_rgb)

        if results.pose_landmarks:
            landmarks = results.pose_landmarks.landmark

            try:
                # Extract relevant pose landmarks (shoulders and hips)
                l_sh = (int(landmarks[mp_pose.PoseLandmark.LEFT_SHOULDER].x * w),
                        int(landmarks[mp_pose.PoseLandmark.LEFT_SHOULDER].y * h))
                r_sh = (int(landmarks[mp_pose.PoseLandmark.RIGHT_SHOULDER].x * w),
                        int(landmarks[mp_pose.PoseLandmark.RIGHT_SHOULDER].y * h))
                l_hip = (int(landmarks[mp_pose.PoseLandmark.LEFT_HIP].x * w),
                         int(landmarks[mp_pose.PoseLandmark.LEFT_HIP].y * h))
                r_hip = (int(landmarks[mp_pose.PoseLandmark.RIGHT_HIP].x * w),
                         int(landmarks[mp_pose.PoseLandmark.RIGHT_HIP].y * h))

                # Calculate hoodie overlay position
                center_x = (l_sh[0] + r_sh[0]) // 2
                top_y = min(l_sh[1], r_sh[1]) // 2
                bottom_y = max(l_hip[1], r_hip[1])
                width = int(np.linalg.norm(np.array(l_sh) - np.array(r_sh)) * 2)
                height = bottom_y - top_y

                # Resize the current hoodie
                current_hoodie = shirt_images[hoodie_list[current_hoodie_idx]]
                resized_hoodie = current_hoodie.resize((width, height), Image.LANCZOS)
                shirt_x = center_x - width // 2
                shirt_y = top_y

                # Add an offset to lower the hoodie
                offset = 20  # Adjust this value as needed to move the hoodie lower
                shirt_y = top_y + offset

                # Overlay the hoodie onto the frame
                frame_pil = Image.fromarray(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)).convert("RGBA")
                frame_pil.paste(resized_hoodie, (shirt_x, shirt_y), resized_hoodie)

                # Convert back to OpenCV
                frame = cv2.cvtColor(np.array(frame_pil), cv2.COLOR_RGBA2BGR)

            except IndexError:
                pass  # If any landmark is missing, it will simply continue

            # Gesture recognition for switching hoodie
            if hand_results.multi_hand_landmarks:
                for hand_landmark in hand_results.multi_hand_landmarks:
                    # Extract thumb and index finger landmarks
                    thumb_tip = hand_landmark.landmark[mp_hands.HandLandmark.THUMB_TIP]
                    index_tip = hand_landmark.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP]
                    distance_thumb_index = calculate_distance(thumb_tip, index_tip)

                    # Pinch gesture detection (thumb and index touching)
                    if distance_thumb_index < 0.01 and not pinch_detected:  # Pinch detected
                        # Switch to the next hoodie
                        current_hoodie_idx = (current_hoodie_idx + 1) % len(hoodie_list)
                        print(f"Pinch gesture detected, switching to {hoodie_list[current_hoodie_idx]} hoodie...")
                        sock.sendto(f"switch_hoodie_{hoodie_list[current_hoodie_idx]}".encode(), server_address)

                        # Set pinch_detected to True to prevent continuous switching
                        pinch_detected = True

                    # Once pinch is no longer detected, reset the state
                    if distance_thumb_index > 0.1:
                        pinch_detected = False

        # Display the result
        cv2.imshow('MediaPipe Virtual Fitting Room', frame)

        # Exit with Esc
        if cv2.waitKey(1) & 0xFF == 27:
            break

cap.release()
cv2.destroyAllWindows()
