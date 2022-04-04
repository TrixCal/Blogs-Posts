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
            var db = new BloggingContext();
            try{
                string input = "";
                while(input.ToLower() != "q"){
                    Console.WriteLine("Enter your selection:");
                    Console.WriteLine("1) Display all Blogs");
                    Console.WriteLine("2) Create Blog");
                    Console.WriteLine("3) Display alll Posts");
                    Console.WriteLine("4) Create Post");
                    Console.WriteLine("Enter q to quit");
                    input = Console.ReadLine();
                    Console.Clear();
                    if(input == "1"){
                        logger.Info("Option 1 selected");

                        var query = db.Blogs.OrderBy(b => b.Name);
                        Console.WriteLine($"{db.Blogs.Count()} Blogs returned");
                        foreach (var item in query){
                            Console.WriteLine(item.Name);
                        }
                    }else if(input == "2"){
                        logger.Info("Option 2 selected");

                        Console.Write("Enter a name for a new Blog: ");
                        string name = Console.ReadLine();
                        var blog = new Blog { Name = name };
                        db.AddBlog(blog);
                        logger.Info($"Blog added - {name}");
                    }else if(input == "3"){

                    }else if(input == "4"){

                    }
                    Console.WriteLine();
                }
                // Create and save a new Blog
                /*Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = name };

                
                db.AddBlog(blog);
                logger.Info("Blog added - {name}", name);*/
            }
            catch (Exception ex){
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
