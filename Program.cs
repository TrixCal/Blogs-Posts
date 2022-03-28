using System;
using System.IO;
using NLog.Web;

namespace BlogConsole
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            Console.WriteLine("Hello World!");

            logger.Info("Program ended");
        }
    }
}
