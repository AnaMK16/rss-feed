using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RssFeedAggregator;
using RssFeedAggregator.Models;

namespace rssFeed.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RssFeedController : ControllerBase
{
    private readonly AppDbContext _db;

    public RssFeedController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("read-all-feeds")]
    public async Task<List<Article>> GetAll()
    {
        return await _db.Articles.ToListAsync();
    }
    [HttpPost("read-all-articles-in-chronological-order")]
    public async Task<List<Article>> GetAllArticlesChronologically(int PageSize, int PageIndex)
    {
        return await _db.Articles.OrderBy(a => a.PublishDate)
            .Skip(PageIndex * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
    [HttpPost("search-article-by-tag")]
    public async Task<List<string>>SearchArticleByTag(string tag)
    {
        var articles = await  _db.Articles.Include(a => a.Tags)
            .Where(a => a.Tags.Any(t => t.Name == tag))
            .Select(a => a.Title)
            .ToListAsync();

        return articles;
    }
}