namespace VirtualFittingRoom
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnFaceIDLogin = new Button();
            SignIn = new Button();
            loginPass = new TextBox();
            loginUser = new TextBox();
            label5 = new Label();
            label6 = new Label();
            tabPage2 = new TabPage();
            btnUploadPhoto = new Button();
            pictureBox = new PictureBox();
            gender = new Label();
            cmbGender = new ComboBox();
            SignUp = new Button();
            Password = new TextBox();
            Username = new TextBox();
            label3 = new Label();
            label4 = new Label();
            LastName = new TextBox();
            FirstName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(32, 16);
            tabControl1.Margin = new Padding(3, 2, 3, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(542, 302);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnFaceIDLogin);
            tabPage1.Controls.Add(SignIn);
            tabPage1.Controls.Add(loginPass);
            tabPage1.Controls.Add(loginUser);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label6);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(3, 2, 3, 2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3, 2, 3, 2);
            tabPage1.Size = new Size(534, 274);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Sign In";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnFaceIDLogin
            // 
            btnFaceIDLogin.Location = new Point(166, 138);
            btnFaceIDLogin.Margin = new Padding(3, 2, 3, 2);
            btnFaceIDLogin.Name = "btnFaceIDLogin";
            btnFaceIDLogin.Size = new Size(125, 29);
            btnFaceIDLogin.TabIndex = 13;
            btnFaceIDLogin.Text = "Login With Face ID";
            btnFaceIDLogin.UseVisualStyleBackColor = true;
            btnFaceIDLogin.Click += btnFaceIDLogin_Click;
            // 
            // SignIn
            // 
            SignIn.Location = new Point(37, 138);
            SignIn.Margin = new Padding(3, 2, 3, 2);
            SignIn.Name = "SignIn";
            SignIn.Size = new Size(87, 29);
            SignIn.TabIndex = 12;
            SignIn.Text = "Sign In";
            SignIn.UseVisualStyleBackColor = true;
            SignIn.Click += SignIn_Click;
            // 
            // loginPass
            // 
            loginPass.Location = new Point(182, 67);
            loginPass.Margin = new Padding(3, 2, 3, 2);
            loginPass.Name = "loginPass";
            loginPass.Size = new Size(110, 23);
            loginPass.TabIndex = 11;
            loginPass.UseSystemPasswordChar = true;
            // 
            // loginUser
            // 
            loginUser.Location = new Point(37, 67);
            loginUser.Margin = new Padding(3, 2, 3, 2);
            loginUser.Name = "loginUser";
            loginUser.Size = new Size(110, 23);
            loginUser.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(37, 48);
            label5.Name = "label5";
            label5.Size = new Size(60, 15);
            label5.TabIndex = 9;
            label5.Text = "Username";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(182, 48);
            label6.Name = "label6";
            label6.Size = new Size(57, 15);
            label6.TabIndex = 8;
            label6.Text = "Password";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(btnUploadPhoto);
            tabPage2.Controls.Add(pictureBox);
            tabPage2.Controls.Add(gender);
            tabPage2.Controls.Add(cmbGender);
            tabPage2.Controls.Add(SignUp);
            tabPage2.Controls.Add(Password);
            tabPage2.Controls.Add(Username);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(LastName);
            tabPage2.Controls.Add(FirstName);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(label1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(3, 2, 3, 2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3, 2, 3, 2);
            tabPage2.Size = new Size(534, 274);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Sign Up";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnUploadPhoto
            // 
            btnUploadPhoto.Location = new Point(388, 198);
            btnUploadPhoto.Margin = new Padding(3, 2, 3, 2);
            btnUploadPhoto.Name = "btnUploadPhoto";
            btnUploadPhoto.Size = new Size(111, 22);
            btnUploadPhoto.TabIndex = 12;
            btnUploadPhoto.Text = "Upload Photo";
            btnUploadPhoto.UseVisualStyleBackColor = true;
            btnUploadPhoto.Click += btnUploadPhoto_Click;
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(374, 68);
            pictureBox.Margin = new Padding(3, 2, 3, 2);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(140, 123);
            pictureBox.TabIndex = 11;
            pictureBox.TabStop = false;
            // 
            // gender
            // 
            gender.AutoSize = true;
            gender.Location = new Point(56, 176);
            gender.Name = "gender";
            gender.Size = new Size(45, 15);
            gender.TabIndex = 10;
            gender.Text = "Gender";
            // 
            // cmbGender
            // 
            cmbGender.FormattingEnabled = true;
            cmbGender.Location = new Point(56, 194);
            cmbGender.Margin = new Padding(3, 2, 3, 2);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(110, 23);
            cmbGender.TabIndex = 9;
            cmbGender.Text = "Gender";
            // 
            // SignUp
            // 
            SignUp.Location = new Point(209, 193);
            SignUp.Margin = new Padding(3, 2, 3, 2);
            SignUp.Name = "SignUp";
            SignUp.Size = new Size(88, 27);
            SignUp.TabIndex = 8;
            SignUp.Text = "Sign Up";
            SignUp.UseVisualStyleBackColor = true;
            SignUp.Click += SignUp_Click;
            // 
            // Password
            // 
            Password.Location = new Point(201, 136);
            Password.Margin = new Padding(3, 2, 3, 2);
            Password.Name = "Password";
            Password.Size = new Size(110, 23);
            Password.TabIndex = 7;
            Password.UseSystemPasswordChar = true;
            // 
            // Username
            // 
            Username.Location = new Point(56, 136);
            Username.Margin = new Padding(3, 2, 3, 2);
            Username.Name = "Username";
            Username.Size = new Size(110, 23);
            Username.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(56, 117);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 5;
            label3.Text = "Username";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(201, 117);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 4;
            label4.Text = "Password";
            // 
            // LastName
            // 
            LastName.Location = new Point(201, 68);
            LastName.Margin = new Padding(3, 2, 3, 2);
            LastName.Name = "LastName";
            LastName.Size = new Size(110, 23);
            LastName.TabIndex = 3;
            // 
            // FirstName
            // 
            FirstName.Location = new Point(56, 68);
            FirstName.Margin = new Padding(3, 2, 3, 2);
            FirstName.Name = "FirstName";
            FirstName.Size = new Size(110, 23);
            FirstName.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(56, 50);
            label2.Name = "label2";
            label2.Size = new Size(64, 15);
            label2.TabIndex = 1;
            label2.Text = "First Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(201, 50);
            label1.Name = "label1";
            label1.Size = new Size(63, 15);
            label1.TabIndex = 0;
            label1.Text = "Last Name";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(579, 38);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(268, 280);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(858, 378);
            Controls.Add(pictureBox1);
            Controls.Add(tabControl1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "LoginForm";
            Text = "LoginForm";
            Load += LoginForm_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox LastName;
        private TextBox FirstName;
        private Label label2;
        private Label label1;
        private TextBox Password;
        private TextBox Username;
        private Label label3;
        private Label label4;
        private Button SignUp;
        private Button SignIn;
        private TextBox loginPass;
        private TextBox loginUser;
        private Label label5;
        private Label label6;
        private Label gender;
        private ComboBox cmbGender;
        private PictureBox pictureBox1;
        private Button btnUploadPhoto;
        private PictureBox pictureBox;
        private Button btnFaceIDLogin;
    }
}