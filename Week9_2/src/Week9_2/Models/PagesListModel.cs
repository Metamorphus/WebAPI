using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Week9_2.Models
{
    public class PagesListModel
    {
        public List<Page> pages;
        public int CurPage { get; set; }
        public int MaxPage { get; set; }
        public int PageSize { get; set; }
    }
}
