using System;
using System.Windows.Forms;

namespace zAjedrez
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Front.MainForm());
        }
    }
}
