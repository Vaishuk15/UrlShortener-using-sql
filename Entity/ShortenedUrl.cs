namespace UrlShortener.Entity
{
    public class ShortenedUrl
    {
        public Guid Id { get; set; }
        public string LongUrl { get; set; }=String.Empty;
        public string ShortUrl { get; set; }=String.Empty;
        public  string Code { get; set; }=String.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
