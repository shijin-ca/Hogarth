using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadNews.DBModel
{
    public class RssFeedItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? PublishingDate { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }


    }
}
