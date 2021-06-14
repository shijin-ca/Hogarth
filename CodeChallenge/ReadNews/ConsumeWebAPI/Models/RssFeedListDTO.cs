using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumeWebAPI.Models
{
    public class RssFeedListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? PublishingDate { get; set; }

    }
}
