using Microsoft.EntityFrameworkCore;
namespace BlogConsole{
    public class BloggingContext : DbContext{
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public void AddBlog(Blog blog){
            this.Blogs.Add(blog);
            this.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBilder optionsBuilder){
            //remove from github
            optionsBuilder.UseSqlServer();
        }
    }
}