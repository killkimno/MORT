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

            //args[4]: expectedHash (SHA256 hex). 없으면 빈 문자열 -> Updater 내부에서 검증 건너뛰기(구 버전 호환)
            string expectedHash = "";
            if (args.Length >= 5)
            {
                expectedHash = args[4];
            }

            Application.Run(new Updater(args[0], args[1], args[2], localize, expectedHash));
        }
    }
}