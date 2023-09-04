namespace Updater
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if(args.Length < 3 )
            {
                return;
            }

            string localize = "";
            if(args.Length >= 4)
            {
                localize = args[3];
            }

            Application.Run(new Updater(args[0], args[1], args[2], localize));
        }
    }
}