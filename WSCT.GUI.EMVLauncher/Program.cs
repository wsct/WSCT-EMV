using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WSCT.GUI.EMVLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WSCT.GUI.WinSCardGUI());

            // Force close all other existing ApplicationID.Run() 
            Application.Exit();
        }
    }
}
