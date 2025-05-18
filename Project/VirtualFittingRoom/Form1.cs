using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Svg;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using ExCSS;
using DrawingFontStyle = System.Drawing.FontStyle;
using ExCssFontStyle = ExCSS.FontStyle;
using DrawingPoint = System.Drawing.Point;
using ExCssPoint = ExCSS.Point;

namespace VirtualFittingRoom
{
    public partial class Form1 : Form
    {
        private enum Gender { Male, Female }
        private enum MenuState { MainMenu, SubMenu }

        private string currentUser;
        private Gender currentGender;
        private MenuState menuState = MenuState.MainMenu;

        private string[] mainCategories;
        private string[] mainIcons;
        private Bitmap[] mainIconsRendered;

        private string[] subMenuLabels;
        private Bitmap[] subMenuIcons;
        private int selectedIndex = -1;

        List<string> commands_tuio=new List<string>();

        private readonly string outfitsBasePath = @"D:/UNI/HCI/Projecthci/Project/Python/outfits";
        private readonly string arrowIconPath = "arrow.png";

        private string currentFolderName;

        private ListBox orderHistoryListBox;

        public Form1(string username, string gender)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;
            this.MouseClick += Form1_MouseClick;

            currentUser = username;
            currentGender = gender == "Male" ? Gender.Male : Gender.Female;

            commands_tuio.Add("rotate-right");
            commands_tuio.Add("rotate-left");
            commands_tuio.Add("select");
            

            AddOrderHistoryListBox();
            LoadMainMenu();
            StartSocketListener();
            SendUsernameToPython(currentUser);
            LoadOrderHistory();
        }

        private void AddOrderHistoryListBox()
        {
            orderHistoryListBox = new ListBox();
            orderHistoryListBox.Dock = DockStyle.Bottom;
            orderHistoryListBox.Height = 120;
            orderHistoryListBox.Font = new Font("Segoe UI", 10);
            this.Controls.Add(orderHistoryListBox);
        }

        private void LoadOrderHistory()
        {
            orderHistoryListBox.Items.Clear();
            string usersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users");
            string userDir = Path.Combine(usersPath, currentUser);
            string filePath = Path.Combine(userDir, "orders.txt");

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    orderHistoryListBox.Items.Add(line);
                }
            }
            else
            {
                orderHistoryListBox.Items.Add("No orders placed yet.");
            }
        }

        private void CreateOrderFile(string category, string color)
        {
            try
            {
                string usersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users");
                string userDir = Path.Combine(usersPath, currentUser);
                Directory.CreateDirectory(userDir);

                string filePath = Path.Combine(userDir, "orders.txt");
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string entry = $"{timestamp} - {category}, {color}";

                File.AppendAllText(filePath, entry + Environment.NewLine);
                MessageBox.Show($"Order placed!\n{entry}", "Order Confirmed");
                LoadOrderHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing order file: " + ex.Message);
            }
        }
        private void CreatReturnFile()
        {
            try
            {

                string category = "Tie";
                string color = "Red";

                string usersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users");
                string userDir = Path.Combine(usersPath, currentUser);

                string filePath = Path.Combine(userDir, "order_return.txt");
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string entry = $"{timestamp} - {category}, {color}";

                File.AppendAllText(filePath, entry + Environment.NewLine);
                MessageBox.Show($"Order Returned!\n{entry}", "Sorry For Not Matching Your Standerds");
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing return file: " + ex.Message);
            }
        }
        private void SendUsernameToPython(string username)
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 9003))
                using (StreamWriter writer = new StreamWriter(client.GetStream()))
                {
                    writer.WriteLine(username);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send username to Python: " + ex.Message);
            }
        }

        private void LoadMainMenu()
        {
            menuState = MenuState.MainMenu;
            mainCategories = new string[] {
                "T-Shirts", "Trousers", "Hoodies",
                currentGender == Gender.Male ? "Polo Shirts" : "Dresses"
            };
            mainIcons = new string[] {
                "tshirt.svg", "trousers.svg", "hoodies.svg",
                currentGender == Gender.Male ? "polo.svg" : "dresses.svg"
            };
            mainIconsRendered = mainIcons.Select(path =>
            {
                if (File.Exists(path))
                    return SvgDocument.Open(path).Draw(64, 64);
                return new Bitmap(64, 64);
            }).ToArray();

            selectedIndex = -1;
            this.Invalidate();
        }

        private void LoadSubMenu(string category)
        {
            menuState = MenuState.SubMenu;

            var folderMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "T-Shirts", "shirts" },
                { "Trousers", "trousers" },
                { "Hoodies", "hoodies" },
                { "Polo Shirts", "polo" },
                { "Dresses", "dresses" }
            };

            string folderName = folderMappings.ContainsKey(category) ? folderMappings[category] : category.ToLower().Replace(" ", "");
            currentFolderName = folderName;

            string genderFolder = currentGender == Gender.Male ? "male" : "female";
            string folderPath = Path.Combine(outfitsBasePath, genderFolder, folderName);

            string[] images = Directory.Exists(folderPath)
                ? Directory.GetFiles(folderPath, "*.png")
                : new string[0];

            subMenuLabels = images.Select(img => Path.GetFileNameWithoutExtension(img))
                                  .Append("Go Back").ToArray();

            subMenuIcons = images.Select(img => new Bitmap(img))
                                 .Append(new Bitmap(arrowIconPath)).ToArray();

            selectedIndex = -1;
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int radius = 200;
            int iconSize = 64;
            FontFamily fontFamily = new FontFamily("Arial");
            Font labelFont = new Font("Arial", 9, DrawingFontStyle.Bold);
            DrawingPoint center = new DrawingPoint(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

            string[] labels = menuState == MenuState.MainMenu ? mainCategories : subMenuLabels;
            Bitmap[] icons = menuState == MenuState.MainMenu ? mainIconsRendered : subMenuIcons;

            float anglePerItem = 360f / labels.Length;

            for (int i = 0; i < labels.Length; i++)
            {
                using (Brush brush = new SolidBrush(i == selectedIndex ? System.Drawing.Color.LightSkyBlue : System.Drawing.Color.White))
                {
                    g.FillPie(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2, i * anglePerItem, anglePerItem);
                }

                using (Pen pen = new Pen(System.Drawing.Color.Gray, 2))
                {
                    g.DrawPie(pen, center.X - radius, center.Y - radius, radius * 2, radius * 2, i * anglePerItem, anglePerItem);
                }

                double angle = (i + 0.5) * (2 * Math.PI / labels.Length);
                int iconX = center.X + (int)((radius - 80) * Math.Cos(angle)) - iconSize / 2;
                int iconY = center.Y + (int)((radius - 80) * Math.Sin(angle)) - iconSize / 2;
                g.DrawImage(icons[i], iconX, iconY, iconSize, iconSize);

                SizeF labelSize = g.MeasureString(labels[i], labelFont);
                int textX = iconX + iconSize / 2 - (int)(labelSize.Width / 2);
                int textY = iconY + iconSize + 5;
                g.DrawString(labels[i], labelFont, Brushes.Black, textX, textY);
            }

            g.FillEllipse(Brushes.White, center.X - 60, center.Y - 60, 120, 120);
            g.DrawEllipse(Pens.Gray, center.X - 60, center.Y - 60, 120, 120);
            string centerText = menuState == MenuState.MainMenu ? "Categories" : "Back";
            SizeF centerTextSize = g.MeasureString(centerText, new Font("Arial", 12, DrawingFontStyle.Bold));
            g.DrawString(centerText, new Font("Arial", 12, DrawingFontStyle.Bold),
                         Brushes.Black, center.X - centerTextSize.Width / 2, center.Y - centerTextSize.Height / 2);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            DrawingPoint center = new DrawingPoint(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
            double dx = e.X - center.X;
            double dy = e.Y - center.Y;
            double angle = Math.Atan2(dy, dx);
            if (angle < 0) angle += 2 * Math.PI;

            double distance = Math.Sqrt(dx * dx + dy * dy);
            if (distance < 60 || distance > 200) return;

            string[] labels = menuState == MenuState.MainMenu ? mainCategories : subMenuLabels;
            int clickedIndex = (int)(angle / (2 * Math.PI / labels.Length));

            if (menuState == MenuState.MainMenu)
            {
                LoadSubMenu(labels[clickedIndex]);
            }
            else
            {
                selectedIndex = clickedIndex;
                this.Invalidate();
                Application.DoEvents();

                if (labels[clickedIndex] == "Go Back")
                {
                    LoadMainMenu();
                }
                else
                {
                    string selectedLabel = labels[clickedIndex];
                    string genderFolder = currentGender == Gender.Male ? "male" : "female";
                    string imagePath = Path.Combine(outfitsBasePath, genderFolder, currentFolderName, selectedLabel + ".png");
                    SendImagePathToPython(imagePath);
                }
            }
        }

        private void StartSocketListener()
        {
            Thread thread = new Thread(() =>
            {
                TcpListener listener = new TcpListener(IPAddress.Any, 9001);
                listener.Start();

                while (true)
                {
                    using (TcpClient client = listener.AcceptTcpClient())
                    using (NetworkStream stream = client.GetStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string data = reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            data = data.Trim().ToLower();

                            if (commands_tuio.Contains(data))
                            {
                                HandleTuioCommand(data);

                            }
                            if (data.StartsWith("return"))
                            {
                                CreatReturnFile();
                            }
                        }
                    }
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private void HandleTuioCommand(string command)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string[] labels = menuState == MenuState.MainMenu ? mainCategories : subMenuLabels;
                int count = labels.Length;

                if (command == "rotate-right")
                {
                    selectedIndex = (selectedIndex + 1) % count;
                    this.Invalidate();
                }
                else if (command == "rotate-left")
                {
                    selectedIndex = (selectedIndex - 1 + count) % count;
                    this.Invalidate();
                }
                else if (command == "select" && selectedIndex != -1)
                {
                    if (menuState == MenuState.MainMenu)
                    {
                        LoadSubMenu(labels[selectedIndex]);
                    }
                    else
                    {
                        if (labels[selectedIndex] == "Go Back")
                        {
                            LoadMainMenu();
                        }
                        else
                        {
                            string selectedLabel = labels[selectedIndex];
                            string genderFolder = currentGender == Gender.Male ? "male" : "female";
                            string imagePath = Path.Combine(outfitsBasePath, genderFolder, currentFolderName, selectedLabel + ".png");
                            SendImagePathToPython(imagePath);
                        }
                    }
                }
                else if (command.StartsWith("place-order:"))
                {
                    string payload = command.Substring("place-order:".Length);
                    string[] parts = payload.Split(',');
                    if (parts.Length == 2)
                    {
                        CreateOrderFile(parts[0].Trim(), parts[1].Trim());
                    }
                }
            });
        }
    

        private void SendImagePathToPython(string imagePath)
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 9002))
                using (StreamWriter writer = new StreamWriter(client.GetStream()))
                {
                    writer.WriteLine(imagePath);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send image path: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
