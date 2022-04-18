using System;
using System.IO;
using System.Linq;
using NLog.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
                        Console.WriteLine($"{db.Blogs.Count()} Blog(s) returned");
                        foreach (var item in query){
                            Console.WriteLine(item.Name);
                        }
                    }else if(input == "2"){
                        logger.Info("Option 2 selected");

                        Console.Write("Enter a name for a new Blog: ");
                        var blog = new Blog { Name = Console.ReadLine() };
                        ValidationContext context = new ValidationContext(blog, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(blog, context, results, true);
                        if(isValid){
                            //unique name
                            if(db.Blogs.Any(b => b.Name == blog.Name)){
                                isValid = false;
                                results.Add(new ValidationResult("Blog name exists", new string[] {"Name"}));
                            }
                            else{
                                logger.Info("Validation passed");
                                db.AddBlog(blog);
                                logger.Info($"Blog added - {blog.Name}");
                            }
                        }
                        if(!isValid){
                            foreach(var result in results){
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }else if(input == "3"){
                        logger.Info("Option 3 selected");

                        //display blogs
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("0) Posts from all blogs");
                        foreach(var item in query){
                            Console.WriteLine($"{item.BlogId}) Posts from {item.Name}");
                        }
                        int blogId;
                        bool validCheck = int.TryParse(Console.ReadLine(), out blogId);
                        if(validCheck){
                            if(blogId == 0){
                                //creates a joined query in order to pull the blog name from table
                                //selects all posts
                                var joinQuery = from p in db.Posts
                                                join b in db.Blogs on p.BlogId equals b.BlogId
                                                select new {
                                                    BlogName = b.Name,
                                                    PostTitle = p.Title,
                                                    PostContent = p.Content
                                                };
                                Console.WriteLine($"{joinQuery.Count()} post(s) returned");
                                foreach(var item in joinQuery){
                                    Console.WriteLine($"Blog: {item.BlogName}\nTitle: {item.PostTitle}\nContent: {item.PostContent}\n");
                                }
                            }
                            else{
                                bool existCheck = db.Blogs.Any(b => b.BlogId == blogId);
                                if(existCheck){
                                    //selects all posts that match the blogId selected
                                    var joinQuery = from p in db.Posts
                                                join b in db.Blogs on p.BlogId equals b.BlogId
                                                where p.BlogId == blogId
                                                select new {
                                                    BlogName = b.Name,
                                                    PostTitle = p.Title,
                                                    PostContent = p.Content
                                                };
                                    Console.WriteLine($"{joinQuery.Count()} post(s) returned");
                                    foreach(var item in joinQuery){
                                        Console.WriteLine($"Blog: {item.BlogName}\nTitle: {item.PostTitle}\nContent: {item.PostContent}\n");
                                    }
                                }
                                else
                                    logger.Error("There are no Blogs with that Id");
                            }
                        }
                        else
                            logger.Error("Invalid Blog Id");
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
