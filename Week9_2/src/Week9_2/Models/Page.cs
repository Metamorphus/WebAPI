using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Week9_2.Models
{
    public class Page
    {
        public int PageId { get; set; }

        public string UrlName { get; set; }
        public string Content { get; set; }

        public string Description { get; set; }
        public string Title { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
