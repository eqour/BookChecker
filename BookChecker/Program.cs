using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using LinkCheckerLib;

namespace WFInterface
{
    static class Program
    {
        public static HandleLinksInfo HandleLinksInfo { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string startPath = AppContext.BaseDirectory;
            startPath = startPath.Substring(0, startPath.LastIndexOf('\\') + 1);
            //MessageBox.Show(startPath);

            HandleLinksInfo = ConfigLoader.LoadHandleLinksInfo(startPath + "config.xml");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
