using Microsoft.EntityFrameworkCore;
using RssFeedAggregator;

List<String> urls = new List<string>()
{
    "https://stackoverflow.blog/feed/",
    "https://www.freecodecamp.org/news/rss",
    "https://martinfowler.com/feed.atom",
    "https://codeblog.jonskeet.uk/feed/",
    "https://devblogs.microsoft.com/visualstudio/feed/",
    "https://feed.infoq.com/",
    "https://css-tricks.com/feed/",
    "https://codeopinion.com/feed/",
    "https://andrewlock.net/rss.xml",
    "https://michaelscodingspot.com/index.xml",
    "https://www.tabsoverspaces.com/feed.xml"
};

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlServer("Server=localhost;Database=RssFeedDb;User Id=sa;Password=Anabec3451.;Encrypt=False;");

var db = new AppDbContext(optionsBuilder.Options);
db.Database.EnsureCreated();


var aggregator = new RssFeedAggreggator(optionsBuilder.Options);
while (true)
{
    aggregator.ProcessRssFeeds(urls);
   
    Task.Delay(2000);
}