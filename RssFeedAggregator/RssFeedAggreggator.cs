using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using RssFeedAggregator.Models;

namespace RssFeedAggregator;
public class RssFeedAggreggator
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
   

    public RssFeedAggreggator(DbContextOptions<AppDbContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
     
    }
     public void ProcessRssFeeds(List<string> feeds)
        {
            var allTags = new List<Tag>();

            Parallel.ForEach(feeds, feedUrl =>
            {
                using var db = new AppDbContext(_dbContextOptions);
                var reader = XmlReader.Create(feedUrl);
                var syndicationFeed = SyndicationFeed.Load(reader);
                reader.Close();
                var tags = new List<Tag>();
                foreach (var syndicationItem in syndicationFeed.Items)
                {
                    if (db.Articles.Any(a => a.Title == syndicationItem.Title.Text && a.Link == feedUrl))
                    {
                        continue;
                    }
                    tags.AddRange(syndicationItem.Categories?.Select(c => new Tag { Name = c.Name })!);
                    var article = new Article();
                    article.Title = syndicationItem.Title.Text;
                    article.Link = feedUrl;
                    article.Author = syndicationItem.Authors.FirstOrDefault()?.Name;
                    article.Description = syndicationItem.Summary.Text;
                    article.Image = syndicationItem.Links.FirstOrDefault(l => l.MediaType == "image/")?.Uri.ToString();
                    article.PublishDate = syndicationItem.PublishDate.UtcDateTime;
                    db.Articles.Add(article);
                    db.SaveChanges();
                }

                allTags.AddRange(tags.Distinct());
                ProcessTags(tags.Distinct(), db);
            });

            using var allTagsDbContext = new AppDbContext(_dbContextOptions);
            ProcessTags(allTags.Distinct(), allTagsDbContext);
        }

        public void ProcessTags(IEnumerable<Tag> tags, AppDbContext db)
        {
            foreach (var tag in tags)
            {
                if (!db.Tags.Any(t => t.Name == tag.Name))
                {
                    db.Tags.Add(tag);
                    db.SaveChanges();
                }
            }
        }
    }