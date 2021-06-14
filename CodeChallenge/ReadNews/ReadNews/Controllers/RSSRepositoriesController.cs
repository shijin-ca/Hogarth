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
    public class RSSRepositoriesController : ControllerBase
    {
        private readonly RssFeedDBContext _context;

        public RSSRepositoriesController(RssFeedDBContext context)
        {
            _context = context;
        }

        // GET: api/RSSRepositories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RssFeedListDTO>>> GetRssFeedItems()
        {
            var rssFeedItems = await _context.RssFeedItems
                .Select(x => ListItemToDTO(x))
                .ToListAsync();

            return new JsonResult(JsonConvert.SerializeObject(rssFeedItems));
            //return await _context.RssFeedItems.ToListAsync();
        }

        // GET: api/RSSRepositories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RssFeedItemDTO>> GetRSSRepository(int id)
        {
            var rssFeedItem = await _context.RssFeedItems.FindAsync(id);
            if (rssFeedItem == null)
            {
                return NotFound();
            }
            return new JsonResult(JsonConvert.SerializeObject(ItemToDTO(rssFeedItem)));
        }
 
        // POST: api/RSSRepositories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RSSRepository>> PostRSSRepository(object rSSRepository)
        {
            
            var feed = await FeedReader.ReadAsync(rSSRepository.ToString());
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

            //return Ok();
            return CreatedAtAction("GetRSSRepository", new { id = rSSRepository }, rSSRepository);
        }

        
        private bool RSSRepositoryExists(int id)
        {
            return _context.RssFeedItems.Any(e => e.Id == id);
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

    }
}
