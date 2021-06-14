using Microsoft.EntityFrameworkCore;
using ReadNews;

namespace ReadNews.Models
{
    public class RssFeedDBContext : DbContext
    {
        public RssFeedDBContext(DbContextOptions<RssFeedDBContext> options)
            : base(options)
        {
        }

        public DbSet<RSSRepository> RssFeedItems { get; set; }
    }
}