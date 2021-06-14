using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadNews.DBModel
{
    public class RssFeedListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? PublishingDate { get; set; }

    }
}
