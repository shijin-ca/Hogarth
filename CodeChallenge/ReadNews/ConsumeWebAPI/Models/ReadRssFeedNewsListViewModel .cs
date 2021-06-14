using System;
using System.Collections.Generic;

namespace ConsumeWebAPI.Models
{
    public class ReadRssFeedNewsListViewModel
    {
        public ReadRssFeedNewsListViewModel()
        {
            SortBy = 1;
            Desc = true;
        }
        public string RssFeedUrl { get; set; }
        public List<RssFeedListDTO> RssFeedList { get; set; }
        public int SortBy { get; set; }
        public bool Desc { get; set; }

    }
}
