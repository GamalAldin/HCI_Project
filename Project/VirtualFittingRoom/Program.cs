using System;
using System.Windows.Forms;
using VirtualFittingRoom;

namespace VirtualFittingRoom
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the app.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
