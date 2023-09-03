namespace UrlShortener.Services
{
    public class UrlShorteningService
    {
        public const int NumberOfCharInShortUrl = 7;
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private readonly Random random = new ();
        private readonly ApplicationDbContext _dbContext;

        public UrlShorteningService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public string GenerateUniqueCode()
        {
            var codeChars = new char[NumberOfCharInShortUrl];
            while (true)
            {
                for (var i = 0; i < NumberOfCharInShortUrl; i++)
                {
                    var randomIndex = random.Next(Alphabet.Length - 1);
                    codeChars[i] = Alphabet[randomIndex];
                }
                var code = new string(codeChars);
                if (! _dbContext.ShortenedUrls.Any(s => s.Code == code))
                {
                    return code;
                }
            }
        }
    }
}
