using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_Database
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());

            while (AppData.isLogin)
            {
                switch (AppData.openingScreen)
                {
                    case "Info":
                        Application.Run(new Login());
                        break;
                    default:
                        Application.Run(new Form1());
                        break;
                }
            }
        }

    }
}
