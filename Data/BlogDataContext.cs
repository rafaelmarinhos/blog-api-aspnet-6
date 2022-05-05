using blog_api_aspnet_6.Data.Mappings;
using blog_api_aspnet_6.Models;
using Microsoft.EntityFrameworkCore;

namespace blog_api_aspnet_6.Data
{
    public class BlogDataContext : DbContext
    {
        public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options)
        {
        }

        public DbSet<Category>? Categories { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Post>? Posts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PostMap());
        }
    }
}
