using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.News
{
    public class NewsModelAPI
    {
        public object Error { get; set; }
        public object Query { get; set; }
        public object QueryModified { get; set; }
        public string Category { get; set; }
        public int PerPage { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public bool HasMoreResults { get; set; }
        public IList<FeedItem> FeedItems { get; set; }
    }

    public class Fm
    {
#pragma warning disable IDE1006 // Naming Styles

        public int id { get; set; }
        public int st { get; set; }
        public string it { get; set; }
        public string u { get; set; }
        public int h { get; set; }
        public int w { get; set; }
        public string mu { get; set; }
        public int mh { get; set; }
        public int mw { get; set; }
        public string tu { get; set; }
        public int th { get; set; }
        public int tw { get; set; }
        public string iu { get; set; }
        public int ih { get; set; }
        public int iw { get; set; }
        public DateTime Ud { get; set; }

#pragma warning restore IDE1006 // Naming Styles
    }

    public class F
    {
    }

    public class FeedItem
    {
#pragma warning disable IDE1006 // Naming Styles
      
        public int id { get; set; }
        public int fi { get; set; }
        public string ui { get; set; }
        public string t { get; set; }
        public string B { get; set; }
        public string im { get; set; }
        public string cu { get; set; }
        public DateTime pd { get; set; }
        public DateTime cd { get; set; }
        public int rci { get; set; }
        public string fn { get; set; }
        public string rc { get; set; }
        public Fm fm { get; set; }
        public F f { get; set; }

#pragma warning restore IDE1006 // Naming Styles
    }
}
