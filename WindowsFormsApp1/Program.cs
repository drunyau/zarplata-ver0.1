using System;
using System.Windows.Forms;
using ClassLibrary1;


namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataAccess.InitializeDatabase();
            Application.Run(new Form1());
            
        }
    }
}
