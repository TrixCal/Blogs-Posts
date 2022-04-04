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
                string input = "";
                while(input.ToLower() != "q"){
                    Console.WriteLine("Enter your selection:");
                    Console.WriteLine("1) Display Blogs");
                    Console.WriteLine("2) Create Blog");
                    Console.WriteLine("3) Display Posts");
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
                        if(name != ""){
                            db.AddBlog(blog);
                            logger.Info($"Blog added - {name}");
                        }
                        else
                            logger.Error("Blog name cannot be null");
                    }else if(input == "3"){
                        
                    }else if(input == "4"){
                        logger.Info("Option 4 selected");
                        
                        //display blogs
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("Select the blog you would like to post to: ");
                        foreach(var item in query){
                            Console.WriteLine($"{item.BlogId}) {item.Name}");
                        }
                        //blog checks
                        int blogId;
                        bool validCheck = int.TryParse(Console.ReadLine(), out blogId);
                        if(validCheck){
                            bool existCheck = db.Blogs.Any(b => b.BlogId == blogId);
                            if(existCheck){
                                //post creation
                                Console.WriteLine("Enter the Post title");
                                string title = Console.ReadLine();
                                if(title != ""){
                                    Console.WriteLine("Enter the Post content");
                                    string content = Console.ReadLine();
                                    var post = new Post() { Title = title, Content = content, BlogId = blogId };
                                    //add post to db
                                    db.AddPost(post);
                                    logger.Info($"Post added - \"{title}\"");
                                }
                                else
                                    logger.Error("Post title cannot be null");
                            }
                            else
                                logger.Error("There are no Blogs saved with that Id");
                        }
                        else
                            logger.Error("Invalid Blog Id");
                    }
                    Console.WriteLine();
                }

            logger.Info("Program ended");
        }
    }
}
