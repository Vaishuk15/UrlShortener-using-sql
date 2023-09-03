using Microsoft.EntityFrameworkCore;
using UrlShortener.Entity;
using UrlShortener.Services;

namespace UrlShortener
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ShortenedUrl>ShortenedUrls {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>(builder =>
            {
                builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharInShortUrl);
                builder.HasIndex(s=>s.Code).IsUnique();
            });
        }
    }
}
