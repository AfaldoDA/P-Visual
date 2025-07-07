using System;
using System.Windows.Forms;

namespace GuestRegistrationApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // untuk .NET 6 ke atas
            Application.Run(new GuestRegistrationApp.Views.MainForm()); // panggil namespace lengkap langsung
        }
    }
}
