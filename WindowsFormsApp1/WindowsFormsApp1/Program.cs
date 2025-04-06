using System;
using System.Windows.Forms;
using VirtualFittingRoom; // Make sure to include this if your form is in a different namespace

namespace VirtualFittingRoom  // Change this to match your Form1 namespace
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // Ensure Form1 exists in this namespace
        }
    }
}
