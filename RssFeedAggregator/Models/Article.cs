namespace RssFeedAggregator.Models;

public class Article
{
    public int Id { get; set; }
    public string Link { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Image { get; set; }
    public DateTime? PublishDate { get; set; }
    public  List<Tag>? Tags { get; set; }
}