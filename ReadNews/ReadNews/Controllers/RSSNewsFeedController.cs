using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReadNews;
using ReadNews.DBModel;
using ReadNews.Models;

namespace ReadNewsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSSNewsFeedController : ControllerBase
    {
        private RssFeedDBContext _context;

        public RSSNewsFeedController(RssFeedDBContext context)
        {
            _context = context;
        }

        // GET: api/RSSNewsFeed
        [HttpGet]
        public async Task<ActionResult> Get()
        {           
            var rssFeedItems = await _context.RssFeedItems
                .Select(x => ListItemToDTO(x))
                .ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(rssFeedItems));
        }

        // GET: api/RSSNewsFeed/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<RssFeedItemDTO>> Get(int id)
        {
            var rssFeedItem = await _context.RssFeedItems.FindAsync(id);

            if (rssFeedItem == null)
            {
                return NotFound();
            }
            return new JsonResult(JsonConvert.SerializeObject(ItemToDTO(rssFeedItem)));
        }

        // POST: api/RSSNewsFeed
        [HttpPost]
        public async void Post([FromBody] object rssFeedUrl)
        {
            var feed = await FeedReader.ReadAsync(rssFeedUrl.ToString());
            _context.Database.EnsureDeleted();
            _context.RssFeedItems.AddRange(feed.Items
                .Where(x => x.PublishingDate.HasValue)
                .Select((x, id) => new RSSRepository()
                {
                    Title = x.Title,
                    Description = x.Description,
                    Link = x.Link,
                    PublishingDate = x.PublishingDate
                }));

            await _context.SaveChangesAsync();
        }

        private static RssFeedListDTO ListItemToDTO(RSSRepository rssRepository) =>
           new RssFeedListDTO
           {
               Id = rssRepository.Id,
               Title = rssRepository.Title,
               PublishingDate = rssRepository.PublishingDate
           };

        private static RssFeedItemDTO ItemToDTO(RSSRepository rssRepository) =>
        new RssFeedItemDTO
        {
            Id = rssRepository.Id,
            Title = rssRepository.Title,
            PublishingDate = rssRepository.PublishingDate,
            Description = rssRepository.Description,
            Link = rssRepository.Link
        };

        //private string GetRssFeedUrl()
        //{
        //    return "http://rss.cnn.com/rss/edition.rss";
        //}
    }
}
