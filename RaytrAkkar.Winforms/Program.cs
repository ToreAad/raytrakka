using Akka.Actor;
using Akka.Configuration;
using RaytrAkkar.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaytrAkkar.Winforms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var config = ConfigurationFactory.Load();
            using (var system = ActorSystem.Create("raytrakkar", config))
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new RaytrAkkaForm(system));
            }
        }
    }
}
