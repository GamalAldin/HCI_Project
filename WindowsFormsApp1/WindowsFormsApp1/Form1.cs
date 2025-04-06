using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace VirtualFittingRoom
{
    public partial class Form1 : Form
    {
        UdpClient udpClient;
        PictureBox hoodiePictureBox, trouserPictureBox;
        Label hoodieLabel, trouserLabel;
        int hoodieMarkerID = 42;
        int trouserMarkerID = 1;
        float lastHoodieRotation = 0.0f;
        float lastTrouserRotation = 0.0f;
        int currentHoodieIndex = 0;
        int currentTrouserIndex = 0;

        string[] hoodieImages = { "blue.png", "red.png", "green.png" };
        string[] trouserImages = { "trouser1.png", "trouser2.png", "trouser3.png" };
        Image[] hoodieImageCache, trouserImageCache;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Virtual Fitting Room";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White; // Ensure background is visible

            // Create Labels
            hoodieLabel = CreateLabel("T-Shirts", new Point(100, 20));
            trouserLabel = CreateLabel("Trousers", new Point(500, 20));

            // Load images into memory
            hoodieImageCache = LoadImages(hoodieImages);
            trouserImageCache = LoadImages(trouserImages);

            // Create PictureBoxes
            hoodiePictureBox = CreatePictureBox(new Point(100, 80), hoodieImageCache[currentHoodieIndex]);
            trouserPictureBox = CreatePictureBox(new Point(500, 80), trouserImageCache[currentTrouserIndex]);

            // Add Labels AFTER PictureBoxes to ensure they are visible
            this.Controls.Add(hoodieLabel);
            this.Controls.Add(trouserLabel);

            hoodieLabel.BringToFront();
            trouserLabel.BringToFront();

            // Initialize UDP listener
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 5005));
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            var receiveThread = new System.Threading.Thread(ReceiveData)
            {
                IsBackground = true
            };
            receiveThread.Start();
        }

        private Image[] LoadImages(string[] imagePaths)
        {
            Image[] images = new Image[imagePaths.Length];
            for (int i = 0; i < imagePaths.Length; i++)
            {
                images[i] = Image.FromFile(imagePaths[i]);
            }
            return images;
        }

        private Label CreateLabel(string text, Point location)
        {
            Label label = new Label
            {
                Text = text,
                Font = new Font("Arial", 16, FontStyle.Bold),
                Size = new Size(200, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Black,
                BackColor = Color.White, // Ensure visibility over images
                Location = location
            };
            return label;
        }

        private PictureBox CreatePictureBox(Point location, Image image)
        {
            PictureBox pictureBox = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(200, 300),
                Location = location,
                BorderStyle = BorderStyle.FixedSingle // Adds a black single-line border
            };
            this.Controls.Add(pictureBox);
            return pictureBox;
        }


        private void ReceiveData()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 5005);

            while (true)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);
                    string[] parts = message.Split(',');

                    if (parts.Length >= 4)
                    {
                        int objId = int.Parse(parts[0]);
                        float x = float.Parse(parts[1]);
                        float y = float.Parse(parts[2]);
                        float rotationRadians = float.Parse(parts[3]);
                        float rotationDegrees = rotationRadians * (180.0f / (float)Math.PI);

                        this.Invoke((MethodInvoker)delegate {
                            if (objId == hoodieMarkerID)
                            {
                                float rotationDiff = NormalizeAngle(rotationDegrees - lastHoodieRotation);
                                if (rotationDiff > 10)
                                {
                                    currentHoodieIndex = (currentHoodieIndex + 1) % hoodieImageCache.Length;
                                    hoodiePictureBox.Image = hoodieImageCache[currentHoodieIndex];
                                }
                                else if (rotationDiff < -10)
                                {
                                    currentHoodieIndex = (currentHoodieIndex - 1 + hoodieImageCache.Length) % hoodieImageCache.Length;
                                    hoodiePictureBox.Image = hoodieImageCache[currentHoodieIndex];
                                }
                                lastHoodieRotation = rotationDegrees;
                            }
                            else if (objId == trouserMarkerID)
                            {
                                float rotationDiff = NormalizeAngle(rotationDegrees - lastTrouserRotation);
                                if (rotationDiff > 10)
                                {
                                    currentTrouserIndex = (currentTrouserIndex + 1) % trouserImageCache.Length;
                                    trouserPictureBox.Image = trouserImageCache[currentTrouserIndex];
                                }
                                else if (rotationDiff < -10)
                                {
                                    currentTrouserIndex = (currentTrouserIndex - 1 + trouserImageCache.Length) % trouserImageCache.Length;
                                    trouserPictureBox.Image = trouserImageCache[currentTrouserIndex];
                                }
                                lastTrouserRotation = rotationDegrees;
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private float NormalizeAngle(float angle)
        {
            while (angle > 180) angle -= 360;
            while (angle < -180) angle += 360;
            return angle;
        }
    }
}
