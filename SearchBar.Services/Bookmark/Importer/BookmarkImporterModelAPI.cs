using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bookmark.Importer
{
#pragma warning disable IDE1006 // Naming Styles

    public class BookmarkImporterModelAPI
    {
        public string checksum { get; set; }
        public Roots roots { get; set; }
        public int version { get; set; }
    }

    public class BookmarkBar
    {
        public IList<Child> children { get; set; }
        public string date_added { get; set; }
        public string date_modified { get; set; }
        public string guid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Other
    {
        public IList<object> children { get; set; }
        public string date_added { get; set; }
        public string date_modified { get; set; }
        public string guid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Synced
    {
        public IList<object> children { get; set; }
        public string date_added { get; set; }
        public string date_modified { get; set; }
        public string guid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Roots
    {
        public BookmarkBar bookmark_bar { get; set; }
        public Other other { get; set; }
        public Synced synced { get; set; }
    }

    public class Child
    {
        public string date_added { get; set; }
        public string guid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public IList<Child> children { get; set; }
        public string date_modified { get; set; }
    }

#pragma warning restore IDE1006 // Naming Styles
}
