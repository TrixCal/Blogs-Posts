using System;
using System.IO;
using System.Linq;
using NLog.Web;

namespace BlogConsole
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            try{

                // Create and save a new Blog
                var db = new BloggingContext();
                /*Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = name };

                
                db.AddBlog(blog);
                logger.Info("Blog added - {name}", name);*/

                // Display all Blogs from the database
                var query = db.Blogs.OrderBy(b => b.Name);

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query){
                    Console.WriteLine(item.Name);
                }
            }
            catch (Exception ex){
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
