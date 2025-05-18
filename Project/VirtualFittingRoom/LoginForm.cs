using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Sockets;
using System.Text;

namespace VirtualFittingRoom
{
    public partial class LoginForm : Form
    {
        private readonly string usersDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users");
        private string uploadedImagePath = null;

        public LoginForm()
        {
            InitializeComponent();

            // Ensure users directory exists
            if (!Directory.Exists(usersDirectory))
                Directory.CreateDirectory(usersDirectory);

            // Populate gender combo box
            cmbGender.Items.AddRange(new[] { "Male", "Female" });
            cmbGender.SelectedIndex = 0; // Default
        }

        private void SignUp_Click(object sender, EventArgs e)
        {
            string username = Username.Text.Trim();
            string userFolder = Path.Combine(usersDirectory, username);

            if (Directory.Exists(userFolder))
            {
                MessageBox.Show("Username already exists.");
                return;
            }

            string gender = cmbGender.SelectedItem?.ToString() ?? "Unspecified";

            Directory.CreateDirectory(userFolder);
            string infoPath = Path.Combine(userFolder, "info.txt");

            File.WriteAllLines(infoPath, new[]
            {
                $"FirstName={FirstName.Text.Trim()}",
                $"LastName={LastName.Text.Trim()}",
                $"Username={username}",
                $"Password={Password.Text.Trim()}",
                $"Gender={gender}"
            });

            // Save uploaded photo if exists
            if (!string.IsNullOrEmpty(uploadedImagePath))
            {
                string destImagePath = Path.Combine(userFolder, "photo.jpg");
                File.Copy(uploadedImagePath, destImagePath, true);
            }

            MessageBox.Show("Registration successful!");
        }

        private void SignIn_Click(object sender, EventArgs e)
        {
            string username = loginUser.Text.Trim();
            string userFolder = Path.Combine(usersDirectory, username);
            string infoPath = Path.Combine(userFolder, "info.txt");

            if (!File.Exists(infoPath))
            {
                MessageBox.Show("User not found.");
                return;
            }

            var data = File.ReadAllLines(infoPath).ToDictionary(
                line => line.Split('=')[0],
                line => line.Split('=')[1]
            );

            if (data["Password"] != loginPass.Text.Trim())
            {
                MessageBox.Show("Incorrect password.");
                return;
            }

            string gender = data.ContainsKey("Gender") ? data["Gender"] : "Unspecified";

            // Login successful — open main form
            this.Hide();
            Form1 mainForm = new Form1(username, gender);
            mainForm.Show();
        }

        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.Title = "Select Profile Photo";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    uploadedImagePath = openFileDialog.FileName;
                    pictureBox.Image = Image.FromFile(uploadedImagePath);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        private void btnFaceIDLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new TcpClient("127.0.0.1", 9004))
                using (var stream = client.GetStream())
                {
                    // Trigger sent (optional)
                    stream.Write(new byte[] { 1 }, 0, 1);

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                    if (response == "UNKNOWN")
                    {
                        MessageBox.Show("Face not recognized.");
                        return;
                    }

                    string username = response;
                    string userFolder = Path.Combine(usersDirectory, username);
                    string infoPath = Path.Combine(userFolder, "info.txt");

                    if (File.Exists(infoPath))
                    {
                        var data = File.ReadAllLines(infoPath).ToDictionary(
                            line => line.Split('=')[0],
                            line => line.Split('=')[1]
                        );

                        string gender = data.ContainsKey("Gender") ? data["Gender"] : "Unspecified";
                        this.Hide();
                        Form1 mainForm = new Form1(username, gender);
                        mainForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("User data not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
