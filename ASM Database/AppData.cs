using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Database
{
    internal class AppData
    {
        public static bool isLogin = false;
        public static string openingScreen = "Login";
        public static void shutDownApp()
        {
            isLogin = false;
        }
    }
}
