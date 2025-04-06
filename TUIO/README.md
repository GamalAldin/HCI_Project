# Virtual Fitting Room - TUIO

This project allows users to interact with a **virtual fitting room** where markers (like fiducial tags) are detected via **reacTIVision**. The **Python** code processes marker data and sends it to a **C# Windows Forms** app that displays clothing items (hoodies and trousers) based on the marker's position and rotation.

## Folders Overview:
1. **Python**: Contains the Python script (`app.py`) and `requirements.txt` to install the necessary Python packages.
2. **reacTIVision-1.5.1-win64**: Contains the **reacTIVision** software, which detects fiducial markers and sends TUIO data to the Python script.
3. **WindowsFormsApp1**: Contains the C# Windows Forms project, which displays images and responds to the marker data.

---

## 🚀 Getting Started

### 1. Download the Repository

First, clone or download this repository to your local machine.

```bash
git clone https://github.com/GamalAldin/HCI_Project.git
```

---

### 2. Setting Up the Python Environment

#### 2.1 Install Python

Make sure you have **Python 3.10** or higher installed. If not, you can download it from the [official Python website](https://www.python.org/downloads/).

#### 2.2 Create a Virtual Environment

Navigate to the **Python** folder and create a virtual environment:

```bash
cd path/to/your/project/Python
python3 -m venv venv
```

Activate the virtual environment:
- On **Windows**:
  ```bash
  venv\Scripts\activate
  ```
- On **Mac/Linux**:
  ```bash
  source venv/bin/activate
  ```

#### 2.3 Install Required Packages

Once the virtual environment is activated, install the required Python packages:

```bash
pip install -r requirements.txt
```

---

### 3. Setting Up reacTIVision

#### 3.1 Install reacTIVision

1. Navigate to the **reacTIVision-1.5.1-win64** folder and open the `reacTIVision` application (`reacTIVision.exe`).
2. Follow the instructions in the **reacTIVision** app to set up your camera and detect fiducial markers.

#### 3.2 Start reacTIVision

Make sure the reacTIVision app is running and set to send TUIO data to **localhost** on **port 3333**. This will send marker data to your Python script.

---

### 4. Running the Python Script

Now that **reacTIVision** is running and sending data, you can start the Python application.

1. Navigate to the **Python** folder.
2. Run the Python script with the following command:

```bash
python app.py
```

This script listens for TUIO data from reacTIVision and processes it to send marker information (position and rotation) to the C# Windows Forms app.

---

### 5. Running the C# Windows Forms App

#### 5.1 Open the C# Solution

1. Navigate to the **WindowsFormsApp1** folder and open `WindowsFormsApp1.sln` in **Visual Studio**.
2. Ensure your project is set to run as a Windows Forms application.

#### 5.2 Start the C# App

Once the project is open in Visual Studio, press **F5** (or click on the "Start" button) to run the application.

The C# app will open, displaying the **virtual fitting room** with the hoodie and trouser images. It will respond to marker rotations to cycle through the images.

---

## 📝 How It Works

- **Python**: Receives TUIO data from **reacTIVision**, processes it, and sends position/rotation data to the C# app over UDP.
- **reacTIVision**: Detects fiducial markers (e.g., `42` for hoodie, `1` for trouser) and sends their position and rotation data.
- **C# Windows Forms**: Displays the appropriate images (hoodies or trousers) based on the received marker data, cycling through images when the marker rotates.

---

## ⚙️ Troubleshooting

### 1. Python Dependencies

If you encounter issues with the Python dependencies, try upgrading `pip` and re-installing the requirements:

```bash
pip install --upgrade pip
pip install -r requirements.txt
```

### 2. UDP Communication Issues

Ensure that your firewall or antivirus software is not blocking UDP traffic on port **5005** (Python → C# communication).

---

## 📂 File Structure

```
HCI_Project-TUIO/
├── Python/
│   ├── app.py               # Python script for receiving TUIO data and sending to C#
│   ├── requirements.txt     # Required Python packages
├── reacTIVision-1.5.1-win64/
│   ├── reacTIVision.exe     # reacTIVision application for detecting markers
├── WindowsFormsApp1/
│   ├── WindowsFormsApp1.sln # C# Visual Studio solution file
│   └── ...                  # C# code files for the Windows Forms application
```

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🔧 Acknowledgements

- **reacTIVision**: [http://reactivision.sourceforge.net/](http://reactivision.sourceforge.net/) for fiducial marker detection.
- **Python-OSC**: [https://github.com/attwad/python-osc](https://github.com/attwad/python-osc) for handling OSC communication in Python.
