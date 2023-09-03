using Microsoft.EntityFrameworkCore;
using UrlShortener;
using UrlShortener.Entity;
using UrlShortener.Models;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<UrlShorteningService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/shorten", (ShortenUrlRequest request,
    UrlShorteningService urlShorteningService,
    ApplicationDbContext dbContext,
    HttpContext httpContext) =>
{
    if(!Uri.TryCreate(request.Url,UriKind.Absolute,out _))
    {
        return Results.BadRequest("The Url is invalid");
    }
    var code =  urlShorteningService.GenerateUniqueCode();
    var shortenedUrl = new ShortenedUrl
    {
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
        CreatedOn = DateTime.Now,
    };
    dbContext.ShortenedUrls.Add(shortenedUrl);
    dbContext.SaveChanges();
    return Results.Ok(shortenedUrl.ShortUrl);

});

app.MapGet("api/{code}", async (string code, ApplicationDbContext dbContext) =>
{
    var shortenedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);
    if(shortenedUrl== null)
    {
        return Results.NotFound();
    }
    return Results.Redirect(shortenedUrl.LongUrl);
});

app.UseHttpsRedirection();
app.Run();