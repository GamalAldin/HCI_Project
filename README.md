# Virtual Fitting Room for HCI Course - MSA University

## Overview

This project is being developed as part of the **Human-Computer Interaction (HCI)** course at **MSA University**, Faculty of Computer Science. The goal of the project is to create a virtual fitting room that uses augmented reality (AR) and human motion tracking for a more immersive shopping experience.

### Project Team
- **Gamal Aldin Moshgi**
- **Shehab Elden Wael**
- **Omar Gamal**
- **Omar Eissa**
- **Maya Mohamed**
- **Shahenaz Saied**

### Current Status
The project is still under development. As of now, the following features have been implemented:
- **TUIO (Tangible User Interface Object) Integration**: This allows interaction with virtual objects using physical markers.
- **Human Skeleton Tracking**: A function to track human skeletal movements using the **MediaPipe** library.

The project is being continuously developed, and additional features will be integrated in the coming phases.

---

## Features

- **TUIO Integration**: We are using TUIO markers to interact with the virtual fitting room. The user can interact with virtual images of clothing by moving and rotating markers.
- **Human Skeleton Tracking**: The human skeleton tracking function utilizes **MediaPipe** to track a user’s skeleton in real-time, allowing gestures and body movements to influence the virtual fitting room environment.
- **Socket Programming for Communication**: We use socket programming to enable communication between the **Python** backend and the **C#** frontend. The content displayed in the virtual fitting room changes dynamically based on the marker positions and rotations received via TUIO, providing a seamless and interactive experience.

---

## Installation

To set up this project locally, follow these steps:

1. **Clone the repository**:

   ```bash
   git clone https://github.com/GamalAldin/HCI_Project.git
   ```

2. **Install dependencies**:

   Make sure you have **Python 3.x** installed on your machine. Then, install the required Python packages by running:

   ```bash
   pip install -r requirements.txt
   ```

   For C# (Windows Forms App) dependencies, ensure you have **Visual Studio** installed, and open the solution file (`WindowsFormsApp1.sln`) in Visual Studio to restore any NuGet packages.

3. **Setup TUIO**:

   Download and set up the **reacTIVision** software from the provided folder. Ensure it is running to send the necessary TUIO data to the Python code.

---

## Usage

1. **Run the Python Script**:
   - After setting up the environment, run the Python script to start the TUIO server and skeleton tracking:

   ```bash
   python app.py
   ```

2. **Run the C# Windows Forms App**:
   - Open the **WindowsFormsApp1.sln** file in **Visual Studio** and run the application. The app will receive TUIO data via socket programming and update the virtual fitting room UI accordingly.

3. **Use the Human Skeleton Tracker**:
   - The skeleton tracking is powered by **MediaPipe**. Ensure your webcam is enabled and connected properly to track your movements.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
